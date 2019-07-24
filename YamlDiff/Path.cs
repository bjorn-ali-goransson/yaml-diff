using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace YamlDiff
{
    [DebuggerDisplay("{string.Join(\".\", Segments)}")]
    public class Path
    {
        public IEnumerable<string> Segments { get; }

        public Path(params string[] segments)
        {
            Segments = segments.ToList().AsReadOnly();
        }

        public Path Append(string segment)
        {
            var segments = Segments.ToList();

            segments.Add(segment);

            return new Path(segments.ToArray());
        }

        public bool StartsWith(Path path)
        {
            return Enumerable.SequenceEqual(Segments.Take(path.Segments.Count()), path.Segments);
        }

        public bool IsRoot()
        {
            return !Segments.Any();
        }
    }
}
