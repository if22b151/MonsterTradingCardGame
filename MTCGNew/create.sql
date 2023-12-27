DROP TABLE cards_table;
DROP TABLE deck;
DROP TABLE stack;
DROP TABLE packages;
DROP TABLE cards;

ALTER TABLE users
ADD COLUMN Bio varchar(50),
ADD COLUMN Image varchar(50),
ADD COLUMN Name varchar(50);

-- postgresql
CREATE TABLE cards (
	c_id SERIAL PRIMARY KEY,
	card_id varchar(50) UNIQUE,
	Name varchar(50),
	Damage float
);

CREATE TABLE packages (
	package_id SERIAL PRIMARY KEY,
	cards_in_package_id integer,
	fk_card_id varchar(50), 
	FOREIGN KEY (fk_card_id) REFERENCES cards(card_id)
);

CREATE TABLE stack (
	stack_id SERIAL PRIMARY KEY,
	in_deck BOOLEAN NOT NULL DEFAULT FALSE,
	fk_card_id varchar(50),
	fk_user_id integer,
	FOREIGN KEY (fk_card_id ) REFERENCES cards(card_id),
	FOREIGN KEY (fk_user_id) REFERENCES users(user_id)
);

CREATE TABLE deck (
	deck_id SERIAL PRIMARY KEY,
	fk_card_id  varchar(50),
	fk_user_id integer,
	fk_stack_id integer,
	FOREIGN KEY (fk_card_id) REFERENCES cards(card_id),
	FOREIGN KEY (fk_user_id) REFERENCES users(user_id),
	FOREIGN KEY (fk_stack_id) REFERENCES stack(stack_id)
);

TRUNCATE cards CASCADE;
TRUNCATE packages CASCADE;
TRUNCATE users CASCADE;
TRUNCATE stack CASCADE;
TRUNCATE deck CASCADE;