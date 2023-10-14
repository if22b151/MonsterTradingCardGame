using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Enums {
    internal class EnumHelperClass {



        public static void DecideCardName<T>(T cardtype) {
            if (cardtype.Equals(CardType.monstercard)) {
                RandomGenerateCardname();
                Array enumvalues = Enum.GetValues(typeof(MonsterCardNames));
                Random rand = new();
                MonsterCardNames monstercardname = (MonsterCardNames)enumvalues.GetValue(rand.Next(enumvalues.Length));
                return monstercardname;
            } else {
                Array enumvalues = Enum.GetValues(typeof(SpellCardNames));
                Random rand = new();
                SpellCardNames spellcardname = (SpellCardNames)enumvalues.GetValue(rand.Next(enumvalues.Length));
                return spellcardname;
            }
        }
         public static ElementType DecideElementType<T>(T cardname) {
            if (cardname.ToString().StartsWith("Water")) {
                return ElementType.water;
            } else if (cardname.ToString().StartsWith("Fire")) {
                return ElementType.fire;
            } else {
                return ElementType.normal;
            }
        }
    }
}
