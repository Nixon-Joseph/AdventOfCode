namespace AdventOfCode.Puzzles.Day4
{
    public abstract class CampCleanSolutionBase : BaseSolution<IEnumerable<AssignmentPair>>
    {
        internal override IEnumerable<AssignmentPair> ReadInputFromFile()
        {
            return File.ReadLines("./Puzzles/Day4/Input.txt")
                .Select(x => new AssignmentPair(x));
        }
    }

    public class AssignmentPair
    {
        public Assignment Assignment1 { get; set; }
        public Assignment Assignment2 { get; set; }

        public AssignmentPair(string assignments) {
            var pairParts = assignments.Split(",");
            Assignment1 = new Assignment(pairParts[0]);
            Assignment2 = new Assignment(pairParts[1]);
        }

        public bool FullyContains() => Assignment1.FullyContains(Assignment2) || Assignment2.FullyContains(Assignment1);

        public bool Overlaps() => Assignment1.Overlaps(Assignment2) || Assignment2.Overlaps(Assignment1);

        public class Assignment
        {
            public int Start { get; set; }
            public int End { get; set; }

            public Assignment(string assignment) {
                var parts = assignment.Split("-");
                Start = int.Parse(parts[0]);
                End = int.Parse(parts[1]);
            }

            public bool Overlaps(Assignment otherAssignment) => (otherAssignment.Start >= Start && otherAssignment.Start <= End) || (otherAssignment.End <= End && otherAssignment.End >= Start);

            public bool FullyContains(Assignment otherAssignment) => Start <= otherAssignment.Start && End >= otherAssignment.End;
        }
    }
}
