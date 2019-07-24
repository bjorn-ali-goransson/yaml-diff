using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public interface INodeComparer
    {
        IEnumerable<Difference> Compare(Path path, YamlNode original, YamlNode changed);
    }
}