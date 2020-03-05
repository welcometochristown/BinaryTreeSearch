using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeSearch
{
    class Program
    {
        class Node
        {
            public readonly List<Node> Neighbours = new List<Node>();
            public readonly string Name;

            public Node(string name)
            {
                this.Name = name;
            }
        }

        static void Main(string[] args)
        {
            Dictionary<string, Node> Nodes = BuildNodeTree();

            var startNode = Nodes["0"];
            var endNode = Nodes["5"];

            var fastestRoute = FastestRoute(startNode, endNode);

            if (fastestRoute == null)
            {
                Console.WriteLine($"No Route Between {startNode.Name} and {endNode.Name}");
            }
            else
            {

                int position = 1;
                foreach (var n in fastestRoute)
                    Console.WriteLine($"{position++}). {n.Name}");

            }

            Console.ReadKey();
        }

        static Dictionary<string, Node> BuildNodeTree()
        {

           /*
                    (0*)
                  /    \
               (1)------(2*)
                          \
                           (3*)
                           / \
                        (4*)   (5)

            */

            Node n0 = new Node("0");
            Node n1 = new Node("1");
            Node n2 = new Node("2");
            Node n3 = new Node("3");
            Node n4 = new Node("4");
            Node n5 = new Node("5");

            n0.Neighbours.Add(n1);
            n0.Neighbours.Add(n2);
            n1.Neighbours.Add(n0);
            n1.Neighbours.Add(n2);
            n2.Neighbours.Add(n0);
            n2.Neighbours.Add(n3);
            n3.Neighbours.Add(n2);
            n3.Neighbours.Add(n4);
            n3.Neighbours.Add(n5);
            n4.Neighbours.Add(n3);
            n5.Neighbours.Add(n3);

            Dictionary<string, Node> dict = new Dictionary<string, Node>();
            dict.Add(n0.Name, n0);
            dict.Add(n1.Name, n0);
            dict.Add(n2.Name, n0);
            dict.Add(n3.Name, n0);
            dict.Add(n4.Name, n0);
            dict.Add(n5.Name, n0);
            return dict;
        }

        static IEnumerable<Node> FastestRoute(Node nStart, Node nEnd)
        {
            if (!nStart.Neighbours.Any() || !nEnd.Neighbours.Any())
                return null;

            if (nStart.Neighbours.Any(n => n == nEnd))
                return new[] { nStart, nEnd };

            Dictionary<Node, Tuple<Node, int>> nodemap = new Dictionary<Node, Tuple<Node, int>>(); // this will keep track of all nodes, their parent and distance from [nStart]


            Node last = null;
            Queue<Node> toVisit = new Queue<Node>();

            //first node
            nodemap.Add(nStart, new Tuple<Node, int>(null, 0));
            toVisit.Enqueue(nStart);

            while (toVisit.Count > 0)
            {

                //pop entity
                 var e = toVisit.Dequeue();

                //find the current distance of this node from [e1]
                int distance = nodemap[e].Item2;

                //iterate over each neighbour
                foreach (var n in e.Neighbours)
                {
                    //ignore the previous entity
                    if (last == n)
                        continue;

                    //if the nodemap doesnt contain this node, add it
                    if (!nodemap.ContainsKey(n))
                    {
                        //add node with parent and distance
                        nodemap.Add(n, new Tuple<Node, int>(e, distance + 1));
                        toVisit.Enqueue(n);
                    }
                    else
                    {
                        Tuple<Node, int> t = nodemap[n];

                        //update if a shorter distance was found
                        if (distance + 1 < t.Item2)
                            nodemap[e] = new Tuple<Node, int>(n, distance + 1);
                    }
                }

                last = e;

            }

            List<Node> results = new List<Node>();

            //iterate back through the nodes of the best solution
            for (var node = nEnd; node != null;)
            {
                if (!nodemap.ContainsKey(node))
                    return null;

                //reverse nodes
                results.Insert(0, node);
                node = nodemap[node].Item1;
            }

            return results;

        }
    }
}
