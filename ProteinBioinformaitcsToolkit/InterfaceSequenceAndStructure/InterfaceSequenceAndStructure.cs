using ProteinBioinformaticsSharedLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;

namespace InterfaceSequenceAndStructure
{
    public class InterfaceSequenceAndStructure
    {
        public class InterfaceFragment
        {
            public int FragmentLength;
            public string PdbId;
            public char ReceptorChainId;
            public int ReceptorResSeq;
            public int ReceptorIndex;
            public string ReceptorAminoAcidSequence;
            public string ReceptorSecondaryStructure;
            public char LigandChainId;
            public int LigandResSeq;
            public int LigandIndex;
            public string LigandAminoAcidSequence;
            public string LigandSecondaryStructure;

            public override string ToString()
            {
                return
                    string.Join(",",
                    new string[]
                    {
          ""+FragmentLength,
           PdbId,
          ""+ReceptorChainId,
          ""+ReceptorResSeq,
          ""+ReceptorIndex,
           ReceptorAminoAcidSequence,
           ReceptorSecondaryStructure,
          ""+LigandChainId,
          ""+LigandResSeq,
          ""+LigandIndex,
           LigandAminoAcidSequence,
           LigandSecondaryStructure,
        });



            }

            private sealed class InterfaceFragmentEqualityComparer : IEqualityComparer<InterfaceFragment>
            {
                public bool Equals(InterfaceFragment x, InterfaceFragment y)
                {
                    if (ReferenceEquals(x, y)) return true;
                    if (ReferenceEquals(x, null)) return false;
                    if (ReferenceEquals(y, null)) return false;
                    if (x.GetType() != y.GetType()) return false;
                    return x.FragmentLength == y.FragmentLength && string.Equals(x.PdbId, y.PdbId) && x.ReceptorChainId == y.ReceptorChainId && x.ReceptorResSeq == y.ReceptorResSeq && x.ReceptorIndex == y.ReceptorIndex && string.Equals(x.ReceptorAminoAcidSequence, y.ReceptorAminoAcidSequence) && string.Equals(x.ReceptorSecondaryStructure, y.ReceptorSecondaryStructure) && x.LigandChainId == y.LigandChainId && x.LigandResSeq == y.LigandResSeq && x.LigandIndex == y.LigandIndex && string.Equals(x.LigandAminoAcidSequence, y.LigandAminoAcidSequence) && string.Equals(x.LigandSecondaryStructure, y.LigandSecondaryStructure);
                }

                public int GetHashCode(InterfaceFragment obj)
                {
                    unchecked
                    {
                        var hashCode = obj.FragmentLength;
                        hashCode = (hashCode*397) ^ (obj.PdbId != null ? obj.PdbId.GetHashCode() : 0);
                        hashCode = (hashCode*397) ^ obj.ReceptorChainId.GetHashCode();
                        hashCode = (hashCode*397) ^ obj.ReceptorResSeq;
                        hashCode = (hashCode*397) ^ obj.ReceptorIndex;
                        hashCode = (hashCode*397) ^ (obj.ReceptorAminoAcidSequence != null ? obj.ReceptorAminoAcidSequence.GetHashCode() : 0);
                        hashCode = (hashCode*397) ^ (obj.ReceptorSecondaryStructure != null ? obj.ReceptorSecondaryStructure.GetHashCode() : 0);
                        hashCode = (hashCode*397) ^ obj.LigandChainId.GetHashCode();
                        hashCode = (hashCode*397) ^ obj.LigandResSeq;
                        hashCode = (hashCode*397) ^ obj.LigandIndex;
                        hashCode = (hashCode*397) ^ (obj.LigandAminoAcidSequence != null ? obj.LigandAminoAcidSequence.GetHashCode() : 0);
                        hashCode = (hashCode*397) ^ (obj.LigandSecondaryStructure != null ? obj.LigandSecondaryStructure.GetHashCode() : 0);
                        return hashCode;
                    }
                }
            }

            private static readonly IEqualityComparer<InterfaceFragment> InterfaceFragmentComparerInstance = new InterfaceFragmentEqualityComparer();
            public static IEqualityComparer<InterfaceFragment> InterfaceFragmentComparer { get { return InterfaceFragmentComparerInstance; } }

