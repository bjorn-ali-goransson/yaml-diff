using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class Parser : IParser
    {
        public YamlMappingNode Parse(string value)
        {
            using (var read = new StringReader(value))
            {
                var yaml = new YamlStream();
                yaml.Load(read);
                return (YamlMappingNode)yaml.Documents.Single().RootNode;
            }
        }
    }
}
