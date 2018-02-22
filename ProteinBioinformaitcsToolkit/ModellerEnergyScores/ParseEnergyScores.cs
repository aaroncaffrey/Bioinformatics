using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;


namespace ParseEnergyScores
{
    public class ParseEnergyScores
    {
        public class EnergyScore
        {
            //public string ResultGroupNumber;
            public string Input1QprReceptorCluster;
            public string Input1QprReceptorPdbId;
            public string Input1QprLigandCluster;
            public string Input1QprLigandPdbId;
            public string Input1QprLigandInterface;

            public string Input2QplReceptorCluster;
            public string Input2QplReceptorPdbId;
            public string Input2QplLigandCluster;
            public string Input2QplLigandPdbId;
            public string Input2QplLigandInterface;

            public string SubstitutionDescription;
            public string StructureFilename;
            public string StructureFolder;

            public string StructuralTruncation;

            public string ModellerModelChainName;
            public string ModellerGa341;
            public string ModellerGa341Comptactness;
            public string ModellerGa341NativeEnergyPair;
            public string ModellerGa341NativeEnergySurface;
            public string ModellerGa341NativeEnergyCombined;
            public string ModellerGa341ZScorePair;
            public string ModellerGa341ZScoreSurface;
            public string ModellerGa341ZScoreCombined;

            public string ModellerMolPdf;
            public string ModellerDope;
            public string ModellerDopeHr;
            public string ModellerDopeNormalized;
            public string ModellerSoap;

            public string PisaMonomerSerial1;
            public string PisaMonomerId1;
            public string PisaMonomerName1;
            public string PisaMonomerClass1;
            public string PisaMonomerTotalAtoms1;
            public string PisaMonomerTotalResidues1;
            public string PisaMonomerSurfaceAtoms1;
            public string PisaMonomerSurfaceResidues1;
            public string PisaMonomerArea1;
            public string PisaMonomerDeltaG1;


            public string PisaMonomerSerial2;
            public string PisaMonomerId2;
            public string PisaMonomerName2;
            public string PisaMonomerClass2;
            public string PisaMonomerTotalAtoms2;
            public string PisaMonomerTotalResidues2;
            public string PisaMonomerSurfaceAtoms2;
            public string PisaMonomerSurfaceResidues2;
            public string PisaMonomerArea2;
            public string PisaMonomerDeltaG2;

            public string PisaInterfaceMonomerFirstId1;
            public string PisaInterfaceMonomerSecondId1;
            public string PisaInterfaceSymmetryOperation1;
            public string PisaInterfaceSymmetryId1;
            public string PisaInterfaceArea1;
            public string PisaInterfaceDeltaG1;
            public string PisaInterfaceNumHydrogenBonds1;
            public string PisaInterfaceNumSaltBridges1;
            public string PisaInterfaceNumDisulfideBonds1;

            public string PisaInterfaceMonomerFirstId2;
            public string PisaInterfaceMonomerSecondId2;
            public string PisaInterfaceSymmetryOperation2;
            public string PisaInterfaceSymmetryId2;
            public string PisaInterfaceArea2;
            public string PisaInterfaceDeltaG2;
            public string PisaInterfaceNumHydrogenBonds2;
            public string PisaInterfaceNumSaltBridges2;
            public string PisaInterfaceNumDisulfideBonds2;

            //public string PdbToolsCoulombEp1;
            //public string PdbToolsCoulombIonStr1;
            //public string PdbToolsCoulombPh1;
            //public string PdbToolsCoulombT1;
            //public string PdbToolsCoulombDeltaG1;

            //public string PdbToolsCoulombEp2;
            //public string PdbToolsCoulombIonStr2;
            //public string PdbToolsCoulombPh2;
            //public string PdbToolsCoulombT2;
            //public string PdbToolsCoulombDeltaG2;

