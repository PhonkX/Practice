using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var graphReader = new GraphReader();
            int populationSize = 100;
            string fileName = "queen5_5.col";
            try
            {
                var nodes = graphReader.ReadGraphFromFile(fileName);
                var chain = new GraphChain(nodes);
                var evolutionProcess = new Evolution(chain, populationSize);
                evolutionProcess.StartEvolutionProcess();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
        }
    }
}
