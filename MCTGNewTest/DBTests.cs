using MTCGNew.Cards;
using MTCGNew.Models;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Net;


namespace MCTGNewTest {
    internal class DBTests {
        private const string _connectionstring = "Host = 127.0.0.1; Database = unittests; Username = MCTG; Password = MonstercardTradinggame";
        private readonly string createscriptpath = "../../../../MTCGNew/Sqlscripts/create.sql";
        private readonly string dropscriptpath = "../../../../MTCGNew/Sqlscripts/drop.sql";
        private readonly string insertscriptpath = "../../../../MTCGNew/Sqlscripts/insert.sql";


        [SetUp]
        public void Setup() {
            using NpgsqlConnection _dbconnection = new NpgsqlConnection(_connectionstring);
            _dbconnection.Open();
            string createscript = File.ReadAllText(createscriptpath);
            using (NpgsqlCommand _dbcommand = new NpgsqlCommand(createscript, _dbconnection)) {
                _dbcommand.ExecuteNonQuery();
            }
            string insertscript = File.ReadAllText(insertscriptpath);
            using (NpgsqlCommand _dbcommand = new NpgsqlCommand(insertscript, _dbconnection)) {
                _dbcommand.ExecuteNonQuery();       
            }
        }
        [TearDown]
        public void TearDown() {
            using NpgsqlConnection _dbconnection = new NpgsqlConnection(_connectionstring);
            _dbconnection.Open();
            string dropscript = File.ReadAllText(dropscriptpath);
            using NpgsqlCommand _dbcommand = new NpgsqlCommand(dropscript, _dbconnection);
            _dbcommand.ExecuteNonQuery();
            _dbcommand.Dispose();
        }

        [Test]
        public void TestGetUser() {
            //Arrange
            UserData? user;
            UserData? failuser;
            //Act
            user = GetUser("testuser");
            failuser = GetUser("fail");
            //Assert
            Assert.That(user.Name, Is.EqualTo("name"));
            Assert.That(failuser, Is.EqualTo(null));
        }

        [Test]
        public void TestGetCards() {
            //Arrange
            Stackcards? stack;
            Stackcards? emptystack;
            //Act
            stack = GetCards("testuser");
            emptystack = GetCards("testuser3");
            //Assert
            Assert.That(stack.Stack.Count, Is.EqualTo(5));
            Assert.That(emptystack.Stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestGetScoreboard() {
            //Arrange
            List<UserStats> scoreboard;
            //Act
            scoreboard = GetScoreboard();
            //Assert
            Assert.That(scoreboard.Count, Is.EqualTo(3));
            Assert.That(scoreboard[1].Name, Is.EqualTo("name2"));
        }

        [Test]
        public void TestCreateUser() {
            //Arrange
            Credentials user = new Credentials() {
                Username = "testuser4",
                Password = "testpassword"
            };
            Credentials failuser = new Credentials() {
                Username = "testuser",
                Password = "testpassword"
            };
            //Act
            try {
                CreateUser(user);
                CreateUser(failuser);

            }
            catch(SqlAlreadyFilledException e) {
                //Assert
                Assert.That(e.Message, Is.EqualTo("User with same username already registered"));
                return;
            }
            //Assert
            Assert.That(GetUser("testuser4").Name, Is.EqualTo("testuser4"));
        }

        [Test]
        public void TestBattlePrep() {
            //Arrange
            Player? player1 = new Player();
            Deckcards deck = new Deckcards();           
            player1.Name = "testuser";
            FillDeck(deck);
            //Act
            player1 = BattlePrep(deck, player1.Name);
            //Assert
            Assert.That(player1.Deck.Count, Is.EqualTo(4));
            Assert.That(player1.HasWon, Is.EqualTo(false));
        }

        private void FillDeck(Deckcards deck) {
            Card card1 = new Card() {
                Id = "1",
                Name = "testcard1",
                Damage = 10
            };
            Card card2 = new Card() {
                Id = "2",
                Name = "testcard2",
                Damage = 20
            };
            Card card3 = new Card() {
                Id = "3",
                Name = "testcard3",
                Damage = 30
            };
            Card card4 = new Card() {
                Id = "4",
                Name = "testcard4",
                Damage = 40
            };
            deck.Deck.Add(card1);
            deck.Deck.Add(card2);
            deck.Deck.Add(card3);
            deck.Deck.Add(card4);
        }
        private UserData? GetUser(string username) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connectionstring);
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
            reader.Close();
            _dbcommand.Parameters.Clear();
            return null;   
        }

        private Stackcards GetCards(string username) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connectionstring);
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
            reader.Close();
            _dbcommand.Parameters.Clear();
            return stack;
            
        }

        private List<UserStats> GetScoreboard() {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connectionstring);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            _dbcommand.CommandText = "SELECT name, elo, wins, losses FROM users ORDER BY elo DESC";
            var reader = _dbcommand.ExecuteReader();
            List<UserStats> scoreboard = new List<UserStats>();
            while (reader.Read()) {
                if (reader.IsDBNull(reader.GetOrdinal("name"))) {
                    continue;
                }
                scoreboard.Add(new UserStats() {
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                    Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                    Losses = reader.GetInt32(reader.GetOrdinal("losses"))
                });
            }
            reader.Close();
            _dbcommand.Parameters.Clear();
            return scoreboard;
        }

        private void CreateUser(Credentials user) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connectionstring);
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
            _dbcommand.Parameters.Clear();

        }

        private Player? BattlePrep(Deckcards deck, string username) {
            using IDbConnection _dbconnection = new NpgsqlConnection(_connectionstring);
            using IDbCommand _dbcommand = _dbconnection.CreateCommand();
            _dbconnection.Open();
            Player player = new Player();
            foreach (Card card in deck.Deck) {
                _dbcommand.CommandText = "SELECT card_id, damage, name FROM cards WHERE card_id IN (SELECT fk_card_id FROM deck WHERE fk_user_id = (SELECT user_id FROM users WHERE username = @username))";
                AddParameter(_dbcommand, "@username", username, DbType.String);
                var reader = _dbcommand.ExecuteReader();
                if (reader.Read()) {
                    player.Deck.Add(new Card() {
                        Id = reader.GetString(reader.GetOrdinal("card_id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Damage = (float)reader.GetDouble(reader.GetOrdinal("damage"))
                    });

                }
                reader.Close();
                _dbcommand.Parameters.Clear();

                }
            return player;
            

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
