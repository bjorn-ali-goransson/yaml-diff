using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class Difference
    {
        public ChangeType ChangeType { get; }
        public Path Path { get; }
        public YamlNode OriginalNode { get; }
        public YamlNode ChangedNode { get; }

        public Difference(ChangeType changeType, Path path, YamlNode originalNode, YamlNode changedNode)
        {
            ChangeType = changeType;
            Path = path;
            OriginalNode = originalNode;
            ChangedNode = changedNode;
        }
    }
}