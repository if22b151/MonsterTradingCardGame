using MTCGNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Battle {
    public class Lobby {

        public static Queue<Player> players = new Queue<Player>();

        public static bool AddtoLobby(Player player) {
            players.Enqueue(player);
            lock(players) {
                if(players.Count >= 2) {
                    return true;
                }
                return false;
            }
        }

    }
}
