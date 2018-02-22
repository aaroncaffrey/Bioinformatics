using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary;


namespace FindHomologs
{
    public class FindHomologs
    {
        public class HomologChain
        {
            public string QueryPdbId;
            public char QueryChainId;
            public int QueryComplexTotalChains;

            public string TargetPdbId;
            public char TargetChainId;
            public int TargetComplexTotalChains;

            public string AlignmentType;
            public decimal AlignmentScore;
            public decimal AlignmentScoreEvo;

            public override string ToString()
            {
                return string.Join(",", new string[]
                {
                    QueryPdbId,
                    ""+QueryChainId,
                    ""+QueryComplexTotalChains,

                    TargetPdbId,
                    ""+TargetChainId,
                    ""+TargetComplexTotalChains,

                    AlignmentType,
                    ""+AlignmentScore,
                    ""+AlignmentScoreEvo,
                });
            }

            public HomologChain(string line)
            {
                var split = line.Split(',');

                //if (split.Length != 9) return;

                QueryPdbId = split[0];
                QueryChainId = split[1][0];
                QueryComplexTotalChains = int.Parse(split[2]);
                TargetPdbId = split[3];
                TargetChainId = split[4][0];
                TargetComplexTotalChains = int.Parse(split[5]);
                AlignmentType = split[6];
                AlignmentScore = decimal.Parse(split[7]);
                AlignmentScoreEvo = decimal.Parse(split[8]);
            }

            public HomologChain(string queryPdbId, char queryChainId, int queryComplexTotalChains, string targetPdbId, char targetChainId, int targetComplexTotalChains, string alignmentType, decimal alignmentScore, decimal alignmentScoreEvo)
            {
                QueryPdbId = queryPdbId;
                QueryChainId = queryChainId;
                QueryComplexTotalChains = queryComplexTotalChains;
                TargetPdbId = targetPdbId;
                TargetChainId = targetChainId;
                TargetComplexTotalChains = targetComplexTotalChains;
                AlignmentType = alignmentType;
                AlignmentScore = alignmentScore;
                AlignmentScoreEvo = alignmentScoreEvo;
            }

            public static List<HomologChain> Load(string file)
            {
                var result = new List<HomologChain>();

                var data = File.ReadAllLines(file);

                for (var index = 0; index < data.Length; index++)
                {
                    if (index < 2) continue;
                    var line = data[index];

                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line[0] == ';') continue;

                    result.Add(new HomologChain(line));
                }

                return result;
            }

            public static List<HomologChain> Load(string[] files)
            {
                var result = new List<HomologChain>();

                foreach (var file in files)
                {
                    result.AddRange(Load(file));
                }

                return result;
            }
        }

