using MTCGNew.Enums;
using MTCGNew.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace MTCGNew.Cards
{
    internal class CardFactory : IDecide {
        public CardFactory()
        { }

        public CardType DecideCardType() {
            Array enumvalues = Enum.GetValues(typeof(CardType));
            Random random = new();
            CardType cardType = (CardType)enumvalues.GetValue(random.Next(enumvalues.Length));
            return cardType;
        }

        public int DecideDamage() {
            Random random = new();
            return random.Next(5, 21);
        }

        public ElementType DecideElementTypeforSpells(SpellCardNames spellcardname) {
            if(spellcardname.ToString().StartsWith("Water")) {
                return ElementType.water;
            } else if(spellcardname.ToString().StartsWith("Fire")) {
                return ElementType.fire;
            } else { 
                return ElementType.normal; 
            }
        }

        public ElementType DecideElementTypeforMonsters(MonsterCardNames monstercardname) {
            if (monstercardname.ToString().StartsWith("Water")) {
                return ElementType.water;
            } else if (monstercardname.ToString().StartsWith("Fire")) {
                return ElementType.fire;
            } else {
                return ElementType.normal;
            }
        }


        public SpeciesType DecideSpeciesType(MonsterCardNames monstercardname) {
            if(monstercardname.ToString().EndsWith("Goblin")) {
                return SpeciesType.Goblin;
            }
            else if (monstercardname.ToString().EndsWith("Dragon")) {
                return SpeciesType.Dragon;
            }
            else if(monstercardname.ToString().EndsWith("Wizzard")) {
                return SpeciesType.Wizzard;
            }
            else if(monstercardname.ToString().EndsWith("Ork")) {
                return SpeciesType.Ork;
            }
            else if(monstercardname.ToString().EndsWith("Knight")) {
                return SpeciesType.Knight;
            }
            else if(monstercardname.ToString().EndsWith("Kraken")) {
                return SpeciesType.Kraken;
            }else {
                return SpeciesType.Elve;
            }
        }
        public MonsterCardNames DecideCardNameMonster() {
            Array enumvalues = Enum.GetValues(typeof(MonsterCardNames));
            Random random = new();
            MonsterCardNames monstercardname = (MonsterCardNames)enumvalues.GetValue(random.Next(enumvalues.Length));
            return monstercardname;
        }

        public SpellCardNames DecideCardNameSpell() {
            Array enumvalues = Enum.GetValues(typeof(SpellCardNames));
            Random random = new();
            SpellCardNames spellcardname = (SpellCardNames)enumvalues.GetValue(random.Next(enumvalues.Length));
            return spellcardname;
        }
     

        public Card GenerateCard()
        {
            if (DecideCardType() == CardType.monstercard) {
                MonsterCardNames cardname = DecideCardNameMonster();       
                Card card = new MonsterCard(DecideDamage(), cardname.ToString(), DecideElementTypeforMonsters(cardname), DecideSpeciesType(cardname));
                return card;
            } else {
                SpellCardNames spellcardname = DecideCardNameSpell();  
                Card card = new SpellCard(DecideDamage(), spellcardname.ToString(), DecideElementTypeforSpells(spellcardname));
                return card;
            }
        }

    }
}