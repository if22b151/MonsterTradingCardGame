using MTCGNew.Cards;
using MTCGNew.Enums;
using MTCGNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Battle {
    public class Rules {

        public static void EffectivenessCheck(Card currentcard1, Card currentcard2) {
            if (currentcard1.Elementtype == ElementType.water && currentcard2.Elementtype == ElementType.fire) {
                currentcard1.Damage *= 2;
                currentcard2.Damage /= 2;
                return;
            } else if (currentcard2.Elementtype == ElementType.water && currentcard1.Elementtype == ElementType.fire) {
                currentcard2.Damage *= 2;
                currentcard1.Damage /= 2;
                return;
            }
            if (currentcard1.Elementtype == ElementType.fire && currentcard2.Elementtype == ElementType.normal) {
                currentcard1.Damage *= 2;
                currentcard2.Damage /= 2;
                return;
            } else if (currentcard2.Elementtype == ElementType.fire && currentcard1.Elementtype == ElementType.normal) {
                currentcard2.Damage *= 2;
                currentcard1.Damage /= 2;
                return;
            }
            if (currentcard1.Elementtype == ElementType.normal && currentcard2.Elementtype == ElementType.water) {
                currentcard1.Damage *= 2;
                currentcard2.Damage /= 2;
                return;
            } else if (currentcard2.Elementtype == ElementType.normal && currentcard1.Elementtype == ElementType.water) {
                currentcard2.Damage *= 2;
                currentcard1.Damage /= 2;
                return;
            }
        }

        public static bool CheckSpecialties(Card currentcard1, Card currentcard2) {
            string specialty1 = currentcard1.SplitCardNameforSpecialty(currentcard1.Name);
            string specialty2 = currentcard2.SplitCardNameforSpecialty(currentcard2.Name);
            if(specialty1 == "Goblin" && specialty2 == "Dragon") {
                return true;
            } else if(specialty1 == "Dragon" && specialty2 == "Goblin") {
                return true;
            }
            if(specialty1 == "Knight" && specialty2 == "Ork") {
                return true;
            } else if(specialty1 == "Ork" && specialty2 == "Knight") {
                return true;
            }
            if(specialty1 == "FireElves" && specialty2 == "Dragon") {
                return true;
            } else if(specialty1 == "Dragon" && specialty2 == "FireElves") {
                return true;
            }
            if(specialty1 == "Kraken" && specialty2 == "Spell") {
                return true;
            } else if(specialty1 == "Spell" && specialty2 == "Kraken") {
                return true;
            }
            if(specialty1 == "Knight" && specialty2 == "Spell" && currentcard2.Elementtype == ElementType.water) {
                return true;
            } else if(specialty1 == "Spell" && currentcard1.Elementtype == ElementType.water && specialty2 == "Knight") {
                return true;
            }

            return false;
        }


    }
}