            public static string Header()
            {
                return string.Join(",", new string[]
                {
                    //"Result Group Number",
                    "Folder",
                    "File",
                    //"Description",
                    //"Full Structure or Interface Truncation",

                    //"Input 1 QPR Template Receptor Interface Cluster (Not scored)",
                    //"Input 1 QPR Template Receptor PDBID (Not scored)",
                    //"Input 1 QPR Template Ligand Interface Cluster",
                    //"Input 1 QPR Template Ligand PDBID (energy scores)",
                    //"Input 1 QPR Template Ligand Interface (will be overwritten)",

                    //"Input 2 QPL Sequence Protein Receptor Cluster (For ID only)",
                    //"Input 2 QPL Sequence Protein Receptor PDBID (For ID only)",
                    //"Input 2 QPL Sequence Protein Ligand Cluster",
                    //"Input 2 QPL Sequence Protein Ligand PDBID",
                    //"Input 2 QPL Sequence Protein Ligand Interface (copied to the template)",

                    //"R1",
                    //"L1",
                    //"R2",
                    //"L2",
                    //"Chain Id",

                    //"Modeller Monomer GA341 Score",
                    //"Modeller Ga341 Comptactness",
                    //"Modeller Ga341 Native Energy Pair",
                    //"Modeller Ga341 Native Energy Surface",
                    //"Modeller Ga341 Native Energy Combined",
                    //"Modeller Ga341 Z Score Pair",
                    //"Modeller Ga341 Z Score Surface",
                    //"Modeller Ga341 Z Score Combined",

                    //"Modeller Monomer molpdf Score",
                    //"Normalized DOPE Score",
                    "DOPE",
                    "DOPE-HR",
                    
                    //"SOAP",

                    //"Pisa Monomer Serial",
                    //"Pisa Monomer Id",
                    //"Pisa Monomer Total Atoms",
                    //"Pisa Monomer Total Residues",
                    //"Pisa Monomer Surface Atoms",
                    //"Pisa Monomer Surface Residues",
                    //"Pisa Monomer Area",
                    //"PISA-DG",

                    //"Pisa Interface Monomer 1st Id",
                    //"Pisa Interface Monomer 2nd Id",
                    //"Pisa Interface Area",
                    //"Pisa Interface Delta G",
                    //"Pisa Interface Hydrogen Bonds",
                    //"Pisa Interface Salt Bridges",
                    //"Pisa Interface Disulfide Bonds",

                    //"PdbTools Coulomb Ep",
                    //"PdbTools Coulomb Ion Str",
                    //"PdbTools Coulomb Ph",
                    //"PdbTools Coulomb T",
                    //"PdbTools Coulomb Delta G",
                });
            }
            public override string ToString()
            {
                return string.Join(",", new string[]
             {
                //ResultGroupNumber,
                StructureFolder,
                StructureFilename,
                //SubstitutionDescription,
                //StructuralTruncation,

                //Input1QprReceptorCluster,
                //Input1QprReceptorPdbId,
                //Input1QprLigandCluster,
                //Input1QprLigandPdbId,
                //Input1QprLigandInterface,

                //Input2QplReceptorCluster,
                //Input2QplReceptorPdbId,
                //Input2QplLigandCluster,
                //Input2QplLigandPdbId,
                //Input2QplLigandInterface, // do not make excel readable at cost of not machine readable without filtering

                //ModellerModelChainName,

                 //ModellerGa341,
                 //ModellerGa341Comptactness,
                 //ModellerGa341NativeEnergyPair,
                 //ModellerGa341NativeEnergySurface,
                 //ModellerGa341NativeEnergyCombined,
                 //ModellerGa341ZScorePair,
                 //ModellerGa341ZScoreSurface,
                 //ModellerGa341ZScoreCombined,

                 //ModellerMolPdf,
                 //ModellerDopeNormalized,
                 ModellerDope,
                ModellerDopeHr,
                
               // ModellerSoap,

                //PisaMonomerSerial1,
                //PisaMonomerId1,
                //PisaMonomerTotalAtoms1,
                //PisaMonomerTotalResidues1,
                //PisaMonomerSurfaceAtoms1,
                //PisaMonomerSurfaceResidues1,
                //PisaMonomerArea1,
                //PisaMonomerDeltaG1,

                //PisaMonomerSerial2,
                //PisaMonomerId2,
                //PisaMonomerTotalAtoms2,
                //PisaMonomerTotalResidues2,
                //PisaMonomerSurfaceAtoms2,
                //PisaMonomerSurfaceResidues2,
                //PisaMonomerArea2,
                //PisaMonomerDeltaG2,

                //PisaInterfaceMonomerFirstId1,
                //PisaInterfaceMonomerSecondId1,
                //PisaInterfaceSymmetryOperation1,
                //PisaInterfaceSymmetryId1,
                //PisaInterfaceArea1,
                //PisaInterfaceDeltaG1,
                //PisaInterfaceNumHydrogenBonds1,
                //PisaInterfaceNumSaltBridges1,
                //PisaInterfaceNumDisulfideBonds1,

                //PisaInterfaceMonomerFirstId2,
                //PisaInterfaceMonomerSecondId2,
                //PisaInterfaceSymmetryOperation2,
                //PisaInterfaceSymmetryId2,
                //PisaInterfaceArea2,
                //PisaInterfaceDeltaG2,
                //PisaInterfaceNumHydrogenBonds2,
                //PisaInterfaceNumSaltBridges2,
                //PisaInterfaceNumDisulfideBonds2,

                //PdbToolsCoulombEp1,
                //PdbToolsCoulombIonStr1,
                //PdbToolsCoulombPh1,
                //PdbToolsCoulombT1,
                //PdbToolsCoulombDeltaG1,

                //PdbToolsCoulombEp2,
                //PdbToolsCoulombIonStr2,
                //PdbToolsCoulombPh2,
                //PdbToolsCoulombT2,
                //PdbToolsCoulombDeltaG2,
                });
            }
        }

