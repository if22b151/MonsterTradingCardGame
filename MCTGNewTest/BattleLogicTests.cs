using MTCGNew.Cards;
using MTCGNew.Enums;
using MTCGNew.Models;
using MTCGNew.Battle;


namespace MCTGNewBattleTests {
    public class BattleTests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void TestSetElementType() {
            //Arrange
            Card card = new Card();
            Card card2 = new Card();
            card.Name = "FireSpell";
            card2.Name = "WaterGoblin";
            //Act
            card.SetElementType();
            //Assert
            Assert.That(card.Elementtype, Is.EqualTo(ElementType.fire));
            Assert.That(card2.Elementtype, Is.Not.EqualTo(ElementType.fire));

        }
        [Test]
        public void TestSetCardType() {
            //Arrange
            Card card = new Card();
            Card card2 = new Card();
            card.Name = "FireSpell";
            card2.Name = "FireGoblin";
            //Act
            card.SetCardType();
            //Assert
            Assert.That(card.Cardtype, Is.EqualTo(CardType.spell));
            Assert.That(card2.Cardtype, Is.Not.EqualTo(CardType.spell));
        }


        [Test]
        public void TestSplitforSpecialty() {
            //Arrange
            Card card = new Card();
            Card card2 = new Card();
            Card card3 = new Card();
            Card card4 = new Card();
            card.Name = "FireGoblin";
            card2.Name = "Ork";
            card3.Name = "WaterSpell";
            card4.Name = "FireElves";
            //Act
            string specialty = card.SplitCardNameforSpecialty(card.Name);
            string specialty2 = card2.SplitCardNameforSpecialty(card2.Name);
            string specialty3 = card3.SplitCardNameforSpecialty(card3.Name);
            string specialty4 = card4.SplitCardNameforSpecialty(card4.Name);
            //Assert
            Assert.That(specialty, Is.EqualTo("Goblin"));
            Assert.That(specialty2, Is.EqualTo("Ork"));
            Assert.That(specialty3, Is.EqualTo("WaterSpell"));
            Assert.That(specialty4, Is.EqualTo("FireElves"));
        }

        [Test]
        public void TestCardsareMonstercards() {
            //Arrange
            Card card1 = new Card();
            Card card2 = new Card();
            //Act
            card1.Cardtype = CardType.monster;
            card2.Cardtype = CardType.spell;
            //Assert
            Assert.That(card1.Cardtype, Is.EqualTo(CardType.monster));
            Assert.That(card2.Cardtype, Is.Not.EqualTo(CardType.monster));
        }
        [Test]
        public void TestCardsareSpellcards() {
            //Arrange
            Card card1 = new Card();
            Card card2 = new Card();
            //Act
            card1.Cardtype = CardType.spell;
            card2.Cardtype = CardType.monster;
            //Assert
            Assert.That(card1.Cardtype, Is.EqualTo(CardType.spell));
            Assert.That(card2.Cardtype, Is.Not.EqualTo(CardType.spell));
        }

        [Test]
        public void TestRandomCardfromDeck() {
            //Arrange
            Player player = new Player();
            Card card1 = new Card();
            Card card2 = new Card();
            Card card3 = new Card();
            player.Deck.Add(card1);
            player.Deck.Add(card2);
            player.Deck.Add(card3);
            //Act
            int randomcard = BattleLogic.RandomCardfromDeck(player);
            //Assert
            Assert.That(randomcard, Is.GreaterThanOrEqualTo(0));
            Assert.That(randomcard, Is.LessThanOrEqualTo(2));
        }

        [Test]
        public void TestEffectivenessCheck() {
            //Arrange
            Card card1 = new Card();
            Card card2 = new Card();
            //Act
            card1.Elementtype = ElementType.fire;
            card2.Elementtype = ElementType.water;
            card1.Damage = 10;
            card2.Damage = 10;
            Rules.EffectivenessCheck(card1, card2);
            //Assert
            Assert.That(card1.Damage, Is.EqualTo(5.0));
            Assert.That(card2.Damage, Is.EqualTo(20) );
        }
        [Test]
        public void TestCheckSpecialties() {
            //Arrange
            Card card1 = new Card();
            Card card2 = new Card();
            Card card3 = new Card();
            //Act
            card1.Name = "Knight";
            card2.Name = "WaterSpell";
            card3.Name = "FireDragon";
            int result = Rules.CheckSpecialties(card1, card2);
            int result2 = Rules.CheckSpecialties(card2, card3);
            //Assert
            Assert.That(result, Is.EqualTo(9));
            Assert.That(result2, Is.EqualTo(-1));
        }

        [Test]
        public void TestAddtoLobby() {
            //Arrange
            Player player = new Player();
            Player player2 = new Player();
            //Act
            bool result = Lobby.AddtoLobby(player);
            bool result2 = Lobby.AddtoLobby(player2);
            //Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(result2, Is.EqualTo(true));

        }

        [Test]
        public void TestMonsterCardBattle() {
            //Arrange
            Player player = new Player();
            Player player2 = new Player();
            Card card1 = new Card();
            Card card2 = new Card();
            card1.Name = "FireDragon";
            card1.Damage = 10;
            card2.Name = "WaterGoblin";
            card2.Damage = 20;
            card1.SetCardType();
            card1.SetElementType();
            card2.SetCardType();
            card2.SetElementType();
            player.Deck.Add(card1);
            player2.Deck.Add(card2);
            BattleLogic battle = new BattleLogic(player, player2);
            //Act
            battle.BattleLoop();
            //Assert
            Assert.That(player2.Deck.Count, Is.EqualTo(0));
        }
        [Test]
        public void TestSpellCardBattle() {
            //Arrange
            Player player = new Player();
            Player player2 = new Player();
            Card card1 = new Card();
            Card card2 = new Card();
            card1.Name = "FireSpell";
            card1.Damage = 10;
            card2.Name = "WaterSpell";
            card2.Damage = 20;
            card1.SetCardType();
            card1.SetElementType();
            card2.SetCardType();
            card2.SetElementType();
            player.Deck.Add(card1);
            player2.Deck.Add(card2);
            BattleLogic battle = new BattleLogic(player, player2);
            //Act
            battle.BattleLoop();
            //Assert
            Assert.That(player2.HasWon, Is.EqualTo(true));
            Assert.That(player2.Deck.Count, Is.EqualTo(2));
        }
        
        [Test]
        public void TestMixedCardBattle() {
            //Arrange
            Player player = new Player();
            Player player2 = new Player();
            Card card1 = new Card();
            Card card2 = new Card();
            card1.Name = "WaterDragon";
            card1.Damage = 25;
            card2.Name = "FireSpell";
            card2.Damage = 30;
            card1.SetCardType();
            card1.SetElementType();
            card2.SetCardType();
            card2.SetElementType();
            player.Deck.Add(card1);
            player2.Deck.Add(card2);
            BattleLogic battle = new BattleLogic(player, player2);
            //Act
            battle.BattleLoop();
            //Assert
            Assert.That(player2.Deck.Count, Is.EqualTo(0));
            Assert.That(player2.HasWon, Is.EqualTo(false));
        }

    }

}

