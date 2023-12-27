using MCTGServer;
using MTCGNew.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGNew.Endpoints {
    internal class Stackendpoint : IHTTPEndpoint {
        public bool HandleRequest(RequestParser request, HTTPResponder response) {
            if(request.Method == HttpMethods.GET) {
                GetCards(request, response);
                return true;
            }
            return false;
        }

        private void GetCards(RequestParser request, HTTPResponder responder) {
            if(!request.Headers.ContainsKey("Authorization")) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Access token is missing or invalid";
                return;
            }

            string inputString = request.Headers["Authorization"];
            string username = SessionHandling.GetUsername(inputString);
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if (SessionHandling.CheckSession("admin") == false) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Not logged in!";
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if(rqauthtoken == usersessiontoken) {
                Stackrepository stackrepository = new Stackrepository();
                try {
                    StackCards? stack = stackrepository.GetCards(username);
                    if(stack == null) {
                        responder.ReturnCode = 204;
                        responder.ReturnText = "No Content";
                        responder.Content = "The request was fine, but the user doesn't have any cards";
                        return;
                    }
                    responder.ReturnCode = 200;
                    responder.ReturnText = "OK";
                    responder.Content = JsonSerializer.Serialize(stack);
                    responder.Headers.Add("Content-Type", "application/json");
                    return;
                }
                catch (Exception e) {
                    responder.ReturnCode = 400;
                    responder.ReturnText = "Bad Request";
                    responder.Content = e.Message;
                    return;
                }

            }
        }
    }
}
