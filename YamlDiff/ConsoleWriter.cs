using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class ConsoleWriter
    {
        public void OutputDiff(string original, YamlNode originalDocument, string changed, YamlNode changedDocument, List<Difference> changes)
        {
            var originalLines = original.Split('\n');
            var changedLines = changed.Split('\n');

            var finder = new NodeFinder();
            var deletions = new Dictionary<int, string>();
            foreach(var change in changes.Where(ch => ch.ChangeType == ChangeType.Deletion))
            {
                deletions[finder.Find(change.Path, originalDocument).Start.Line - 1] = string.Join('\n', changedLines.Skip(change.Node.Start.Line - 1).Take(change.Node.AllNodes.Max(n => n.End.Line) - change.Node.Start.Line + 1));
            }
            
            changes.RemoveAll(ch => ch.ChangeType == ChangeType.Deletion);

            var colorizations = Colorization.CreateFrom(changes);

            for (var y = 0; y < originalLines.Length; y++)
            {
                if (deletions.ContainsKey(y))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(deletions[y]);
                    Console.ForegroundColor = Colorization.DefaultTextColor;
                }

                var line = originalLines[y];
                var colors = new Queue<Colorization>(colorizations.Where(c => c.Line == y + 1).OrderBy(c => c.StartColumn));

                Console.ForegroundColor = Colorization.DefaultTextColor;

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
                    Console.ForegroundColor = Colorization.DefaultTextColor;
                }

                Console.Write(line.Substring(x));

                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