        public static void Main(string[] args)
        {
            //var logResultsFolder = @"c:\r\r\";
            //var logResultsFolder = @"c:\r-some modelled\";
            //var logResultsFolder = @"c:\r\" ; //args[0];
            //var saveFile = args[1];

            //var logResultsFolder = @"c:\pdbe_split\models\" ; //args[0];
            //var logResultsFolder = @"C:\pdbe_split\manual\sw_1SBNI_2SICI_4GI3C\"; //args[0];
            //var logResultsFolder = @"C:\pdbe_split\manual\sw_1OYVI_1R0RI_1SBNI_1V5IB_2SICI_3BX1C_4GI3C_4LVNP\"; //args[0];

            var logResultsFolder = @"C:\pdbe_split\manual\sw_3BX1C\"; //args[0];
            //C:\pdbe_split\manual\sw_1H1VG_1KXPD_1RGIG_1T44G_3JBIV_4EAHA_4PKHB_5AFUb

            var seq = ProteinBioinformaticsSharedLibrary.Sequence.LoadSequenceFile(logResultsFolder + "sequences.fasta");
            var inf = ProteinBioinformaticsSharedLibrary.Sequence.LoadSequenceFile(logResultsFolder + "interfaces_fixed_length.fasta");

            foreach (var s1 in seq)
            {
                var r = new List<Tuple<string, ProteinBioClass.AlignmentScore>>();

                foreach (var s2 in seq)
                {
                    //if (s1==s2) continue;

                    var nmw = new NeedlemanWunsch(s1.FullSequence, s2.FullSequence);

                    var a = nmw.getAlignment();

                    ProteinBioClass.AlignmentScore s = ProteinBioClass.SequenceSimilarityPercentage(a[0], a[1], ProteinBioClass.AlignmentIdentityOption.MinimumSequenceLength);

                    //r.Add(s1.Id.Substring(1, 5) + " " + s2.Id.Substring(1, 5) + " " + s.Score + " " + s.ScoreEvo);
                    r.Add(new Tuple<string, ProteinBioClass.AlignmentScore>(s1.Id.Substring(1, 5) + "," + s2.Id.Substring(1, 5), s));
                }
                r = r.OrderByDescending(a => a.Item2.Score).ThenByDescending(a => a.Item2.ScoreEvo).ToList();
                var e = r.Select(a => a.Item1 + "," + string.Format("{0:0.00}", Math.Round(a.Item2.Score, 2)) + "," + string.Format("{0:0.00}", Math.Round(a.Item2.ScoreEvo, 2))).ToList();
                e.Insert(0, "Sequence Alignment");
                e.Insert(1, "ID1,ID2,Match%,Physicochemical%");

                e = e.Select(a => a.Replace(",", "\t")).ToList();
                File.WriteAllLines(logResultsFolder + "score_all_" + s1.Id.Substring(1, 5) + ".txt", e);
            }

            foreach (var s1 in inf)
            {
                var r = new List<Tuple<string, ProteinBioClass.AlignmentScore>>();

                foreach (var s2 in inf)
                {
                    //if (s1==s2) continue;

                    var nmw = new NeedlemanWunsch(s1.FullSequence, s2.FullSequence);

                    var a = nmw.getAlignment();

                    ProteinBioClass.AlignmentScore s = ProteinBioClass.SequenceSimilarityPercentage(a[0], a[1], ProteinBioClass.AlignmentIdentityOption.MinimumSequenceLength);

                    //r.Add(s1.Id.Substring(1, 5) + " " + s2.Id.Substring(1, 5) + " " + s.Score + " " + s.ScoreEvo);
                    r.Add(new Tuple<string, ProteinBioClass.AlignmentScore>(s1.Id.Substring(1, 5) + "," + s2.Id.Substring(1, 5), s));
                }
                r = r.OrderByDescending(a => a.Item2.Score).ThenByDescending(a => a.Item2.ScoreEvo).ToList();
                var e = r.Select(a => a.Item1 + "," + string.Format("{0:0.00}", Math.Round(a.Item2.Score, 2)) + "," + string.Format("{0:0.00}", Math.Round(a.Item2.ScoreEvo, 2))).ToList();
                e.Insert(0, "Interface Alignment");
                e.Insert(1, "ID1,ID2,Match%,Physicochemical%");
                e.Insert(0,"");

                e = e.Select(a => a.Replace(",", "\t")).ToList();
                File.AppendAllLines(logResultsFolder + "score_all_" + s1.Id.Substring(1, 5) + ".txt", e);
            }
            //return;

            //r-some modelled

            //var pdbFileNames = Directory.GetFiles(logResultsFolder, "*.pdb", SearchOption.AllDirectories).Select(a=>Path.GetFileName(a).ToLowerInvariant()).Distinct().ToList();

            var modellerLogFiles = Directory.GetFiles(logResultsFolder, "modeller_monomer_assessment.log", SearchOption.AllDirectories).ToList();
            //modellerLogFiles = modellerLogFiles.Where(a => a.StartsWith(logResultsFolder + @"sw2\")).ToList();
            //var dimerModellerLogFiles = Directory.GetFiles(logResultsFolder, "modeller_dimer_assessment.log", SearchOption.AllDirectories).ToList();

            //var pisaLogFiles = Directory.GetFiles(logResultsFolder, "pisa_monomer_assessment.log", SearchOption.AllDirectories).ToList();

            var data = new List<List<string>>();
            var nats = new List<List<string>>();

            var rowlen = 0;

            var scores = modellerLogFiles.SelectMany(m => ParseModellerLog(m)).ToList();
            foreach (var scoreGroup in scores.GroupBy(a =>
                                                      {
                                                          var structureFolderSplit = a.StructureFolder.Split('\\');
                                                          // \                                       -4        \ -3  \  -2 \    -1           \
                                                          // \sw_1OYVI_1R0RI_1SBNI_1V5IB_2SICI_3BX1C_4GI3C_4LVNP\1V5IB\1V5IB\all_0016_0026_1_1\
                                                          return structureFolderSplit[structureFolderSplit.Length - 1].Substring(0, 3) + '_' + structureFolderSplit[structureFolderSplit.Length - 3] + '_' + structureFolderSplit[structureFolderSplit.Length - 2];
                                                      }))
            {
                var group = scoreGroup.ToList();
                group = group.OrderBy(a => a.StructureFolder).ToList();

                var natives1 = group.Where(a => a.StructureFolder.Contains("_native")).ToList();



                foreach (var n in natives1)
                {
                    nats.Add(new List<string>() { "nat_" + scoreGroup.Key.Substring(4) , n.ModellerDope});                        
                }



                //data.Add(group.Select(a => a.ModellerDope).ToList());

                if (!scoreGroup.Key.StartsWith("nat"))
                {
                    // make index line
                    if (scoreGroup.Key.Substring(4, 5) == scoreGroup.Key.Substring(10, 5))
                    {
                        //data.Add(new List<string>());

                        data.Add(group.Select(a => a.StructureFolder.Split('\\').Last().Substring(4)).ToList());

                        rowlen = data[data.Count - 1].Count;


                        data[data.Count - 1].Insert(0, scoreGroup.Key + "_index");

                    }
                }


                data.Add(group.Select(a => a.ModellerDope).ToList());
                data[data.Count - 1].Insert(0, scoreGroup.Key + "_energy");
            }

            var output = new List<string>();
            var nats2 = nats.Select(a => string.Join(",", a)).Distinct().OrderBy(a=>a[0]).ToList();

            //nats = nats.Distinct().OrderBy(a => a[0]).ToList();

            foreach (var g in data.GroupBy(a => a[0].Substring(0, 3+1+5)))
            {
                var gi = g.ToList();

                var index = gi.First(a => a[0].Contains("_index"));
                var len = index.Count - 1;
                var main = gi.First(a => a != index && a[0].Substring(4, 5) == a[0].Substring(10, 5));
                var others = gi.Where(a => a != index && a != main).OrderBy(a=>a[0]).ToList();

                var natives = nats2.Where(a => a.Substring(4, 5) == index[0].Substring(4, 5)).OrderBy(a=>a[0]).ToList();
                natives = natives.Select(a =>
                                         {
                                             
                                             var b = a.Split(',');
                                             var r = b[0];
                                             for (var j = 0; j < len; j++) { r = r + ',' + string.Join(",", b.Skip(1).ToList()); }
                                             return r;
                                         }).ToList();

                var nativemain = natives.First(a => a.Substring(4, 5) == a.Substring(10, 5));
                natives.Remove(nativemain);

                output.Add(string.Join(",", index));
                output.Add(string.Join(",", main));
                others.ForEach(a => output.Add(string.Join(",", a)));
                output.Add(string.Join(",", nativemain));
                natives.ForEach(a => output.Add(string.Join(",", a)));
                output.Add("");
            }

            //var output = data.Select(a => string.Join(",", a))
            //    .Distinct()
            //    .OrderByDescending(a => a.Substring(4, 5))
            //    .ThenBy(a => a.Substring(0, 3))
            //    .ThenByDescending(a => a.Substring(4, 5) == a.Substring(10, 5))
            //    .ThenByDescending(a => a.Contains("_index"))
            //    .ToList();


            //for (var j = output.Count - 1; j >= 0; j--)
            //{
            //    if (output[j].Contains("_index"))
            //        output.Insert(j, "");
            //}
            File.WriteAllLines(logResultsFolder + Environment.MachineName + "_energy.csv", output);
        }

