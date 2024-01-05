--insert users into users table no admin with testuser1 and testuser2
INSERT INTO users (username, password, coins, wins, losses, elo, bio, image, name) VALUES ('testuser', 'testuser', 20, 2, 1, 110, 'bio', 'image', 'name');
INSERT INTO users (username, password, coins, wins, losses, elo, bio, image, name) VALUES ('testuser2', 'testuser2', 20, 1, 3, 78, 'bio2', 'image2', 'name2');
INSERT INTO users (username, password, coins, wins, losses, elo, bio, image, name) VALUES ('testuser3', 'testuser3', 20, 1, 3, 78, 'bio2', 'image2', 'name2');

--insert cards into cards table
INSERT INTO cards (card_id, name, damage) VALUES ('1', 'testcard1', 10);
INSERT INTO cards (card_id, name, damage) VALUES ('2', 'testcard2', 20);
INSERT INTO cards (card_id, name, damage) VALUES ('3', 'testcard3', 30);
INSERT INTO cards (card_id, name, damage) VALUES ('4', 'testcard4', 15);
INSERT INTO cards (card_id, name, damage) VALUES ('5', 'testcard5', 31);
INSERT INTO cards (card_id, name, damage) VALUES ('6', 'testcard6', 12);
INSERT INTO cards (card_id, name, damage) VALUES ('7', 'testcard7', 17);
INSERT INTO cards (card_id, name, damage) VALUES ('8', 'testcard8', 25);
INSERT INTO cards (card_id, name, damage) VALUES ('9', 'testcard9', 75);
INSERT INTO cards (card_id, name, damage) VALUES ('10', 'testcard10', 24);

--insert packages into packages table
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (1, '1');
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (1, '2');
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (1, '3');
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (1, '4');
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (1, '5');
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (2, '6');
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (2, '7');
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (2, '8');
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (2, '9');
INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (2, '10');

--insert stack into stack table
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('1', 1);
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('2', 1);
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('3', 1);
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('4', 1);
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('5', 1);
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('6', 2);
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('7', 2);
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('8', 2);
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('9', 2);
INSERT INTO stack (fk_card_id, fk_user_id) VALUES ('10', 2);

--insert deck into deck table
INSERT INTO deck (fk_card_id, fk_user_id) VALUES ('1', 1);
INSERT INTO deck (fk_card_id, fk_user_id) VALUES ('2', 1);
INSERT INTO deck (fk_card_id, fk_user_id) VALUES ('3', 1);
INSERT INTO deck (fk_card_id, fk_user_id) VALUES ('4', 1);
UPDATE stack SET in_deck = TRUE WHERE fk_card_id = '1' AND fk_user_id = 1;
UPDATE stack SET in_deck = TRUE WHERE fk_card_id = '2' AND fk_user_id = 1;
UPDATE stack SET in_deck = TRUE WHERE fk_card_id = '3' AND fk_user_id = 1;
UPDATE stack SET in_deck = TRUE WHERE fk_card_id = '4' AND fk_user_id = 1;
INSERT INTO deck (fk_card_id, fk_user_id) VALUES ('6', 2);
INSERT INTO deck (fk_card_id, fk_user_id) VALUES ('7', 2);
INSERT INTO deck (fk_card_id, fk_user_id) VALUES ('8', 2);
INSERT INTO deck (fk_card_id, fk_user_id) VALUES ('9', 2);
UPDATE stack SET in_deck = TRUE WHERE fk_card_id = '6' AND fk_user_id = 2;
UPDATE stack SET in_deck = TRUE WHERE fk_card_id = '7' AND fk_user_id = 2;
UPDATE stack SET in_deck = TRUE WHERE fk_card_id = '8' AND fk_user_id = 2;
UPDATE stack SET in_deck = TRUE WHERE fk_card_id = '9' AND fk_user_id = 2;
