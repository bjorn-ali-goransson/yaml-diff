using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class NodeTraverser : INodeTraverser
    {
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
                    result.Add(position);

                    for (var i = 0; i < sequenceNode.Children.Count; i++)
                    {
                        var path = position.Path.Append(i);

                        queue.Enqueue(new NodeTraversalPosition(path, sequenceNode.Children[i]));
                    }
                }
            }

            return result;
        }
    }
}
