using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTGServer {
    public interface IHTTPEndpoint {
        public bool HandleRequest(RequestParser request, HTTPResponder response);
    }
}
