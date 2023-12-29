using NUnit.Framework;
using MTCGNew.Models;
using MTCGNew.Cards;
using MTCGNew.Enums;


namespace MCTGNewTest {
    public class Tests {
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
    }
}