using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MCTGServer {
    public class Server {
        private readonly TcpListener _listener;

        public Dictionary<string, IHTTPEndpoint> Endpoints { get; set; } = new();

        public Server()
        {
            _listener = new TcpListener(IPAddress.Loopback, 10001);
        }

        public void StartServer() 
        {
            _listener.Start();
            Console.WriteLine("Server started!!");
            StartServerLoop();
        }

        private void StartServerLoop() {
            while (true) {
                 var clientSocket = _listener.AcceptTcpClient();
                ClientProcessor clientprocessor = new ClientProcessor(clientSocket, this);
                ThreadPool.QueueUserWorkItem(c => clientprocessor.ClientProcessing());
            }

        }

        public void AddEndpoint(string path, IHTTPEndpoint endpoint) {
            Endpoints.Add(path, endpoint);
        }
        
    }
}
