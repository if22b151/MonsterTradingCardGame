using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGNew.Cards;

namespace MTCGNew.Models {
    public class Player {
        public string Name { get; set; } = "";

        public bool HasWon { get; set; } = false;

        public Card CurrentCard { get; set; } = new Card();

        public List<Card> Deck { get; set; } = new List<Card>();
    }
}
