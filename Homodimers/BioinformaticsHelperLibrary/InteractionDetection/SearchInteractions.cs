using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Measurements;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.TaskManagement;

namespace BioinformaticsHelperLibrary.InteractionDetection
{
    public static class SearchInteractions
    {
        /// <summary>
        ///     This method finds chemical interaction bonds between atoms on separate chains.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="pdbFilename">The filename of the PDB file to parse for chemical interactions.</param>
        /// <param name="pdbIdChainIdList"></param>
        /// <param name="breakWhenFirstInteractionFound"></param>
        /// <param name="totalThreads"></param>
        /// <returns>Returns a list of atom pairs which are close enough in distance to have chemical interactions.</returns>
        public static List<AtomPair> FindInteractions(CancellationToken cancellationToken, decimal maxAtomInterationDistance /*= 8.0m*/, string pdbFilename, Dictionary<string, List<string>> pdbIdChainIdList, bool breakWhenFirstInteractionFound = false, int totalThreads = -1, bool sort = true, int requiredChains = -1)
        {
            if (ParameterValidation.IsLoadFilenameInvalid(pdbFilename)) // && ParameterValidation.IsProteinChainListContainerNullOrEmpty(pdbFileChains))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename));
            }

            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

            bool useCache = false;

            if (useCache && !string.IsNullOrWhiteSpace(proteinId))
            {
                var cachedInteractions = InteractionsCache.LoadPdbInteractionCache(proteinId, requiredChains);

                if (cachedInteractions != null)
                {
                    return cachedInteractions;
                }
            }

            var chainIdList = pdbIdChainIdList != null ? (proteinId != null && pdbIdChainIdList.ContainsKey(proteinId) ? pdbIdChainIdList[proteinId].ToArray() : null) : null;

            ProteinChainListContainer proteinFileChains = ProteinDataBankFileOperations.PdbAtomicChains(pdbFilename, chainIdList, requiredChains, requiredChains, true);

            List<AtomPair> atomPairList = FindInteractions(cancellationToken, maxAtomInterationDistance , proteinId, pdbIdChainIdList, proteinFileChains, breakWhenFirstInteractionFound, totalThreads, sort, requiredChains);

            if (atomPairList == null)
            {
                // only save if null, otherwise, already saved in other method
                atomPairList = new List<AtomPair>();
                if (useCache)
                {
                    InteractionsCache.SavePdbInteractionCache(proteinId, atomPairList, requiredChains);
                }
            }
            
            return atomPairList;
        }

