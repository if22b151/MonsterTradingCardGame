using MTCGNew.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Models {
    internal class TradingDeal {

        public string Id { get; set; } = "";
        public string CardToTrade { get; set; } = "";

        public string Type { get; set; } = "";

        public double MinimumDamage { get; set; }

    }
}
