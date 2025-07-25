using System;
using System.IO;
using System.Windows.Forms;

namespace RoboAnalyzerChatAddon.Forms
{
    public partial class UsernameForm : Form
    {
        private const string UsernameFile = "username.txt";
        //public string Username { get; private set; }
        public string Username { get; private set; } = "";

        public UsernameForm()
        {
            InitializeComponent();
            LoadSavedUsername();
        }

        private void LoadSavedUsername()
        {
            if (File.Exists(UsernameFile))
            {
                var savedUsername = File.ReadAllText(UsernameFile).Trim();
                if (!string.IsNullOrEmpty(savedUsername))
                {
                    txtUsername.Text = savedUsername;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username.", "Username Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (username.Length > 20)
            {
                MessageBox.Show("Username must be 20 characters or less.", "Username Too Long",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Username = username;
            File.WriteAllText(UsernameFile, username);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}