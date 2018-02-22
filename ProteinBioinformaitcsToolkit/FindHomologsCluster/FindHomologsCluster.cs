using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;

namespace FindHomologsCluster
{
    public class FindHomologsCluster
    {
        public class HomologClusterData
        {
            public int ClusterIndex;
            public int MemberIndex;
            public string PdbId;
            public char ChainId;
            public int ComplexChains;
            public int SequenceLength;
            public decimal MinClusterSequenceIdentity;
            public decimal MaxClusterSequenceIdentity;
            public decimal MinClusterSequenceIdentityEvo;
            public decimal MaxClusterSequenceIdentityEvo;
            public string Sequence;

            public override string ToString()
            {
                var data = new string[]
                {
                    ""+ClusterIndex,
                    ""+MemberIndex,
                    PdbId,
                    ""+ChainId,
                    ""+ComplexChains,
                    ""+SequenceLength,
                    ""+MinClusterSequenceIdentity,
                    ""+MaxClusterSequenceIdentity,
                    ""+MinClusterSequenceIdentityEvo,
                    ""+MaxClusterSequenceIdentityEvo,
                    Sequence,
                };

                return string.Join(",", data);
            }

            public HomologClusterData(string line)
            {
                if (string.IsNullOrWhiteSpace(line)) return;
                if (line[0] == ';') return;
                if (!line.Any(char.IsNumber)) return;

                var split = line.Split(',');

                //if (split.Length != 11) return;

                ClusterIndex = int.Parse(split[0]);
                MemberIndex = int.Parse(split[1]);
                PdbId = split[2];
                ChainId = split[3][0];
                ComplexChains = int.Parse(split[4]);
                SequenceLength = int.Parse(split[5]);
                MinClusterSequenceIdentity = decimal.Parse(split[6]);
                MaxClusterSequenceIdentity = decimal.Parse(split[7]);
                MinClusterSequenceIdentityEvo = decimal.Parse(split[8]);
                MaxClusterSequenceIdentityEvo = decimal.Parse(split[9]);
                Sequence = split[10];
            }

            public HomologClusterData(int clusterIndex, int memberIndex, string pdbId, char chainId, int complexChains, int sequenceLength, decimal minClusterSequenceIdentity, decimal maxClusterSequenceIdentity, decimal minClusterSequenceIdentityEvo, decimal maxClusterSequenceIdentityEvo, string sequence)
            {
                ClusterIndex = clusterIndex;
                MemberIndex = memberIndex;
                PdbId = pdbId;
                ChainId = chainId;
                ComplexChains = complexChains;
                SequenceLength = sequenceLength;
                MinClusterSequenceIdentity = minClusterSequenceIdentity;
                MaxClusterSequenceIdentity = maxClusterSequenceIdentity;
                MinClusterSequenceIdentityEvo = minClusterSequenceIdentityEvo;
                MaxClusterSequenceIdentityEvo = maxClusterSequenceIdentityEvo;
                Sequence = sequence;
            }

            public static List<HomologClusterData> Load(string file)
            {
                var result = new List<HomologClusterData>();
                var data = File.ReadAllLines(file);

                foreach (var line in data)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line[0] == ';') continue;
                    if (!line.Any(char.IsNumber)) continue;

                    var parsed = new HomologClusterData(line);
                    result.Add(parsed);
                }

