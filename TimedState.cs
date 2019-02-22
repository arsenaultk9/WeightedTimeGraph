using System;
using System.Collections.Generic;
using System.Linq;

namespace WeightedTimeGraph
{
    public class TimedState
    {
        private const decimal WeightUpdateRatio = 1.1m;
        private readonly Dictionary<Node, decimal> nodeStates;

        public string Identifier => string.Join("|", nodeStates.Values);

        public TimedState(Dictionary<Node, decimal> nodeStates)
        {
            this.nodeStates = nodeStates;
        }

        public TimedState GetUpdatedTimedState(Node node)
        {
            var updatedNodeStates = new Dictionary<Node, decimal>();

            foreach (var nodeState in nodeStates)
            {
                if (Equals(nodeState.Key, node))
                {
                    updatedNodeStates.Add(node, 1);
                }
                else
                {
                    var updatedWeight = nodeState.Value / WeightUpdateRatio;
                    updatedNodeStates.Add(nodeState.Key, updatedWeight);
                }
            }

            return new TimedState(updatedNodeStates);
        }

        public decimal GetVarianceWithOtherTimedState(TimedState otherTimedState)
        {
            return nodeStates
                .Select(nodeState => nodeState.Key)
                .Select(node => Math.Abs(nodeStates[node] - otherTimedState.nodeStates[node]))
                .Sum();
        }
    }
}
