using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace YamlDiff.Tests
{
    public class PathTests
    {
        [Fact]
        public void StartsWith()
        {
            Assert.True(new Path("lorem", "ipsum", "dolor").StartsWith(new Path("lorem", "ipsum")));
            Assert.False(new Path("lorem", "ipsum").StartsWith(new Path("lorem", "ipsum", "dolor")));
        }
    }
}
