using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace YamlDiff.Tests
{
    public class SequenceNodeComparerTests
    {
        [Fact]
        public void IdenticalSequenceLengthPasses()
        {
            var originalDocument = @"
                - lorem: ipsum
                - dolor: 1
            ";
            var changedDocument = @"
                - lorem: dolor
                - dolor: 1
            ";

            var result = new SequenceNodeComparer().Compare(new Path(), (YamlSequenceNode)Parser.Parse(originalDocument), (YamlSequenceNode)Parser.Parse(changedDocument));

            Assert.Empty(result);
        }

        [Fact]
        public void DetectAddedChild()
        {
            var originalDocument = @"
                - lorem: ipsum
                - dolor: sit
            ";
            var changedDocument = @"
                - lorem: ipsum
            ";

            var result = new SequenceNodeComparer().Compare(new Path(), (YamlSequenceNode)Parser.Parse(originalDocument), (YamlSequenceNode)Parser.Parse(changedDocument));

            Assert.Single(result);
            Assert.Equal(new object[] { 1 }, result.Single().Path.Segments);
            Assert.Equal(ChangeType.Addition, result.Single().ChangeType);
        }

        [Fact]
        public void DetectDeletedChild()
        {
            var originalDocument = @"
                - lorem: ipsum
            ";
            var changedDocument = @"
                - lorem: ipsum
                - dolor: sit
            ";

            var result = new SequenceNodeComparer().Compare(new Path(), (YamlSequenceNode)Parser.Parse(originalDocument), (YamlSequenceNode)Parser.Parse(changedDocument));

            Assert.Single(result);
            Assert.Equal(new object[] { 1 }, result.Single().Path.Segments);
            Assert.Equal(ChangeType.Deletion, result.Single().ChangeType);
        }

        [Fact]
        public void DetectTransposedChild()
        {
            var originalDocument = @"
                - lorem: ipsum
                - dolor: sit
            ";
            var changedDocument = @"
                - dolor: sit
                - lorem: ipsum
            ";

            var result = new SequenceNodeComparer().Compare(new Path(), (YamlSequenceNode)Parser.Parse(originalDocument), (YamlSequenceNode)Parser.Parse(changedDocument));

            Assert.Equal(2, result.Count());
            Assert.Equal(new object[] { 0 }, result.Single().Path.Segments);
            Assert.Equal(ChangeType.Transposition, result.Single().ChangeType);
            Assert.Equal(new object[] { 1 }, result.Single().Path.Segments);
            Assert.Equal(ChangeType.Transposition, result.Single().ChangeType);
        }
    }
}
