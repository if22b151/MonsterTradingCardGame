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
            while(true) {
                var clientSocket = _listener.AcceptTcpClient();
                using var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
                using var reader = new StreamReader(clientSocket.GetStream());

                string? line;

                line = reader.ReadLine();
                if(line != null) {
                    Console.WriteLine(line);
                }
                int content_length = 0;

                while((line = reader.ReadLine()) != null) {
                    Console.WriteLine(line);
                    if(line == "") {
                        break;
                    }
                    var parts = line.Split(':');
                    if(parts.Length == 2 && parts[0] == "Content-Length") { 
                        content_length = int.Parse(parts[1].Trim());
                    }
                }
                if (content_length > 0) {
                    var data = new StringBuilder(200);
                    char[] chars = new char[1024];
                    int bytesTotal = 0;
                    while(bytesTotal < content_length) {
                        var bytesRead = reader.Read(chars, 0, chars.Length);
                        bytesTotal += bytesRead;
                        if(bytesRead == 0) {
                            break;
                        }
                        data.Append(chars, 0, bytesRead);
                    }
                    Console.WriteLine(data.ToString());
                }

                Console.WriteLine("----------------------------------------");

                // ----- 3. Write the HTTP-Response -----
                var writerAlsoToConsole = new StreamTracer(writer);  // we use a simple helper-class StreamTracer to write the HTTP-Response to the client and to the console

                writerAlsoToConsole.WriteLine("HTTP/1.0 200 OK");    // first line in HTTP-Response contains the HTTP-Version and the status code
                writerAlsoToConsole.WriteLine("Content-Type: text/html; charset=utf-8");     // the HTTP-headers (in HTTP after the first line, until the empy line)
                writerAlsoToConsole.WriteLine();
                writerAlsoToConsole.WriteLine("<html><body><h1>Hello World!</h1></body></html>");    // the HTTP-content (here we just return a minimalistic HTML Hello-World)

                Console.WriteLine("========================================");

            }


        }
    }
}
