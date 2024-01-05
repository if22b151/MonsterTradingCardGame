using System.Net;

namespace MCTGServer {
    public class HTTPResponder {

        private StreamWriter _writer;

        public int ReturnCode { get; set; } = 200;

        public string ReturnText { get; set; } = "OK";

        public Dictionary<string, string> Headers { get; set; } = new();
        public string? Content { get; set; }

        public HTTPResponder(StreamWriter writer)
        {
            _writer = writer;
        }

        public void SetResponse(int responsecode, string responsetext, string responsecontent) {
            ReturnCode = responsecode;
            ReturnText = responsetext;
            Content = responsecontent;
        }

        public void SetResponse(int responsecode, string responsetext, string responsecontent, string headername, string headervalue) {
            ReturnCode = responsecode;
            ReturnText = responsetext;
            Content = responsecontent;
            Headers[headername] = headervalue;
        }

        public void SendResponse() {
            Console.WriteLine("----------------------------------------");

            // ----- 3. Write the HTTP-Response -----
            var writerAlsoToConsole = new StreamTracer(_writer);  

            writerAlsoToConsole.WriteLine($"HTTP/1.1 {ReturnCode} {ReturnText}");     
            
            if(Content != null) {
                Headers["Content-Length"] = Content.Length.ToString();

            }

            writerAlsoToConsole.WriteLine();
            if(Content != null) {
                 writerAlsoToConsole.WriteLine($"{Content}");    

            }

            Console.WriteLine("========================================");
        }
    }
}