        //var scoresGrouped = scores.GroupBy(a => new Tuple<string, bool>(a.StructureFilename, a.StructureFolder.Contains("Reversed")));

        //foreach (var scoreGrouped in scoresGrouped)
        //{

        //var output = scoreGrouped.ToList().Select(a=>a.ToString()).ToList();// .SelectMany(a => a.SubstitutionDescription == "Native (No change)" ? new[] {"", a.ToString()} : new[] {a.ToString()}).ToList();
        //var output = scores.ToList().Select(a => a.ToString()).ToList();// .SelectMany(a => a.SubstitutionDescription == "Native (No change)" ? new[] {"", a.ToString()} : new[] {a.ToString()}).ToList();

        //output.Insert(0, EnergyScore.Header());
        //scores[1].ToString()
        //File.WriteAllLines(logResultsFolder + Environment.MachineName + "_energy.csv", output);

        //Environment.MachineName
        //File.WriteAllLines(saveFile, output);
        //}
        //}

        /*
            var pisaLogFiles = Directory.GetFiles(logResultsFolder, "pisa_monomer.log", SearchOption.AllDirectories);
            //var pdbtoolsLogFiles = Directory.GetFiles(logResultsFolder, "pdbtools_monomer.log", SearchOption.AllDirectories);
            var infoLogFiles = Directory.GetFiles(logResultsFolder, "substitution.log", SearchOption.AllDirectories);

            var folders = new List<string>();

            folders.AddRange(modellerLogFiles.Select(Path.GetDirectoryName).ToList());
            folders.AddRange(pisaLogFiles.Select(Path.GetDirectoryName).ToList());
            //folders.AddRange(pdbtoolsLogFiles.Select(Path.GetDirectoryName).ToList());
            folders.AddRange(infoLogFiles.Select(Path.GetDirectoryName).ToList());
            folders = folders.Distinct().ToList();

            var scores = new List<EnergyScore>();

            for (int folderIndex = 0; folderIndex < folders.Count; folderIndex++)
            {
                var folder = folders[folderIndex];
                var modellerMonomerLogFile = folder + @"\modeller_monomer_assessment.log";
                var modellerDimerLogFile = folder + @"\modeller_dimer_assessment.log";
                var pisaMonomerLogFile = folder + @"\pisa_monomer.log";
                var pisaDimerLogFile = folder + @"\pisa_dimer.log";
                //var pdbtoolsLogFile = folder + @"\pdbtools_monomer.log";
                var infoLogFile = folder + @"\substitution.log";

                var pisaMonomerLogFileLines = new string[0];
                var pisaDimerLogFileLines = new string[0];
                var pdbtoolsLogFileLines = new string[0];
                var modellerMonomerLogFileLines = new string[0];
                var modellerDimerLogFileLines = new string[0];
                var infoLogFileLines = new string[0];

                if (File.Exists(modellerMonomerLogFile)) modellerMonomerLogFileLines = File.ReadAllLines(modellerMonomerLogFile);
                if (File.Exists(modellerDimerLogFile)) modellerMonomerLogFileLines = File.ReadAllLines(modellerMonomerLogFile);
                if (File.Exists(pisaMonomerLogFile)) pisaMonomerLogFileLines = File.ReadAllLines(pisaMonomerLogFile);
                if (File.Exists(pisaDimerLogFile)) pisaDimerLogFileLines = File.ReadAllLines(pisaDimerLogFile);
                
                if (File.Exists(infoLogFile)) infoLogFileLines = File.ReadAllLines(infoLogFile);

                if (pisaMonomerLogFileLines.Length == 0 && pdbtoolsLogFileLines.Length == 0 && modellerMonomerLogFileLines.Length == 0 &&
                    infoLogFileLines.Length == 0) continue;

                var folderNameSplit = folder.Replace(logResultsFolder, @"").Replace('\\', ',').Split(',');

                var infoScore = new EnergyScore();

                infoScore.StructureFolder = folder;

                infoScore.StructuralTruncation = folderNameSplit[0];

                var line = -1;



                infoScore.Input1QprReceptorCluster = infoLogFileLines[++line] + "-" + infoLogFileLines[++line];
                infoScore.Input1QprLigandCluster = infoLogFileLines[++line] + "-" + infoLogFileLines[++line];

                infoScore.Input2QplReceptorCluster = infoLogFileLines[++line] + "-" + infoLogFileLines[++line];
                infoScore.Input2QplLigandCluster = infoLogFileLines[++line] + "-" + infoLogFileLines[++line];

                infoScore.Input1QprReceptorPdbId = infoLogFileLines[++line] + "-" + infoLogFileLines[++line];
                infoScore.Input1QprLigandPdbId = infoLogFileLines[++line] + "-" + infoLogFileLines[++line];

                infoScore.Input2QplReceptorPdbId = infoLogFileLines[++line] + "-" + infoLogFileLines[++line];
                infoScore.Input2QplLigandPdbId = infoLogFileLines[++line] + "-" + infoLogFileLines[++line];

                infoScore.SubstitutionDescription = infoLogFileLines[++line].Replace("_", " ").Replace("Reversed", "(Reversed)");

                infoScore.Input1QprLigandInterface = infoLogFileLines[++line];

                ++line;

                infoScore.Input2QplLigandInterface = infoLogFileLines[++line];


                
                //var monomerStructFiles = new List<string> { "model_monomer.pdb", "model_locked_monomer.pdb", "model_fragment_monomer.pdb" };
                //var dimerStructFiles = new List<string>   {"model_dimer.pdb", "model_locked_dimer.pdb", "model_fragment_dimer.pdb"};

                //crystal structure
                //if (infoScore.Input2QplLigandCluster == infoScore.Input1QprLigandCluster && infoScore.SubstitutionDescription.Replace("_", " ").ToUpperInvariant() == "Query Interface".ToUpperInvariant()) { structFiles.Add("template_ligand_all.pdb"); }

                foreach (var p in pdbFileNames)
                {
                    var modellerMonomerLogResult = ParseModellerLog(modellerMonomerLogFileLines, infoScore, new List<string>() { p });
                    //var modellerDimerLogResult = ParseModellerLog(modellerDimerLogFileLines, infoScore, new List<string>() {p});
                    var pisaMonomerLogResult = ParsePisaLog(pisaMonomerLogFileLines, infoScore, new List<string>() { p });
                    //var pisaDimerLogResult = ParsePisaLog(pisaDimerLogFileLines, infoScore, dimerStructFiles);
                    //var pdbtoolsLogResult = ParsePdbtoolsLog(pdbtoolsLogFileLines, infoScore, structFiles);



                    //var monomerStructFile = monomerStructFiles[index];
                    //var dimerStructFile = dimerStructFiles[index];

                          var score = new EnergyScore();
                    
                        var modellerMonomerScore = modellerMonomerLogResult.FirstOrDefault(a => a.StructureFilename == p);
                        //var modellerDimerScoreA = modellerDimerLogResult.FirstOrDefault(a => a.StructureFilename == dimerStructFile);
                        //var modellerDimerScoreB = modellerDimerLogResult.Skip(1).FirstOrDefault(a => a.StructureFilename == dimerStructFile);

                        var pisaMonomerScore = pisaMonomerLogResult.FirstOrDefault(a => a.StructureFilename == p);
                        //var pisaDimerScoreA = pisaDimerLogResult.FirstOrDefault(a => a.StructureFilename == dimerStructFile);
                        //var pisaDimerScoreB = pisaDimerLogResult.Skip(1).FirstOrDefault(a => a.StructureFilename == dimerStructFile);

                        //var pdbtoolsScore = pdbtoolsLogResult.FirstOrDefault(a => a.StructureFilename == structFile);

                        //score.ResultGroupNumber = infoScore.ResultGroupNumber;
                        score.StructureFilename = p;
                        score.StructureFolder = infoScore.StructureFolder;
                        score.StructuralTruncation = infoScore.StructuralTruncation;

                        score.Input1QprReceptorCluster = infoScore.Input1QprReceptorCluster;
                        score.Input1QprLigandCluster = infoScore.Input1QprLigandCluster;
                        score.Input2QplReceptorCluster = infoScore.Input2QplReceptorCluster;
                        score.Input2QplLigandCluster = infoScore.Input2QplLigandCluster;
                        score.Input1QprReceptorPdbId = infoScore.Input1QprReceptorPdbId;
                        score.Input1QprLigandPdbId = infoScore.Input1QprLigandPdbId;
                        score.Input2QplReceptorPdbId = infoScore.Input2QplReceptorPdbId;
                        score.Input2QplLigandPdbId = infoScore.Input2QplLigandPdbId;
                        score.SubstitutionDescription = infoScore.SubstitutionDescription;
                        score.Input1QprLigandInterface = infoScore.Input1QprLigandInterface;
                        score.Input2QplLigandInterface = infoScore.Input2QplLigandInterface;

                        if (modellerMonomerScore != null)
                        {
                            score.ModellerModelChainName = modellerMonomerScore.ModellerModelChainName;

                            score.ModellerGa341 = modellerMonomerScore.ModellerGa341;
                            score.ModellerGa341Comptactness = modellerMonomerScore.ModellerGa341Comptactness;
                            score.ModellerGa341NativeEnergyCombined = modellerMonomerScore.ModellerGa341NativeEnergyCombined;
                            score.ModellerGa341NativeEnergyPair = modellerMonomerScore.ModellerGa341NativeEnergyPair;
                            score.ModellerGa341NativeEnergySurface = modellerMonomerScore.ModellerGa341NativeEnergySurface;
                            score.ModellerGa341ZScoreCombined = modellerMonomerScore.ModellerGa341ZScoreCombined;
                            score.ModellerGa341ZScorePair = modellerMonomerScore.ModellerGa341ZScorePair;
                            score.ModellerGa341ZScoreSurface = modellerMonomerScore.ModellerGa341ZScoreSurface;

                            score.ModellerMolPdf = modellerMonomerScore.ModellerMolPdf;
                            score.ModellerDope = modellerMonomerScore.ModellerDope;
                            score.ModellerDopeHr = modellerMonomerScore.ModellerDopeHr;
                            score.ModellerDopeNormalized = modellerMonomerScore.ModellerDopeNormalized;
                            score.ModellerSoap = modellerMonomerScore.ModellerSoap;
                        }

                        if (pisaMonomerScore != null)
                        {
                            score.PisaMonomerSerial1 = pisaMonomerScore.PisaMonomerSerial1;
                            score.PisaMonomerId1 = pisaMonomerScore.PisaMonomerId1;
                            score.PisaMonomerName1 = pisaMonomerScore.PisaMonomerName1;
                            score.PisaMonomerClass1 = pisaMonomerScore.PisaMonomerClass1;
                            score.PisaMonomerTotalAtoms1 = pisaMonomerScore.PisaMonomerTotalAtoms1;
                            score.PisaMonomerTotalResidues1 = pisaMonomerScore.PisaMonomerTotalResidues1;
                            score.PisaMonomerSurfaceAtoms1 = pisaMonomerScore.PisaMonomerSurfaceAtoms1;
                            score.PisaMonomerSurfaceResidues1 = pisaMonomerScore.PisaMonomerSurfaceResidues1;
                            score.PisaMonomerArea1 = pisaMonomerScore.PisaMonomerArea1;
                            score.PisaMonomerDeltaG1 = pisaMonomerScore.PisaMonomerDeltaG1;
                        }


                        //if (pdbtoolsScore != null)
                        //{
                        //    score.PdbToolsCoulombEp1 = pdbtoolsScore.PdbToolsCoulombEp1;
                        //    score.PdbToolsCoulombIonStr1 = pdbtoolsScore.PdbToolsCoulombIonStr1;
                        //    score.PdbToolsCoulombPh1 = pdbtoolsScore.PdbToolsCoulombPh1;
                        //    score.PdbToolsCoulombT1 = pdbtoolsScore.PdbToolsCoulombT1;
                        //    score.PdbToolsCoulombDeltaG1 = pdbtoolsScore.PdbToolsCoulombDeltaG1;
                        //}

                        if (score.StructureFilename == "template_ligand_all.pdb" && score.SubstitutionDescription.Replace("_", " ").ToUpperInvariant() == "Query Interface".ToUpperInvariant()) { score.SubstitutionDescription = "Native (No change)"; }
                        else if (score.Input2QplLigandCluster == score.Input1QprLigandCluster && score.SubstitutionDescription.Replace("_", " ").ToUpperInvariant() == "Query Interface".ToUpperInvariant()) { score.SubstitutionDescription = "Native Theoretical Model"; }
                        else if (score.Input2QplLigandCluster == score.Input1QprLigandCluster && score.SubstitutionDescription.Replace("_", " ").ToUpperInvariant() == "Query Interface (Reversed)".ToUpperInvariant()) { score.SubstitutionDescription = "Native Theoretical Model (Reversed)"; }

                        scores.Add(score);
                    //}
                }
                var subDesc = new List<string>
                {
                "Native (No change)",
                "Native Theoretical Model",
                "Query Interface",
                "Similar Physiochemical",
                "Similar Propensity",
                "Similar Blosum62",
                "Dissimilar Physiochemical",
                "Dissimilar Propensity",
                "Dissimilar Blosum62",
                };

                subDesc.AddRange(subDesc.Select(s => s + " (Reversed)").ToList());
                subDesc = subDesc.Select(a => a.Replace("_", " ").ToUpperInvariant()).ToList();

                var scoresDistinct = scores.Select(a => new
                {
                    Input1QPR_ReceptorCluster = a.Input1QprReceptorCluster,
                    Input1QPR_LigandCluster = a.Input1QprLigandCluster,
                    Input2QPL_ReceptorCluster = a.Input2QplReceptorCluster,
                }).Distinct().ToList();

                scores =
                                    scores.OrderBy(a => a.StructuralTruncation.Trim())
                                        .ThenBy(a => a.StructureFolder.Substring(0, a.StructureFolder.LastIndexOfAny(new char[] { '/', '\\' })).Trim())

                                    .ThenBy(a => subDesc.IndexOf(a.SubstitutionDescription.Replace("_", " ").ToUpperInvariant()))
                                    .ToList();
                var scoresGrouped = scores.GroupBy(a => new Tuple<string,bool>(a.StructureFilename,a.SubstitutionDescription.Contains("(Reversed)")));
                foreach (var scoreGrouped in scoresGrouped)
                {
                    
                    var output = scoreGrouped.ToList().SelectMany(a => a.SubstitutionDescription == "Native (No change)" ? new[] {"", a.ToString()} : new[] {a.ToString()}).ToList();
                    output.Insert(0, EnergyScore.Header());
                    File.WriteAllLines(logResultsFolder + "energy_" + scoreGrouped.Key.Item1 + (scoreGrouped.Key.Item2?" reversed":"") +  ".csv", output);
                }

                
            }
            */
        //}

