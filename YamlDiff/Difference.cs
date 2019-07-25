using System.Collections.Generic;
using System.Diagnostics;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    [DebuggerDisplay("{Path}: {ChangeType}")]
    public class Difference
    {
        public ChangeType ChangeType { get; }
        public Path Path { get; }
        public YamlNode OriginalNode { get; }

        public Difference(ChangeType changeType, Path path, YamlNode originalNode)
        {
            ChangeType = changeType;
            Path = path;
            OriginalNode = originalNode;
        }
    }
}