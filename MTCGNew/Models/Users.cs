using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Models {
    internal class Users {

        //Properties
        
        public int Id { get; set; }

        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int Coins { get; set; }
        public int Wins { get; set; }

        public int Losses { get; set; }

        public int Elo { get; set; }

        public string? Bio { get; set; }

        public string? Image { get; set; }

        public string? Name { get; set; }

        public Users() {
            Coins = 20;
            Wins = 0;
            Losses = 0;
            Elo = 100;
        }
    }
}
