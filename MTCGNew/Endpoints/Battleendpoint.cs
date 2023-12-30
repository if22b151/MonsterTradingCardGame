using MCTGServer;
using MTCGNew.Battle;
using MTCGNew.Cards;
using MTCGNew.Models;
using MTCGNew.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGNew.Endpoints {
    internal class Battleendpoint : IHTTPEndpoint {

        public bool HandleRequest(RequestParser request, HTTPResponder responder) {
            if(request.Method == HttpMethods.GET) {
                if (request.Path[1] == "stats") {
                    GetStats(request, responder);
                    return true;
                }
                if (request.Path[1] == "scoreboard") {
                    GetScoreboard(request, responder);
                    return true;
                }
            }
            if(request.Method == HttpMethods.POST) {
                    BattleLogin(request, responder);
                    return true;
                }
            
            return false;
        }

        private void BattleLogin(RequestParser request, HTTPResponder responder) {
            Player? player = new Player();
            lock (this) {
                if (!request.Headers.ContainsKey("Authorization")) {
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
                    try {
                        Deckrepository deckrepository = new Deckrepository();
                        Deckcards? deck = deckrepository.GetCards(username);
                        if (deck == null) {
                            responder.ReturnCode = 400;
                            responder.ReturnText = "Bad Request";
                            responder.Content = "Deck is invalid!";
                            return;
                        }

                        foreach (Card card in deck.Deck) {
                            card.SetCardandElementtype();
                        }

                        Battlerepository battlerepository = new Battlerepository();
                        player = battlerepository.BattlePrep(deck, username);
                        if (player == null) {
                            responder.ReturnCode = 400;
                            responder.ReturnText = "Bad Request";
                            responder.Content = "Player is invalid!";
                            return;
                        }
                        player.Name = username;
                    }
                    catch(Exception e) {
                        responder.ReturnCode = 400;
                        responder.ReturnText = "Bad Request";
                        responder.Content = e.Message;
                        return;
                    }
                 }


                if(Lobby.AddtoLobby(player)) {
                    BattleLogic battle = new BattleLogic(Lobby.players.Dequeue(), Lobby.players.Dequeue());
                    battle.BattleLoop();
                    responder.ReturnCode = 200;
                    responder.ReturnText = "OK";
                    responder.Content = "The battle has been carried out successfully.";
                    return;
                } else {
                    responder.ReturnCode = 202;
                    responder.ReturnText = "Accepted";
                    responder.Content = "The battle has been queued.";
                    return;
                }

            }
        }


        private void GetScoreboard(RequestParser request, HTTPResponder responder) {
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
                try {
                    Battlerepository battlerepository = new Battlerepository();
                    List<UserStats> scoreboard = battlerepository.GetScoreboard();
                    responder.ReturnCode = 200;
                    responder.ReturnText = "OK";
                    responder.Content = JsonSerializer.Serialize(scoreboard);
                    responder.Headers.Add("Content-Type", "application/json");
                    return;
                }
                catch(Exception e) {
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

        private void GetStats(RequestParser request, HTTPResponder responder) {
            if (!request.Headers.ContainsKey("Authorization")) {
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
                try {
                    Battlerepository battlerepository = new Battlerepository();
                    UserStats? stats = battlerepository.GetStats(username);
                    responder.ReturnCode = 200;
                    responder.ReturnText = "OK";
                    responder.Content = JsonSerializer.Serialize(stats);
                    responder.Headers.Add("Content-Type", "application/json");
                    return;
                }
                catch(Exception e) {
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
