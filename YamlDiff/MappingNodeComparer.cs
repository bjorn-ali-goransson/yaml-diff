using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class MappingNodeComparer : IMappingNodeComparer
    {
        public IEnumerable<Difference> Compare(Path path, YamlMappingNode original, YamlMappingNode changed)
        {
            var result = new List<Difference>();

            foreach(var pair in original.Children.Where(p => p.Value is YamlScalarNode))
            {
                var key = (YamlScalarNode)pair.Key;
                var originalValue = (YamlScalarNode)pair.Value;
                var changedValue = changed.Children.ContainsKey(key) ? changed.Children[key] as YamlScalarNode : null;

                if(changedValue == null || originalValue.Value != changedValue.Value)
                {
                    result.Add(new Difference(ChangeType.Mutation, path.Append(key.Value), originalValue));
                }
            }

            return result.AsReadOnly();
        }
    }
}
