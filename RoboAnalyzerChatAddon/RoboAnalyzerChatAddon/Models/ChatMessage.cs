using System;

namespace RoboAnalyzerChatAddon.Models
{
    public class ChatMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string RoomCode { get; set; } = "";
        public MessageType Type { get; set; } = MessageType.Text;
        public string FileId { get; set; } = "";
        public string FileName { get; set; } = "";
    }

    public enum MessageType
    {
        Text,
        File,
        System
    }
}