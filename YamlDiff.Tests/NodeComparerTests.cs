using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace YamlDiff.Tests
{
    public class NodeComparerTests
    {
        [Fact]
        public void CallsMappingNodeComparer()
        {
            var original = @"
                lorem: ipsum
            ";
            var changed = @"
                lorem: dolor
            ";

            var mappingNodeComparer = Mock.Of<IMappingNodeComparer>();

            var result = new NodeComparer(mappingNodeComparer, Mock.Of<ISequenceNodeComparer>()).Compare(new Path(), Parser.Parse(original), Parser.Parse(changed));

            Mock.Get(mappingNodeComparer).Verify(c => c.Compare(It.Is<Path>(p => p.IsRoot()), It.IsAny<YamlMappingNode>(), It.IsAny<YamlMappingNode>()));
        }

        [Fact]
        public void CallsSequenceNodeComparer()
        {
            var original = @"
                - lorem: ipsum
            ";
            var changed = @"
                - lorem: dolor
            ";

            var sequenceNodeComparer = Mock.Of<ISequenceNodeComparer>();

            var result = new NodeComparer(Mock.Of<IMappingNodeComparer>(), sequenceNodeComparer).Compare(new Path(), Parser.Parse(original), Parser.Parse(changed));

            Mock.Get(sequenceNodeComparer).Verify(c => c.Compare(It.Is<Path>(p => p.IsRoot()), It.IsAny<YamlSequenceNode>(), It.IsAny<YamlSequenceNode>()));
        }
    }
}
