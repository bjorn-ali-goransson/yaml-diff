using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class SequenceNodeComparer : ISequenceNodeComparer
    {
        public IEnumerable<Difference> Compare(Path path, YamlSequenceNode original, YamlSequenceNode changed)
        {
            var result = new List<Difference>();

            if (original.Children.Count == changed.Children.Count)
            {
                // transposition detection
            }
            else
            {
                foreach (var originalChild in original.Children)
                {
                    if (!changed.Children.Contains(originalChild))
                    {
                        result.Add(new Difference(ChangeType.Addition, path.Append(original.Children.IndexOf(originalChild)), originalChild, null));
                    }
                }

                foreach (var changedChild in changed.Children)
                {
                    if (!original.Children.Contains(changedChild))
                    {
                        result.Add(new Difference(ChangeType.Deletion, path.Append(changed.Children.IndexOf(changedChild)), changedChild, null));
                    }
                }
            }

            return result;
        }
    }
}
