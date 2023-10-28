using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MCTGServer {
    internal class Server {
        private readonly TcpListener _listener;

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
                ClientProcessor clientprocessor = new ClientProcessor();
                clientprocessor.ClientProcessing(clientSocket);
            }

        }

       

        
    }
}
