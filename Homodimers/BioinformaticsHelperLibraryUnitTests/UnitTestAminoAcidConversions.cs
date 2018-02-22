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
    public class UnitTestAminoAcidConversions
    {
        [TestMethod]
        public void TestListAminoAcidsByProperty()
        {
            var x = new AminoAcidProperties<string>()
            {
                //Acidic = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Acidic = true}),
                Aliphatic = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Aliphatic = true}),
                Aromatic = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Aromatic = true}),
                Charged = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Charged = true}),
                Hydrophobic = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Hydrophobic = true}),
                Hydroxylic = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Hydroxylic = true}),
                Negative = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Negative = true}),
                Polar = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Polar = true}),
                Positive = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Positive = true}),
                Small = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Small = true}),
                Sulphur = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {Sulphur = true}),
                Tiny = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() { Tiny = true }),
                
            };

            var uncommon = AminoAcidConversions.ListAminoAcidsByProperty(new AminoAcidProperties<bool>() {}, AminoAcidPropertyMatchType.AllMatch);

            var y = new AminoAcidProperties<string>()
            {

            };
            

            for (var i = 0; i < AminoAcidTotals.TotalAminoAcids(); i++)
            {
                var aa = AminoAcidConversions.AminoAcidNumberToAminoAcidObject(i + 1);

                //if (aa.Acidic) y.Acidic += aa.Code1L;
                if (aa.Aliphatic) y.Aliphatic += aa.Code1L;
                if (aa.Aromatic) y.Aromatic += aa.Code1L;
                if (aa.Charged) y.Charged += aa.Code1L;
                if (aa.Hydrophobic) y.Hydrophobic += aa.Code1L;
                if (aa.Hydroxylic) y.Hydroxylic += aa.Code1L;
                if (aa.Negative) y.Negative += aa.Code1L;
                if (aa.Polar) y.Polar += aa.Code1L;
                if (aa.Positive) y.Positive += aa.Code1L;
                if (aa.Small) y.Small += aa.Code1L;
                if (aa.Sulphur) y.Sulphur += aa.Code1L;
                if (aa.Tiny) y.Tiny += aa.Code1L;
            }

            //Console.WriteLine("Acidic: " + x.Acidic);
            Console.WriteLine("Aliphatic: " + x.Aliphatic);
            Console.WriteLine("Aromatic: " + x.Aromatic);
            Console.WriteLine("Charged: " + x.Charged);
            Console.WriteLine("Hydrophobic: " + x.Hydrophobic);
            Console.WriteLine("Hydroxylic: " + x.Hydroxylic);
            Console.WriteLine("Negative: " + x.Negative);
            Console.WriteLine("Polar: " + x.Polar);
            Console.WriteLine("Positive: " + x.Positive);
            Console.WriteLine("Small: " + x.Small);
            Console.WriteLine("Sulphur: " + x.Sulphur);
            Console.WriteLine("Tiny: " + x.Tiny);
            Console.WriteLine("Others: " + uncommon);

            //Assert.AreEqual(string.Join(" ", y.Acidic.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Acidic.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Aliphatic.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Aliphatic.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Aromatic.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Aromatic.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Charged.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Charged.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Hydrophobic.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Hydrophobic.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Hydroxylic.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Hydroxylic.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Negative.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Negative.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Polar.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Polar.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Positive.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Positive.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Small.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Small.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Sulphur.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Sulphur.OrderBy(a=>a).ToArray()));
            Assert.AreEqual(string.Join(" ", y.Tiny.OrderBy(a=>a).ToArray()), string.Join(" ",  x.Tiny.OrderBy(a=>a).ToArray()));
        }
    }
}
