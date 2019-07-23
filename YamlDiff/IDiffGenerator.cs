using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public interface IDiffGenerator
    {
        Diff Generate(YamlNode original, YamlNode changed);
    }
}