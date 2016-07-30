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
        public int Number;

        public Node(Node node)
        {
            Color = node.Color;
            Number = node.Number;
            Neighbours = new List<int>(node.Neighbours);
        }

        public Node(int number, int color)
        {
            Color = color;
            this.Number = number;
            Neighbours = new List<int>();
        }
    }


    class GraphChain
    {
        public List<Node> Chain;
        private int chainSize;
        private int maxColors;
        public Dictionary<int, int> Colors;

        public int Length //нужно ли это?
        {
            get
            {
                return Chain.Count;
            }
        }

        public GraphChain(int chainSize, int maxColors)
        {
            //TODO: доделать, сделать генерацию upd: сделать генерацию с вводом графа
            //TODO: подумать, надо ли делать верификацию сразу
            this.chainSize = chainSize;
            this.maxColors = maxColors;
            Chain = new List<Node>(chainSize);
            Random rand = new Random();
            for (int i = 0; i < chainSize; ++i)
            {
                int color = rand.Next(this.chainSize);
                Chain.Add(new Node(i, color));
                if (!Colors.Keys.Contains(color))
                    Colors.Add(color, 1);
                else Colors[color]++;
            }
        }

        public GraphChain()
        {
            int size = int.Parse(Console.ReadLine());
            if (size <= 0)
            {
                throw new Exception("В графе должно быть положительное число вершин.");
            }

            Chain = new List<Node>(size);
            Colors = new Dictionary<int, int>();
            for (int i = 0; i < size; ++i)
            {
                Chain.Add(new Node(i, i));
                Colors[i] = 1;
            }

            maxColors = size;

            string edgeString;
            while ((edgeString = Console.ReadLine()) != String.Empty)
            {
                var nodes = edgeString.Split(' ');
                int firstNodePosition = int.Parse(nodes[0]);
                int secondNodePosition = int.Parse(nodes[1]);
                Chain[firstNodePosition].Neighbours.Add(secondNodePosition);
                Chain[secondNodePosition].Neighbours.Add(firstNodePosition);
            }
        }

        public GraphChain(GraphChain chain1, GraphChain chain2, int n)
        {
            Colors = new Dictionary<int, int>();
            Chain = chain1.Chain
                    .Take(n)
                    .Concat(
                        chain2.Chain
                        .Skip(n)
                    )
                    .ToList();
            maxColors = Chain.Count;
            foreach (var node in Chain)
            {
                if (!Colors.Keys.Contains(node.Color))
                    Colors.Add(node.Color, 1);
                else
                    Colors[node.Color]++;
            }
           
        }

        public GraphChain(GraphChain chain)
        {
            Chain = new List<Node>();
            Colors = new Dictionary<int, int>();
            foreach (var node in chain.Chain)
            {
                Chain.Add(new Node(node));
                var color = node.Color;
                if (!Colors.Keys.Contains(color))
                {
                    Colors.Add(color, 1);
                }
                else
                {
                    Colors[color]++;
                }
             }
        }

        public bool IsCorrect()
        {
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

        public int Rating()
        {
            return Colors.Count;
        }
    }
}
