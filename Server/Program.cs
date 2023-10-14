using System.ComponentModel.DataAnnotations;

namespace MCTGServer {
    internal class Program {
        static void Main(string[] args) {
            var httpserver = new Server();
            httpserver.StartServer();
        }
    }
}