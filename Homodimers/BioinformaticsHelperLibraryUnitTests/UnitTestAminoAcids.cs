using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.AminoAcids;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BioinformaticsHelperLibraryUnitTests
{
    [TestClass]
    public class UnitTestAminoAcids
    {
        [TestMethod]
        public void TestAminoAcids()
        {
            //int intAcidic = 0;
            int intAliphatic = 0;
            int intAromatic = 0;
            int intCharged = 0;
            int intHydrophobic = 0;
            int intHydroxylic = 0;
            int intNegative = 0;
            int intPolar = 0;
            int intPositive = 0;
            int intSmall = 0;
            int intSulphur = 0;
            int intTiny = 0;

            //string strAcidic = "";
            string strAliphatic = "";
            string strAromatic = "";
            string strCharged = "";
            string strHydrophobic = "";
            string strHydroxylic = "";
            string strNegative = "";
            string strPolar = "";
            string strPositive = "";
            string strSmall = "";
            string strSulphur = "";
            string strTiny = "";

            for (var i = 1; i <= 26; i++)
            {
                var x = AminoAcidConversions.AminoAcidNumberToAminoAcidObject(i);

                //if (x.Acidic)
                //{
                    //intAcidic++;
                    //strAcidic += x.Code1L;
                //}

                if (x.Aliphatic)
                {
                    intAliphatic++;
                    strAliphatic += x.Code1L;
                }

                if (x.Aromatic)
                {
                    intAromatic++;
                    strAromatic += x.Code1L;
                }

                if (x.Charged)
                {
                    intCharged++;
                    strCharged += x.Code1L;
                }

                if (x.Hydrophobic)
                {
                    intHydrophobic++;
                    strHydrophobic += x.Code1L;
                }

                if (x.Hydroxylic)
                {
                    intHydroxylic++;
                    strHydroxylic += x.Code1L;
                }

                if (x.Negative)
                {
                    intNegative++;
                    strNegative += x.Code1L;
                }

                if (x.Polar)
                {
                    intPolar++;
                    strPolar += x.Code1L;
                }

                if (x.Positive)
                {
                    intPositive++;
                    strPositive += x.Code1L;
                }

                if (x.Small)
                {
                    intSmall++;
                    strSmall += x.Code1L;
                }

                if (x.Sulphur)
                {
                    intSulphur++;
                    strSulphur += x.Code1L;
                }

                if (x.Tiny)
                {
                    intTiny++;
                    strTiny += x.Code1L;
                }
            }

            //Console.WriteLine("Acidic: " + intAcidic + " " + strAcidic);
            Console.WriteLine("Aliphatic: " + intAliphatic + " " + strAliphatic);
            Console.WriteLine("Aromatic: " + intAromatic + " " + strAromatic);
            Console.WriteLine("Charged: " + intCharged + " " + strCharged);
            Console.WriteLine("Hydrophobic: " + intHydrophobic + " " + strHydrophobic);
            Console.WriteLine("Hydroxylic: " + intHydroxylic + " " + strHydroxylic);
            Console.WriteLine("Negative: " + intNegative + " " + strNegative);
            Console.WriteLine("Polar: " + intPolar + " " + strPolar);
            Console.WriteLine("Positive: " + intPositive + " " + strPositive);
            Console.WriteLine("Small: " + intSmall + " " + strSmall);
            Console.WriteLine("Sulphur: " + intSulphur + " " + strSulphur);
            Console.WriteLine("Tiny: " + intTiny + " " + strTiny);            
        }
    }
}
