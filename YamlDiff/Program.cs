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

            new ConsoleWriter().GenerateDiff(args[0], args[1]);
        }
    }
}
