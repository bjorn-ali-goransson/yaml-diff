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
            var queue = new Queue<NodeTraversalPosition>();

            queue.Enqueue(new NodeTraversalPosition(new Path(), root));

            while (queue.Any())
            {
                var position = queue.Dequeue();

                if (position.Node is YamlMappingNode mappingNode)
                {
                    foreach (var pair in mappingNode.Children)
                    {
                        queue.Enqueue(new NodeTraversalPosition(position.Path.Append(((YamlScalarNode)pair.Key).Value), pair.Value));
                    }
                }
                else if(position.Node is YamlSequenceNode sequenceNode)
                {
                    foreach (var child in sequenceNode.Children.OfType<YamlMappingNode>().Where(n => n.Children.Keys.OfType<YamlScalarNode>().Contains(NameNode)))
                    {
                        var path = position.Path.Append(((YamlScalarNode)child[NameNode]).Value);

                        foreach (var pair in child.Children)
                        {
                            if(pair.Key.Equals(NameNode))
                            {
                                continue;
                            }
                            queue.Enqueue(new NodeTraversalPosition(path.Append(((YamlScalarNode)pair.Key).Value), pair.Value));
                        }
                    }
                }
                else
                {
                    yield return position;
                }
            }


            yield break;
        }
    }
}
