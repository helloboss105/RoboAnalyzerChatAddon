using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using RoboAnalyzerChatAddon.Models;

namespace RoboAnalyzerChatAddon.Services
{
    public class ChatHubClient : IDisposable
    {
        private HubConnection? _connection;
        private readonly string _serverUrl;

        public event Action<ChatMessage>? MessageReceived;
        public event Action<List<string>>? UsersUpdated;
        public event Action<string>? UserJoined;
        public event Action<string>? UserLeft;
        public event Action<FileShareInfo>? FileShared;

        public ChatHubClient(string serverUrl = "http://localhost:5000/chathub")
        {
            _serverUrl = serverUrl;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _connection = new HubConnectionBuilder()
                    .WithUrl(_serverUrl)
                    .Build();

                SetupEventHandlers();

                await _connection.StartAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                return false;
            }
        }

        private void SetupEventHandlers()
        {
            if (_connection == null) return;

            _connection.On<ChatMessage>("ReceiveMessage", (message) =>
            {
                MessageReceived?.Invoke(message);
            });

            _connection.On<List<string>>("UpdateUsers", (users) =>
            {
                UsersUpdated?.Invoke(users);
            });

            _connection.On<string>("UserJoined", (username) =>
            {
                UserJoined?.Invoke(username);
            });

            _connection.On<string>("UserLeft", (username) =>
            {
                UserLeft?.Invoke(username);
            });

            _connection.On<FileShareInfo>("FileShared", (fileShare) =>
            {
                FileShared?.Invoke(fileShare);
            });
        }

        public async Task JoinRoomAsync(string username, string roomCode)
        {
            if (_connection != null)
                await _connection.InvokeAsync("JoinRoom", username, roomCode);
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            if (_connection != null)
                await _connection.InvokeAsync("SendMessage", message);
        }

        public async Task ShareFileAsync(FileShareInfo fileShare)
        {
            if (_connection != null)
                await _connection.InvokeAsync("ShareFile", fileShare);
        }

        public async Task<List<ChatMessage>> GetRoomHistoryAsync(string roomCode)
        {
            if (_connection != null)
                return await _connection.InvokeAsync<List<ChatMessage>>("GetRoomHistory", roomCode);
            return new List<ChatMessage>();
        }

        public async Task DisconnectAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
                _connection = null;
            }
        }

        public void Dispose()
        {
            DisconnectAsync().Wait();
        }
    }
}