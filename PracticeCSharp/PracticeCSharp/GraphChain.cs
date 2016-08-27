using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp
{
    public class Node
    {
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


    public class GraphChain
    {
        public List<Node> Chain;
        private int chainSize;
        private int maxColors;
        public Dictionary<int, int> Colors;

        public int Length 
        {
            get
            {
                return Chain.Count;
            }
        }

        public GraphChain(int chainSize, int maxColors)
        {
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
            Chain = new List<Node>();
            Colors = new Dictionary<int, int>();
        }

        public GraphChain(List<Node> nodes)
        {
            Chain = nodes;
            Colors = new Dictionary<int, int>();
            foreach(var node in nodes)
            {
                if (Colors.ContainsKey(node.Color))
                {
                    Colors[node.Color]++;
                }
                else
                {
                    Colors.Add(node.Color, 1);
                }
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
