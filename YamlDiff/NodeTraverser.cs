using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class NodeTraverser : INodeTraverser
    {
        YamlScalarNode NameNode { get; } = new YamlScalarNode("name");

        public IEnumerable<NodeTraversalPosition> Traverse(YamlNode root)
        {
            var result = new List<NodeTraversalPosition>();

            var queue = new Queue<NodeTraversalPosition>();

            queue.Enqueue(new NodeTraversalPosition(new Path(), root));

            while (queue.Any())
            {
                var position = queue.Dequeue();

                if (position.Node is YamlMappingNode mappingNode)
                {
                    result.Add(position);

                    foreach (var pair in mappingNode.Children)
                    {
                        queue.Enqueue(new NodeTraversalPosition(position.Path.Append(((YamlScalarNode)pair.Key).Value), pair.Value));
                    }
                }

                if (position.Node is YamlSequenceNode sequenceNode)
                {
                    var children = sequenceNode.Children.OfType<YamlMappingNode>().Where(n => n.Children.Keys.OfType<YamlScalarNode>().Contains(NameNode));

                    if (children.Any())
                    {
                        result.Add(position);
                    }

                    foreach (var child in children)
                    {
                        var path = position.Path.Append(((YamlScalarNode)child[NameNode]).Value);

                        queue.Enqueue(new NodeTraversalPosition(path, child));
                    }
                }
            }

            return result;
        }
    }
}
