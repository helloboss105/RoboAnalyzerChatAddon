using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatLauncher.Services
{
    public class ServerService
    {
        private WebApplication _app;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action<string> OnStatusUpdate;

        public async Task<bool> StartAsync(int port = 5000)
        {
            try
            {
                OnStatusUpdate?.Invoke("Starting chat server...");

                var builder = WebApplication.CreateBuilder();

                // Configure services
                builder.Services.AddSignalR();
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
                });

                _app = builder.Build();

                // Configure pipeline
                _app.UseCors();
                _app.UseRouting();
                _app.MapHub<ChatHub>("/chathub");

                // Set the server URL
                _app.Urls.Clear();
                _app.Urls.Add($"http://localhost:{port}");

                // Initialize cancellation token source before starting the server
                _cancellationTokenSource = new CancellationTokenSource();

                // Start server
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _app.RunAsync(_cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        // Expected when cancelling
                    }
                });

                // Wait a moment for server to start
                await Task.Delay(2000);

                OnStatusUpdate?.Invoke($"Chat server running on port {port}");
                return true;
            }
            catch (Exception ex)
            {
                OnStatusUpdate?.Invoke($"Error starting server: {ex.Message}");
                return false;
            }
        }

        public async Task StopAsync()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                //await _app?.DisposeAsync();
                OnStatusUpdate?.Invoke("Chat server stopped");
            }
            catch (Exception ex)
            {
                OnStatusUpdate?.Invoke($"Error stopping server: {ex.Message}");
            }
        }
    }
}