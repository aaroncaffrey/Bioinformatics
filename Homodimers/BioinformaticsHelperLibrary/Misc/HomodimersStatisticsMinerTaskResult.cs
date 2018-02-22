using System.Collections.Generic;

namespace BioinformaticsHelperLibrary.Misc
{
    public class HomodimersStatisticsMinerTaskResult
    {
        public List<AminoAcidChainComposition> InteractionChainsAminoAcidCounter = new List<AminoAcidChainComposition>();
        public List<InteractionMatchPercentage> InteractionMatchPercentageList = new List<InteractionMatchPercentage>();
        public List<ProteinInteractionRecord> InteractionRecordList = new List<ProteinInteractionRecord>();
        public AminoAcidPairCompositionMatrix InteractionsAminoAcidToAminoAcidCounter = new AminoAcidPairCompositionMatrix();
        public List<AminoAcidChainComposition> WholeProteinChainsAminoAcidCounter = new List<AminoAcidChainComposition>();
    }
}