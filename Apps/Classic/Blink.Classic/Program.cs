using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blink.Shared.Engine;
using Wintellect.Sterling.Core;
using Wintellect.Sterling.Server;
using Wintellect.Sterling.Server.FileSystem;

namespace Blink.Classic
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Sterling.Activate(() => new PlatformAdapter(), () => new FileSystemDriver());

            Application.Run(new BlinkForm());

            Sterling.Deactivate();
        }
    }
}
