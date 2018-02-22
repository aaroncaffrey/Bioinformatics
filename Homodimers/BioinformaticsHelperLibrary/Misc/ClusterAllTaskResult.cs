using System.Collections.Generic;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;

namespace BioinformaticsHelperLibrary.Misc
{
    internal class ClusterAllTaskResult
    {
        public InteractionProteinInterfaceClusteringHierarchySpreadsheet InteractionProteinInterfaceClusteringHierarchySpreadsheet = new InteractionProteinInterfaceClusteringHierarchySpreadsheet();
        //public Dictionary<string, List<AtomPair>[,]> InteractionsBetweenProteinInterfacesDictionary = new Dictionary<string, List<AtomPair>[,]>();
        public Dictionary<string, InteractionBetweenProteinInterfacesListContainer> InteractionsBetweenProteinInterfacesDictionary = new Dictionary<string, InteractionBetweenProteinInterfacesListContainer>();
        public Dictionary<string, int[]> ProteinInterfaceCountDictionary = new Dictionary<string, int[]>();
        public List<ProteinInterfaceSequenceAndPositionData> ProteinInterfacePositionDataList = new List<ProteinInterfaceSequenceAndPositionData>();
        public Dictionary<string, ProteinInterfaceSymmetryModeValues> SymmetryModeDictionary = new Dictionary<string, ProteinInterfaceSymmetryModeValues>();
        //public Dictionary<string, > 
    }
}