        //public static List<EnergyScore> ParsePdbtoolsLog(string[] pdbtoolsLogFileLines, EnergyScore paramEnergyScore, List<string> structFiles)
        //{
        //    var monomerMarker = "                                     pdb        ep   ion_str        pH         T        dG";

        //    var result = new List<EnergyScore>();
        //    var score = new EnergyScore();

        //    for (int index = 0; index < pdbtoolsLogFileLines.Length; index++)
        //    {
        //        var line = pdbtoolsLogFileLines[index];
        //        var lineSplit = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        //        if (line.StartsWith("; pdb_coulomb.py")) score.StructureFilename = lineSplit[2];

        //        if (line == monomerMarker)
        //        {
        //            var line2 = pdbtoolsLogFileLines[index + 1];
        //            var lineSplit2 = line2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(a => a != "|").ToList();

        //            if (structFiles.Contains(score.StructureFilename))
        //            {
        //                score.PdbToolsCoulombEp1 = lineSplit2[2];
        //                score.PdbToolsCoulombIonStr1 = lineSplit2[3];
        //                score.PdbToolsCoulombPh1 = lineSplit2[4];
        //                score.PdbToolsCoulombT1 = lineSplit2[5];
        //                score.PdbToolsCoulombDeltaG1 = lineSplit2[6];

