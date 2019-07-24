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

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer(new MappingNodeComparer(), new SequenceNodeComparer())).Generate(Parser.Parse(a), Parser.Parse(b)).ToDictionary(d => d.OriginalNode.Start.Line - 1, d => d);

            var lines = a.Split('\n');

            for(var i = 0; i < lines.Length; i++)
            {
                Console.WriteLine(lines[i]);
                if (result.ContainsKey(i))
                {
                    Console.WriteLine($"{result[i].OriginalNode.Start}---{result[i].OriginalNode.Start} hej");
                }
            }

        }
    }
}
