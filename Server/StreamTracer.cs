namespace MCTGServer {
    internal class StreamTracer {
        private StreamWriter writer;

        public StreamTracer(StreamWriter streamWriter)
        {
            this.writer = streamWriter;
        }

        internal void WriteLine(string v) {
            Console.WriteLine(v);
            writer.WriteLine(v);
        }

        internal void WriteLine() {
            Console.WriteLine();
            writer.WriteLine(); 
        }
    }
}