using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace SmartCarControl.Steering {
    public class UdpSteeringEngine : ISteeringEngine {
        private Socket _socket;

        public void StartEngine(string host) {
            if (_socket != null) {
                Debug.WriteLine("UdpSTeeringEngine is already running...");
            } else {
                try {
                    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    _socket.Connect(host, 1338);
                } catch (Exception ex) {
                    Debug.WriteLine("Error while connecting UDP Socket: {0}", ex.Message);
                }
            }
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