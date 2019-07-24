using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class SequenceNodeComparer : ISequenceNodeComparer
    {
        public IEnumerable<Difference> Compare(Path path, YamlSequenceNode original, YamlSequenceNode changed)
        {
            throw new NotImplementedException();
        }
    }
}
