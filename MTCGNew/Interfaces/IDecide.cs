using MTCGNew.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Interfaces {
    internal interface IDecide {
        public int DecideDamage();

        public string DecideCardNameMonster(ElementType elementtype, SpeciesType speciestype);
        public string DecideCardNameSpell(ElementType elementtype);

        public ElementType DecideElementType();

        public CardType DecideCardType();

        public SpeciesType DecideSpeciesType();
    }
}
