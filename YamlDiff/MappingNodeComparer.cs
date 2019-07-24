using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class MappingNodeComparer : IMappingNodeComparer
    {
        public IEnumerable<Difference> Compare(Path path, YamlMappingNode original, YamlMappingNode changed)
        {
            throw new NotImplementedException();
        }
    }
}
