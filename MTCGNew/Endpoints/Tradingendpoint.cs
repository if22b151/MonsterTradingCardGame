using MCTGServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGNew.Endpoints {
    internal class Tradingendpoint : IHTTPEndpoint {
        public bool HandleRequest(RequestParser request, HTTPResponder responder) {
            responder.ReturnCode = 501;
            responder.ReturnText = "Not Implemented";
            responder.Content = "Not Implemented";
            return true;
        }


    }
}
