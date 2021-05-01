using LumosLIB.Kernel.Log;
using MyKlc.Plugin.Infrastructure.Messages;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MyKlc.Plugin.Infrastructure.Sockets
{
    public class KlcUdpSocket
    {
        private readonly ILumosLog _logger;
        private readonly Socket _socket;
        private readonly KlcSocketState _state;
        private EndPoint _endPointFrom;
        private AsyncCallback _receive;
   
        public event MessageSentEventHandler MessageSent;
        public event MessageReceivedEventHandler MessageReceived;
        
        public KlcUdpSocket()
        {
            _logger = LumosLogger.getInstance(typeof(MyKlcPlugin));
            _socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp
            );
            _state = new KlcSocketState();
            _endPointFrom = new IPEndPoint(IPAddress.Any, 0);
            _receive = null;
        }

        public void CreateServer()
        {
            ConnectServer(new IPEndPoint(IPAddress.Any, GetAvailablePort()));
        }

        public void CreateServer(string address)
        {
            ConnectServer(new IPEndPoint(IPAddress.Parse(address), GetAvailablePort()));
        }

        public void CreateServer(int port)
        {
            ConnectServer(new IPEndPoint(IPAddress.Any, GetAvailablePort(port)));
        }

        public void CreateServer(string address, int port)
        {
            ConnectServer(new IPEndPoint(IPAddress.Parse(address), GetAvailablePort(port)));
        }

        public void SendToServer(KlcMessage message)
        {
            var messageStream = KlcMessage.ToStream(message);
            _socket.BeginSend(
               messageStream,
               0,
               messageStream.Length,
               SocketFlags.None,
               (IAsyncResult asyncResult) => _socket.EndSend(asyncResult),
               _state);
        }

        public void CreateClient(int port)
        {
            ConnectClient(IPAddress.Parse(GetLocalIPAddress()), port);
        }

        public void CreateClient(string address, int port)
        {
            ConnectClient(IPAddress.Parse(address), port);
        }

        public void SendToClient(KlcMessage message)
        {
            var messageStream = KlcMessage.ToStream(message);
            _socket.BeginSendTo(
               messageStream,
               0,
               messageStream.Length,
               SocketFlags.None,
               _endPointFrom,
               (IAsyncResult asyncResult) => MessageSent?.Invoke(_socket.EndSend(asyncResult) > 0),
               _state);
        }

        private void ConnectServer(IPEndPoint endpoint)
        {
            _socket.SetSocketOption(
               SocketOptionLevel.IP,
               SocketOptionName.ReuseAddress,
               true
           );
            _socket.Bind(endpoint);
            _logger.Info($"Started server on {endpoint.Address}:{endpoint.Port}");
            Receive();
        }

        private void ConnectClient(IPAddress ipAddress, int port)
        {
            _socket.Connect(ipAddress, port);
            Receive();
        }

        private void Receive()
        {
            _socket.BeginReceiveFrom(
                _state.Buffer,
                0,
                KlcConstants.SOCKET_BUFFER_SIZE,
                SocketFlags.None,
                ref _endPointFrom,
                _receive = (IAsyncResult asyncResult) =>
                {
                    var socketOutput = asyncResult.AsyncState as KlcSocketState;
                    var bytes = _socket.EndReceiveFrom(asyncResult, ref _endPointFrom);
                    _socket.BeginReceiveFrom(
                        socketOutput.Buffer,
                        0,
                        KlcConstants.SOCKET_BUFFER_SIZE,
                        SocketFlags.None,
                        ref _endPointFrom,
                        _receive,
                        socketOutput
                    );

                    MessageReceived?.Invoke(KlcMessage.FromStream(socketOutput.Buffer));
                },
                _state);
        }

        private string GetLocalIPAddress()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList
                                                      .First(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                                                      .ToString();
        }

        private int GetAvailablePort(int startingPort = KlcConstants.DEFAULT_SOCKET_SERVER_PORT)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var connectionsEndpoints = ipGlobalProperties.GetActiveTcpConnections()
                                                         .Select(connectionInformation => connectionInformation.LocalEndPoint);
            var tcpListenersEndpoints = ipGlobalProperties.GetActiveTcpListeners();
            var udpListenersEndpoints = ipGlobalProperties.GetActiveUdpListeners();

            var portsInUse = connectionsEndpoints.Concat(tcpListenersEndpoints)
                                                 .Concat(udpListenersEndpoints)
                                                 .Select(endpoint => endpoint.Port);

            return Enumerable.Range(startingPort, ushort.MaxValue - startingPort + 1)
                             .Except(portsInUse)
                             .FirstOrDefault();
        }
    }
}
