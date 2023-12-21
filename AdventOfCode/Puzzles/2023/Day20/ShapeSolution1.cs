namespace AdventOfCode.Puzzles._2023.Day20
{
    internal class ShapeSolution1 : ShapeBase
    {
        protected override object DoSolve()
        {
            var (dict, shapes) = ReadInputFromFile();
            var acceptedShapes = new List<Shape>();

            foreach (var shape in shapes)
            {
                var workflow = "in";
                do
                {
                    workflow = dict[workflow](shape);
                } while (workflow != "A" && workflow != "R");
                if (workflow == "A")
                {
                    acceptedShapes.Add(shape);
                }
            }

            return acceptedShapes.Sum(s => s.X + s.M + s.A + s.S);
        }

        internal override (Dictionary<string, Func<Shape, string>> workflowDict, List<Shape> shapes) ReadInputFromFile()
        {
            var lines = ReadFileAsLines(@".\Puzzles\2023\Day20\Input.txt");
            var shapes = new List<Shape>();
            var workflowDict = new Dictionary<string, Func<Shape, string>>();

            var buildingWorkflows = true;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    buildingWorkflows = false;
                    continue;
                }
                if (buildingWorkflows)
                {
                    var match = _workflowRegex.Match(line);
                    if (match.Success)
                    {
                        var workflowKey = match.Groups["workflowKey"].ToString();
                        var flows = match.Groups["flows"].ToString();
                        workflowDict.Add(workflowKey, BuildFlow(flows));
                    }
                    else
                    {
                        throw new Exception($"Invalid workflow: {line}");
                    }
                }
                else
                {
                    var match = _shapeRegex.Match(line);
                    if (match.Success)
                    {
                        shapes.Add(new Shape
                        {
                            X = int.Parse(match.Groups["x"].ToString()),
                            M = int.Parse(match.Groups["m"].ToString()),
                            A = int.Parse(match.Groups["a"].ToString()),
                            S = int.Parse(match.Groups["s"].ToString())
                        });
                    }
                    else
                    {
                        throw new Exception($"Invalid shape: {line}");
                    }
                }
            }

            return (workflowDict, shapes);
        }

        private Func<Shape, string> BuildFlow(string flowsToParse)
        {
            var flows = flowsToParse.Split(',').ToList();
            var conditionals = new List<Func<Shape, string?>>();
            string defaultCase = flows.Last();
            flows.RemoveAt(flows.Count - 1);
            foreach (var flow in flows)
            {
                var match = _workflowDetailRegex.Match(flow);
                if (match.Success)
                {
                    var property = match.Groups["property"].ToString();
                    var op = match.Groups["operator"].ToString();
                    var value = int.Parse(match.Groups["value"].ToString());
                    var target = match.Groups["target"].ToString();
                    conditionals.Add((Shape shape) =>
                    {
                        var prop = property switch
                        {
                            "x" => shape.X,
                            "m" => shape.M,
                            "a" => shape.A,
                            "s" => shape.S,
                            _ => throw new Exception($"Invalid property: {property}")
                        };
                        var result = op switch
                        {
                            "<" => prop < value,
                            ">" => prop > value,
                            _ => throw new Exception($"Invalid operator: {op}")
                        };
                        return result ? target : null;
                    });
                }
                else
                {
                    throw new Exception($"Invalid workflow detail: {flow}");
                }
            }
            return (Shape shape) =>
            {
                foreach (var conditional in conditionals)
                {
                    if (conditional(shape) is string newWorkflow)
                    {
                        return newWorkflow;
                    }
                }
                return defaultCase;
            };
        }
    }
}
