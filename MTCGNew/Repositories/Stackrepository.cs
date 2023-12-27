using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGNew.Cards;
using Npgsql;

namespace MTCGNew.Repositories {
    internal class Stackrepository : DBConnection {

        public StackCards? GetCards(string username) {
            lock(this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "SELECT cards.card_id, cards.name, cards.damage FROM cards INNER JOIN stack ON cards.card_id = stack.fk_card_id INNER JOIN users ON users.user_id = stack.fk_user_id WHERE users.username = @username";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                StackCards stack = new StackCards();
                if(reader.Read()) {
                    while (reader.Read()) {
                        stack.Stack.Add(new Card() {
                            Id = reader.GetString(reader.GetOrdinal("card_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Damage = (float)reader.GetDouble(reader.GetOrdinal("damage"))
                        });
                    }
                } else {
                    return null;
                }
                return stack;

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
