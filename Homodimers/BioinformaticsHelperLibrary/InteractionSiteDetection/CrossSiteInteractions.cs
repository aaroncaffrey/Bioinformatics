using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BioinformaticsHelperLibrary.InteractionDetection;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public class CrossProteinInterfaceInteractions
    {
        /// <summary>
        ///     This method finds interactions between detected proteinInterfaces.  It is specific to dimers with exactly two chains.  [Chain A
        ///     ProteinInterface Index, Chain B ProteinInterface Index]
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="pdbFilename"></param>
        /// <param name="pdbFileChains"></param>
        /// <param name="chainInteractingAtomLists"></param>
        /// <param name="fullClusteringResult"></param>
        /// <param name="proteinInterfacesClusteringResult"></param>
        /// <param name="detectedFinalStageIndexes"></param>
        /// <param name="pdbIdChainIdList"></param>
        /// <returns></returns>
        public static InteractionBetweenProteinInterfacesListContainer FindInteractionsBetweenAnyProteinInterfaces(
            CancellationToken cancellationToken,
            decimal maxAtomInterationDistance,
            string pdbFilename,
            Dictionary<string, List<string>> pdbIdChainIdList,
            ProteinChainListContainer pdbFileChains,
            ProteinChainListContainer chainInteractingAtomLists,
            ClusteringFullResultListContainer fullClusteringResult,
            ClusteringFullResultListContainer proteinInterfacesClusteringResult,
            int[] detectedFinalStageIndexes)
        {
            if (string.IsNullOrWhiteSpace(pdbFilename))
            {
                throw new ArgumentOutOfRangeException(nameof(pdbFilename));
            }

            if (!File.Exists(pdbFilename))
            {
                throw new FileNotFoundException("File not found", pdbFilename);
            }

            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(chainInteractingAtomLists))
            {
                throw new ArgumentOutOfRangeException(nameof(chainInteractingAtomLists));
            }

            if (ParameterValidation.IsClusteringFullResultListContainerNullOrEmpty(fullClusteringResult))
            {
                throw new ArgumentOutOfRangeException(nameof(fullClusteringResult));
            }

            if (ParameterValidation.IsClusteringFullResultListContainerNullOrEmpty(proteinInterfacesClusteringResult))
            {
                throw new ArgumentOutOfRangeException(nameof(proteinInterfacesClusteringResult));
            }

            if (ParameterValidation.IsIntArrayNullOrEmpty(detectedFinalStageIndexes))
            {
                throw new ArgumentOutOfRangeException(nameof(detectedFinalStageIndexes));
            }

            string proteinId = ProteinDataBankFileOperations.PdbIdFromPdbFilename(pdbFilename);

            var interactionBetweenProteinInterfacesListContainer = new InteractionBetweenProteinInterfacesListContainer();

            List<AtomPair> interactionList;

            if (pdbFileChains != null && pdbFileChains.ChainList != null && pdbFileChains.ChainList.Count > 0)
            {
                interactionList = SearchInteractions.FindInteractions(cancellationToken, maxAtomInterationDistance, proteinId, pdbIdChainIdList, pdbFileChains); //, false, -1, pdbFileChains);
            }
            else
            {
                interactionList = SearchInteractions.FindInteractions(cancellationToken, maxAtomInterationDistance, pdbFilename, pdbIdChainIdList);
            }

            var interactionInsideProteinInterfaceArray = new bool[interactionList.Count];

            ////////Console.WriteLine("");
            ////////Console.WriteLine("");
            ////////Console.WriteLine("------------------ START ------------------");
            //int c = 0;

            for (int chainIndexA = 0; chainIndexA < proteinInterfacesClusteringResult.ChainList.Count; chainIndexA++)
            {
                for (int chainIndexB = 0; chainIndexB < proteinInterfacesClusteringResult.ChainList.Count; chainIndexB++)
                {
                    if (chainIndexA == chainIndexB || chainIndexB < chainIndexA)
                    {
                        continue;
                    }

                    List<ClusteringFullResultListContainer.Chain.Stage.Cluster> proteinInterfaceListA = proteinInterfacesClusteringResult.ChainList[chainIndexA].StageList[detectedFinalStageIndexes[chainIndexA]].ClusterList;
                    List<ClusteringFullResultListContainer.Chain.Stage.Cluster> proteinInterfaceListB = proteinInterfacesClusteringResult.ChainList[chainIndexB].StageList[detectedFinalStageIndexes[chainIndexB]].ClusterList;

                    int realProteinInterfaceIndexA = -1;


                    for (int proteinInterfaceIndexA = 0; proteinInterfaceIndexA < proteinInterfaceListA.Count; proteinInterfaceIndexA++)
                    {
                        int realProteinInterfaceIndexB = -1;
                        List<int> proteinInterfaceMemberIndexListA = proteinInterfaceListA[proteinInterfaceIndexA].AtomIndexList;
                        List<ATOM_Record> proteinInterfaceAtomListA = proteinInterfaceMemberIndexListA.Select(proteinInterfaceMemberIndexA => chainInteractingAtomLists.ChainList[chainIndexA].AtomList[proteinInterfaceMemberIndexA]).ToList();
                        proteinInterfaceAtomListA = proteinInterfaceAtomListA.OrderBy(a => ProteinDataBankFileOperations.NullableTryParseInt32(a.resSeq.FieldValue)).ToList();
                        if (proteinInterfaceAtomListA.Count > 0)
                        {
                            realProteinInterfaceIndexA++;
                        }
                        else
                        {
                            continue;
                        }

                        for (int proteinInterfaceIndexB = 0; proteinInterfaceIndexB < proteinInterfaceListB.Count; proteinInterfaceIndexB++)
                        {
                            List<int> proteinInterfaceMemberIndexListB = proteinInterfaceListB[proteinInterfaceIndexB].AtomIndexList;
                            List<ATOM_Record> proteinInterfaceAtomListB = proteinInterfaceMemberIndexListB.Select(proteinInterfaceMemberIndexB => chainInteractingAtomLists.ChainList[chainIndexB].AtomList[proteinInterfaceMemberIndexB]).ToList();
                            proteinInterfaceAtomListB = proteinInterfaceAtomListB.OrderBy(b => ProteinDataBankFileOperations.NullableTryParseInt32(b.resSeq.FieldValue)).ToList();
                            if (proteinInterfaceAtomListB.Count > 0)
                            {
                                realProteinInterfaceIndexB++;
                            }
                            else
                            {
                                continue;
                            }

                            for (int proteinInterfaceAtomListIndexA = 0; proteinInterfaceAtomListIndexA < proteinInterfaceAtomListA.Count; proteinInterfaceAtomListIndexA++)
                            {
                                ATOM_Record atomA = proteinInterfaceAtomListA[proteinInterfaceAtomListIndexA];

                                for (int proteinInterfaceAtomListIndexB = 0; proteinInterfaceAtomListIndexB < proteinInterfaceAtomListB.Count; proteinInterfaceAtomListIndexB++)
                                {
                                    ATOM_Record atomB = proteinInterfaceAtomListB[proteinInterfaceAtomListIndexB];

                                    //c++;
                                    ////////Console.WriteLine(c.ToString().PadLeft(5) +
                                    //                  " Chain " + chainIndexA + " (" + proteinInterfaceListA.Count(a => a.AtomIndexList.Count > 0) + " proteinInterfaces) ProteinInterface " + realProteinInterfaceIndexA + " (" + proteinInterfaceAtomListA.Count + " atoms) <--->" +
                                    //                  " Chain " + chainIndexB + " (" + proteinInterfaceListB.Count(a => a.AtomIndexList.Count > 0) + " proteinInterfaces) ProteinInterface " + realProteinInterfaceIndexB + " (" + proteinInterfaceAtomListB.Count + " atoms)  --->" +
                                    //                  " chainID " + atomA.chainID.FieldValue + " resName " + atomA.resName.FieldValue + " resSeq " + atomA.resSeq.FieldValue + " <--->" +
                                    //                  " chainID " + atomB.chainID.FieldValue + " resName " + atomB.resName.FieldValue + " resSeq " + atomB.resSeq.FieldValue);


                                    for (int interactionIndex = 0; interactionIndex < interactionList.Count; interactionIndex++)
                                    {
                                        AtomPair interaction = interactionList[interactionIndex];

                                        if ((interaction.Atom1 == atomA && interaction.Atom2 == atomB) || (interaction.Atom1 == atomB && interaction.Atom2 == atomA))
                                        {
                                            interactionInsideProteinInterfaceArray[interactionIndex] = true;

                                            var interactionBetweenProteinInterfaces = new InteractionBetweenProteinInterfaces();
                                            interactionBetweenProteinInterfacesListContainer.InteractionBetweenProteinInterfacesList.Add(interactionBetweenProteinInterfaces);

                                            interactionBetweenProteinInterfaces.Atom1.Atom = atomA;
                                            interactionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinId = proteinId;
                                            interactionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ChainId = chainIndexA;
                                            interactionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinInterfaceId = realProteinInterfaceIndexA;

                                            interactionBetweenProteinInterfaces.Atom2.Atom = atomB;
                                            interactionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinId = proteinId;
                                            interactionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ChainId = chainIndexB;
                                            interactionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinInterfaceId = realProteinInterfaceIndexB;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            for (int interactionIndex = 0; interactionIndex < interactionInsideProteinInterfaceArray.Length; interactionIndex++)
            {
                bool interactionInsideProteinInterface = interactionInsideProteinInterfaceArray[interactionIndex];

                if (!interactionInsideProteinInterface)
                {
                    var interactionBetweenNonProteinInterfaces = new InteractionBetweenProteinInterfaces();
                    interactionBetweenProteinInterfacesListContainer.InteractionBetweenNonProteinInterfacesList.Add(interactionBetweenNonProteinInterfaces);

                    interactionBetweenNonProteinInterfaces.Atom1.Atom = interactionList[interactionIndex].Atom1;
                    interactionBetweenNonProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinId = proteinId;
                    interactionBetweenNonProteinInterfaces.Atom1.FullProteinInterfaceId.ChainId = interactionList[interactionIndex].Atom1FullProteinInterfaceId.ChainId;
                    interactionBetweenNonProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinInterfaceId = -1;

                    interactionBetweenNonProteinInterfaces.Atom2.Atom = interactionList[interactionIndex].Atom2;
                    interactionBetweenNonProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinId = proteinId;
                    interactionBetweenNonProteinInterfaces.Atom2.FullProteinInterfaceId.ChainId = interactionList[interactionIndex].Atom2FullProteinInterfaceId.ChainId;
                    interactionBetweenNonProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinInterfaceId = -1;
                }
            }

            ////////Console.WriteLine("------------------ END ------------------");

            // ensure sorted order
            interactionBetweenProteinInterfacesListContainer.InteractionBetweenProteinInterfacesList = interactionBetweenProteinInterfacesListContainer.InteractionBetweenProteinInterfacesList
                .OrderBy(a => a.Atom1.FullProteinInterfaceId.ChainId)
                .ThenBy(a => a.Atom1.FullProteinInterfaceId.ProteinInterfaceId)
                .ThenBy(a => ProteinDataBankFileOperations.NullableTryParseInt32(a.Atom1.Atom.resSeq.FieldValue))
                .ThenBy(a => a.Atom2.FullProteinInterfaceId.ChainId)
                .ThenBy(a => a.Atom2.FullProteinInterfaceId.ProteinInterfaceId)
                .ThenBy(a => ProteinDataBankFileOperations.NullableTryParseInt32(a.Atom2.Atom.resSeq.FieldValue))
                .ToList();

            interactionBetweenProteinInterfacesListContainer.InteractionBetweenNonProteinInterfacesList = interactionBetweenProteinInterfacesListContainer.InteractionBetweenNonProteinInterfacesList
                .OrderBy(a => a.Atom1.FullProteinInterfaceId.ChainId)
                .ThenBy(a => a.Atom1.FullProteinInterfaceId.ProteinInterfaceId)
                .ThenBy(a => ProteinDataBankFileOperations.NullableTryParseInt32(a.Atom1.Atom.resSeq.FieldValue))
                .ThenBy(a => a.Atom2.FullProteinInterfaceId.ChainId)
                .ThenBy(a => a.Atom2.FullProteinInterfaceId.ProteinInterfaceId)
                .ThenBy(a => ProteinDataBankFileOperations.NullableTryParseInt32(a.Atom2.Atom.resSeq.FieldValue))
                .ToList();

            // remove duplicates (as the list is sorted, duplicates will always be together in the list)
            for (int index = interactionBetweenProteinInterfacesListContainer.InteractionBetweenProteinInterfacesList.Count - 1; index > 0; index--)
            {
                InteractionBetweenProteinInterfaces lastInteractionBetweenProteinInterfaces = interactionBetweenProteinInterfacesListContainer.InteractionBetweenProteinInterfacesList[index - 1];
                InteractionBetweenProteinInterfaces thisInteractionBetweenProteinInterfaces = interactionBetweenProteinInterfacesListContainer.InteractionBetweenProteinInterfacesList[index];

                if (lastInteractionBetweenProteinInterfaces == null || thisInteractionBetweenProteinInterfaces == null)
                {
                    continue;
                }

                if (thisInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinId == lastInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinId &&
                    thisInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ChainId == lastInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ChainId &&
                    thisInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinInterfaceId == lastInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinInterfaceId &&
                    thisInteractionBetweenProteinInterfaces.Atom1.Atom == lastInteractionBetweenProteinInterfaces.Atom1.Atom &&
                    thisInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinId == lastInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinId &&
                    thisInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ChainId == lastInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ChainId &&
                    thisInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinInterfaceId == lastInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinInterfaceId &&
                    thisInteractionBetweenProteinInterfaces.Atom2.Atom == lastInteractionBetweenProteinInterfaces.Atom2.Atom)
                {
                    interactionBetweenProteinInterfacesListContainer.InteractionBetweenProteinInterfacesList.RemoveAt(index - 1);
                    //////Console.WriteLine("removed duplicate");
                }
            }

            for (int index = interactionBetweenProteinInterfacesListContainer.InteractionBetweenNonProteinInterfacesList.Count - 1; index > 0; index--)
            {
                InteractionBetweenProteinInterfaces lastInteractionBetweenProteinInterfaces = interactionBetweenProteinInterfacesListContainer.InteractionBetweenNonProteinInterfacesList[index - 1];
                InteractionBetweenProteinInterfaces thisInteractionBetweenProteinInterfaces = interactionBetweenProteinInterfacesListContainer.InteractionBetweenNonProteinInterfacesList[index];

                if (lastInteractionBetweenProteinInterfaces == null || thisInteractionBetweenProteinInterfaces == null)
                {
                    continue;
                }

                if (thisInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinId == lastInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinId &&
                    thisInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ChainId == lastInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ChainId &&
                    thisInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinInterfaceId == lastInteractionBetweenProteinInterfaces.Atom1.FullProteinInterfaceId.ProteinInterfaceId &&
                    thisInteractionBetweenProteinInterfaces.Atom1.Atom == lastInteractionBetweenProteinInterfaces.Atom1.Atom &&
                    thisInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinId == lastInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinId &&
                    thisInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ChainId == lastInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ChainId &&
                    thisInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinInterfaceId == lastInteractionBetweenProteinInterfaces.Atom2.FullProteinInterfaceId.ProteinInterfaceId &&
                    thisInteractionBetweenProteinInterfaces.Atom2.Atom == lastInteractionBetweenProteinInterfaces.Atom2.Atom)
                {
                    interactionBetweenProteinInterfacesListContainer.InteractionBetweenNonProteinInterfacesList.RemoveAt(index - 1);
                    //////Console.WriteLine("removed duplicate");
                }
            }

            return interactionBetweenProteinInterfacesListContainer;
        }
    }
}