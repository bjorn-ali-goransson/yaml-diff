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

            var intermediateForms = new List<List<YamlNode>>();

            foreach (var originalChild in original.Children.Where(ch => !changed.Children.Contains(ch)))
            {
                var index = original.Children.IndexOf(originalChild);

                if(index <= changed.Children.Count - 1 && !original.Children.Contains(changed.Children[index]))
                {
                    continue;
                }

                result.Add(new Difference(ChangeType.Addition, path.Append(original.Children.IndexOf(originalChild)), originalChild));

                var intermediateForm = intermediateForms.Any() ? intermediateForms.Last().ToList() : original.Children.ToList();

                intermediateForm.Remove(originalChild);

                intermediateForms.Add(intermediateForm);
            }

            foreach (var changedChild in changed.Children.Where(ch => !original.Children.Contains(ch)))
            {
                var index = changed.Children.IndexOf(changedChild);

                if (index <= original.Children.Count - 1 && !changed.Children.Contains(original.Children[index]))
                {
                    continue;
                }

                result.Add(new Difference(ChangeType.Deletion, path.Append(changed.Children.IndexOf(changedChild)), changedChild));

                var intermediateForm = intermediateForms.Any() ? intermediateForms.Last().ToList() : original.Children.ToList();

                intermediateForm.Insert(index, changedChild);

                intermediateForms.Add(intermediateForm);
            }

            foreach (var child in original.Children.Where(ch => changed.Children.Contains(ch)))
            {
                var oldIndex = original.Children.IndexOf(child);
                var newIndex = changed.Children.IndexOf(child);

                if (oldIndex != newIndex && !intermediateForms.Any(f => f.IndexOf(child) == newIndex))
                {
                    result.Add(new Difference(ChangeType.Transposition, path.Append(oldIndex), child));

                    var intermediateForm = intermediateForms.Any() ? intermediateForms.Last().ToList() : original.Children.ToList();

                    intermediateForm.RemoveAt(oldIndex);
                    intermediateForm.Insert(newIndex, child);

                    intermediateForms.Add(intermediateForm);
                }
            }

            foreach (var child in original.Children.Where(ch => changed.Children.Contains(ch)))
            {
                var oldIndex = original.Children.IndexOf(child);
                var newIndex = changed.Children.IndexOf(child);

                if (oldIndex != newIndex && !result.Any(d => d.Node == child))
                {
                    result.Add(new Difference(ChangeType.ImplicitTransposition, path.Append(oldIndex), child));
                }
            }

            return result.OrderBy(d => d.Path.Segments.Last());
        }
    }
}