                result = result.OrderBy(a => a.ClusterIndex).ThenBy(a => a.MemberIndex).ToList();
                return result;
            }
            public static List<HomologClusterData> Load(string[] files)
            {
                var result = new List<HomologClusterData>();
                foreach (var f in files)
                {
                    var d = Load(f);
                    result.AddRange(d);
                }
                result = result.OrderBy(a => a.ClusterIndex).ThenBy(a => a.MemberIndex).ToList();
                return result;
            }
        }

        private static void Main(string[] args)
        {
            // this program will load the homolog list in csv format and for homologs of X sequence distance return a list of all partners
            // however, some partners may be duplicates, which cannot initially be removed, since they may bind differently in other instances
            // then, because of such cases, unique id to describe each protein must be created... this is slightly problematic because
            // close target homologs of proteins are also considered to be the same protein as the query protein
            // which means that they could exist for more than one query protein


            // FindHomologsCluster.exe c:\pdb\ds96ub_homologs\ c:\pdb\pdb_seqres.fasta 0.9 0.9 > ds96ub_homologs.csv

            var homolog_csv_folder = args[0];
            var sequence_file = args[1];
            var min_similarity_str = args[2];
            var min_similarity_evo_str = args[3];

            var min_similarity = decimal.Parse(min_similarity_str);
            var min_similarity_evo = decimal.Parse(min_similarity_evo_str);

            var seqList = Sequence.LoadSequenceFile(sequence_file, new[] {null, "", "protein"});


            var homologCsvFiles = Directory.GetFiles(homolog_csv_folder, "homologs_?????.csv");

            var parsedData = FindHomologs.FindHomologs.HomologChain.Load(homologCsvFiles);
           

            Array.Clear(homologCsvFiles, 0, homologCsvFiles.Length);

            //var query_pdb_list = parsed_data.Select(a => new Tuple<string, string>(a.query_pdb_id, a.query_chainid)).ToList();

            //var target_pdb_list = parsed_data.Select(a => new Tuple<string, string>(a.target_pdb_id, a.target_chainid)).ToList();


            //var query_alignments = new List<homolog_csv>();

            var homologs_clustered = new List<List<Tuple<string, char>>>();

            //var min_similarity = 0.9m;

            foreach (var rec in parsedData)
            {
                if (rec.AlignmentScore >= min_similarity && rec.AlignmentScoreEvo >= min_similarity_evo)
                {
                    //var query_group = homologs_clustered.FirstOrDefault(a => a.FirstOrDefault(b => b.Item1 == rec.query_pdb_id && b.Item2 == rec.query_chainid) != null);
                    //var target_group = homologs_clustered.FirstOrDefault(a => a.FirstOrDefault(b => b.Item1 == rec.target_pdb_id && b.Item2 == rec.target_chainid) != null);

                    List<Tuple<string, char>> query_group = null;
                    List<Tuple<string, char>> target_group = null;

                    foreach (var cluster in homologs_clustered)
                    {
                        var xq = cluster.FirstOrDefault(b => b.Item1.ToUpperInvariant() == rec.QueryPdbId.ToUpperInvariant() && b.Item2 == rec.QueryChainId);
                        if (xq == null) continue;
                        query_group = cluster;
                        break;
                    }

                    foreach (var cluster in homologs_clustered)
                    {
                        var xt =
                            cluster.FirstOrDefault(b => b.Item1.ToUpperInvariant() == rec.TargetPdbId.ToUpperInvariant() && b.Item2 == rec.TargetChainId);
                        if (xt == null) continue;
                        target_group = cluster;
                        break;
                    }

                    var new_group = new List<Tuple<string, char>>();

                    if (query_group != null)
                    {
                        new_group.AddRange(query_group);
                        homologs_clustered.Remove(query_group);
                        query_group.Clear();
                    }
                    else
                    {
                        new_group.Add(new Tuple<string, char>(rec.QueryPdbId, rec.QueryChainId));
                    }

                    if (target_group != null)
                    {
                        new_group.AddRange(target_group);
                        homologs_clustered.Remove(target_group);
                        target_group.Clear();
                    }
                    else
                    {
                        new_group.Add(new Tuple<string, char>(rec.TargetPdbId, rec.TargetChainId));
                    }

                    new_group = new_group.Distinct().ToList(); // try without distinct?
                    new_group = new_group.OrderBy(a => a.Item1).ThenBy(a => a.Item2).ToList();

                    homologs_clustered.Add(new_group);
                }
            }

            var seq_list_ids = seqList.Select(a => new ProteinBioClass.SequenceId(a.Id)).ToList();


            var wd2 = new WorkDivision(homologs_clustered.Count);
            for (var thread2 = 0; thread2 < wd2.ThreadCount; thread2++)
            {
                var lti2 = thread2;

                wd2.TaskList.Add(Task.Run(() =>
                {
                    var result2 = new List<string>();

                    for (var index2 = wd2.ThreadFirstIndex[lti2]; index2 <= wd2.ThreadLastIndex[lti2]; index2++)
                    {
                        var cluster2 = homologs_clustered[index2];


                        var wd3 = new WorkDivision(cluster2.Count);

                        for (var thread3 = 0; thread3 < wd3.ThreadCount; thread3++)
                        {
                            var lti3 = thread3;
                            var cluster3 = cluster2;

                            var index4 = index2;
                            wd3.TaskList.Add(Task.Run(() =>
                            {
                                var result = new List<HomologClusterData>();
                                for (var index3 = wd3.ThreadFirstIndex[lti3]; index3 <= wd3.ThreadLastIndex[lti3]; index3++)
                                {
                                    var item = cluster3[index3];
                                    Sequence s = null;
                                    for (var j = 0; j < seqList.Count; j++)
                                    {
                                        if (seq_list_ids[j].PdbId.ToUpperInvariant() == item.Item1.ToUpperInvariant() && seq_list_ids[j].ChainId == item.Item2)
                                        {
                                            s = seqList[j];
                                            break;
                                        }
                                    }
                                    if (s == null) throw new Exception("sequence not found for "  + item.Item1 + ":" + item.Item2);

                                    var complexChains = seq_list_ids.Count(a => a.PdbId.ToUpperInvariant() == item.Item1.ToUpperInvariant());

                                    var minAlignmentScore = -1m;
                                    var maxAlignmentScore = -1m;


                                    var minAlignmentScoreEvo = -1m;
                                    var maxAlignmentScoreEvo = -1m;

                                    foreach (var item2 in cluster3)
                                    {
                                        if (ReferenceEquals(item, item2)) continue;

                                        Sequence s2 = null;
                                        for (var j2 = 0; j2 < seqList.Count; j2++)
                                        {
                                            if (seq_list_ids[j2].PdbId.ToUpperInvariant() == item2.Item1.ToUpperInvariant() &&
                                                seq_list_ids[j2].ChainId == item2.Item2)
                                            {
                                                s2 = seqList[j2];
                                                break;
                                            }
                                        }
                                        if (s2 == null) continue;

                                        var alignmentScore = ProteinBioClass.AlignedSequenceSimilarityPercentage(s,
                                            s2,
                                            ProteinBioClass.AlignmentType.NMW);

                                        if (alignmentScore.Score > maxAlignmentScore || maxAlignmentScore == -1m)
                                            maxAlignmentScore = alignmentScore.Score;
                                        if (alignmentScore.Score < minAlignmentScore || minAlignmentScore == -1m)
                                            minAlignmentScore = alignmentScore.Score;

                                        if (alignmentScore.ScoreEvo > maxAlignmentScoreEvo || maxAlignmentScoreEvo == -1m)
                                            maxAlignmentScoreEvo = alignmentScore.ScoreEvo;
                                        if (alignmentScore.ScoreEvo < minAlignmentScoreEvo || minAlignmentScoreEvo == -1m)
                                            minAlignmentScoreEvo = alignmentScore.ScoreEvo;
                                    }

                                    var r = new HomologClusterData(index4+1, index3+1, item.Item1, item.Item2, complexChains, Convert.ToInt32(s.Count()), minAlignmentScore, maxAlignmentScore, minAlignmentScoreEvo, maxAlignmentScoreEvo, s.FullSequence);
                                    
                                    result.Add(r);
                                }
                                return result;
                            }));
                            
                        }
                        wd3.WaitAllTasks();


                        

                        result2.Add("; Cluster # " + (index2+1) + " with " + wd3.ItemsToProcess + " protein chains");
                        result2.Add("cluster index,item index,pdb id,chain id,complex chains,seq len,min clstr sid,max clstr sid,min evo clstr sid,max evo clstr sid,sequence");

                        foreach (var task in wd3.TaskList)
                        {
                            //if (task.IsFaulted || task.IsCanceled) continue;
                            var tr = task as Task<List<HomologClusterData>>;
                            if (tr == null || tr.Result == null) continue;
                            result2.AddRange(tr.Result.Select(a=>a.ToString()).ToList());
                        }

                        result2.Add("");

                        
                    }

                    return result2;
                }));
                //wd2.TaskList.Add(task2);
            }
            wd2.WaitAllTasks();

            var result1 = new List<string>();
            foreach (var task in wd2.TaskList)
            {
                //if (task.IsFaulted || task.IsCanceled) continue;
                var tr = task as Task<List<string>>;
                if (tr == null || tr.Result == null) continue;
                result1.AddRange(tr.Result);
            }

            foreach (var line in result1)
            {
                Console.WriteLine(line);
            }
            // partners may have other interfaces, should those also be considered?
        }

        
    }
}