using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class NodeComparer : INodeComparer
    {
        IMappingNodeComparer MappingNodeComparer { get; }
        ISequenceNodeComparer SequenceNodeComparer { get; }

        public NodeComparer(IMappingNodeComparer mappingNodeComparer, ISequenceNodeComparer sequenceNodeComparer)
        {
            MappingNodeComparer = mappingNodeComparer;
            SequenceNodeComparer = sequenceNodeComparer;
        }

        public IEnumerable<Difference> Compare(Path path, YamlNode original, YamlNode changed)
        {
            var result = new List<Difference>();

            if (original is YamlMappingNode originalMappingNode && changed is YamlMappingNode changedMappingNode)
            {
                result.AddRange(MappingNodeComparer.Compare(path, originalMappingNode, changedMappingNode));
            }

            if (original is YamlSequenceNode originalSequenceNode && changed is YamlSequenceNode changedSequenceNode)
            {
                result.AddRange(SequenceNodeComparer.Compare(path, originalSequenceNode, changedSequenceNode));
            }

            return result.AsReadOnly();
        }
    }
}
