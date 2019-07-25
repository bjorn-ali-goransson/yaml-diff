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
            args = new[] { @"C:\Dev\yaml-diff\YamlDiff\deployment.yaml", @"C:\Dev\yaml-diff\YamlDiff\cluster.yaml" };

            var a = File.ReadAllText(args[0]);
            var b = File.ReadAllText(args[1]);

            var changes = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer(new MappingNodeComparer(), new SequenceNodeComparer())).Generate(Parser.Parse(a), Parser.Parse(b));

            var lines = a.Split('\n');
            var colorizations = changes.SelectMany(ch => ch.OriginalNode.AllNodes.Select(n => new Colorization(ch.ChangeType == ChangeType.Deletion ? ConsoleColor.Red : ch.ChangeType == ChangeType.Mutation ? ConsoleColor.DarkYellow : ConsoleColor.Green, n.Start.Line, n.Start.Column, n.End.Column, n.End.Column - n.Start.Column)));

            var text = Console.ForegroundColor;

            for(var y = 0; y < lines.Length; y++)
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
                while(colors.Any())
                {
                    var color = colors.Dequeue();

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

        class Colorization
        {
            public ConsoleColor Color { get; }
            public int Line { get; }
            public int StartColumn { get; }
            public int EndColumn { get; }
            public int Length { get; }

            public Colorization(ConsoleColor color, int line, int startColumn, int endColumn, int length)
            {
                Color = color;
                Line = line;
                StartColumn = startColumn;
                EndColumn = endColumn;
                Length = length;
            }
        }
    }
}
