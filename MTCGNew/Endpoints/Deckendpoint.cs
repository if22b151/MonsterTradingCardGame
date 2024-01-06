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
                responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
                return;
            }
            string inputString = request.Headers["Authorization"];
            string username = SessionHandling.GetUsername(inputString);
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if(SessionHandling.CheckSession(username) == false) {
                responder.SetResponse(401, "Unauthorized", "Not logged in!");
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if(rqauthtoken == usersessiontoken) {
                List<string>? deck = JsonSerializer.Deserialize<List<string>>(request.Content ?? "");
                if(deck == null || deck.Count != 4) {
                    responder.SetResponse(400, "Bad Request", "The provided deck did not include the required amount of cards");
                    return;
                }
                Deckrepository deckrepository = new Deckrepository();
                try {
                    deckrepository.EditDeck(deck, username);
                    responder.SetResponse(200, "OK", "The deck has been successfully configured");
                    return;
                }

                catch(ArgumentException e) {
                    responder.SetResponse(403, "Forbidden", e.Message);
                    return;
                }


                catch (Exception e) {
                    responder.SetResponse(400, "Bad Request", e.Message);
                    return;
                }

            }
            responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
        }

        private void GetDeck(RequestParser request, HTTPResponder responder) {
            if(!request.Headers.ContainsKey("Authorization")) {
                responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
                return;
            }
            string inputString = request.Headers["Authorization"];
            string username = SessionHandling.GetUsername(inputString);
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if (SessionHandling.CheckSession(username) == false) {
                responder.SetResponse(401, "Unauthorized", "Not logged in!");
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if(rqauthtoken == usersessiontoken) {
                Deckrepository deckrepository = new Deckrepository();
                try {
                    Deckcards deck = deckrepository.GetCards(username);
                    if(deck.Deck.Count() == 0) {
                        responder.SetResponse(204, "No Content", "The request was fine, but the deck doesn't have any cards");
                        return;
                    }
                    foreach (Card card in deck.Deck) {
                        card.SetCardType();
                        card.SetElementType();
                    }
                    if (request.QueryParameters.ContainsKey("format") && request.QueryParameters["format"] == "plain") {
                        responder.SetResponse(200, "OK", deck.ToString(), "Content-Type", "format/plain");
                    } else {
                        responder.SetResponse(200, "OK", JsonSerializer.Serialize(deck.Deck), "Content-Type", "application/json");

                    }
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