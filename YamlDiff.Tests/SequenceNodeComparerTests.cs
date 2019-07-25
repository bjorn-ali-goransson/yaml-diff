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
        public void DetectAddedNamedChild()
        {
            var originalDocument = @"
                - name: ipsum
                - name: dolor
                  sit: 1
            ";
            var changedDocument = @"
                - name: ipsum
            ";

            var result = new SequenceNodeComparer().Compare(new Path(), (YamlSequenceNode)Parser.Parse(originalDocument), (YamlSequenceNode)Parser.Parse(changedDocument));

            Assert.Single(result);
            Assert.Equal(new string[] { "dolor" }, result.Single().Path.Segments);
        }
    }
}
