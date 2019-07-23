using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public interface IParser
    {
        YamlMappingNode Parse(string value);
    }
}