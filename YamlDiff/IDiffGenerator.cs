using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public interface IDiffGenerator
    {
        IEnumerable<Difference> Generate(YamlNode original, YamlNode changed);
    }
}