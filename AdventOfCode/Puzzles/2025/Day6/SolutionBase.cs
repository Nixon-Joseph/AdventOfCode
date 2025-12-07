namespace AdventOfCode.Puzzles._2025.Day6;

public abstract class SolutionBase : BaseSolution<(IEnumerable<IEnumerable<ulong>> operands, IEnumerable<char> operators)>
{
    internal override (IEnumerable<IEnumerable<ulong>> operands, IEnumerable<char> operators) ReadInputFromFile()
    {
        var input = File.ReadAllLines("./Puzzles/2025/Day6/Input.txt");
        var operands = new List<List<ulong>>();
        var operators = new List<char>();
        var expectOperand = true;
        foreach (var line in input)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts[0] == "*" || parts[0] == "+")
            {
                expectOperand = false;
            }
            if (expectOperand)
            {
                operands.Add([.. parts.Select(ulong.Parse)]);
            }
            else
            {
                operators.AddRange(parts.Select(p => p[0]));
            }
        }

        return (operands, operators);
    }

    internal char[,] ReadInputAsCharArraysFromFile()
    {
        return ReadInputFileAsCharArray("./Puzzles/2025/Day6/Input.txt");
    }
}
