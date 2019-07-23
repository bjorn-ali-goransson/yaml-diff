using System.Collections.Generic;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class NodeTraversalPosition
    {
        public Path Path { get; }
        public YamlNode Node { get; }

        public NodeTraversalPosition(Path path, YamlNode node)
        {
            Path = path;
            Node = node;
        }
    }
}