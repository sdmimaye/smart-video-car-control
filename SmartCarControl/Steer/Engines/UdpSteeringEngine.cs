using SmartCarControl.Utils;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SmartCarControl.Steer.Engines {
    public class UdpSteeringEngine : ISteeringEngine {
        private UdpClient _socket;

        public void StartEngine() {
            if (_socket != null) {
                Trace.TraceWarning("UdpSTeeringEngine is already running...");
            } else {
                try {
                    _socket = new UdpClient {
                        Client =
                        {
                            EnableBroadcast = true,
                            ExclusiveAddressUse = false
                        }
                    };
                } catch (Exception ex) {
                    Trace.TraceError("Error while connecting UDP Socket: {0}", ex.Message);
                }
            }
        }

        public void ExecuteStep(SteeringStep step) {
            var ip = new IPEndPoint(IPAddress.Broadcast, 1338);
            var data = step.Serialize();
            var crc = new Crc32();
            var value = crc.ComputeChecksum(data);
            step.Crc = value;

            data = step.Serialize();
            _socket.Send(data, data.Length, ip);
        }

        public void EndEngine() {
            if (_socket == null) {
                Trace.TraceWarning("UdpSteeringEngine is already deinitialized...");
            } else {
                _socket.Close();
                _socket = null;
            }
        }
    }
}