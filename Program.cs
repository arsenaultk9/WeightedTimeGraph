using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WeightedTimeGraph
{
    class Program
    {
        private const string SourceFileName = "Source.txt";
        private const string GeneratedFileName = "Generated.txt";

        static void Main(string[] args)
        {
            string text = File.ReadAllText(SourceFileName, Encoding.UTF8).ToLower();

            var characters = text.ToCharArray().Select(c => new Node(c.ToString())).ToList();

            var nodes = characters.Distinct();
            var graph = new WeightedTimeGraph(nodes);

            TrainGraph(graph, characters);
            var generatedText = graph.GenerateText(new Node("t"), 1000);

            File.WriteAllText(GeneratedFileName, generatedText);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void TrainGraph(WeightedTimeGraph graph, List<Node> characters)
        {
            foreach (var character in characters)
            {
                graph.UpdateGraph(character);
            }
        }
    }
}
