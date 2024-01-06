using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGNew.Cards;
using MTCGNew.Models;
using Npgsql;


namespace MTCGNew.Repositories {
    internal class Stackrepository : DBConnection {

        public Stackcards GetCards(string username) {
            lock(this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "SELECT cards.card_id, cards.name, cards.damage FROM cards INNER JOIN stack ON cards.card_id = stack.fk_card_id INNER JOIN users ON users.user_id = stack.fk_user_id WHERE users.username = @username";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                Stackcards stack = new Stackcards();
                    while (reader.Read()) {
                        stack.Stack.Add(new Card() {
                            Id = reader.GetString(reader.GetOrdinal("card_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Damage = (float)reader.GetDouble(reader.GetOrdinal("damage"))
                        });
                    }
                
                return stack;

            }
        }

        public Card GetCard(string username, string cardid) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            _dbcommand.CommandText = "SELECT cards.card_id, cards.name, cards.damage FROM cards INNER JOIN stack ON cards.card_id = stack.fk_card_id INNER JOIN users ON users.user_id = stack.fk_user_id WHERE users.username = @username AND cards.card_id = @card_id";
            AddParameter(_dbcommand, "@username", username, DbType.String);
            AddParameter(_dbcommand, "@card_id", cardid, DbType.String);
            var reader = _dbcommand.ExecuteReader();
            Card card = new Card();
            if(reader.Read()) {
                card.Id = reader.GetString(reader.GetOrdinal("card_id"));
                card.Name = reader.GetString(reader.GetOrdinal("name"));
                card.Damage = (float)reader.GetDouble(reader.GetOrdinal("damage"));
            }
            return card;
        }

        public bool CardbelongstoUserorinDeck(string cardid, string username) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            _dbcommand.CommandText = "SELECT stack.in_deck FROM stack INNER JOIN users ON users.user_id = stack.fk_user_id WHERE users.username = @username AND stack.fk_card_id = @card_id";
            AddParameter(_dbcommand, "@username", username, DbType.String);
            AddParameter(_dbcommand, "@card_id", cardid, DbType.String);
            var reader = _dbcommand.ExecuteReader();
            if(reader.Read()) {
                if(reader.GetBoolean(reader.GetOrdinal("in_deck")) == false) {
                    reader.Close(); 
                    return true;
                }
            } else {
                reader.Close();
                return false;
            }
            return false;
            
        }

        public void ChangeOwnerofCards(string winnerusername, string looserusername, List<Card> deck) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            foreach(Card card in deck) {
                _dbcommand.CommandText = "UPDATE stack SET fk_user_id = (SELECT user_id FROM users WHERE username = @winner) WHERE fk_card_id = @cardid AND fk_user_id = (SELECT user_id FROM users WHERE username = @looser)";
                AddParameter(_dbcommand, "@winner", winnerusername, DbType.String);
                AddParameter(_dbcommand, "@looser", looserusername, DbType.String);
                AddParameter(_dbcommand, "@cardid", card.Id, DbType.String);
                _dbcommand.ExecuteNonQuery();
                _dbcommand.Parameters.Clear();
            }
            
        }
    }
}
