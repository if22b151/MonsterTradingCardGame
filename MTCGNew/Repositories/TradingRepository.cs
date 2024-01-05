using MTCGNew.Cards;
using MTCGNew.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Repositories {
    internal class TradingRepository : DBConnection {

        public void CreateTradingDeal(TradingDeal tradingdeal, string username) {
            lock(this) {
                using IDbConnection _dbconnection = new Npgsql.NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();

                _dbcommand.CommandText = "SELECT tradingdeal_id FROM tradingdeals WHERE tradingdeal_id = @tradingdeal_id";
                AddParameter(_dbcommand, "@tradingdeal_id", tradingdeal.Id, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                if(reader.Read()) {
                    reader.Close();
                    throw new ArgumentException("A deal with this deal ID already exists.");
                }
                _dbcommand.Parameters.Clear();  
                reader.Close();

                _dbcommand.CommandText = "INSERT INTO tradingdeals (tradingdeal_id, fk_user_id, fk_card_id, min_damage, card_type) VALUES (@tradingdeal_id, (SELECT user_id FROM users WHERE username = @username), @card_id, @minimum_damage, @card_type)";
                AddParameter(_dbcommand, "@tradingdeal_id", tradingdeal.Id, DbType.String);
                AddParameter(_dbcommand, "@username", username, DbType.String);
                AddParameter(_dbcommand, "@card_id", tradingdeal.CardToTrade, DbType.String);
                AddParameter(_dbcommand, "@minimum_damage", tradingdeal.MinimumDamage, DbType.Double);
                AddParameter(_dbcommand, "@card_type", tradingdeal.Type, DbType.String);
                _dbcommand.ExecuteNonQuery();

            }
        }

        public List<TradingDeal> GetTradingDeals(string username) {
            lock(this) {
                using IDbConnection _dbconnection = new Npgsql.NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "SELECT tradingdeal_id, fk_card_id, card_type, min_damage FROM tradingdeals WHERE fk_user_id != (SELECT user_id FROM users WHERE username = @username)";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                List<TradingDeal> tradingDeals = new List<TradingDeal>();
                while(reader.Read()) {
                    tradingDeals.Add(new TradingDeal() {
                        Id = reader.GetString(reader.GetOrdinal("tradingdeal_id")),
                        CardToTrade = reader.GetString(reader.GetOrdinal("fk_card_id")),
                        Type = reader.GetString(reader.GetOrdinal("card_type")),
                        MinimumDamage = (float)reader.GetDouble(reader.GetOrdinal("min_damage"))
                    });
                }
                return tradingDeals;
            }
        }

        internal void DeleteTradingDeal(string tradingdealid, string username) {
            lock (this) {
                using IDbConnection _dbconnection = new Npgsql.NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "SELECT fk_card_id FROM tradingdeals WHERE tradingdeal_id = @tradingdeal_id";
                AddParameter(_dbcommand, "@tradingdeal_id", tradingdealid, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                if(reader.Read()) {
                    string cardid = reader.GetString(reader.GetOrdinal("fk_card_id"));
                    Stackrepository stackrepository = new Stackrepository();
                    if(!stackrepository.CardbelongstoUserorinDeck(cardid, username)) {
                        reader.Close();
                        throw new ArgumentException("The deal contains a card that is not owned by the user.");
                    }
                    _dbcommand.Parameters.Clear();
                    reader.Close();
                    _dbcommand.CommandText = "DELETE FROM tradingdeals WHERE tradingdeal_id = @tradingdeal_id";
                    AddParameter(_dbcommand, "@tradingdeal_id", tradingdealid, DbType.String);
                    _dbcommand.ExecuteNonQuery();
                    return;
                }
                reader.Close();
                throw new SqlNotFilledException("The provided deal ID was not found.");
                
            }
        }

        internal void TryTrading(string tradingdealid, string cardid, string username) {
            Stackrepository stackrepository = new Stackrepository();
            bool cardbelongstoUserorinDeck = stackrepository.CardbelongstoUserorinDeck(cardid, username);
            lock(this) {
                using IDbConnection _dbconnection = new Npgsql.NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "SELECT users.username, tradingdeals.min_damage, tradingdeals.card_type, tradingdeals.fk_card_id FROM tradingdeals INNER JOIN users ON users.user_id = tradingdeals.fk_user_id WHERE tradingdeals.tradingdeal_id = @tradingdeal_id";
                AddParameter(_dbcommand, "@tradingdeal_id", tradingdealid, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                string tradingdealusername = "";
                float tradingdealmin_damage = 0;
                string tradingdealcard_type = "";
                string tradingdealcard_id = "";
                if(reader.Read()) {
                    tradingdealusername = reader.GetString(reader.GetOrdinal("username"));
                    tradingdealmin_damage = (float)reader.GetDouble(reader.GetOrdinal("min_damage"));
                    tradingdealcard_type = reader.GetString(reader.GetOrdinal("card_type"));
                    tradingdealcard_id = reader.GetString(reader.GetOrdinal("fk_card_id"));
                    reader.Close();
                    Card card = stackrepository.GetCard(username, cardid);
                    card.SetCardType();
                    if (!cardbelongstoUserorinDeck || tradingdealusername == username || card.Cardtype.ToString() != tradingdealcard_type || card.Damage < tradingdealmin_damage) {
                        throw new ArgumentException("The offered card is not owned by the user, or the requirements are not met (Type, MinimumDamage), or the offered card is locked in the deck, or the user tries to trade with self");
                    }
                    _dbcommand.CommandText = "UPDATE stack SET fk_user_id = (SELECT user_id FROM users WHERE username = @tradingdealusername) WHERE fk_card_id = @cardid; UPDATE stack SET fk_user_id = (SELECT user_id FROM users WHERE username = @username) WHERE fk_card_id = @tradingdealcard_id; DELETE FROM tradingdeals WHERE tradingdeal_id = @tradingdealid";
                    AddParameter(_dbcommand, "@tradingdealusername", tradingdealusername, DbType.String);
                    AddParameter(_dbcommand, "@cardid", cardid, DbType.String);
                    AddParameter(_dbcommand, "@username", username, DbType.String);
                    AddParameter(_dbcommand, "@tradingdealcard_id", tradingdealcard_id, DbType.String);
                    AddParameter(_dbcommand, "@tradingdealid", tradingdealid, DbType.String);
                    _dbcommand.ExecuteNonQuery();
                } else {
                    throw new SqlNotFilledException("The provided deal ID was not found.");

                }
            }

        }
    }
}
