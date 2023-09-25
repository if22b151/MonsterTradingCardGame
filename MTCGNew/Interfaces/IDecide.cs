using MTCGNew.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Interfaces {
    internal interface IDecide {
        public int DecideDamage();

        public MonsterCardNames DecideCardNameMonster();
        public SpellCardNames DecideCardNameSpell();

        public ElementType DecideElementTypeforSpells(SpellCardNames spellcardname);
        public ElementType DecideElementTypeforMonsters(MonsterCardNames monstercardname);
        public SpeciesType DecideSpeciesType(MonsterCardNames monstercardname);

        public CardType DecideCardType();

    }
}
