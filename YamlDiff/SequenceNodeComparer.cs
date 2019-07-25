using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class SequenceNodeComparer : ISequenceNodeComparer
    {
        YamlScalarNode NameNode { get; } = new YamlScalarNode("name");

        public IEnumerable<Difference> Compare(Path path, YamlSequenceNode original, YamlSequenceNode changed)
        {
            var result = new List<Difference>();

            var originalChildren = original.Children.OfType<YamlMappingNode>().Where(n => n.Children.ContainsKey(NameNode) && n.Children[NameNode] is YamlScalarNode);

            foreach(var originalChild in originalChildren)
            {
                var name = (YamlScalarNode)originalChild.Children[NameNode];
                var changedChild = changed.Children.OfType<YamlMappingNode>().Where(ch => ch.Children.ContainsKey(NameNode) && name.Equals(ch.Children[NameNode])).FirstOrDefault();

                if(changedChild == null)
                {
                    result.Add(new Difference(ChangeType.Addition, path.Append(name.Value), originalChild, changedChild));
                }
            }

            return result;
        }
    }
}
