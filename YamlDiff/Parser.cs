using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public static class Parser
    {
        public static YamlNode Parse(string value)
        {
            using (var read = new StringReader(value))
            {
                var yaml = new YamlStream();
                yaml.Load(read);
                return yaml.Documents.Single().RootNode;
            }
        }
    }
}
