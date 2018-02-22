using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio.Algorithms.Assembly.Padena.Scaffold.ContigOverlapGraph;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using BioinformaticsHelperLibrary.Measurements;
using BioinformaticsHelperLibrary.TypeConversions;
using BioinformaticsHelperLibrary.UserProteinInterface;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace BioinformaticsHelperLibrary.Upgma
{
    public static class UpgmaClustering
    {
        public static MatrixPosition2D UpgmaLowestDistanceIndexes(List<List<decimal>> distanceMatrix)
        {
            var lowestValue = decimal.MaxValue;
            var lowestIndexes = new MatrixPosition2D();

            for (var indexX = 0; indexX < distanceMatrix.Count; indexX++)
            {
                for (var indexY = 0; indexY < distanceMatrix[indexX].Count; indexY++)
                {
                    if (indexX == indexY) continue;
                    if (indexY < indexX) continue;

                    var value = distanceMatrix[indexX][indexY];

                    if (value < lowestValue)
                    {
                        lowestValue = value;
                        lowestIndexes.X = indexX;
                        lowestIndexes.Y = indexY;
                    }
                }
            }

            return lowestIndexes;
        }

        /*public static decimal[,] DeleteDistanceMatrixIndex(decimal[,] distanceMatrix, int indexToDelete)
        {
            var matrix = new decimal[distanceMatrix.GetLength(0) - 1, distanceMatrix.GetLength(1) - 1];

            for (var x = 0; x < matrix.GetLength(0); x++)
            {
                for (var y = 0; y < matrix.GetLength(1); y++)
                {
                    matrix[x, y] = distanceMatrix[x >= indexToDelete ? x + 1 : x, y >= indexToDelete ? y + 1 : y];
                }
            }

            return matrix;
        }

        public static decimal[,] AddDistanceMatrixIndex(decimal[,] distanceMatrix, decimal[] indexToAdd)//, int indexPositionToInsert)
        {
            var matrix = new decimal[distanceMatrix.GetLength(0) + 1, distanceMatrix.GetLength(1) + 1];

            for (var x = 0; x < distanceMatrix.GetLength(0); x++)
            {
                for (var y = 0; y < distanceMatrix.GetLength(1); y++)
                {
                    matrix[x, y] = distanceMatrix[x, y];

                }
            }

            for (var index = 0; index < indexToAdd.Length; index++)
            {
                matrix[matrix.GetLength(0) - 1, index] = indexToAdd[index];
                matrix[index, matrix.GetLength(1) - 1] = indexToAdd[index];
            }

            return matrix;
        }

        public static decimal[] DeleteArrayIndex(decimal[] array, int indexToRemove)
        {
            var result = new decimal[array.Length - 1];

            for (var index = 0; index < result.Length; index++)
                result[index] = array[index >= indexToRemove ? index + 1 : index];

            return result;
        }*/

        public static List<List<decimal>> UpgmaDistanceMatrixNextIteration(List<List<decimal>> distanceMatrix, MatrixPosition2D lowestIndexes)
        {
            // find shortest distance, return index of shortest distance, recalc matrix with mean average

            //var distanceMatrix2 = new decimal[distanceMatrix.GetLength(0), distanceMatrix.GetLength(1)];
            //Array.Copy(distanceMatrix, distanceMatrix2, distanceMatrix.Length);

            var average = new decimal[distanceMatrix.Count];

            for (var index = 0; index < distanceMatrix.Count; index++)
            {
                var valueX = distanceMatrix[index][lowestIndexes.X];
                var valueY = distanceMatrix[index][lowestIndexes.Y];

                average[index] = (valueX + valueY) / 2;
            }

            var indexToReplace = lowestIndexes.X <= lowestIndexes.Y ? lowestIndexes.X : lowestIndexes.Y;
            var indexToDelete = lowestIndexes.X > lowestIndexes.Y ? lowestIndexes.X : lowestIndexes.Y;

            Console.WriteLine(indexToReplace);
            Console.WriteLine(indexToDelete);
            for (var index = 0; index < distanceMatrix.Count; index++)
            {
                distanceMatrix[index][indexToReplace] = average[index];
                distanceMatrix[indexToReplace][index] = average[index];
            }

            if (indexToReplace != indexToDelete)
            {
                foreach (var submatrix in distanceMatrix)
                {
                    submatrix.RemoveAt(indexToDelete);    
                }

                distanceMatrix.RemoveAt(indexToDelete);
            }

            return distanceMatrix;
        }

        public static decimal DistanceNodeToFinalParent(UpgmaNode node, int recursiveLevel = 0)
        {
            if (node.ParentNodeA != null)
            {
                var r = node.DistanceParentNodeA + DistanceNodeToFinalParent(node.ParentNodeA, recursiveLevel + 1);

                if (recursiveLevel == 0)
                {
                    //Console.WriteLine(String.Join("_", node.NodeIdList) + " distance to final parent: " + r);
                }

                return r;
            }

            if (node.ParentNodeB != null)
            {
                var r = node.DistanceParentNodeB + DistanceNodeToFinalParent(node.ParentNodeB, recursiveLevel + 1);

                if (recursiveLevel == 0)
                {
                    //Console.WriteLine(String.Join("_", node.NodeIdList) + " distance to final parent: " + r);
                }

                return r;
            }

            if (!node.IsLeafNode())
            {
                throw new Exception("Node has no parents but is not a leaf node");
            }

            return 0;
        }

        /*public static void OutputUpgmaLog(string outputFilename, List<UpgmaNode> nodeList, List<decimal[,]> distanceMatrixCache, List<List<List<int>>> distanceMatrixMapCache)
        {
            var sb = new StringBuilder();

            for (int index = 0; index < nodeList.Count; index++)
            {
                var node = nodeList[index];

                var nodeId = string.Join("_", node.VectorIndexes);
                var childNodeId = node.ChildNode != null ? string.Join("_", node.ChildNode.VectorIndexes) : "null";
                var parentNodeIdA = node.ParentNodeA != null ? string.Join("_", node.ParentNodeA.VectorIndexes) : "null";
                var parentNodeIdB = node.ParentNodeB != null ? string.Join("_", node.ParentNodeB.VectorIndexes) : "null";

                sb.AppendLine("Node #" + index + ": " + nodeId);
                sb.AppendLine("Is leaf node: " + node.IsLeafNode());
                sb.AppendLine("Is root node: " + node.IsRootNode());
                sb.AppendLine("Is branch node: " + node.IsBranchNode());
                sb.AppendLine("Is unclustered node: " + node.IsNodeUnclustered());
                sb.AppendLine("Loop iteration: " + node.DistanceMatrixIterationNumber);
                sb.AppendLine("Child node: " + childNodeId + ". Distance: " + node.DistanceChildNode);
                sb.AppendLine("Parent node 1: " + parentNodeIdA + ". Distance: " + node.DistanceParentNodeA);
                sb.AppendLine("Parent node 2: " + parentNodeIdB + ". Distance: " + node.DistanceParentNodeB);
                sb.AppendLine();
            }

            for (int index = 0; index < distanceMatrixCache.Count; index++)
            {
                var distanceMatrix = distanceMatrixCache[index];

                var distanceMatrixMap = distanceMatrixMapCache[index];

                sb.AppendLine("Distance Matrix Iteration #" + index + ". " + distanceMatrix.GetLength(0) + " x " + distanceMatrix.GetLength(1) + " matrix:");


                sb.AppendLine();

                for (var matrixIndex = 0; matrixIndex < distanceMatrixMap.Count; matrixIndex++)
                {
                    var matrixIndexIdList = distanceMatrixMap[matrixIndex];

                    sb.AppendLine("Matrix index #" + matrixIndex + ": " + string.Join(", ", matrixIndexIdList));
                }

                sb.AppendLine();

                var width = distanceMatrix.Cast<decimal>().Select(a => a.ToString().Length).Max() + 1;

                if (distanceMatrix.GetLength(0).ToString().Length + 1 > width) width = distanceMatrix.GetLength(0).ToString().Length + 1;
                if (distanceMatrix.GetLength(1).ToString().Length + 1 > width) width = distanceMatrix.GetLength(1).ToString().Length + 1;

                sb.Append("".PadLeft(width));
                for (var x = 0; x < distanceMatrix.GetLength(0); x++)
                {
                    sb.Append(x.ToString().PadLeft(width));
                }

                sb.AppendLine();

                for (var y = 0; y < distanceMatrix.GetLength(1); y++)
                {
                    sb.Append(y.ToString().PadLeft(width));

                    for (var x = 0; x < distanceMatrix.GetLength(0); x++)
                    {
                        string s = distanceMatrix[x, y].ToString().PadLeft(width);
                        sb.Append(s);
                    }

                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            File.WriteAllText(outputFilename, sb.ToString());
        }*/

        /*public static void CacheUpgmaMatrixes(decimal[,] distanceMatrix, List<decimal[,]> distanceMatrixCache, List<List<int>> distanceMatrixMap, List<List<List<int>>> distanceMatrixMapCache)
        {
            // Copy distance matrix
            var distanceMatrixCacheCopy = new decimal[distanceMatrix.GetLength(0), distanceMatrix.GetLength(1)];
            Array.Copy(distanceMatrix, distanceMatrixCacheCopy, distanceMatrix.Length);

            // Cache distance matrix
            distanceMatrixCache.Add(distanceMatrixCacheCopy);


            // Copy matrix map
            var distanceMatrixMapCacheCopy = new List<List<int>>();
            for (var i = 0; i < distanceMatrixMap.Count; i++)
            {
                distanceMatrixMapCacheCopy.Add(new List<int>());
                distanceMatrixMapCacheCopy[i].AddRange(distanceMatrixMap[i].Select(a => a).ToList());
            }

            // Cache matrix map
            distanceMatrixMapCache.Add(distanceMatrixMapCacheCopy);
        }*/

        public static void Upgma(decimal[,] distanceMatrix, List<string> vectorNames, int minimumOutputTreeLeafs,
            out List<List<UpgmaNode>> nodeListList, out List<List<string>> treeListList,
            out List<string> finalTreeLeafOrderList, bool newickTreeEveryIteration = false,
            ProgressActionSet progressActionSet = null)
        {
            if (distanceMatrix == null || distanceMatrix.GetLength(0) == 0 || distanceMatrix.GetLength(1) == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(distanceMatrix), "distance matrix is null or empty");
            }

            Upgma(ConvertTypes.Decimal2DArrayToDecimalListList(distanceMatrix), vectorNames, minimumOutputTreeLeafs, out nodeListList, out treeListList, out finalTreeLeafOrderList, newickTreeEveryIteration, progressActionSet);
        }

        public static void Upgma(List<List<decimal>> distanceMatrix, List<string> vectorNames, int minimumOutputTreeLeafs, out List<List<UpgmaNode>> nodeListList, out List<List<string>> treeListList, out List<string> finalTreeLeafOrderList, bool newickTreeEveryIteration = false, ProgressActionSet progressActionSet = null)
        {
            if (distanceMatrix == null || distanceMatrix.Count == 0 || distanceMatrix.Select(a=>a.Count).Contains(0))
            {
                throw new ArgumentOutOfRangeException(nameof(distanceMatrix), "distance matrix is null or empty");
            }

            //var makeIterationTrees = false;

            //var distanceMatrixCache = new List<decimal[,]>();

            nodeListList = new List<List<UpgmaNode>>();
            var nodeList = new List<UpgmaNode>();
            treeListList = new List<List<string>>();

            finalTreeLeafOrderList=new List<string>();

            // store a list of every index merged to each index position in the distance matrix
            var distanceMatrixMap = new List<List<int>>();
            //var distanceMatrixMapCache = new List<List<List<int>>>();

            for (var matrixIndex = 0; matrixIndex < distanceMatrix.Count; matrixIndex++)
            {
                distanceMatrixMap.Add(new List<int>());
                distanceMatrixMap[matrixIndex].Add(matrixIndex);

                var node = new UpgmaNode();
                node.VectorIndexes.Add(matrixIndex);
                //node.IsLeafNode = true;
                node.CopyPropertiesToGenericNodeProperties();
                nodeList.Add(node);
            }


            int itemsCompleted = 0;
            int itemsTotal = distanceMatrix.Count;

            var startTicks = DateTime.Now.Ticks;


            ProgressActionSet.StartAction(itemsTotal, progressActionSet);

            while (distanceMatrix.Count > 1 && distanceMatrix.Select(a=>a.Count).Max() > 1)
            {

                // find which indexes to join
                var lowestIndexes = UpgmaLowestDistanceIndexes(distanceMatrix);
                var lowestValue = distanceMatrix[lowestIndexes.X][lowestIndexes.Y];
                var lowerIndex = lowestIndexes.X < lowestIndexes.Y ? lowestIndexes.X : lowestIndexes.Y;
                var higherIndex = lowestIndexes.X > lowestIndexes.Y ? lowestIndexes.X : lowestIndexes.Y;
                var nodeDistance = lowestValue / 2;

                if (lowerIndex == higherIndex)
                {
                    throw new Exception("lower index and higher index have the same value");
                }

                // Take parent node identities 
                var parentNodeIdA = distanceMatrixMap[lowerIndex].OrderBy(o => o).ToList();
                var parentNodeIdB = distanceMatrixMap[higherIndex].OrderBy(o => o).ToList();

                var childNodeId = new List<int>();
                childNodeId.AddRange(parentNodeIdA);
                childNodeId.AddRange(parentNodeIdB);
                childNodeId = childNodeId.Distinct().ToList();

                // Find if parent nodes already exist
                var parentNodeCandidatesA = nodeList.Where(a => a.VectorIndexes.OrderBy(o => o).SequenceEqual(parentNodeIdA)).ToList();
                var parentNodeCandidatesB = nodeList.Where(a => a.VectorIndexes.OrderBy(o => o).SequenceEqual(parentNodeIdB)).ToList();

                UpgmaNode parentNodeA;
                parentNodeA = parentNodeCandidatesA[0];

                UpgmaNode parentNodeB;
                parentNodeB = parentNodeCandidatesB[0];


                var childNode = new UpgmaNode();
                nodeList.Add(childNode);

                parentNodeA.ChildNode = childNode;
                parentNodeB.ChildNode = childNode;
                childNode.ParentNodeA = parentNodeA;
                childNode.ParentNodeB = parentNodeB;

                var parentTotalDistanceA = DistanceNodeToFinalParent(parentNodeA);
                var parentTotalDistanceB = DistanceNodeToFinalParent(parentNodeB);

                parentNodeA.DistanceChildNode = nodeDistance - parentTotalDistanceA;
                parentNodeB.DistanceChildNode = nodeDistance - parentTotalDistanceB;
                childNode.DistanceParentNodeA = nodeDistance - parentTotalDistanceA;
                childNode.DistanceParentNodeB = nodeDistance - parentTotalDistanceB;

                childNode.DistanceMatrixIterationNumber = itemsCompleted;// distanceMatrixCache.Count - 1;
                childNode.VectorIndexes = childNodeId;

                parentNodeA.CopyPropertiesToGenericNodeProperties();
                parentNodeB.CopyPropertiesToGenericNodeProperties();
                childNode.CopyPropertiesToGenericNodeProperties();
                // rearrange the matrix map with the new indexes joined distanceMatrixMap[higherIndex]
                distanceMatrixMap[lowerIndex].AddRange(distanceMatrixMap[higherIndex]);
                distanceMatrixMap.RemoveAt(higherIndex);

                // recalculate distance matrix with indexes joined with mean average
                distanceMatrix = UpgmaDistanceMatrixNextIteration(distanceMatrix, lowestIndexes);

                if (newickTreeEveryIteration)
                {
                    List<string> treeLeafOrderList;
                    var iterationTree = Newick.NewickTreeFormat(nodeList.ToList<GenericNode>(), vectorNames, out treeLeafOrderList, minimumOutputTreeLeafs);
                    treeListList.Add(iterationTree);
                    finalTreeLeafOrderList = treeLeafOrderList;
                    nodeListList.Add(UpgmaNode.CopyNetwork(nodeList));
                }

                itemsCompleted++;

                ProgressActionSet.ProgressAction(1, progressActionSet);
                ProgressActionSet.EstimatedTimeRemainingAction(startTicks, itemsCompleted, itemsTotal, progressActionSet);
                
            }

            if (!newickTreeEveryIteration)
            {
                List<string> treeLeafOrderList;
                var iterationTree = Newick.NewickTreeFormat(nodeList.ToList<GenericNode>(), vectorNames, out treeLeafOrderList, minimumOutputTreeLeafs);
                treeListList.Add(iterationTree);
                finalTreeLeafOrderList = treeLeafOrderList;
                nodeListList.Add(UpgmaNode.CopyNetwork(nodeList));
            }

            ProgressActionSet.FinishAction(true, progressActionSet);
        }
    }
}
