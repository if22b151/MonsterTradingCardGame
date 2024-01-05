using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MCTGServer {
    internal class ClientProcessor {

        private readonly Server _httpserver;

        private readonly TcpClient _clientSocket;

        public ClientProcessor(TcpClient client, Server httpServer)
        {
            _clientSocket = client;
            _httpserver = httpServer;
        }

        public void ClientProcessing() {
            
            using var writer = new StreamWriter(_clientSocket.GetStream()) { AutoFlush = true };
            using var reader = new StreamReader(_clientSocket.GetStream());

            RequestParser rq = new(reader);
            rq.RequestParsing();

            HTTPResponder responder = new(writer);
            Console.WriteLine($"Path: {rq.Path[1]}");
            var endpoint = _httpserver.Endpoints.ContainsKey(rq.Path[1]) ? _httpserver.Endpoints[rq.Path[1]] : null;
            if (endpoint == null || !endpoint.HandleRequest(rq, responder)) {
                responder.SetResponse(404, "Not Found", "The requested resource could not be found.");
            }

            responder.SendResponse();
        }
    }
}
