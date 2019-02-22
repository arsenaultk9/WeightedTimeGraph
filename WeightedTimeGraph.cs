using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;

namespace WeightedTimeGraph
{
    public class WeightedTimeGraph
    {
        private readonly Dictionary<string, TimedNodeWeights> timedNodeWeightses;
        private readonly IEnumerable<Node> nodes;

        private readonly ContextState contextState;
        private readonly ThreadSafeRandom randomGen;

        public WeightedTimeGraph(IEnumerable<Node> nodes)
        {
            this.nodes = nodes;
            timedNodeWeightses = new Dictionary<string, TimedNodeWeights>();
            this.contextState = new ContextState(nodes);

            randomGen = new ThreadSafeRandom();
        }

        public void UpdateGraph(Node node)
        {
            var currentTimeState = contextState.GetCurrentTimedState();
            var timedNodeWeights = GetTimeNodesWeights(currentTimeState);

            timedNodeWeights.AddResultingNode(node);

            contextState.UpdateState(node);
        }

        public string GenerateText(Node firstNode, int lenght)
        {
            if(timedNodeWeightses.Count == 0)
                throw new InvalidOperationException("Cannot generate data from empty graph.");

            var generatedContextState = new ContextState(nodes);
            generatedContextState.UpdateState(firstNode);

            return GetGeneratedTextFromContext(generatedContextState, firstNode, lenght);
        }

        #region Private Methods

        private TimedNodeWeights GetTimeNodesWeights(TimedState currentTimedState)
        {
            if (timedNodeWeightses.ContainsKey(currentTimedState.Identifier)) return timedNodeWeightses[currentTimedState.Identifier];

            var newTimedState = new TimedNodeWeights(currentTimedState);
            timedNodeWeightses[currentTimedState.Identifier] = newTimedState;

            return newTimedState;
        }

        private string GetGeneratedTextFromContext(ContextState generatedContextState, Node firstNode, int lenght)
        {
            var timeNodeWeights = generatedContextState.GetCurrentTimedState();
            var closedTimedNodeWeights = GetClosestTimedNodeWeights(timeNodeWeights);

            var textNodes = new List<Node> {firstNode};
            var totalCompletedPercent = 0;

            for (int charIndex = 0; charIndex < lenght - 1; charIndex++)
            {
                var possibleNodes = closedTimedNodeWeights.GetResultingNodes().ToList();
                var selectedNode = possibleNodes[randomGen.Next(0, possibleNodes.Count)];

                textNodes.Add(selectedNode);
                generatedContextState.UpdateState(selectedNode);

                timeNodeWeights = generatedContextState.GetCurrentTimedState();
                closedTimedNodeWeights = GetClosestTimedNodeWeights(timeNodeWeights);

                var currentPercent = charIndex*100/lenght;
                if (currentPercent <= totalCompletedPercent) continue;

                Console.WriteLine($"{currentPercent}% completed.");
                totalCompletedPercent = currentPercent;
            }

            return GetTextFromNodes(textNodes);
        }

        private TimedNodeWeights GetClosestTimedNodeWeights(TimedState timeNodeWeights)
        {
            var closestTimedNodeWeights = timedNodeWeightses.First().Value;
            var closestDifference = decimal.MaxValue;

            foreach (var timedNodeWeights in timedNodeWeightses)
            {
                var otherTimedNodeWeights = timedNodeWeights.Value;
                var difference = otherTimedNodeWeights.GetVarianceWithOtherTimedNodeState(timeNodeWeights);

                if(difference == 0m) return otherTimedNodeWeights;

                if (difference < closestDifference)
                {
                    closestTimedNodeWeights = otherTimedNodeWeights;
                    closestDifference = difference;
                }
            }

            return closestTimedNodeWeights;
        }

        private string GetTextFromNodes(List<Node> textNodes)
        {
            return string.Join("", textNodes.Select(n => n.Value));
        }

        #endregion
    }
}
