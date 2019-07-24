using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace YamlDiff.Tests
{
    public class MappingNodeComparerTests
    {
        [Fact]
        public void DetectMissingScalarNodeInB()
        {
            var originalDocument = @"
                lorem: ipsum
                dolor: sit
            ";
            var changedDocument = @"
                dolor: sit
            ";

            var result = new MappingNodeComparer().Compare(new Path(), (YamlMappingNode)Parser.Parse(originalDocument), (YamlMappingNode)Parser.Parse(changedDocument));

            Assert.Single(result);
            Assert.Equal(new[] { "lorem" }, result.Single().Path.Segments);
        }
    }
}
