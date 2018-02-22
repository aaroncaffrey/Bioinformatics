using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProteinBioinformaticsSharedLibrary;

namespace SequenceIdentityClustering
{
    public class SequenceIdentityClustering
    {
        public class SequenceIdentityClusterMember
        {
            public int ClusterIndex;
            public string PdbId;
            public char ChainId;
            public string Sequence;

            public SequenceIdentityClusterMember(int clusterIndex, string pdbId, char chainId, string sequence)
            {
                ClusterIndex = clusterIndex;
                PdbId = pdbId;
                ChainId = chainId;
                Sequence = sequence;
            }
            public override string ToString()
            {
                return string.Join(",", new string[] {"" + ClusterIndex, PdbId, ""+ChainId, Sequence});
            }

            public static List<SequenceIdentityClusterMember> Load(string filename)
            {
                var result = new List<SequenceIdentityClusterMember>();
                if (!string.IsNullOrWhiteSpace(filename) && File.Exists(filename))
                {
                    var lines = File.ReadAllLines(filename);

                    foreach (var line in lines)
                    {
                        var lineSplit = line.Split(',');

                        result.Add(new SequenceIdentityClusterMember(int.Parse(lineSplit[0]), lineSplit[1],
                            lineSplit[2][0],
                            lineSplit[3]));
                    }
                }
                return result;
            }

            public static void Save(string outputFilename, List<SequenceIdentityClusterMember> output)
            {
                File.WriteAllLines(outputFilename, output.Select(a => a.ToString()).ToList());
            }
        }

        
        public static void PrintClusters(List<List<Sequence>> seqClusters)
        {
            for (int index = 0; index < seqClusters.Count; index++)
            {
                var cluster = seqClusters[index];
                Console.WriteLine("");
                Console.WriteLine("Cluster " + index);

                foreach (var seq in cluster)
                {
                    Console.WriteLine("- " +seq.Id);
                }
            }
        }

