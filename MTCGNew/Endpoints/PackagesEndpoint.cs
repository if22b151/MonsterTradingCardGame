using MCTGServer;
using MTCGNew.Cards;
using MTCGNew.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCGNew.Endpoints {
    internal class PackagesEndpoint : IHTTPEndpoint {

        public bool HandleRequest(RequestParser request, HTTPResponder response) {
            if (request.Method == HttpMethods.POST) {
                if(request.Path.Count() == 2) {
                    CreatePackage(request, response);
                    return true;    

                } else if(request.Path.Count() == 3) {
                    AcquirePackage(request, response);
                    return true;
                }
            }
            return false;
        }

        private void CreatePackage(RequestParser request, HTTPResponder responder) {
            if (request.Headers["Authorization"] == null) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Access token is missing or invalid";
                return;
            }

            PackageRepository packagerepo = new PackageRepository();
            var package = JsonSerializer.Deserialize<List<Card>>(request.Content ?? "");
            if (package == null) {
                responder.ReturnCode = 400;
                responder.ReturnText = "Bad Request";
                responder.Content = "Package was not sent with request!";
                return;
            }
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if (SessionHandling.CheckSession("admin") == false) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Not logged in!";
                return;
            }
            string adminsessiontoken = SessionHandling.Sessions["admin"];
            Console.WriteLine($"{rqauthtoken}, {adminsessiontoken}");
            if (rqauthtoken == adminsessiontoken) {
                try {
                    CardRepository cardRepository = new CardRepository();
                    lock(this) {
                        cardRepository.CreateCard(package);
                        
                    }
                    packagerepo.CreatePackage(package);
                    responder.ReturnCode = 200;
                    responder.ReturnText = "OK";
                    responder.Content = "Package and cards successfully created";
                    return;
                } 
                catch(SqlAlreadyFilledException e) {
                    responder.ReturnCode = 409;
                    responder.ReturnText = "Conflict";
                    responder.Content = e.Message;
                    return;
                }

                catch(Exception e) {
                    responder.ReturnCode = 400;
                    responder.ReturnText = "Bad Request";
                    responder.Content = e.Message;
                    return;
                }
            }
            responder.ReturnCode = 403;
            responder.ReturnText = "Forbidden";
            responder.Content = "Provided user is not admin";
        }
        private void AcquirePackage(RequestParser request, HTTPResponder responder) {
            if (request.Headers["Authorization"] == null) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Access token is missing or invalid";
                return;
            }
            string inputString = request.Headers["Authorization"];
            string username = SessionHandling.GetUsername(inputString);
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if (SessionHandling.CheckSession("admin") == false) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Not logged in!";
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if (rqauthtoken == usersessiontoken ) {
                try {
                    PackageRepository packagerepo = new PackageRepository();
                    packagerepo.AcquirePackage(username);
                    responder.ReturnCode = 200;
                    responder.ReturnText = "OK";
                    responder.Content = "A package has been successfully bought";
                    return;
                }
                catch (SqlNotFilledException e) {
                    responder.ReturnCode = 404;
                    responder.ReturnText = "Not Found";
                    responder.Content = e.Message;
                    return;
                }

                catch(ArgumentException e) {
                    responder.ReturnCode = 403;
                    responder.ReturnText = "Forbidden";
                    responder.Content = e.Message;
                    return;
                }
                
                catch (Exception e) {
                    responder.ReturnCode = 400;
                    responder.ReturnText = "Bad Request";
                    responder.Content = e.Message;
                    return;
                }
            }
            responder.ReturnCode = 401;
            responder.ReturnText = "Unauthorized";
            responder.Content = "Access token is missing or invalid";
        }
    } 
}
