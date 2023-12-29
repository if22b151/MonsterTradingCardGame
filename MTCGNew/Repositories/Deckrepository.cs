using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MTCGNew.Cards;
using MTCGNew.Models;
using Npgsql;

namespace MTCGNew.Repositories {
    internal class Deckrepository : DBConnection {

        public Deckcards? GetCards(string username) {

            lock(this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "SELECT cards.card_id, cards.name, cards.damage FROM cards INNER JOIN deck ON cards.card_id = deck.fk_card_id INNER JOIN users ON users.user_id = deck.fk_user_id WHERE users.username = @username";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                Deckcards deck = new Deckcards();
                if(reader.Read()) {
                    while (reader.Read()) {
                        deck.Deck.Add(new Card() {
                            Id = reader.GetString(reader.GetOrdinal("card_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Damage = (float)reader.GetDouble(reader.GetOrdinal("damage"))
                        });
                    }
                } else {
                    return null;
                }
                return deck;

            }

        }

        private bool CardsbelongtoUser(List<string> deck, string username) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            foreach(string Id in deck) {
                _dbcommand.CommandText = "SELECT stack.in_deck FROM stack INNER JOIN users ON users.user_id = stack.fk_user_id WHERE users.username = @username AND stack.fk_card_id = @card_id";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                AddParameter(_dbcommand, "@card_id", Id, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                if(reader.Read()) {
                    if(reader.GetBoolean(reader.GetOrdinal("in_deck")) == false) {
                        return true;
                    }
                } else {
                    return false;
                }
                _dbcommand.Parameters.Clear();
            }

            return false;

        }


        public void EditDeck(List<string> deck, string username) {
            lock(this) {
                if(!CardsbelongtoUser(deck, username)) {
                    throw new ArgumentException("At least one of the provided cards does not belong to the user or is not available");
                }

                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "DELETE FROM deck WHERE fk_user_id = (SELECT user_id FROM users WHERE username = @username)";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                _dbcommand.ExecuteNonQuery();
                _dbcommand.Parameters.Clear();
                foreach(string Id in deck) {
                    _dbcommand.CommandText = "INSERT INTO deck (fk_user_id, fk_card_id) VALUES ((SELECT user_id FROM users WHERE username = @username), @card_id)";
                    AddParameter(_dbcommand, "@username", username, DbType.String);
                    AddParameter(_dbcommand, "@card_id", Id, DbType.String);
                    _dbcommand.ExecuteNonQuery();
                    _dbcommand.Parameters.Clear();
                }

                _dbcommand.CommandText = "UPDATE stack SET in_deck = true WHERE fk_user_id = (SELECT user_id FROM users WHERE username = @username)";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                _dbcommand.ExecuteNonQuery();
                _dbcommand.Parameters.Clear();


            }
  
        }

        private void AddParameter(IDbCommand dbcommand, string parametername, string value, DbType type) {
            IDbDataParameter parameter = dbcommand.CreateParameter();
            parameter.ParameterName = parametername;
            parameter.Value = value;
            parameter.DbType = type;
            dbcommand.Parameters.Add(parameter);
        }
    }
}
