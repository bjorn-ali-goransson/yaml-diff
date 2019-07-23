using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlDiff
{
    public class NodeComparer : INodeComparer
    {
        public bool Compare(YamlNode a, YamlNode b)
        {
            if(a is YamlScalarNode scalarNodeA && b is YamlScalarNode scalarNodeB)
            {
                return scalarNodeA.Value == scalarNodeB.Value;
            }

            return false;
        }
    }
}
