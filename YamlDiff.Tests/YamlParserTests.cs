using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace YamlDiff.Tests
{
    public class YamlParserTests
    {
        [Fact]
        public void ParsesSimpleDocument()
        {
            var key = "prop";
            var value = "Lorem";
            var document = new Parser().Parse($"{key}: {value}");
            Assert.Single(document.Children);
            Assert.Equal(key, document.Children.Single().Key);
            Assert.Equal(value, document.Children.Single().Value);
        }
    }
}
