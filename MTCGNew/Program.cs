using MTCGNew.Cards;
using System.Threading.Channels;

namespace MTCGNew {
    internal class Program {
        static void Main(string[] args) {
            Package package = new();
            foreach(Card card in package.Cards) {
                Console.WriteLine(card.Cardname);
            }
        }
    }
}