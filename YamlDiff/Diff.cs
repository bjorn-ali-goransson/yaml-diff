using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YamlDiff
{
    public class Diff
    {
        public IEnumerable<Difference> Entries { get; }

        public Diff(IEnumerable<Difference> entries)
        {
            Entries = entries.ToList().AsReadOnly();
        }
    }
}
