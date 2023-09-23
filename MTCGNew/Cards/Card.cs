using MTCGNew.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Cards {
    internal abstract class Card {
        //variables
        protected readonly int _damage;
        protected string _cardname;

        //properties
        public int Damage {
            get { return _damage; }
        }

        public string Cardname {
            get { return _cardname; }
            set { _cardname = value; }
        }

        public ElementType Elementtype { get; private set; }

        //Constructor
        public Card(int damage, string cardname, ElementType element)
        {
            _damage = damage;
            _cardname = cardname;
            Elementtype = element;
            
        }


    }
}
