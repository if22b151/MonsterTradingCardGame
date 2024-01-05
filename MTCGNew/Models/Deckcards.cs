using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGNew.Cards;

namespace MTCGNew.Models {
    public class Deckcards {
        public List<Card> Deck { get; set; } = new List<Card>();

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            sb.Append('[');
            sb.Append('\n');


            foreach (Card card in Deck) {
                sb.Append(card.ToString());
                sb.Append('\n');
            }

            sb.Append(']');

            return sb.ToString();
        }

    }
}
