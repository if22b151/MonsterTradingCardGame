using MCTGServer;
using MTCGNew.Cards;
using MTCGNew.Repositories;
using System.Text.Json;
using MTCGNew.Models;

namespace MTCGNew.Endpoints
{
    internal class Deckendpoint : IHTTPEndpoint {

        public bool HandleRequest(RequestParser request, HTTPResponder responder) {

            if(request.Method == HttpMethods.GET) {
                GetDeck(request, responder);
                return true;
            } else if(request.Method == HttpMethods.PUT) {
                EditDeck(request, responder);
                return true;
            }

            return false;
        }

        private void EditDeck(RequestParser request, HTTPResponder responder) {
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
            if(SessionHandling.CheckSession(username) == false) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Not logged in!";
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if(rqauthtoken == usersessiontoken) {
                List<string>? deck = JsonSerializer.Deserialize<List<string>>(request.Content ?? "");
                if(deck == null || deck.Count != 4) {
                    responder.ReturnCode = 400;
                    responder.ReturnText = "Bad Request";
                    responder.Content = "The provided deck did not include the required amount of cards";
                    return;
                }
                Deckrepository deckrepository = new Deckrepository();
                try {
                    deckrepository.EditDeck(deck, username);
                    responder.ReturnCode = 200;
                    responder.ReturnText = "OK";
                    responder.Content = "The deck has been successfully configured";
                    return;
                }

                catch(ArgumentException e) {
                    responder.ReturnCode = 403;
                    responder.ReturnText = "Forbidden";
                    responder.Content = e.Message;
                    return;
                }


                catch (Exception e) {
                    responder.ReturnCode = 400;
                    responder.ReturnText = "Bad Request";
                    responder.Content = e.Message;
                    return;
                }

            }


            responder.ReturnCode = 401;
            responder.ReturnText = "Unauthorized";
            responder.Content = "Access token is missing or invalid";
        }

        private void GetDeck(RequestParser request, HTTPResponder responder) {
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
            if (SessionHandling.CheckSession(username) == false) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Not logged in!";
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if(rqauthtoken == usersessiontoken) {
                Deckrepository deckrepository = new Deckrepository();
                try {
                    Deckcards? deck = deckrepository.GetCards(username);
                    if(deck == null) {
                        responder.ReturnCode = 204;
                        responder.ReturnText = "No Content";
                        responder.Content = "The request was fine, but the deck doesn't have any cards";
                        return;
                    }
                    responder.ReturnCode = 200;
                    responder.ReturnText = "OK";
                    if (request.QueryParameters.ContainsKey("format") && request.QueryParameters["format"] == "plain") {
                        responder.Headers.Add("Content-Type", "format/plain");
                        responder.Content = JsonSerializer.Serialize(deck);
                    } else {
                        responder.Content = JsonSerializer.Serialize(deck);
                        responder.Headers.Add("Content-Type", "application/json");

                    }
                    return;
                }
                catch (Exception e) {
                    responder.ReturnCode = 400;
                    responder.ReturnText = "Bad Request";
                    responder.Content = e.Message;
                    return;
                }
            }


            responder.ReturnCode = 401;
            responder.ReturnText = "Unauthorized";
            responder.Content = "Access token is missing or invalid";
            return;


        }
    }
}