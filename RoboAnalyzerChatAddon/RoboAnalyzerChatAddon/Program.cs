using System;
using System.Windows.Forms;
using RoboAnalyzerChatAddon.Forms;

namespace RoboAnalyzerChatAddon
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start with username form
            var usernameForm = new UsernameForm();
            if (usernameForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new LobbyForm(usernameForm.Username));
            }
        }
    }
}