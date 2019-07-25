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
        public YamlNode Node { get; }

        public Difference(ChangeType changeType, Path path, YamlNode node)
        {
            ChangeType = changeType;
            Path = path;
            Node = node;
        }
    }
}