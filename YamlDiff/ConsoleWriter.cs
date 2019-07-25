using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YamlDiff
{
    public class ConsoleWriter
    {
        public void GenerateDiff(string original, string changed)
        {
            var a = File.ReadAllText(original);
            var b = File.ReadAllText(changed);

            var changes = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer(new MappingNodeComparer(), new SequenceNodeComparer())).Generate(Parser.Parse(a), Parser.Parse(b));

            var lines = a.Split('\n');
            var colorizations = changes.SelectMany(ch => ch.OriginalNode.AllNodes.Select(n => new Colorization(ch.ChangeType == ChangeType.Deletion ? ConsoleColor.Red : ch.ChangeType == ChangeType.Mutation ? ConsoleColor.DarkYellow : ConsoleColor.Green, n.Start.Line, n.Start.Column, n.End.Column, n.End.Column - n.Start.Column)));

            var text = Console.ForegroundColor;

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                var colors = new Queue<Colorization>(colorizations.Where(c => c.Line == y + 1).OrderBy(c => c.StartColumn));

                Console.ForegroundColor = text;

                if (!colors.Any())
                {
                    Console.WriteLine(line);
                    continue;
                }

                var x = 0;
                while (colors.Any())
                {
                    var color = colors.Dequeue();

                    if(color.StartColumn < x)
                    {
                        continue;
                    }

                    Console.Write(line.Substring(x, color.StartColumn - 1 - x));
                    x = color.StartColumn - 1;
                    Console.ForegroundColor = color.Color;
                    Console.Write(line.Substring(x, color.Length));
                    x = color.EndColumn - 1;
                    Console.ForegroundColor = text;
                }

                Console.Write(line.Substring(x));

                Console.WriteLine();
            }
        }
    }
}
