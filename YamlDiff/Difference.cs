using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class Difference
    {
        public Path Path { get; }
        public YamlNode OriginalNode { get; }
        public YamlNode ChangedNode { get; }

        public Difference(Path path, YamlNode originalNode, YamlNode changedNode)
        {
            Path = path;
            OriginalNode = originalNode;
            ChangedNode = changedNode;
        }
    }
}