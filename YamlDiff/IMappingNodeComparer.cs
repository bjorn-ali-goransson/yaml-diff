using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public interface IMappingNodeComparer
    {
        IEnumerable<Difference> Compare(Path path, YamlMappingNode original, YamlMappingNode changed);
    }
}
