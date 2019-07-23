using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace YamlDiff.Tests
{
    public class NodeFinderTests
    {
        [Fact]
        public void FindsNodeInRootOfMappingNode()
        {
            var document = @"
                lorem: ipsum
            ";

            var result = new NodeFinder().Find(new Path("lorem"), new Parser().Parse(document));

            Assert.Equal(new YamlScalarNode("ipsum"), result);
        }

        [Fact]
        public void FindsNodeInNestedMappingNode()
        {
            var document = @"
                lorem:
                    ipsum: 1
            ";

            var result = new NodeFinder().Find(new Path("lorem", "ipsum"), new Parser().Parse(document));

            Assert.Equal(new YamlScalarNode("1"), result);
        }

        [Fact]
        public void FindsNamedChildInSequenceNode()
        {
            var document = @"
                lorem:
                  - ipsum: 1
                  - name: dolor
                    sit: 2
            ";

            var result = new NodeFinder().Find(new Path("lorem", "dolor", "sit"), new Parser().Parse(document));

            Assert.Equal(new YamlScalarNode("2"), result);
        }
    }
}
