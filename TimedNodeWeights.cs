using System.Collections.Generic;
using System.Linq;

namespace WeightedTimeGraph
{
    public class TimedNodeWeights
    {
        private readonly TimedState timedState;
        private readonly List<Node> resultingNodes;

        public string Identifier => timedState.Identifier;

        public TimedNodeWeights(TimedState timedState)
        {
            this.timedState = timedState;
            resultingNodes = new List<Node>();
        }

        public void AddResultingNode(Node node)
        {
            if (resultingNodes.Any(n => Equals(n, node))) return;
            resultingNodes.Add(node);
        }

        public IEnumerable<Node> GetResultingNodes()
        {
            return resultingNodes;
        }

        public decimal GetVarianceWithOtherTimedNodeState(TimedState otherTimedState)
        {
            return timedState.GetVarianceWithOtherTimedState(otherTimedState);
        }
    }
}
