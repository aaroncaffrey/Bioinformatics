using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Cluto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioinformaticsHelperLibrary;

namespace BioinformaticsHelperLibraryUnitTests
{
    [TestClass]
    public class UnitTestClutoMatrixFile
    {
        [TestMethod]
        public void TestZeroHalf()
        {
            var dm1 = new decimal[,]
            {
                {1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1}
            };

            var dm1r = new decimal[,]
            {
                {1, 1, 1, 1, 1},
                {0, 1, 1, 1, 1},
                {0, 0, 1, 1, 1},
                {0, 0, 0, 1, 1},
                {0, 0, 0, 0, 1}
            };

            var dm2 = ClutoMatrixFile.DiagonalZeroHalfMatrix(dm1, false);


            Console.WriteLine();
            Console.WriteLine("DM1: ");
            CommonMethods.PrintMatrix(dm1);

            Console.WriteLine();
            Console.WriteLine("DM2 expected result: ");
            CommonMethods.PrintMatrix(dm1r);

            Console.WriteLine();
            Console.WriteLine("DM2 actual result: ");
            CommonMethods.PrintMatrix(dm2);

            Assert.IsTrue(dm1r.Cast<decimal>().SequenceEqual(dm2.Cast<decimal>()));
        }
    }
}
