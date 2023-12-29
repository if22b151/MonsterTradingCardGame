using MTCGNew.Enums;
using MTCGNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTCGNew.Cards {
    public class Card {

        //properties
        public float Damage { get; set; }
        public string Name { get; set; } = "";
        public string Id { get; set; } = "";

        public ElementType? Elementtype { get; private set; }

        public CardType? Cardtype { get; set; }

        public string SplitCardNameforSpecialty(string cardname) {
            int uppercaseCount = 0;
            int index = 0;

            foreach (char c in cardname) {
                if (char.IsUpper(c)) {
                    uppercaseCount++;

                    if (uppercaseCount == 2) {
                        break;
                    }
                }

                index++;
            }

            return cardname.Substring(index);

        }
        public void SetCardandElementtype() {
            if (this.Name.Contains("Spell")) {
                this.Cardtype = CardType.spellcard;
            } else {
                this.Cardtype = CardType.monstercard;
            }
            if (this.Name.Contains("Fire")) {
                this.Elementtype = ElementType.fire;
            } else if (this.Name.Contains("Water")) {
                this.Elementtype = ElementType.water;
            } else {
                this.Elementtype = ElementType.normal;
            }

        }
    }
}
