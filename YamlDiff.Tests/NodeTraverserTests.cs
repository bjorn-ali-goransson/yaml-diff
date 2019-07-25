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
            Assert.Equal(new object[] { }, result.Single().Path.Segments);
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
            Assert.Equal(new object[] { }, result.First().Path.Segments);
            Assert.IsType<YamlMappingNode>(result.First().Node);
            Assert.Equal(new object[] { "lorem" }, result.Last().Path.Segments);
            Assert.IsType<YamlMappingNode>(result.Last().Node);
        }

        [Fact]
        public void TraversesSequenceNodes()
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

            Assert.Equal(new object[] { }, result.ElementAt(0).Path.Segments);
            Assert.IsType<YamlMappingNode>(result.ElementAt(0).Node);

            Assert.Equal(new object[] { "lorem" }, result.ElementAt(1).Path.Segments);
            Assert.IsType<YamlSequenceNode>(result.ElementAt(1).Node);

            Assert.Equal(new object[] { "amet" }, result.ElementAt(2).Path.Segments);
            Assert.IsType<YamlSequenceNode>(result.ElementAt(2).Node);

            Assert.Equal(new object[] { "lorem", 0 }, result.ElementAt(3).Path.Segments);
            Assert.IsType<YamlMappingNode>(result.ElementAt(3).Node);

            Assert.Equal(new object[] { "lorem", 1 }, result.ElementAt(4).Path.Segments);
            Assert.IsType<YamlMappingNode>(result.ElementAt(4).Node);

            Assert.Equal(new object[] { "amet", 0 }, result.ElementAt(5).Path.Segments);
            Assert.IsType<YamlMappingNode>(result.ElementAt(5).Node);
        }
    }
}
