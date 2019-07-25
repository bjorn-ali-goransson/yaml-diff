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
        public void IdenticalSequencePasses()
        {
            var originalDocument = @"
                - lorem: ipsum
            ";
            var changedDocument = @"
                - lorem: ipsum
            ";

            var result = new SequenceNodeComparer().Compare(new Path(), (YamlSequenceNode)Parser.Parse(originalDocument), (YamlSequenceNode)Parser.Parse(changedDocument));

            Assert.Empty(result);
        }

        [Fact]
        public void ChangedElementsPasses()
        {
            var originalDocument = @"
                - lorem: ipsum
            ";
            var changedDocument = @"
                - lorem: dolor
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
            Assert.Equal(Parser.Parse("dolor: sit"), result.Single().Node);
        }

        [Fact]
        public void DetectTransposedChild()
        {
            var originalDocument = @"
                - lorem: ipsum
                - dolor: sit
                - amet: elit
            ";
            var changedDocument = @"
                - dolor: sit
                - amet: elit
                - lorem: ipsum
            ";

            var result = new SequenceNodeComparer().Compare(new Path(), (YamlSequenceNode)Parser.Parse(originalDocument), (YamlSequenceNode)Parser.Parse(changedDocument));

            Assert.Equal(3, result.Count());
            Assert.Equal(new object[] { 0 }, result.ElementAt(0).Path.Segments);
            Assert.Equal(ChangeType.Transposition, result.ElementAt(0).ChangeType);
            Assert.Equal(new object[] { 1 }, result.ElementAt(1).Path.Segments);
            Assert.Equal(ChangeType.ImplicitTransposition, result.ElementAt(1).ChangeType);
            Assert.Equal(new object[] { 2 }, result.ElementAt(2).Path.Segments);
            Assert.Equal(ChangeType.ImplicitTransposition, result.ElementAt(2).ChangeType);
        }

        [Fact]
        public void IgnoreTranspositionByAddition()
        {
            var originalDocument = @"
                - lorem: ipsum
                - amet: elit
                - dolor: sit
            ";
            var changedDocument = @"
                - lorem: ipsum
                - dolor: sit
            ";

            var result = new SequenceNodeComparer().Compare(new Path(), (YamlSequenceNode)Parser.Parse(originalDocument), (YamlSequenceNode)Parser.Parse(changedDocument));

            Assert.Equal(2, result.Count());
            Assert.Equal(new object[] { 1 }, result.ElementAt(0).Path.Segments);
            Assert.Equal(ChangeType.Addition, result.ElementAt(0).ChangeType);
            Assert.Equal(new object[] { 2 }, result.ElementAt(1).Path.Segments);
            Assert.Equal(ChangeType.ImplicitTransposition, result.ElementAt(1).ChangeType);
        }

        [Fact]
        public void IgnoreTranspositionByDeletion()
        {
            var originalDocument = @"
                - dolor: sit
            ";
            var changedDocument = @"
                - amet: elit
                - dolor: sit
            ";

            var result = new SequenceNodeComparer().Compare(new Path(), (YamlSequenceNode)Parser.Parse(originalDocument), (YamlSequenceNode)Parser.Parse(changedDocument));

            Assert.Equal(2, result.Count());
            Assert.Equal(new object[] { 0 }, result.ElementAt(0).Path.Segments);
            Assert.Equal(ChangeType.Deletion, result.ElementAt(0).ChangeType);
            Assert.Equal(new object[] { 0 }, result.ElementAt(1).Path.Segments);
            Assert.Equal(ChangeType.ImplicitTransposition, result.ElementAt(1).ChangeType);
        }
    }
}
