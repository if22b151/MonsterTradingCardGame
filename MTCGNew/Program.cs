using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCTGServer;
using MTCGNew.Endpoints;

namespace MTCGNew {
    internal class Program {
        static void Main(string[] args) {
            Server server = new Server();
            server.AddEndpoint("users", new UsersEndpoint());
            server.AddEndpoint("sessions", new SessionsEndpoint());
            server.AddEndpoint("packages", new PackagesEndpoint());
            server.AddEndpoint("transactions", new PackagesEndpoint());
            server.AddEndpoint("cards", new Stackendpoint());
            server.AddEndpoint("deck", new Deckendpoint());
            server.AddEndpoint("stats", new Battleendpoint());
            server.AddEndpoint("scoreboard", new Battleendpoint());
            server.AddEndpoint("battles", new Battleendpoint());
            server.StartServer();
        }
    }

}

