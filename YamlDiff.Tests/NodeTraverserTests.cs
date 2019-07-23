using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace YamlDiff.Tests
{
    public class NodeTraverserTests
    {
        [Fact]
        public void TraversesMappingNodes()
        {
            var document = @"
                lorem: ipsum
            ";

            var result = new NodeTraverser().Traverse(new Parser().Parse(document));

            Assert.Single(result);
            Assert.Single(result.Single().Path.Segments);
            Assert.Equal("lorem", result.Single().Path.Segments.Single());
            Assert.Equal(new YamlScalarNode("ipsum"), result.Single().Node);
        }

        [Fact]
        public void TraversesNestedMappingNodes()
        {
            var document = @"
                lorem:
                    ipsum: dolor
            ";
            
            var result = new NodeTraverser().Traverse(new Parser().Parse(document));

            Assert.Single(result);
            Assert.Equal(2, result.Single().Path.Segments.Count());
            Assert.Equal("lorem", result.Single().Path.Segments.First());
            Assert.Equal("ipsum", result.Single().Path.Segments.Last());
            Assert.Equal(new YamlScalarNode("dolor"), result.Single().Node);
        }

        [Fact]
        public void OnlyTraversesNamedChildrenInSequenceNodes()
        {
            var document = @"
                lorem:
                  - name: ipsum
                    dolor: 1
                  - sit: 2
            ";

            var result = new NodeTraverser().Traverse(new Parser().Parse(document));

            Assert.Single(result);
            Assert.Equal(3, result.Single().Path.Segments.Count());
            Assert.Equal("lorem", result.Single().Path.Segments.ElementAt(0));
            Assert.Equal("ipsum", result.Single().Path.Segments.ElementAt(1));
            Assert.Equal("dolor", result.Single().Path.Segments.ElementAt(2));
            Assert.Equal(new YamlScalarNode("1"), result.Single().Node);
        }
    }
}
