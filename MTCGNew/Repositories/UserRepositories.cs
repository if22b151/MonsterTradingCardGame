using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using MCTGServer;
using MTCGNew.Models;
using Npgsql;

namespace MTCGNew.Repositories {
    internal class UserRepositories : DBConnection {

        public UserRepositories() : base() { }

        internal List<Users> GetUsers() {
            List<Users> users = new List<Users>();
            lock(this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();

                _dbcommand.CommandText = "SELECT * FROM users";
                var reader = _dbcommand.ExecuteReader();
                while (reader.Read()) {
                    users.Add(new Users() {
                        Id = reader.GetInt32(reader.GetOrdinal("user_id")),
                        Username = reader.GetString(reader.GetOrdinal("username")),
                        Password = reader.GetString(reader.GetOrdinal("password")),
                        Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                        Losses = reader.GetInt32(reader.GetOrdinal("losses")),
                        Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                        Coins = reader.GetInt32(reader.GetOrdinal("coins")),
                        Bio = reader.IsDBNull(reader.GetOrdinal("bio")) ? null : reader.GetString(reader.GetOrdinal("bio")),
                        Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString(reader.GetOrdinal("image")),
                        Name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"))

                    });
                }
            
                return users;
            }

        }

        internal Users? GetUser(string username) {
            lock(this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "SELECT * FROM users WHERE username = @username";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                if (reader.Read()) {
                    return new Users() {
                        Id = reader.GetInt32(reader.GetOrdinal("user_id")),
                        Username = reader.GetString(reader.GetOrdinal("username")),
                        Password = reader.GetString(reader.GetOrdinal("password")),
                        Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                        Losses = reader.GetInt32(reader.GetOrdinal("losses")),
                        Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                        Coins = reader.GetInt32(reader.GetOrdinal("coins")),
                        Bio = reader.IsDBNull(reader.GetOrdinal("bio")) ? null : reader.GetString(reader.GetOrdinal("bio")),
                        Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString(reader.GetOrdinal("image")),
                        Name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"))
                    };
                }
                return null;

            }
        }

        internal void EditUser(EditableUserData userdata, string username) {
            lock(this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();

                _dbcommand.CommandText = "SELECT username FROM users WHERE username = @username";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                var db_username = _dbcommand.ExecuteScalar();
                if (db_username == null) {
                    throw new SqlNotFilledException("User not found.");
                }


                _dbcommand.CommandText = "UPDATE users SET bio = @bio, image = @image, name = @name WHERE username = @username";
                AddParameter(_dbcommand, "@bio", value: userdata.Bio, DbType.String);
                AddParameter(_dbcommand, "@image", value: userdata.Image, DbType.String);
                AddParameter(_dbcommand, "@name", value: userdata.Name, DbType.String);
                AddParameter(_dbcommand, "@username", username, DbType.String);
                _dbcommand.ExecuteNonQuery();


            }

        }
        
        internal void CreateUser(Users user) {
            lock (this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "SELECT username FROM users WHERE username = @username";
                AddParameter(_dbcommand, "@username", user.Username, DbType.String);
                var db_username = _dbcommand.ExecuteScalar();
                if (db_username != null) {
                    throw new SqlAlreadyFilledException("User with same username already registered");
                }
                _dbcommand.CommandText = "INSERT INTO users (username, password, wins, losses, elo, coins) VALUES (@username, @password, @wins, @losses, @elo, @coins)";
                AddParameter(_dbcommand, "@username", user.Username, DbType.String);
                AddParameter(_dbcommand, "@password", user.Password, DbType.String);
                AddParameter(_dbcommand, "@wins", user.Wins, DbType.Int32);
                AddParameter(_dbcommand, "@losses", user.Losses, DbType.Int32);
                AddParameter(_dbcommand, "@elo", user.Elo, DbType.Int32);
                AddParameter(_dbcommand, "@coins", user.Coins, DbType.Int32);
                _dbcommand.ExecuteNonQuery();

            }
           
        }

        private void AddParameter(IDbCommand command, string name, object value, DbType dbtype) {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = dbtype;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
 
    }
}
