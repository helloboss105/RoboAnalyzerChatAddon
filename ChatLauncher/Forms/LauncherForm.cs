using ChatLauncher.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatLauncher.Forms
{
    public partial class LauncherForm : Form
    {
        private NgrokService _ngrokService;
        private ServerService _serverService;
        private ClientService _clientService;

        private Label lblStatus;
        private TextBox txtPublicUrl;
        private Button btnStart;
        private Button btnOpenClient;
        private Button btnCopyUrl;
        private Button btnStop;
        private TextBox txtLogs;

        public LauncherForm()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeComponent()
        {
            this.Text = "RoboAnalyzer Chat - One Click Setup";
            this.Size = new System.Drawing.Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Status label
            lblStatus = new Label
            {
                Text = "Ready to start...",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(560, 30),
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold)
            };

            // Public URL textbox
            txtPublicUrl = new TextBox
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(450, 25),
                ReadOnly = true,
                PlaceholderText = "Public URL will appear here..."
            };

            // Start button
            btnStart = new Button
            {
                Text = "🚀 Start Chat Server",
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(150, 40),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold)
            };
            btnStart.Click += BtnStart_Click;

            // Open client button
            btnOpenClient = new Button
            {
                Text = "💬 Open Chat",
                Location = new System.Drawing.Point(180, 100),
                Size = new System.Drawing.Size(120, 40),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White,
                Enabled = false
            };
            btnOpenClient.Click += BtnOpenClient_Click;

            // Copy URL button
            btnCopyUrl = new Button
            {
                Text = "📋 Copy URL",
                Location = new System.Drawing.Point(480, 60),
                Size = new System.Drawing.Size(100, 25),
                Enabled = false
            };
            btnCopyUrl.Click += BtnCopyUrl_Click;

            // Stop button
            btnStop = new Button
            {
                Text = "⏹️ Stop",
                Location = new System.Drawing.Point(310, 100),
                Size = new System.Drawing.Size(100, 40),
                BackColor = System.Drawing.Color.Red,
                ForeColor = System.Drawing.Color.White,
                Enabled = false
            };
            btnStop.Click += BtnStop_Click;

            // Logs textbox
            txtLogs = new TextBox
            {
                Location = new System.Drawing.Point(20, 160),
                Size = new System.Drawing.Size(560, 280),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new System.Drawing.Font("Consolas", 9F)
            };

            Controls.AddRange(new Control[] {
                lblStatus, txtPublicUrl, btnStart, btnOpenClient,
                btnCopyUrl, btnStop, txtLogs
            });
        }

        private void InitializeServices()
        {
            _ngrokService = new NgrokService();
            _serverService = new ServerService();
            _clientService = new ClientService();

            // Subscribe to events
            _ngrokService.OnStatusUpdate += UpdateStatus;
            _ngrokService.OnTunnelCreated += OnTunnelCreated;
            _serverService.OnStatusUpdate += UpdateStatus;
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;
                UpdateStatus("🔄 Starting setup...");

                // Initialize ngrok
                var ngrokReady = await _ngrokService.InitializeAsync();
                if (!ngrokReady)
                {
                    MessageBox.Show("Failed to initialize ngrok", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Start chat server
                var serverStarted = await _serverService.StartAsync(5000);
                if (!serverStarted)
                {
                    MessageBox.Show("Failed to start chat server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Start ngrok tunnel
                var publicUrl = await _ngrokService.StartTunnelAsync(5000);
                if (string.IsNullOrEmpty(publicUrl))
                {
                    MessageBox.Show("Failed to create ngrok tunnel", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UpdateStatus("✅ Chat server is ready!");
                btnStop.Enabled = true;
                btnOpenClient.Enabled = true;
                btnCopyUrl.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting services: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
            }
        }

        private void BtnOpenClient_Click(object sender, EventArgs e)
        {
            var publicUrl = _ngrokService.GetPublicUrl();
            if (!string.IsNullOrEmpty(publicUrl))
            {
                _clientService.StartClient(publicUrl + "/chathub");
            }
        }

        private void BtnCopyUrl_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPublicUrl.Text))
            {
                Clipboard.SetText(txtPublicUrl.Text);
                MessageBox.Show("URL copied to clipboard!\nShare this with your friends.", "URL Copied",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void BtnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            UpdateStatus("🔄 Stopping services...");

            _ngrokService.Stop();
            await _serverService.StopAsync();

            btnStart.Enabled = true;
            btnOpenClient.Enabled = false;
            btnCopyUrl.Enabled = false;
            txtPublicUrl.Clear();

            UpdateStatus("⏹️ Services stopped");
        }

        private void OnTunnelCreated(string publicUrl)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnTunnelCreated), publicUrl);
                return;
            }

            txtPublicUrl.Text = publicUrl;
        }

        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), message);
                return;
            }

            lblStatus.Text = message;
            txtLogs.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
            txtLogs.ScrollToCaret();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            BtnStop_Click(null, null);
            base.OnFormClosing(e);
        }
    }
}