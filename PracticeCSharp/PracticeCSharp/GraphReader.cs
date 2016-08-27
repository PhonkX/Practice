using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp
{
    public class GraphReader
    {
        public List<Node> ReadGraphFromFile(string fileName)
        {
            try
            {
                List<Node> chain = null;
                string[] tokens;
                int edgeCount = 0;
                int declaredEdgeCount = 0;
                var lines = File.ReadAllLines(fileName);
                foreach (var rawLine in lines)
                {
                    tokens = rawLine.Trim().Split(' ');

                    if (tokens[0] == "p")
                    {
                        if (chain == null)
                        {
                            chain = CreateEmptyChain(tokens, out edgeCount);
                        }
                        else
                        {
                            throw new Exception("Должно быть только одно объявление графа");
                        }
                    }

                    if (tokens[0] == "e")
                    {
                        AddEdgeToGraph(chain, tokens);
                        declaredEdgeCount++;
                        if (declaredEdgeCount == edgeCount)
                        {
                            break;
                        }
                    }
                }

                return chain;
            }
            catch
            {
                throw;
            }
        }

        private List<Node> CreateEmptyChain(string[] tokens, out int edgeCount)
        {
            if (tokens.Length != 4)
            {
                throw new Exception("Некорректное объявление графа");
            }
            int vertexCount;
            var isVertexCountValid = int.TryParse(tokens[2], NumberStyles.Any,
                CultureInfo.InvariantCulture, out vertexCount);
            if (!isVertexCountValid || vertexCount < 0)
            {
                throw new Exception("Некорректное количество вершин");
            }

            var chain = new List<Node>(vertexCount);

            var isEdgeCountValid = int.TryParse(tokens[3], NumberStyles.Any,
                CultureInfo.InvariantCulture, out edgeCount);
            if (!isEdgeCountValid || vertexCount < 0)
            {
                throw new Exception("Некорректное количество рёбер");
            }

            for (int i = 0; i < vertexCount; ++i)
            {
                chain.Add(new Node(i, i));
            }

            return chain;
        }

 
        private void AddEdgeToGraph(List<Node> chain, string[] tokens)
        {
            if (tokens.Length != 3)
            {
                throw new Exception("Некорректное объявление ребра");
            }

            int firstNodePosition;
            int secondNodePosition;
            var isFirstNodePositionValid = int.TryParse(tokens[1], NumberStyles.Any,
                    CultureInfo.InvariantCulture, out firstNodePosition);
            var isSecondNodePositionValid = int.TryParse(tokens[2], NumberStyles.Any,
                    CultureInfo.InvariantCulture, out secondNodePosition);
            if (!isFirstNodePositionValid || !isSecondNodePositionValid
                || firstNodePosition < 1 || secondNodePosition < 1
                || firstNodePosition > chain.Count || secondNodePosition > chain.Count)
            {
                throw new Exception("Некорректное задание ребра");
            }

            firstNodePosition--;
            secondNodePosition--;
            chain[firstNodePosition].Neighbours.Add(secondNodePosition);
            chain[secondNodePosition].Neighbours.Add(firstNodePosition);
        }
    }
}
