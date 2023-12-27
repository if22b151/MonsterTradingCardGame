using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Battle {
    internal class Lobby {

        public static Queue<Player> players = new Queue<Player>();

        public static void AddtoLobby(Player player) {
            players.Enqueue(player);
        }

    }
}
