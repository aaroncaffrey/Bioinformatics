using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bio;
using BioinformaticsHelperLibrary.AminoAcids;
//using BioinformaticsHelperLibrary.Cluto;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.Spreadsheets;
using BioinformaticsHelperLibrary.TypeConversions;
using BioinformaticsHelperLibrary.Upgma;
using BioinformaticsHelperLibrary.UserProteinInterface;
using System.IO.Compression;
using DocumentFormat.OpenXml.Office.CoverPageProps;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public static class TreeOutputs
    {
        private static readonly string[,] SheetNamesAndDescriptions = new string[,]
        {
            {"sheet legend", "this sheet - explanation of sheets in this spreadsheet"},

            {"zip 3d align", "list of zip files with protein proteinInterfaces for 3d alignment with multiprot"},

            {"motif s.", "list of suggested motifs based on the prospensity profiles"},
            {"motif hit", "distinct list of suggested motifs, how many times they were suggested and how many hits in the database"},

            { "motif profile", "profiles for more advanced motifs taking into consideration positional information in the proteinInterfaces"},

            {"tree v. aa ss", "list of all proteinInterfaces found, with their amino acids, secondary structure, interaction vectors, in the same order as the newick tree taxa"},

            {"proteinInterface aa prop.", "proteinInterface amino acid properties, groups coloured according to the colouring rules, property true/false vectors for each proteinInterface position"},
            {"proteinInterface int. aa prop.", "proteinInterface interactions amino acid properties, groups coloured according to the colouring rules, property true/false vectors for each proteinInterface position"},
            {"proteinInterface noint. aa prop.", "proteinInterface non interactions amino acid properties, groups coloured according to the colouring rules, property true/false vectors for each proteinInterface position"},

            {"symm.", "symmetry values for proteinInterface amino acids, proteinInterface interactions, interaction vectors, etc."},

            {"dist. ss", "standard distribution of amino acids within distinct binding proteinInterface secondary structures"},

            {"dist. s. len", "standard distribution of amino acids in proteinInterfaces according to proteinInterface length"},

            {"dist. v. short", "standard distribution of amino acids in proteinInterfaces within distinct short vectors"},
            {"dist. v. long", "standard distribution of amino acids in proteinInterfaces within distinct long vectors"},

            {"dist. v. short no", "standard distribution of amino acids in proteinInterfaces within distinct short vectors with non-proteinInterface interactions removed"},
            {"dist. v. long no", "standard distribution of amino acids in proteinInterfaces within distinct long vectors with non-proteinInterface interactions removed"},

            {"short v. patt.", "distinct list of every short vector found showing total counted, total fwd/rev, total chain a/b"},
            {"long v. patt.", "distinct list of every long vector found showing total counted, total fwd/rev, total chain a/b"},

            {"short v. patt. no", "distinct list of every short vector with non-proteinInterface interactions removed found showing total counted, total fwd/rev, total chain a/b"},
            {"long v. patt. no", "distinct list of every long vector with non-proteinInterface interactions removed found showing total counted, total fwd/rev, total chain a/b"},

            {"symm. m. patt. short v.", "distinct list of every short vector mirror symmetry value found showing total counted, total fwd/rev, total chain a/b"},
            {"symm. m. patt. long v.", "distinct list of every long vector mirror symmetry value found showing total counted, total fwd/rev, total chain a/b"},

            {"symm. m. patt. ss.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. int. ss.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. noint. ss.", "distinct list of every pattern's mirror symmetry percentage value"},

            {"symm. m. patt. seq. ", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. int. seq.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. noint. seq.", "distinct list of every pattern's mirror symmetry percentage value"},

            {"symm. m. patt. phys.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. int. phys.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. noint. phys.", "distinct list of every pattern's mirror symmetry percentage value"},

            {"symm. m. patt. hydr.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. int. hydr.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. noint. hydr.", "distinct list of every pattern's mirror symmetry percentage value"},

            {"symm. m. patt. pdbs.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. int. pdbs.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. noint. pdbs.", "distinct list of every pattern's mirror symmetry percentage value"},

            {"symm. m. patt. unip.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. int. unip.", "distinct list of every pattern's mirror symmetry percentage value"},
            {"symm. m. patt. noint. unip.", "distinct list of every pattern's mirror symmetry percentage value"},

            {"symm. c. patt. short v.", "distinct list of every short vector clone symmetry value found showing total counted, total fwd/rev, total chain a/b"},
            {"symm. c. patt. long v.", "distinct list of every long vector clone symmetry value found showing total counted, total fwd/rev, total chain a/b"},

            {"symm. c. patt. ss.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. int. ss.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. noint. ss.", "distinct list of every pattern's clone symmetry percentage value"},

            {"symm. c. patt. seq. ", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. int. seq.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. noint. seq.", "distinct list of every pattern's clone symmetry percentage value"},

            {"symm. c. patt. phys.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. int. phys.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. noint. phys.", "distinct list of every pattern's clone symmetry percentage value"},

            {"symm. c. patt. hydr.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. int. hydr.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. noint. hydr.", "distinct list of every pattern's clone symmetry percentage value"},

            {"symm. c. patt. pdbs.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. int. pdbs.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. noint. pdbs.", "distinct list of every pattern's clone symmetry percentage value"},

            {"symm. c. patt. unip.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. int. unip.", "distinct list of every pattern's clone symmetry percentage value"},
            {"symm. c. patt. noint. unip.", "distinct list of every pattern's clone symmetry percentage value"},

            {"aa patt.", "distinct list of each proteinInterface amino acid string found with appearance statistics"},
            {"aa int. patt.", "distinct list of each proteinInterface interactions amino acid string found with appearance statistics"},
            {"aa noint. patt.", "distinct list of each proteinInterface non interactions amino acid string found with appearance statistics"},

            {"aa patt. az sort", "distinct list of each alphabetically sorted proteinInterface amino acid string found with appearance statistics"},
            {"aa int. patt. az sort", "distinct list of each alphabetically sorted proteinInterface interactions amino acid string found with appearance statistics"},
            {"aa noint. patt. az sort", "distinct list of each alphabetically sorted proteinInterface non interactions amino acid string found with appearance statistics"},

            {"ss patt.", "secondary structure pattern with appearance statistics"},
            {"ss int. patt.", "secondary structure pattern with appearance statistics for interactions only"},
            {"ss noint. patt.", "secondary structure pattern with appearance statistics for non interactions only"},

            {"ss patt. az sort", "alphabetically sorted secondary structure pattern with appearance statistics"},
            {"ss int. patt. az sort", "alphabetically sorted secondary structure pattern with appearance statistics for interactions only"},
            {"ss noint. patt. az sort", "alphabetically sorted secondary structure pattern with appearance statistics for non interactions only"},

            {"aa g. phys. patt.", "amino acids converted to physicochemical [clustalw2] group number codes"},
            {"aa int. g. phys. patt.", "interaction amino acids converted to physicochemical [clustalw2] group number codes"},
            {"aa noint. g. phys. patt.", "non interaction amino acids converted to physicochemical [clustalw2] group number codes"},

            {"aa g. phys. patt. az sort", "alphabetically sorted amino acids converted to physicochemical [clustalw2] group number codes"},
            {"aa int. g. phys. patt. az sort", "alphabetically sorted interaction amino acids converted to physicochemical [clustalw2] group number codes"},
            {"aa noint. g. phys. patt. az sort", "alphabetically sorted non interaction amino acids converted to physicochemical [clustalw2] group number codes"},

            {"aa g. hydr. patt.", "amino acids converted to hydrophobicity [protein colourer] group number codes"},
            {"aa int. g. hydr. patt.", "interaction amino acids converted to hydrophobicity [protein colourer] group number codes"},
            {"aa noint. g. hydr. patt.", "non interaction amino acids converted to hydrophobicity [protein colourer] group number codes"},

            {"aa g. hydr. patt. az sort", "alphabetically sorted amino acids converted to hydrophobicity [protein colourer] group number codes"},
            {"aa int. g. hydr. patt. az sort", "alphabetically sorted interaction amino acids converted to hydrophobicity [protein colourer] group number codes"},
            {"aa noint. g. hydr. patt. az sort", "alphabetically sorted non interaction amino acids converted to hydrophobicity [protein colourer] group number codes"},

            {"aa g. pdbs. patt.", "amino acids converted to physicochemical [pdbsum] group number codes"},
            {"aa int. g. pdbs. patt.", "interaction amino acids converted to physicochemical [pdbsum] group number codes"},
            {"aa noint. g. pdbs. patt.", "non interaction amino acids converted to physicochemical [pdbsum] group number codes"},

            {"aa g. pdbs. patt. az sort", "alphabetically sorted amino acids converted to physicochemical [pdbsum] group number codes"},
            {"aa int. g. pdbs. patt. az sort", "alphabetically sorted interaction amino acids converted to physicochemical [pdbsum] group number codes"},
            {"aa noint. g. pdbs. patt. az sort", "alphabetically sorted non interaction amino acids converted to physicochemical [pdbsum] group number codes"},

            {"aa g. unip. patt.", "amino acids converted to physicochemical [uniProtkb/trembl] group number codes"},
            {"aa int. g. unip. patt.", "interaction amino acids converted to physicochemical [uniProtkb/trembl] group number codes"},
            {"aa noint. g. unip. patt.", "non interaction amino acids converted to physicochemical [uniProtkb/trembl] group number codes"},

            {"aa g. unip. patt. az sort", "alphabetically sorted amino acids converted to physicochemical [uniProtkb/trembl] group number codes"},
            {"aa int. g. unip. patt. az sort", "alphabetically sorted interaction amino acids converted to physicochemical [uniProtkb/trembl] group number codes"},
            {"aa noint. g. unip. patt. az sort", "alphabetically sorted non interaction amino acids converted to physicochemical [uniProtkb/trembl] group number codes"},

            //{"acidic v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            //{"int. acidic v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            //{"noint. acidic v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            //{"acidic % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            //{"int. acidic % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            //{"noint. acidic % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"aliphatic v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. aliphatic v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. aliphatic v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"aliphatic % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. aliphatic % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. aliphatic % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"aromatic v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. aromatic v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. aromatic v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"aromatic % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. aromatic % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. aromatic % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"charged v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. charged v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. charged v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"charged % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. charged % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. charged % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"hydrophobic v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. hydrophobic v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. hydrophobic v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"hydrophobic % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. hydrophobic % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. hydrophobic % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"hydroxylic v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. hydroxylic v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. hydroxylic v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"hydroxylic % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. hydroxylic % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. hydroxylic % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"negative v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. negative v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. negative v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"negative % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. negative % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. negative % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"polar v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. polar v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. polar v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"polar % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. polar % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. polar % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"positive v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. positive v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. positive v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"positive % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. positive % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. positive % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"small  v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. small v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. small v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"small % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. small % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. small % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"sulphur v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. sulphur  v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. sulphur  v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            { "sulphur % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. sulphur % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. sulphur % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},

            {"tiny v. patt.", "distinct property true/false vector for each proteinInterface position with appearance statistics"},
            {"int. tiny v. patt.", "distinct property true/false vector for each proteinInterface interaction position with appearance statistics"},
            {"noint. tiny v. patt.", "distinct property true/false vector for each proteinInterface non interaction position with appearance statistics"},

            {"tiny % patt.", "distinct list of percentages of lengths of the proteinInterfaces which are this property"},
            {"int. tiny % patt.", "distinct list of percentages of lengths of the proteinInterfaces interactions which are this property"},
            {"noint. tiny % patt.", "distinct list of percentages of lengths of the proteinInterfaces non interactions which are this property"},
        };

        public static void MakeTreeCompanionSpreadsheet(string[] pdbFilesArray, List<string> pdbIdList, List<ISequence> seqList, List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, string outputFolderName, string spreadsheetName, List<string> finalTreeLeafOrderList, FileExistsHandler.FileExistsOptions fileExistsOptions, ProgressActionSet progressActionSet)
        {
            if (pdbFilesArray == null) throw new ArgumentNullException(nameof(pdbFilesArray));
            if (pdbIdList == null) throw new ArgumentNullException(nameof(pdbIdList));
            if (seqList == null) throw new ArgumentNullException(nameof(seqList));
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));
            if (outputFolderName == null) throw new ArgumentNullException(nameof(outputFolderName));
            if (spreadsheetName == null) throw new ArgumentNullException(nameof(spreadsheetName));
            if (finalTreeLeafOrderList == null) throw new ArgumentNullException(nameof(finalTreeLeafOrderList));

            ProgressActionSet.Report("Task: Making tree proteinInterface data spreadsheet...", progressActionSet);

            ProgressActionSet.Report("Starting: Generating spreadsheet data", progressActionSet);
            ProgressActionSet.StartAction(vectorProteinInterfaceWholeList.Count, progressActionSet);
            var treeDataSpreadsheetRecordList = TreeDataSpreadsheetRecord.TreeDataSpreadsheetRecords(pdbFilesArray, pdbIdList, seqList, vectorProteinInterfaceWholeList, outputFolderName, spreadsheetName, finalTreeLeafOrderList, fileExistsOptions, progressActionSet);


            ProgressActionSet.Report("Starting: Sorting the spreadsheet into tree order", progressActionSet);
            treeDataSpreadsheetRecordList =
                treeDataSpreadsheetRecordList.OrderBy(a => finalTreeLeafOrderList.IndexOf(a.TreeId)).ToList();
            ProgressActionSet.Report("Finished.", progressActionSet);


            var treeDataSheet = TreeDataSpreadsheetRecord.SpreadsheetTreeData(treeDataSpreadsheetRecordList);
            var treeDataAaPropertiesSheet = TreeDataSpreadsheetRecord.SpreadsheetAminoAcidData(treeDataSpreadsheetRecordList);
            var treeDataAaInteractionPropertiesSheet = TreeDataSpreadsheetRecord.SpreadsheetInteractionAminoAcidData(treeDataSpreadsheetRecordList);
            var treeDataAaNonInteractionPropertiesSheet = TreeDataSpreadsheetRecord.SpreadsheetNonInteractionAminoAcidData(treeDataSpreadsheetRecordList);
            var treeDataSymmetrySheet = TreeDataSpreadsheetRecord.SpreadsheetSymmetryData(treeDataSpreadsheetRecordList);
            ProgressActionSet.Report("Finished.", progressActionSet);

            /*
            ProgressActionSet.Report("Starting: Sorting the spreadsheet into tree order", progressActionSet);
            var startTicks = DateTime.Now.Ticks;
            var itemsCompleted = 0;
            
            var taskList = new List<Task>
            {
                Task.Run(() => { SortTreeSpreadsheetArray(treeDataSheet, finalTreeLeafOrderList); }),
                Task.Run(() => { SortTreeSpreadsheetArray(treeDataAaPropertiesSheet, finalTreeLeafOrderList); }),
                Task.Run(() => { SortTreeSpreadsheetArray(treeDataAaInteractionPropertiesSheet, finalTreeLeafOrderList); }),
                Task.Run(() => { SortTreeSpreadsheetArray(treeDataAaNonInteractionPropertiesSheet, finalTreeLeafOrderList); }),
                Task.Run(() => { SortTreeSpreadsheetArray(treeDataSymmetrySheet, finalTreeLeafOrderList); })
            };

            var itemsTotal = taskList.Count;
            ProgressActionSet.StartAction(itemsTotal, progressActionSet);
            while (taskList.Count(t => !t.IsCompleted) > 0 && Task.WaitAny(taskList.ToArray()) > -1)
            {
                UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);
            }

            Task.WaitAll(taskList.Where(t => !t.IsCompleted).ToArray());
            taskList.Clear();
            ProgressActionSet.Report("Finished.", progressActionSet);
            */
            ProgressActionSet.Report("Starting: Finding patterns and generating histograms", progressActionSet);
            var startTicks = DateTime.Now.Ticks;
            var itemsCompleted = 0;
            var itemsTotal = 146;
            ProgressActionSet.StartAction(itemsTotal, progressActionSet);


            var headerTreeData = TreeDataSpreadsheetRecord.Header().ToStringsTreeData();
            //var headerAminoAcidData = TreeDataSpreadsheetRecord.Header().ToStringsAminoAcidData();
            //var headerInteractionAminoAcidData = TreeDataSpreadsheetRecord.Header().ToStringsInteractionAminoAcidData();
            var headerSymmetryData = TreeDataSpreadsheetRecord.Header().ToStringsSymmetryData();

            // vector patterns
            var indexBindingProteinInterfaceVectorShort = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceVectorShort);
            var indexBindingProteinInterfaceVectorLong = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceVectorLong);

            var treeDataShortVectorPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Short Vector Pattern", treeDataSheet, indexBindingProteinInterfaceVectorShort);
            var treeDataLongVectorPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Long Vector Pattern", treeDataSheet, indexBindingProteinInterfaceVectorLong);
            var treeDataShortVectorPatternNoneOutsideSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Short Vector Pattern", ReplaceStringsSpreadsheet(treeDataSheet, (new Dictionary<string, string>() { { "+0", "" }, { "+1", "" } }), indexBindingProteinInterfaceVectorShort), indexBindingProteinInterfaceVectorShort);
            var treeDataLongVectorPatternNoneOutsideSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Long Vector Pattern", ReplaceStringsSpreadsheet(treeDataSheet, (new Dictionary<string, string>() { { "+0", "" }, { "+1", "" } }), indexBindingProteinInterfaceVectorLong), indexBindingProteinInterfaceVectorLong);

            UpdateProgress(startTicks, 4, ref itemsCompleted, itemsTotal, progressActionSet);

            // symmetry patterns

            var indexBindingProteinInterfaceSecondaryStructureMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceSecondaryStructureMirrorSymmetryPercentage);
            var indexBindingProteinInterfaceInteractionSecondaryStructureMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceInteractionSecondaryStructureMirrorSymmetryPercentage);
            var indexBindingProteinInterfaceNonInteractionSecondaryStructureMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceNonInteractionSecondaryStructureMirrorSymmetryPercentage);

            var indexBindingProteinInterfaceSequence1LMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceSequence1LMirrorSymmetryPercentage);
            var indexBindingProteinInterfaceInteractionSequence1LMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceInteractionSequence1LMirrorSymmetryPercentage);
            var indexBindingProteinInterfaceNonInteractionSequence1LMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceNonInteractionSequence1LMirrorSymmetryPercentage);

            var indexBindingProteinInterfaceVectorShortMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceVectorShortMirrorSymmetryPercentage);
            var indexBindingProteinInterfaceVectorLongMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceVectorLongMirrorSymmetryPercentage);

            var indexPhysicochemicalGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PhysicochemicalGroupNumbersMirrorSymmetryPercentage);
            var indexPhysicochemicalInteractionGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PhysicochemicalInteractionGroupNumbersMirrorSymmetryPercentage);
            var indexPhysicochemicalNonInteractionGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PhysicochemicalNonInteractionGroupNumbersMirrorSymmetryPercentage);

            var indexHydrophobicityGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().HydrophobicityGroupNumbersMirrorSymmetryPercentage);
            var indexHydrophobicityInteractionGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().HydrophobicityInteractionGroupNumbersMirrorSymmetryPercentage);
            var indexHydrophobicityNonInteractionGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().HydrophobicityNonInteractionGroupNumbersMirrorSymmetryPercentage);

            var indexPdbSumGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PdbSumGroupNumbersMirrorSymmetryPercentage);
            var indexPdbSumInteractionGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PdbSumInteractionGroupNumbersMirrorSymmetryPercentage);
            var indexPdbSumNonInteractionGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PdbSumNonInteractionGroupNumbersMirrorSymmetryPercentage);

            var indexUniProtKbGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().UniProtKbGroupNumbersMirrorSymmetryPercentage);
            var indexUniProtKbInteractionGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().UniProtKbInteractionGroupNumbersMirrorSymmetryPercentage);
            var indexUniProtKbNonInteractionGroupNumbersMirrorSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().UniProtKbNonInteractionGroupNumbersMirrorSymmetryPercentage);

            var patternSheetBindingProteinInterfaceVectorShortMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Short Vector Mirror Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceVectorShortMirrorSymmetryPercentage);
            var patternSheetBindingProteinInterfaceVectorLongMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Long Vector Mirror Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceVectorLongMirrorSymmetryPercentage);


            var indexBindingProteinInterfaceSecondaryStructureCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceSecondaryStructureCloneSymmetryPercentage);
            var indexBindingProteinInterfaceInteractionSecondaryStructureCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceInteractionSecondaryStructureCloneSymmetryPercentage);
            var indexBindingProteinInterfaceNonInteractionSecondaryStructureCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceNonInteractionSecondaryStructureCloneSymmetryPercentage);

            var indexBindingProteinInterfaceSequence1LCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceSequence1LCloneSymmetryPercentage);
            var indexBindingProteinInterfaceInteractionSequence1LCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceInteractionSequence1LCloneSymmetryPercentage);
            var indexBindingProteinInterfaceNonInteractionSequence1LCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceNonInteractionSequence1LCloneSymmetryPercentage);

            var indexBindingProteinInterfaceVectorShortCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceVectorShortCloneSymmetryPercentage);
            var indexBindingProteinInterfaceVectorLongCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceVectorLongCloneSymmetryPercentage);

            var indexPhysicochemicalGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PhysicochemicalGroupNumbersCloneSymmetryPercentage);
            var indexPhysicochemicalInteractionGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PhysicochemicalInteractionGroupNumbersCloneSymmetryPercentage);
            var indexPhysicochemicalNonInteractionGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PhysicochemicalNonInteractionGroupNumbersCloneSymmetryPercentage);

            var indexHydrophobicityGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().HydrophobicityGroupNumbersCloneSymmetryPercentage);
            var indexHydrophobicityInteractionGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().HydrophobicityInteractionGroupNumbersCloneSymmetryPercentage);
            var indexHydrophobicityNonInteractionGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().HydrophobicityNonInteractionGroupNumbersCloneSymmetryPercentage);

            var indexPdbSumGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PdbSumGroupNumbersCloneSymmetryPercentage);
            var indexPdbSumInteractionGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PdbSumInteractionGroupNumbersCloneSymmetryPercentage);
            var indexPdbSumNonInteractionGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().PdbSumNonInteractionGroupNumbersCloneSymmetryPercentage);

            var indexUniProtKbGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().UniProtKbGroupNumbersCloneSymmetryPercentage);
            var indexUniProtKbInteractionGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().UniProtKbInteractionGroupNumbersCloneSymmetryPercentage);
            var indexUniProtKbNonInteractionGroupNumbersCloneSymmetryPercentage = Array.FindIndex(headerSymmetryData, a => a == TreeDataSpreadsheetRecord.Header().UniProtKbNonInteractionGroupNumbersCloneSymmetryPercentage);

            var patternSheetBindingProteinInterfaceVectorShortCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Short Vector Clone Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceVectorShortCloneSymmetryPercentage);
            var patternSheetBindingProteinInterfaceVectorLongCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Long Vector Clone Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceVectorLongCloneSymmetryPercentage);

            UpdateProgress(startTicks, 4, ref itemsCompleted, itemsTotal, progressActionSet);

            var patternSheetBindingProteinInterfaceSecondaryStructureMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Secondary Structure Mirror Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceSecondaryStructureMirrorSymmetryPercentage);
            var patternSheetBindingProteinInterfaceInteractionSecondaryStructureMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Secondary Structure Interactions Mirror Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceInteractionSecondaryStructureMirrorSymmetryPercentage);
            var patternSheetBindingProteinInterfaceNonInteractionSecondaryStructureMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Secondary Structure Non Interactions Mirror Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceNonInteractionSecondaryStructureMirrorSymmetryPercentage);

            var patternSheetBindingProteinInterfaceSequence1LMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("BS Seq 1L Mirror Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceSequence1LMirrorSymmetryPercentage);
            var patternSheetBindingProteinInterfaceInteractionSequence1LMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("BS Interactions Seq 1L Mirror Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceInteractionSequence1LMirrorSymmetryPercentage);
            var patternSheetBindingProteinInterfaceNonInteractionSequence1LMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("BS Non Interactions Seq 1L Mirror Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceNonInteractionSequence1LMirrorSymmetryPercentage);

            var patternSheetPhysicochemicalGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Physicochemical Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexPhysicochemicalGroupNumbersMirrorSymmetryPercentage);
            var patternSheetPhysicochemicalInteractionGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Physicochemical Interactions Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexPhysicochemicalInteractionGroupNumbersMirrorSymmetryPercentage);
            var patternSheetPhysicochemicalNonInteractionGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Physicochemical Non Interactions Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexPhysicochemicalNonInteractionGroupNumbersMirrorSymmetryPercentage);

            var patternSheetHydrophobicityGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobicity Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexHydrophobicityGroupNumbersMirrorSymmetryPercentage);
            var patternSheetHydrophobicityInteractionGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobicity Interactions Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexHydrophobicityInteractionGroupNumbersMirrorSymmetryPercentage);
            var patternSheetHydrophobicityNonInteractionGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobicity Non Interactions Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexHydrophobicityNonInteractionGroupNumbersMirrorSymmetryPercentage);

            var patternSheetPdbSumGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("PDBsum Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexPdbSumGroupNumbersMirrorSymmetryPercentage);
            var patternSheetPdbSumInteractionGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("PDBsum Interactions Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexPdbSumInteractionGroupNumbersMirrorSymmetryPercentage);
            var patternSheetPdbSumNonInteractionGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("PDBsum Non Interactions Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexPdbSumNonInteractionGroupNumbersMirrorSymmetryPercentage);

            var patternSheetUniProtKbGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("UniProtKb Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexUniProtKbGroupNumbersMirrorSymmetryPercentage);
            var patternSheetUniProtKbInteractionGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("UniProtKb Interactions Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexUniProtKbInteractionGroupNumbersMirrorSymmetryPercentage);
            var patternSheetUniProtKbNonInteractionGroupNumbersMirrorSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("UniProtKb Non Interactions Group Numbers Mirror Symmetry %", treeDataSymmetrySheet, indexUniProtKbNonInteractionGroupNumbersMirrorSymmetryPercentage);

            UpdateProgress(startTicks, 18, ref itemsCompleted, itemsTotal, progressActionSet);


            var patternSheetBindingProteinInterfaceSecondaryStructureCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Secondary Structure Clone Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceSecondaryStructureCloneSymmetryPercentage);
            var patternSheetBindingProteinInterfaceInteractionSecondaryStructureCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Secondary Structure Interactions Clone Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceInteractionSecondaryStructureCloneSymmetryPercentage);
            var patternSheetBindingProteinInterfaceNonInteractionSecondaryStructureCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Secondary Structure Non Interactions Clone Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceNonInteractionSecondaryStructureCloneSymmetryPercentage);

            var patternSheetBindingProteinInterfaceSequence1LCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("BS Seq 1L Clone Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceSequence1LCloneSymmetryPercentage);
            var patternSheetBindingProteinInterfaceInteractionSequence1LCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("BS Interactions Seq 1L Clone Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceInteractionSequence1LCloneSymmetryPercentage);
            var patternSheetBindingProteinInterfaceNonInteractionSequence1LCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("BS Non Interactions Seq 1L Clone Symmetry %", treeDataSymmetrySheet, indexBindingProteinInterfaceNonInteractionSequence1LCloneSymmetryPercentage);

            var patternSheetPhysicochemicalGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Physicochemical Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexPhysicochemicalGroupNumbersCloneSymmetryPercentage);
            var patternSheetPhysicochemicalInteractionGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Physicochemical Interactions Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexPhysicochemicalInteractionGroupNumbersCloneSymmetryPercentage);
            var patternSheetPhysicochemicalNonInteractionGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Physicochemical Non Interactions Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexPhysicochemicalNonInteractionGroupNumbersCloneSymmetryPercentage);

            var patternSheetHydrophobicityGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobicity Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexHydrophobicityGroupNumbersCloneSymmetryPercentage);
            var patternSheetHydrophobicityInteractionGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobicity Interactions Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexHydrophobicityInteractionGroupNumbersCloneSymmetryPercentage);
            var patternSheetHydrophobicityNonInteractionGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobicity Non Interactions Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexHydrophobicityNonInteractionGroupNumbersCloneSymmetryPercentage);

            var patternSheetPdbSumGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("PDBsum Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexPdbSumGroupNumbersCloneSymmetryPercentage);
            var patternSheetPdbSumInteractionGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("PDBsum Interactions Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexPdbSumInteractionGroupNumbersCloneSymmetryPercentage);
            var patternSheetPdbSumNonInteractionGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("PDBsum Non Interactions Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexPdbSumNonInteractionGroupNumbersCloneSymmetryPercentage);

            var patternSheetUniProtKbGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("UniProtKb Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexUniProtKbGroupNumbersCloneSymmetryPercentage);
            var patternSheetUniProtKbInteractionGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("UniProtKb Interactions Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexUniProtKbInteractionGroupNumbersCloneSymmetryPercentage);
            var patternSheetUniProtKbNonInteractionGroupNumbersCloneSymmetryPercentage = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("UniProtKb Non Interactions Group Numbers Clone Symmetry %", treeDataSymmetrySheet, indexUniProtKbNonInteractionGroupNumbersCloneSymmetryPercentage);

            UpdateProgress(startTicks, 18, ref itemsCompleted, itemsTotal, progressActionSet);


            // amino acid patterns
            var indexBindingProteinInterfaceSequence1L = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceSequence1L);
            var indexBindingProteinInterfaceInteractionSequence1L = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceInteractionSequence1L);
            var indexBindingProteinInterfaceNonInteractionSequence1L = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceNonInteractionSequence1L);

            var treeDataAminoAcidPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All AA Pattern", treeDataSheet, indexBindingProteinInterfaceSequence1L);
            var treeDataInteractionAminoAcidPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction AA Pattern", treeDataSheet, indexBindingProteinInterfaceInteractionSequence1L);
            var treeDataNonInteractionAminoAcidPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction AA Pattern", treeDataSheet, indexBindingProteinInterfaceNonInteractionSequence1L);

            var treeDataAminoAcidPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All AA Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexBindingProteinInterfaceSequence1L), indexBindingProteinInterfaceSequence1L);
            var treeDataInteractionAminoAcidPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction AA Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexBindingProteinInterfaceInteractionSequence1L), indexBindingProteinInterfaceInteractionSequence1L);
            var treeDataNonInteractionAminoAcidPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction AA Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexBindingProteinInterfaceNonInteractionSequence1L), indexBindingProteinInterfaceNonInteractionSequence1L);

            UpdateProgress(startTicks, 6, ref itemsCompleted, itemsTotal, progressActionSet);

            // amino acid group patterns

            var indexPhysicochemicalGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().PhysicochemicalGroupNumbers);
            var indexPhysicochemicalInteractionGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().PhysicochemicalInteractionGroupNumbers);
            var indexPhysicochemicalNonInteractionGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().PhysicochemicalNonInteractionGroupNumbers);

            var treeDataAminoAcidPhysicochemicalGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " AA Group Pattern", treeDataSheet, indexPhysicochemicalGroupNumbers);
            var treeDataInteractionAminoAcidPhysicochemicalGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " AA Group Pattern", treeDataSheet, indexPhysicochemicalInteractionGroupNumbers);
            var treeDataNonInteractionAminoAcidPhysicochemicalGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " AA Group Pattern", treeDataSheet, indexPhysicochemicalNonInteractionGroupNumbers);

            var treeDataAminoAcidPhysicochemicalGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexPhysicochemicalGroupNumbers), indexPhysicochemicalGroupNumbers);
            var treeDataInteractionAminoAcidPhysicochemicalGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexPhysicochemicalInteractionGroupNumbers), indexPhysicochemicalInteractionGroupNumbers);
            var treeDataNonInteractionAminoAcidPhysicochemicalGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Physicochemical) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexPhysicochemicalNonInteractionGroupNumbers), indexPhysicochemicalNonInteractionGroupNumbers);

            UpdateProgress(startTicks, 6, ref itemsCompleted, itemsTotal, progressActionSet);

            var indexHydrophobicityGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().HydrophobicityGroupNumbers);
            var indexHydrophobicityInteractionGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().HydrophobicityInteractionGroupNumbers);
            var indexHydrophobicityNonInteractionGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().HydrophobicityNonInteractionGroupNumbers);

            var treeDataAminoAcidHydrophobicityGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " AA Group Pattern", treeDataSheet, indexHydrophobicityGroupNumbers);
            var treeDataInteractionAminoAcidHydrophobicityGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " AA Group Pattern", treeDataSheet, indexHydrophobicityInteractionGroupNumbers);
            var treeDataNonInteractionAminoAcidHydrophobicityGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " AA Group Pattern", treeDataSheet, indexHydrophobicityNonInteractionGroupNumbers);

            var treeDataAminoAcidHydrophobicityGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexHydrophobicityGroupNumbers), indexHydrophobicityGroupNumbers);
            var treeDataInteractionAminoAcidHydrophobicityGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexHydrophobicityInteractionGroupNumbers), indexHydrophobicityInteractionGroupNumbers);
            var treeDataNonInteractionAminoAcidHydrophobicityGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.Hydrophobicity) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexHydrophobicityNonInteractionGroupNumbers), indexHydrophobicityNonInteractionGroupNumbers);

            UpdateProgress(startTicks, 6, ref itemsCompleted, itemsTotal, progressActionSet);

            var indexPdbSumGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().PdbSumGroupNumbers);
            var indexPdbSumInteractionGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().PdbSumInteractionGroupNumbers);
            var indexPdbSumNonInteractionGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().PdbSumNonInteractionGroupNumbers);

            var treeDataAminoAcidPdbSumGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " AA Group Pattern", treeDataSheet, indexPdbSumGroupNumbers);
            var treeDataInteractionAminoAcidPdbSumGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " AA Group Pattern", treeDataSheet, indexPdbSumInteractionGroupNumbers);
            var treeDataNonInteractionAminoAcidPdbSumGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " AA Group Pattern", treeDataSheet, indexPdbSumNonInteractionGroupNumbers);

            var treeDataAminoAcidPdbSumGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexPdbSumGroupNumbers), indexPdbSumGroupNumbers);
            var treeDataInteractionAminoAcidPdbSumGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexPdbSumInteractionGroupNumbers), indexPdbSumInteractionGroupNumbers);
            var treeDataNonInteractionAminoAcidPdbSumGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.PdbSum) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexPdbSumNonInteractionGroupNumbers), indexPdbSumNonInteractionGroupNumbers);

            UpdateProgress(startTicks, 6, ref itemsCompleted, itemsTotal, progressActionSet);

            var indexUniProtKbGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().UniProtKbGroupNumbers);
            var indexUniProtKbInteractionGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().UniProtKbInteractionGroupNumbers);
            var indexUniProtKbNonInteractionGroupNumbers = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().UniProtKbNonInteractionGroupNumbers);

            var treeDataAminoAcidUniProtKbGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " AA Group Pattern", treeDataSheet, indexUniProtKbGroupNumbers);
            var treeDataInteractionAminoAcidUniProtKbGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " AA Group Pattern", treeDataSheet, indexUniProtKbInteractionGroupNumbers);
            var treeDataNonInteractionAminoAcidUniProtKbGroupPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " AA Group Pattern", treeDataSheet, indexUniProtKbNonInteractionGroupNumbers);

            var treeDataAminoAcidUniProtKbGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexUniProtKbGroupNumbers), indexUniProtKbGroupNumbers);
            var treeDataInteractionAminoAcidUniProtKbGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexUniProtKbInteractionGroupNumbers), indexUniProtKbInteractionGroupNumbers);
            var treeDataNonInteractionAminoAcidUniProtKbGroupPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction " + AminoAcidGroups.AminoAcidGroups.GetGroupRuleName(AminoAcidGroups.AminoAcidGroups.EnumAminoAcidGroups.UniProtKb) + " AA Group Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexUniProtKbNonInteractionGroupNumbers), indexUniProtKbNonInteractionGroupNumbers);

            UpdateProgress(startTicks, 6, ref itemsCompleted, itemsTotal, progressActionSet);

            // secondary structure patterns

            var indexBindingProteinInterfaceSecondaryStructure = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceSecondaryStructure);
            var indexBindingProteinInterfaceInteractionSecondaryStructure = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceInteractionSecondaryStructure);
            var indexBindingProteinInterfaceNonInteractionSecondaryStructure = Array.FindIndex(headerTreeData, a => a == TreeDataSpreadsheetRecord.Header().BindingProteinInterfaceNonInteractionSecondaryStructure);

            var treeDataSecStructPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All SS Pattern", treeDataSheet, indexBindingProteinInterfaceSecondaryStructure);
            var treeDataInteractionSecStructPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction SS Pattern", treeDataSheet, indexBindingProteinInterfaceInteractionSecondaryStructure);
            var treeDataNonInteractionSecStructPatternSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction SS Pattern", treeDataSheet, indexBindingProteinInterfaceNonInteractionSecondaryStructure);

            var treeDataSecStructPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("All SS Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexBindingProteinInterfaceSecondaryStructure), indexBindingProteinInterfaceSecondaryStructure);
            var treeDataInteractionSecStructPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Interaction SS Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexBindingProteinInterfaceInteractionSecondaryStructure), indexBindingProteinInterfaceInteractionSecondaryStructure);
            var treeDataNonInteractionSecStructPatternAzSortedSheet = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Non Interaction SS Pattern A-Z Sorted", SortStringsSpreadsheet(treeDataSheet, indexBindingProteinInterfaceNonInteractionSecondaryStructure), indexBindingProteinInterfaceNonInteractionSecondaryStructure);

            UpdateProgress(startTicks, 6, ref itemsCompleted, itemsTotal, progressActionSet);

            var treeDataSheets = new AminoAcidProperties<string[,]>()
            {
                //Acidic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Acidic Vector Pattern", treeDataAaPropertiesSheet, 09 ),
                Aliphatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aliphatic Pattern", treeDataAaPropertiesSheet, 10 ),
                Aromatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aromatic Vector Pattern", treeDataAaPropertiesSheet, 12 ),
                Charged = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Charged Vector Pattern", treeDataAaPropertiesSheet, 14 ),
                Hydrophobic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobic Vector Pattern", treeDataAaPropertiesSheet, 16 ),
                Hydroxylic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydroxylic Vector Pattern", treeDataAaPropertiesSheet, 18 ),
                Negative = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Negative Vector Pattern", treeDataAaPropertiesSheet, 20 ),
                Polar = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Polar Vector Pattern", treeDataAaPropertiesSheet, 22 ),
                Positive = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Positive Vector Pattern", treeDataAaPropertiesSheet, 24 ),
                Small = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Small Vector Pattern", treeDataAaPropertiesSheet, 26 ),
                Sulphur = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Sulphur Vector Pattern", treeDataAaPropertiesSheet, 28 ),
                Tiny = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Tiny Vector Pattern", treeDataAaPropertiesSheet, 30 ),
            };
            UpdateProgress(startTicks, 11, ref itemsCompleted, itemsTotal, progressActionSet);

            var treeDataInteractionSheets = new AminoAcidProperties<string[,]>()
            {
                //Acidic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Acidic Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 09 ),
                Aliphatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aliphatic Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 10 ),
                Aromatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aromatic Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 12 ),
                Charged = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Charged Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 14 ),
                Hydrophobic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobic Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 16 ),
                Hydroxylic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydroxylic Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 18 ),
                Negative = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Negative Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 20 ),
                Polar = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Polar Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 22 ),
                Positive = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Positive Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 24 ),
                Small = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Small Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 26 ),
                Sulphur = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Sulphur Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 28 ),
                Tiny = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Tiny Interaction Vector Pattern", treeDataAaInteractionPropertiesSheet, 30 ),
            };
            UpdateProgress(startTicks, 11, ref itemsCompleted, itemsTotal, progressActionSet);

            var treeDataNonInteractionSheets = new AminoAcidProperties<string[,]>()
            {
                //Acidic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Acidic Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 09 ),
                Aliphatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aliphatic Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 10 ),
                Aromatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aromatic Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 12 ),
                Charged = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Charged Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 14 ),
                Hydrophobic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobic Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 16 ),
                Hydroxylic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydroxylic Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 18 ),
                Negative = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Negative Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 20 ),
                Polar = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Polar Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 22 ),
                Positive = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Positive Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 24 ),
                Small = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Small Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 26 ),
                Sulphur = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Sulphur Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 28 ),
                Tiny = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Tiny Non Interaction Vector Pattern", treeDataAaNonInteractionPropertiesSheet, 30 ),
            };
            UpdateProgress(startTicks, 11, ref itemsCompleted, itemsTotal, progressActionSet);

            var treeDataPercentageSheets = new AminoAcidProperties<string[,]>()
            {
                //Acidic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Acidic % Pattern", treeDataAaPropertiesSheet, 09 ),
                Aliphatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aliphatic Pattern", treeDataAaPropertiesSheet, 11 ),
                Aromatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aromatic % Pattern", treeDataAaPropertiesSheet, 13 ),
                Charged = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Charged % Pattern", treeDataAaPropertiesSheet, 15 ),
                Hydrophobic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobic % Pattern", treeDataAaPropertiesSheet, 17 ),
                Hydroxylic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydroxylic % Pattern", treeDataAaPropertiesSheet, 19 ),
                Negative = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Negative % Pattern", treeDataAaPropertiesSheet, 21 ),
                Polar = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Polar % Pattern", treeDataAaPropertiesSheet, 23 ),
                Positive = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Positive % Pattern", treeDataAaPropertiesSheet, 25 ),
                Small = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Small % Pattern", treeDataAaPropertiesSheet, 27 ),
                Sulphur = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Sulphur % Pattern", treeDataAaPropertiesSheet, 29 ),
                Tiny = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Tiny % Pattern", treeDataAaPropertiesSheet, 31 ),
            };
            UpdateProgress(startTicks, 11, ref itemsCompleted, itemsTotal, progressActionSet);

            var treeDataInteractionPercentageSheets = new AminoAcidProperties<string[,]>()
            {
                //Acidic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Acidic Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 09 ),
                Aliphatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aliphatic Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 11 ),
                Aromatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aromatic Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 13 ),
                Charged = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Charged Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 15 ),
                Hydrophobic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobic Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 17 ),
                Hydroxylic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydroxylic Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 19 ),
                Negative = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Negative Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 21 ),
                Polar = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Polar Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 23 ),
                Positive = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Positive Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 25 ),
                Small = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Small Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 27 ),
                Sulphur = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Sulphur Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 29 ),
                Tiny = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Tiny Interaction % Pattern", treeDataAaInteractionPropertiesSheet, 31 ),
            };
            UpdateProgress(startTicks, 11, ref itemsCompleted, itemsTotal, progressActionSet);


            var treeDataNonInteractionPercentageSheets = new AminoAcidProperties<string[,]>()
            {
                //Acidic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Acidic Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 09 ),
                Aliphatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aliphatic Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 11 ),
                Aromatic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Aromatic Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 13 ),
                Charged = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Charged Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 15 ),
                Hydrophobic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydrophobic Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 17 ),
                Hydroxylic = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Hydroxylic Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 19 ),
                Negative = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Negative Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 21 ),
                Polar = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Polar Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 23 ),
                Positive = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Positive Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 25 ),
                Small = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Small Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 27 ),
                Sulphur = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Sulphur Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 29 ),
                Tiny = PatternStatisticsSpreadsheetRecord.CountPatternsSpreadsheet("Tiny Non Interaction % Pattern", treeDataAaNonInteractionPropertiesSheet, 31 ),
            };
            UpdateProgress(startTicks, 11, ref itemsCompleted, itemsTotal, progressActionSet);

            ProgressActionSet.FinishAction(true, progressActionSet);
            ProgressActionSet.Report("Finished.", progressActionSet);

            ProgressActionSet.Report("Starting: Calculating standing distribution", progressActionSet);
            startTicks = DateTime.Now.Ticks;
            itemsCompleted = 0;

            string[,] distributionSpreadsheetVectorShort = null;
            string[,] distributionSpreadsheetVectorLong = null;
            string[,] distributionSpreadsheetVectorInsideProteinInterfaceShort = null;
            string[,] distributionSpreadsheetVectorInproteinInterfaceProteinInterfaceLong = null;
            string[,] distributionSpreadsheetSecondaryStructure = null;
            string[,] distributionSpreadsheetProteinInterfaceLength = null;

            var taskList2 = new List<Task>
            {
                Task.Run(() => { distributionSpreadsheetVectorShort = CalculateDistribution(vectorProteinInterfaceWholeList, pdbIdList, seqList, 0); }),
                Task.Run(() => { distributionSpreadsheetVectorLong = CalculateDistribution(vectorProteinInterfaceWholeList, pdbIdList, seqList, 1); }),
                Task.Run(() => { distributionSpreadsheetVectorInsideProteinInterfaceShort = CalculateDistribution(vectorProteinInterfaceWholeList, pdbIdList, seqList, 2); }),
                Task.Run(() => { distributionSpreadsheetVectorInproteinInterfaceProteinInterfaceLong = CalculateDistribution(vectorProteinInterfaceWholeList, pdbIdList, seqList, 3); }),
                Task.Run(() => { distributionSpreadsheetSecondaryStructure = CalculateDistribution(vectorProteinInterfaceWholeList, pdbIdList, seqList, 4); }),
                Task.Run(() => { distributionSpreadsheetProteinInterfaceLength = CalculateDistribution(vectorProteinInterfaceWholeList, pdbIdList, seqList, 5); })
            };

            itemsTotal = taskList2.Count;
            ProgressActionSet.StartAction(itemsTotal, progressActionSet);
            while (taskList2.Count(t => !t.IsCompleted) > 0 && Task.WaitAny(taskList2.ToArray()) > -1)
            {
                UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);
            }
            Task.WaitAll(taskList2.ToArray());
            taskList2.Clear();

            ProgressActionSet.Report("Finished.", progressActionSet);


            ProgressActionSet.Report("Starting: Creating proteinInterface profiles/motifs", progressActionSet);
            itemsTotal = 5;
            startTicks = DateTime.Now.Ticks;
            itemsCompleted = 0;
            ProgressActionSet.StartAction(itemsTotal, progressActionSet);

            var motifData = ProproteinInterfaceSpreadsheetRecord.MotifSpreadsheetData(vectorProteinInterfaceWholeList);
            UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);

            var motifSheet = ProproteinInterfaceSpreadsheetRecord.MotifSpreadsheet(motifData);
            UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);

            var motifHitSheet = MotifHitSpreadsheetRecord.MotifSpreadsheet(MotifHitSpreadsheetRecord.MotifRecordList(MotifHitSpreadsheetRecord.MotifDistinctWithCount(motifData)));
            UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);

            var motifProfileData = MotifProfileSpreadsheetRecord.MotifSpreadsheetData(vectorProteinInterfaceWholeList);
            UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);

            var motifProfileSheet = MotifProfileSpreadsheetRecord.Spreadsheet(motifProfileData);
            UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);

            progressActionSet.FinishAction(true);
            ProgressActionSet.Report("Finished.", progressActionSet);



            ProgressActionSet.Report("Starting: Truncating protein proteinInterfaces in PDB files", progressActionSet);
            itemsTotal = pdbFilesArray.Length;
            startTicks = DateTime.Now.Ticks;
            itemsCompleted = 0;
            ProgressActionSet.StartAction(itemsTotal, progressActionSet);
            var patternZipFilesSheet = PatternZipFiles(pdbFilesArray, vectorProteinInterfaceWholeList, outputFolderName, progressActionSet);

            progressActionSet.FinishAction(true);
            ProgressActionSet.Report("Finished.", progressActionSet);



            ProgressActionSet.Report("Starting: Merging all data into spreadsheets", progressActionSet);
            var sheetNames = TreeOutputsWorksheetNames();
            var sheetNamesSpreadsheet = TreeOutputsSheetNamesAndDescriptionsSpreadsheet();

            var spreadsheet = new List<SpreadsheetCell[,]>();

            itemsTotal = SheetNamesAndDescriptions.GetLength(0);
            ProgressActionSet.StartAction(itemsTotal, progressActionSet);
            startTicks = DateTime.Now.Ticks;
            itemsCompleted = 0;

            AddSpreadsheet(spreadsheet, sheetNamesSpreadsheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternZipFilesSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, motifSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, motifHitSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, motifProfileSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataAaPropertiesSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataAaInteractionPropertiesSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataAaNonInteractionPropertiesSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataSymmetrySheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, distributionSpreadsheetSecondaryStructure, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, distributionSpreadsheetProteinInterfaceLength, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, distributionSpreadsheetVectorShort, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, distributionSpreadsheetVectorLong, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, distributionSpreadsheetVectorInsideProteinInterfaceShort, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, distributionSpreadsheetVectorInproteinInterfaceProteinInterfaceLong, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataShortVectorPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataLongVectorPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataShortVectorPatternNoneOutsideSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataLongVectorPatternNoneOutsideSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceVectorShortMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceVectorLongMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceSecondaryStructureMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceInteractionSecondaryStructureMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceNonInteractionSecondaryStructureMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceSequence1LMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceInteractionSequence1LMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceNonInteractionSequence1LMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetPhysicochemicalGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetPhysicochemicalInteractionGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetPhysicochemicalNonInteractionGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetHydrophobicityGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetHydrophobicityInteractionGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetHydrophobicityNonInteractionGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetPdbSumGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetPdbSumInteractionGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetPdbSumNonInteractionGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetUniProtKbGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetUniProtKbInteractionGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetUniProtKbNonInteractionGroupNumbersMirrorSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceVectorShortCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceVectorLongCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceSecondaryStructureCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceInteractionSecondaryStructureCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceNonInteractionSecondaryStructureCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceSequence1LCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceInteractionSequence1LCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetBindingProteinInterfaceNonInteractionSequence1LCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetPhysicochemicalGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetPhysicochemicalInteractionGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetPhysicochemicalNonInteractionGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetHydrophobicityGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetHydrophobicityInteractionGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetHydrophobicityNonInteractionGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetPdbSumGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetPdbSumInteractionGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetPdbSumNonInteractionGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, patternSheetUniProtKbGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetUniProtKbInteractionGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, patternSheetUniProtKbNonInteractionGroupNumbersCloneSymmetryPercentage, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSecStructPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSecStructPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSecStructPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSecStructPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSecStructPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSecStructPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidPhysicochemicalGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidPhysicochemicalGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidPhysicochemicalGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidPhysicochemicalGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidPhysicochemicalGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidPhysicochemicalGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidHydrophobicityGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidHydrophobicityGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidHydrophobicityGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidHydrophobicityGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidHydrophobicityGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidHydrophobicityGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidPdbSumGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidPdbSumGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidPdbSumGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidPdbSumGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidPdbSumGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidPdbSumGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidUniProtKbGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidUniProtKbGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidUniProtKbGroupPatternSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataAminoAcidUniProtKbGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionAminoAcidUniProtKbGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionAminoAcidUniProtKbGroupPatternAzSortedSheet, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            //AddSpreadsheet(spreadsheet, treeDataSheets.Acidic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            //AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Acidic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            //AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Acidic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            //AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Acidic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            //AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Acidic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            //AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Acidic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Aliphatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Aliphatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Aliphatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Aliphatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Aliphatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Aliphatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Aromatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Aromatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Aromatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Aromatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Aromatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Aromatic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Charged, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Charged, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Charged, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Charged, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Charged, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Charged, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Hydrophobic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Hydrophobic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Hydrophobic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Hydrophobic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Hydrophobic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Hydrophobic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Hydroxylic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Hydroxylic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Hydroxylic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Hydroxylic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Hydroxylic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Hydroxylic, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Negative, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Negative, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Negative, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Negative, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Negative, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Negative, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Polar, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Polar, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Polar, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Polar, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Polar, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Polar, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Positive, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Positive, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Positive, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Positive, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Positive, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Positive, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Small, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Small, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Small, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Small, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Small, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Small, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Sulphur, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Sulphur, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Sulphur, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Sulphur, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Sulphur, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Sulphur, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataSheets.Tiny, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionSheets.Tiny, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionSheets.Tiny, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            AddSpreadsheet(spreadsheet, treeDataPercentageSheets.Tiny, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataInteractionPercentageSheets.Tiny, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);
            AddSpreadsheet(spreadsheet, treeDataNonInteractionPercentageSheets.Tiny, startTicks, ref itemsCompleted, itemsTotal, progressActionSet);

            ProgressActionSet.FinishAction(true, progressActionSet);
            ProgressActionSet.Report("Finished.", progressActionSet);
            //SetColumnCellColouring(spreadsheet[1], 10, SpreadsheetCellColourScheme.AminoAcidsUniProtKb);

            var savedFiles = SpreadsheetFileHandler.SaveSpreadsheet(FileAndPathMethods.MergePathAndFilename(outputFolderName, spreadsheetName), sheetNames, spreadsheet, progressActionSet, false, true, fileExistsOptions);
            ProgressActionSet.ReportFilesSaved(savedFiles, progressActionSet);

        }

        public static string ShortenFileNameToMaxLen(string filename)
        {
            var path = Path.GetDirectoryName(filename);
            var ext = Path.GetExtension(filename);
            var file = Path.GetFileNameWithoutExtension(filename);

            var alias = 0;

            while (filename.Length > 259 ||  File.Exists(filename))
            {
                var overflow = filename.Length > 259 ? filename.Length - 259 : 0;

                var aliasStr = "~" + alias;

                filename = path + file.Substring(0,(file.Length - overflow) - aliasStr.Length) + aliasStr + ext;

                alias++;
            }

            return filename;
        }

        public class PatternZipFileMapSpreadsheet
        {
            public class PatternZipFileMapSpreadsheetRecord
            {
                public string PatternType;
                public string Pattern;
                public string Total;
                public string FileName;

                public string[] ToStrings()
                {
                    var result = new string[]
                    {
                        PatternType,
                        Pattern,
                        Total,
                        FileName
                    };

                    return result;
                }
            }

            public PatternZipFileMapSpreadsheetRecord Header()
            {
                var header = new PatternZipFileMapSpreadsheetRecord()
                {
                    PatternType = "Pattern Type",
                    Pattern = "Pattern",
                    FileName = "Filename",
                    Total = "Total"
                };

                return header;
            }

            public List<PatternZipFileMapSpreadsheetRecord> Records = new List<PatternZipFileMapSpreadsheetRecord>();

            public string[,] ToSpreadsheetStrings()
            {
                var header = Header().ToStrings();

                var result = new string[Records.Count + 1, header.Length];

                for (int index = 0; index < header.Length; index++)
                {
                    result[0, index] = header[index];
                }

                for (int i = 0; i < Records.Count; i++)
                {
                    
                    var recordArray = Records[i].ToStrings();

                    for (int index = 0; index < recordArray.Length; index++)
                    {
                        result[i + 1, index] = recordArray[index];
                    }
                }

                return result;
            }

            
        }

        public static string[,] PatternZipFiles(string[] pdbFilesArray, List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, string outputFolderName, ProgressActionSet progressActionSet)
        {
            var patternZipFileMapSpreadsheet = new PatternZipFileMapSpreadsheet();
            
            var startTicks = DateTime.Now.Ticks;
            var itemsCompleted = 0;
            var itemsTotal = pdbFilesArray.Length;
            ProgressActionSet.StartAction(itemsTotal, progressActionSet);

            // 1. filter all pdb files
            // 2. copy into category dictionaries
            // 3. create zip files
            var proteinInterfacePdbFileDictionary = new Dictionary<string, List<string>>();

            foreach (var pdbFile in pdbFilesArray)
            {
                var pdbId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFile);

                var proteinInterfaceIdList = new List<IsolateInteractionStructure.ProteinInterfaceId>();

                foreach (var pdbProteinInterface in vectorProteinInterfaceWholeList.Where(a => a.FullProteinInterfaceId.ProteinId == pdbId))
                {
                    proteinInterfaceIdList.Add(new IsolateInteractionStructure.ProteinInterfaceId()
                    {
                        ChainId = pdbProteinInterface.FullProteinInterfaceId.ChainId,
                        FirstPosition = pdbProteinInterface.FirstResidueSequenceIndex,
                        LastPosition = pdbProteinInterface.LastResidueSequenceIndex
                    });
                }

                if (proteinInterfaceIdList.Count > 0)
                {
                    var onlyProteinInterfacesPdb = IsolateInteractionStructure.RemoveNonProteinInterfaceRecords(pdbFile, proteinInterfaceIdList);

                    proteinInterfacePdbFileDictionary.Add(pdbId, onlyProteinInterfacesPdb);
                }
                UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);
            }

            var proteinInterfaceLengthPatternPdbs = new Dictionary<string, List<string>>();
            var shortVectorPatternPdbs = new Dictionary<string, List<string>>();
            var longVectorPatternPdbs = new Dictionary<string, List<string>>();
            var shortIgnoreOutsideVectorPatternPdbs = new Dictionary<string, List<string>>();
            var longIgnoreOutsideVectorPatternPdbs = new Dictionary<string, List<string>>();
            var secondaryStructurePatternPdbs = new Dictionary<string, List<string>>();

            startTicks = DateTime.Now.Ticks;
            itemsCompleted = 0;
            itemsTotal = vectorProteinInterfaceWholeList.Count;
            ProgressActionSet.StartAction(itemsTotal, progressActionSet);

            foreach (var vectorProteinInterfaceWhole in vectorProteinInterfaceWholeList)
            {
                var interactionBools = vectorProteinInterfaceWhole.InteractionBools();
                var bsVectorShort = string.Join("", interactionBools.Select(Convert.ToInt32)) + "+" + (vectorProteinInterfaceWhole.VectorProteinInterfacePartList.Count(a => a.InteractionToNonProteinInterface) > 0 ? 1 : 0);
                var bsVectorLong = string.Join(" ", vectorProteinInterfaceWhole.VectorProteinInterfacePartList.Select(a => string.Join("", a.InteractionFlagBools.Select(Convert.ToInt32)) + "+" + Convert.ToInt32(a.InteractionToNonProteinInterface)).ToList());

                var bsVectorShortIgnoreOutside = string.Join("", interactionBools.Select(Convert.ToInt32));
                var bsVectorLongIgnoreOutside = string.Join(" ", vectorProteinInterfaceWhole.VectorProteinInterfacePartList.Select(a => string.Join("", a.InteractionFlagBools.Select(Convert.ToInt32))).ToList());

                var bsSecondaryStructure = vectorProteinInterfaceWhole.SecondaryStructure;
                var bsLen = ""+vectorProteinInterfaceWhole.ProteinInterfaceLength;

                if (!String.IsNullOrWhiteSpace(bsVectorShort))
                {
                    if (!shortVectorPatternPdbs.ContainsKey(bsVectorShort))
                    {
                        shortVectorPatternPdbs.Add(bsVectorShort, new List<string>());
                    }
                    shortVectorPatternPdbs[bsVectorShort].Add(vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinId);
                }

                if (!String.IsNullOrWhiteSpace(bsVectorLong))
                {
                    if (!longVectorPatternPdbs.ContainsKey(bsVectorLong))
                    {
                        longVectorPatternPdbs.Add(bsVectorLong, new List<string>());
                    }
                    longVectorPatternPdbs[bsVectorLong].Add(vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinId);
                }

                if (!String.IsNullOrWhiteSpace(bsVectorShortIgnoreOutside))
                {
                    if (!shortIgnoreOutsideVectorPatternPdbs.ContainsKey(bsVectorShortIgnoreOutside))
                    {
                        shortIgnoreOutsideVectorPatternPdbs.Add(bsVectorShortIgnoreOutside, new List<string>());
                    }
                    shortIgnoreOutsideVectorPatternPdbs[bsVectorShortIgnoreOutside].Add(vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinId);
                }

                if (!String.IsNullOrWhiteSpace(bsVectorLongIgnoreOutside))
                {
                    if (!longIgnoreOutsideVectorPatternPdbs.ContainsKey(bsVectorLongIgnoreOutside))
                    {
                        longIgnoreOutsideVectorPatternPdbs.Add(bsVectorLongIgnoreOutside, new List<string>());
                    }
                    longIgnoreOutsideVectorPatternPdbs[bsVectorLongIgnoreOutside].Add(vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinId);
                }

                if (!String.IsNullOrWhiteSpace(bsSecondaryStructure))
                {
                    if (!secondaryStructurePatternPdbs.ContainsKey(bsSecondaryStructure))
                    {
                        secondaryStructurePatternPdbs.Add(bsSecondaryStructure, new List<string>());
                    }
                    secondaryStructurePatternPdbs[bsSecondaryStructure].Add(vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinId);
                }

                if (!String.IsNullOrWhiteSpace(bsLen))
                {
                    if (!proteinInterfaceLengthPatternPdbs.ContainsKey(bsLen))
                    {
                        proteinInterfaceLengthPatternPdbs.Add(bsLen, new List<string>());
                    }
                    proteinInterfaceLengthPatternPdbs[bsLen].Add(vectorProteinInterfaceWhole.FullProteinInterfaceId.ProteinId);
                }

                UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);
            }

            var proteinInterfaceLengthFolder = FileAndPathMethods.MergePathAndFilename(outputFolderName, "patterns - proteinInterface length");
            var shortVectorFolder = FileAndPathMethods.MergePathAndFilename(outputFolderName, "patterns - short vectors");
            var longVectorFolder = FileAndPathMethods.MergePathAndFilename(outputFolderName, "patterns - long vectors");
            var shortIgnoreOutsideVectorFolder = FileAndPathMethods.MergePathAndFilename(outputFolderName, "patterns - short vectors ignore outside");
            var longIgnoreOutsideVectorFolder = FileAndPathMethods.MergePathAndFilename(outputFolderName, "patterns - long vectors ignore outside");
            var secondaryStructureFolder = FileAndPathMethods.MergePathAndFilename(outputFolderName, "patterns - secondary structure");

            Directory.CreateDirectory(proteinInterfaceLengthFolder);
            Directory.CreateDirectory(shortVectorFolder);
            Directory.CreateDirectory(longVectorFolder);
            Directory.CreateDirectory(shortIgnoreOutsideVectorFolder);
            Directory.CreateDirectory(longIgnoreOutsideVectorFolder);
            Directory.CreateDirectory(secondaryStructureFolder);

            startTicks = DateTime.Now.Ticks;
            itemsCompleted = 0;
            itemsTotal = shortVectorPatternPdbs.Count + longVectorPatternPdbs.Count + shortIgnoreOutsideVectorPatternPdbs.Count + longIgnoreOutsideVectorPatternPdbs.Count + secondaryStructurePatternPdbs.Count + proteinInterfaceLengthPatternPdbs.Count;
            ProgressActionSet.StartAction(itemsTotal, progressActionSet);

            var invalid = Path.GetInvalidFileNameChars();


            var shortVectorPatternPdbsMax = shortVectorPatternPdbs != null && shortVectorPatternPdbs.Count > 0 ? shortVectorPatternPdbs.Select(a => a.Value.Count).Max() : 0;
            var longVectorPatternPdbsMax = longVectorPatternPdbs != null && longVectorPatternPdbs.Count > 0 ? longVectorPatternPdbs.Select(a => a.Value.Count).Max() : 0;

            var shortIgnoreOutsideVectorPatternPdbsMax = shortIgnoreOutsideVectorPatternPdbs != null && shortIgnoreOutsideVectorPatternPdbs.Count > 0 ? shortIgnoreOutsideVectorPatternPdbs.Select(a => a.Value.Count).Max() : 0;
            var longIgnoreOutsideVectorPatternPdbsMax = longIgnoreOutsideVectorPatternPdbs != null && longIgnoreOutsideVectorPatternPdbs.Count > 0 ? longIgnoreOutsideVectorPatternPdbs.Select(a => a.Value.Count).Max() : 0;

            var secondaryStructurePatternPdbsMax = secondaryStructurePatternPdbs != null && secondaryStructurePatternPdbs.Count > 0 ? secondaryStructurePatternPdbs.Select(a => a.Value.Count).Max() : 0;
            var proteinInterfaceLengthPatternPdbsMax = proteinInterfaceLengthPatternPdbs != null && proteinInterfaceLengthPatternPdbs.Count > 0 ? proteinInterfaceLengthPatternPdbs.Select(a => a.Value.Count).Max() : 0;


            //var maxPatternCount = (shortVectorPatternPdbs.Select(a => a.Value.Count).Max() + longVectorPatternPdbs.Select(a => a.Value.Count).Max() + shortIgnoreOutsideVectorPatternPdbs.Select(a => a.Value.Count).Max() + longIgnoreOutsideVectorPatternPdbs.Select(a => a.Value.Count).Max() + secondaryStructurePatternPdbs.Select(a => a.Value.Count).Max() + proteinInterfaceLengthPatternPdbs.Select(a => a.Value.Count).Max()).ToString();
            var maxPatternCount = (shortVectorPatternPdbsMax+ longVectorPatternPdbsMax+ shortIgnoreOutsideVectorPatternPdbsMax+ longIgnoreOutsideVectorPatternPdbsMax+ secondaryStructurePatternPdbsMax+ proteinInterfaceLengthPatternPdbsMax).ToString();



            var patternNumber = 0;

            var maxPatternNumber = (shortVectorPatternPdbs.Count + longVectorPatternPdbs.Count + shortIgnoreOutsideVectorPatternPdbs.Count + longIgnoreOutsideVectorPatternPdbs.Count + secondaryStructurePatternPdbs.Count + proteinInterfaceLengthPatternPdbs.Count).ToString();

            foreach (PatternTypes patternType in Enum.GetValues(typeof (PatternTypes)))
            {
                Dictionary<string, List<string>> patternsDictionary;
                string patternOutputFolder;
                string patternTypeStr = "";

                if (patternType == PatternTypes.ShortVector)
                {
                    patternsDictionary = shortVectorPatternPdbs;
                    patternOutputFolder = shortVectorFolder;

                    patternTypeStr = "short vector";
                    ProgressActionSet.Report("Truncating short vector patterns:", progressActionSet);
                }
                else if (patternType == PatternTypes.LongVector)
                {
                    patternsDictionary = longVectorPatternPdbs;
                    patternOutputFolder = longVectorFolder;

                    patternTypeStr = "long vector";
                    ProgressActionSet.Report("Truncating long vector patterns:", progressActionSet);
                }
                else if (patternType == PatternTypes.ShortVectorIgnoreOutside)
                {
                    patternsDictionary = shortIgnoreOutsideVectorPatternPdbs;
                    patternOutputFolder = shortIgnoreOutsideVectorFolder;

                    patternTypeStr = "short vector without outside";
                    ProgressActionSet.Report("Truncating short vector without outside patterns:", progressActionSet);
                }
                else if (patternType == PatternTypes.LongVectorIgnoreOutside)
                {
                    patternsDictionary = longIgnoreOutsideVectorPatternPdbs;
                    patternOutputFolder = longIgnoreOutsideVectorFolder;

                    patternTypeStr = "long vector without outside";
                    ProgressActionSet.Report("Truncating long vector without outside patterns:", progressActionSet);
                }
                else if (patternType == PatternTypes.SecondaryStructure)
                {
                    patternsDictionary = secondaryStructurePatternPdbs;
                    patternOutputFolder = secondaryStructureFolder;

                    patternTypeStr = "secondary structure";
                    ProgressActionSet.Report("Truncating secondary structure patterns:", progressActionSet);
                }
                else if (patternType == PatternTypes.ProteinInterfaceLength)
                {
                    patternsDictionary = proteinInterfaceLengthPatternPdbs;
                    patternOutputFolder = proteinInterfaceLengthFolder;

                    patternTypeStr = "proteinInterface length";
                    ProgressActionSet.Report("Truncating proteinInterface length patterns:", progressActionSet);
                }
                else
                {
                    throw new IndexOutOfRangeException(nameof(patternType));
                }



                foreach (var pattern in patternsDictionary) // loop list of patterns
                {
                    patternNumber++;
                        
                    if (!String.IsNullOrWhiteSpace(pattern.Key) && pattern.Value != null && pattern.Value.Count != 0)
                    {
                        ProgressActionSet.Report("Truncating proteinInterface pattern: " + pattern.Key + " (" + pattern.Value.Count + " pattern matches)", progressActionSet);

                        var filename = ("" + pattern.Value.Count).PadLeft(maxPatternCount.Length, '0') + " - " + ("" + patternNumber).PadLeft(maxPatternNumber.Length, '0') + " - " + String.Join("", pattern.Key.Select(a => invalid.Contains(a) ? '_' : a).ToArray());
                        
                        var patternZipFile = FileAndPathMethods.MergePathAndFilename(patternOutputFolder, filename + ".zip");

                        if (patternZipFile.Length > 259)
                        {
                            var overflow = patternZipFile.Length - 259;
                            filename = filename.Substring(0, filename.Length - overflow);
                            patternZipFile = FileAndPathMethods.MergePathAndFilename(patternOutputFolder, filename + ".zip");
                        }

                        patternZipFileMapSpreadsheet.Records.Add(new PatternZipFileMapSpreadsheet.PatternZipFileMapSpreadsheetRecord()
                        {
                            FileName = patternZipFile,
                            Pattern = pattern.Key,
                            PatternType = patternTypeStr,
                            Total = ""+pattern.Value.Count
                        });

                        using (FileStream zipToOpen = new FileStream(patternZipFile, FileMode.Create))
                        {
                            var proteinInterfacePdbs = proteinInterfacePdbFileDictionary.Where(a => pattern.Value.Contains(a.Key)).ToList();

                            if (proteinInterfacePdbs.Count != 0)
                            {
                                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                                {
                                    foreach (var proteinInterfacePdb in proteinInterfacePdbs) // loop list of pdbs in the pattern
                                    {
                                        if (proteinInterfacePdb.Value == null || proteinInterfacePdb.Value.Count == 0) continue;


                                        ZipArchiveEntry readmeEntry = archive.CreateEntry(String.Join("", proteinInterfacePdb.Key.Select(a => invalid.Contains(a) ? '_' : a).ToArray()) + ".pdb");
                                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                                        {
                                            foreach (var line in proteinInterfacePdb.Value)
                                            {
                                                if (string.IsNullOrEmpty(line)) continue;

                                                writer.WriteLine(line);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);
                }
            }

            var result = patternZipFileMapSpreadsheet.ToSpreadsheetStrings();

            return result;
        }

        public enum PatternTypes
        {
            ShortVector,
            ShortVectorIgnoreOutside,
            LongVector,
            LongVectorIgnoreOutside,
            SecondaryStructure,
            ProteinInterfaceLength
        }

        public static string[,] TreeOutputsSheetNamesAndDescriptionsSpreadsheet()
        {
            var result = new List<string[]>();

            var header = new string[]
            {
                "Sheet #",
                "Sheet name",
                "Sheet contents",
            };

            result.Add(header);

            for (int rowIndex = 0; rowIndex < SheetNamesAndDescriptions.GetLength(0); rowIndex++)
            {
                var record = new string[]
                {
                    "" + (rowIndex + 1),
                    SheetNamesAndDescriptions[rowIndex, 0].Substring(0,
                        SheetNamesAndDescriptions[rowIndex, 0].Length > 31
                            ? 31
                            : SheetNamesAndDescriptions[rowIndex, 0].Length),
                    SheetNamesAndDescriptions[rowIndex, 1],
                };

                result.Add(record);
            }

            return ConvertTypes.StringJagged2DArrayTo2DArray(result.ToArray());
        }

        public static string[] TreeOutputsWorksheetNames()
        {
            var sheetNames = new string[SheetNamesAndDescriptions.GetLength(0)];

            for (var rowIndex = 0; rowIndex < SheetNamesAndDescriptions.GetLength(0); rowIndex++)
            {
                sheetNames[rowIndex] = SheetNamesAndDescriptions[rowIndex, 0].Substring(0,
                    SheetNamesAndDescriptions[rowIndex, 0].Length > 31
                        ? 31
                        : SheetNamesAndDescriptions[rowIndex, 0].Length);
            }

            return sheetNames;
        }

        public static string[,] CalculateDistribution(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, List<string> pdbIdList, List<ISequence> seqList, int vectorType)
        {
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));
            if (pdbIdList == null) throw new ArgumentNullException(nameof(pdbIdList));
            if (seqList == null) throw new ArgumentNullException(nameof(seqList));

            var distributionData = new List<AminoAcidDistributionSpreadsheetRecord>();
            distributionData.AddRange(AminoAcidDistributionSpreadsheetRecord.OverallDistributionRecords(vectorProteinInterfaceWholeList, pdbIdList, seqList/*, vectorType*/));
            distributionData.AddRange(AminoAcidDistributionSpreadsheetRecord.PatternDistributionSpreadsheetRecords(vectorProteinInterfaceWholeList, pdbIdList, seqList, vectorType));
            var distributionSpreadsheet = AminoAcidDistributionSpreadsheetRecord.Spreadsheet(distributionData);

            return distributionSpreadsheet;
        }

        public static void AddSpreadsheet(List<SpreadsheetCell[,]> spreadsheet, string[,] sheet, long startTicks, ref int itemsCompleted, int itemsTotal, ProgressActionSet progressActionSet)
        {
            if (spreadsheet == null) throw new ArgumentNullException(nameof(spreadsheet));
            if (sheet == null) throw new ArgumentNullException(nameof(sheet));

            spreadsheet.Add(ConvertTypes.String2DArrayToSpreadsheetCell2DArray(sheet));
            UpdateProgress(startTicks, 1, ref itemsCompleted, itemsTotal, progressActionSet);
        }

        public static void UpdateProgress(long startTicks, int newItemsCompleted, ref int itemsPreviouslyCompleted, int itemsTotal, ProgressActionSet progressActionSet)
        {
            itemsPreviouslyCompleted += newItemsCompleted;
            ProgressActionSet.ProgressAction(newItemsCompleted, progressActionSet);
            ProgressActionSet.EstimatedTimeRemainingAction(startTicks, itemsPreviouslyCompleted, itemsTotal, progressActionSet);
        }

        //public static void SetColumnCellColouring(SpreadsheetCell[,] spreadsheet, int columnIndex, SpreadsheetCellColourScheme spreadsheetCellColourScheme)
        //{
        //    for (var rowIndex = 0; rowIndex < spreadsheet.GetLength(0); rowIndex++)
        //    {
        //        if (spreadsheet[rowIndex,columnIndex] == null || String.IsNullOrWhiteSpace(spreadsheet[rowIndex, columnIndex].CellData)) continue;

        //        spreadsheet[rowIndex, columnIndex].CellColourScheme = spreadsheetCellColourScheme;
        //    }
        //}

        public static string[,] SortStringsSpreadsheet(string[,] treeDataSheet, int columnIndex)
        {
            if (treeDataSheet == null) throw new ArgumentNullException(nameof(treeDataSheet));

            var result = new string[treeDataSheet.GetLength(0), treeDataSheet.GetLength(1)];
            Array.Copy(treeDataSheet, result, treeDataSheet.Length);
            for (var rowIndex = 0; rowIndex < result.GetLength(0); rowIndex++)
            {
                result[rowIndex, columnIndex] = string.Join("", result[rowIndex, columnIndex].OrderBy(a => a).ToList());
            }
            return result;
        }


        public static string[,] ReplaceStringsSpreadsheet(string[,] treeDataSheet, Dictionary<string, string> replacements, int columnIndex)
        {
            if (treeDataSheet == null) throw new ArgumentNullException(nameof(treeDataSheet));
            if (replacements == null) throw new ArgumentNullException(nameof(replacements));

            var result = new string[treeDataSheet.GetLength(0), treeDataSheet.GetLength(1)];
            Array.Copy(treeDataSheet, result, treeDataSheet.Length);

            for (var rowIndex = 0; rowIndex < result.GetLength(0); rowIndex++)
            {
                foreach (var kvp in replacements)
                {
                    result[rowIndex, columnIndex] = result[rowIndex, columnIndex].Replace(kvp.Key, kvp.Value);
                }
            }
            return result;
        }




        public static int[] CountAminoAcids(string aminoAcidString)
        {
            if (aminoAcidString == null) throw new ArgumentNullException(nameof(aminoAcidString));

            var result = new int[AminoAcids.AminoAcidTotals.TotalAminoAcids()];

            foreach (var aminoAcid in aminoAcidString)
            {
                var index = AminoAcidConversions.AminoAcidNameToNumber(aminoAcid);

                result[index]++;
            }

            return result;
        }

        public static void SortTreeSpreadsheetArray(string[,] treeDataSheet, List<string> finalTreeLeafOrderList)
        {
            if (treeDataSheet == null) throw new ArgumentNullException(nameof(treeDataSheet));
            if (finalTreeLeafOrderList == null || finalTreeLeafOrderList.Count == 0) throw new ArgumentNullException(nameof(finalTreeLeafOrderList));

            for (var index = 1; index < treeDataSheet.GetLength(0); index++)
            {
                var treeIdToFind = finalTreeLeafOrderList[index - 1];

                for (var index2 = 1; index2 < treeDataSheet.GetLength(0); index2++)
                {
                    if (treeDataSheet[index2, 0] == treeIdToFind)
                    {

                        for (var index3 = 0; index3 < treeDataSheet.GetLength(1); index3++)
                        {
                            var first = treeDataSheet[index2, index3];
                            var last = treeDataSheet[index, index3];

                            treeDataSheet[index2, index3] = last;
                            treeDataSheet[index, index3] = first;
                        }

                    }
                }
            }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputFolderName"></param>
        /// <param name="vectorProteinInterfaceWholeList"></param>
        /// <param name="vectorOptimisticDistanceMatrix"></param>
        /// <param name="fileExistsOptions"></param>
        /// <param name="progressActionSet"></param>
        public static void OutputClutoFiles(string outputFolderName, List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, double[,] vectorOptimisticDistanceMatrix, / *double[,] vectorPessimisticDistanceMatrix,* / FileExistsHandler.FileExistsOptions fileExistsOptions, ProgressActionSet progressActionSet)
        {
            if (outputFolderName == null) throw new ArgumentNullException(nameof(outputFolderName));
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));


            string[] vectorHeadings = vectorProteinInterfaceWholeList.Select(a => a.FullProteinInterfaceId.ToString()).ToArray();

            string outputClutoFolderName = FileAndPathMethods.MergePathAndFilename(outputFolderName, @"cluto");

            for (var i = 0; i < 2; i++)
            {
                string matrixName = "";
                double[,] matrix;

                if (i == 0)
                {
                    if (vectorOptimisticDistanceMatrix == null || vectorOptimisticDistanceMatrix.Length == 0) continue;
                    matrixName = @"optimistic";
                    matrix = vectorOptimisticDistanceMatrix;
                }
                //else if (i == 1)
                //{
                //    if (vectorPessimisticDistanceMatrix == null || vectorPessimisticDistanceMatrix.Length == 0) continue;
                //    matrixName = @"pessimistic";
                //    matrix = vectorPessimisticDistanceMatrix;
                //}
                else
                {
                    break;
                }

                foreach (ClutoMatrixFormatTypes clutoMatrixFormatType in Enum.GetValues(typeof(ClutoMatrixFormatTypes)))
                {
                    string filename = FileAndPathMethods.MergePathAndFilename(outputClutoFolderName, matrixName + clutoMatrixFormatType.ToString() + ClutoMatrixFile.ClutoMatrixFileExt);

                    string[] savedFiles = ClutoMatrixFile.ConvertToMatrixFile
                        (
                            ConvertTypes.Double2DArrayToDecimal2DArray(matrix),
                            filename,
                            vectorHeadings,
                            filename + ClutoMatrixFile.ClutoRowFileExt,
                            vectorHeadings,
                            filename + ClutoMatrixFile.ClutoColumnFileExt,
                            clutoMatrixFormatType,
                            fileExistsOptions
                        );
                    ProgressActionSet.ReportFilesSaved(savedFiles, progressActionSet);
                }
            }
        }
        */

        public static void OutputTrees(List<VectorProteinInterfaceWhole> vectorProteinInterfaceWholeList, double[,] distanceMatrix, int minimumOutputTreeLeafs, string outputFolderName, string subfolder, out List<string> finalTreeLeafOrderList, FileExistsHandler.FileExistsOptions fileExistsOptions, ProgressActionSet progressActionSet)
        {
            if (vectorProteinInterfaceWholeList == null) throw new ArgumentNullException(nameof(vectorProteinInterfaceWholeList));
            if (distanceMatrix == null) throw new ArgumentNullException(nameof(distanceMatrix));
            if (outputFolderName == null) throw new ArgumentNullException(nameof(outputFolderName));

            bool outputClusterFastaFiles = true;
            bool outputClusterTreeFiles = true;

            bool finalFastaOnly = true;
            bool finalTreeOnly = true;

            List<string> vectorNames;
            List<List<UpgmaNode>> nodeListList;
            List<List<string>> treeListList;
            vectorNames = vectorProteinInterfaceWholeList.Select(VectorController.VectorProteinInterfaceWholeTreeHeader).ToList();

            ProgressActionSet.Report("Starting UPGMA clustering", progressActionSet);

            UpgmaClustering.Upgma(ConvertTypes.Double2DArrayToDecimal2DArray(distanceMatrix), vectorNames, minimumOutputTreeLeafs, out nodeListList, out treeListList, out finalTreeLeafOrderList, false, progressActionSet);

            ProgressActionSet.Report("Finished UPGMA clustering", progressActionSet);

            ProgressActionSet.Report("Starting converting node network to newick trees", progressActionSet);

            ProgressActionSet.StartAction(treeListList.Count, progressActionSet);
            var startTicks = DateTime.Now.Ticks;

            for (int index = 0; index < treeListList.Count; index++)
            {
                var treeList = treeListList[index];

                if (treeList == null) continue;
                
                var nodeList = nodeListList[index];

                string fileNumber = (index + 1).ToString().PadLeft(treeListList.Count.ToString().Length, '0');
                int treeCount = treeList.Count;
                if (treeCount == 0) continue;

                List<int> treeLeafCountList = new List<int>();
                int treeIndex = 0;
                foreach (UpgmaNode rootNode in nodeList.Where(a => a.IsRootNode()))
                {

                    var treeLeafsCount = rootNode.GetNodeTreeLeafs(nodeList.Select(b => (GenericNode)b).ToList()).Count;
                    if (treeLeafsCount != 0 && treeLeafsCount >= minimumOutputTreeLeafs)
                    {
                        treeIndex++;
                        treeLeafCountList.Add(treeLeafsCount);

                        var treeProteinInterfaceWholeList = vectorProteinInterfaceWholeList.Where((a, i) => rootNode.VectorIndexes.Contains(i)).ToList();

                        if ((outputClusterFastaFiles && !finalTreeOnly) || (outputClusterFastaFiles && finalFastaOnly && index == treeListList.Count - 1))
                        {
                            var fastaEntries = treeProteinInterfaceWholeList.Select(a => a.FastaFormatProteinInterface()).ToList();


                            var fastaOutputFilename = FileAndPathMethods.MergePathAndFilename(outputFolderName, subfolder + @"\fasta", @"iteration [" + fileNumber + @"] treeid [" + treeIndex + @"] proteinInterfaces [" + treeProteinInterfaceWholeList.Count + @"].fasta");

                            FileInfo fileInfo = new FileInfo(fastaOutputFilename);
                            if (fileInfo.Directory != null) fileInfo.Directory.Create();

                            File.WriteAllLines(fastaOutputFilename, fastaEntries);

                            ProgressActionSet.ReportFilesSaved(new[] { fastaOutputFilename }, progressActionSet);
                        }
                    }
                }

                if ((outputClusterTreeFiles && !finalTreeOnly) || (outputClusterTreeFiles && finalTreeOnly && index == treeListList.Count - 1))
                {
                    var leafs = string.Join(@" ", treeLeafCountList.OrderBy(a => a).Select(v => treeLeafCountList.Count(b => b == v) + @"x" + v).Distinct().ToList());
                    //var latestDistanceAdded = nodeList.Last().DistanceParentNodeA > nodeList.Last().DistanceParentNodeB ? nodeList.Last().DistanceParentNodeA : nodeList.Last().DistanceParentNodeB;
                    //latestDistanceAdded = Decimal.Round(latestDistanceAdded, 2);

                    //string outputOptimisticTreeFilename = FileAndPathMethods.MergePathAndFilename(outputFolderName, "newick tree optimistic", "iteration [" + fileNumber + "] trees [" + treeCount + "] dist [" + String.Format("{0:00.00}", latestDistanceAdded) + "] leafs [" + leafs + "].tree");
                    string outputTreeFilename = FileAndPathMethods.MergePathAndFilename(outputFolderName, subfolder, @"iteration [" + fileNumber + @"] trees [" + treeCount + @"] leafs [" + leafs + @"].tree");
                    string savedTreeFile = Newick.SaveNewickTree(outputTreeFilename, treeList, fileExistsOptions);
                    ProgressActionSet.ReportFilesSaved(new[] { savedTreeFile }, progressActionSet);
                }

                ProgressActionSet.ProgressAction(1, progressActionSet);
                ProgressActionSet.EstimatedTimeRemainingAction(startTicks, index + 1, treeListList.Count, progressActionSet);
            }

            ProgressActionSet.FinishAction(true, progressActionSet);
            ProgressActionSet.Report("Finished converting node network to newick trees", progressActionSet);
        }
    }
}
