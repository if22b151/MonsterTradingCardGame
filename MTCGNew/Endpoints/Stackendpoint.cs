using MCTGServer;
using MTCGNew.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MTCGNew.Models;
using MTCGNew.Cards;
using System.Text.Json.Serialization;

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
                responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
                return;
            }

            string inputString = request.Headers["Authorization"];
            string username = SessionHandling.GetUsername(inputString);
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if (SessionHandling.CheckSession("admin") == false) {
                responder.SetResponse(401, "Unauthorized", "Not logged in!");
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if(rqauthtoken == usersessiontoken) {
                Stackrepository stackrepository = new Stackrepository();
                try {
                    Stackcards stack = stackrepository.GetCards(username);
                    if(stack.Stack.Count == 0) {
                        responder.SetResponse(204, "No Content", "The request was fine, but the user doesn't have any cards");
                        return;
                    }
                    foreach(Card card in stack.Stack) {
                        card.SetCardType();
                        card.SetElementType();
                    }
                    responder.SetResponse(200, "OK", JsonSerializer.Serialize(stack.Stack), "Content-Type", "application/json");
                    return;
                }
                catch (Exception e) {
                    responder.SetResponse(400, "Bad Request", e.Message);
                    return;
                }

            }
            responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
            return;
        }
    }
}