        public static List<AtomPair> FindInteractions(CancellationToken cancellationToken, decimal maxAtomInterationDistance /*= 8.0m*/, string proteinId, Dictionary<string, List<string>> pdbIdChainIdList, ProteinChainListContainer proteinFileChains, bool breakWhenFirstInteractionFound = false, int totalThreads = -1, bool sort = true, int requiredChains = -1)
        {
            //const decimal maxInterationDistance = 8.0m;
            bool useCache = false;

            if (useCache && !string.IsNullOrWhiteSpace(proteinId))
            {
                var cachedInteractions = InteractionsCache.LoadPdbInteractionCache(proteinId, requiredChains);

                if (cachedInteractions != null)
                {
                    return cachedInteractions;
                }
            }

            // check required number of chains are found
            if (proteinFileChains == null || proteinFileChains.ChainList == null || (requiredChains > -1 && proteinFileChains.ChainList.Count != requiredChains))
            {
                return null;
            }

            // check that all chains have atoms
            if (proteinFileChains.ChainList.Any(chain => chain.AtomList == null || chain.AtomList.Count == 0))
            {
                return null;
            }

            // Make list of 3D positions of atoms.
            var positions = new List<Point3D>[proteinFileChains.ChainList.Count];

            for (int chainIndex = 0; chainIndex < proteinFileChains.ChainList.Count; chainIndex++)
            {
                positions[chainIndex] = Clustering.AtomRecordListToPoint3DList(proteinFileChains.ChainList[chainIndex]);
            }

            var tasks = new List<Task<List<AtomPair>>>();

            for (int chainIndexA = 0; chainIndexA < proteinFileChains.ChainList.Count; chainIndexA++)
            {
                for (int chainIndexB = 0; chainIndexB < proteinFileChains.ChainList.Count; chainIndexB++)
                {
                    if (chainIndexB == chainIndexA || chainIndexB < chainIndexA)
                    {
                        continue;
                    }

                    WorkDivision<List<AtomPair>> workDivision = new WorkDivision<List<AtomPair>>(proteinFileChains.ChainList[chainIndexA].AtomList.Count, totalThreads);

                    bool breakOut = false;
                    var lockBreakOut = new object();

                    for (int threadIndex = 0; threadIndex < workDivision.ThreadCount; threadIndex++)
                    {
                        int localThreadIndex = threadIndex;
                        int localChainIndexA = chainIndexA;
                        int localChainIndexB = chainIndexB;
                        WorkDivision<List<AtomPair>> localWorkDivision = workDivision;

                        Task<List<AtomPair>> task = Task.Run(() =>
                        {
                            var taskResult = new List<AtomPair>();

                            for (int atomIndexA = localWorkDivision.ThreadFirstIndex[localThreadIndex]; atomIndexA <= localWorkDivision.ThreadLastIndex[localThreadIndex]; atomIndexA++)
                            {
                                if (breakOut)
                                {
                                    break;
                                }

                                for (int atomIndexB = 0; atomIndexB < proteinFileChains.ChainList[localChainIndexB].AtomList.Count; atomIndexB++)
                                {
                                    if (breakOut || (breakWhenFirstInteractionFound && taskResult.Count > 0))
                                    {
                                        lock (lockBreakOut)
                                        {
                                            breakOut = true;
                                        }

                                        break;
                                    }

                                    if ((!positions[localChainIndexA][atomIndexA].ParseOK) || (!positions[localChainIndexB][atomIndexB].ParseOK)) continue;

                                    decimal atomicDistanceAngstroms3D = Point3D.Distance3D(positions[localChainIndexA][atomIndexA], positions[localChainIndexB][atomIndexB], true);

                                    // Chemical proteinInterface bonds found at 5 angstrom or less.
                                    if (atomicDistanceAngstroms3D <= 0.0m || atomicDistanceAngstroms3D > maxAtomInterationDistance) continue;

                                    var atomPair = new AtomPair(
                                        proteinId,
                                        proteinFileChains.ChainList[localChainIndexA].AtomList[atomIndexA],
                                        localChainIndexA,
                                        proteinId,
                                        localChainIndexB,
                                        proteinFileChains.ChainList[localChainIndexB].AtomList[atomIndexB],
                                        atomicDistanceAngstroms3D);


                                    taskResult.Add(atomPair);
                                }
                            }

                            if (taskResult.Count == 0)
                            {
                                return null;
                            }

                            return taskResult;
                        }, cancellationToken);

                        workDivision.TaskList.Add(task);
                    }

                    tasks.AddRange(workDivision.TaskList);
                }
            }

            
            try
            {
                Task[] tasksToWait = tasks.Where(task => task != null && !task.IsCompleted).ToArray<Task>();
                if (tasksToWait.Length > 0)
                {
                    Task.WaitAll(tasksToWait);
                }
            }
            catch (AggregateException)
            {
            }

            // merge all results
            
            var atomPairList = new List<AtomPair>();

            foreach (var task in tasks.Where(t => t != null && t.IsCompleted && !t.IsCanceled && !t.IsFaulted && t.Result != null && t.Result.Count > 0))
            {
                atomPairList.AddRange(task.Result);
            }

            if (sort && atomPairList != null && atomPairList.Count > 1)
            {
                atomPairList = atomPairList
                    .OrderBy(i => ProteinDataBankFileOperations.NullableTryParseInt32(i.Atom1.resSeq.FieldValue))
                    .ThenBy(i => ProteinDataBankFileOperations.NullableTryParseInt32(i.Atom1.serial.FieldValue))
                    .ThenBy(j => ProteinDataBankFileOperations.NullableTryParseInt32(j.Atom2.resSeq.FieldValue))
                    .ThenBy(j => ProteinDataBankFileOperations.NullableTryParseInt32(j.Atom2.serial.FieldValue))
                    .ToList();
            }

            if (useCache)
            {
                InteractionsCache.SavePdbInteractionCache(proteinId, atomPairList, requiredChains);
            }

            return atomPairList;
        }
    }
}