namespace WeightedTimeGraph
{
    public class Node
    {
        public readonly string Value;

        public Node(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var otherNode = obj as Node;
            if (otherNode == null) return false;

            return Value == otherNode.Value;
        }

        protected bool Equals(Node other)
        {
            return string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}
