using MTCGNew.Cards;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data.SqlTypes;
using Npgsql.PostgresTypes;

namespace MTCGNew.Repositories {
    internal class CardRepository : DBConnection {

        public CardRepository() : base() { }

        private void CardExists(List<Card> package) {
            
            using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            using var transaction = _dbconnection.BeginTransaction();
            foreach (Card card in package) {
                _dbcommand.CommandText = "SELECT card_id FROM cards WHERE card_id = @card_id";
                AddParameter(_dbcommand, "@card_id", card.Id, DbType.String);
                var id = _dbcommand.ExecuteScalar();
                if (id != null) {
                    throw new SqlAlreadyFilledException("At least one card in the packages already exists");
                }
                _dbcommand.Parameters.Clear();
            }
        }


        public void CreateCard(List<Card> package) {
            CardExists(package);
            using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            using var transaction = _dbconnection.BeginTransaction();

            foreach(Card card in package) {
                _dbcommand.CommandText = "INSERT INTO cards (card_id, name, damage) VALUES (@card_id, @name, @damage)";
                AddParameter(_dbcommand, "@card_id", card.Id, DbType.String);
                AddParameter(_dbcommand, "@name", card.Name, DbType.String);
                AddParameter(_dbcommand, "@damage", card.Damage, DbType.Double);
                _dbcommand.ExecuteNonQuery();
                _dbcommand.Parameters.Clear();
            }
            
            transaction.Commit();

            
        }

    }
}
