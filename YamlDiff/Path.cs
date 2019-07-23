using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YamlDiff
{
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
    }
}