        //                result.Add(score);
        //                score = new EnergyScore();
        //            }
        //        }

        //    }

        //    return result;
        //}

        public static List<EnergyScore> ParsePisaLog(string[] pisaLogFileLines, /*EnergyScore paramEnergyScore,*/ List<string> structFiles = null)
        {
            var monomerMarker = "  ## Id |   Monomer   |  Class    Nat Nres   Sat Sres     Area   DeltaG";
            //var interfaceMarker = "  ## Id |   Monomer1  |   Monomer2    Symmetry operation Sym.Id |   Area  DeltaG Nhb Nsb Nds";


            var result = new List<EnergyScore>();
            var score = new EnergyScore();

            for (int index = 0; index < pisaLogFileLines.Length; index++)
            {
                var line = pisaLogFileLines[index];
                var lineSplit = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (line.StartsWith("; pisa")) score.StructureFilename = lineSplit[2];

                if (line == monomerMarker)
                {
                    var line2 = pisaLogFileLines[index + 2];
                    var lineSplit2 = line2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(a => a != "|").ToList();

                    if (structFiles == null || structFiles.Count == 0 || structFiles.Contains(score.StructureFilename))
                    {
                        score.PisaMonomerSerial1 = lineSplit2[0];
                        score.PisaMonomerId1 = lineSplit2[1];
                        score.PisaMonomerName1 = lineSplit2[2];
                        score.PisaMonomerClass1 = lineSplit2[3];
                        score.PisaMonomerTotalAtoms1 = lineSplit2[4];
                        score.PisaMonomerTotalResidues1 = lineSplit2[5];
                        score.PisaMonomerSurfaceAtoms1 = lineSplit2[6];
                        score.PisaMonomerSurfaceResidues1 = lineSplit2[7];
                        score.PisaMonomerArea1 = lineSplit2[8];
                        score.PisaMonomerDeltaG1 = lineSplit2[9];


                        result.Add(score);
                        score = new EnergyScore();
                    }
                }

            }

            return result;
        }

