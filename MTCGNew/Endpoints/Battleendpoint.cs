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
            lock (this) {
                if (!request.Headers.ContainsKey("Authorization")) {
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
                    try {
                        Deckrepository deckrepository = new Deckrepository();
                        Deckcards? deck = deckrepository.GetCards(username);
                        if (deck == null) {
                            responder.SetResponse(400, "Bad Request", "Deck is invalid!");
                            return;
                        }
                        Player player = new Player();
                        player.Deck = deck.Deck;
                        player.Name = username;
                        foreach (Card card in player.Deck) {
                            card.SetCardType();
                            card.SetElementType();
                        }
                        if(Lobby.AddtoLobby(player)) {
                            BattleLogic battle = new BattleLogic(Lobby.players.Dequeue(), Lobby.players.Dequeue());
                            string battlelog = battle.BattleLoop();
                            responder.SetResponse(200, "OK", battlelog);
                            return;
                        } else {
                            responder.SetResponse(202, "Accepted", "You are waiting in the lobby for other opponent!");
                            return;
                        }
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


        private void GetScoreboard(RequestParser request, HTTPResponder responder) {
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
                try {
                    Battlerepository battlerepository = new Battlerepository();
                    List<UserStats> scoreboard = battlerepository.GetScoreboard();
                    foreach(UserStats user in scoreboard) {
                        user.Winrate = user.CalculateWinrate();
                    }
                    responder.SetResponse(200, "OK", JsonSerializer.Serialize(scoreboard), "Content-Type", "application/json");
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

        private void GetStats(RequestParser request, HTTPResponder responder) {
            if (!request.Headers.ContainsKey("Authorization")) {
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
                try {
                    Battlerepository battlerepository = new Battlerepository();
                    UserStats? stats = battlerepository.GetStats(username);
                    if(stats == null) {
                        responder.SetResponse(404, "Not Found", "Stats not found!");
                        return;
                    }
                    stats.Winrate = stats.CalculateWinrate();
                    responder.SetResponse(200, "OK", JsonSerializer.Serialize(stats), "Content-Type", "application/json");
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
