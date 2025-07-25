using System;

namespace ChatServer.Models
{
    public class FileShareInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FileName { get; set; } = "";
        public byte[] FileData { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "";
        public long FileSize { get; set; }
        public string RoomCode { get; set; } = "";
        public string SharedBy { get; set; } = "";
        public DateTime SharedAt { get; set; }
    }
}