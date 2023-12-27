using System.Text;

namespace MCTGServer {
    public class RequestParser {

        private StreamReader _reader;

        public HttpMethods Method { get; set; } = HttpMethods.GET;
        public string[] Path { get; set; } = new string[0];
        public Dictionary<string, string> Headers { get; set; } = new();

        public Dictionary<string, string> QueryParameters { get; set; } = new();

        public string? Content { get; set; } = "";

        public RequestParser(StreamReader reader) {
            _reader = reader;
        }

        public void RequestParsing() {
            string? line = _reader.ReadLine();
            if (line != null) {
                string[] parts = line.Split(' ');
                Method = (HttpMethods)Enum.Parse(typeof(HttpMethods), parts?[0] ?? "GET");
                string[]? pathandQuery = parts?[1].Split('?') ?? Array.Empty<string>();
                Path = pathandQuery[0].Split('/');
                if(pathandQuery.Length > 1) {
                    string[] queryParams = pathandQuery[1].Split('&');
                    foreach(var queryParam in queryParams) {
                        string[] queryParamParts = queryParam.Split('=');
                        if(queryParamParts?.Length > 1)
                            QueryParameters[queryParamParts[0]] = (queryParamParts?.Length == 2) ? queryParamParts[1] : "";
                    }
                }
            }
            int content_length = 0;

            while ((line = _reader.ReadLine()) != null) {
                Console.WriteLine(line);
                if (line == "") {
                    break;
                }
                var parts = line.Split(':');
                if(parts.Length >= 2) { 
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