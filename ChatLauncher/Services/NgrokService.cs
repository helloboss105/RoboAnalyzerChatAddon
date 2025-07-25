using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatLauncher.Services
{
    public class NgrokService
    {
        private Process _ngrokProcess;
        private readonly string _ngrokPath;
        private string _publicUrl;

        public event Action<string> OnTunnelCreated;
        public event Action<string> OnStatusUpdate;

        public NgrokService()
        {
            _ngrokPath = Path.Combine(Path.GetTempPath(), "RoboChat", "ngrok.exe");
        }

        public async Task<bool> InitializeAsync()
        {
            try
            {
                OnStatusUpdate?.Invoke("Preparing ngrok...");

                // Extract ngrok from embedded resources
                await ExtractNgrokAsync();

                return true;
            }
            catch (Exception ex)
            {
                OnStatusUpdate?.Invoke($"Error initializing ngrok: {ex.Message}");
                return false;
            }
        }

        private async Task ExtractNgrokAsync()
        {
            var directory = Path.GetDirectoryName(_ngrokPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (File.Exists(_ngrokPath))
                return; // Already extracted

            // Extract ngrok from embedded resources
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("ChatLauncher.Resources.ngrok.exe");

            if (stream == null)
            {
                // Download ngrok if not embedded
                await DownloadNgrokAsync();
                return;
            }

            using var fileStream = File.Create(_ngrokPath);
            await stream.CopyToAsync(fileStream);
        }

        private async Task DownloadNgrokAsync()
        {
            OnStatusUpdate?.Invoke("Downloading ngrok...");

            using var httpClient = new HttpClient();
            var downloadUrl = "https://bin.equinox.io/c/bNyj1mQVY4c/ngrok-v3-stable-windows-amd64.zip";

            var zipPath = Path.Combine(Path.GetTempPath(), "ngrok.zip");
            var response = await httpClient.GetAsync(downloadUrl);

            using var fileStream = File.Create(zipPath);
            await response.Content.CopyToAsync(fileStream);

            // Extract zip
            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, Path.GetDirectoryName(_ngrokPath));
            File.Delete(zipPath);
        }

        public async Task<string> StartTunnelAsync(int port)
        {
            try
            {
                OnStatusUpdate?.Invoke("Starting ngrok tunnel...");

                var startInfo = new ProcessStartInfo
                {
                    FileName = _ngrokPath,
                    Arguments = $"http {port} --log=stdout",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                _ngrokProcess = Process.Start(startInfo);

                // Wait for ngrok to start and get the public URL
                await Task.Delay(3000); // Give ngrok time to start

                _publicUrl = await GetPublicUrlAsync();

                if (!string.IsNullOrEmpty(_publicUrl))
                {
                    OnTunnelCreated?.Invoke(_publicUrl);
                    OnStatusUpdate?.Invoke($"Tunnel created: {_publicUrl}");
                    return _publicUrl;
                }

                throw new Exception("Failed to get public URL from ngrok");
            }
            catch (Exception ex)
            {
                OnStatusUpdate?.Invoke($"Error starting tunnel: {ex.Message}");
                throw;
            }
        }

        private async Task<string> GetPublicUrlAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync("http://localhost:4040/api/tunnels");

                var tunnelsData = JsonSerializer.Deserialize<JsonElement>(response);
                var tunnels = tunnelsData.GetProperty("tunnels");

                if (tunnels.GetArrayLength() > 0)
                {
                    var tunnel = tunnels[0];
                    return tunnel.GetProperty("public_url").GetString();
                }
            }
            catch (Exception ex)
            {
                OnStatusUpdate?.Invoke($"Error getting public URL: {ex.Message}");
            }

            return null;
        }

        public void Stop()
        {
            try
            {
                _ngrokProcess?.Kill();
                _ngrokProcess?.Dispose();
                OnStatusUpdate?.Invoke("ngrok stopped");
            }
            catch (Exception ex)
            {
                OnStatusUpdate?.Invoke($"Error stopping ngrok: {ex.Message}");
            }
        }

        public string GetPublicUrl() => _publicUrl;
    }
}