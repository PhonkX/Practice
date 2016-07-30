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
            int populationSize = 2;
            try
            {
                var chain = new GraphChain();
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