        static void Main(string[] args)
        {
            // this program takes a fasta or pdb file and finds all matching homologs 

            // FindHomologs.exe "c:\ds96ub\ds96ub.fasta" * "c:\pdb\pdb_seqres.fasta" NMW Y 0.3 75 c:\pdb\

            // alignment_type = (n)one, (s)imple, NMW, SWM

            var query_sequence_file = args[0]; //query.fasta
            var query_id_chain = args[1]; //1A2G:B
            var target_sequence_file = args[2]; //targets.fasta
            var alignment_type_str = args[3]; //NMW,SWM,SIM,NON
            if (alignment_type_str == "*") alignment_type_str = "NMW,SWM,SIM,NON";
            var alignment_type_str_split = alignment_type_str.ToUpperInvariant().Split(new char[] { ',', ';', ' ', '\t' });
            var compare_physicochemically = args[4]; //Y/N
            var compare_physicochemically_bool = compare_physicochemically == "Y";
            var min_similarity_str = args[5]; // 0.3
            var max_len_difference = args[6];
            var max_len_difference_int = int.Parse(max_len_difference);
            var output_folder = args[7];

            var minSimilarity = decimal.Parse(min_similarity_str);

            var alignmentTypes = new List<ProteinBioClass.AlignmentType>();
            if (alignment_type_str_split.Contains("NMW"))
            {
                alignmentTypes.Add(ProteinBioClass.AlignmentType.NMW);
            }
            if (alignment_type_str_split.Contains("SWM"))
            {
                alignmentTypes.Add(ProteinBioClass.AlignmentType.SWM);
            }
            if (alignment_type_str_split.Contains("SIM"))
            {
                alignmentTypes.Add(ProteinBioClass.AlignmentType.SIM);
            }
            if (alignment_type_str_split.Contains("NON") || alignmentTypes.Count == 0)
            {
                alignmentTypes.Add(ProteinBioClass.AlignmentType.NON);
            }
            if (alignmentTypes.Count < alignment_type_str_split.Length)
            {
                Console.WriteLine("; unknown alignment type");
                return;
            }

            // load list of query sequences
            var queryPdbid = query_id_chain.Split(new char[] { ':' })[0];
            var queryChainid = (query_id_chain.Contains(":") ? query_id_chain.Split(new char[] { ':' })[1] : "*")[0];


            var querySeq = Sequence.LoadSequenceFile(query_sequence_file, null);
            var queryResults = querySeq.Where(a =>
            {
                var id = new ProteinBioClass.SequenceId(a.Id);
                return (queryPdbid == "*" || id.PdbId.ToUpperInvariant() == queryPdbid.ToUpperInvariant()) &&
                       (queryChainid == '*' || id.ChainId == queryChainid);
            }).ToList();

            if (queryResults.Count == 0)
            {
                Console.WriteLine("; the query pdbids/chainids were not found");
                return;
            }


            // load list of target sequences
            var targetSeq = Sequence.LoadSequenceFile(target_sequence_file, new string[] { null, "", "protein" });
            targetSeq = targetSeq.Where(a => a.Count() >= 50).ToList();

            Console.WriteLine("; aligning " + queryResults.Count + " query sequences to " + targetSeq.Count + " target sequences");

            // perform alignment

            //var startTime = DateTime.Now;


            //var progress = 0;
            //var progressLock = new object();


            //var tasks = new List<Task<StringBuilder>>();

            var queryPdbIds = queryResults.Select(a => new ProteinBioClass.SequenceId(a.Id).PdbId);
            var targetPdbIds = targetSeq.Select(a => new ProteinBioClass.SequenceId(a.Id).PdbId);

            var queryPdbIdCounts = new Dictionary<string, int>();
            foreach (var x in queryPdbIds)
            {
                if (!queryPdbIdCounts.ContainsKey(x)) queryPdbIdCounts.Add(x, 1);
                else queryPdbIdCounts[x]++;
            }

            var targetPdbIdCounts = new Dictionary<string, int>();
            foreach (var x in targetPdbIds)
            {
                if (!targetPdbIdCounts.ContainsKey(x)) targetPdbIdCounts.Add(x, 1);
                else targetPdbIdCounts[x]++;
            }

            foreach (var _query in queryResults)
            {
                var _queryId = new ProteinBioClass.SequenceId(_query.Id);
                var filename = (new DirectoryInfo(output_folder).FullName) + @"\homologs_" + _queryId.PdbId + _queryId.ChainId + @".csv";

                // skip if already processed
                if (File.Exists(filename) && new FileInfo(filename).Length > 0) continue;

                var totalQueryPdbIdChains = queryPdbIdCounts[_queryId.PdbId];

                WorkDivision wd = new WorkDivision(targetSeq.Count);


                for (var thread = 0; thread < wd.ThreadCount; thread++)
                {
                    var query = _query;
                    var queryId = _queryId;
                    var lti = thread;
                    wd.TaskList.Add(Task.Run(() =>
                    {

                        var result = new List<HomologChain>();


                        for (var target = wd.ThreadFirstIndex[lti]; target <= wd.ThreadLastIndex[lti]; target++)
                        {
                            var targetobj = targetSeq[target];

                            if (max_len_difference_int != -1 && Math.Abs(targetobj.Count() - query.Count()) > max_len_difference_int) continue;

                            var targetId = new ProteinBioClass.SequenceId(targetobj.Id);

                            //var totalTargetPdbIdChains = targetSeq.Count(a => FindAtomicContacts.SequenceIdToPdbIdAndChainId(a.ID).PdbId.ToUpperInvariant() == targetId.PdbId.ToUpperInvariant());



                            //var timeRemaining =
                            //    TimeSpan.FromTicks(DateTime.Now.Subtract(startTime).Ticks *
                            //                       ((targetSeq.Count * queryResults.Count) - (progress + 1)) /
                            //                       (progress + 1));

                            foreach (var alignmentType in alignmentTypes)
                            {
                                var scores = ProteinBioClass.AlignedSequenceSimilarityPercentage(query, targetobj, alignmentType/*,
                                    compare_physicochemically_bool*/);
                                decimal percentSimilar;

                                if (compare_physicochemically_bool)
                                {
                                    percentSimilar = scores.ScoreEvo;
                                }
                                else
                                {
                                    percentSimilar = scores.Score;
                                }

                                if (percentSimilar >= minSimilarity)
                                {
                                    result.Add(new HomologChain(
                                            queryId.PdbId, queryId.ChainId, totalQueryPdbIdChains,
                                            targetId.PdbId, targetId.ChainId, targetPdbIdCounts[targetId.PdbId],

                                            alignmentType.ToString(),
                                            scores.Score,
                                            scores.ScoreEvo));
                                }
                            }
                            //if (progress % 20 == 0)
                            //    Console.Write("\r{0}% eta {1}     ",
                            //        Math.Round((decimal)(progress + 1) / (decimal)(targetSeq.Count * queryResults.Count),
                            //            2)
                            //            .ToString(CultureInfo.InvariantCulture),
                            //        timeRemaining.ToString(@"d\d\:h\h\:m\m\:s\s",
                            //            CultureInfo.InvariantCulture));
                            //lock (progressLock)
                            //    progress++;

                        }

                        return result;
                    }));


                }

                wd.WaitAllTasks();

                var mergedlist = new List<string>();

                mergedlist.Add("; " + _queryId.PdbId + ":" + _queryId.ChainId);
                mergedlist.Add(String.Join(",",
                new string[]
                {
                    "query pdb id", "query chain id", "query chains",
                    "target pdb id", "target chain id", "target chains",

                    "alignment method", "sequence similarity", "sequence evo similarity"
                }));

                foreach (var t in wd.TaskList)
                {
                    var tc = t as Task<List<HomologChain>>;

                    if (tc == null) throw new Exception("task in tasklist was null");

                    mergedlist.AddRange(tc.Result.Select(a=>a.ToString()).ToList());
                }

                if (string.IsNullOrWhiteSpace(output_folder))
                {
                    Console.WriteLine(String.Join(Environment.NewLine, mergedlist));
                }
                else
                {

                    File.WriteAllLines(filename, mergedlist);

                }




            }
        }
    }
}