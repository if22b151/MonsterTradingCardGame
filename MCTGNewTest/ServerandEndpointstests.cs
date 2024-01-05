using MCTGServer;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGNew;
using MTCGNew.Models;

namespace MCTGNewTest {
    internal class ServerandEndpointstests {
        [SetUp]
        public void Setup() {
        }
        [Test]
        public void TestCheckSession() {
            //Arrange
            SessionHandling.Sessions["test"] = "test";              
            //Act
            bool result = SessionHandling.CheckSession("test");
            bool result2 = SessionHandling.CheckSession("test2");
            //Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(result2, Is.EqualTo(false));
        }
        [Test]
        public void TestHandleRequest() {
            //Arrange
            //irrelevant for this test but needed for instantiation
            StreamReader reader = new StreamReader("../../../test.txt");
            //irrelevant for this test but needed for instantiation
            StreamWriter writer = new StreamWriter("../../../test2txt.txt");
            RequestParser request = new RequestParser(reader);
            HTTPResponder response = new HTTPResponder(writer);
            UsersEndpoint userEndpoint = new UsersEndpoint();
            request.Method = HttpMethods.GET;
            //Act
            bool result = userEndpoint.HandleRequest(request, response);
            //Assert
            Assert.That(result, Is.EqualTo(true));

        }
        [Test]
        public void TestCalculateWinRatio() {
            //Arrange
            UserStats user = new UserStats();
            user.Wins = 2;
            user.Losses = 1;
            //Act
            double result = user.CalculateWinrate();
            //Assert
            Assert.That(result, Is.EqualTo(67));

        }


      
    }
}
