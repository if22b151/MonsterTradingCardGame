using MCTGServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGNew.Repositories;
using System.Text.Json;
using MTCGNew.Models;

namespace MTCGNew.Endpoints {
    internal class SessionsEndpoint : IHTTPEndpoint {

        public string? SessionToken { get; private set; }

        public bool HandleRequest(RequestParser request, HTTPResponder response) {
            if (request.Method == HttpMethods.POST) {
                Login(request, response);
                return true;
            }
            return false;
        }

        private void Login(RequestParser request, HTTPResponder response) {
            SessionRepository sessionRepository = new SessionRepository();
            var user = JsonSerializer.Deserialize<Credentials>(request.Content ?? "");
            if (user is null) {
                response.ReturnCode = 400;
                response.ReturnText = "Bad Request";
                response.Content = "User was not sent with request!";
                return;
            }
            SessionToken = sessionRepository.Login(user);
            if (SessionToken is null) {
                response.ReturnCode = 401;
                response.ReturnText = "Unauthorized";
                response.Content = "Invalid username/password provided";
                return;
            }
            SessionHandling.Sessions.TryAdd(user.Username, SessionToken);
            response.ReturnCode = 200;
            response.ReturnText = "OK";
            response.Content = "User login successful";
        }
    }
}
