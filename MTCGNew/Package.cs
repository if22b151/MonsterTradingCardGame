using MTCGNew.Cards;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
namespace MTCGNew
{
    internal class Package {

        private List<Card>? _cards;

        public List<Card>? Cards {
            get { return _cards; }
        }
        
        public Package()
        { }
    }
}
