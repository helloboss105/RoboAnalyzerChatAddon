using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ChatLauncher.Services
{
    public class ClientService
    {
        public void StartClient(string serverUrl)
        {
            try
            {
                // Extract and run the chat client
                var clientPath = ExtractClient();

                // Create config file with server URL
                CreateClientConfig(serverUrl, Path.GetDirectoryName(clientPath));

                // Start the client
                Process.Start(new ProcessStartInfo
                {
                    FileName = clientPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error starting client: {ex.Message}",
                    "Error", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private string ExtractClient()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "RoboChat", "Client");
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            var clientPath = Path.Combine(tempDir, "RoboAnalyzerChatClient.exe");

            if (File.Exists(clientPath))
                return clientPath;

            // Extract client from embedded resources
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();

            foreach (var resourceName in resourceNames)
            {
                if (resourceName.Contains("client-files"))
                {
                    var fileName = resourceName.Substring(resourceName.LastIndexOf('.') + 1);
                    var filePath = Path.Combine(tempDir, fileName);

                    using var stream = assembly.GetManifestResourceStream(resourceName);
                    using var fileStream = File.Create(filePath);
                    stream.CopyTo(fileStream);
                }
            }

            return clientPath;
        }

        private void CreateClientConfig(string serverUrl, string clientDir)
        {
            var configPath = Path.Combine(clientDir, "appsettings.json");
            var config = $@"{{
  ""ChatServer"": {{
    ""Url"": ""{serverUrl}""
  }}
}}";
            File.WriteAllText(configPath, config);
        }
    }
}