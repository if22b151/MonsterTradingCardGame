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

        public void SendResponse() {
            Console.WriteLine("----------------------------------------");

            // ----- 3. Write the HTTP-Response -----
            var writerAlsoToConsole = new StreamTracer(_writer);  // we use a simple helper-class StreamTracer to write the HTTP-Response to the client and to the console

            writerAlsoToConsole.WriteLine($"HTTP/1.1 {ReturnCode} {ReturnText}");     // first line in HTTP-Response contains the HTTP-Version and the status code
            
            if(Content != null) {
                Headers["Content-Length"] = Content.Length.ToString();

            }

            writerAlsoToConsole.WriteLine();
            if(Content != null) {
                 writerAlsoToConsole.WriteLine($"{Content}");    // the HTTP-content (here we just return a minimalistic HTML Hello-World)

            }

            Console.WriteLine("========================================");
        }
    }
}