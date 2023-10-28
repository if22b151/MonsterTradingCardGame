using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MCTGServer {
    internal class ClientProcessor {

        public void ClientProcessing(TcpClient clientSocket) {
            using var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
            using var reader = new StreamReader(clientSocket.GetStream());



            RequestParser parser = new(reader);
            parser.RequestParsing();

            //TODO: Requesthandling

            HTTPResponder responder = new(writer);
            responder.SendResponse();
        }
    }
}