        public static List<EnergyScore> ParseModellerLog(string modellerLogFilename, /*EnergyScore paramEnergyScore,*/ List<string> structFiles = null)
        {

            var folder = Path.GetDirectoryName(modellerLogFilename);
            var file = Path.GetFileName(modellerLogFilename);

            var modellerLogFileLines = File.ReadAllLines(modellerLogFilename);

            var modellerLogFileMarkers = new string[] { "; ENERGY ", "; FILE ", "; CHAIN ", "; FINISHED " };
            modellerLogFileLines = modellerLogFileLines.Where(a => modellerLogFileMarkers.Any(a.StartsWith)).ToArray();

            var result = new List<EnergyScore>();
            var score = new EnergyScore();
            score.StructureFolder = folder;
            score.StructureFilename = file;
            score.SubstitutionDescription = "";
            score.StructuralTruncation = "";

            foreach (var line in modellerLogFileLines)
            {
                var lineSplit = line.Split(' ').ToList();
                lineSplit.RemoveAt(0);

                if (lineSplit[0] == "FILE")
                {
                    score.StructureFilename = lineSplit[1];
                    continue;
                }

                if (lineSplit[0] == "CHAIN")
                {
                    //if (structFiles==null|| structFiles.Count == 0 || structFiles.Contains(score.StructureFilename))
                    {
                        score.ModellerModelChainName = lineSplit[1];
                    }
                    continue;
                }

                if (lineSplit[0] == "ENERGY")
                {
                    //if (structFiles==null|| structFiles.Count == 0 || structFiles.Contains(score.StructureFilename))
                    {
                        if (lineSplit[1] == "assess_ga341")
                        {
                            var ga341Index = -1;
                            var ga341 = line.Substring("; ENERGY assess_ga341 ".Length).Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
                            score.ModellerGa341 = ga341[++ga341Index];
                            score.ModellerGa341Comptactness = ga341[++ga341Index];
                            score.ModellerGa341NativeEnergyPair = ga341[++ga341Index];
                            score.ModellerGa341NativeEnergySurface = ga341[++ga341Index];
                            score.ModellerGa341NativeEnergyCombined = ga341[++ga341Index];
                            score.ModellerGa341ZScoreSurface = ga341[++ga341Index];
                            score.ModellerGa341ZScorePair = ga341[++ga341Index];
                            score.ModellerGa341ZScoreCombined = ga341[++ga341Index];
                        }
                        if (lineSplit[1] == "molpdf") score.ModellerMolPdf = lineSplit[2];
                        if (lineSplit[1] == "assess_dope") score.ModellerDope = string.IsNullOrWhiteSpace( lineSplit[2] ) ? "" : string.Format("{0:0.00}",Math.Round(decimal.Parse(lineSplit[2]),2));
                        if (lineSplit[1] == "assess_dopehr") score.ModellerDopeHr = lineSplit[2];
                        if (lineSplit[1] == "assess_normalized_dope") score.ModellerDopeNormalized = lineSplit[2];
                        if (lineSplit[1] == "assess_soap_protein_od") score.ModellerSoap = lineSplit[2];
                    }
                    continue;
                }

                if (lineSplit[0] == "FINISHED")
                {
                    var lastScore = score;
                    //if (structFiles.Contains(score.StructureFilename))
                    //{
                    result.Add(lastScore);
                    //}

                    score = new EnergyScore();
                    score.StructureFolder = lastScore.StructureFolder;
                    score.StructureFilename = lastScore.StructureFilename;
                    score.SubstitutionDescription = lastScore.SubstitutionDescription;
                    score.StructuralTruncation = lastScore.StructuralTruncation;
                }
            }
            return result;
        }
    }
}
