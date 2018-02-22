using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace BioinformaticsHelperLibrary.Upgma
{

    public class GenericNode
    {
        /// <summary>
        /// Parents of this node
        /// </summary>
        public List<GenericNodeWithDistance> ParentList = new List<GenericNodeWithDistance>();

        /// <summary>
        /// Children of this node
        /// </summary>
        public List<GenericNodeWithDistance> ChildrenList = new List<GenericNodeWithDistance>();
        
        /// <summary>
        /// The indexes of the 1d vector (for example, column or row number) which this node was calculated or derived from
        /// </summary>
        public List<int> VectorIndexes = new List<int>();

        public static List<GenericNode> CopyNetwork(List<GenericNode> genericNodeList)
        {
            var networkCopy = new List<GenericNode>();

            for (int index = 0; index < genericNodeList.Count; index++)
            {
                var node = genericNodeList[index];
                var nodeCopy = new GenericNode();
                nodeCopy.VectorIndexes = new List<int>(node.VectorIndexes);
                networkCopy.Add(nodeCopy);
            }

            for (int index = 0; index < genericNodeList.Count; index++)
            {
                var node = genericNodeList[index];
                var nodeCopy = networkCopy[index];

                foreach (var parentNode in node.ParentList)
                {
                    var parentNodeIndex = genericNodeList.IndexOf(parentNode.Node);

                    var parentNodeCopy = networkCopy[parentNodeIndex];

                    nodeCopy.ParentList.Add(new GenericNodeWithDistance(parentNodeCopy, parentNode.DistanceToNode));
                }

                foreach (var childNode in node.ChildrenList)
                {
                    var childNodeIndex = genericNodeList.IndexOf(childNode.Node);

                    var childNodeCopy = networkCopy[childNodeIndex];

                    nodeCopy.ParentList.Add(new GenericNodeWithDistance(childNodeCopy, childNode.DistanceToNode));
                }
            }

            return networkCopy;
        }

        public bool IsRootNode()
        {
            return ChildrenList == null || ChildrenList.Count == 0 || ChildrenList.Count(m => m.Node != null) == 0;
        }

        public bool IsLeafNode()
        {
            return ParentList == null || ParentList.Count == 0 || ParentList.Count(m => m.Node != null) == 0;
        }

        public bool IsBranchNode()
        {
            return (ParentList != null && ParentList.Count > 0) && (ChildrenList != null && ChildrenList.Count > 0);
        }

        public bool IsNodeUnclustered()
        {
            return (ParentList == null || ParentList.Count == 0) && (ChildrenList == null || ChildrenList.Count == 0);
        }


        public List<GenericNode> GetNodeTreeLeafs(List<GenericNode> nodeList)
        {
            var leafNodeList = nodeList.Where(n => n.VectorIndexes.Count == 1 && n.ParentList.Count == 0 && n.ChildrenList.Count > 0 && this.VectorIndexes.Contains(n.VectorIndexes[0])).ToList();

            return leafNodeList;
        }

        public static List<GenericNode> GetRootNodeList(List<GenericNode> nodeList)
        {
            var rootNodeList = nodeList.Where(n => (n.ChildrenList == null || n.ChildrenList.Count == 0 || n.ChildrenList.Count(m => m.Node != null) == 0) && (n.ParentList.Count > 0)).ToList();

            return rootNodeList;
        }

    }
}
