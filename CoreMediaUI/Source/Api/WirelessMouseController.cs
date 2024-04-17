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
        private static bool run = true;

        public static bool TryInitializeService() {
            var address = GetDNS.workingAddress;
            var port = GetDNS.wiringMouseControllerPort;

            if (address == null) return false;

            _listener = new(address, port);
            
            _whileLoopWaitingConnection = new Thread(() => {
                run = true;
                _listener.Start();
                while(run) {
                    var client = _listener.AcceptTcpClient();

                    while (client.Connected) {
                        var ns = client.GetStream();

                        byte[] buffer = new byte[1024];
                        ns.Read(buffer, 0, buffer.Length);

                        label.Text = buffer.ToString();
                        Debug.WriteLine(buffer.ToString());
                    }
                }
            });

            _whileLoopWaitingConnection.Start();
            return true;
        }

        public static bool ShutdownService() {
            if (_listener != null && _whileLoopWaitingConnection != null) {
                run = false;
                _whileLoopWaitingConnection.Interrupt();
                return true;
            }
            return false;
        }

    }
}
