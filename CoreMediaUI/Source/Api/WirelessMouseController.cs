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

        private static TcpListener? _listener;
        public static Label label { get; set; } = null!;
        private static Thread? _whileLoopWaitingConnection;

        public static bool TryInitializeService() {
            var address = GetDNS.workingAddress;
            var port = GetDNS.wiringMouseControllerPort;

            if (address == null) return false;

            _listener = new(address, port);
            
            _whileLoopWaitingConnection = new Thread(() => { 
                _listener.Start();
                while(true) {
                    var client = _listener.AcceptTcpClient();

                    while (client.Connected) {
                        var ns = client.GetStream();

                        byte[] buffer = new byte[1024];
                        ns.Read(buffer, 0, buffer.Length);

                        Console.WriteLine(Encoding.UTF8.GetString(buffer));
                    }
                }
            });

            _whileLoopWaitingConnection.Start();
            return true;
        }

        public static bool ShutdownService() {
            if (_listener != null && _whileLoopWaitingConnection != null) {
                _whileLoopWaitingConnection.Interrupt();
                return true;
            }
            return false;
        }

    }
}
