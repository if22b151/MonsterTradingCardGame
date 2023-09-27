using MTCGNew.Cards;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
namespace MTCGNew
{
    internal class Package {
        private const int amount = 5;

        private List<Card> _cards;

        public List<Card> Cards {
            get { return _cards; }
        }
        
        public Package()
        {
           _cards = new List<Card>(); 
           CardFactory factory = new();
           for(int i = 0; i < amount; i++) {
               _cards.Add(factory.GenerateCard());
            }
        }
    }
}