            protected bool Equals(InterfaceFragment other) {
                return FragmentLength == other.FragmentLength && string.Equals(PdbId, other.PdbId) && ReceptorChainId == other.ReceptorChainId && ReceptorResSeq == other.ReceptorResSeq && ReceptorIndex == other.ReceptorIndex && string.Equals(ReceptorAminoAcidSequence, other.ReceptorAminoAcidSequence) && string.Equals(ReceptorSecondaryStructure, other.ReceptorSecondaryStructure) && LigandChainId == other.LigandChainId && LigandResSeq == other.LigandResSeq && LigandIndex == other.LigandIndex && string.Equals(LigandAminoAcidSequence, other.LigandAminoAcidSequence) && string.Equals(LigandSecondaryStructure, other.LigandSecondaryStructure);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((InterfaceFragment) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = FragmentLength;
                    hashCode = (hashCode*397) ^ (PdbId != null ? PdbId.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ ReceptorChainId.GetHashCode();
                    hashCode = (hashCode*397) ^ ReceptorResSeq;
                    hashCode = (hashCode*397) ^ ReceptorIndex;
                    hashCode = (hashCode*397) ^ (ReceptorAminoAcidSequence != null ? ReceptorAminoAcidSequence.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ (ReceptorSecondaryStructure != null ? ReceptorSecondaryStructure.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ LigandChainId.GetHashCode();
                    hashCode = (hashCode*397) ^ LigandResSeq;
                    hashCode = (hashCode*397) ^ LigandIndex;
                    hashCode = (hashCode*397) ^ (LigandAminoAcidSequence != null ? LigandAminoAcidSequence.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ (LigandSecondaryStructure != null ? LigandSecondaryStructure.GetHashCode() : 0);
                    return hashCode;
                }
            }

            public static bool operator ==(InterfaceFragment left, InterfaceFragment right) { return Equals(left, right); }
            public static bool operator !=(InterfaceFragment left, InterfaceFragment right) { return !Equals(left, right); }

            public static void Save(string filename, List<InterfaceFragment> ifList)
            {
                File.WriteAllLines(filename, ifList.Select(a => a.ToString()).ToList());
            }

            public static List<InterfaceFragment> Load(string filename)
            {
                if (!File.Exists(filename)) return new List<InterfaceFragment>();

                var d = File.ReadAllLines(filename);

                var result = new List<InterfaceFragment>();

                foreach (var l in d)
                {
                    var p = -1;
                    var s = l.Split(',');
                    result.Add(new InterfaceFragment()
                    {
                       
                           FragmentLength= int.Parse(s[++p]),
                        PdbId = s[++p],
                         ReceptorChainId= s[++p][0],
                         ReceptorResSeq= int.Parse(s[++p]),
                        ReceptorIndex = int.Parse(s[++p]),
                        ReceptorAminoAcidSequence = s[++p],
                        ReceptorSecondaryStructure= s[++p],
                         LigandChainId= s[++p][0],
                         LigandResSeq= int.Parse(s[++p]),
                        LigandIndex = int.Parse(s[++p]),
                        LigandAminoAcidSequence = s[++p],
                        LigandSecondaryStructure= s[++p],
                    });
                }

                return result;

            }
        }

        static void Main(string[] args)
        {
            //var requiredInterfaceLengths = new int[] { 7, 9, 11, 13, 15 };
            var requiredInterfaceLengths = new int[] { 9 };

            var sequenceFilename = @"C:\pdbe\pdb_seqres.fasta";
            var dsspFilename = @"C:\pdbe\ss.txt";

            var sequenceListFromFastaFile = Sequence.LoadSequenceFile(sequenceFilename, new string[] { null, "", "protein" });
            var dsspList = Sequence.LoadSequenceFile(dsspFilename, new string[] { null, "", "protein" });
            dsspList = dsspList.Where(a => a.IdSplit.Description == "secstr").ToList();

            //var pdbIdList = new List<string>(); {"1a00"};
            var pdbFiles = Directory.GetFiles(@"c:\pdbe\contacts_all\", "????.pdb", SearchOption.TopDirectoryOnly).ToList();
            var pdbFileLengths = pdbFiles.Select(a => new Tuple<string, long>(a, new FileInfo(a).Length)).ToList();
            pdbFileLengths = pdbFileLengths.Where(a => a.Item2 > 0).ToList();
            pdbFileLengths = pdbFileLengths.OrderBy(a => a.Item2).ToList();

            var ifList = InterfaceFragment.Load(@"c:\r\if.csv");
            ifList = ifList.Where(a => requiredInterfaceLengths.Contains(a.FragmentLength)).ToList();

            if (ifList.Count == 0)
            {



                var pdbIdList = pdbFileLengths.Select(a => a.Item1).Select(Path.GetFileNameWithoutExtension).Select(a => a.ToUpperInvariant()).ToList();
                //pdbIdList = pdbIdList.GetRange(0, 100);
                //var result = new List<InterfaceFragment>();

                var taskList1 = new List<Task<List<InterfaceFragment>>>();


                foreach (var pdbId1 in pdbIdList)
                {
                    var pdbId = pdbId1;

                    Console.WriteLine(pdbId + " " + (pdbIdList.IndexOf(pdbId) + 1) + "/" + pdbIdList.Count);
                    while (taskList1.Count(a => !a.IsCompleted) >= Environment.ProcessorCount) { Task.WaitAny(taskList1.Where(a => !a.IsCompleted).ToArray<Task>()); }

                    taskList1.Add(Task.Run(() =>
                                          {

                                              var pdbResult = new List<InterfaceFragment>();

                                              var atomsFilename = @"c:\pdbe\atoms_all\" + pdbId + ".pdb";
                                              var contactsFilename = @"c:\pdbe\contacts_all\" + pdbId + ".pdb";
                                              var interfaceCsv = @"c:\r\interface_9\" + pdbId + @".csv";

                                              if (File.Exists(interfaceCsv)) return InterfaceFragment.Load(interfaceCsv);
                                              if (!File.Exists(atomsFilename) || !File.Exists(contactsFilename)) return null;// new List<InterfaceFragment>(); // continue;
                                              if (new FileInfo(atomsFilename).Length == 0 || new FileInfo(contactsFilename).Length == 0) return null;//new List<InterfaceFragment>(); // continue;

                                              var contacts = ProteinBioClass.AtomPair.LoadAtomPairList(contactsFilename);
                                              var contactChainIds = contacts.SelectMany(a => new char[] { a.Atom1.chainID.FieldValue[0], a.Atom2.chainID.FieldValue[0] }).Distinct().ToList();

                                              var proteinFileChains = ProteinBioClass.PdbAtomicChains(atomsFilename, contactChainIds.ToArray(), -1, -1, false);


                                              var sequenceListFromPdbFile = proteinFileChains.ChainList.Select(pdbChain => Sequence.LoadStructureFile(atomsFilename, new[] { pdbChain.ChainId }, true, null, null, '-', '-')).ToList();

                                              var dsspFromFastaFile = proteinFileChains.ChainList.Select(pdbChain => dsspList.FirstOrDefault(a => a.IdSplit.PdbId.ToUpperInvariant() == pdbId.Substring(0, a.IdSplit.PdbId.Length).ToUpperInvariant() && a.IdSplit.ChainId == pdbChain.ChainId)?.FullSequence).ToList();
                                              var sequenceFromFastaFile = proteinFileChains.ChainList.Select(pdbChain => sequenceListFromFastaFile.FirstOrDefault(a => a.IdSplit.PdbId.ToUpperInvariant() == pdbId.Substring(0, a.IdSplit.PdbId.Length).ToUpperInvariant() && a.IdSplit.ChainId == pdbChain.ChainId)?.FullSequence).ToList();
                                              var sequenceFromPdbFile = proteinFileChains.ChainList.Select((pdbChain, i) => sequenceListFromPdbFile[i].FirstOrDefault(a => (string.IsNullOrWhiteSpace(a.IdSplit.PdbId) || a.IdSplit.PdbId.ToUpperInvariant() == pdbId.Substring(0, a.IdSplit.PdbId.Length).ToUpperInvariant()) && a.IdSplit.ChainId == pdbChain.ChainId)?.FullSequence).ToList();
                                              var structureToSequenceAlignmentResult = proteinFileChains.ChainList.Select((pdbChain, i) => ProteinBioClass.StructureToSequenceAlignment.Align(pdbChain.AtomList, sequenceFromFastaFile[i], sequenceFromPdbFile[i])).ToList();

                                              if (structureToSequenceAlignmentResult.Any(a => a == null)) return null;

                                              for (var chainIndex3 = 0; chainIndex3 < proteinFileChains.ChainList.Count; chainIndex3++)
                                              {
                                                  for (var i = 0; i < structureToSequenceAlignmentResult[chainIndex3].FastaSequenceAligned.Length; i++)
                                                  {
                                                      if (structureToSequenceAlignmentResult[chainIndex3].FastaSequenceAligned[i] == '-') { dsspFromFastaFile[chainIndex3] = dsspFromFastaFile[chainIndex3].Insert(i, "-"); }
                                                  }
                                              }

                                              foreach (var contactChainId1 in contactChainIds)
                                              {
                                                  var chainIndex1 = proteinFileChains.ChainList.FindIndex(g => g.ChainId == contactChainId1);
                                                  if (structureToSequenceAlignmentResult[chainIndex1] == null) continue;

                                                  if (string.IsNullOrWhiteSpace(sequenceFromPdbFile[chainIndex1]) || string.IsNullOrWhiteSpace(sequenceFromFastaFile[chainIndex1]) || string.IsNullOrWhiteSpace(dsspFromFastaFile[chainIndex1])) continue;

                                                  var chainResult = new List<InterfaceFragment>();
                                                  //var pdbChain = proteinFileChains.ChainList.First(a => a.ChainId == contactChainId).AtomList;

                                                  var chainContacts = contacts.Where(a => a.Atom1.chainID.FieldValue[0] == contactChainId1 || a.Atom2.chainID.FieldValue[0] == contactChainId1).Select(a => a.Atom1.chainID.FieldValue[0] == contactChainId1 ? a : a.SwapAtoms()).ToList();
                                                  //chainContacts = chainContacts.GroupBy(a => int.Parse(a.resSeq.FieldValue)).ToList().Select(a => a.First()).ToList();
                                                  //chainContacts = chainContacts.Distinct().ToList();
                                                  chainContacts = chainContacts.OrderBy(a => int.Parse(a.Atom1.resSeq.FieldValue)).ThenBy(a => int.Parse(a.Atom1.serial.FieldValue)).ToList();






                                                  foreach (var atomPair in chainContacts)
                                                  {
                                                      foreach (var requiredInterfaceLength in requiredInterfaceLengths)
                                                      {
                                                          var contactChainId2 = atomPair.Atom2.chainID.FieldValue[0];
                                                          var chainIndex2 = proteinFileChains.ChainList.FindIndex(g => g.ChainId == contactChainId2);
                                                          if (structureToSequenceAlignmentResult[chainIndex2] == null) continue;

                                                          if (string.IsNullOrWhiteSpace(sequenceFromPdbFile[chainIndex2]) || string.IsNullOrWhiteSpace(sequenceFromFastaFile[chainIndex2]) || string.IsNullOrWhiteSpace(dsspFromFastaFile[chainIndex2])) continue;

                                                          var interfaceLength1 = requiredInterfaceLength;
                                                          var interfaceLength2 = requiredInterfaceLength;

                                                          var resSeq1 = int.Parse(atomPair.Atom1.resSeq.FieldValue);
                                                          var resSeq2 = int.Parse(atomPair.Atom2.resSeq.FieldValue);

                                                          var resSeqIndex1a = structureToSequenceAlignmentResult[chainIndex1].AlignmentMap.ToList().FindIndex(a => a == resSeq1);
                                                          var resSeqIndex2a = structureToSequenceAlignmentResult[chainIndex2].AlignmentMap.ToList().FindIndex(a => a == resSeq2);

                                                          if (structureToSequenceAlignmentResult[chainIndex1].FastaSequenceAligned.Length < interfaceLength1) interfaceLength1 = structureToSequenceAlignmentResult[chainIndex1].FastaSequenceAligned.Length;
                                                          if (structureToSequenceAlignmentResult[chainIndex2].FastaSequenceAligned.Length < interfaceLength2) interfaceLength2 = structureToSequenceAlignmentResult[chainIndex2].FastaSequenceAligned.Length;

                                                          var resSeqIndex1 = resSeqIndex1a - (interfaceLength1 / 2);
                                                          var resSeqIndex2 = resSeqIndex2a - (interfaceLength2 / 2);

                                                          if (resSeqIndex1 < 0) resSeqIndex1 = 0;
                                                          if (resSeqIndex2 < 0) resSeqIndex2 = 0;

                                                          if (resSeqIndex1 + interfaceLength1 > structureToSequenceAlignmentResult[chainIndex1].FastaSequenceAligned.Length) resSeqIndex1 = (structureToSequenceAlignmentResult[chainIndex1].FastaSequenceAligned.Length - interfaceLength1);
                                                          if (resSeqIndex2 + interfaceLength2 > structureToSequenceAlignmentResult[chainIndex2].FastaSequenceAligned.Length) resSeqIndex2 = (structureToSequenceAlignmentResult[chainIndex2].FastaSequenceAligned.Length - interfaceLength2);


                                                          var interfaceSuper1 = structureToSequenceAlignmentResult[chainIndex1].FastaSequenceAligned.Substring(resSeqIndex1, interfaceLength1);
                                                          var interfaceSuper2 = structureToSequenceAlignmentResult[chainIndex2].FastaSequenceAligned.Substring(resSeqIndex2, interfaceLength2);

                                                          var dsspSuper1 = dsspFromFastaFile[chainIndex1].Substring(resSeqIndex1, interfaceLength1);
                                                          var dsspSuper2 = dsspFromFastaFile[chainIndex2].Substring(resSeqIndex2, interfaceLength2);

                                                          if (interfaceSuper1.Length != requiredInterfaceLength || dsspSuper1.Length != requiredInterfaceLength) continue;
                                                          if (interfaceSuper2.Length != requiredInterfaceLength || dsspSuper2.Length != requiredInterfaceLength) continue;

                                                          var interfaceFragment = new InterfaceFragment()
                                                          {
                                                              FragmentLength = requiredInterfaceLength,
                                                              PdbId = pdbId,
                                                              ReceptorChainId = contactChainId1,
                                                              ReceptorResSeq = resSeq1,
                                                              ReceptorIndex = resSeqIndex1a,
                                                              ReceptorAminoAcidSequence = interfaceSuper1,
                                                              ReceptorSecondaryStructure = dsspSuper1,
                                                              LigandChainId = contactChainId2,
                                                              LigandResSeq = resSeq2,
                                                              LigandIndex = resSeqIndex2a,
                                                              LigandAminoAcidSequence = interfaceSuper2,
                                                              LigandSecondaryStructure = dsspSuper2
                                                          };


                                                          chainResult.Add(interfaceFragment);
                                                      }
                                                  }
                                                  pdbResult.AddRange(chainResult);
                                              }

                                              InterfaceFragment.Save(interfaceCsv, pdbResult.Distinct().ToList());
                                              return null;// return pdbResult;
                                          }));
                    //result.AddRange(pdbResult);

                }
                //try
                //{
                    Task.WaitAll(taskList1.ToArray<Task>());
                //}
                //catch (AggregateException ae)
                //{
                //    throw ae.Flatten();
                //}

                ifList = taskList1.Where(a=>a.Result!=null).SelectMany(a => a.Result).ToList();

                ifList = ifList.Distinct().ToList();
                InterfaceFragment.Save(@"c:\r\if.csv", ifList);
            }

            var pairs1 = ifList.Select(a => new Tuple<string, string>(a.ReceptorAminoAcidSequence, a.ReceptorSecondaryStructure)).Distinct().ToList();

            sequenceFilename = null;
            dsspFilename = null;
            sequenceListFromFastaFile.Clear(); sequenceListFromFastaFile = null;
            dsspList.Clear(); dsspList = null;
            pdbFiles.Clear(); pdbFiles = null;
            pdbFileLengths.Clear(); pdbFileLengths = null;
            //ifList.Clear(); ifList = null;

            var similarAA = new string[] { "LAGVIP", "DE", "ST", "RKH", "FYW", "NQ", "CM", "BJOUXZ", "-" };
            var simAaList = similarAA.SelectMany(a => a.ToList()).ToList();
            var simAaDict = new Dictionary<char, string>();




            foreach (var s in simAaList) { simAaDict.Add(s, similarAA.First(a => a.Contains(s))); }


            DateTime startTime = DateTime.Now;

            var simList = new List<Tuple<string, string, string, string, decimal, decimal, decimal>>(); // aa, ss, aa-sim, aa-evo-sim, ss-sim
            // compare without alignments, as aligments takes too long, also allow for insertion/deletions with index pos -1/+1.
            for (int i = 0; i < pairs1.Count; i++)
            {
                TimeSpan timeRemaining = TimeSpan.FromTicks(DateTime.Now.Subtract(startTime).Ticks * (pairs1.Count - (i + 1)) / (i + 1));
                Console.WriteLine((i + 1) + " / " + pairs1.Count + " " + timeRemaining.ToString("d'd 'h'h 'm'm 's's'"));
                var a = pairs1[i];
                for (int j = 0; j < pairs1.Count; j++)
                {

                    if (j <= i) continue;

                    var b = pairs1[j];

                    if (a.Item1.Length != b.Item1.Length) continue;

                    var scoreAa = 0;
                    var scoreSs = 0;

                    var scoreAaEvo = 0;

                    for (var x = 0; x < a.Item1.Length; x++)
                    {
                        if (a.Item1[x] == b.Item1[x]) scoreAa++;
                        else if (x > 0 && (a.Item1[x - 1] == b.Item1[x] || a.Item1[x] == b.Item1[x - 1])) scoreAa++;
                        else if (x < a.Item1.Length - 1 && (a.Item1[x + 1] == b.Item1[x] || a.Item1[x] == b.Item1[x + 1])) scoreAa++;

                        if (simAaDict[a.Item1[x]].Contains(b.Item1[x])) scoreAaEvo++;
                        else if (x > 0 && (simAaDict[a.Item1[x - 1]].Contains(b.Item1[x]) || simAaDict[a.Item1[x]].Contains(b.Item1[x - 1]))) scoreAaEvo++;
                        else if (x < a.Item1.Length - 1 && (simAaDict[a.Item1[x + 1]].Contains(b.Item1[x]) || simAaDict[a.Item1[x]].Contains(b.Item1[x + 1]))) scoreAaEvo++;


                        if (a.Item2[x] == b.Item2[x]) scoreSs++;
                        else if (x > 0 && (a.Item2[x - 1] == b.Item2[x] || a.Item2[x] == b.Item2[x - 1])) scoreSs++;
                        else if (x < a.Item2.Length - 1 && (a.Item2[x + 1] == b.Item2[x] || a.Item2[x] == b.Item2[x + 1])) scoreSs++;
                    }

                    decimal scoreAaPct = (decimal)scoreAa / (decimal)a.Item1.Length;
                    decimal scoreAaEvoPct = (decimal)scoreAaEvo / (decimal)a.Item1.Length;
                    decimal scoreSsPct = (decimal)scoreSs / (decimal)a.Item2.Length;

                    simList.Add(new Tuple<string, string, string, string, decimal, decimal, decimal>(a.Item1, a.Item2, b.Item1, b.Item2, scoreAaPct, scoreAaEvoPct, scoreSsPct));
                    //simList.Add(new Tuple<string, string, string, string, decimal, decimal, decimal>(b.AminoAcidSequence, b.SecondaryStructure, a.AminoAcidSequence, a.SecondaryStructure, scoreAaPct, scoreAaEvoPct, scoreSsPct));


                }
            }

            //var if2 = simList.SelectMany(a => new List<string>() { string.Join(",", new string[] { a.Item1, a.Item2, a.Item3, a.Item4, "" + a.Item5, "" + a.Item6, "" + a.Item7 }) /*,
            //    string.Join(",", new string[] { a.Item3, a.Item4, a.Item1, a.Item2, "" + a.Item5, "" + a.Item6, "" + a.Item7 })*/ }).ToList();

            var taskList3 = new List<Task>();
            for (int i = 0; i < pairs1.Count; i++)
            {
                TimeSpan timeRemaining = TimeSpan.FromTicks(DateTime.Now.Subtract(startTime).Ticks * (pairs1.Count - (i + 1)) / (i + 1));
                Console.WriteLine((i + 1) + " / " + pairs1.Count + " " + timeRemaining.ToString("d'd 'h'h 'm'm 's's'"));

                while (taskList3.Count(a => !a.IsCompleted) >= Environment.ProcessorCount) { Task.WaitAny(taskList3.Where(a => !a.IsCompleted).ToArray<Task>()); }
                var i1 = i;
                var t = Task.Run(() =>
                                 {
                                     var a = pairs1[i1];

                                     // find all instances of this pair with good simple alignment
                                     var cluster = simList.Where(c => (c.Item1 == a.Item1 || c.Item3 == a.Item1) && (c.Item2 == a.Item2 || c.Item4 == a.Item2) && (c.Item5 >= 0.4m && c.Item6 >= 0.8m && c.Item7 >= 0.9m) && (c.Item5 < 1.0m /*&& c.Item6 < 1.0m && c.Item7 < 1.0m*/)).ToList();

                                     if (cluster.Count < 5) return;
                                     var clusterPdbs = ifList.Where(c => cluster.Any(d => (d.Item1 == c.ReceptorAminoAcidSequence || d.Item3 == c.ReceptorAminoAcidSequence) && (d.Item2 == c.ReceptorSecondaryStructure || d.Item4 == c.ReceptorSecondaryStructure))).Select(e => new Tuple<string, char>(e.PdbId, e.ReceptorChainId)).ToList();
                                     if (clusterPdbs.Count < 5) return;
                                     List<string> o = new List<string>();
                                     o.Add("delete *");
                                     o.AddRange(clusterPdbs.Select(e => @"load c:\pdbe\" + e.Item1 + ".pdb").ToList());
                                     o.Add("hide all");
                                     o.Add("show cartoon");
                                     o.AddRange(clusterPdbs.Select((e, w) => w == 0 ? "" : @"super /" + e.Item1 + @"//" + e.Item2 + @", /" + clusterPdbs[0].Item1 + @"//" + clusterPdbs[0].Item2).ToList());


                                     File.WriteAllLines(@"c:\r\cluster_" + (i1 + 1) + ".txt", o);
                                 });
                taskList3.Add(t);
            }
            Task.WaitAll(taskList3.ToArray<Task>());

            //File.WriteAllLines(@"c:\r\if2.csv", if2);
            Console.WriteLine("Calculating aa/ss sequence identities - part 1");

            var taskList2 = new List<Task<InterfaceFragmentData>>();
            //var interfaceFragmentLengths = new int[] { 15, 13, 11, 9, 7, 5, 3 };
            //var interfaceFragmentLengths = new int[] { requiredInterfaceLengths[0] };
            var interfaceFragmentLengths = new int[] { 11 };
            for (int index = 0; index < interfaceFragmentLengths.Length; index++)
            {
                var index1 = index;
                var interfaceFragmentLength = interfaceFragmentLengths[index1];

                taskList2.Add(Task.Run(() =>
                                       {
                                           var pairs = ifList.Where(a => a.FragmentLength == interfaceFragmentLength).Select(a => new Tuple<string, string>(a.ReceptorAminoAcidSequence.Substring(index1, interfaceFragmentLength), a.ReceptorSecondaryStructure.Substring(index1, interfaceFragmentLength))).Distinct().ToList();

                                           var aassList = new List<Tuple<string, string, string, string, decimal, decimal, decimal>>();

                                           var aaSequenceList = pairs.Select(a => a.Item1).Distinct().ToList();
                                           var aaAlignmentList = Align(aaSequenceList);

                                           var ssSequenceList = pairs.Select(a => a.Item2).Distinct().ToList();
                                           var ssAlignmentList = Align(ssSequenceList);


                                           for (int i = 0; i < pairs.Count; i++)
                                           {
                                               Console.WriteLine((i + 1) + " / " + pairs.Count);

                                               var pair1 = pairs[i];
                                               for (int j = 0; j < pairs.Count; j++)
                                               {
                                                   if (j < i) continue;

                                                   var pair2 = pairs[j];

                                                   var aaSid = aaAlignmentList.First(alignment => alignment.Item1 == pair1.Item1 && alignment.Item2 == pair2.Item1).Item3;
                                                   var ssSid = ssAlignmentList.First(alignment => alignment.Item1 == pair1.Item2 && alignment.Item2 == pair2.Item2).Item3;
                                                   var weighted = (aaSid * 0.5m) + (ssSid * 0.5m);
                                                   aassList.Add(new Tuple<string, string, string, string, decimal, decimal, decimal>(pair1.Item1, pair1.Item2, pair2.Item1, pair2.Item2, aaSid, ssSid, weighted));
                                                   aassList.Add(new Tuple<string, string, string, string, decimal, decimal, decimal>(pair2.Item1, pair2.Item2, pair1.Item1, pair1.Item2, aaSid, ssSid, weighted));
                                               }

                                           }


                                           var r = new InterfaceFragmentData();
                                           r.AaSsData = aassList;
                                           r.AaData = aaAlignmentList;
                                           r.SsData = ssAlignmentList;
                                           return r;
                                       }));
            }




            Task.WaitAll(taskList2.ToArray<Task>());

            Console.WriteLine("Calculating aa/ss sequence identities - part 2");

            var sids = new decimal[] { 1.0m, 0.9m, 0.8m, 0.7m, 0.6m, 0.5m, 0.4m, 0.3m, 0.2m, 0.1m, 0.0m };

            //aa count non-transivity clusters
            var aaData = taskList2.SelectMany(a => a.Result.AaData).ToList();
            var aaDistinct = aaData.Select(a => a.Item1).Distinct().ToList();
            var aaNeighbours = new List<Tuple<string, decimal, decimal>>();
            foreach (var aa in aaDistinct)
            {
                var subset = aaData.Where(a => a.Item1 == aa).ToList();
                foreach (var sid in sids) { aaNeighbours.Add(new Tuple<string, decimal, decimal>(aa, sid, subset.Count(b => b.Item3 >= sid))); }
            }
            File.WriteAllLines(@"c:\r\aa.csv", aaData.Select(a => string.Join(",", new string[] { "" + a.Item1.Length, a.Item1, a.Item2, "" + a.Item3 })));
            File.WriteAllLines(@"c:\r\aa-clusters-1.csv", aaDistinct.Select(a => a + "," + string.Join(",", aaNeighbours.Where(d => d.Item1 == a).Select(d => "" + d.Item3).ToArray())));

            //ss count non-transivity clusters
            var ssData = taskList2.SelectMany(a => a.Result.SsData).ToList();
            var ssDistinct = ssData.Select(a => a.Item1).Distinct().ToList();
            var ssNeighbours = new List<Tuple<string, decimal, decimal>>();
            foreach (var ss in ssDistinct)
            {
                var subset = ssData.Where(a => a.Item1 == ss).ToList();
                foreach (var sid in sids) { ssNeighbours.Add(new Tuple<string, decimal, decimal>(ss, sid, subset.Count(b => b.Item3 >= sid))); }
            }
            File.WriteAllLines(@"c:\r\ss.csv", ssData.Select(a => string.Join(",", new string[] { "" + a.Item1.Length, a.Item1, a.Item2, "" + a.Item3 })));
            File.WriteAllLines(@"c:\r\ss-clusters-1.csv", ssDistinct.Select(a => a + "," + string.Join(",", ssNeighbours.Where(d => d.Item1 == a).Select(d => "" + d.Item3).ToList())));

            //aa-ss count non-transivity clusters
            var aaSsData = taskList2.SelectMany(a => a.Result.AaSsData).ToList();
            var aaSsDistinct = aaSsData.Select(a => new Tuple<string, string>(a.Item1, a.Item2)).Distinct().ToList();
            var aaSsNeighbours = new List<Tuple<string, string, decimal, decimal>>();
            foreach (var aaSs in aaSsDistinct)
            {
                var subset = aaSsData.Where(a => a.Item1 == aaSs.Item1 && a.Item2 == aaSs.Item2).ToList();
                foreach (var sid in sids) { aaSsNeighbours.Add(new Tuple<string, string, decimal, decimal>(aaSs.Item1, aaSs.Item2, sid, subset.Count(b => b.Item5 >= sid))); }
            }
            File.WriteAllLines(@"c:\r\aa-ss.csv", aaSsData.Select(a => string.Join(",", new string[] { "" + a.Item1.Length, a.Item1, a.Item2, a.Item3, a.Item4, "" + a.Item5, "" + a.Item6, "" + a.Item7 })));
            File.WriteAllLines(@"c:\r\aa-ss-clusters-1.csv", aaSsDistinct.Select(a => a.Item1 + "," + a.Item2 + "," + string.Join(",", aaSsNeighbours.Where(d => d.Item1 == a.Item1 && d.Item2 == a.Item2).Select(d => "" + d.Item4).ToList())));


            // cluster by transivity
            //var aaSsPairList = aaSsData.Select(a => new Tuple<string, string>(a.Item1, a.Item2)).Distinct().ToList();

            var aaSsPairClusters = aaSsData.Select(a => new Tuple<string, string>(a.Item1, a.Item2)).Distinct().Select(a => new List<Tuple<string, string>>() { a }).ToList();
            var aaClusters = aaSsData.Select(a => a.Item1).Distinct().Select(a => new List<string>() { a }).ToList();
            var ssClusters = aaSsData.Select(a => a.Item2).Distinct().Select(a => new List<string>() { a }).ToList();

            decimal minTransivitySid = 0.3m;

            foreach (var x in aaSsDistinct)
            {
                foreach (var y in aaSsDistinct)
                {
                    if (x == y) continue;
                    if (x.Item1.Length != y.Item1.Length) continue;//items have not been sequence aligned if not the same length


                    var z = aaSsData.First(a => a.Item1 == x.Item1 && a.Item2 == x.Item2 && a.Item3 == y.Item1 && a.Item4 == y.Item2);

                    if (z.Item5 >= minTransivitySid)
                    {
                        var c1 = aaClusters.First(a => a.Any(b => b == x.Item1));
                        var c2 = aaClusters.First(a => a.Any(b => b == y.Item1));

                        if (c1 != c2)
                        {
                            c1.AddRange(c2);
                            aaClusters.Remove(c2);
                        }
                    }

                    if (z.Item6 >= minTransivitySid)
                    {
                        var c1 = ssClusters.First(a => a.Any(b => b == x.Item2));
                        var c2 = ssClusters.First(a => a.Any(b => b == y.Item2));

                        if (c1 != c2)
                        {
                            c1.AddRange(c2);
                            ssClusters.Remove(c2);
                        }
                    }

                    if (z.Item7 >= minTransivitySid)
                    {
                        var c1 = aaSsPairClusters.First(a => a.Any(b => b.Item1 == x.Item1 && b.Item2 == x.Item2));
                        var c2 = aaSsPairClusters.First(a => a.Any(b => b.Item1 == y.Item1 && b.Item2 == y.Item2));

                        if (c1 != c2)
                        {
                            c1.AddRange(c2);
                            aaSsPairClusters.Remove(c2);
                        }
                    }
                }
            }

            var aaSsPairClusters2 = aaSsPairClusters.SelectMany((a, i) => a.Select(b => new Tuple<int, string, string>(i + 1, b.Item1, b.Item2))).ToList();
            aaSsPairClusters2 = aaSsPairClusters2.OrderByDescending(a => aaSsPairClusters2.Count(b => b.Item1 == a.Item1)).ToList();
            File.WriteAllLines(@"c:\r\aa-ss-clusters-2.csv", aaSsPairClusters2.Select(a => string.Join(",", new string[] { "" + a.Item1, a.Item2, a.Item3 })));

            var aaClusters2 = aaClusters.SelectMany((a, i) => a.Select(b => new Tuple<int, string>(i + 1, b))).ToList();
            aaClusters2 = aaClusters2.OrderByDescending(a => aaClusters2.Count(b => b.Item1 == a.Item1)).ToList();
            File.WriteAllLines(@"c:\r\aa-clusters-2.csv", aaClusters2.Select(a => string.Join(",", new string[] { "" + a.Item1, a.Item2 })));

            var ssClusters2 = ssClusters.SelectMany((a, i) => a.Select(b => new Tuple<int, string>(i + 1, b))).ToList();
            ssClusters2 = ssClusters2.OrderByDescending(a => ssClusters2.Count(b => b.Item1 == a.Item1)).ToList();
            File.WriteAllLines(@"c:\r\ss-clusters-2.csv", ssClusters2.Select(a => string.Join(",", new string[] { "" + a.Item1, a.Item2 })));

            //File.WriteAllLines(@"c:\r\clusters-ss.csv", ssClusters2.Select(a => string.Join(",", new string[] { "" + a.Item1, a.Item2 })));
            // clusters by aa sid, ss sid, aa-ss sid


        }

        public class InterfaceFragmentData
        {
            public List<Tuple<string, string, decimal>> AaData;
            public List<Tuple<string, string, decimal>> SsData;
            public List<Tuple<string, string, string, string, decimal, decimal, decimal>> AaSsData;

        }

        public static List<Tuple<string, string, decimal>> Align(List<string> sequences)
        {
            var aaAlignments = new List<Tuple<string, string, decimal>>();

            for (int i = 0; i < sequences.Count; i++)
            {
                Console.WriteLine((i + 1) + " / " + sequences.Count);
                var aa1 = sequences[i];

                for (int j = 0; j < sequences.Count; j++)
                {
                    if (j < i) continue;

                    var aa2 = sequences[j];
                    if (aaAlignments.Any(a => a.Item1 == aa1 && a.Item2 == aa2)) continue;

                    var nmw = new NeedlemanWunsch(aa1, aa2);
                    var aligned = nmw.getAlignment();

                    decimal match = 0;

                    for (var k = 0; k < aligned[0].Length; k++)
                    {
                        if (aligned[0][k] == aligned[1][k]) match++;
                    }
                    match = (decimal)match / (decimal)aligned[0].Length;

                    aaAlignments.Add(new Tuple<string, string, decimal>(aa1, aa2, match));
                    aaAlignments.Add(new Tuple<string, string, decimal>(aa2, aa1, match));
                }
            }

            //File.WriteAllLines(saveFilename, aaAlignments.Select(a => string.Join(",", new string[] { a.Item1, a.Item2, a.Item3, a.Item4, "" + a.Item5 })));

            return aaAlignments;
        }


        public static List<Tuple<string, string, decimal>> Cluster(List<Tuple<string, string, string, string, decimal>> alignments)
        {
            var clusters = new List<Tuple<string, string, decimal>>();

            foreach (var alignment in alignments)
            {
                for (decimal sid = 0.1m; sid <= 1.0m; sid += 0.1m)
                {
                    var matches = alignments.Where(a => a.Item1 == alignment.Item1 && a.Item5 >= sid).ToList();
                    foreach (var match in matches) { clusters.Add(new Tuple<string, string, decimal>(alignment.Item1, match.Item1, sid)); }
                    var sequenceSidClusterSize = matches.Count;
                }

            }

            //File.WriteAllLines(saveFilename, clusters.Select(a => string.Join(",", new string[] { a.Item1, a.Item2, "" + a.Item3 })));

            return clusters;
        }
    }
}
