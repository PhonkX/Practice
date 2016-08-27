using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp
{
    public class Evolution
    {
        private List<GraphChain> population;
        private List<GraphChain> buffer;
        private int populationSize;
        private Random rand;
        private int nodesToTakeFromFirstChain;
        private HashSet<int> randomPositionsInPopulation;
        private HashSet<int> randomPositionsInChain;

        public Evolution(List<GraphChain> startChains, int n, int populationSize)
        {
            this.population = startChains;
            this.nodesToTakeFromFirstChain = n;
            this.populationSize = populationSize;
            rand = new Random();
            randomPositionsInPopulation = new HashSet<int>();
            randomPositionsInChain = new HashSet<int>();
        }

        public Evolution(GraphChain chain, int populationSize)
        {
            this.populationSize = populationSize;
            population = new List<GraphChain>(this.populationSize);
            buffer = new List<GraphChain>();
            rand = new Random();
            randomPositionsInPopulation = new HashSet<int>();
            randomPositionsInChain = new HashSet<int>();
            for (int i = 0; i < this.populationSize; ++i)
            {
                population.Add(MakeChainMutation(chain, chain.Length).Result);
                Console.WriteLine("Generated {0} chains", i + 1);
            }
            nodesToTakeFromFirstChain = chain.Length / 2;
        }

        public async Task StartEvolutionProcess()
        {
            int iteration = 1;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                await MakeEvolutionIteration();
                InformationAboutTopChain(stopwatch.Elapsed, iteration++);
            }
        }

        public async Task MakeEvolutionIteration()
        {
            await MakeMutationInPopulation(population);
            buffer.Clear();
            foreach (var chain1 in population)
            {
                foreach (var chain2 in population)
                {
                    if (chain1 != chain2) // тут такое сравнение сработает, поскольку список один и тот же
                    {
                        var newChain = new GraphChain(chain1, chain2, nodesToTakeFromFirstChain);
                        if (newChain.IsCorrect())
                        {
                            buffer.Add(newChain);
                        }
                    }
                }
            }
            if (buffer.Count < populationSize)
            {
                for (int i = buffer.Count; i < populationSize; ++i)
                {
                    int mutatingChainPosition = rand.Next(buffer.Count);
                    buffer.Add(await MakeChainMutation(
                               buffer[mutatingChainPosition],
                               rand.Next(buffer[mutatingChainPosition].Length - 1) + 1 // чтобы точно что-то поменялось;
                               ));
                }
            }

            population.Clear();
            population = buffer.OrderBy(chain => chain.Rating()).Take(populationSize).ToList();
        }

        public async Task MakeMutationsAndAddToBuffer(List<GraphChain> buffer)
        {
            randomPositionsInPopulation.Clear(); 
            int position = 0;
            var mutationCount = rand.Next(populationSize - 1) + 1;
            for (int i = 0; i < mutationCount; ++i)
            {
                while (randomPositionsInPopulation.Contains(position = rand.Next(populationSize)));
                randomPositionsInPopulation.Add(position);

                var chainToMutate = population[position];
                var newChain = await MakeChainMutation(chainToMutate, rand.Next(chainToMutate.Length));
                buffer.Add(newChain);
            }
        }

        public async Task<GraphChain> MakeChainMutation(GraphChain chain, int numberOfMutatingNodes)
        {
            if (numberOfMutatingNodes == 0)
                return chain; //чтобы не копировать лишний раз
            var tempChain = new GraphChain(chain);
            randomPositionsInChain.Clear();
            int randomColor = -1; //чтобы анализатор студию не ругался на неинициализированную переменную
            int position = 0;
            for (int i = 0; i < numberOfMutatingNodes; ++i)
            {
                while (randomPositionsInChain.Contains(position = rand.Next(tempChain.Length)));
                randomPositionsInChain.Add(position);
                
                var currentColor = tempChain.Chain[position].Color;
                
                bool isChainCorrect = false;
                while (!isChainCorrect)
                {
                    while ((randomColor = rand.Next(tempChain.Length + 1)) ==
                        tempChain.Chain[position].Color) ;

                    tempChain.Chain[position].Color = randomColor;

                    if (tempChain.IsCorrect())
                    {
                        isChainCorrect = true;
                        tempChain.Colors[currentColor]--;

                        if (tempChain.Colors[currentColor] == 0)
                            tempChain.Colors.Remove(currentColor);

                        if (!tempChain.Colors.Keys.Contains(randomColor))
                            tempChain.Colors.Add(randomColor, 1);
                        else
                            tempChain.Colors[randomColor]++;
                    }
                    else
                    {
                        tempChain.Chain[position].Color = currentColor;
                    }
                }
            }
            
            return tempChain;
        }

        public async Task MakeMutationInPopulation(List<GraphChain> population)
        {
            int position = 0;
            var mutationCount = rand.Next(populationSize - 1) + 1;
            for (int i = 0; i < mutationCount; ++i)
            {
                position = rand.Next(populationSize);
                var chainToMutate = population[position];
                chainToMutate = await MakeChainMutation(chainToMutate, rand.Next(chainToMutate.Length));
                population[position] = chainToMutate;
            }
        }

        public async Task InformationAboutTopChain(TimeSpan elapsed, int iteration)
        {
            var colorsCount = population.First().Colors.Count;
            Console.WriteLine("Elapsed time: {0} ", elapsed.ToString());
            Console.WriteLine("Iteration: {0}", iteration);
            Console.WriteLine("Minimal colors count: {0}", colorsCount);
        }
    }
}
