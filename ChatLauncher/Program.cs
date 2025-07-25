using ChatLauncher.Forms;
using System;
using System.Windows.Forms;


namespace ChatLauncher
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if already running
            var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);

            if (processes.Length > 1)
            {
                MessageBox.Show("RoboAnalyzer Chat is already running!", "Already Running",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Application.Run(new LauncherForm());
        }
    }
}