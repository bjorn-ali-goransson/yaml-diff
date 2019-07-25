using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace YamlDiff.Tests
{
    public class DiffGeneratorTests
    {
        [Fact]
        public void CallsNodeComparer()
        {
            var originalDocument = @"
                lorem: ipsum
            ";
            var changedDocument = @"
                lorem: dolor
            ";

            var difference = new Difference(ChangeType.Mutation, new Path("lorem"), null, null);

            var nodeComparer = Mock.Of<INodeComparer>();
            Mock.Get(nodeComparer).Setup(c => c.Compare(It.IsAny<Path>(), It.IsAny<YamlNode>(), It.IsAny<YamlNode>())).Returns(() => new[] { difference });

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), nodeComparer).Generate(Parser.Parse(originalDocument), Parser.Parse(changedDocument));

            Mock.Get(nodeComparer).Verify(c => c.Compare(It.Is<Path>(p => p.IsRoot()), It.IsAny<YamlNode>(), It.IsAny<YamlNode>()));
            Assert.Single(result);
            Assert.Same(difference, result.Single());
        }

        [Fact]
        public void DetectChangedNestedMappingNodeScalarNode()
        {
            var originalDocument = @"
                lorem:
                    ipsum: dolor
            ";
            var changedDocument = @"
                lorem:
                    ipsum: sit
            ";

            var result = new DiffGenerator(new NodeTraverser(), new NodeFinder(), new NodeComparer(new MappingNodeComparer(), new SequenceNodeComparer())).Generate(Parser.Parse(originalDocument), Parser.Parse(changedDocument));

            Assert.Single(result);
            Assert.Equal(new object[] { "lorem", "ipsum" }, result.Single().Path.Segments);
        }
    }
}
