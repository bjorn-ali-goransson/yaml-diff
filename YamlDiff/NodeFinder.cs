using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class NodeFinder : INodeFinder
    {
        YamlScalarNode NameNode { get; } = new YamlScalarNode("name");

        public YamlNode Find(Path path, YamlNode root)
        {
            var position = root;

            foreach (var segment in path.Segments)
            {
                if (position is YamlMappingNode mappingNode)
                {
                    var key = new YamlScalarNode(segment);

                    if (mappingNode.Children.ContainsKey(key))
                    {
                        position = mappingNode[key];
                    }
                    else
                    {
                        return null;
                    }
                }
                else if(position is YamlSequenceNode sequenceNode)
                {
                    var key = new YamlScalarNode(segment);

                    var child = sequenceNode.Children.OfType<YamlMappingNode>().Where(n => n.Children.ContainsKey(NameNode) && n.Children[NameNode].Equals(key)).FirstOrDefault();

                    if (child != null)
                    {
                        position = child;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            return position;
        }
    }
}
