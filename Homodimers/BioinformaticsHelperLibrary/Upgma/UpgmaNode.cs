using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Math;

namespace BioinformaticsHelperLibrary.Upgma
{
    public class UpgmaNode : GenericNode
    {
        public int DistanceMatrixIterationNumber = 0;

        public UpgmaNode ParentNodeA = null;
        public decimal DistanceParentNodeA = 0;

        public UpgmaNode ParentNodeB = null;
        public decimal DistanceParentNodeB = 0;

        public UpgmaNode ChildNode = null;
        public decimal DistanceChildNode = 0;

        public UpgmaNode()
        {
            
        }

        public UpgmaNode(int distanceMatrixIterationNumber, UpgmaNode parentNodeA, decimal distanceParentNodeA, UpgmaNode parentNodeB, decimal distanceParentNodeB, UpgmaNode childNode, decimal distanceChildNode)
        {
            DistanceMatrixIterationNumber = distanceMatrixIterationNumber;
            ParentNodeA = parentNodeA;
            DistanceParentNodeA = distanceParentNodeA;
            ParentNodeB = parentNodeB;
            DistanceParentNodeB = distanceParentNodeB;
            ChildNode = childNode;
            DistanceChildNode = distanceChildNode;
        }

        public static List<UpgmaNode> CopyNetwork(List<UpgmaNode> network)
        {
            var networkCopy = new List<UpgmaNode>();

            for (int index = 0; index < network.Count; index++)
            {
                var node = network[index];
                var nodeCopy = new UpgmaNode();
                nodeCopy.VectorIndexes = new List<int>(node.VectorIndexes);
                networkCopy.Add(nodeCopy);
            }

            for (int index = 0; index < network.Count; index++)
            {
                var node = network[index];
                var nodeCopy = networkCopy[index];

                if (node.ChildNode != null) nodeCopy.ChildNode = networkCopy[network.IndexOf(node.ChildNode)];
                if (node.ParentNodeA != null) nodeCopy.ParentNodeA = networkCopy[network.IndexOf(node.ParentNodeA)];
                if (node.ParentNodeB != null) nodeCopy.ParentNodeB = networkCopy[network.IndexOf(node.ParentNodeB)];
                nodeCopy.DistanceChildNode = node.DistanceChildNode;
                nodeCopy.DistanceParentNodeA = node.DistanceParentNodeA;
                nodeCopy.DistanceParentNodeB = node.DistanceParentNodeB;
                nodeCopy.DistanceMatrixIterationNumber = node.DistanceMatrixIterationNumber;

                foreach (var parentNode in node.ParentList)
                {
                    UpgmaNode parentNodeCopy = null;
                    if (parentNode.Node != null) parentNodeCopy = networkCopy[network.IndexOf((UpgmaNode) parentNode.Node)];

                    nodeCopy.ParentList.Add(new GenericNodeWithDistance(parentNodeCopy, parentNode.DistanceToNode));
                }

                foreach (var childNode in node.ChildrenList)
                {
                    UpgmaNode childNodeCopy = null;
                    if (childNode.Node != null) childNodeCopy = networkCopy[network.IndexOf((UpgmaNode) childNode.Node)];

                    nodeCopy.ChildrenList.Add(new GenericNodeWithDistance(childNodeCopy, childNode.DistanceToNode));
                }
            }

            return networkCopy;

        }


        public void CopyPropertiesToGenericNodeProperties()
        {
            if (ParentNodeA != null)
            {
                var genericParentA = new GenericNodeWithDistance(ParentNodeA, DistanceParentNodeA);
                if (!ParentList.Contains(genericParentA))
                {
                    ParentList.Add(genericParentA);
                }
            }

            if (ParentNodeB != null)
            {
                var genericParentB = new GenericNodeWithDistance(ParentNodeB, DistanceParentNodeB);
                if (!ParentList.Contains(genericParentB))
                {
                    ParentList.Add(genericParentB);
                }
            }

            if (ChildNode != null)
            {
                var genericChild = new GenericNodeWithDistance(ChildNode, DistanceChildNode);
                if (!ChildrenList.Contains(genericChild))
                {
                    ChildrenList.Add(genericChild);
                }
            }

        }
    }
}
