using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.TypeConversions;
using BioinformaticsHelperLibrary.Upgma;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BioinformaticsHelperLibraryUnitTests
{
    [TestClass]
    public class UnitTestUpgma
    {
        /*[TestMethod]
        public void TestAddDistanceMatrixIndex()
        {
            decimal[,] distanceMatrix1 =
            {
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
            };

            decimal[,] distanceMatrix2 =
            {
                { 0, 1, 2, 3, 4, 0 },
                { 0, 1, 2, 3, 4, 1 },
                { 0, 1, 2, 3, 4, 2 },
                { 0, 1, 2, 3, 4, 3 },
                { 0, 1, 2, 3, 4, 4 },
                { 0, 1, 2, 3, 4, 5 }
            };

            decimal[] add = { 0, 1, 2, 3, 4, 5 };

            var result = UpgmaClustering.AddDistanceMatrixIndex(distanceMatrix1, add);

            Assert.IsFalse(distanceMatrix1.Cast<decimal>().SequenceEqual(result.Cast<decimal>()));
            Assert.IsTrue(distanceMatrix2.Cast<decimal>().SequenceEqual(result.Cast<decimal>()));
        }*/

        /*[TestMethod]
        public void TestDeleteDistanceMatrixIndex()
        {
            decimal[,] distanceMatrix1 =
            {
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
            };

            decimal[,] distanceMatrix2 =
            {
              //{ /*0,* / 1, 2, 3, 4 },
                { /*0,* / 1, 2, 3, 4 },
                { /*0,* / 1, 2, 3, 4 },
                { /*0,* / 1, 2, 3, 4 },
                { /*0,* / 1, 2, 3, 4 },
            };

            decimal[,] distanceMatrix3 =
            {
                { 0, /*1,* / 2, 3, 4 },
              //{ 0, /*1,* / 2, 3, 4 },
                { 0, /*1,* / 2, 3, 4 },
                { 0, /*1,* / 2, 3, 4 },
                { 0, /*1,* / 2, 3, 4 },
            };

            decimal[,] distanceMatrix4 =
            {
                { 0, 1, 2, 3, /*4* / },
                { 0, 1, 2, 3, /*4* / },
                { 0, 1, 2, 3, /*4* / },
                { 0, 1, 2, 3, /*4* / },
              //{ 0, 1, 2, 3, /*4* / },
            };

            var result1 = UpgmaClustering.DeleteDistanceMatrixIndex(distanceMatrix1, 0);
            var result2 = UpgmaClustering.DeleteDistanceMatrixIndex(distanceMatrix1, 1);
            var result3 = UpgmaClustering.DeleteDistanceMatrixIndex(distanceMatrix1, 5);

            //Assert.IsFalse(distanceMatrix1.Cast<decimal>().SequenceEqual(distanceMatrix2.Cast<decimal>()));

            Assert.IsTrue(distanceMatrix2.Cast<decimal>().SequenceEqual(result1.Cast<decimal>()));
            Assert.IsTrue(distanceMatrix3.Cast<decimal>().SequenceEqual(result2.Cast<decimal>()));
            Assert.IsTrue(distanceMatrix4.Cast<decimal>().SequenceEqual(result3.Cast<decimal>()));
        }*/

        [TestMethod]
        public void TestUpgmaLowestDistanceIndexes()
        {
            decimal[,] distanceMatrix1 =
            {
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
                { 0, 1, 2, 3, 4 },
            };

            decimal[,] distanceMatrix2 =
            {
                { 10, 1, 2, 3, 4 },
                { 10, 1, 2, 3, 4 },
                { 10, 1, 2, 0, 4 },
                { 10, 1, 2, 3, 4 },
                { 10, 1, 2, 3, 4 },
            };

            decimal[,] distanceMatrix3 =
            {
                { 10, 1, 2, 3, 4 },
                { 10, 1, 2, 3, 4 },
                { 10, 1, 2, 3, 4 },
                { 10, 1, 2, 3, 0 },
                { 10, 1, 2, 0, 0 },
            };

            var result1 = UpgmaClustering.UpgmaLowestDistanceIndexes(ConvertTypes.Decimal2DArrayToDecimalListList(distanceMatrix1));
            var result2 = UpgmaClustering.UpgmaLowestDistanceIndexes(ConvertTypes.Decimal2DArrayToDecimalListList(distanceMatrix2));
            var result3 = UpgmaClustering.UpgmaLowestDistanceIndexes(ConvertTypes.Decimal2DArrayToDecimalListList(distanceMatrix3));

            Assert.IsTrue(result1.X == 0 && result1.Y == 1);
            Assert.IsTrue(result2.X == 2 && result2.Y == 3);
            Assert.IsTrue(result3.X == 3 && result3.Y == 4);
        }



        [TestMethod]
        public void TestUpgmaDistanceMatrixNextIteration()
        {
            List<List<decimal>> distanceMatrix1 = new List<List<decimal>>()
            {
                new List<decimal>(){ 0, 0, 0, 0, 0 },
                new List<decimal>(){ 0, 0, 1, 1, 1 },
                new List<decimal>(){ 0, 1, 0, 2, 2 },
                new List<decimal>(){ 0, 1, 2, 0, 3 },
                new List<decimal>(){ 0, 1, 2, 3, 0 },
            };

            var expectedResult = new List<List<List<decimal>>>()
            {
                new List<List<decimal>>()
                {
                    new List<decimal>() {0.0m, 0.5m, 0.5m, 0.5m},
                    new List<decimal>() {0.5m, 0.0m, 2.0m, 2.0m},
                    new List<decimal>() {0.5m, 2.0m, 0.0m, 3.0m},
                    new List<decimal>() {0.5m, 2.0m, 3.0m, 0.0m}
                },

                new List<List<decimal>>()
                {
                    new List<decimal>() {0.25m, 1.25m, 1.25m},
                    new List<decimal>() {1.25m, 0.00m, 3.00m},
                    new List<decimal>() {1.25m, 3.00m, 0.00m},
                },

                new List<List<decimal>>()
                {
                    new List<decimal>() {0.750m, 2.125m},
                    new List<decimal>() {2.125m, 0.000m},
                },

                new List<List<decimal>>()
                {
                    new List<decimal>() {1.4375m},

                }
            };
            Console.WriteLine();
            Console.WriteLine("Sample matrix:");
            CommonMethods.PrintMatrix(distanceMatrix1);
            Console.WriteLine();

            for (var i = 0; i < 4; i++)
            {
                Console.WriteLine("Upgma iteration #" + i);

                var ind1 = UpgmaClustering.UpgmaLowestDistanceIndexes(distanceMatrix1);
                var result1 = UpgmaClustering.UpgmaDistanceMatrixNextIteration(distanceMatrix1, ind1);

                Console.WriteLine();
                Console.WriteLine("expected result:");
                CommonMethods.PrintMatrix(expectedResult[i]);
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("actual result:");
                CommonMethods.PrintMatrix(result1);
                Console.WriteLine();

                Console.WriteLine();

                for (var x = 0; x < expectedResult[i].Count; x++)
                    for (var y = 0; y < expectedResult[i][x].Count; y++)
                        Assert.IsTrue(expectedResult[i][x][y] == result1[x][y]);

            }

        }

        public static void CheckUpgmaDistanceConsistencyRecursive(UpgmaNode node, decimal c = 0, decimal p = 0)
        {



            if (node.IsLeafNode() || node.IsRootNode())
            {
                c = c + node.DistanceChildNode;
                Console.WriteLine(string.Join("_", node.VectorIndexes) + ": is leaf node: " + node.IsLeafNode() + ". is root node: " + node.IsRootNode() + ".");
                Console.WriteLine("- child: " + c);
                Console.WriteLine("- parent: " + p);
                Console.WriteLine();
                return;
            }

            CheckUpgmaDistanceConsistencyRecursive(node.ParentNodeA, c + node.DistanceChildNode, p + node.DistanceParentNodeA);
            CheckUpgmaDistanceConsistencyRecursive(node.ParentNodeB, c + node.DistanceChildNode, p + node.DistanceParentNodeB);

        }

        public static void CheckUpgmaDistanceConsistency(List<UpgmaNode> nodeList)
        {
            // (((A,B)AB,(C,D)CD)ABC)ABCD

            // Find root node
            var finalNodeCandidates = nodeList.Where(a => a.VectorIndexes.Count == nodeList.Select(b => b.VectorIndexes.Count).Max()).ToList();
            var finalNode = finalNodeCandidates[0];

            CheckUpgmaDistanceConsistencyRecursive(finalNode);
        }

        [TestMethod]
        public void TestUpgma()
        {
            //decimal[,] distanceMatrix1 =
            //{
            //    { 0, 0, 0, 0, 0 },
            //    { 0, 0, 1, 1, 1 },
            //    { 0, 1, 0, 2, 2 },
            //    { 0, 1, 2, 0, 3 },
            //    { 0, 1, 2, 3, 0 },
            //};

            //var result = UpgmaClustering.Upgma(distanceMatrix1);

            decimal[,] distanceMatrix2 =
            {
                { 0, 19, 27, 8, 33, 18, 13 },
                { 19, 0, 31, 18, 36, 1, 13 },
                { 27, 31, 0, 26, 41, 32, 29 },
                { 8, 18, 26, 0, 31, 17, 14 },
                { 33, 36, 41, 31, 0, 35, 28 },
                { 18, 1, 32, 17, 35, 0, 12 },
                { 13, 13, 29, 14, 28, 12, 0 }
            };

            var names = new List<string>();
            for (var i = 0; i < distanceMatrix2.GetLength(0); i++)
            {
                names.Add("" + i);
            }


            List<string> vectorNames = new List<string>();
            var maxVectorLength = distanceMatrix2.GetLength(0) > distanceMatrix2.GetLength(1) ? distanceMatrix2.GetLength(0) : distanceMatrix2.GetLength(1);
            for (var i = 0; i < maxVectorLength; i++)
            {
                vectorNames.Add(SpreadsheetFileHandler.AlphabetLetterRollOver(i) + i);
            }

            List<List<UpgmaNode>> nodeList;
            List<List<string>> treeList;

            var minimumOutputTreeLeafs = 1;

            List<string> finalTreeLeafOrderList;
            UpgmaClustering.Upgma(distanceMatrix2, vectorNames, minimumOutputTreeLeafs, out nodeList, out treeList, out finalTreeLeafOrderList);

            CheckUpgmaDistanceConsistency(nodeList[nodeList.Count - 1]);

            List<string> treeLeafOrderList;
            var tree = Newick.NewickTreeFormat(nodeList[nodeList.Count - 1].ToList<GenericNode>(), names, out treeLeafOrderList, minimumOutputTreeLeafs);

            Console.WriteLine(tree);
        }
    }
}
