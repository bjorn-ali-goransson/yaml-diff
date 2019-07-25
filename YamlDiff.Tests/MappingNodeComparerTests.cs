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
        public void IdenticalSinglePropertyDocuments()
        {
            var originalDocument = @"
                lorem: ipsum
            ";
            var changedDocument = @"
                lorem: ipsum
            ";

            var result = new MappingNodeComparer().Compare(new Path(), (YamlMappingNode)Parser.Parse(originalDocument), (YamlMappingNode)Parser.Parse(changedDocument));

            Assert.Empty(result);
        }

        [Fact]
        public void DetectMissingScalarNode()
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

        [Fact]
        public void DetectChangedScalarNode()
        {
            var originalDocument = @"
                lorem: ipsum
            ";
            var changedDocument = @"
                lorem: dolor
            ";

            var result = new MappingNodeComparer().Compare(new Path(), (YamlMappingNode)Parser.Parse(originalDocument), (YamlMappingNode)Parser.Parse(changedDocument));

            Assert.Single(result);
            Assert.Equal(new[] { "lorem" }, result.Single().Path.Segments);
        }

        [Fact]
        public void DetectScalarNodeChangedToMappingNode()
        {
            var originalDocument = @"
                lorem: ipsum
            ";
            var changedDocument = @"
                lorem:
                    ipsum: sit
            ";

            var result = new MappingNodeComparer().Compare(new Path(), (YamlMappingNode)Parser.Parse(originalDocument), (YamlMappingNode)Parser.Parse(changedDocument));

            Assert.Single(result);
            Assert.Equal(new[] { "lorem" }, result.Single().Path.Segments);
        }
    }
}
