using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SmartCarControl.Steer {
    public class UdpSteeringEngine : ISteeringEngine {
        private UdpClient _socket;

        public void StartEngine() {
            if (_socket != null) {
                Debug.WriteLine("UdpSTeeringEngine is already running...");
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
                    Debug.WriteLine("Error while connecting UDP Socket: {0}", ex.Message);
                }
            }
        }

        public void ExecuteStep(SteeringStep step) {
            var ip = new IPEndPoint(IPAddress.Broadcast, 1338);
            var data = step.Serialize();
            _socket.Send(data, data.Length, ip);
        }

        public void EndEngine() {
            if (_socket == null) {
                Debug.WriteLine("UdpSteeringEngine is already deinitialized...");
            } else {
                _socket.Close();
                _socket = null;
            }
        }
    }
}