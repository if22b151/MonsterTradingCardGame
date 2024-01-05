using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MCTGServer;
using MTCGNew.Repositories;
using MTCGNew.Models;
using System.Data.SqlTypes;

namespace MTCGNew {
    public class UsersEndpoint : IHTTPEndpoint {
        
        public bool HandleRequest(RequestParser request, HTTPResponder response) {

            if(request.Method == HttpMethods.GET) {
               if(request.Path.Count() == 3) {
                    GetUser(request, response);
                    return true;
               }
            }
            else if(request.Method == HttpMethods.POST) {
                CreateUser(request, response);
                return true;
            } 
            else if(request.Method == HttpMethods.PUT) {
                EditUser(request, response);
                return true;
            }
            
            return false;
        }

        public void GetUser(RequestParser request, HTTPResponder responder) {
            if (request.Headers["Authorization"] == null) {
                responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
                return;
            }
            string inputString = request.Headers["Authorization"];
            string username = SessionHandling.GetUsername(inputString);
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if (SessionHandling.CheckSession(username) == false) {
                responder.SetResponse(401, "Unauthorized", "Not logged in!");
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if(rqauthtoken == usersessiontoken) {
                UserRepository userRepository = new UserRepository();
                var user = userRepository.GetUser(username);
                if (user is null) {
                    responder.SetResponse(404, "Not Found", "User not found");
                    return;
                }
                responder.SetResponse(200, "OK", JsonSerializer.Serialize(user), "Content-Type", "application/json");
                return;
            }
            responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
            return;
        }

        public void EditUser(RequestParser request, HTTPResponder responder) {
            if (request.Headers["Authorization"] == null) {
                responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");   
                return;
            }   
            string inputString = request.Headers["Authorization"];
            string username = SessionHandling.GetUsername(inputString);
            int index = request.Headers["Authorization"].IndexOf(" ");
            string rqauthtoken = request.Headers["Authorization"][(index + 1)..];
            if (SessionHandling.CheckSession(username) == false) {
                responder.SetResponse(401, "Unauthorized", "Not logged in!");
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if (rqauthtoken == usersessiontoken) {
                UserRepository userRepository = new UserRepository();
                var userdata = JsonSerializer.Deserialize<UserData>(request.Content ?? "");
                if (userdata is null) {
                    responder.SetResponse(400, "Bad Request", "User was not sent with request!");
                    return;
                }
                try {
                    userRepository.EditUser(userdata, username);
                    responder.SetResponse(200, "OK", "User sucessfully updated.");
                    return;

                } 
                catch(SqlNotFilledException e) {
                    responder.SetResponse(404, "Not Found", e.Message);
                    return;
                }
                catch(Exception e) {
                    responder.SetResponse(400, "Bad Request", e.Message);
                    return;
                }
            }
            responder.SetResponse(401, "Unauthorized", "Access token is missing or invalid");
            return;
         
        }
        

        public void CreateUser(RequestParser request, HTTPResponder response) {
            UserRepository userRepository = new UserRepository();
            try {
                var user = JsonSerializer.Deserialize<Credentials>(request.Content ?? "");
                if (user is null) {
                    throw new ArgumentNullException("User was not sent with request!");
                }

                userRepository.CreateUser(user);
                response.SetResponse(201, "OK", "User successfully created");
                return;
            }

            catch(ArgumentNullException e) {
                response.SetResponse(400, "Bad Request", e.Message);
                return;
            }

            catch(SqlAlreadyFilledException e) {
                response.SetResponse(409, "Conflict", e.Message);
                return;
            }

            catch(Exception) {
                response.SetResponse(400, "Bad Request", "User couldnt be created");
                return;
            }
        }
    }
}
