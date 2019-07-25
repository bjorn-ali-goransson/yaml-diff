using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace YamlDiff
{
    [DebuggerDisplay("{ToString()}")]
    public class Path
    {
        public IEnumerable<object> Segments { get; }

        public Path(params object[] segments)
        {
            Segments = segments.ToList().AsReadOnly();
        }

        public Path Append(object segment)
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

        public override string ToString()
        {
            return string.Join(string.Empty, Segments.Select(s => s is int ? $"[{s}]" : $".{s}"));
        }

        public bool IsUnder(Path path)
        {
            return StartsWith(path);
        }

        public override bool Equals(object obj)
        {
            if(obj is Path path)
            {
                return Enumerable.SequenceEqual(Segments, path.Segments);
            }

            return false;
        }
    }
}
