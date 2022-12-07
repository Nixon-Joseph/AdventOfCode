namespace AdventOfCode.Puzzles.Day7
{
    internal class DirectorySolution1 : DirectorySolutionBase
    {
        private const int targetSize = 100000;

        public override object? Solve()
        {
            var rootFolder = ReadInputFromFile();
            int totalSize = 0;
            CheckFolderSize(rootFolder, ref totalSize);
            return totalSize;
        }
        
        public void CheckFolderSize(Directory? folder, ref int totalSizeSoFar)
        {
            if (folder.Size <= targetSize)
            {
                totalSizeSoFar += folder.Size;
            }
            IEnumerable<Directory?> childFolders = folder.Children.OfType<Directory>();
            if (childFolders.Any())
            {
                foreach (var childFolder in childFolders)
                {
                    CheckFolderSize(childFolder, ref totalSizeSoFar);
                }
            }
        }
    }
}
