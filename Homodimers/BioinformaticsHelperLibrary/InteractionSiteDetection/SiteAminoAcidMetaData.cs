//-----------------------------------------------------------------------
// <copyright file="ProteinInterfaceAminoAcidMetaData.cs" company="Aaron Caffrey">
//     Copyright (c) 2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public class ProteinInterfaceAminoAcidMetaData
    {
        public int ArrayMemberIndex;
        public int PdbResidueSequenceIndex;

        public bool[] OppoproteinInterfaceInteractions;

        public string NonProteinInterfaceInteractionResidueNames1L = "";
        
        public string ResidueName1L = "";
        public string ProteinInterfaceInteractionResidueNames1L = "";

        public string NonProteinInterfaceInteractionResidueNames3L = "";
        
        public string ResidueName3L = "";
        public string ProteinInterfaceInteractionResidueNames3L = "";

        public ProteinInterfaceInteractionType ProteinInterfaceInteractionType = ProteinInterfaceInteractionType.NoInteractionFound;
    }
}