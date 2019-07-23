using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public interface INodeFinder
    {
        YamlNode Find(Path path, YamlNode root);
    }
}