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
        public void TestSetElementandCardtype() {
            //Arrange
            Card card = new Card();
            card.Name = "FireSpell";
            //Act
            card.SetCardandElementtype();
            //Assert
            Assert.That(card.Cardtype, Is.EqualTo(CardType.spellcard));

        }
        [Test]
        public void TestSplitforSpecialty() {
            //Arrange
            Card card = new Card();
            //Act
            string specialty = card.SplitCardNameforSpecialty("FireSpell");
            //Assert
            Assert.That(specialty, Is.EqualTo("Spell"));
        }

        [Test]
        public void TestCardsareMonstercards() {
            //Arrange
            Card card1 = new Card();
            Card card2 = new Card();
            //Act
            card1.Cardtype = CardType.monstercard;
            card2.Cardtype = CardType.spellcard;
            //Assert
            Assert.That(card1.Cardtype, Is.EqualTo(CardType.monstercard));
            Assert.That(card2.Cardtype, Is.Not.EqualTo(CardType.monstercard));
        }
        [Test]
        public void TestCardsareSpellcards() {
            //Arrange
            Card card1 = new Card();
            Card card2 = new Card();
            //Act
            card1.Cardtype = CardType.spellcard;
            card2.Cardtype = CardType.monstercard;
            //Assert
            Assert.That(card1.Cardtype, Is.EqualTo(CardType.spellcard));
            Assert.That(card2.Cardtype, Is.Not.EqualTo(CardType.spellcard));
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
            int randomcard = Battle.RandomCardfromDeck(player);
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
            card1.Name = "FireGoblin";
            card2.Name = "WaterDragon";
            card3.Name = "FireDragon";
            bool result = Rules.CheckSpecialties(card1, card2);
            bool result2 = Rules.CheckSpecialties(card2, card3);
            //Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(result2, Is.EqualTo(false));
        }
        



        


    }

}

