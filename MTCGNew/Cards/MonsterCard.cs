using MTCGNew.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Cards {
    internal class MonsterCard : Card {

        public CardType Cardtype { get; }

        public SpeciesType Speciestype { get; }


        //Constructor
        public MonsterCard(int damage, string cardname, ElementType element, SpeciesType species) : base(damage, cardname, element) {
            Cardtype = CardType.monstercard;
            Speciestype = species;
        }
    }
}
