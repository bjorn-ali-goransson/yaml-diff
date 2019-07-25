using System;
using System.Collections.Generic;
using System.Linq;

namespace YamlDiff
{
    public class Colorization
    {
        public static ConsoleColor DefaultTextColor { get; } = ConsoleColor.DarkGray;

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

        public static IEnumerable<Colorization> CreateFrom(IEnumerable<Difference> differences)
        {
            return differences.SelectMany(ch => ch.Node.AllNodes.Select(n => new Colorization(GetColor(ch.ChangeType), n.Start.Line, n.Start.Column, n.End.Column, n.End.Column - n.Start.Column)));
        }

        public static ConsoleColor GetColor(ChangeType changeType)
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
                return ConsoleColor.Yellow;
            }

            return DefaultTextColor;
        }
    }
}