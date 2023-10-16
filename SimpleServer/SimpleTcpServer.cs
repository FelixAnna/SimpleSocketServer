using System.Net;
using System.Net.Sockets;

namespace SimpleServer
{
    public class SimpleTcpServer
    {
        private Socket _serverSocket;
        private readonly IPAddress _ipAddress;
        private readonly int _port;

        private bool _isActive = false;

        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;
        private CancellationTokenSource _listenerTokenSource;
        private CancellationToken _listenerToken;


        public Action<string> Logger;

        public ClientRequestHandler clientRequestHandler { get; set; }

        public SimpleTcpServer(string ipAddress, int port)
        {
            this._port = port;
            this._ipAddress = IPAddress.Parse(ipAddress);
        }

        public void Start()
        {
            if (_isActive)
            {
                return;
            }

            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(_ipAddress, _port);
            _serverSocket.Bind(endPoint);
            _serverSocket.Listen(128);
            _isActive = true;

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _listenerTokenSource = new CancellationTokenSource();
            _listenerToken = _listenerTokenSource.Token;

            Task.Run(AcceptConnections, _listenerToken);
        }

        public void Stop()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;
            _tokenSource.Cancel();
            _listenerTokenSource.Cancel();
            _serverSocket.Close();
            _serverSocket?.Dispose();

            Logger?.Invoke($"{nameof(SimpleTcpServer)}: stopping");
        }

        private async Task AcceptConnections()
        {
            while (!_listenerToken.IsCancellationRequested)
            {
                try
                {
                    /**
                     * 1. get socket;
                     * 2. receive date and deserialize to RequestData
                     * 3. switch type of the Request, use different Handler to handler the request
                     * 4. done
                     * */
                    var clientSocket = await _serverSocket!.AcceptAsync(_token).ConfigureAwait(false);

                    await clientRequestHandler.HandleAsync(clientSocket, _token).ConfigureAwait(false);

                }
                catch (Exception ex)
                {
                    if (ex is TaskCanceledException
                        || ex is OperationCanceledException
                        || ex is ObjectDisposedException
                        || ex is InvalidOperationException)
                    {
                        _serverSocket?.Dispose();
                        Logger?.Invoke($"{nameof(SimpleTcpServer)}: stopped listening");
                        break;
                    }
                    else
                    {
                        _serverSocket?.Dispose();
                        Logger?.Invoke($"{nameof(SimpleTcpServer)}: exception while awaiting connections: {ex}");
                        continue;
                    }
                }
            }

            clientRequestHandler.Stop(_token);
        }

    }
}
