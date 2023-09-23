using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew {
    internal class User {
        //Variables

        private static int _id = 0;

        StackCards stackcards;

        Deck deckcards;

        //Properties
        public string Username { get; set; }
        public string Password { get; set; }
        public int Coins { get; set; }
        public int Wins { get; set; }

        public int Losses { get; set; }

        public User(string username, string password)
        {
            _id = _id++;
            _ = new Credentials(username, password);
            Coins = 20;
            Wins = 0;
            Losses = 0;

        }
    }
}
