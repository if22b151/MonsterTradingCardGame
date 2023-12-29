using MTCGNew.Cards;
using MTCGNew.Enums;
using MTCGNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Battle {
    internal class Battle {
        private Player player1;
        private Player player2;
        private const int MAXROUNDS = 100;

        public Battle(Player player1, Player player2) {
            this.player1 = player1;
            this.player2 = player2;
            BattleLoop();
        }

        private bool CardsareMonstercards(Card card1, Card card2) {
            if(card1.Cardtype == CardType.monstercard && card2.Cardtype == CardType.monstercard) {
                return true;
            }
            return false;
        }
        private bool CardsareSpellcards(Card currentCard1, Card currentCard2) {
            if(currentCard1.Cardtype == CardType.spellcard && currentCard2.Cardtype == CardType.spellcard) {
                return true;
            }
            return false;
        }

        private int RandomCardfromDeck(Player player) {
            Random rnd = new Random();
            int randomcard = rnd.Next(0, player.Deck.Count);
            return randomcard;
        }


        private void PrintRoundtoConsole() {
            Console.WriteLine($"{player1.Name}: {player1.CurrentCard.Name} ({player1.CurrentCard.Damage} Damage) vs {player2.Name}: {player1.CurrentCard.Name} ({player1.CurrentCard.Damage}");
        }

        private void TransferCardtoRoundWinner(Player player2, Card currentCard) {
            

        }

        private void MonsterCardFight() {
            PrintRoundtoConsole();
            string stringforspecialty = player1.CurrentCard.SplitCardNameforSpecialty(player1.CurrentCard.Name);
            string stringforspecialty2 = player2.CurrentCard.SplitCardNameforSpecialty(player2.CurrentCard.Name);
            if(stringforspecialty == "Goblin" && stringforspecialty2 == "Dragon") {
                Console.WriteLine("Goblin is too afraid of Dragon to attack!\n");
                Console.WriteLine("Dragon defeats Goblin!\n");
                TransferCardtoRoundWinner(player2, player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard); 
                return;
            } else if (stringforspecialty == "Dragon" && stringforspecialty2 == "Goblin") {
                Console.WriteLine("Goblin is too afraid of Dragon to attack!\n");
                Console.WriteLine("Dragon defeats Goblin!\n");
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            }
            if(stringforspecialty == "Wizzard" && stringforspecialty2 == "Ork") {
                Console.WriteLine("Wizzard controls Ork! Therefore Ork is not able to damage him!\n");
                Console.WriteLine("Wizzard defeats Ork!\n");
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            } else if(stringforspecialty == "Ork" && stringforspecialty2 == "Wizzard") {
                Console.WriteLine("Wizzard controls Ork! Therefore Ork is not able to damage him!\n");
                Console.WriteLine("Wizzard defeats Ork!\n");
                TransferCardtoRoundWinner(player2, player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
                return;
            }
            if(stringforspecialty == "FireElves" && stringforspecialty2 == "Dragon") {
                Console.WriteLine("FireElves know Dragons since they were little! Therefore they can dodge his attacks!\n");
                Console.WriteLine("FireElves defeats Dragon!\n");
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            } else if(stringforspecialty == "Dragon" && stringforspecialty2 == "FireElves") {
                Console.WriteLine("FireElves know Dragons since they were little! Therefore they can dodge his attacks!\n");
                Console.WriteLine("FireElves defeats Dragon!\n");
                TransferCardtoRoundWinner(player2, player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
                return;
            }

            Console.WriteLine($"");

            
        }


        private int BattleLoop() {
            Console.WriteLine($"Battle starts between {player1.Name} and {player2.Name}!");
            int round = 0;
            while(round <= MAXROUNDS) {
                Console.WriteLine("===================================================");
                Console.WriteLine($"Round {round} starts!");
                player1.CurrentCard = player1.Deck[RandomCardfromDeck(player1)];
                player2.CurrentCard = player2.Deck[RandomCardfromDeck(player2)];
                if(CardsareMonstercards(player1.CurrentCard, player2.CurrentCard)) {
                    MonsterCardFight();
                } 
                else if (CardsareSpellcards(player1.CurrentCard, player2.CurrentCard)) {
                    SpellCardFight();
                } 
                else {
                    MixedFight();
                }
                if(player1.Deck.Count == 0) {
                    Console.WriteLine($"{player1.Name} has no cards left! {player2.Name} wins!");
                    Console.WriteLine($"Battle ends at Round {round}!");
                    Console.WriteLine("==================================================="); 
                    return 1;
                } else if (player2.Deck.Count == 0) {
                    Console.WriteLine($"Battle ends at Round {round}!");
                    Console.WriteLine("==================================================="); 
                    Console.WriteLine($"{player2.Name} has no cards left! {player1.Name} wins!");
                    return 2;
                }
                Console.WriteLine($"Round {round} ends!");
                Console.WriteLine("===================================================");
                round++;
            }
            Console.WriteLine("Battle ends in a draw!");
            return 0;
        }

    }
}
