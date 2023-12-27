using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using MTCGNew.Cards;
using Npgsql;
using Npgsql.PostgresTypes;

namespace MTCGNew.Repositories {
    internal class PackageRepository : DBConnection {
        public PackageRepository() : base() { }

        public void CreatePackage(List<Card> package) {
            int maxpackageid = 0;
            lock(this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                using var transaction = _dbconnection.BeginTransaction(); 


                _dbcommand.CommandText = "SELECT MAX(cards_in_package_id) AS max_cards_in_package_id FROM packages";
                var reader = _dbcommand.ExecuteReader();
                if(reader.Read()) {
                    maxpackageid = reader.IsDBNull(reader.GetOrdinal("max_cards_in_package_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("max_cards_in_package_id"));
                }
                else {
                    throw new Exception("Could not get max package id!");
                }
                reader.Close();

            }

            lock(this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                using var transaction = _dbconnection.BeginTransaction();

                foreach (Card card in package) {
                    _dbcommand.CommandText = "INSERT INTO packages (cards_in_package_id, fk_card_id) VALUES (@cards_in_package_id, @card_id)";
                    AddParameter(maxpackageid + 1, _dbcommand, "@cards_in_package_id", DbType.Int32);
                    AddParameter(card.Id, _dbcommand, "@card_id", DbType.String);
                    _dbcommand.ExecuteNonQuery();
                    _dbcommand.Parameters.Clear();
                }
                transaction.Commit();

            }

        }

        public void AcquirePackage(string username) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            using var transaction = _dbconnection.BeginTransaction();

            _dbcommand.CommandText = "SELECT count(*) FROM (SELECT package_id FROM packages LIMIT 1)";
            var reader = _dbcommand.ExecuteReader();
            if(reader.Read()) {
                if(reader.GetInt32(0) == 0) {
                    throw new SqlNotFilledException("No card package available for buying");
                }
            }
            reader.Close();

            _dbcommand.CommandText = "SELECT coins FROM users WHERE username = @username";
            AddParameter(username, _dbcommand, "@username", DbType.String);
            reader = _dbcommand.ExecuteReader();
            if(reader.Read()) {
                if(reader.GetInt32(0) < 5) {
                    throw new ArgumentException("Not enough money for buying a card package");
                }
            }
            _dbcommand.Parameters.Clear();
            reader.Close();

            // Get the cards with the highest cards_in_package_id and insert them into stack table for the user fk_user_id is the user_id of the user who bought the package
            _dbcommand.CommandText = "INSERT INTO stack (fk_user_id, fk_card_id) SELECT (SELECT user_id FROM users WHERE username = @username), fk_card_id FROM packages WHERE cards_in_package_id = (SELECT MIN(cards_in_package_id) FROM packages)";
            AddParameter(username, _dbcommand, "@username", DbType.String);
            _dbcommand.ExecuteNonQuery();
            _dbcommand.Parameters.Clear();

            // Update the coins of the user who bought the package
            _dbcommand.CommandText = "UPDATE users SET coins = coins - 5 WHERE username = @username";
            AddParameter(username, _dbcommand, "@username", DbType.String);
            _dbcommand.ExecuteNonQuery();
            _dbcommand.Parameters.Clear();

            // Delete the cards with the highest cards_in_package_id from the packages table
            _dbcommand.CommandText = "DELETE FROM packages WHERE cards_in_package_id = (SELECT MIN(cards_in_package_id) FROM packages)";
            _dbcommand.ExecuteNonQuery();


            transaction.Commit();
            
        }

        private static void AddParameter<T>(T queryparameter, IDbCommand _dbcommand, string parametername, DbType type) {
            IDbDataParameter parameter = _dbcommand.CreateParameter();
            parameter.ParameterName = parametername;
            parameter.Value = queryparameter;
            parameter.DbType = type;
            _dbcommand.Parameters.Add(parameter);
        }
        
    }
}
