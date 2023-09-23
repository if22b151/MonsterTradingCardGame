using MTCGNew.Cards;

namespace MTCGNew {
    internal class StackCards {
        private List<Card> _stack;

        public List<Card> Stack {
            get { return _stack; }
            set { _stack = value; }
        }

        public StackCards()
        {
            _stack = new List<Card>();
        }

    }
}