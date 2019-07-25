using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YamlDiff
{
    class Program
    {
        public static void Main(string[] args)
        {
            var original = File.ReadAllText(args[0]);
            var originalDocument = Parser.Parse(original);
            var changed = File.ReadAllText(args[1]);
            var changedDocument = Parser.Parse(changed);

            var changes = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer(new MappingNodeComparer(), new SequenceNodeComparer())).Generate(originalDocument, changedDocument).ToList();

            new ConsoleWriter().OutputDiff(original, originalDocument, changed, changedDocument, changes);
        }
    }
}
