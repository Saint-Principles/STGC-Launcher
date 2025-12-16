using System;
using System.Windows.Forms;

namespace STGCLauncher
{
    internal static class Program
    {
        [STAThread]
        static async void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Window());
        }
    }
}
