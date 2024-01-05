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
        private StringBuilder battlelog = new StringBuilder();

        public BattleLogic(Player player1, Player player2) {
            this.player1 = player1;
            this.player2 = player2;
        }

        private bool CardsareMonstercards(Card card1, Card card2) {
            if(card1.Cardtype == CardType.monster && card2.Cardtype == CardType.monster) {
                return true;
            }
            return false;
        }
        private bool CardsareSpellcards(Card currentCard1, Card currentCard2) {
            if(currentCard1.Cardtype == CardType.spell && currentCard2.Cardtype == CardType.spell) {
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
            battlelog.Append($"{player1.Name}: {player1.CurrentCard.Name} ({player1.CurrentCard.Damage} Damage) vs {player2.Name}: {player2.CurrentCard.Name} ({player2.CurrentCard.Damage} Damage)\n");
        }

        private void PrintDamagetoConsole() {
            battlelog.Append($"{player1.CurrentCard.Damage} against {player2.CurrentCard.Damage}!\n");
        }

        private void TransferCardstoBattleWinner(Player player, List<Card> deck) {
            Stackrepository stackrepository = new Stackrepository();
            stackrepository.ChangeOwnerofCards(player.Name, deck);                
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

            if (Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 1) {
                battlelog.Append("Goblin is too afraid of Dragon\n");
                battlelog.Append("Dragon defeats Goblin!\n");
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
                return;
            }

            if(Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 2) {
                battlelog.Append("Goblin is too afraid of Dragon\n");
                battlelog.Append("Dragon defeats Goblin!\n");
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            }

            if (Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 3) {
                battlelog.Append("Wizzard controls Ork! Therefore Ork is not able to damage him!\n");
                battlelog.Append("Wizzard defeats Ork!\n");
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            }
            if (Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 4) {
                battlelog.Append("Wizzard controls Ork! Therefore Ork is not able to damage him!\n");
                battlelog.Append("Wizzard defeats Ork!\n");
                player1.Deck.Remove(player2.CurrentCard);
                player2.Deck.Add(player2.CurrentCard);
                return;
            }
            if (Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 5) {
                battlelog.Append("FireElves know Dragons since they were little! Therefore they can dodge his attacks!\n");
                battlelog.Append("FireElves defeats Dragon!\n");
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            }
            if (Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 6) {
                battlelog.Append("FireElves know Dragons since they were little! Therefore they can dodge his attacks!\n");
                battlelog.Append("FireElves defeats Dragon!\n");
                player1.Deck.Remove(player2.CurrentCard);
                player2.Deck.Add(player2.CurrentCard);
                return;
            }

            PrintDamagetoConsole();

            if(player1.CurrentCard.Damage > player2.CurrentCard.Damage) {
                battlelog.Append($"{player1.Name} wins!\n");
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
            } else if (player1.CurrentCard.Damage < player2.CurrentCard.Damage) {
                battlelog.Append($"{player2.Name} wins!\n");
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
            } else {
                battlelog.Append("Draw!\n");
            }

            
        }

        private void SpellCardFight() {
            PrintRoundtoConsole();

            float currentcarddamage1 = player1.CurrentCard.Damage;
            float currentcarddamage2 = player2.CurrentCard.Damage;

            PrintDamagetoConsole();

            Rules.EffectivenessCheck(player1.CurrentCard, player2.CurrentCard);

            battlelog.Append("Effectiveness have been applied!\n");

            PrintDamagetoConsole();

            if (player1.CurrentCard.Damage > player2.CurrentCard.Damage) {
                battlelog.Append($"{player1.Name} wins!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
            } else if (player1.CurrentCard.Damage < player2.CurrentCard.Damage) {
                battlelog.Append($"{player2.Name} wins!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
            } else {
                battlelog.Append("Draw!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;

            }
        }

        private void MixedCardFight() {
            PrintRoundtoConsole();

            if (Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 7) {
                battlelog.Append("Kraken is imune to spells!\n");
                battlelog.Append("Kraken defeats Spell!\n");
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            } 
            
            if (Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 8) {
                battlelog.Append("Kraken is imune to spells!\n");
                battlelog.Append("Kraken defeats Spell!\n");
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
                return;
            }


            if (Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 9) {
                battlelog.Append("Knight's armor is so heavy that the Waterspell makes him drown instantly!\n");
                battlelog.Append("Waterspell defeats Knight!\n");
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
                return;
            }
            
            if (Rules.CheckSpecialties(player1.CurrentCard, player2.CurrentCard) == 10) {
                battlelog.Append("Knight's armor is so heavy that the Waterspell makes him drown instantly!\n");
                battlelog.Append("Waterspell defeats Knight!\n");
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
                return;
            }

            PrintDamagetoConsole();

            float currentcarddamage1 = player1.CurrentCard.Damage;
            float currentcarddamage2 = player2.CurrentCard.Damage;

            Rules.EffectivenessCheck(player1.CurrentCard, player2.CurrentCard);

            battlelog.Append("Effectiveness have been applied!\n");

            PrintDamagetoConsole();

            if (player1.CurrentCard.Damage > player2.CurrentCard.Damage) {
                battlelog.Append($"{player1.Name} wins!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
                player2.Deck.Remove(player2.CurrentCard);
                player1.Deck.Add(player2.CurrentCard);
            } else if (player1.CurrentCard.Damage < player2.CurrentCard.Damage) {
                battlelog.Append($"{player2.Name} wins!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
                player1.Deck.Remove(player1.CurrentCard);
                player2.Deck.Add(player1.CurrentCard);
            } else {
                battlelog.Append("Draw!\n");
                player1.CurrentCard.Damage = currentcarddamage1;
                player2.CurrentCard.Damage = currentcarddamage2;
            }


        }


        public string BattleLoop() {
            battlelog.Append($"Battle starts between {player1.Name} and {player2.Name}!\n");
            int round = 1;
            while(round <= MAXROUNDS) {
                battlelog.Append("===================================================\n");
                battlelog.Append($"Round {round} starts!\n");
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
                    battlelog.Append($"{player1.Name} has no cards left! {player2.Name} wins!\n");
                    battlelog.Append($"Battle ends at Round {round}!\n");
                    battlelog.Append("===================================================\n");
                    player2.HasWon = true;
                    DeleteDeck(player1);
                    TransferCardstoBattleWinner(player2, player2.Deck);
                    UpdateUserStats();
                    return battlelog.ToString();
                } else if (player2.Deck.Count == 0) {
                    battlelog.Append($"Battle ends at Round {round}!\n");
                    battlelog.Append("===================================================\n"); 
                    battlelog.Append($"{player2.Name} has no cards left! {player1.Name} wins!\n");
                    player1.HasWon = true;
                    DeleteDeck(player2);
                    TransferCardstoBattleWinner(player1, player1.Deck);
                    UpdateUserStats();
                    return battlelog.ToString();
                }
                battlelog.Append($"Round {round} ends!\n");
                battlelog.Append("===================================================\n");
                round++;
            }
            battlelog.Append("Battle ends in a draw!\n");
            return battlelog.ToString();
        }

    }
}
