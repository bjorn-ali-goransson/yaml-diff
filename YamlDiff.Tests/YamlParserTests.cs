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

        [Fact]
        public void YamlNodeShallowEquals()
        {
            var original = @"
                lorem: ipsum
            ";
            var changed = @"
                lorem: ipsum
            ";
            var changed2 = @"
                lorem: dolor
            ";

            Assert.Equal(Parser.Parse(original), Parser.Parse(changed));
            Assert.NotEqual(Parser.Parse(original), Parser.Parse(changed2));
        }

        [Fact]
        public void YamlNodeDeepEquals()
        {
            var original = @"
                lorem: ipsum
                dolor:
                    sit: 1
                    amet: 2
            ";
            var changed = @"
                lorem: ipsum
                dolor:
                    sit: 1
                    amet: 2
            ";
            var changed2 = @"
                lorem: ipsum
                dolor:
                    sit: 1
                    amet: 0
            ";

            Assert.Equal(Parser.Parse(original), Parser.Parse(changed));
            Assert.NotEqual(Parser.Parse(original), Parser.Parse(changed2));
        }
    }
}
