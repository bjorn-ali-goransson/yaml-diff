using System;

namespace YamlDiff
{
    public class Colorization
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