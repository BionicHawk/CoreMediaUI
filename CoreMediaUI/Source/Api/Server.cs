using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CoreMediaUI.Source.Api {
    public static class Server {

        // Working Directories
        public static string API_PATH { get; private set; } = "C:\\dev\\projects\\media_controller";
        public static string API_ENV { get; private set; } = ".venv\\Scripts\\activate";
        public static string API_APP { get; private set; } = "app.py";

        // Server components
        private static Process? server;
        private static string cmd = "powershell.exe";
        public static string ApiUri { get; private set; } = null!;
        private static HttpClient? _client;

        private static void StartupShell() {
            ProcessStartInfo serverInfo = new() {
                FileName = cmd,
                WorkingDirectory = API_PATH,
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            server = Process.Start(serverInfo);
        }

        public static bool TryInitializeServer(string address, ushort port) {
            StartupShell();
            if (server != null) {
                server.StandardInput.WriteLine(API_ENV);
                server.StandardInput.WriteLine($"python {API_APP} {address} {port}");
                ApiUri = $"http://{address}:{port}";
                _client = new();
                return true;
            }
            return false;
        }

        public static bool TryCloseServer() {
            if (server != null) {

                var processes = Process.GetProcessesByName("python");

                foreach (var process in processes) {
                    process.Kill();
                }

                server.StandardInput.WriteLine("deactivate");
                
                server.Kill();
                return true;
            }
            return false;
        }

    }
}
