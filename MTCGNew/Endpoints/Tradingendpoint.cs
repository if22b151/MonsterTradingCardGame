using MCTGServer;
using MTCGNew.Models;
using MTCGNew.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGNew.Endpoints {
    internal class Tradingendpoint : IHTTPEndpoint {
        public bool HandleRequest(RequestParser request, HTTPResponder responder) {
            if(request.Method == HttpMethods.POST) {
                if(request.Path.Count() == 2) {
                    StartTradingDeal(request, responder);
                    return true;
                }
                if(request.Path.Count() == 3) {
                    TryTrading(request, responder);
                    return true;
                }
            }
            if(request.Method == HttpMethods.GET) {
                GetTradingDeals(request, responder);
                return true;
            }
            if(request.Method == HttpMethods.DELETE) {
                if(request.Path.Count() == 3) {
                    DeleteTradingDeal(request, responder);
                    return true;
                }
            }
            return false;
        }

        private void DeleteTradingDeal(RequestParser request, HTTPResponder responder) {
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
                string tradingdealid = request.Path[2];
                try {
                    TradingRepository tradingRepository = new TradingRepository();
                    tradingRepository.DeleteTradingDeal(tradingdealid, username);
                    responder.SetResponse(200, "OK", "Trading deal successfully deleted");
                    return;
                }
                catch(ArgumentException e) {
                    responder.SetResponse(403, "Forbidden", e.Message);
                    return;
                }
                catch(SqlNotFilledException e) {
                    responder.SetResponse(404, "Not Found", e.Message);
                    return;
                }
                catch(Exception e) {
                    responder.SetResponse(400, "Bad Request", e.Message);   
                    return;
                }
            }
            responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
            return;
        }

        private void TryTrading(RequestParser request, HTTPResponder responder) {
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
                string? cardid = JsonSerializer.Deserialize<string>(request.Content ?? "");
                if(cardid == "" || cardid == null) {
                    responder.SetResponse(400, "Bad Request", "No card ID was sent with request!");
                    return;
                }
                string tradingdealid = request.Path[2];
                try {
                    TradingRepository tradingRepository = new TradingRepository();
                    tradingRepository.TryTrading(tradingdealid, cardid, username);
                    responder.SetResponse(200, "OK", "Trading deal successfully executed");
                    return;
                }
                catch(ArgumentException e) {
                    responder.SetResponse(403, "Forbidden", e.Message);
                    return;
                }
                catch(SqlNotFilledException e) {
                    responder.SetResponse(404, "Not Found", e.Message);
                    return;
                }
                catch(Exception e) {
                    responder.SetResponse(400, "Bad Request", e.Message);
                    return;
                }
            }
        }

        private void GetTradingDeals(RequestParser request, HTTPResponder responder) {
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
            if (rqauthtoken == usersessiontoken) {
                TradingRepository tradingRepository = new TradingRepository();
                List<TradingDeal> tradingDeals = tradingRepository.GetTradingDeals(username);
                if (tradingDeals.Count() == 0) {
                    responder.SetResponse(204, "No Content", "The request was fine, but there are no trading deals available");
                    return;
                }
                responder.SetResponse(200, "OK", JsonSerializer.Serialize(tradingDeals), "Content-Type", "application/json");
                return;
            }
            responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
            return;
        }

        private void StartTradingDeal(RequestParser request, HTTPResponder responder) {
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
            if (rqauthtoken == usersessiontoken) { 
                TradingDeal? tradingDeal = JsonSerializer.Deserialize<TradingDeal>(request.Content ?? "");
                if(tradingDeal == null) {
                    responder.SetResponse(400, "Bad Request", "No trading deal was sent with request!");
                    return;
                }
                Stackrepository stackrepository = new Stackrepository();
                if(!stackrepository.CardbelongstoUserorinDeck(tradingDeal.CardToTrade, username)) {
                    responder.SetResponse(403, "Forbidden", "The deal contains a card that is not owned by the user or locked in the deck.");
                    return;
                }
                try {
                    TradingRepository tradingDealRepository = new TradingRepository();
                    tradingDealRepository.CreateTradingDeal(tradingDeal, username);
                    responder.SetResponse(201, "Created", "Trading deal successfully created");
                    return;
                }
                catch(ArgumentException e) {
                    responder.SetResponse(403, "Forbidden", e.Message);
                    return;
                }
                catch(Exception e) {
                    responder.SetResponse(400, "Bad Request", e.Message);
                    return;
                }
            }
            responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
            return;
        }
    }
}
