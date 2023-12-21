namespace AdventOfCode.Puzzles
{
    public class XYCoord
    {
        public int X { get; set; }
        public int Y { get; set; }
        public object? Value { get; set; }

        public XYCoord() { }

        public XYCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(XYCoord other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        public int HeuristicDistanceTo(XYCoord other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        // override == and != operators
        public static bool operator ==(XYCoord a, XYCoord b)
        {
            if (a is null && b is null)
                return true;
            if (a is null || b is null)
                return false;
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(XYCoord a, XYCoord b)
        {
            return !(a == b);
        }
    }

    public class DirectionalXYCoord : XYCoord
    {
        public Direction Direction { get; set; }

        public DirectionalXYCoord() { }

        public DirectionalXYCoord(int x, int y, Direction direction = Direction.None) : base(x, y)
        {
            Direction = direction;
        }

        public Direction OppositeDirection => Direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => Direction.None
        };
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
}
