using MTCGNew.Cards;
using MTCGNew.Enums;
using MTCGNew.Models;
using MTCGNew.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Battle {
    public class BattleLogic {
        private Player player1;
        private Player player2;
        private const int MAXROUNDS = 100;

        public BattleLogic(Player player1, Player player2) {
            this.player1 = player1;
            this.player2 = player2;
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

        public static int RandomCardfromDeck(Player player) {
            Random rnd = new Random();
            int randomcard = rnd.Next(0, player.Deck.Count);
            return randomcard;
        }


        private void PrintRoundtoConsole() {
            Console.WriteLine($"{player1.Name}: {player1.CurrentCard.Name} ({player1.CurrentCard.Damage} Damage) vs {player2.Name}: {player1.CurrentCard.Name} ({player1.CurrentCard.Damage} Damage)");
        }

        private void PrintDamagetoConsole() {
            Console.WriteLine($"{player1.CurrentCard.Damage} against {player2.CurrentCard.Damage}!");
        }

        private void TransferCardtoRoundWinner(Player player, Card currentcard) {
            Stackrepository stackrepository = new Stackrepository();
            stackrepository.ChangeOwnerofCard(player.Name, currentcard);

            Console.WriteLine($"{player.Name} wins the round and gets {currentcard.Name}!");
                
        }

        private void UpdateUserStats() {
            UserRepository userrepository = new UserRepository();
            userrepository.UpdateUserStats(player1, player2);
        }

        private void DeleteDeck(Player player) {
            Deckrepository deckrepository = new Deckrepository();
            deckrepository.DeleteDeck(player.Name);
        }

        private void MonsterCardFight() {
            PrintRoundtoConsole();
       
            if(Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard)) {
                Console.WriteLine("Goblin is too afraid of Dragon to attack!\n");
                Console.WriteLine("Dragon defeats Goblin!\n");
                TransferCardtoRoundWinner(player2, player1.CurrentCard);
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
                return;
            } 
            if(Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard)) {
                Console.WriteLine("Wizzard controls Ork! Therefore Ork is not able to damage him!\n");
                Console.WriteLine("Wizzard defeats Ork!\n");
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            } 
            if(Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard)) {
                Console.WriteLine("FireElves know Dragons since they were little! Therefore they can dodge his attacks!\n");
                Console.WriteLine("FireElves defeats Dragon!\n");
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            } 
            
            PrintDamagetoConsole();

            if(player1.CurrentCard.Damage > player2.CurrentCard.Damage) {
                Console.WriteLine($"{player1.Name} wins!\n");
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
            } else if (player1.CurrentCard.Damage < player2.CurrentCard.Damage) {
                Console.WriteLine($"{player2.Name} wins!\n");
                TransferCardtoRoundWinner(player2, player1.CurrentCard);
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
            } else {
                Console.WriteLine("Draw!\n");
            }

            
        }

        private void SpellCardFight() {
            PrintRoundtoConsole();

            float currentcarddamage1 = player1.CurrentCard.Damage;
            float currentcarddamage2 = player2.CurrentCard.Damage;

            Rules.EffectivenessCheck(player1.CurrentCard, player2.CurrentCard);

            if (player1.CurrentCard.Damage > player2.CurrentCard.Damage) {
                Console.WriteLine($"{player1.Name} wins!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
            } else if (player1.CurrentCard.Damage < player2.CurrentCard.Damage) {
                Console.WriteLine($"{player2.Name} wins!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
                TransferCardtoRoundWinner(player2, player1.CurrentCard);
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
            } else {
                Console.WriteLine("Draw!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;

            }
        }

        private void MixedCardFight() {
            PrintRoundtoConsole();
            
            if(Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard)) {
                Console.WriteLine("Kraken is imune to spells!\n");
                Console.WriteLine("Kraken defeats Spell!\n");
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            } 
            
            if(Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard)) {
                Console.WriteLine("Knight's armor is so heavy that the Waterspell makes him drown instantly!\n");
                Console.WriteLine("Waterspell defeats Knight!\n");
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            } 

            PrintDamagetoConsole();

            float currentcarddamage1 = player1.CurrentCard.Damage;
            float currentcarddamage2 = player2.CurrentCard.Damage;

            Rules.EffectivenessCheck(player1.CurrentCard, player2.CurrentCard);

            if (player1.CurrentCard.Damage > player2.CurrentCard.Damage) {
                Console.WriteLine($"{player1.Name} wins!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
                TransferCardtoRoundWinner(player1, player2.CurrentCard);
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
            } else if (player1.CurrentCard.Damage < player2.CurrentCard.Damage) {
                Console.WriteLine($"{player2.Name} wins!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
                TransferCardtoRoundWinner(player2, player1.CurrentCard);
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
            } else {
                Console.WriteLine("Draw!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
            }


        }


        public void BattleLoop() {
            Console.WriteLine($"Battle starts between {player1.Name} and {player2.Name}!");
            int round = 1;
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
                    MixedCardFight();
                }
                if(player1.Deck.Count == 0) {
                    Console.WriteLine($"{player1.Name} has no cards left! {player2.Name} wins!");
                    Console.WriteLine($"Battle ends at Round {round}!");
                    Console.WriteLine("===================================================");
                    player2.HasWon = true;
                    DeleteDeck(player1);
                    UpdateUserStats();
                    return;
                } else if (player2.Deck.Count == 0) {
                    Console.WriteLine($"Battle ends at Round {round}!");
                    Console.WriteLine("==================================================="); 
                    Console.WriteLine($"{player2.Name} has no cards left! {player1.Name} wins!");
                    player1.HasWon = true;
                    DeleteDeck(player2);
                    UpdateUserStats();
                    return;
                }
                Console.WriteLine($"Round {round} ends!");
                Console.WriteLine("===================================================");
                round++;
            }
            Console.WriteLine("Battle ends in a draw!");
            return;
        }

    }
}
