using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace ChatServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting RoboAnalyzer Chat Server...");
            Console.WriteLine("Server will be available at: http://localhost:5000");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5000");
                });
    }
}