        public static List<SequenceIdentityClusterMember>  ClusterSequenceByAlignedSequenceIdentity(List<Sequence> seqList, ProteinBioClass.AlignmentIdentityOption alignmentIdentityOption, decimal mininumClusterPairwiseSimilarity = 0.3m, decimal mininumEvoClusterPairwiseSimilarity = 0.3m)
        {
            var allsequences = seqList.Select(a => new Tuple<string,char,string>(new ProteinBioClass.SequenceId(a.Id).PdbId,new ProteinBioClass.SequenceId(a.Id).ChainId, Sequence.EscapeAminoAcidSequence(a.FullSequence))).ToList();

            var sequences = allsequences.Select(a => a.Item3).Distinct().ToList();

            var sequenceIds = sequences.Select(a => allsequences.Where(b => b.Item3 == a).ToList()).ToList();
            

            var seqClusters = new List<List<string>>();
         


            for (int x = 0; x < sequences.Count; x++)
            {
                var seq1 = sequences[x];
                var newCluster = new List<string>();
                newCluster.Add(seq1);
                seqClusters.Add(newCluster);
            }

            for (int indexX = 0; indexX < sequences.Count; indexX++)
            {
                Console.WriteLine("Aligning sequence " + indexX);
                var seqX = sequences[indexX];
                //List<decimal> scoreList = new List<decimal>();
                //List<decimal> scoreEvoList = new List<decimal>();

                for (int indexY = 0; indexY < sequences.Count; indexY++)
                {
                   if (indexY <= indexX) continue;

                   var seqY = sequences[indexY];

                   if ((decimal)Math.Min(seqX.Length, seqY.Length) / (decimal)Math.Max(seqX.Length, seqY.Length) < mininumClusterPairwiseSimilarity) continue;

                    var cluster1 = seqClusters.FirstOrDefault(a => a.Contains(seqX));
                    var cluster2 = seqClusters.FirstOrDefault(a => a.Contains(seqY));

                    if (cluster1 != null && cluster2 != null && cluster1 == cluster2) continue;


                    var score = ProteinBioClass.AlignedSequenceSimilarityPercentage(seqX, seqY, ProteinBioClass.AlignmentType.NON, alignmentIdentityOption);

                    Console.WriteLine("1: " + seqX);
                    Console.WriteLine("2: " + seqY);
                    Console.WriteLine("Score1: " + score.Score);
                    Console.WriteLine("Score2: " + score.ScoreEvo);

                    if (score.Score < mininumClusterPairwiseSimilarity || score.ScoreEvo < mininumEvoClusterPairwiseSimilarity)
                    {
                        var x = ProteinBioClass.AlignedSequenceSimilarityPercentage(seqX, seqY, ProteinBioClass.AlignmentType.SIM, alignmentIdentityOption);
                        if (x.Score > score.Score) score.Score = x.Score;
                        if (x.ScoreEvo > score.ScoreEvo) score.ScoreEvo = x.ScoreEvo;
                    }

                    if (score.Score < mininumClusterPairwiseSimilarity || score.ScoreEvo < mininumEvoClusterPairwiseSimilarity)
                    {
                        var x = ProteinBioClass.AlignedSequenceSimilarityPercentage(seqX, seqY, ProteinBioClass.AlignmentType.NMW, alignmentIdentityOption);
                        if (x.Score > score.Score) score = x;
                        if (x.ScoreEvo > score.ScoreEvo) score.ScoreEvo = x.ScoreEvo;
                    }


                    if (score.Score >= mininumClusterPairwiseSimilarity && score.ScoreEvo >= mininumEvoClusterPairwiseSimilarity)
                    {
                        var newCluster = new List<string>();

                        newCluster.AddRange(cluster1);
                        newCluster.AddRange(cluster2);

                        seqClusters.Remove(cluster1);
                        seqClusters.Remove(cluster2);

                        seqClusters.Add(newCluster);
                    }

                    //scoreList.Add(score.Score);
                    //scoreEvoList.Add(score.ScoreEvo);

                }
                //Console.WriteLine("[" + string.Join(", ", scoreList.Select(a => String.Format("{0:0.00}", a)).ToList()) + "]");
                //Console.WriteLine("[" + string.Join(", ", scoreEvoList.Select(a => String.Format("{0:0.00}", a)).ToList()) + "]");
            }

            seqClusters = seqClusters.OrderBy(a => a.Count).ToList();

            var output = new List<SequenceIdentityClusterMember>();
            for (var index = 0; index < seqClusters.Count; index++)
            {
                var seqCluster = seqClusters[index];
                foreach (var item in seqCluster)
                {
                    var indexIds = sequences.IndexOf(item);
                    var ids = sequenceIds[indexIds];

                    foreach (var id in ids)
                    {
                        output.Add(new SequenceIdentityClusterMember(index + 1, ProteinBioClass.PdbIdFromPdbFilename(id.Item1), id.Item2, id.Item3));
                    }
                }
            }

            return output;
        }
        static void Main(string[] args)
        {
            // this program takes any set of proteins
            // 1. cluster all proteins by sequence identity >30%
            // 2. assign each protein its cluster id
            // 3. find clusters with many parnters of differing cluster ids

            //var sequenceFilename = args[0];
            //var minSequenceIdentity = decimal.Parse(args[1]);
            //var minEvoSequenceIdentity = decimal.Parse(args[2]);
            //var alignmentCalculationLength = int.Parse(args[3]);
            //var outputFilename = args[4];


            //var sequenceFilename = @"c:\viva\1b3ua\1b3ua.fasta";
            //var minSequenceIdentity = 0.3m;
            //var minEvoSequenceIdentity = 0.3m;
            //var alignmentCalculationLength = 1;
            //var outputFilename = @"c:\viva\1b3ua\1b3ua_homology.csv";

            var sequenceFilename = @"c:\categorym\catm.fasta";// @"c:\viva\1avw.fasta";
            var minSequenceIdentity = 0.3m;
            var minEvoSequenceIdentity = 0.3m;
            var alignmentCalculationLength = 2;
            var outputFilename = @"c:\categorym\catm_homology.csv";

            if (!File.Exists(sequenceFilename))
            {
                Console.WriteLine("Sequence file not found: " + sequenceFilename);
                return;
            }

            List<Sequence> seqList = Sequence.LoadSequenceFile(sequenceFilename, new string[] { null, "", "protein" });

            if (seqList == null || seqList.Count == 0)
            {
                Console.WriteLine("Sequence file could not be loaded: " + sequenceFilename);
                return;
            }

            if (alignmentCalculationLength < 0 || alignmentCalculationLength > 2)
            {
                Console.WriteLine("Invaid alignment identity option: " + sequenceFilename);
                return;
            }

            var alignmentIdentityOption = ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength;
            if (alignmentCalculationLength == 0) alignmentIdentityOption = ProteinBioClass.AlignmentIdentityOption.MinimumSequenceLength;
            else if (alignmentCalculationLength == 1) alignmentIdentityOption = ProteinBioClass.AlignmentIdentityOption.MeanAverageSequenceLength;
            else if (alignmentCalculationLength == 2) alignmentIdentityOption = ProteinBioClass.AlignmentIdentityOption.MaximumSequenceLength;

            List<SequenceIdentityClusterMember> output = ClusterSequenceByAlignedSequenceIdentity(seqList, alignmentIdentityOption, minSequenceIdentity, minEvoSequenceIdentity);

            SequenceIdentityClusterMember.Save(outputFilename, output);

        }
    }
}
