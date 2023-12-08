namespace AdventOfCode.Puzzles._2023.Day08
{
    internal abstract class StormBase : BaseSolution<(IEnumerable<char> directions, Dictionary<string, Node> nodeDict)>
    {
        internal override (IEnumerable<char> directions, Dictionary<string, Node> nodeDict) ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@".\Puzzles\2023\Day08\Input.txt");
            var nodes= new List<Node>();
            var directions = lines[0].Trim().ToCharArray();

            foreach (var line in lines.Skip(1))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var name = line[..3];
                    nodes.Add(new Node { Name = name });
                }
            }
            var nodeDict = nodes.ToDictionary(nodes => nodes.Name);
            foreach (var line in lines.Skip(1))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var name = line[..3];
                    var leftRight = line[5..].Trim('(', ')', ' ').Split(',', StringSplitOptions.TrimEntries);
                    nodeDict[name].Left = nodeDict[leftRight[0]];
                    nodeDict[name].Right = nodeDict[leftRight[1]];
                }
            }

            return (directions, nodeDict);
        }
    }

    public class Node
    {
        public string Name { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node this[char direction] => direction switch
        {
            'L' => Left,
            'R' => Right,
            _ => throw new IndexOutOfRangeException()
        };
    }
}
