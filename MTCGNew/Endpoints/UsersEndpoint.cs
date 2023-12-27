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
    internal class UsersEndpoint : IHTTPEndpoint {
        
        public bool HandleRequest(RequestParser request, HTTPResponder response) {

            if(request.Method == HttpMethods.GET) {
                if(request.Path.Count() == 2) {
                    GetUsers(request, response);
                    return true;
                } else if(request.Path.Count() == 3) {
                    GetUser(request, response);
                    return true;
                }
                return true;
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

        public void GetUsers(RequestParser request, HTTPResponder response) {
            UserRepositories userRepository = new UserRepositories();
            var userlist = userRepository.GetUsers();
            if(userlist.Count == 0) {
                response.ReturnCode = 404;
                response.ReturnText = "Not Found";
                response.Content = "No users";
                return;
            }

            response.Content = JsonSerializer.Serialize(userlist);
            response.Headers.Add("Content-Type", "application/json");
        }

        public void GetUser(RequestParser request, HTTPResponder responder) {
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
            if (SessionHandling.CheckSession(username) == false) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Not logged in!";
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if(rqauthtoken == usersessiontoken) {
                UserRepositories userRepository = new UserRepositories();
                var user = userRepository.GetUser(username);
                if (user is null) {
                    responder.ReturnCode = 404;
                    responder.ReturnText = "Not Found";
                    responder.Content = "User not found";
                    return;
                }
                responder.ReturnCode = 200;
                responder.ReturnText = "OK";
                responder.Content = JsonSerializer.Serialize(user);
                responder.Headers.Add("Content-Type", "application/json");
                return;
            }
            responder.ReturnCode = 401;
            responder.ReturnText = "Unauthorized";
            responder.Content = "Access token is missing or invalid";
        }

        public void EditUser(RequestParser request, HTTPResponder responder) {
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
            if (SessionHandling.CheckSession(username) == false) {
                responder.ReturnCode = 401;
                responder.ReturnText = "Unauthorized";
                responder.Content = "Not logged in!";
                return;
            }
            string usersessiontoken = SessionHandling.Sessions[username];
            if (rqauthtoken == usersessiontoken) {
                UserRepositories userRepository = new UserRepositories();
                var userdata = JsonSerializer.Deserialize<EditableUserData>(request.Content ?? "");
                if (userdata is null) {
                    responder.ReturnCode = 400;
                    responder.ReturnText = "Bad Request";
                    responder.Content = "User was not sent with request!";
                    return;
                }
                try {
                    userRepository.EditUser(userdata, username);
                    responder.ReturnCode = 200;
                    responder.ReturnText = "OK";
                    responder.Content = "User sucessfully updated.";
                    return;

                } 
                catch(SqlNotFilledException e) {
                    responder.ReturnCode = 404;
                    responder.ReturnText = "Not Found";
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
            responder.ReturnCode = 401;
            responder.ReturnText = "Unauthorized";
            responder.Content = "Access token is missing or invalid";
            return;
         
        }
        

        public void CreateUser(RequestParser request, HTTPResponder response) {
            UserRepositories userRepository = new UserRepositories();
            try {
                var user = JsonSerializer.Deserialize<Users>(request.Content ?? "");
                if (user is null) {
                    throw new ArgumentNullException("User was not sent with request!");
                }

                userRepository.CreateUser(user);
                response.ReturnCode = 201;
                response.ReturnText = "OK";
                response.Content = "User successfully created";
            }

            catch(ArgumentNullException e) {
                response.ReturnCode = 400;
                response.ReturnText = "Bad Request";
                response.Content = e.Message;
            }

            catch(SqlAlreadyFilledException e) {
                response.ReturnCode = 409;
                response.ReturnText = "Conflict";
                response.Content = e.Message;
            }

            catch(Exception) {
                response.ReturnCode = 400;
                response.ReturnText = "Bad Request";
                response.Content = "User couldnt be created";
            }
        }
    }
}
