using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp
{
    class Node
    {
        //TODO: сделать соседей
        public int Color;
        public List<int> Neighbours;
        public int number;

        public Node(int number, int color)
        {
            Color = color;
            this.number = number;
        }
    }


    class GraphChain : IChain
    {
        private List<Node> Chain;
        private int colorsCount;
        private int chainSize;
        private int maxColors;
        public int Length
        {
            get
            {
                return Chain.Count;
            }
        }

        public GraphChain(int chainSize, int maxColors)
        {
            //TODO: доделать, сделать генерацию upd: генерация сделана?
            //TODO: подумтаь над посчётом количества цветов
            this.chainSize = chainSize;
            this.maxColors = maxColors;
            Chain = new List<Node>(chainSize);
            Random rand = new Random();
            for (int i = 0; i < chainSize; ++i)
            {
                Chain.Add(new Node(i, rand.Next(maxColors)));
            }
        }

        public GraphChain(GraphChain chain1, GraphChain chain2, int n)
        {
            Chain = chain1.Chain
                    .Take(n)
                    .ToList()
                    .Concat(
                        chain2.Chain
                        .Skip(n)
                        .ToList()
                    )
                    .ToList();
        }

        public bool IsCorrect()
        {
            //throw new NotImplementedException();
            foreach (var node in Chain)
            {
                foreach (var neighbour in node.Neighbours)
                {
                    if (Chain[neighbour].Color == node.Color)
                        return false;
                }
            }
            return true;
        }

        public IChain MakeChain(IChain chain1, IChain chain2, int n)
        {
            //throw new NotImplementedException();
            return new GraphChain(chain1 as GraphChain, chain2 as GraphChain, n);
            //а нужен ли этот метод?
        }

        public int Rating()
        {
            //throw new NotImplementedException();
            return 1 - colorsCount / maxColors;
        }
    }
}
