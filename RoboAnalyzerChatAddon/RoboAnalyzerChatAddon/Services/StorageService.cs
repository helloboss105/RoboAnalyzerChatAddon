using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using RoboAnalyzerChatAddon.Models;

namespace RoboAnalyzerChatAddon.Services
{
    public class StorageService
    {
        private const string ChatHistoryFolder = "ChatHistory";
        private const string SharedFilesFolder = "SharedFiles";

        public StorageService()
        {
            if (!Directory.Exists(ChatHistoryFolder))
            {
                Directory.CreateDirectory(ChatHistoryFolder);
            }

            if (!Directory.Exists(SharedFilesFolder))
            {
                Directory.CreateDirectory(SharedFilesFolder);
            }
        }

        public void SaveChatHistory(string roomCode, List<ChatMessage> messages)
        {
            try
            {
                var filePath = Path.Combine(ChatHistoryFolder, $"{roomCode}.json");
                var json = JsonSerializer.Serialize(messages, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving chat history: {ex.Message}");
            }
        }

        public List<ChatMessage> LoadChatHistory(string roomCode)
        {
            try
            {
                var filePath = Path.Combine(ChatHistoryFolder, $"{roomCode}.json");
                if (!File.Exists(filePath))
                    return new List<ChatMessage>();

                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<ChatMessage>>(json) ?? new List<ChatMessage>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading chat history: {ex.Message}");
                return new List<ChatMessage>();
            }
        }

        public void SaveSharedFile(FileShareInfo fileShare)
        {
            try
            {
                var fileName = $"{fileShare.Id}_{fileShare.FileName}";
                var filePath = Path.Combine(SharedFilesFolder, fileName);
                File.WriteAllBytes(filePath, fileShare.FileData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving shared file: {ex.Message}");
            }
        }

        public byte[] LoadSharedFile(string fileId, string fileName)
        {
            try
            {
                var fullFileName = $"{fileId}_{fileName}";
                var filePath = Path.Combine(SharedFilesFolder, fullFileName);
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading shared file: {ex.Message}");
            }
            return Array.Empty<byte>();
        }
    }
}