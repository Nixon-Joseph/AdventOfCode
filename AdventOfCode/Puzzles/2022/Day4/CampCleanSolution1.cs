using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles.Day4
{
    public class CampCleanSolution1 : CampCleanSolutionBase
    {
        protected override object DoSolve()
        {
            var assignmentPairs = ReadInputFromFile();
            return assignmentPairs.Count(x => x.FullyContains());
        }
    }
}
