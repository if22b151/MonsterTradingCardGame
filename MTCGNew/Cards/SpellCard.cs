using MTCGNew.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Cards {
    internal class SpellCard : Card {
        public CardType Cardtype { get; }


        public SpellCard(int damage, string cardname, ElementType element) : base(damage, cardname, element) {

            Cardtype = CardType.spellcard;
        }
    }
}
