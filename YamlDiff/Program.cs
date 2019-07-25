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
            new ConsoleWriter().GenerateDiff(args[0], args[1]);
        }
    }
}
