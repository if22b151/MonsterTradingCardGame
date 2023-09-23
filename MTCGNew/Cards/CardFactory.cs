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
            return random.Next(21);
        }

        public ElementType DecideElementType() {
            Array enumvalues = Enum.GetValues(typeof(ElementType));
            Random random = new();
            ElementType elementType = (ElementType)enumvalues.GetValue(random.Next(enumvalues.Length));
            return elementType;
        }

        public SpeciesType DecideSpeciesType() {
            Array enumvalues = Enum.GetValues(typeof(SpeciesType));
            Random random = new();
            SpeciesType speciesType = (SpeciesType)enumvalues.GetValue(random.Next(enumvalues.Length));
            return speciesType;
        }
        public string DecideCardNameMonster(ElementType elementtype, SpeciesType speciestype) {
            if (elementtype == ElementType.fire && speciestype == SpeciesType.Wizzard) {
                return "FireWizard";
            } else if (elementtype == ElementType.fire && speciestype == SpeciesType.Dragon) {
                return "FireDragon";
            }
            return "FireElve";
        }

        public string DecideCardNameSpell(ElementType elementtype) {
            return "RegularSpell";
        }
     

        public Card GenerateCard()
        {
            ElementType elementtype = DecideElementType();
            if (DecideCardType() == CardType.monstercard) {
                SpeciesType speciestype = DecideSpeciesType();
                string cardname = DecideCardNameMonster(elementtype, speciestype);
                Card card = new MonsterCard(DecideDamage(), cardname, elementtype, speciestype);
                return card;
            } else {
                Card card = new SpellCard(DecideDamage(), DecideCardNameSpell(elementtype), elementtype);
                return card;
            }
        }

    }
}