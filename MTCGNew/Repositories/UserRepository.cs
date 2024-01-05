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
    internal class UserRepository : DBConnection {

        public UserRepository() : base() { }

        internal UserData? GetUser(string username) {
            lock (this) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "SELECT name, bio, image FROM users WHERE username = @username";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                if (reader.Read()) {
                    return new UserData() {
                        Bio = reader.IsDBNull(reader.GetOrdinal("bio")) ? null : reader.GetString(reader.GetOrdinal("bio")),
                        Image = reader.IsDBNull(reader.GetOrdinal("image")) ? null : reader.GetString(reader.GetOrdinal("image")),
                        Name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"))
                    };
                }
                return null;

            }
        }

        internal void EditUser(UserData userdata, string username) {
            lock (this) {
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
#pragma warning disable CS8604 // Possible null reference argument.
                AddParameter(_dbcommand, "@bio", value: userdata.Bio, DbType.String);
                AddParameter(_dbcommand, "@image", value: userdata.Image, DbType.String);
                AddParameter(_dbcommand, "@name", value: userdata.Name, DbType.String);
#pragma warning restore CS8604 // Possible null reference argument.
                AddParameter(_dbcommand, "@username", username, DbType.String);
                _dbcommand.ExecuteNonQuery();


            }

        }

        internal void CreateUser(Credentials user) {
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
                _dbcommand.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password)";
                AddParameter(_dbcommand, "@username", user.Username, DbType.String);
                AddParameter(_dbcommand, "@password", user.Password, DbType.String);
                _dbcommand.ExecuteNonQuery();

            }

        }

        public void UpdateUserStats(Player player1, Player player2) {
            if (player1.HasWon) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "UPDATE users SET wins = wins + 1, elo = elo + 5 WHERE username = @username";
                AddParameter(_dbcommand, "@username", player1.Name, DbType.String);
                _dbcommand.ExecuteNonQuery();
                _dbcommand.Parameters.Clear();
                _dbcommand.CommandText = "UPDATE users SET losses = losses + 1, elo = elo - 3 WHERE username = @username";
                AddParameter(_dbcommand, "@username", player2.Name, DbType.String);
                _dbcommand.ExecuteNonQuery();
                _dbcommand.Parameters.Clear();
            } else if (player2.HasWon) {
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                _dbcommand.CommandText = "UPDATE users SET wins = wins + 1, elo = elo + 5 WHERE username = @username";
                AddParameter(_dbcommand, "@username", player2.Name, DbType.String);
                _dbcommand.ExecuteNonQuery();
                _dbcommand.Parameters.Clear();
                _dbcommand.CommandText = "UPDATE users SET losses = losses + 1, elo = elo - 3 WHERE username = @username";
                AddParameter(_dbcommand, "@username", player1.Name, DbType.String);
                _dbcommand.ExecuteNonQuery();
                _dbcommand.Parameters.Clear();
            } 
        }

    }
}
