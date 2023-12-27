using MTCGNew.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTCGNew.Cards {
    internal class Card {
        //variables

        //properties
        public float Damage { get; set; }
        public string Name { get; set; } = "";
        public string Id { get; set; } = "";

        // public ElementType? Elementtype { get; private set; }

        /*  public virtual void PrintStats() {
              Console.WriteLine($"Cardname: {_cardname}, Damage: {_damage}, Elementtype: {Elementtype}");
          }*/
        public Card() {
           
        }

    }
}
