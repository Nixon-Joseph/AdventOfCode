namespace AdventOfCode.Puzzles._2025.Day3;

public abstract class ElevatorBase :  BaseSolution<IEnumerable<string>>
{
    internal override IEnumerable<string> ReadInputFromFile()
    {
        return File.ReadAllLines("./Puzzles/2025/Day3/Input.txt");
    }
}
