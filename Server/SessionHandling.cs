using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTGServer {
    public class SessionHandling {

        public static ConcurrentDictionary<string, string> Sessions = new ConcurrentDictionary<string, string>();

        public static bool CheckSession(string token) {
            if (Sessions.ContainsKey(token)) {
                return true;
            }
            return false;
        }



        public static string GetUsername(string inputString) {
            string username = "";
            int startIndex = inputString.IndexOf("Bearer ") + "Bearer ".Length;

            if (startIndex != -1 && startIndex < inputString.Length) {

                int endIndex = inputString.IndexOf('-', startIndex);
                if (endIndex != -1) {
                    username = inputString[startIndex..endIndex];
                }
            }
            return username;

        }
    }
}
