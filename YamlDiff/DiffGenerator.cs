using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class DiffGenerator : IDiffGenerator
    {
        INodeTraverser NodeTraverser { get; }
        INodeFinder NodeFinder { get; }
        INodeComparer NodeComparer { get; }

        public DiffGenerator(INodeTraverser nodeTraverser, INodeFinder nodeFinder, INodeComparer nodeComparer)
        {
            NodeTraverser = nodeTraverser;
            NodeFinder = nodeFinder;
            NodeComparer = nodeComparer;
        }

        public Diff Generate(YamlNode original, YamlNode changed)
        {
            var result = new List<Difference>();

            foreach (var position in NodeTraverser.Traverse(original))
            {
                var path = position.Path;
                var originalNode = position.Node;
                var changedNode = NodeFinder.Find(path, changed);

                if(!NodeComparer.Compare(originalNode, changedNode))
                {
                    result.Add(new Difference(path));
                }
            }

            return new Diff(result);
        }
    }
}
