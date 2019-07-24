using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace YamlDiff.Tests
{
    public class YamlParserTests
    {
        [Fact]
        public void ParsesSimpleDocument()
        {
            var key = "prop";
            var value = "Lorem";
            var document = (YamlMappingNode)Parser.Parse($"{key}: {value}");
            Assert.Single(document.Children);
            Assert.Equal(key, document.Children.Single().Key);
            Assert.Equal(value, document.Children.Single().Value);
        }
    }
}
