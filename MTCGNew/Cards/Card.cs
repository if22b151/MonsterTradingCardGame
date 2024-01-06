using Microsoft.VisualBasic;
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
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public float Damage { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CardType Cardtype { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ElementType Elementtype { get; set; }


        public override string ToString() {
            return $"Id: {Id}, Cardname: {Name}, Damage: {Damage}, CardType: {Cardtype}, Elementtype: {Elementtype}";
        }


        public string SplitCardNameforSpecialty(string cardname) {

            if (cardname == "FireElves" || cardname == "WaterSpell") {
                return cardname;
            }

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

            if (index == cardname.Length) {
                return cardname;
            }

            return cardname.Substring(index);

        }
        public void SetCardType() {
            if (this.Name.Contains("Spell")) {
                this.Cardtype = CardType.spell;
            } else {
                this.Cardtype = CardType.monster;
            }

        }
        public void SetElementType() {
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
