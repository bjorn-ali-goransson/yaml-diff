using System.Collections.Generic;

namespace YamlDiff
{
    public class Difference
    {
        public Path Path { get; }

        public Difference(Path path)
        {
            Path = path;
        }
    }
}