using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Models;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> UserConnections = new();
        private static readonly ConcurrentDictionary<string, List<string>> RoomUsers = new();
        private static readonly ConcurrentDictionary<string, List<ChatMessage>> RoomHistory = new();
        private static readonly ConcurrentDictionary<string, List<FileShareInfo>> RoomFiles = new();

        public async Task JoinRoom(string username, string roomCode)
        {
            UserConnections[Context.ConnectionId] = username;

            await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);

            if (!RoomUsers.ContainsKey(roomCode))
            {
                RoomUsers[roomCode] = new List<string>();
            }

            if (!RoomUsers[roomCode].Contains(username))
            {
                RoomUsers[roomCode].Add(username);
            }

            await Clients.Group(roomCode).SendAsync("UpdateUsers", RoomUsers[roomCode]);
            await Clients.OthersInGroup(roomCode).SendAsync("UserJoined", username);

            // Send room history to the joining user
            if (RoomHistory.ContainsKey(roomCode))
            {
                foreach (var message in RoomHistory[roomCode])
                {
                    await Clients.Caller.SendAsync("ReceiveMessage", message);
                }
            }
        }

        public async Task SendMessage(ChatMessage message)
        {
            if (!RoomHistory.ContainsKey(message.RoomCode))
            {
                RoomHistory[message.RoomCode] = new List<ChatMessage>();
            }

            RoomHistory[message.RoomCode].Add(message);
            await Clients.Group(message.RoomCode).SendAsync("ReceiveMessage", message);
        }

        public async Task ShareFile(FileShareInfo fileShare)
        {
            if (!RoomFiles.ContainsKey(fileShare.RoomCode))
            {
                RoomFiles[fileShare.RoomCode] = new List<FileShareInfo>();
            }

            RoomFiles[fileShare.RoomCode].Add(fileShare);
            await Clients.Group(fileShare.RoomCode).SendAsync("FileShared", fileShare);
        }

        public async Task<List<ChatMessage>> GetRoomHistory(string roomCode)
        {
            return RoomHistory.ContainsKey(roomCode) ? RoomHistory[roomCode] : new List<ChatMessage>();
        }

        public async Task<FileShareInfo?> GetFile(string fileId, string roomCode)
        {
            if (RoomFiles.ContainsKey(roomCode))
            {
                return RoomFiles[roomCode].FirstOrDefault(f => f.Id == fileId);
            }
            return null;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (UserConnections.TryRemove(Context.ConnectionId, out var username))
            {
                var roomsToUpdate = RoomUsers.Where(kvp => kvp.Value.Contains(username)).ToList();

                foreach (var room in roomsToUpdate)
                {
                    room.Value.Remove(username);
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.Key);
                    await Clients.Group(room.Key).SendAsync("UpdateUsers", room.Value);
                    await Clients.Group(room.Key).SendAsync("UserLeft", username);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}