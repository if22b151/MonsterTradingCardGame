using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Models {
    public class UserStats {
        public string Name { get; set; } = "";
        public int Elo { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public double? Winrate { get; set;}

        public double CalculateWinrate() {
            if (Wins == 0 && Losses == 0) {
                return 0;
            }
            else if (Wins == 0) {
                return 0;
            }
            else if (Losses == 0) {
                return 100;
            }
            else {
                double winrate = (double)Wins / (Wins + Losses) * 100;
                return Math.Round(winrate, 0);
                
            }
           
        }
            
        
    }
}
