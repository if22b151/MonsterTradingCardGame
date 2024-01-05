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
                responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
                return;
            }

            PackageRepository packagerepo = new PackageRepository();
            var package = JsonSerializer.Deserialize<List<Card>>(request.Content ?? "");
            if (package == null) {
                responder.SetResponse(400, "Bad Request", "Package was not sent with request!");
                return;
            }
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if (SessionHandling.CheckSession("admin") == false) {
                responder.SetResponse(401, "Unauthorized", "Not logged in!");
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
                    responder.SetResponse(201, "Created", "Package and cards successfully created");
                    return;
                } 
                catch(SqlAlreadyFilledException e) {
                    responder.SetResponse(409, "Conflict", e.Message);
                    return;
                }

                catch(Exception e) {
                    responder.SetResponse(400, "Bad Request", e.Message);
                    return;
                }
            }
            responder.SetResponse(403, "Forbidden", "Provided user is not admin");
            return;
        }
        private void AcquirePackage(RequestParser request, HTTPResponder responder) {
            if (request.Headers["Authorization"] == null) {
                responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
                return;
            }
            string inputString = request.Headers["Authorization"];
            string username = SessionHandling.GetUsername(inputString);
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if (SessionHandling.CheckSession("admin") == false) {
                responder.SetResponse(401, "Unauthorized", "Not logged in!");
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if (rqauthtoken == usersessiontoken ) {
                try {
                    PackageRepository packagerepo = new PackageRepository();
                    packagerepo.AcquirePackage(username);
                    responder.SetResponse(200, "OK", "Package successfully acquired");
                    return;
                }
                catch (SqlNotFilledException e) {
                    responder.SetResponse(404, "Not Found", e.Message);
                    return;
                }

                catch(ArgumentException e) {
                    responder.SetResponse(403, "Forbidden", e.Message);
                    return;
                }
                
                catch (Exception e) {
                    responder.SetResponse(400, "Bad Request", e.Message);
                    return;
                }
            }
            responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
            return;
        }
    } 
}
