using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public interface ISequenceNodeComparer
    {
        IEnumerable<Difference> Compare(Path path, YamlSequenceNode original, YamlSequenceNode changed);
    }
}