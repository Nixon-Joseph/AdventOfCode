using System.Security;

namespace AdventOfCode.Puzzles
{
    public class WeightedGridPathSolver
    {
        private class Node
        {
            public Node Parent;
            public DirectionalXYCoord Position;
            public int G;
            public int H;
            public int F => G + H;
        }

        private int[,] grid;
        private List<Node> openList;
        private List<Node> closedList;

        public WeightedGridPathSolver(int[,] grid) => this.grid = grid;

        public List<DirectionalXYCoord>? Solve(DirectionalXYCoord start, DirectionalXYCoord end)
        {
            openList = new List<Node> { new() { Position = start, G = (int)start.Value } };
            closedList = new List<Node>();

            while (openList.Count > 0)
            {
                var current = openList.OrderBy(node => node.F).First();

                if (current.Position == end)
                {
                    var path = new List<DirectionalXYCoord>();
                    while (current != null)
                    {
                        path.Add(current.Position);
                        current = current.Parent;
                    }
                    path.Reverse();
                    return path;
                }

                openList.Remove(current);
                closedList.Add(current);

                Direction tooManySameDir = GetBlockedDirection(current);

                var adjacentNodes = GetAdjacentNodes(current, current.Position.OppositeDirection, tooManySameDir);
                foreach (var adjacentNode in adjacentNodes)
                {
                    if (closedList.Any(node => node.Position == adjacentNode.Position))
                        continue;

                    if (adjacentNode.G < current.G || !openList.Any(node => node.Position == adjacentNode.Position))
                    {
                        adjacentNode.G = current.G + grid[adjacentNode.Position.Y, adjacentNode.Position.X];
                        adjacentNode.H = adjacentNode.Position.HeuristicDistanceTo(end);
                        adjacentNode.Parent = current;

                        if (!openList.Any(node => node.Position == adjacentNode.Position))
                            openList.Add(adjacentNode);
                    }
                }
            }

            return null;
        }

        private Direction GetBlockedDirection(Node current)
        {
            if (current.Parent?.Parent?.Parent is not null)
            {
                if (current.Parent.Position.Direction == current.Parent.Parent.Position.Direction && current.Parent.Position.Direction == current.Parent.Parent.Parent.Position.Direction)
                {
                    return current.Parent.Position.Direction;
                }
            }
            return Direction.None;
        }

        private List<Node> GetAdjacentNodes(Node current, params Direction[] dirsToSkip)
        {
            var nodes = new List<Node>();

            // foreach over list of Directions
            foreach (var dir in Enum.GetValues(typeof(Direction)))
            {
                if ((Direction)dir == Direction.None || dirsToSkip.Contains((Direction)dir))
                {
                    continue;
                }

                var xPos = current.Position.X;
                var yPos = current.Position.Y;

                switch (dir)
                {
                    case Direction.Up:
                        yPos--;
                        break;
                    case Direction.Down:
                        yPos++;
                        break;
                    case Direction.Left:
                        xPos--;
                        break;
                    case Direction.Right:
                        xPos++;
                        break;
                }

                if (xPos >= 0 && xPos < grid.GetLength(1) && yPos >= 0 && yPos < grid.GetLength(0))
                {
                    nodes.Add(new Node { Position = new DirectionalXYCoord(xPos, yPos, (Direction)dir) { Value = grid[yPos, xPos] }, G = grid[yPos, xPos] });
                }
            }

            return nodes;
        }
    }
}
