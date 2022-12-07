using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Day7
{
    public abstract class DirectorySolutionBase : BaseSolution<Directory>
    {
        private static readonly Regex CommandRegex = new("^[$] (?<command>cd|ls) (?<path>.*)$", RegexOptions.Compiled);
        private static readonly Regex FileSystemItemRegex = new("^(?<dirOrSize>dir|[0-9]+) (?<name>.*)$", RegexOptions.Compiled);

        internal override Directory ReadInputFromFile()
        {
            var codeLines = System.IO.File.ReadAllLines(@"./Puzzles/Day7/Input.txt");
            var folder = new Directory(null, "/");
            var workingFolder = folder;
            foreach(var line in codeLines)
            {
                if (line.StartsWith("$")) // command
                {
                    var commandMatch = CommandRegex.Match(line);
                    if (commandMatch.Success)
                    {
                        var path = commandMatch.Groups["path"].Value;
                        switch (commandMatch.Groups["command"].Value) {
                            case "cd":
                                workingFolder = path switch
                                    {
                                        "/" => folder,
                                        ".." => workingFolder?.Parent,
                                        _ => workingFolder?.Children?.First( x => x.Name == commandMatch.Groups[ "path" ].Value ) as Directory
                                    };
                                break;
                            //case "ls":
                            //    break;
                        }
                    }
                }
                else // output
                {
                    var fileSystemItemMatch = FileSystemItemRegex.Match(line);
                    if (fileSystemItemMatch.Success)
                    {
                        var dirOrSize = fileSystemItemMatch.Groups["dirOrSize"].Value;
                        var name = fileSystemItemMatch.Groups["name"].Value;
                        if (dirOrSize == "dir")
                            workingFolder?.Children?.Add(new Directory(workingFolder, name));
                        else
                            workingFolder?.Children?.Add(new File(name, int.Parse(dirOrSize)));
                    }
                }
            }
            return folder;
        }
    }

    public class Directory : IFileSystemItem
    {
        public string Name { get; set; }
        public int Size { get => Children?.Sum(x => x.Size) ?? 0; set => throw new NotImplementedException(); }
        public List<IFileSystemItem> Children { get; } = new();
        public Directory? Parent { get; set; }

        public Directory(Directory? parent, string name)
        {
            Parent = parent;
            Name = name;
        }
    }

    public class File : IFileSystemItem {
        public string Name { get; set; }
        public int Size { get; set; }
        
        public File(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }

    public interface IFileSystemItem
    {
        string Name { get; set; }
        int Size { get; set; }
    }
}
