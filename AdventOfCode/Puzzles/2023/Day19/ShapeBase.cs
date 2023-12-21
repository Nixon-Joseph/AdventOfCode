using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day19
{
    internal abstract class ShapeBase : BaseSolution<(Dictionary<string, Func<Shape, string>> workflowDict, List<Shape> shapes)>
    {
        protected static readonly Regex _workflowRegex = new (@"^(?<workflowKey>[\w]+)\{(?<flows>[^\}]+)\}$");
        protected static readonly Regex _workflowDetailRegex = new (@"^(?<property>[xmas])(?<operator>[\<\>])(?<value>[\d]+):(?<target>[\w]+)$");
        protected static readonly Regex _shapeRegex = new (@"^\{x=(?<x>[\d]+),m=(?<m>[\d]+),a=(?<a>[\d]+),s=(?<s>[\d]+)\}$");
    }

    public class Shape
    {
        public int X { get; set; }
        public int M { get; set; }
        public int A { get; set; }
        public int S { get; set; }
    }
}
