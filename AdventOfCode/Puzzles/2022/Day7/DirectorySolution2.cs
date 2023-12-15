namespace AdventOfCode.Puzzles.Day7
{
    internal class DirectorySolution2 : DirectorySolutionBase
    {
        private const int totalAvailable = 70000000;
        private const int updateSize = 30000000;

        protected override object DoSolve()
        {
            var rootFolder = ReadInputFromFile();
            var targetSize = updateSize - (totalAvailable - rootFolder.Size);
            var foundFolder = FindOptimalDirectoryToDelete(rootFolder, targetSize);
            return foundFolder?.Size ?? 0;
        }
        
        public Directory? FindOptimalDirectoryToDelete(Directory directory, int targetSize, Directory? foundFolder = null)
        {
            // folder is too small to delete - and no need to recurse over children
            if (directory.Size >= targetSize)
            { 
                if (foundFolder == null || directory.Size < foundFolder.Size)
                {
                    foundFolder = directory;
                }
                IEnumerable<Directory> childFolders = directory.Children.OfType<Directory>();
                if (childFolders.Any())
                {
                    foreach (var childFolder in childFolders)
                    {
                        foundFolder = FindOptimalDirectoryToDelete(childFolder, targetSize, foundFolder);
                    }
                }
            }
            return foundFolder;
        }
    }
}
