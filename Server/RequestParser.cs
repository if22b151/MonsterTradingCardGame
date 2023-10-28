using System.Text;

namespace MCTGServer {
    internal class RequestParser {

        private StreamReader _reader;

        public string HttpMethod { get; set; } = "";
        public string Path { get; set; } = "";
        public Dictionary<string, string> Headers { get; set; } = new();

        public string? Content { get; set; } = "";

        public RequestParser(StreamReader reader) {
            _reader = reader;
        }

        public void RequestParsing() {
            string? line = _reader.ReadLine();
            if (line != null) {
                string[] parts = line.Split(' ');
                HttpMethod = parts[0];
                Path = parts[1];
            }
            int content_length = 0;

            while ((line = _reader.ReadLine()) != null) {
                Console.WriteLine(line);
                if (line == "") {
                    break;
                }
                var parts = line.Split(':');
                if(parts.Length > 2) { 
                    Headers[parts[0]] = parts[1];
                    if (parts[0] == "Content-Length") {
                        content_length = int.Parse(parts[1].Trim());
                    }
                }
       
            }
            if (content_length > 0) {
                var data = new StringBuilder(200);
                char[] chars = new char[1024];
                int bytesTotal = 0;
                while (bytesTotal < content_length) {
                    var bytesRead = _reader.Read(chars, 0, chars.Length);
                    bytesTotal += bytesRead;
                    if (bytesRead == 0) {
                        break;
                    }
                    data.Append(chars, 0, bytesRead);
                }
                Console.WriteLine(data.ToString());
                Content = data.ToString();
            }
        }
    }
}