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
            var a = File.ReadAllText(args[0]);
            var b = File.ReadAllText(args[1]);

            var parser = new Parser();

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer()).Generate(parser.Parse(a), parser.Parse(b));

            foreach(var difference in result.Entries)
            {
                Console.WriteLine(string.Join(".", difference.Path.Segments));
            }
        }
    }
}
