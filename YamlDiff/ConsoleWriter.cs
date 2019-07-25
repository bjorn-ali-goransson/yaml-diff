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

            var changes = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer(new MappingNodeComparer(), new SequenceNodeComparer())).Generate(Parser.Parse(a), Parser.Parse(b)).ToList();

            changes.RemoveAll(ch => ch.ChangeType == ChangeType.ImplicitTransposition && changes.Any(c => c.ChangeType != ChangeType.ImplicitTransposition && c.Path.Equals(ch.Path)));

            var lines = a.Split('\n');
            var colorizations = changes.SelectMany(ch => ch.OriginalNode.AllNodes.Select(n => new Colorization(GetColor(ch.ChangeType), n.Start.Line, n.Start.Column, n.End.Column, n.End.Column - n.Start.Column)));

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

        ConsoleColor GetColor(ChangeType changeType)
        {
            if (changeType == ChangeType.Addition)
            {
                return ConsoleColor.Green;
            }
            if (changeType == ChangeType.Deletion)
            {
                return ConsoleColor.Red;
            }
            if (changeType == ChangeType.Transposition)
            {
                return ConsoleColor.Blue;
            }
            if (changeType == ChangeType.Mutation)
            {
                return ConsoleColor.DarkYellow;
            }
            return ConsoleColor.Gray;
        }
    }
}
