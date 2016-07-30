using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp
{
    class Evolution //подумать, надо ли делать отдельный интерфейс, или лучше вынести MakeChain в класс с цепочкой,
                    //и там сразу проверять, а тут проверять, что вернулось
    {
        private List<GraphChain> population;
        private List<GraphChain> buffer;
        private int populationSize;
        private Random rand;
        private int n; //подумать, как лучше назвать переменную, говорящую, сколько ячеек из цепочки нужно вытащить

        public Evolution(List<GraphChain> startChains, int n, int populationSize)//TODO: добавить генерацию списка цепочек
        {
            this.population = startChains;
            this.n = n;
            this.populationSize = populationSize;
            rand = new Random();
        }

        public Evolution(GraphChain chain, int populationSize)
        {
            this.populationSize = populationSize;
            population = new List<GraphChain>(this.populationSize);
            buffer = new List<GraphChain>();
            rand = new Random();
            for (int i = 0; i < this.populationSize; ++i)
            {
                population.Add(MakeMutation(chain, chain.Length));
            }
            n = chain.Length / 2;
        }

        public void StartEvolutionProcess()
        {
            while (true)
            {
                MakeEvolutionIteration();
                InformationAboutTopChain();
            }
        }

        public void MakeEvolutionIteration()
        {
            buffer.Clear();
            foreach (var chain1 in population)
            {
                foreach (var chain2 in population)
                {
                    var newChain = new GraphChain(chain1, chain2, n);
                    if (newChain.IsCorrect())
                        buffer.Add(newChain);
                }
            }
            if (buffer.Count < populationSize)
            {
                for (int i = buffer.Count; i < populationSize; ++i)
                {
                    int mutatingChainPosition = rand.Next(buffer.Count);
                    buffer.Add(MakeMutation(
                               buffer[mutatingChainPosition],
                               rand.Next(buffer[mutatingChainPosition].Length)
                               ));
                }
            }
            population.Clear();
            population = buffer.OrderBy(chain => chain.Rating()).Take(populationSize).ToList();
        }

        public GraphChain MakeMutation(GraphChain chain, int numberOfMutatingNodes)
        {
            if (numberOfMutatingNodes == 0)
                return chain; //чтобы не копировать лишний раз
            var tempChain = new GraphChain(chain);
            int randomColor = -1; //чтобы анализатор студию не ругался на неинициализированную переменную
            for (int i = 0; i < numberOfMutatingNodes; ++i)
            {
                var position = rand.Next(tempChain.Length);
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

        public void InformationAboutTopChain()
        {
            var colorsCount = population.First().Colors.Count;
            Console.WriteLine("Minimal colors count: {0}", colorsCount);
        }
    }
}
