using System.Collections.Generic;
using System.Linq;

namespace WeightedTimeGraph
{
    public class ContextState
    {
        private TimedState currentTimedState;

        public ContextState(IEnumerable<Node> nodes)
        {
            var nodeContextStates = nodes.ToDictionary(node => node, node => 0m);
            currentTimedState = new TimedState(nodeContextStates);
        }

        public void UpdateState(Node node)
        {
            currentTimedState = currentTimedState.GetUpdatedTimedState(node);
        }

        public TimedState GetCurrentTimedState()
        {
            return currentTimedState;
        }
    }
}
