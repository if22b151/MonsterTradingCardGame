using MTCGNew.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MTCGNew.Repositories {
    internal class SessionRepository : DBConnection {

        public SessionRepository() : base() { }

        public string? Login(Credentials user) {
            string comparison_username = user.Username;
            string comparison_password = user.Password;
            using IDbConnection _dbconnection = new Npgsql.NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            using var transaction = _dbconnection.BeginTransaction();
            lock(this) {
                _dbcommand.CommandText = "SELECT username FROM users WHERE username = @comparison_username AND password = @comparison_password";
                AddComparisonParameter(comparison_username, _dbcommand, "@comparison_username");
                AddComparisonParameter(comparison_password, _dbcommand, "@comparison_password");
                var reader = _dbcommand.ExecuteReader();
                int usernameOrdinal = reader.GetOrdinal("username");
                string? usernameValue = "";
                while (reader.Read()) {
                    usernameValue = reader.IsDBNull(usernameOrdinal) ? null : reader.GetString(usernameOrdinal);
                    if (usernameValue != null) {
                        return $"Bearer {usernameValue}-mtcgToken";
                    }
                }
                return null;

            }
        }

        private static void AddComparisonParameter(string comparison_param, IDbCommand _dbcommand, string parametername) {
            IDbDataParameter parameter = _dbcommand.CreateParameter();
            parameter.ParameterName = parametername;
            parameter.Value = comparison_param;
            _dbcommand.Parameters.Add(parameter);
        }

    }
}
