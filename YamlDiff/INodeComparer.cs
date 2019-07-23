using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public interface INodeComparer
    {
        bool Compare(YamlNode a, YamlNode b);
    }
}