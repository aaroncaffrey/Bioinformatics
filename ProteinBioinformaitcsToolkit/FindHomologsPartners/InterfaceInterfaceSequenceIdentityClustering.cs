using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;

namespace InterfaceInterfaceSequenceIdentityClustering
{
    public class InterfaceInterfaceSequenceIdentityClustering
    {


        public static void Main(string[] args)
        {
            // this purpose of this program is to add cluster indexes for the protein and the interfaces
            // to do: make program not require separate clustering program

            /* 1. load interfaces
             * 2. cluster interfaces
             * 3. cluster chain sequences (from both structure and sequence - add them both to the same cluster to start with)
             * 4. output new extended interfaces file, containing all interfaces
             * 5. to do: mark multibinding sites (by measuring whether there is an overlap with any other site in the same chain)
             */

            var outputCsvFile = @"C:\CategoryM\catm_combined_data.csv";//@"c:\viva\1avwa_homology_interfaces_out.csv";

            //var dsspFastaFile = @"C:\CategoryM\ss.txt";
            //var dsspSequenceList = Sequence.LoadSequenceFile(dsspFastaFile);


            // load output from ComplexInterfaces program
            var interfaceInterfaceDataCsvFolder = @"C:\CategoryM\interfaces\";//@"c:\thepdb\interfaces\";
            var interfaceInterfaceDataCsvFiles = Directory.GetFiles(interfaceInterfaceDataCsvFolder, "interface-interface_*????.pdb.csv");

            var interfaceInterfaceDataFileList = interfaceInterfaceDataCsvFiles.Select(ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData.Load).ToList();

            interfaceInterfaceDataFileList = interfaceInterfaceDataFileList.Select(a => a.Where(b => char.IsLetter(b.ReceptorChainId) && char.IsLetter(b.LigandChainId)).ToList()).ToList();
            //var pdbIds = interfaceInterfaceDataFileList.SelectMany(a => a.Where(c=>char.IsLetter(c.ReceptorChainId) && char.IsLetter(c.LigandChainId)).SelectMany(b => new List<string> {b.ReceptorPdbId.Substring(0,4).ToUpperInvariant() + ":" + b.ReceptorChainId, b.LigandPdbId.Substring(0,4).ToUpperInvariant() + ":" + b.LigandChainId}).ToList()).Distinct().ToList();
            //pdbIds = pdbIds.Select(a=>">" + a + ":secstr").ToList();

            //dsspSequenceList = dsspSequenceList.Where(a => pdbIds.Contains(a.Id)).ToList();

            var dsspSequences = interfaceInterfaceDataFileList.SelectMany(a => a.SelectMany(b => new List<string>() { b.ReceptorSequenceDssp.Replace("_", "C"), b.LigandSequenceDssp.Replace("_", "C") }).ToList()).ToList();
            dsspSequences = dsspSequences.Select(Sequence.TrimSequence).Select(a => Sequence.CleanAminoAcidSequence(a)).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();
            var dsspSequencesClustered = SequenceIdentityClustering.SequenceIdentityClustering.ClusterSequenceByAlignedSequenceIdentity(dsspSequences.Select(a => new Sequence(a)).ToList(), ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength, 0.6m, 0.0m).OrderBy(a => a.ClusterIndex).ToList();
            var dsspClusterIndexes = dsspSequencesClustered.Select(a => a.ClusterIndex).Distinct().ToList();


            var dsspInterfaceSequenceList = interfaceInterfaceDataFileList.SelectMany(a => a.SelectMany(b => new List<string> { b.ReceptorInterfaceDsspSecondaryStructure.Replace("_", "C"), b.LigandInterfaceDsspSecondaryStructure.Replace("_", "C") }).ToList()).ToList();
            dsspInterfaceSequenceList = dsspInterfaceSequenceList.Select(Sequence.TrimSequence).Select(a => Sequence.CleanAminoAcidSequence(a)).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();
            var dsspInterfaceSequencesClustered = SequenceIdentityClustering.SequenceIdentityClustering.ClusterSequenceByAlignedSequenceIdentity(dsspInterfaceSequenceList.Select(a => new Sequence(a)).ToList(), ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength, 0.75m, 0.0m).OrderBy(a => a.ClusterIndex).ToList();
            var dsspInterfaceClusterIndexes = dsspInterfaceSequencesClustered.Select(a => a.ClusterIndex).Distinct().ToList();


            var strideSequences = interfaceInterfaceDataFileList.SelectMany(a => a.SelectMany(b => new List<string>() { b.ReceptorSequenceStride.Replace("_", "C"), b.LigandSequenceStride.Replace("_", "C") }).ToList()).ToList();
            strideSequences = strideSequences.Select(Sequence.TrimSequence).Select(a => Sequence.CleanAminoAcidSequence(a)).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();
            var strideSequencesClustered = SequenceIdentityClustering.SequenceIdentityClustering.ClusterSequenceByAlignedSequenceIdentity(strideSequences.Select(a => new Sequence(a)).ToList(), ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength, 0.6m, 0.0m).OrderBy(a => a.ClusterIndex).ToList();
            var strideClusterIndexes = strideSequencesClustered.Select(a => a.ClusterIndex).Distinct().ToList();


            var strideInterfaceSequenceList = interfaceInterfaceDataFileList.SelectMany(a => a.SelectMany(b => new List<string> { b.ReceptorInterfaceStrideSecondaryStructure.Replace("_", "C"), b.LigandInterfaceStrideSecondaryStructure.Replace("_", "C") }).ToList()).ToList();
            strideInterfaceSequenceList = strideInterfaceSequenceList.Select(Sequence.TrimSequence).Select(a => Sequence.CleanAminoAcidSequence(a)).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();
            var strideInterfaceSequencesClustered = SequenceIdentityClustering.SequenceIdentityClustering.ClusterSequenceByAlignedSequenceIdentity(strideInterfaceSequenceList.Select(a => new Sequence(a)).ToList(), ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength, 0.75m, 0.0m).OrderBy(a => a.ClusterIndex).ToList();
            var strideInterfaceClusterIndexes = strideInterfaceSequencesClustered.Select(a => a.ClusterIndex).Distinct().ToList();


            var sequences = new List<string>();
            sequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.ReceptorSequenceFromSequenceFile).ToList()).ToList());
            sequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.ReceptorSequenceFromStructureFile).ToList()).ToList());
            sequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.ReceptorSequenceSuper).ToList()).ToList());
            sequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.LigandSequenceFromSequenceFile).ToList()).ToList());
            sequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.LigandSequenceFromStructureFile).ToList()).ToList());
            sequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.LigandSequenceSuper).ToList()).ToList());
            sequences = sequences.Select(Sequence.TrimSequence).Select(a => Sequence.CleanAminoAcidSequence(a)).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();
            var sequencesClustered = SequenceIdentityClustering.SequenceIdentityClustering.ClusterSequenceByAlignedSequenceIdentity(sequences.Select(a => new Sequence(a)).ToList(), ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength, 0.3m, 0.4m).OrderBy(a => a.ClusterIndex).ToList();

            var interfaceSequences = new List<string>();
            interfaceSequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.ReceptorInterfaceAllAminoAcidsSuper).ToList()).ToList());
            interfaceSequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.ReceptorInterfaceAllAminoAcidsSuper).ToList()).ToList());
            interfaceSequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.LigandInterfaceAllAminoAcidsSuper).ToList()).ToList());
            interfaceSequences.AddRange(interfaceInterfaceDataFileList.SelectMany(a => a.Select(b => b.LigandInterfaceAllAminoAcidsSuper).ToList()).ToList());
            interfaceSequences = interfaceSequences.Select(Sequence.TrimSequence).Select(a => Sequence.CleanAminoAcidSequence(a)).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();
            var interfaceSequencesClustered = SequenceIdentityClustering.SequenceIdentityClustering.ClusterSequenceByAlignedSequenceIdentity(interfaceSequences.Select(a => new Sequence(a)).ToList(), ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength, 0.4m, 0.4m).OrderBy(a => a.ClusterIndex).ToList();


            // load each interface, add cluster indexes 
            // batch processing program will then select the first matching cluster member

            foreach (var x in interfaceInterfaceDataFileList)
            {
                foreach (var z in x)
                {
                    // dssp for sequence
                    var sequenceIdentityClusterMemberReceptorDssp = dsspSequencesClustered.FirstOrDefault(a => (!string.IsNullOrWhiteSpace(z.ReceptorSequenceDssp) && Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.ReceptorSequenceDssp.Replace("_", "C")))));
                    if (sequenceIdentityClusterMemberReceptorDssp != null)
                        z.ReceptorSequenceDsspClusterIndex =
                            sequenceIdentityClusterMemberReceptorDssp.ClusterIndex;


                    var sequenceIdentityClusterMemberLigandDssp = dsspSequencesClustered.FirstOrDefault(a => (!string.IsNullOrWhiteSpace(z.LigandSequenceDssp) && Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.LigandSequenceDssp.Replace("_", "C")))));
                    if (sequenceIdentityClusterMemberLigandDssp != null)
                        z.LigandSequenceDsspClusterIndex =
                            sequenceIdentityClusterMemberLigandDssp.ClusterIndex;

                    // dssp for interface

                    var sequenceIdentityClusterMemberReceptorInterfaceDssp = dsspInterfaceSequencesClustered.FirstOrDefault(a => (!string.IsNullOrWhiteSpace(z.ReceptorInterfaceDsspSecondaryStructure) && Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.ReceptorInterfaceDsspSecondaryStructure.Replace("_", "C")))));
                    if (sequenceIdentityClusterMemberReceptorInterfaceDssp != null)
                        z.ReceptorInterfaceSequenceDsspClusterIndex =
                            sequenceIdentityClusterMemberReceptorInterfaceDssp.ClusterIndex;


                    var sequenceIdentityClusterMemberLigandInterfaceDssp = dsspInterfaceSequencesClustered.FirstOrDefault(a => (!string.IsNullOrWhiteSpace(z.LigandInterfaceDsspSecondaryStructure) && Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.LigandInterfaceDsspSecondaryStructure.Replace("_", "C")))));
                    if (sequenceIdentityClusterMemberLigandInterfaceDssp != null)
                        z.LigandInterfaceSequenceDsspClusterIndex =
                            sequenceIdentityClusterMemberLigandInterfaceDssp.ClusterIndex;


                    // stride for sequence
                    var sequenceIdentityClusterMemberReceptorStride = strideSequencesClustered.FirstOrDefault(a => (!string.IsNullOrWhiteSpace(z.ReceptorSequenceStride) && Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.ReceptorSequenceStride.Replace("_", "C")))));
                    if (sequenceIdentityClusterMemberReceptorStride != null)
                        z.ReceptorSequenceStrideClusterIndex =
                            sequenceIdentityClusterMemberReceptorStride.ClusterIndex;


                    var sequenceIdentityClusterMemberLigandStride = strideSequencesClustered.FirstOrDefault(a => (!string.IsNullOrWhiteSpace(z.LigandSequenceStride) && Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.LigandSequenceStride.Replace("_", "C")))));
                    if (sequenceIdentityClusterMemberLigandStride != null)
                        z.LigandSequenceStrideClusterIndex =
                            sequenceIdentityClusterMemberLigandStride.ClusterIndex;

                    // stride for interface

                    var sequenceIdentityClusterMemberReceptorInterfaceStride = strideInterfaceSequencesClustered.FirstOrDefault(a => (!string.IsNullOrWhiteSpace(z.ReceptorInterfaceStrideSecondaryStructure) && Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.ReceptorInterfaceStrideSecondaryStructure.Replace("_", "C")))));
                    if (sequenceIdentityClusterMemberReceptorInterfaceStride != null)
                        z.ReceptorInterfaceSequenceStrideClusterIndex =
                            sequenceIdentityClusterMemberReceptorInterfaceStride.ClusterIndex;


                    var sequenceIdentityClusterMemberLigandInterfaceStride = strideInterfaceSequencesClustered.FirstOrDefault(a => (!string.IsNullOrWhiteSpace(z.LigandInterfaceStrideSecondaryStructure) && Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.LigandInterfaceStrideSecondaryStructure.Replace("_", "C")))));
                    if (sequenceIdentityClusterMemberLigandInterfaceStride != null)
                        z.LigandInterfaceSequenceStrideClusterIndex =
                            sequenceIdentityClusterMemberLigandInterfaceStride.ClusterIndex;


                    // sequence 

                    var sequenceIdentityClusterMemberReceptor = sequencesClustered.FirstOrDefault(
                        a =>
                            (!string.IsNullOrWhiteSpace(z.ReceptorSequenceSuper) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.ReceptorSequenceSuper))) ||
                            (!string.IsNullOrWhiteSpace(z.ReceptorSequenceFromSequenceFile) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.ReceptorSequenceFromSequenceFile))) ||
                            (!string.IsNullOrWhiteSpace(z.ReceptorSequenceFromStructureFile) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.ReceptorSequenceFromStructureFile))));
                    if (sequenceIdentityClusterMemberReceptor != null)
                        z.ReceptorSequenceClusterIndex =
                            sequenceIdentityClusterMemberReceptor.ClusterIndex;

                    var sequenceIdentityClusterMemberLigand = sequencesClustered.FirstOrDefault(
                        a =>
                            (!string.IsNullOrWhiteSpace(z.LigandSequenceSuper) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.LigandSequenceSuper))) ||
                            (!string.IsNullOrWhiteSpace(z.LigandSequenceFromSequenceFile) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.LigandSequenceFromSequenceFile))) ||
                            (!string.IsNullOrWhiteSpace(z.LigandSequenceFromStructureFile) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.LigandSequenceFromStructureFile))));
                    if (sequenceIdentityClusterMemberLigand != null)
                        z.LigandSequenceClusterIndex =
                            sequenceIdentityClusterMemberLigand.ClusterIndex;


                    // interface

                    var sequenceIdentityClusterMemberReceptorInterface = interfaceSequencesClustered.FirstOrDefault(
                        a =>
                            (!string.IsNullOrWhiteSpace(z.ReceptorInterfaceAllAminoAcidsSuper) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.ReceptorInterfaceAllAminoAcidsSuper))) ||
                            (!string.IsNullOrWhiteSpace(z.ReceptorInterfaceAllAminoAcids) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.ReceptorInterfaceAllAminoAcids))));
                    if (sequenceIdentityClusterMemberReceptorInterface != null)
                        z.ReceptorInterfaceSequenceClusterIndex =
                            sequenceIdentityClusterMemberReceptorInterface.ClusterIndex;

                    var sequenceIdentityClusterMemberLigandInterface = interfaceSequencesClustered.FirstOrDefault(
                        a =>
                            (!string.IsNullOrWhiteSpace(z.LigandInterfaceAllAminoAcidsSuper) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.LigandInterfaceAllAminoAcidsSuper))) ||
                            (!string.IsNullOrWhiteSpace(z.LigandInterfaceAllAminoAcids) &&
                             Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(a.Sequence)) == Sequence.CleanAminoAcidSequence(Sequence.TrimSequence(z.LigandInterfaceAllAminoAcids))));
                    if (sequenceIdentityClusterMemberLigandInterface != null)
                        z.LigandInterfaceSequenceClusterIndex =
                            sequenceIdentityClusterMemberLigandInterface.ClusterIndex;
                }

            }

            var joined = interfaceInterfaceDataFileList.SelectMany(a => a).ToList();



            var receptorConsensus = new List<List<ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData>>();
            var ligandConsensus = new List<List<ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData>>();

            foreach (var j in joined)
            {
                receptorConsensus.Add(new List<ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData>() { j });
                ligandConsensus.Add(new List<ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData>() { j });
            }

            foreach (var x in joined)
            {
                foreach (var y in joined)
                {
                    if (x == y) continue;

                    int receptorScore = 0;
                    if (x.ReceptorSequenceClusterIndex == y.ReceptorSequenceClusterIndex) receptorScore++;
                    if (x.ReceptorInterfaceSequenceClusterIndex == y.ReceptorInterfaceSequenceClusterIndex) receptorScore++;

                    if (x.ReceptorSequenceDsspClusterIndex == y.ReceptorSequenceDsspClusterIndex) receptorScore++;
                    if (x.ReceptorInterfaceSequenceDsspClusterIndex == y.ReceptorInterfaceSequenceDsspClusterIndex) receptorScore++;

                    if (x.ReceptorSequenceStrideClusterIndex == y.ReceptorSequenceStrideClusterIndex) receptorScore++;
                    if (x.ReceptorInterfaceSequenceStrideClusterIndex == y.ReceptorInterfaceSequenceStrideClusterIndex) receptorScore++;

                    if (x.ReceptorMaxCluster == y.ReceptorMaxCluster) receptorScore++;

                    if (receptorScore >= 4)
                    {
                        var clusterA = receptorConsensus.First(a => a.Contains(x));
                        var clusterB = receptorConsensus.First(a => a.Contains(y));

                        if (clusterA == clusterB) continue;

                        clusterA.AddRange(clusterB);
                        receptorConsensus.Remove(clusterB);
                    }

                    int ligandScore = 0;
                    if (x.LigandSequenceClusterIndex == y.LigandSequenceClusterIndex) ligandScore++;
                    if (x.LigandInterfaceSequenceClusterIndex == y.LigandInterfaceSequenceClusterIndex) ligandScore++;

                    if (x.LigandSequenceDsspClusterIndex == y.LigandSequenceDsspClusterIndex) ligandScore++;
                    if (x.LigandInterfaceSequenceDsspClusterIndex == y.LigandInterfaceSequenceDsspClusterIndex) ligandScore++;

                    if (x.LigandSequenceStrideClusterIndex == y.LigandSequenceStrideClusterIndex) ligandScore++;
                    if (x.LigandInterfaceSequenceStrideClusterIndex == y.LigandInterfaceSequenceStrideClusterIndex) ligandScore++;

                    if (x.LigandMaxCluster == y.LigandMaxCluster) ligandScore++;

                    if (ligandScore >= 4)
                    {
                        var clusterA = ligandConsensus.First(a => a.Contains(x));
                        var clusterB = ligandConsensus.First(a => a.Contains(y));

                        if (clusterA == clusterB) continue;

                        clusterA.AddRange(clusterB);
                        ligandConsensus.Remove(clusterB);
                    }
                }
            }

            for (int index = 0; index < receptorConsensus.Count; index++)
            {
                var x = receptorConsensus[index];
                foreach (var y in x) { y.ReceptorConsensusClusterIndex = index + 1; }
            }

            for (int index = 0; index < ligandConsensus.Count; index++)
            {
                var x = ligandConsensus[index];
                foreach (var y in x) { y.LigandConsensusClusterIndex = index + 1; }
            }

            var output = joined.Select(a => a.ToString()).ToList();
            output.Insert(0, ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData.Header());
            File.WriteAllLines(outputCsvFile, output);
            //Console.ReadLine();
            /*
            //// load output from SequenceIdentityClustering program
            //var homologClusterCsvFile = args[0];

            //// load output from ComplexInterfaces program
            //var interfaceInterfaceDataCsvFolder = args[1];

            //// filename where to output data
            //var outputCsvFile = args[2];



            // load output from SequenceIdentityClustering program
            var inputOutputProteinSequenceHomologClusterCsvFile = @"C:\CategoryM\catm_homology.csv";// @"c:\viva\1avwa\1avwa_homology.csv";
            //@"c:\viva\1b3ua\1b3ua_homology.csv";

            // filename where to save interface clusters
            var inputOutputInterfaceSequenceHomologClustersFile = @"C:\CategoryM\catm_homology_interfaces.csv"; //@"c:\viva\1avwa_homology_interfaces.csv";

            // filename where to output data
            var outputCsvFile = @"C:\CategoryM\catm_combined_data.csv";//@"c:\viva\1avwa_homology_interfaces_out.csv";

           */

        }
    }
}
