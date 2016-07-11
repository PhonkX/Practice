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
        private List<IChain> population;
        private List<IChain> buffer;
        private int populationSize;
        private Random rand;
        private int n; //подумать, как лучше назвать переменную, говорящую, сколько ячеек из цепочки нужно вытащить

        public Evolution(List<IChain> startChains, int n, int populationSize)//TODO: добавить генерацию списка цепочек
        {
            this.population = startChains;
            this.n = n;
            this.populationSize = populationSize;
            rand = new Random();
        }
        public void MakeEvolution()
        {
            // throw new NotImplementedException();
            buffer.Clear();
            foreach (var chain1 in population)
            {
                foreach (var chain2 in population)
                {
                    var newChain = chain1.MakeChain(chain1, chain2, n);//TODO: подумать, как вытащить метод от объекта; может, просто заменить
                                                                       //на конструктор?
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
            population = buffer.OrderByDescending(chain => chain.Rating()).Take(populationSize).ToList();
        }

        public IChain MakeMutation(IChain chain, int numberOfMutatingNodes)
        {
            throw new NotImplementedException();
        }
    }
}
