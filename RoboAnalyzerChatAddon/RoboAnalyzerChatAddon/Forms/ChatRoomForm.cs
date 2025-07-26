using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoboAnalyzerChatAddon.Models;
using RoboAnalyzerChatAddon.Services;

namespace RoboAnalyzerChatAddon.Forms
{
    public partial class ChatRoomForm : Form
    {
        private readonly string _username;
        private readonly string _roomCode;
        private readonly ChatHubClient _hubClient;
        private readonly StorageService _storageService;
        private readonly List<ChatMessage> _messages;

        public ChatRoomForm(string username, string roomCode, bool isCreator)
        {
            InitializeComponent();
            _username = username;
            _roomCode = roomCode;
            _hubClient = new ChatHubClient();
            _storageService = new StorageService();
            _messages = new List<ChatMessage>();

            this.Text = $"Chat Room: {roomCode} - {username}";
            lblRoomCode.Text = $"Room Code: {roomCode}";

            SetupEventHandlers();
            ConnectToRoom();
        }

        private void SetupEventHandlers()
        {
            _hubClient.MessageReceived += OnMessageReceived;
            _hubClient.UsersUpdated += OnUsersUpdated;
            _hubClient.UserJoined += OnUserJoined;
            _hubClient.UserLeft += OnUserLeft;
            _hubClient.FileShared += OnFileShared;

            this.FormClosing += ChatRoomForm_FormClosing;
        }

        private async void ConnectToRoom()
        {
            try
            {
                var connected = await _hubClient.ConnectAsync();
                if (!connected)
                {
                    MessageBox.Show("Failed to connect to chat server.", "Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                await _hubClient.JoinRoomAsync(_username, _roomCode);

                // Load chat history
                var history = _storageService.LoadChatHistory(_roomCode);
                foreach (var message in history)
                {
                    AddMessageToChat(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error joining room: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void OnMessageReceived(ChatMessage message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ChatMessage>(OnMessageReceived), message);
                return;
            }

            _messages.Add(message);
            AddMessageToChat(message);
            _storageService.SaveChatHistory(_roomCode, _messages);
        }

        private void OnUsersUpdated(List<string> users)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<List<string>>(OnUsersUpdated), users);
                return;
            }

            lstUsers.Items.Clear();
            foreach (var user in users)
            {
                lstUsers.Items.Add(user);
            }
        }

        private void OnUserJoined(string username)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnUserJoined), username);
                return;
            }

            var systemMessage = new ChatMessage
            {
                Username = "System",
                Message = $"{username} joined the room",
                Timestamp = DateTime.Now,
                Type = MessageType.System,
                RoomCode = _roomCode
            };

            AddMessageToChat(systemMessage);
        }

        private void OnUserLeft(string username)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnUserLeft), username);
                return;
            }

            var systemMessage = new ChatMessage
            {
                Username = "System",
                Message = $"{username} left the room",
                Timestamp = DateTime.Now,
                Type = MessageType.System,
                RoomCode = _roomCode
            };

            AddMessageToChat(systemMessage);
        }

        private void OnFileShared(FileShareInfo fileShare)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<FileShareInfo>(OnFileShared), fileShare);
                return;
            }

            var fileMessage = new ChatMessage
            {
                Username = fileShare.SharedBy,
                Message = $"Shared file: {fileShare.FileName}",
                Timestamp = fileShare.SharedAt,
                Type = MessageType.File,
                FileId = fileShare.Id,
                FileName = fileShare.FileName,
                RoomCode = _roomCode
            };

            _messages.Add(fileMessage);
            AddMessageToChat(fileMessage);
            _storageService.SaveChatHistory(_roomCode, _messages);
        }

        private void AddMessageToChat(ChatMessage message)
        {
            var timestamp = message.Timestamp.ToString("HH:mm");
            var messageText = "";

            switch (message.Type)
            {
                case MessageType.Text:
                    messageText = $"[{timestamp}] {message.Username}: {message.Message}";
                    break;
                case MessageType.File:
                    messageText = $"[{timestamp}] {message.Username} shared: {message.FileName} (Click to download)";
                    break;
                case MessageType.System:
                    messageText = $"[{timestamp}] {message.Message}";
                    break;
            }

            rtbChat.AppendText(messageText + Environment.NewLine);
            rtbChat.ScrollToCaret();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await SendMessage();
        }

        private async void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                await SendMessage();
                e.Handled = true;
            }
        }

        private async Task SendMessage()
        {
            var messageText = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(messageText))
                return;

            var message = new ChatMessage
            {
                Username = _username,
                Message = messageText,
                Timestamp = DateTime.Now,
                RoomCode = _roomCode,
                Type = MessageType.Text
            };

            try
            {
                await _hubClient.SendMessageAsync(message);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnShareFile_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Supported Files|*.jpg;*.jpeg;*.png;*.pdf";
                openFileDialog.Title = "Select a file to share";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var fileInfo = new FileInfo(openFileDialog.FileName);
                        if (fileInfo.Length > 10 * 1024 * 1024) // 10MB limit
                        {
                            MessageBox.Show("File size must be less than 10MB.", "File Too Large",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var fileData = await File.ReadAllBytesAsync(openFileDialog.FileName);
                        var fileShare = new FileShareInfo
                        {
                            FileName = Path.GetFileName(openFileDialog.FileName),
                            FileData = fileData,
                            ContentType = GetContentType(openFileDialog.FileName),
                            FileSize = fileData.Length,
                            RoomCode = _roomCode,
                            SharedBy = _username,
                            SharedAt = DateTime.Now
                        };

                        await _hubClient.ShareFileAsync(fileShare);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error sharing file: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream";
            }
        }

        private void rtbChat_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            // Handle file download when clicking on file links
            // This would need additional implementation to track file messages
        }

        private async void ChatRoomForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                await _hubClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disconnecting: {ex.Message}");
            }
        }
    }
}