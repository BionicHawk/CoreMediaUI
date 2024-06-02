using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoreMediaUI.Source.Api {
    public static class WirelessMouseController {

        private static string WirelessControllerPath = "C:\\dev\\projects\\WirelessPointerDriver\\bin\\Release\\net8.0\\win-x64\\native\\WirelessPointerDriver.exe";
        private static Process? _process;
        public static Label label { get; set; } = null!;
        public static bool IsActive { get; private set; } = false;

        public static bool TryInitializeService() {
            ProcessStartInfo startInfo = new() {
                FileName = WirelessControllerPath,
                Arguments = $"{GetDNS.workingAddress} {GetDNS.wiringMouseControllerPort}",
            };

            _process = new() {
                StartInfo = startInfo
            };

            _process.Start();
            IsActive = true;
            
            return IsActive;
        }

        public static bool ShutdownService() {
            if (_process != null) {
                _process.Kill();
                IsActive = false;
            }

            return IsActive;
        }

    }
}
