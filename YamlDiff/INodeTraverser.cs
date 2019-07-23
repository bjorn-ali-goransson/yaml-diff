using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public interface INodeTraverser
    {
        IEnumerable<NodeTraversalPosition> Traverse(YamlNode node);
    }
}