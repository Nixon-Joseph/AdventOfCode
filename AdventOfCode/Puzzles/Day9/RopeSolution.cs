using System.Text.RegularExpressions;
using Rope = System.Collections.Generic.List<AdventOfCode.Puzzles.Day9.RopeSegment>;

namespace AdventOfCode.Puzzles.Day9
{
    public class RopeSolution : BaseSolution<IEnumerable<RopeInstruction>>
    {
        protected RopeGrid RopeGrid { get; } = new();

        public override object Solve()
        {
            var instructions = ReadInputFromFile();
            foreach (var instruction in instructions)
            {
                RopeGrid.ProcessInstruction(instruction);
            }
            return RopeGrid.GetTailVisitedCount();
        }

        internal override IEnumerable<RopeInstruction> ReadInputFromFile()
        {
            return File.ReadAllLines("./Puzzles/Day9/Input.txt").Select(x => new RopeInstruction(x));
        }
    }

    public class RopeGrid
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        public readonly Rope Rope;
        public readonly GridCoordinates StartPosition;
        private readonly List<List<RopeGridNode>> _nodes = new();
        private const int RopeLength = 10;

        public RopeGrid()
        {
            _nodes.Add(new List<RopeGridNode> { new RopeGridNode(true) });
            //Width = 1;
            //Height = 1;
            Rope = new Rope();
            var currentSegmentCount = 0;
            RopeSegment lastSegment = null;
            while (currentSegmentCount++ < RopeLength)
            {
                var newSegment = new RopeSegment { Head = lastSegment, Position = new GridCoordinates() };
                if (lastSegment != null)
                    lastSegment.Tail = newSegment;
                Rope.Add(newSegment);
                lastSegment = newSegment;
            }
            StartPosition = new GridCoordinates();
        }

        private readonly List<GridCoordinates> TailPositions = new List<GridCoordinates> { new GridCoordinates() };

        public void ProcessInstruction(RopeInstruction instruction)
        {
            var iteration = 0;
            while (iteration++ < instruction.Distance)
            {
                switch (instruction.Direction)
                {
                    case RopeInstructionDirection.Up: Rope.First().Position.Y++; break;
                    case RopeInstructionDirection.Left: Rope.First().Position.X--; break;
                    case RopeInstructionDirection.Down: Rope.First().Position.Y--; break;
                    case RopeInstructionDirection.Right: Rope.First().Position.X++; break;
                    default: throw new ArgumentOutOfRangeException(nameof(instruction), instruction.Direction, null);
                }
                Rope.First().Tail.MoveSegments(TailPositions);
            }
        }

        public RopeGridNode this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                {
                    throw new IndexOutOfRangeException();
                }
                return _nodes.ElementAt(x).ElementAt(y);
            }
        }

        public int GetTailVisitedCount()
        {
            return TailPositions.Count();
            //return _nodes.Sum(x => x.Count(y => y.HasHadTail));
        }
    }

    public class RopeSegment
    {
        public RopeSegment Head { get; set; }
        public GridCoordinates Position { get; set; }
        public RopeSegment Tail { get; set; }

        public void MoveSegments( List<GridCoordinates> tailPositions )
        {
            var diffX = Math.Abs(Position.X - Head.Position.X);
            var diffY = Math.Abs(Position.Y - Head.Position.Y);
            var moved = false;
            if (diffX + diffY >= 3) // diagonal movement needed
            {
                if (Head.Position.Y - Position.Y > 1) // 2 up
                {
                    Position.X = Head.Position.X;
                    Position.Y = Head.Position.Y - 1;
                }
                else if (Position.Y - Head.Position.Y > 1) // 2 down
                {
                    Position.X = Head.Position.X;
                    Position.Y = Head.Position.Y + 1;
                }
                else if (Head.Position.X - Position.X > 1) // 2 right
                {
                    Position.Y = Head.Position.Y;
                    Position.X = Head.Position.X - 1;
                }
                else // if (Position.X - Position.X > 1) // 2 left
                {
                    Position.Y = Head.Position.Y;
                    Position.X = Head.Position.X + 1;
                }
                moved = true;
            }
            else if (diffY > 1)
            {
                if (Head.Position.Y - Position.Y > 1)
                    Position.Y++;
                else
                    Position.Y--;
                moved = true;
            }
            else if (diffX > 1)
            {
                if (Head.Position.X - Position.X > 1)
                    Position.X++;
                else
                    Position.X--;
                moved = true;
            }
            if (Tail == null && !tailPositions.Any(x => x == Position))
                tailPositions.Add(Position.Copy());
            if (moved && Tail != null)
                Tail.MoveSegments(tailPositions);
        }
    }

    public class RopeGridNode
    {
        public bool HasHadTail { get; set; }

        public RopeGridNode() { }
        public RopeGridNode(bool hasHadTail) => HasHadTail = hasHadTail;
    }

    public class GridCoordinates : IComparable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static bool operator ==(GridCoordinates left, GridCoordinates right) => left.X == right.X && left.Y == right.Y;

        public static bool operator !=(GridCoordinates left, GridCoordinates right) => !(left == right);
        public int CompareTo( object? obj )
        {
            if (obj is GridCoordinates compareObj)
            {
                if (compareObj == this)
                    return 0;
                if (compareObj.X > X || compareObj.Y > Y)
                    return -1;
                if (compareObj.X < X || compareObj.Y < Y)
                    return 1;
            }
            return -1;
        }

        public GridCoordinates Copy() => new GridCoordinates { Y = Y, X = X };
    }

    public class RopeInstruction
    {
        private static readonly Regex InstructionRegex = new("^(?<direction>[DULR]{1}) (?<distance>[0-9]{1,})$");
        public readonly RopeInstructionDirection Direction;
        public readonly int Distance;

        public RopeInstruction(string instruction)
        {
            if (InstructionRegex.Match(instruction) is { Success: true } match)
            {
                Direction = match.Groups["direction"].Value switch
                {
                    "U" => RopeInstructionDirection.Up,
                    "R" => RopeInstructionDirection.Right,
                    "L" => RopeInstructionDirection.Left,
                    "D" => RopeInstructionDirection.Down,
                    _ => Direction
                };
                Distance = int.Parse(match.Groups["distance"].Value);
            }
            else
            {
                throw new Exception("Failed to parse instruction");
            }
        }
    }
    public enum RopeInstructionDirection
    {
        Up, Left, Down, Right
    }
}
