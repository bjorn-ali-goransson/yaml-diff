using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace YamlDiff.Tests
{
    public class NodeComparerTests
    {
        [Theory]
        [InlineData("lorem", "lorem", true)]
        [InlineData("lorem", "ipsum", false)]
        public void ComparesScalarString(string a, string b, bool expected)
        {
            var result = new NodeComparer().Compare(new YamlScalarNode(a), new YamlScalarNode(b));

            Assert.Equal(expected, result);
        }
    }
}
