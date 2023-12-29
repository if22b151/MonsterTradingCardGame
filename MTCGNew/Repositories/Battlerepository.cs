﻿using MTCGNew.Cards;
using MTCGNew.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MTCGNew.Repositories {
    internal class Battlerepository : DBConnection {

        public List<UserStats> GetScoreboard() {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            _dbcommand.CommandText = "SELECT name, elo, wins, losses FROM users ORDER BY elo DESC";
            var reader = _dbcommand.ExecuteReader();
            List<UserStats> scoreboard = new List<UserStats>();
            while (reader.Read()) {
                if(reader.IsDBNull(reader.GetOrdinal("name"))) {
                    continue;
                }
                scoreboard.Add(new UserStats() {
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                    Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                    Losses = reader.GetInt32(reader.GetOrdinal("losses"))
                });
            }
            return scoreboard;
        }
        public UserStats? GetStats(string username) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            _dbcommand.CommandText = "SELECT name, elo, wins, losses FROM users WHERE username = @username";
            AddParameter(_dbcommand, "@username", username, DbType.String);
            var reader = _dbcommand.ExecuteReader();
            if (reader.Read()) {
                return new UserStats() {
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                    Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                    Losses = reader.GetInt32(reader.GetOrdinal("losses"))
                };
            }
          
            return null;

        }

        public Player? BattlePrep(Deckcards deck, string username) {
            lock(this) {
                Player player = new Player();
                using IDbConnection _dbconnection = new NpgsqlConnection(_connection);
                using IDbCommand _dbcommand = _dbconnection.CreateCommand();
                _dbconnection.Open();
                foreach(Card card in deck.Deck) {
                    _dbcommand.CommandText = "SELECT card_id, damage, name FROM cards WHERE card_id IN (SELECT fk_card_id FROM deck WHERE fk_user_id = (SELECT user_id FROM users WHERE username = @username))";
                    AddParameter(_dbcommand, "@username", username, DbType.String);
                    var reader = _dbcommand.ExecuteReader();
                    if(reader.Read()) {
                        player.Deck.Add(new Card() {
                            Id = reader.GetString(reader.GetOrdinal("card_id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Damage = (float)reader.GetDouble(reader.GetOrdinal("damage"))
                        });
                    }

                }
                return player;
            }

        }


        private void AddParameter(IDbCommand dbcommand, string parametername, string username, DbType type) {
            var parameter = dbcommand.CreateParameter();
            parameter.ParameterName = parametername;
            parameter.Value = username;
            parameter.DbType = type;
            dbcommand.Parameters.Add(parameter);
        }
    }
}