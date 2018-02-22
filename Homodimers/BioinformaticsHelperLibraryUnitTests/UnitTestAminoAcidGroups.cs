using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.AminoAcidGroups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BioinformaticsHelperLibraryUnitTests
{
    [TestClass]
    public class UnitTestAminoAcidGroups
    {
        [TestMethod]
        public void TestAminoAcidGroups()
        {
            for (var ruleIndex = 0; ruleIndex < AminoAcidGroups.GetTotalGroups(); ruleIndex++)
            {
                var groups = AminoAcidGroups.GetSubgroupAminoAcidsCodesStrings((AminoAcidGroups.EnumAminoAcidGroups)ruleIndex);

                for (var i = 0; i < groups.Length; i++)
                {
                    Console.WriteLine(ruleIndex + ": " + i + ": " + groups[i]);
                }
            }
        }
    }
}
