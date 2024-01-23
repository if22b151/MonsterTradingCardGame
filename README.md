# <center>MonsterTradingCardGame</center>

## This HTTP/REST-based server is built to be a platform for trading and battling with and against each other in a magical card-game world.

1. Game mechanics
  - register and login to the server,
  - acquire some cards,
  - define a deck of monsters/spells,
  - battle against each other and
  - compare their stats in the score-board.

2. The Battle Logic
  - Your cards are split into 2 categories:
    - monster cards
    ```
    cards with active attacks and damage based on an element type (fire, water, normal).
    The element type does not effect pure monster fights.
    ```
    - spell cards:
    ```
      a spell card can attack with an element based spell (again fire, water, normal) which is:
    – effective (eg: water is effective against fire, so damage is doubled)
    – not effective (eg: fire is not effective against water, so damage is halved)
    – no effect (eg: normal monster vs normal spell, no change of damage, direct
    comparison between damages) Effectiveness:
    - water -> fire
    - fire -> normal
    - normal -> water
    ```
  - The following specialties are to consider:
    ```
    Goblins are too afraid of Dragons to attack.
    Wizzard can control Orks so they are not able to damage them.
    The armor of Knights is so heavy that WaterSpells make them drown them instantly.
    The Kraken is immune against spells.
    The FireElves know Dragons since they were little and can evade their attacks.
    ```
3. Trading Deals \
  You can request a trading deal by pushing a card (concrete instance, not card type) into the
  store (MUST NOT BE IN THE DECK and is locked for the deck in further usage) and add a
  requirement (Spell or Monster and additionally a type requirement or a minimum damage)
  for the card to trade with (eg: “spell or monster” and “min-damage: 50”).
