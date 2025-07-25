using System;
using System.Windows.Forms;
using RoboAnalyzerChatAddon.Forms;

namespace RoboAnalyzerChatAddon.Forms
{
    public partial class LobbyForm : Form
    {
        private readonly string _username;

        public LobbyForm(string username)
        {
            InitializeComponent();
            _username = username;
            this.Text = $"RoboAnalyzer Chat - {username}";
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            var roomCode = GenerateRoomCode();
            OpenChatRoom(roomCode, true);
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            var roomCode = txtRoomCode.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(roomCode))
            {
                MessageBox.Show("Please enter a room code.", "Room Code Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (roomCode.Length != 6)
            {
                MessageBox.Show("Room code must be 6 characters.", "Invalid Room Code",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenChatRoom(roomCode, false);
        }

        private string GenerateRoomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var roomCode = new char[6];

            for (int i = 0; i < roomCode.Length; i++)
            {
                roomCode[i] = chars[random.Next(chars.Length)];
            }

            return new string(roomCode);
        }

        private void OpenChatRoom(string roomCode, bool isCreator)
        {
            try
            {
                var chatForm = new ChatRoomForm(_username, roomCode, isCreator);
                chatForm.Show();

                if (isCreator)
                {
                    MessageBox.Show($"Room created! Share this code with others: {roomCode}",
                        "Room Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening chat room: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtRoomCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnJoinRoom_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}