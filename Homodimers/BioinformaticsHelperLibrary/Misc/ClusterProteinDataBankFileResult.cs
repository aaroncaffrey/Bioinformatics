//-----------------------------------------------------------------------
// <copyright file="____________.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;

namespace BioinformaticsHelperLibrary.Misc
{
    public class ClusterProteinDataBankFileResult
    {
        /// <summary>
        ///     This property is a list of all singular interactions between amino acids found in the protein (without any
        ///     clustering or other operations)
        /// </summary>
        public List<AtomPair> AaToAaInterationsList;

        /// <summary>
        ///     This property is the result of clustering interactions to form interaction proteinInterfaces.  Each step in the clustering
        ///     algorithm is recorded as a stage.  These stages can then be analysed to predict the most appropriate finishing
        ///     point for the clustering which is unlikely to be the final stage as at that stage all interactions will have been
        ///     grouped into one proteinInterface.
        /// </summary>
        public ClusteringFullResultListContainer ClusteringFullResultListContainer;

        /// <summary>
        ///     This property is the method which was used to perform the clustering.
        /// </summary>
        public ClusteringMethodOptions ClusteringMethodOptions;

        /// <summary>
        ///     This property is the maximum distance which was allowed when finding interacting amino acids.  -1 is the default
        ///     which means no limit.
        /// </summary>
        public int MaximumDistance;

        /// <summary>
        ///     This property is the maximum group size which was allowed for clustering amino acids together to form interaction
        ///     proteinInterfaces.  -1 is the default which means no limit.
        /// </summary>
        public int MaximumGroupSize;

        public ProteinChainListContainer PdbFileChains;

        /// <summary>
        ///     this property is the local filename of the PDB file as originally specified by the calling method.
        /// </summary>
        public string PdbFilename;

        /// <summary>
        ///     This property is a reference to the predicted final ("just the right amount") stage of clustering.
        /// </summary>
        public List<ClusteringFullResultListContainer.Chain.Stage> PredictedFinalChainClusteringStageList;

        /// <summary>
        ///     This property is the unique id of the protein in the protein data bank database.
        /// </summary>
        public string ProteinId;


        /// <summary>
        ///     This property is a reference to the proteinInterface symmetry mode data. The data available includes a list of interactions
        ///     found between interaction proteinInterfaces.
        /// </summary>
        public ProteinInterfaceAnalysisResultData ProteinInterfaceAnalysisResultData; //ProteinInterfaceSymmetryModeData;
    }
}