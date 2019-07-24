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

            var result = new NodeTraverser().Traverse(Parser.Parse(document));

            Assert.Single(result);
            Assert.Equal(new string[] { }, result.Single().Path.Segments);
            Assert.IsType<YamlMappingNode>(result.Single().Node);
        }

        [Fact]
        public void TraversesNestedMappingNodes()
        {
            var document = @"
                lorem:
                    ipsum: dolor
            ";
            
            var result = new NodeTraverser().Traverse(Parser.Parse(document));

            Assert.Equal(2, result.Count());
            Assert.Equal(new string[] { }, result.First().Path.Segments);
            Assert.IsType<YamlMappingNode>(result.First().Node);
            Assert.Equal(new string[] { "lorem" }, result.Last().Path.Segments);
            Assert.IsType<YamlMappingNode>(result.Last().Node);
        }

        [Fact]
        public void OnlyTraversesNamedChildrenInSequenceNodes()
        {
            var document = @"
                lorem:
                  - name: ipsum
                    dolor: 1
                  - sit: 2
                amet:
                  - adipiscing: 3
            ";

            var result = new NodeTraverser().Traverse(Parser.Parse(document));

            Assert.Equal(new string[] { }, result.ElementAt(0).Path.Segments);
            Assert.IsType<YamlMappingNode>(result.ElementAt(0).Node);
            Assert.Equal(new string[] { "lorem" }, result.ElementAt(1).Path.Segments);
            Assert.IsType<YamlSequenceNode>(result.ElementAt(1).Node);
            Assert.Equal(new string[] { "lorem", "ipsum" }, result.ElementAt(2).Path.Segments);
            Assert.IsType<YamlMappingNode>(result.ElementAt(2).Node);
        }
    }
}
