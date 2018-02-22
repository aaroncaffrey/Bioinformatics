using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProteinBioinformaticsSharedLibrary;
using ProteinBioinformaticsSharedLibrary.AminoAcids;
using ProteinBioinformaticsSharedLibrary.Dssp;
using ProteinBioinformaticsSharedLibrary.ProteinDataBankRecordTypes;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;
using ProteinBioinformaticsSharedLibrary.Stride;

namespace ComplexInterfaces
{
    public class ComplexInterfaces
    {
        public static int SafeIntParse(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? -1 : int .Parse(string.Join("", str.Where(c => char.IsDigit(c)).ToList()));
        }

        public class InterfaceInterfaceData
        {
            public int ReceptorConsensusClusterIndex;
            public int ReceptorSequenceClusterIndex;
            public int ReceptorInterfaceSequenceClusterIndex;
            public int ReceptorSequenceDsspClusterIndex;
            public int ReceptorInterfaceSequenceDsspClusterIndex;
            public int ReceptorSequenceStrideClusterIndex;
            public int ReceptorInterfaceSequenceStrideClusterIndex;
            public int ReceptorMaxCluster;
            public string ReceptorInterfaceSitePositionLigandSpecificClusterIndex;
            //public int ReceptorInterfaceSitePositionAllLigandsClusterIndex;
            public string ReceptorUniqueIdentifier;
            public string ReceptorPdbId;
            public char ReceptorChainId;
            public int ReceptorInterfaceIndex;
            public int ReceptorInterfaceResSeqStart;
            public int ReceptorInterfaceResSeqEnd;
            public int ReceptorInterfaceResSeqLength;
            public int ReceptorInterfaceSuperStart;
            public int ReceptorInterfaceSuperEnd;
            public int ReceptorInterfaceSuperLength;
            public int ReceptorInterfaceTotalContacts;
            public int ReceptorInterfaceTotalNonContacts;
            public decimal ReceptorInterfaceDensity;
            public decimal ReceptorInterfaceMininumDensity;
            //public string ReceptorInterfaceFlankingBefore;
            public string ReceptorInterfaceAllAminoAcids;
            public string ReceptorInterfaceAllAminoAcidsSuper;
            //public string ReceptorInterfaceFlankingAfter;
            public string ReceptorInterfaceInteractionAminoAcids;
            public string ReceptorInterfaceNonInteractionAminoAcids;
            public string ReceptorInterfaceDsspSecondaryStructure;
            public string ReceptorInterfaceStrideSecondaryStructure;
            public string ReceptorSequenceFromSequenceFile;
            public string ReceptorSequenceFromStructureFile;
            public string ReceptorSequenceFromSequenceFileAligned;
            public string ReceptorSequenceFromStructureFileAligned;
            public string ReceptorSequenceSuper;
            public string ReceptorSequenceMissingResidues;
            public string ReceptorSequenceSuperMissingResidues;
            public string ReceptorSequenceDssp;
            public string ReceptorSequenceStride;

            public int LigandConsensusClusterIndex;
            public int LigandSequenceClusterIndex;
            public int LigandInterfaceSequenceClusterIndex;
            public int LigandSequenceDsspClusterIndex;
            public int LigandInterfaceSequenceDsspClusterIndex;
            public int LigandSequenceStrideClusterIndex;
            public int LigandInterfaceSequenceStrideClusterIndex;
            public int LigandMaxCluster;
            public string LigandInterfaceSitePositionLigandSpecificClusterIndex;
           // public int LigandInterfaceSitePositionAllLigandsClusterIndex;
            public string LigandUniqueIdentifier;
            public string LigandPdbId;
            public char LigandChainId;
            public int LigandInterfaceIndex;
            public int LigandInterfaceResSeqStart;
            public int LigandInterfaceResSeqEnd;
            public int LigandInterfaceResSeqLength;
            public int LigandInterfaceSuperStart;
            public int LigandInterfaceSuperEnd;
            public int LigandInterfaceSuperLength;
            public int LigandInterfaceTotalContacts;
            public int LigandInterfaceTotalNonContacts;
            public decimal LigandInterfaceDensity;
            public decimal LigandInterfaceMininumDensity;
            //public string LigandInterfaceFlankingBefore;
            public string LigandInterfaceAllAminoAcids;
            public string LigandInterfaceAllAminoAcidsSuper;
            //public string LigandInterfaceFlankingAfter;
            public string LigandInterfaceInteractionAminoAcids;
            public string LigandInterfaceNonInteractionAminoAcids;
            public string LigandInterfaceDsspSecondaryStructure;
            public string LigandInterfaceStrideSecondaryStructure;
            public string LigandSequenceFromSequenceFile;
            public string LigandSequenceFromStructureFile;
            public string LigandSequenceFromSequenceFileAligned;
            public string LigandSequenceFromStructureFileAligned;
            public string LigandSequenceSuper;
            public string LigandSequenceMissingResidues;
            public string LigandSequenceSuperMissingResidues;
            public string LigandSequenceDssp;
            public string LigandSequenceStride;
            public InterfaceInterfaceData(int receptorConsensusClusterIndex, int receptorSequenceClusterIndex, int receptorInterfaceSequenceClusterIndex, int receptorSequenceDsspClusterIndex, int receptorInterfaceSequenceDsspClusterIndex, int receptorSequenceStrideClusterIndex, int receptorInterfaceSequenceStrideClusterIndex, int receptorMaxCluster, string receptorInterfaceSitePositionLigandSpecificClusterIndex, string receptorUniqueIdentifier, string receptorPdbId, char receptorChainId, int receptorInterfaceIndex, int receptorInterfaceResSeqStart, int receptorInterfaceResSeqEnd, int receptorInterfaceResSeqLength, int receptorInterfaceSuperStart, int receptorInterfaceSuperEnd, int receptorInterfaceSuperLength, int receptorInterfaceTotalContacts, int receptorInterfaceTotalNonContacts, decimal receptorInterfaceDensity, decimal receptorInterfaceMininumDensity, string receptorInterfaceAllAminoAcids, string receptorInterfaceAllAminoAcidsSuper, string receptorInterfaceInteractionAminoAcids, string receptorInterfaceNonInteractionAminoAcids, string receptorInterfaceDsspSecondaryStructure, string receptorInterfaceStrideSecondaryStructure, string receptorSequenceFromSequenceFile, string receptorSequenceFromStructureFile, string receptorSequenceFromSequenceFileAligned, string receptorSequenceFromStructureFileAligned, string receptorSequenceSuper, string receptorSequenceMissingResidues, string receptorSequenceSuperMissingResidues, string receptorSequenceDssp, string receptorSequenceStride, int ligandConsensusClusterIndex, int ligandSequenceClusterIndex, int ligandInterfaceSequenceClusterIndex, int ligandSequenceDsspClusterIndex, int ligandInterfaceSequenceDsspClusterIndex, int ligandSequenceStrideClusterIndex, int ligandInterfaceSequenceStrideClusterIndex, int ligandMaxCluster, string ligandInterfaceSitePositionLigandSpecificClusterIndex, string ligandUniqueIdentifier, string ligandPdbId, char ligandChainId, int ligandInterfaceIndex, int ligandInterfaceResSeqStart, int ligandInterfaceResSeqEnd, int ligandInterfaceResSeqLength, int ligandInterfaceSuperStart, int ligandInterfaceSuperEnd, int ligandInterfaceSuperLength, int ligandInterfaceTotalContacts, int ligandInterfaceTotalNonContacts, decimal ligandInterfaceDensity, decimal ligandInterfaceMininumDensity, string ligandInterfaceAllAminoAcids, string ligandInterfaceAllAminoAcidsSuper, string ligandInterfaceInteractionAminoAcids, string ligandInterfaceNonInteractionAminoAcids, string ligandInterfaceDsspSecondaryStructure, string ligandInterfaceStrideSecondaryStructure, string ligandSequenceFromSequenceFile, string ligandSequenceFromStructureFile, string ligandSequenceFromSequenceFileAligned, string ligandSequenceFromStructureFileAligned, string ligandSequenceSuper, string ligandSequenceMissingResidues, string ligandSequenceSuperMissingResidues, string ligandSequenceDssp, string ligandSequenceStride)
            {
                ReceptorConsensusClusterIndex = receptorConsensusClusterIndex;
                ReceptorSequenceClusterIndex = receptorSequenceClusterIndex;
                ReceptorInterfaceSequenceClusterIndex = receptorInterfaceSequenceClusterIndex;
                ReceptorSequenceDsspClusterIndex = receptorSequenceDsspClusterIndex;
                ReceptorInterfaceSequenceDsspClusterIndex = receptorInterfaceSequenceDsspClusterIndex;
                ReceptorSequenceStrideClusterIndex = receptorSequenceStrideClusterIndex;
                ReceptorInterfaceSequenceStrideClusterIndex = receptorInterfaceSequenceStrideClusterIndex;
                ReceptorMaxCluster = receptorMaxCluster;
                ReceptorInterfaceSitePositionLigandSpecificClusterIndex = receptorInterfaceSitePositionLigandSpecificClusterIndex;
                ReceptorUniqueIdentifier = receptorUniqueIdentifier;
                ReceptorPdbId = receptorPdbId;
                ReceptorChainId = receptorChainId;
                ReceptorInterfaceIndex = receptorInterfaceIndex;
                ReceptorInterfaceResSeqStart = receptorInterfaceResSeqStart;
                ReceptorInterfaceResSeqEnd = receptorInterfaceResSeqEnd;
                ReceptorInterfaceResSeqLength = receptorInterfaceResSeqLength;
                ReceptorInterfaceSuperStart = receptorInterfaceSuperStart;
                ReceptorInterfaceSuperEnd = receptorInterfaceSuperEnd;
                ReceptorInterfaceSuperLength = receptorInterfaceSuperLength;
                ReceptorInterfaceTotalContacts = receptorInterfaceTotalContacts;
                ReceptorInterfaceTotalNonContacts = receptorInterfaceTotalNonContacts;
                ReceptorInterfaceDensity = receptorInterfaceDensity;
                ReceptorInterfaceMininumDensity = receptorInterfaceMininumDensity;
                ReceptorInterfaceAllAminoAcids = receptorInterfaceAllAminoAcids;
                ReceptorInterfaceAllAminoAcidsSuper = receptorInterfaceAllAminoAcidsSuper;
                ReceptorInterfaceInteractionAminoAcids = receptorInterfaceInteractionAminoAcids;
                ReceptorInterfaceNonInteractionAminoAcids = receptorInterfaceNonInteractionAminoAcids;
                ReceptorInterfaceDsspSecondaryStructure = receptorInterfaceDsspSecondaryStructure;
                ReceptorInterfaceStrideSecondaryStructure = receptorInterfaceStrideSecondaryStructure;
                ReceptorSequenceFromSequenceFile = receptorSequenceFromSequenceFile;
                ReceptorSequenceFromStructureFile = receptorSequenceFromStructureFile;
                ReceptorSequenceFromSequenceFileAligned = receptorSequenceFromSequenceFileAligned;
                ReceptorSequenceFromStructureFileAligned = receptorSequenceFromStructureFileAligned;
                ReceptorSequenceSuper = receptorSequenceSuper;
                ReceptorSequenceMissingResidues = receptorSequenceMissingResidues;
                ReceptorSequenceSuperMissingResidues = receptorSequenceSuperMissingResidues;
                ReceptorSequenceDssp = receptorSequenceDssp;
                ReceptorSequenceStride = receptorSequenceStride;
                LigandConsensusClusterIndex = ligandConsensusClusterIndex;
                LigandSequenceClusterIndex = ligandSequenceClusterIndex;
                LigandInterfaceSequenceClusterIndex = ligandInterfaceSequenceClusterIndex;
                LigandSequenceDsspClusterIndex = ligandSequenceDsspClusterIndex;
                LigandInterfaceSequenceDsspClusterIndex = ligandInterfaceSequenceDsspClusterIndex;
                LigandSequenceStrideClusterIndex = ligandSequenceStrideClusterIndex;
                LigandInterfaceSequenceStrideClusterIndex = ligandInterfaceSequenceStrideClusterIndex;
                LigandMaxCluster = ligandMaxCluster;
                LigandInterfaceSitePositionLigandSpecificClusterIndex = ligandInterfaceSitePositionLigandSpecificClusterIndex;
                LigandUniqueIdentifier = ligandUniqueIdentifier;
                LigandPdbId = ligandPdbId;
                LigandChainId = ligandChainId;
                LigandInterfaceIndex = ligandInterfaceIndex;
                LigandInterfaceResSeqStart = ligandInterfaceResSeqStart;
                LigandInterfaceResSeqEnd = ligandInterfaceResSeqEnd;
                LigandInterfaceResSeqLength = ligandInterfaceResSeqLength;
                LigandInterfaceSuperStart = ligandInterfaceSuperStart;
                LigandInterfaceSuperEnd = ligandInterfaceSuperEnd;
                LigandInterfaceSuperLength = ligandInterfaceSuperLength;
                LigandInterfaceTotalContacts = ligandInterfaceTotalContacts;
                LigandInterfaceTotalNonContacts = ligandInterfaceTotalNonContacts;
                LigandInterfaceDensity = ligandInterfaceDensity;
                LigandInterfaceMininumDensity = ligandInterfaceMininumDensity;
                LigandInterfaceAllAminoAcids = ligandInterfaceAllAminoAcids;
                LigandInterfaceAllAminoAcidsSuper = ligandInterfaceAllAminoAcidsSuper;
                LigandInterfaceInteractionAminoAcids = ligandInterfaceInteractionAminoAcids;
                LigandInterfaceNonInteractionAminoAcids = ligandInterfaceNonInteractionAminoAcids;
                LigandInterfaceDsspSecondaryStructure = ligandInterfaceDsspSecondaryStructure;
                LigandInterfaceStrideSecondaryStructure = ligandInterfaceStrideSecondaryStructure;
                LigandSequenceFromSequenceFile = ligandSequenceFromSequenceFile;
                LigandSequenceFromStructureFile = ligandSequenceFromStructureFile;
                LigandSequenceFromSequenceFileAligned = ligandSequenceFromSequenceFileAligned;
                LigandSequenceFromStructureFileAligned = ligandSequenceFromStructureFileAligned;
                LigandSequenceSuper = ligandSequenceSuper;
                LigandSequenceMissingResidues = ligandSequenceMissingResidues;
                LigandSequenceSuperMissingResidues = ligandSequenceSuperMissingResidues;
                LigandSequenceDssp = ligandSequenceDssp;
                LigandSequenceStride = ligandSequenceStride;
            }
            public InterfaceInterfaceData(string line)
            {
                if (string.IsNullOrWhiteSpace(line)) return;
                if (line[0] == ';') return;

                if (!line.Any(char.IsNumber)) return;

                var split = line.Split(',');

                var p = -1;

                ReceptorConsensusClusterIndex = SafeIntParse(split[++p]);
                ReceptorSequenceClusterIndex = SafeIntParse(split[++p]);
                ReceptorInterfaceSequenceClusterIndex = SafeIntParse(split[++p]);
                ReceptorSequenceDsspClusterIndex = SafeIntParse(split[++p]);
                ReceptorInterfaceSequenceDsspClusterIndex = SafeIntParse(split[++p]);
                ReceptorSequenceStrideClusterIndex = SafeIntParse(split[++p]);
                ReceptorInterfaceSequenceStrideClusterIndex = SafeIntParse(split[++p]);
                ReceptorMaxCluster = SafeIntParse(split[++p]);
                ReceptorInterfaceSitePositionLigandSpecificClusterIndex = split[++p];
                //ReceptorInterfaceSitePositionAllLigandsClusterIndex = SafeIntParse(split[++p]);
                ReceptorUniqueIdentifier = split[++p];
                ReceptorPdbId = split[++p];
                ReceptorChainId = split[++p][0];
                ReceptorInterfaceIndex = SafeIntParse(split[++p]);
                ReceptorInterfaceResSeqStart = SafeIntParse(split[++p]);
                ReceptorInterfaceResSeqEnd = SafeIntParse(split[++p]);
                ReceptorInterfaceResSeqLength = SafeIntParse(split[++p]);
                ReceptorInterfaceSuperStart = SafeIntParse(split[++p]);
                ReceptorInterfaceSuperEnd = SafeIntParse(split[++p]);
                ReceptorInterfaceSuperLength = SafeIntParse(split[++p]);
                ReceptorInterfaceTotalContacts = SafeIntParse(split[++p]);
                ReceptorInterfaceTotalNonContacts = SafeIntParse(split[++p]);
                ReceptorInterfaceDensity = decimal.Parse(split[++p]);
                ReceptorInterfaceMininumDensity = decimal.Parse(split[++p]);
                //ReceptorInterfaceFlankingBefore = split[++p];
                ReceptorInterfaceAllAminoAcids = split[++p];
                ReceptorInterfaceAllAminoAcidsSuper = split[++p];
                //ReceptorInterfaceFlankingAfter = split[++p];
                ReceptorInterfaceInteractionAminoAcids = split[++p];
                ReceptorInterfaceNonInteractionAminoAcids = split[++p];
                ReceptorInterfaceDsspSecondaryStructure = split[++p];
                ReceptorInterfaceStrideSecondaryStructure = split[++p];
                ReceptorSequenceFromSequenceFile = split[++p];
                ReceptorSequenceFromStructureFile = split[++p];
                ReceptorSequenceFromSequenceFileAligned = split[++p];
                ReceptorSequenceFromStructureFileAligned = split[++p];
                ReceptorSequenceSuper = split[++p];
                ReceptorSequenceMissingResidues = split[++p];
                ReceptorSequenceSuperMissingResidues = split[++p];
                ReceptorSequenceDssp = split[++p];
                ReceptorSequenceStride = split[++p];




                LigandConsensusClusterIndex = SafeIntParse(split[++p]);
                LigandSequenceClusterIndex = SafeIntParse(split[++p]);
                LigandInterfaceSequenceClusterIndex = SafeIntParse(split[++p]);
                LigandSequenceDsspClusterIndex = SafeIntParse(split[++p]);
                LigandInterfaceSequenceDsspClusterIndex = SafeIntParse(split[++p]);
                LigandSequenceStrideClusterIndex = SafeIntParse(split[++p]);
                LigandInterfaceSequenceStrideClusterIndex = SafeIntParse(split[++p]);
                LigandMaxCluster = SafeIntParse(split[++p]);
                LigandInterfaceSitePositionLigandSpecificClusterIndex = split[++p];
                //LigandInterfaceSitePositionAllLigandsClusterIndex = SafeIntParse(split[++p]);
                LigandUniqueIdentifier = split[++p];
                LigandPdbId = split[++p];
                LigandChainId = split[++p][0];
                LigandInterfaceIndex = SafeIntParse(split[++p]);
                LigandInterfaceResSeqStart = SafeIntParse(split[++p]);
                LigandInterfaceResSeqEnd = SafeIntParse(split[++p]);
                LigandInterfaceResSeqLength = SafeIntParse(split[++p]);
                LigandInterfaceSuperStart = SafeIntParse(split[++p]);
                LigandInterfaceSuperEnd = SafeIntParse(split[++p]);
                LigandInterfaceSuperLength = SafeIntParse(split[++p]);
                LigandInterfaceTotalContacts = SafeIntParse(split[++p]);
                LigandInterfaceTotalNonContacts = SafeIntParse(split[++p]);
                LigandInterfaceDensity = decimal.Parse(split[++p]);
                LigandInterfaceMininumDensity = decimal.Parse(split[++p]);
                //LigandInterfaceFlankingBefore = split[++p];
                LigandInterfaceAllAminoAcids = split[++p];
                LigandInterfaceAllAminoAcidsSuper = split[++p];
                //LigandInterfaceFlankingAfter = split[++p];
                LigandInterfaceInteractionAminoAcids = split[++p];
                LigandInterfaceNonInteractionAminoAcids = split[++p];
                LigandInterfaceDsspSecondaryStructure = split[++p];
                LigandInterfaceStrideSecondaryStructure = split[++p];
                LigandSequenceFromSequenceFile = split[++p];
                LigandSequenceFromStructureFile = split[++p];
                LigandSequenceFromSequenceFileAligned = split[++p];
                LigandSequenceFromStructureFileAligned = split[++p];
                LigandSequenceSuper = split[++p];
                LigandSequenceMissingResidues = split[++p];
                LigandSequenceSuperMissingResidues = split[++p];
                LigandSequenceDssp = split[++p];
                LigandSequenceStride = split[++p];
            }

            public static string Header()
            {
                return string.Join(",", new string[]
                {
                    "Receptor Consensus Cluster Index",
                    "Receptor Sequence Cluster Index",
                    "Receptor Interface Sequence Cluster Index",
                    "Receptor Sequence Dssp Cluster Index",
                    "Receptor Interface Sequence Dssp Cluster Index",
                    "Receptor Sequence Stride Cluster Index",
                    "Receptor Interface Sequence Stride Cluster Index",
                    "Receptor Max Cluster",
                    "Receptor Interface Site Position Ligand Specific Cluster Index",
                    //"Receptor Interface Site Position All Ligand Cluster Index",
                    "Receptor Unique Id",
                    "Receptor Pdb Id",
                    "Receptor Chain Id",
                    "Receptor Interface Index",
                    "Receptor Start",
                    "Receptor End",
                    "Receptor Length",
                    "Receptor Super Start",
                    "Receptor Super End",
                    "Receptor Super Length",
                    "Receptor Total Contacts",
                    "Receptor Total Non Contacts",
                    "Receptor Clustering Density",
                    "Receptor Clustering Mininum Density",
                    //"Receptor Interface Flanking Before",
                    "Receptor Interface Sequence",
                    "Receptor Interface Sequence Super",
                    //"Receptor Interface Flanking After",
                    "Receptor Interaction Sequence",
                    "Receptor Non-interaction Sequence",
                    "Receptor Interface Dssp",
                    "Receptor Interface Stride",
                    "Receptor Sequence From Sequence File",
                    "Receptor Sequence From Structure File",
                    "Receptor Sequence From Sequence File Aligned",
                    "Receptor Sequence From Structure File Aligned",
                    "Receptor Sequence Super",
                    "Receptor Sequence Missing Residues",
                    "Receptor Sequence Super Missing Residues",
                    "Receptor Sequence Dssp",
                    "Receptor Sequence Stride",

                    "Ligand Consensus Cluster Index",
                    "Ligand Sequence Cluster Index",
                    "Ligand Interface Sequence Cluster Index",
                    "Ligand Sequence Dssp Cluster Index",
                    "Ligand Interface Sequence Dssp Cluster Index",
                    "Ligand Sequence Stride Cluster Index",
                    "Ligand Interface Sequence Stride Cluster Index",
                    "Ligand Max Cluster",
                    "Ligand Interface Site Position Ligand Specific Cluster Index",
                    //"Ligand Interface Site Position All Ligand Cluster Index",
                    "Ligand Unique Id",
                    "Ligand Pdb Id",
                    "Ligand Chain Id",
                    "Ligand Interface Index",
                    "Ligand Res Seq Start",
                    "Ligand Res Seq End",
                    "Ligand Res Seq Length",
                    "Ligand Super Start",
                    "Ligand Super End",
                    "Ligand Super Length",
                    "Ligand Total Contacts",
                    "Ligand Total Non Contacts",
                    "Ligand Clustering Density",
                    "Ligand Clustering Mininum Density",
                    //"Ligand Interface Flanking Before",
                    "Ligand Interface Sequence",
                    "Ligand Interface Sequence Super",
                    //"Ligand Interface Flanking After",
                    "Ligand Interaction Sequence",
                    "Ligand Non-interaction Sequence",
                    "Ligand Interface Dssp",
                    "Ligand Interface Stride",
                    "Ligand Sequence From Sequence File",
                    "Ligand Sequence From Structure File",
                    "Ligand Sequence From Sequence File Aligned",
                    "Ligand Sequence From Structure File Aligned",
                    "Ligand Sequence Super",
                    "Ligand Sequence Missing Residues",
                    "Ligand Sequence Super Missing Residues",
                    "Ligand Sequence Dssp",
                    "Ligand Sequence Stride"
                });
            }



            public override string ToString()
            {
                return string.Join(",", new string[]
                {
                    ""+ReceptorConsensusClusterIndex,
                    ""+ReceptorSequenceClusterIndex,
                    ""+ReceptorInterfaceSequenceClusterIndex,
                    ""+ReceptorSequenceDsspClusterIndex,
                    ""+ReceptorInterfaceSequenceDsspClusterIndex,
                    ""+ReceptorSequenceStrideClusterIndex,
                    ""+ReceptorInterfaceSequenceStrideClusterIndex,
                    ""+ReceptorMaxCluster,
                    ""+ReceptorInterfaceSitePositionLigandSpecificClusterIndex,
                    //""+ReceptorInterfaceSitePositionAllLigandsClusterIndex,
                    ReceptorUniqueIdentifier,
                    ReceptorPdbId,
                    ""+ReceptorChainId,
                    ""+ReceptorInterfaceIndex,
                    ""+ReceptorInterfaceResSeqStart,
                    ""+ReceptorInterfaceResSeqEnd,
                    ""+ReceptorInterfaceResSeqLength,
                    ""+ReceptorInterfaceSuperStart,
                    ""+ReceptorInterfaceSuperEnd,
                    ""+ReceptorInterfaceSuperLength,
                    ""+ReceptorInterfaceTotalContacts,
                    ""+ReceptorInterfaceTotalNonContacts,
                    ""+ReceptorInterfaceDensity,
                    ""+ReceptorInterfaceMininumDensity,
                    //ReceptorInterfaceFlankingBefore,
                    ReceptorInterfaceAllAminoAcids,
                    ReceptorInterfaceAllAminoAcidsSuper,
                    //ReceptorInterfaceFlankingAfter,
                    ReceptorInterfaceInteractionAminoAcids,
                    ReceptorInterfaceNonInteractionAminoAcids,
                    ReceptorInterfaceDsspSecondaryStructure,
                    ReceptorInterfaceStrideSecondaryStructure,
                    ReceptorSequenceFromSequenceFile,
                    ReceptorSequenceFromStructureFile,
                    ReceptorSequenceFromSequenceFileAligned,
                    ReceptorSequenceFromStructureFileAligned,
                    ReceptorSequenceSuper,
                    ReceptorSequenceMissingResidues,
                    ReceptorSequenceSuperMissingResidues,
                    ReceptorSequenceDssp,
                    ReceptorSequenceStride,

                    ""+LigandConsensusClusterIndex,
                    ""+LigandSequenceClusterIndex,
                    ""+LigandInterfaceSequenceClusterIndex,
                    ""+LigandSequenceDsspClusterIndex,
                    ""+LigandInterfaceSequenceDsspClusterIndex,
                    ""+LigandSequenceStrideClusterIndex,
                    ""+LigandInterfaceSequenceStrideClusterIndex,
                    ""+LigandMaxCluster,
                    ""+LigandInterfaceSitePositionLigandSpecificClusterIndex,
                    //""+LigandInterfaceSitePositionAllLigandsClusterIndex,
                    LigandUniqueIdentifier,
                    LigandPdbId,
                    ""+LigandChainId,
                    ""+LigandInterfaceIndex,
                    ""+LigandInterfaceResSeqStart,
                    ""+LigandInterfaceResSeqEnd,
                    ""+LigandInterfaceResSeqLength,
                    ""+LigandInterfaceSuperStart,
                    ""+LigandInterfaceSuperEnd,
                    ""+LigandInterfaceSuperLength,
                    ""+LigandInterfaceTotalContacts,
                    ""+LigandInterfaceTotalNonContacts,
                    ""+LigandInterfaceDensity,
                    ""+LigandInterfaceMininumDensity,
                   // LigandInterfaceFlankingBefore,
                    LigandInterfaceAllAminoAcids,
                    LigandInterfaceAllAminoAcidsSuper,
                    //LigandInterfaceFlankingAfter,
                    LigandInterfaceInteractionAminoAcids,
                    LigandInterfaceNonInteractionAminoAcids,
                    LigandInterfaceDsspSecondaryStructure,
                    LigandInterfaceStrideSecondaryStructure,
                    LigandSequenceFromSequenceFile,
                    LigandSequenceFromStructureFile,
                    LigandSequenceFromSequenceFileAligned,
                    LigandSequenceFromStructureFileAligned,
                    LigandSequenceSuper,
                    LigandSequenceMissingResidues,
                    LigandSequenceSuperMissingResidues,
                    LigandSequenceDssp,
                    LigandSequenceStride

                });
            }

            public static List<InterfaceInterfaceData> Load(string file)
            {
                if (!File.Exists(file) || new FileInfo(file).Length == 0) return null;
                var result = new List<InterfaceInterfaceData>();
                var data = File.ReadAllLines(file);

                foreach (var line in data)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line[0] == ';') continue;
                    if (!line.Any(char.IsNumber)) continue;

                    var parsed = new InterfaceInterfaceData(line);

                    result.Add(parsed);
                }

                result = result.OrderBy(a => a.ReceptorUniqueIdentifier).ThenBy(a => a.LigandUniqueIdentifier).ToList();

                return result;
            }

            public static List<InterfaceInterfaceData> Load(string[] files)
            {
                var result = new List<InterfaceInterfaceData>();
                foreach (var file in files)
                {
                    var data = Load(file);
                    result.AddRange(data);
                }
                result = result.OrderBy(a => a.ReceptorUniqueIdentifier).ThenBy(a => a.LigandUniqueIdentifier).ToList();
                return result;
            }
        }

        public class InterfaceData
        {
            public string ReceptorInterfaceSitePositionLigandSpecificClusterIndex;
            public int MaxCluster;
            //public int ReceptorInterfaceSitePositionAllLigandsClusterIndex;
            public string UniqueIdentifier;
            public string PdbId;
            public char ReceptorChainId;
            public char LigandChainId;
            public int ReceptorInterfaceIndex;
            public int InterfaceResSeqStart;
            public int InterfaceResSeqEnd;
            public int InterfaceResSeqLength;
            public int InterfaceSuperStart;
            public int InterfaceSuperEnd;
            public int InterfaceSuperLength;
            public int InterfaceTotalContacts;
            public int InterfaceTotalNonContacts;
            public decimal InterfaceDensity;
            public decimal InterfaceMininumDensity;
            public string InterfaceDsspSecondaryStructure;
            public string InterfaceStrideSecondaryStructure;
            public string InterfaceContactAminoAcids;
            public string InterfaceNoContactAminoAcids;
            //public string InterfaceFlankingAminoAcidsBefore;
            public string InterfaceAllAminoAcids;
            public string InterfaceAllAminoAcidsSuper;
            //public string InterfaceFlankingAminoAcidsAfter;
            public string SequenceFromSequenceFile;
            public string SequenceFromStructureFile;
            public string SequenceFromSequenceFileAligned;
            public string SequenceFromStructureFileAligned;
            public string SequenceSuper;
            public string SequenceMissingResidues;
            public string SequenceSuperMissingResidues;
            public string SequenceDssp;
            public string SequenceStride;

            public static string Header()
            {
                return string.Join(",", new string[]
                {
                    "Receptor Interface Site Position Ligand Specific Cluster Index",
                    "Max Cluster",
                    //"Receptor Interface Site Position All Ligands Cluster Index",
                    "Unique Identifier",
                    "Pdb Id",
                    "Receptor Chain Id",
                    "Ligand Chain Id",
                    "Chain Interface Index",
                    "Interface Res Seq Start",
                    "Interface Res Seq End",
                    "Interface Res Seq Length",
                    "Interface Super Start",
                    "Interface Super End",
                    "Interface Super Length",
                    "Interface Contact",
                    "Interface No Contact",
                    "Interface Density",
                    "Interface Mininum Density",
                    "Interface Dssp Secondary Structure",
                    "Interface Stride Secondary Structure",
                    "Interface Contact Amino Acids",
                    "Interface No Contact Amino Acids",
                    //"Interface Flanking Amino Acids Before",
                    "Interface All Amino Acids",
                    "Interface All Amino Acids Super",
                    //"Interface Flanking Amino Acids After",
                    "Sequence From Sequence File",
                    "Sequence From Structure File",
                    "Sequence From Sequence File Aligned",
                    "Sequence From Structure File Aligned",
                    "Sequence Super",
                    "Sequence Missing Residues",
                    "Sequence Super Missing Residues",
                    "Sequence Dssp",
                    "Sequence Stride"
                });
            }
            public override string ToString()
            {
                return string.Join(",", new string[]
                {
                    ""+ReceptorInterfaceSitePositionLigandSpecificClusterIndex,
                    ""+MaxCluster,
                    //""+ReceptorInterfaceSitePositionAllLigandsClusterIndex,
            UniqueIdentifier,
            PdbId,
            ""+ReceptorChainId,
            ""+LigandChainId,
            ""+ReceptorInterfaceIndex,
            ""+InterfaceResSeqStart,
            ""+InterfaceResSeqEnd,
            ""+InterfaceResSeqLength,
            ""+InterfaceSuperStart,
            ""+InterfaceSuperEnd,
            ""+InterfaceSuperLength,
            ""+InterfaceTotalContacts,
            ""+InterfaceTotalNonContacts,
            ""+InterfaceDensity,
            ""+InterfaceMininumDensity,
            InterfaceDsspSecondaryStructure,
            InterfaceStrideSecondaryStructure,
            InterfaceContactAminoAcids,
            InterfaceNoContactAminoAcids,
            //InterfaceFlankingAminoAcidsBefore,
            InterfaceAllAminoAcids,
            InterfaceAllAminoAcidsSuper,
            //InterfaceFlankingAminoAcidsAfter,
            SequenceFromSequenceFile,
            SequenceFromStructureFile,
            SequenceFromSequenceFileAligned,
            SequenceFromStructureFileAligned,
            SequenceSuper,
            SequenceMissingResidues,
            SequenceSuperMissingResidues,
            SequenceDssp,
            SequenceStride
        });
            }

            public InterfaceData() {
                
            }

            public InterfaceData(string receptorInterfaceSitePositionLigandSpecificClusterIndex, int maxCluster, string uniqueIdentifier, string pdbId, char receptorChainId, char ligandChainId, int receptorInterfaceIndex, int interfaceResSeqStart, int interfaceResSeqEnd, int interfaceResSeqLength, int interfaceSuperStart, int interfaceSuperEnd, int interfaceSuperLength, int interfaceTotalContacts, int interfaceTotalNonContacts, decimal interfaceDensity, decimal interfaceMininumDensity, string interfaceDsspSecondaryStructure, string interfaceStrideSecondaryStructure, string interfaceContactAminoAcids, string interfaceNoContactAminoAcids, string interfaceAllAminoAcids, string interfaceAllAminoAcidsSuper, string sequenceFromSequenceFile, string sequenceFromStructureFile, string sequenceFromSequenceFileAligned, string sequenceFromStructureFileAligned, string sequenceSuper, string sequenceMissingResidues, string sequenceSuperMissingResidues, string sequenceDssp, string sequenceStride)
            {
                ReceptorInterfaceSitePositionLigandSpecificClusterIndex = receptorInterfaceSitePositionLigandSpecificClusterIndex;
                MaxCluster = maxCluster;
                UniqueIdentifier = uniqueIdentifier;
                PdbId = pdbId;
                ReceptorChainId = receptorChainId;
                LigandChainId = ligandChainId;
                ReceptorInterfaceIndex = receptorInterfaceIndex;
                InterfaceResSeqStart = interfaceResSeqStart;
                InterfaceResSeqEnd = interfaceResSeqEnd;
                InterfaceResSeqLength = interfaceResSeqLength;
                InterfaceSuperStart = interfaceSuperStart;
                InterfaceSuperEnd = interfaceSuperEnd;
                InterfaceSuperLength = interfaceSuperLength;
                InterfaceTotalContacts = interfaceTotalContacts;
                InterfaceTotalNonContacts = interfaceTotalNonContacts;
                InterfaceDensity = interfaceDensity;
                InterfaceMininumDensity = interfaceMininumDensity;
                InterfaceDsspSecondaryStructure = interfaceDsspSecondaryStructure;
                InterfaceStrideSecondaryStructure = interfaceStrideSecondaryStructure;
                InterfaceContactAminoAcids = interfaceContactAminoAcids;
                InterfaceNoContactAminoAcids = interfaceNoContactAminoAcids;
                InterfaceAllAminoAcids = interfaceAllAminoAcids;
                InterfaceAllAminoAcidsSuper = interfaceAllAminoAcidsSuper;
                SequenceFromSequenceFile = sequenceFromSequenceFile;
                SequenceFromStructureFile = sequenceFromStructureFile;
                SequenceFromSequenceFileAligned = sequenceFromSequenceFileAligned;
                SequenceFromStructureFileAligned = sequenceFromStructureFileAligned;
                SequenceSuper = sequenceSuper;
                SequenceMissingResidues = sequenceMissingResidues;
                SequenceSuperMissingResidues = sequenceSuperMissingResidues;
                SequenceDssp = sequenceDssp;
                SequenceStride = sequenceStride;
            }
            public InterfaceData(string line)
            {
                if (string.IsNullOrWhiteSpace(line)) return;
                if (line[0] == ';') return;
                if (!line.Any(char.IsNumber)) return;

                var split = line.Split(',');

                //if (split.Length != 18) return;

                var p = -1;

                ReceptorInterfaceSitePositionLigandSpecificClusterIndex = split[++p];
                MaxCluster = SafeIntParse(split[++p]);
                //ReceptorInterfaceSitePositionAllLigandsClusterIndex = SafeIntParse(split[++p]);
                UniqueIdentifier = split[++p];
                PdbId = split[++p];
                ReceptorChainId = split[++p][0];
                LigandChainId = split[++p][0];
                ReceptorInterfaceIndex = SafeIntParse(split[++p]);
                InterfaceResSeqStart = SafeIntParse(split[++p]);
                InterfaceResSeqEnd = SafeIntParse(split[++p]);
                InterfaceResSeqLength = SafeIntParse(split[++p]);
                InterfaceSuperStart = SafeIntParse(split[++p]);
                InterfaceSuperEnd = SafeIntParse(split[++p]);
                InterfaceSuperLength = SafeIntParse(split[++p]);
                InterfaceTotalContacts = SafeIntParse(split[++p]);
                InterfaceTotalNonContacts = SafeIntParse(split[++p]);
                InterfaceDensity = decimal.Parse(split[++p]);
                InterfaceMininumDensity = decimal.Parse(split[++p]);
                InterfaceDsspSecondaryStructure = split[++p];
                InterfaceStrideSecondaryStructure = split[++p];
                InterfaceContactAminoAcids = split[++p];
                InterfaceNoContactAminoAcids = split[++p];
                //InterfaceFlankingAminoAcidsBefore = split[++p];
                InterfaceAllAminoAcids = split[++p];
                InterfaceAllAminoAcidsSuper = split[++p];
                //InterfaceFlankingAminoAcidsAfter = split[++p];
                SequenceFromSequenceFile = split[++p];
                SequenceFromStructureFile = split[++p];
                SequenceFromSequenceFileAligned = split[++p];
                SequenceFromStructureFileAligned = split[++p];
                SequenceSuper = split[++p];
                SequenceMissingResidues = split[++p];
                SequenceSuperMissingResidues = split[++p];
                SequenceDssp = split[++p];
                SequenceStride = split[++p];
            }

            public static List<InterfaceData> Load(string file)
            {
                var result = new List<InterfaceData>();
                var data = File.ReadAllLines(file);

                foreach (var line in data)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line[0] == ';') continue;
                    if (!line.Any(char.IsNumber)) continue;

                    var parsed = new InterfaceData(line);
                    result.Add(parsed);
                }

                result = result.OrderBy(a => a.UniqueIdentifier).ToList();

                return result;
            }

            public static List<InterfaceData> Load(string[] files)
            {
                var result = new List<InterfaceData>();
                foreach (var f in files)
                {
                    var d = Load(f);
                    result.AddRange(d);
                }
                result = result.OrderBy(a => a.UniqueIdentifier).ToList();
                return result;
            }
        }

        public static bool WithinRange(int query, int min, int max)
        {
            return query >= min && query <= max;
        }

        public static string UniqueInterfaceId(string pdbId, string chainIdToChainId, int min, int max, decimal minimumDensity)
        {
            return String.Join("-",
                new List<string>()
                {
                    "IF",
                    pdbId,
                    chainIdToChainId,
                    (min < 0 ? "-" : "") + min.ToString().PadLeft(5, '0').Replace("-", "0"),
                    (max < 0 ? "-" : "") + max.ToString().PadLeft(5, '0').Replace("-", "0"),
                    "" + minimumDensity
                });
        }

        static void Main(string[] args)
        {
            var parameters = new string[,]
                       {
                { "[sequence_file]", "sequence file" },//1
                { "[pdb_or_atoms_file]", "output from the ComplexAtoms program"},//2
                { "[contacts_file]", "output from the ComplexContacts program"},//3
                { "[maxcluster_csv]", "output from the maxcluster program in csv format"},//3
                { "[dssp_file]", "output from DSSP program" },//4
                { "[stride_file]", "output from STRIDE program" },//5
                { "[chain_ids]", "list of chains to check [i.e. A,B,C or - for all]"},//6
                { "[max_distance]", "maximum allowed contact distance in angstroms [i.e. 5.0 or 8.0]"},//7
                { "[min_length]", "mininum interface length [i.e. 1]"},//8
                { "[flanking_residues]", "number of interface flanking residues to obtain"},//9
                { "[[densities]]", "optional densities.  when ommitted, calculated at every 0.1"},//10
                { "[[output_file1]]", "optional output file.  when ommitted, output to console"},//11
                { "[[output_file2]]", "optional output file.  when ommitted, output to console"},//12
                { "[[overwrite]]", "overwrite output files if they already exist"},//12
                       };

            var maxParamLength = parameters.Cast<string>().Where((a, i) => i % 2 == 0).Max(a => a.Length);
            var exeFilename = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);


            if (args.Length < 8)
            {
                Console.WriteLine(exeFilename + @" is a program to list protein-protein interfaces for a PDB file ATOM records with PDB contacts data file from ComplexContacts.");
                Console.WriteLine();
                Console.WriteLine(@"Usage:");
                Console.WriteLine(ProteinBioClass.WrapConsoleText(exeFilename + @" " + String.Join(" ", parameters.Cast<string>().Where((a, i) => i % 2 == 0)), maxParamLength + 2, 1));
                Console.WriteLine();
                Console.WriteLine(@"Example:");
                Console.WriteLine(ProteinBioClass.WrapConsoleText(exeFilename + @" ""c:\pdb_db\atoms\atoms1a12.pdb"" ""c:\pdb_db\contacts\contacts1a12.pdb"" - 8.0 1 ""0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,1.0"" ""c:\pdb_db\interfaces\interfaces1a12.csv""", maxParamLength + 2, 1));
                Console.WriteLine();
                Console.WriteLine(@"Arguments:");
                for (var i = 0; i < parameters.GetLength(0); i++) Console.WriteLine(@" " + parameters[i, 0].PadLeft(maxParamLength, ' ') + " " + ProteinBioClass.WrapConsoleText(parameters[i, 1], maxParamLength + 2, 1, false));
                Console.WriteLine();
                return;
            }

            // load arguments (most are min length of 1, so that - or etc can be used to leave out arguments, except for ids that can actually be 1 char such as chain ids
            var p = 0;
            var sequenceFilename = args.Length > p && args[p].Length > 1 ? args[p] : "";
            sequenceFilename = sequenceFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + sequenceFilename);

            p++;
            var fullOrTraceAtomsFilename = args.Length > p && args[p].Length > 1 ? args[p] : "";
            fullOrTraceAtomsFilename = fullOrTraceAtomsFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + fullOrTraceAtomsFilename);

            p++;
            var contactsFilename = args.Length > p && args[p].Length > 1 ? args[p] : "";
            contactsFilename = contactsFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + contactsFilename);

            p++;
            var maxclusterCsvFilename = args.Length > p && args[p].Length > 1 ? args[p] : "";
            maxclusterCsvFilename = maxclusterCsvFilename.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + maxclusterCsvFilename);

            p++;
            var dsspFolder = args.Length > p && args[p].Length > 1 ? args[p] : "";
            dsspFolder = dsspFolder.Replace("\"", "");
            //if (!string.IsNullOrWhiteSpace(dsspFilename)) dsspFilename = Path.GetDirectoryName(dsspFilename) + @"\" + Path.GetFileNameWithoutExtension(dsspFilename).Substring(0, 4) + ".dssp";//Path.GetExtension(dsspFilename);
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + dsspFolder);
            

            p++;
            var strideFolder = args.Length > p && args[p].Length > 1 ? args[p] : "";
            strideFolder = strideFolder.Replace("\"", "");
            //if (!string.IsNullOrWhiteSpace(strideFilename)) strideFilename = Path.GetDirectoryName(strideFilename) + @"\" + Path.GetFileNameWithoutExtension(strideFilename).Substring(0, 4) + ".stride";//Path.GetExtension(dsspFilename);
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + strideFolder);
            

            p++;
            var chainIds = args.Length > p && args[p].Length > 0 ? args[p].Replace(" ", "").Replace(",", ", ") : "";
            if (string.IsNullOrWhiteSpace(chainIds) || chainIds.Contains('-')) chainIds = "-";
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + chainIds);

            p++;
            var maxDistance = args.Length > p && args[p].Length > 0 ? Decimal.Parse(args[p]) : 0.0m;
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + maxDistance);

            p++;
            var minLength = args.Length > p && args[p].Length > 0 ? args[p] : "";
            if (string.IsNullOrWhiteSpace(minLength))
            {
                minLength = "1";
            }
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + minLength);

            p++;
            var flankingLength = args.Length > p && args[p].Length > 0 ? SafeIntParse(args[p]) : 0;
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + flankingLength);

            p++;
            var densities = args.Length > p && args[p].Length > 0 ? args[p] : "";
            if (string.IsNullOrWhiteSpace(densities) || densities.Contains('-'))
            {
                densities = "0.0,0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,1.0";
            }
            densities = densities.Replace(" ", "").Replace(",", ", ");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + densities);

            p++;
            var outputFilename1 = args.Length > p && args[p].Length > 0 ? args[p] : "";
            outputFilename1 = outputFilename1.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + outputFilename1);

            p++;
            var outputFilename2 = args.Length > p && args[p].Length > 0 ? args[p] : "";
            outputFilename2 = outputFilename2.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + outputFilename2);

            p++;
            var overwrite = args.Length > p && args[p].Length > 0 ? args[p] : "Y";
            overwrite = overwrite.Replace("\"", "");
            Console.WriteLine("; " + parameters[p, 0].PadLeft(maxParamLength, ' ') + " = " + overwrite);

            Console.WriteLine();

            if (overwrite != "Y")
            {
                var exit = false;
                if (!string.IsNullOrWhiteSpace(outputFilename1) && File.Exists(outputFilename1))
                {
                    Console.WriteLine("; File exists: " + outputFilename1);

                    exit = true;
                }
                if (!string.IsNullOrWhiteSpace(outputFilename2) && File.Exists(outputFilename2))
                {
                    Console.WriteLine("; File exists: " + outputFilename2);
                    exit = true;
                }

                if (exit) return;
            }


            // check arguments
            var sequenceListFromSequenceFile = new List<Sequence>();
            if (!string.IsNullOrWhiteSpace(sequenceFilename) && sequenceFilename.Trim().Length > 1 && File.Exists(sequenceFilename))
            {
                sequenceListFromSequenceFile = Sequence.LoadSequenceFile(sequenceFilename, new string[] { null, "", "protein" });
            }

            //var atomsFilenameNoFolderNoExtension = Path.GetFileNameWithoutExtension(fullOrTraceAtomsFilename);
            //var atomsFilenamePdbId = atomsFilenameNoFolderNoExtension.Length > 4 ? atomsFilenameNoFolderNoExtension.Substring(atomsFilenameNoFolderNoExtension.Length - 4) : atomsFilenameNoFolderNoExtension;
            var atomsFilenamePdbId = ProteinBioClass.PdbIdFromPdbFilename(fullOrTraceAtomsFilename);

            //var contactsFilenameNoFolderNoExtension = Path.GetFileNameWithoutExtension(contactsFilename);
            //var contactsFilenamePdbId = contactsFilenameNoFolderNoExtension.Length > 4 ? contactsFilenameNoFolderNoExtension.Substring(contactsFilenameNoFolderNoExtension.Length - 4) : contactsFilenameNoFolderNoExtension;
            var contactsFilenamePdbId = ProteinBioClass.PdbIdFromPdbFilename(contactsFilename);

            if (atomsFilenamePdbId != contactsFilenamePdbId)
            {
                Console.WriteLine(@"; Warning: Atoms and Contacts PDB filename ends do not match [ " + atomsFilenamePdbId + " , " + contactsFilenamePdbId + " ]");
                Console.WriteLine();
            }

            var inputFilePdbId = contactsFilenamePdbId;
            var minLengthInt = SafeIntParse(minLength);

            var strideFilename = strideFolder + inputFilePdbId + ".stride";
            var dsspFilename = dsspFolder + inputFilePdbId + ".dssp";

            var splitDensities = densities.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(decimal.Parse).ToList();

            if (!File.Exists(fullOrTraceAtomsFilename))
            {
                Console.WriteLine("; File not found: " + fullOrTraceAtomsFilename);
            }
            var proteinFileChains = ProteinBioClass.PdbAtomicChains(fullOrTraceAtomsFilename, null, -1, -1, false);

            if (!File.Exists(contactsFilename))
            {
                Console.WriteLine("; File not found: " + contactsFilename);
            }
            var interactions = ProteinBioClass.AtomPair.LoadAtomPairList(contactsFilename);

            var dssp = !string.IsNullOrWhiteSpace(dsspFilename) && File.Exists(dsspFilename) ? DsspFormatFile.LoadDsspFile(dsspFilename) : new List<DsspRecord>();
            var stride = !string.IsNullOrWhiteSpace(strideFilename) && File.Exists(strideFilename) ? StrideFile.Load(strideFilename).Where(a => a is StrideFile.Stride_DetailedSecondaryStructureAssignments).ToList() : new List<StrideFile.Stride_Record>();
            var stride_asg = stride.Select(a => (StrideFile.Stride_DetailedSecondaryStructureAssignments)a).ToList();
            var stride_simple = stride_asg.Select(a => ((a.ProteinChainIdentifier + " ")[0], string.IsNullOrWhiteSpace(a.PdbResidueNumber) ? 0 : SafeIntParse(a.PdbResidueNumber), (a.OneLetterSecondaryStructureCode + " ")[0])).ToList();
            var dssp_simple = dssp.Select(a => ((a.FieldChain.FieldValue + " ")[0], string.IsNullOrWhiteSpace(a.FieldPdbResidueSequenceIndex.FieldValue) ? 0 : SafeIntParse(a.FieldPdbResidueSequenceIndex.FieldValue), (a.FieldSecondaryStructure.FieldValue + "C")[0])).ToList();

            ///////////
            // strand only
            var illegalStrideCodes = new char[] { 'C', 'H' };
            var illegalDsspCodes = new char[] { 'C', 'H' };

            var illegalInteractions = new List<ProteinBioClass.AtomPair>();

            for (var index = 0; index < interactions.Count; index++)
            {
                var inter = interactions[index];

                var strideIndex1 = stride_simple.FindIndex(a => a.Item1 == inter.Atom1.chainID.FieldValue[0] && a.Item2 == SafeIntParse(inter.Atom1.resSeq.FieldValue));
                var strideIndex2 = stride_simple.FindIndex(a => a.Item1 == inter.Atom2.chainID.FieldValue[0] && a.Item2 == SafeIntParse(inter.Atom2.resSeq.FieldValue));

                var dsspIndex1 = dssp_simple.FindIndex(a => a.Item1 == inter.Atom1.chainID.FieldValue[0] && a.Item2 == SafeIntParse(inter.Atom1.resSeq.FieldValue));
                var dsspIndex2 = dssp_simple.FindIndex(a => a.Item1 == inter.Atom2.chainID.FieldValue[0] && a.Item2 == SafeIntParse(inter.Atom2.resSeq.FieldValue));



                if (strideIndex1 >= 0) { var stride1 = stride_simple[strideIndex1].Item3; if (illegalStrideCodes.Contains(stride1)) illegalInteractions.Add(inter); continue; }
                if (strideIndex2 >= 0) { var stride2 = stride_simple[strideIndex2].Item3; if (illegalStrideCodes.Contains(stride2)) illegalInteractions.Add(inter); continue; }
                if (dsspIndex1 >= 0) { var dssp1 = dssp_simple[dsspIndex1].Item3; if (illegalDsspCodes.Contains(dssp1)) illegalInteractions.Add(inter); continue; }
                if (dsspIndex2 >= 0) { var dssp2 = dssp_simple[dsspIndex2].Item3; if (illegalDsspCodes.Contains(dssp2)) illegalInteractions.Add(inter); continue; }
            }

            interactions = interactions.Except(illegalInteractions).ToList();
            ///////////


            var interactionResidueIds =
                interactions.Select(
                    a =>
                        new Tuple<string, string, string, string, string, string>(
                            a.Atom1.chainID.FieldValue, a.Atom1.resSeq.FieldValue, a.Atom1.resName.FieldValue,
                            a.Atom2.chainID.FieldValue, a.Atom2.resSeq.FieldValue, a.Atom2.resName.FieldValue
                        ))
                    .Distinct()
                    .ToList();

            interactions = interactionResidueIds.Select(a => interactions.First(b =>
                b.Atom1.chainID.FieldValue == a.Item1 &&
                b.Atom1.resSeq.FieldValue == a.Item2 &&
                b.Atom1.resName.FieldValue == a.Item3 &&
                b.Atom2.chainID.FieldValue == a.Item4 &&
                b.Atom2.resSeq.FieldValue == a.Item5 &&
                b.Atom2.resName.FieldValue == a.Item6
                )).ToList();

            var maxClusterList = !string.IsNullOrWhiteSpace(maxclusterCsvFilename) && File.Exists(maxclusterCsvFilename) ? File.ReadAllLines(maxclusterCsvFilename).Where(a=>!string.IsNullOrWhiteSpace(a)).Select(a => a.Split(',')).Where(a=>a[2].All(char.IsDigit)).Select(b => new Tuple<string, char, int>(b[0].ToUpperInvariant(), b[1][0], SafeIntParse(b[2]))).ToList() : null;

            // list of the atoms involved in contacts (not all atoms in the pdb file)
            var allContactAtoms = new List<ATOM_Record>();
            allContactAtoms.AddRange(interactions.Select(a => a.Atom1));
            allContactAtoms.AddRange(interactions.Select(a => a.Atom2));
            allContactAtoms = allContactAtoms.Distinct().ToList();

            // list of the chains which have contacts
            var contactChains = allContactAtoms.Select(a => a.chainID.FieldValue).Distinct().OrderBy(a => a).ToList();
            if (!string.IsNullOrWhiteSpace(chainIds) && chainIds != "-")
            {
                contactChains = contactChains.Where(a => chainIds.Contains(a)).ToList();
            }

            if (contactChains == null || contactChains.Count == 0) return;

            var contactsChainToChain = new Dictionary<string, Tuple<List<ATOM_Record>, List<ATOM_Record>>>();

            // get sequences from structures
            var sequenceListFromStructureFile = new List<Sequence>();

            foreach (var chainIdA in contactChains)
            {
                //var s = ProteinBioClass.StructureFileToAaSequence(fullOrTraceAtomsFilename, new[] { chainIdA[0] }, true, '-', '-');
                //sequenceListFromStructureFile.AddRange(s.Select(a => new Sequence(a.Item1, a.Item2)).ToList());
                sequenceListFromStructureFile.AddRange(Sequence.LoadStructureFile(fullOrTraceAtomsFilename, new[] {chainIdA[0]}, true, null, null, '-', '-'));
            }

            // A-->B List of A residues involved, List of B resides involved
            // A-->B  A--C  A--* B-->A B-->C  C-->A C-->B
            foreach (var chainIdA in contactChains)
            {
                var id = chainIdA + "-" + "*";
                var list = interactions.Where(a => a.Atom1.chainID.FieldValue == chainIdA || a.Atom2.chainID.FieldValue == chainIdA).ToList();

                var listA = list.Where(a => a.Atom1.chainID.FieldValue == chainIdA).Select(a => a.Atom1).ToList();
                listA.AddRange(list.Where(a => a.Atom2.chainID.FieldValue == chainIdA).Select(a => a.Atom2).ToList());

                var listB = list.Where(a => a.Atom1.chainID.FieldValue != chainIdA).Select(a => a.Atom1).ToList();
                listB.AddRange(list.Where(a => a.Atom2.chainID.FieldValue != chainIdA).Select(a => a.Atom2).ToList());

                contactsChainToChain.Add(id, new Tuple<List<ATOM_Record>, List<ATOM_Record>>(listA, listB));

                foreach (var chainIdB in contactChains)
                {
                    if (chainIdA == chainIdB) continue;

                    id = chainIdA + "-" + chainIdB;

                    list = interactions.Where(a => (a.Atom1.chainID.FieldValue == chainIdA && a.Atom2.chainID.FieldValue == chainIdB)
                                                       || (a.Atom1.chainID.FieldValue == chainIdB && a.Atom2.chainID.FieldValue == chainIdA)).ToList();

                    listA = list.Where(a => a.Atom1.chainID.FieldValue == chainIdA).Select(a => a.Atom1).ToList();
                    listA.AddRange(list.Where(a => a.Atom2.chainID.FieldValue == chainIdA).Select(a => a.Atom2).ToList());

                    listB = list.Where(a => a.Atom1.chainID.FieldValue == chainIdB).Select(a => a.Atom1).ToList();
                    listB.AddRange(list.Where(a => a.Atom2.chainID.FieldValue == chainIdB).Select(a => a.Atom2).ToList());

                    contactsChainToChain.Add(id, new Tuple<List<ATOM_Record>, List<ATOM_Record>>(listA, listB));

                }
            }

            // make distance matrices
            var dms = new Dictionary<string, List<Tuple<ATOM_Record, ATOM_Record, decimal>>>();

            foreach (var id in contactsChainToChain)
            {
                var chainAtoms = id.Value.Item1;

                var dm = new List<Tuple<ATOM_Record, ATOM_Record, decimal>>();

                for (var x = 0; x < chainAtoms.Count; x++)
                {
                    for (var y = 0; y < chainAtoms.Count; y++)
                    {
                        var atom1 = chainAtoms[x];
                        var atom2 = chainAtoms[y];

                        if (!dm.Any(a => a.Item1 == atom1 && a.Item2 == atom2) &&
                            !dm.Any(a => a.Item1 == atom2 && a.Item2 == atom1))
                        {
                            dm.Add(new Tuple<ATOM_Record, ATOM_Record, decimal>(atom1, atom2,
                                Math.Abs(SafeIntParse(atom1.resSeq.FieldValue) - SafeIntParse(atom2.resSeq.FieldValue))));
                        }
                    }
                }

                dm = dm.OrderBy(a => a.Item3).ThenBy(a => SafeIntParse(a.Item1.resSeq.FieldValue)).ThenBy(a => SafeIntParse(a.Item2.resSeq.FieldValue)).ToList();

                dms.Add(id.Key, dm);
            }


            // cluster
            var chainClusters = new Dictionary<Tuple<string, decimal>, List<List<ATOM_Record>>>();

            foreach (var id in contactsChainToChain)
            {
                var chainAtoms = id.Value.Item1;

                var dm = dms[id.Key];

                foreach (var minDensity in splitDensities)
                {
                    var clusters = new List<List<ATOM_Record>>();


                    var finished = false;
                    while (!finished)
                    {
                        finished = true;
                        foreach (var t in dm)
                        {
                            var atom1 = t.Item1;
                            var atom2 = t.Item2;
                            var distance = t.Item3;

                            var atom1Group = clusters.FirstOrDefault(a => a.Contains(atom1));
                            var atom2Group = clusters.FirstOrDefault(a => a.Contains(atom2));

                            if (atom1Group != null && atom2Group != null && atom1Group == atom2Group) continue;

                            var newgroup = new List<ATOM_Record>();

                            if (atom1Group != null) newgroup.AddRange(atom1Group);
                            else newgroup.Add(atom1);

                            if (atom2Group != null) newgroup.AddRange(atom2Group);
                            else newgroup.Add(atom2);

                            newgroup = newgroup.Distinct().ToList();

                            var min = newgroup.Min(a => SafeIntParse(a.resSeq.FieldValue));
                            var max = newgroup.Max(a => SafeIntParse(a.resSeq.FieldValue));
                            var len = (max - min) + 1;
                            var ifdensity = (decimal)newgroup.Count / (decimal)len;

                            if (ifdensity >= minDensity)
                            {
                                if (atom1Group != null) clusters.Remove(atom1Group);
                                if (atom2Group != null) clusters.Remove(atom2Group);
                                clusters.Add(newgroup);
                                finished = false;
                            }
                        }
                    }
                    clusters = clusters.OrderBy(a => a.Min(b => SafeIntParse(b.resSeq.FieldValue))).ThenBy(a => a.Max(b => SafeIntParse(b.resSeq.FieldValue))).ToList();
                    chainClusters.Add(new Tuple<string, decimal>(id.Key, minDensity), clusters);
                }
            }

            var chainIndexMap = new string[proteinFileChains.ChainList.Count];
            for (var i = 0; i < proteinFileChains.ChainList.Count; i++) chainIndexMap[i] = proteinFileChains.ChainList[i].AtomList.First().chainID.FieldValue;



            var interfaceDataList = new List<InterfaceData>();

            const int minOverlapDistance = -1;

            foreach (var id in contactsChainToChain)
            {
                foreach (var minDensity in splitDensities)
                {
                    var receptorChainIndex = 0;
                    var clusters = chainClusters[new Tuple<string, decimal>(id.Key, minDensity)];

                    var sitePositionClusters = clusters.Select(a => new List<List<ATOM_Record>> { a }).ToList();

                    foreach (var cluster1 in clusters)
                    {
                        var min1 = cluster1.Min(a => SafeIntParse(a.resSeq.FieldValue)) - flankingLength;
                        var max1 = cluster1.Max(a => SafeIntParse(a.resSeq.FieldValue)) + flankingLength;

                        foreach (var cluster2 in clusters)
                        {
                            if (cluster1==cluster2) continue;

                            var min2 = cluster2.Min(a => SafeIntParse(a.resSeq.FieldValue)) - flankingLength;
                            var max2 = cluster2.Max(a => SafeIntParse(a.resSeq.FieldValue)) + flankingLength;

                            var overlap = ProteinBioClass.InterfaceOverlap(min1, max1, min2, max2);

                            if (overlap > minOverlapDistance)
                            {
                                var sitePositionCluster1 = sitePositionClusters.First(a => a.Contains(cluster1));
                                var sitePositionCluster2 = sitePositionClusters.First(a => a.Contains(cluster2));

                                if (sitePositionCluster1 == sitePositionCluster2) continue;

                                sitePositionCluster1.AddRange(sitePositionCluster2);

                                sitePositionClusters.Remove(sitePositionCluster2);
                            }
                        }
                    }

                    foreach (var cluster in clusters)
                    {
                        var interfaceResSeqMin = cluster.Min(a => SafeIntParse(a.resSeq.FieldValue)) - flankingLength;
                        var interfaceResSeqMax = cluster.Max(a => SafeIntParse(a.resSeq.FieldValue)) + flankingLength;
                        var interfaceResSeqLen = (interfaceResSeqMax - interfaceResSeqMin) + 1;

                        if (interfaceResSeqLen < minLengthInt) continue;

                        


                        var interfaceDensity = (decimal)cluster.Count / (decimal)interfaceResSeqLen;

                        receptorChainIndex++;


                        var receptorChainId = id.Key.Substring(0, 1)[0];
                        var ligandChainId = id.Key.Substring(2, 1)[0];


                        var seqDssp = new string(dssp.Where(a => a.FieldChain.FieldValue.ToUpperInvariant() ==""+ receptorChainId &&
                        !string.IsNullOrWhiteSpace(a.FieldPdbResidueSequenceIndex.FieldValue) &&
                        a.FieldPdbResidueSequenceIndex.FieldValue.Any(char.IsDigit))
                        .OrderBy(a=> SafeIntParse(new string(a.FieldPdbResidueSequenceIndex.FieldValue.Where(char.IsDigit).ToArray())))
                        .Select(a=>a.FieldSecondaryStructure.FieldValue.Trim().DefaultIfEmpty('_').First()).ToArray());

                        var seqStride = new string(stride.Where(a => ((StrideFile.Stride_DetailedSecondaryStructureAssignments)a).ProteinChainIdentifier.ToUpperInvariant() == ""+ receptorChainId && 
                        !string.IsNullOrWhiteSpace(((StrideFile.Stride_DetailedSecondaryStructureAssignments)a).PdbResidueNumber) &&
                        ((StrideFile.Stride_DetailedSecondaryStructureAssignments)a).PdbResidueNumber.Any(char.IsDigit))
                        .OrderBy(a=> SafeIntParse(new string(((StrideFile.Stride_DetailedSecondaryStructureAssignments)a).PdbResidueNumber.Where(char.IsDigit).ToArray())))
                        .Select(a=>((StrideFile.Stride_DetailedSecondaryStructureAssignments)a).OneLetterSecondaryStructureCode.Trim().DefaultIfEmpty('_').First()).ToArray());

                        var sequenceFromSequenceFile = sequenceListFromSequenceFile.FirstOrDefault(a => a.IdSplit.PdbId.ToUpperInvariant() == inputFilePdbId.Substring(0, a.IdSplit.PdbId.Length).ToUpperInvariant() && a.IdSplit.ChainId == receptorChainId)?.FullSequence;

                        
                        var sequenceFromStructureFile = sequenceListFromStructureFile.FirstOrDefault(a => (string.IsNullOrWhiteSpace(a.IdSplit.PdbId) || a.IdSplit.PdbId.ToUpperInvariant() == inputFilePdbId.Substring(0, a.IdSplit.PdbId.Length).ToUpperInvariant()) && a.IdSplit.ChainId == receptorChainId)?.FullSequence;




                        var ifaaContacts = new char[interfaceResSeqLen];
                        var ifaaNonContacts = new char[interfaceResSeqLen];
                        var ifaaAll = new char[interfaceResSeqLen];
                        
                        var ifDssp = new char[interfaceResSeqLen];
                        var ifStride = new char[interfaceResSeqLen];
                        //var ifDssp = ProteinBioinformaticsSharedLibrary.Dssp.DsspStructureSequence.LoadDsspStructureSequence(atomsFilename,)

                        var flankBefore = new char[flankingLength];
                        var flankAfter = new char[flankingLength];
                        for (var i = 0; i < flankBefore.Length; i++) flankBefore[i] = '_';
                        for (var i = 0; i < flankAfter.Length; i++) flankAfter[i] = '_';


                        for (var i = 0; i < ifaaContacts.Length; i++) ifaaContacts[i] = '_';
                        for (var i = 0; i < ifaaContacts.Length; i++) ifaaNonContacts[i] = '_';
                        for (var i = 0; i < ifaaContacts.Length; i++) ifaaAll[i] = '_';
                        
                        for (var i = 0; i < ifaaContacts.Length; i++) ifDssp[i] = '_';
                        for (var i = 0; i < ifaaContacts.Length; i++) ifStride[i] = '_';



                        var atomsChainIndex = Array.IndexOf(chainIndexMap, id.Key.Substring(0, id.Key.IndexOf("-")));

                        var structureToSequenceAlignmentResult = ProteinBioClass.StructureToSequenceAlignment.Align(proteinFileChains.ChainList[atomsChainIndex].AtomList, sequenceFromSequenceFile, sequenceFromStructureFile);



                        foreach (var atom in proteinFileChains.ChainList[atomsChainIndex].AtomList)
                        {
                            var resId = SafeIntParse(atom.resSeq.FieldValue);

                            if (resId >= interfaceResSeqMin && resId <= interfaceResSeqMax)
                            {
                                var array_index = resId - interfaceResSeqMin;

                                var resDssp = dssp.FirstOrDefault(a => a.FieldChain.FieldValue.ToUpperInvariant() == atom.chainID.FieldValue.ToUpperInvariant() && 
                                !string.IsNullOrWhiteSpace(a.FieldPdbResidueSequenceIndex.FieldValue) && a.FieldPdbResidueSequenceIndex.FieldValue.Any(char.IsDigit) &&
                                SafeIntParse(new string(a.FieldPdbResidueSequenceIndex.FieldValue.Where(char.IsDigit).ToArray())) == resId);

                                if (resDssp != null && ifDssp[array_index] == '_') ifDssp[array_index] = resDssp.FieldSecondaryStructure.FieldValue.DefaultIfEmpty('C').First();



                                var resStride = stride.FirstOrDefault(a => ((StrideFile.Stride_DetailedSecondaryStructureAssignments)a).ProteinChainIdentifier.ToUpperInvariant() == atom.chainID.FieldValue.ToUpperInvariant() &&
                                !string.IsNullOrWhiteSpace(((StrideFile.Stride_DetailedSecondaryStructureAssignments)a).PdbResidueNumber) &&
                                ((StrideFile.Stride_DetailedSecondaryStructureAssignments)a).PdbResidueNumber.Any(char.IsDigit) &&
                                SafeIntParse(new string(((StrideFile.Stride_DetailedSecondaryStructureAssignments)a).PdbResidueNumber.Where(char.IsDigit).ToArray())) == resId);

                                if (resStride != null && ifStride[array_index] == '_') ifStride[array_index] = ((StrideFile.Stride_DetailedSecondaryStructureAssignments)resStride).OneLetterSecondaryStructureCode.DefaultIfEmpty('C').First();



                                if (ifaaAll[array_index] == '_') ifaaAll[array_index] = AminoAcidConversions.AminoAcidNumberToCode1L(AminoAcidConversions.AminoAcidNameToNumber(atom.resName.FieldValue)).First();

                                

                                if (!cluster.Any(atom.Equals))
                                {
                                    if (ifaaNonContacts[array_index] == '_') ifaaNonContacts[array_index] = AminoAcidConversions.AminoAcidNumberToCode1L(AminoAcidConversions.AminoAcidNameToNumber(atom.resName.FieldValue)).First();
                                }
                            }
                            
                        }

                        var superMinIndex = structureToSequenceAlignmentResult == null ? -1 : structureToSequenceAlignmentResult.AlignmentMap.Where(a => a >= structureToSequenceAlignmentResult.ChainResSeqMin && a <= structureToSequenceAlignmentResult.ChainResSeqMax).ToList().FindIndex(a => a >= interfaceResSeqMin && a <= interfaceResSeqMax);
                        var superMaxIndex = structureToSequenceAlignmentResult == null ? -1 : structureToSequenceAlignmentResult.AlignmentMap.Where(a => a >= structureToSequenceAlignmentResult.ChainResSeqMin && a <= structureToSequenceAlignmentResult.ChainResSeqMax).ToList().FindLastIndex(a => a >= interfaceResSeqMin && a <= interfaceResSeqMax);
                        var superLen = (superMaxIndex - superMinIndex) + 1;
                        var sequenceSuper = structureToSequenceAlignmentResult == null ? "": new string(structureToSequenceAlignmentResult.FastaSequenceAligned.Where((a, i) => structureToSequenceAlignmentResult.AlignmentMap[i] >= structureToSequenceAlignmentResult.ChainResSeqMin && structureToSequenceAlignmentResult.AlignmentMap[i] <= structureToSequenceAlignmentResult.ChainResSeqMax).ToArray());
                        var interfaceSuper = structureToSequenceAlignmentResult == null ? "" : new string(structureToSequenceAlignmentResult.FastaSequenceAligned.Where((a, i) => structureToSequenceAlignmentResult.AlignmentMap[i] >= interfaceResSeqMin && structureToSequenceAlignmentResult.AlignmentMap[i] <= interfaceResSeqMax).ToArray());

                        foreach (var atom in cluster)
                        {
                            var resId = SafeIntParse(atom.resSeq.FieldValue);
                            var array_index = resId - interfaceResSeqMin;
                            ifaaContacts[array_index] = AminoAcidConversions.AminoAcidNumberToCode1L(AminoAcidConversions.AminoAcidNameToNumber(atom.resName.FieldValue)).First();
                        }

                        var uniqueIdentifier = UniqueInterfaceId(inputFilePdbId, id.Key, interfaceResSeqMin, interfaceResSeqMax, minDensity);

                        var maxCluster = maxClusterList?.FirstOrDefault(a => a.Item1.Substring(0,4).ToUpperInvariant() == inputFilePdbId.Substring(0,4).ToUpperInvariant() && a.Item2 == receptorChainId);

                        var data = new InterfaceData(
                            id.Key + "-" + (sitePositionClusters.FindIndex(a=>a.Contains(cluster)) + 1),
                            maxCluster!=null? maxCluster.Item3:-1,
                            uniqueIdentifier,
                            inputFilePdbId,
                            receptorChainId,
                            ligandChainId,
                            receptorChainIndex,
                            interfaceResSeqMin,
                            interfaceResSeqMax,
                            interfaceResSeqLen,
                            superMinIndex,
                            superMaxIndex,
                            superLen,
                            ifaaContacts.Count(a => a != '_'),
                            ifaaContacts.Count(a => a == '_'),
                            interfaceDensity,
                            minDensity,
                            String.Join("", ifDssp),
                            String.Join("", ifStride),
                            String.Join("", ifaaContacts),
                            String.Join("", ifaaNonContacts),

                            String.Join("", ifaaAll),
                            interfaceSuper,

                            sequenceFromSequenceFile,
                            sequenceFromStructureFile,
                            structureToSequenceAlignmentResult == null ? "" : structureToSequenceAlignmentResult.FastaSequenceAligned,
                            structureToSequenceAlignmentResult == null ? "" : structureToSequenceAlignmentResult.PdbSequenceAligned,
                            sequenceSuper,
                            structureToSequenceAlignmentResult == null ? "" : string.Join("|", structureToSequenceAlignmentResult.StructureMissingResidues),
                            structureToSequenceAlignmentResult == null ? "" : string.Join("|", structureToSequenceAlignmentResult.StructureMissingResiduesAligned),
                            seqDssp,
                            seqStride
                            );

                        interfaceDataList.Add(data);
                    }
                }
            }

            interfaceDataList = interfaceDataList.OrderBy(a => a.UniqueIdentifier).ToList();

            var receptorToLigandInterfaceList = new List<InterfaceInterfaceData>();

            // foreach interface, find start and end position, check whether residues within that block are in contact with all opposite interfaces
            foreach (var receptorInterface in interfaceDataList)
            {

                var ligandInterfaceList = new List<InterfaceData>();

                if (receptorInterface.LigandChainId != '*')
                {

                    // find list of interactions involving the receptor chain and the ligand chain(s)
                    var interfaceInteractions =
                         interactions.Where(
                        atomPair => (
                                            (atomPair.Atom1.chainID.FieldValue[0] == receptorInterface.ReceptorChainId && WithinRange(SafeIntParse(atomPair.Atom1.resSeq.FieldValue), receptorInterface.InterfaceResSeqStart, receptorInterface.InterfaceResSeqEnd))
                                            ||
                                            (atomPair.Atom2.chainID.FieldValue[0] == receptorInterface.ReceptorChainId && WithinRange(SafeIntParse(atomPair.Atom2.resSeq.FieldValue), receptorInterface.InterfaceResSeqStart, receptorInterface.InterfaceResSeqEnd))
                                        )
                                        &&
                                        (
                                            (atomPair.Atom1.chainID.FieldValue[0] == receptorInterface.LigandChainId)
                                            ||
                                            (atomPair.Atom2.chainID.FieldValue[0] == receptorInterface.LigandChainId)
                                        )
                                  )
                        .ToList();

                    // get the atoms for the ligand chain id, and then get min and max residue sequence index
                    var ligandInterfaceAtoms = new List<ATOM_Record>();
                    ligandInterfaceAtoms.AddRange(interfaceInteractions.Where(atomPair => receptorInterface.LigandChainId == atomPair.Atom1.chainID.FieldValue[0]).Select(b => b.Atom1).ToList());
                    ligandInterfaceAtoms.AddRange(interfaceInteractions.Where(atomPair => receptorInterface.LigandChainId == atomPair.Atom2.chainID.FieldValue[0]).Select(b => b.Atom2).ToList());
                    ligandInterfaceAtoms = ligandInterfaceAtoms.Distinct().ToList();

                    var ligandInterfaceResSeqMin = ligandInterfaceAtoms.Min(a => SafeIntParse(a.resSeq.FieldValue));
                    var ligandInterfaceResSeqMax = ligandInterfaceAtoms.Max(a => SafeIntParse(a.resSeq.FieldValue));

                    // swap receptor and ligand to find the ligands
                    // find interfaces that have an residue sequence index between ligandInterfaceResSeqMin and ligandInterfaceResSeqMax

                    ligandInterfaceList.AddRange(interfaceDataList.Where(interfaceData =>
                                                                           (interfaceData.ReceptorChainId == receptorInterface.LigandChainId)
                                                                        && (interfaceData.LigandChainId == receptorInterface.ReceptorChainId)
                                                                        && ProteinBioClass.InterfaceOverlap(ligandInterfaceResSeqMin, ligandInterfaceResSeqMax, interfaceData.InterfaceResSeqStart, interfaceData.InterfaceResSeqEnd) > 0
                                                                        //(WithinRange(ligandInterfaceResSeqMin, interfaceData.InterfaceStart, interfaceData.InterfaceEnd)
                                                                        //|| WithinRange(ligandInterfaceResSeqMax, interfaceData.InterfaceStart, interfaceData.InterfaceEnd))
                                                                        ).ToList());

                    //&& ((WithinRange(interfaceData.InterfaceStart, ligandInterfaceResSeqMin, ligandInterfaceResSeqMax))
                    //|| (WithinRange(interfaceData.InterfaceEnd, ligandInterfaceResSeqMin, ligandInterfaceResSeqMax)))

                }
                else if (receptorInterface.LigandChainId == '*')
                {
                    var x = new InterfaceData {LigandChainId = '*'};

                    ligandInterfaceList.Add(x);//"", "", "", '*', ' ', -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }


                foreach (var ligandInterface in ligandInterfaceList)
                {
                    //if (receptorInterface == ligandInterface) continue;

                    receptorToLigandInterfaceList.Add(new InterfaceInterfaceData(
                              -1,
                              -1,
                              -1,
                              -1,
                              -1,
                              -1,
                              -1,
                              receptorInterface.MaxCluster,
                              receptorInterface.ReceptorInterfaceSitePositionLigandSpecificClusterIndex,
                              receptorInterface.UniqueIdentifier,
                              receptorInterface.PdbId,
                              receptorInterface.ReceptorChainId,
                              receptorInterface.ReceptorInterfaceIndex,
                              receptorInterface.InterfaceResSeqStart,
                              receptorInterface.InterfaceResSeqEnd,
                              receptorInterface.InterfaceResSeqLength,
                              receptorInterface.InterfaceSuperStart,
                              receptorInterface.InterfaceSuperEnd,
                              receptorInterface.InterfaceSuperLength,
                              receptorInterface.InterfaceTotalContacts,
                              receptorInterface.InterfaceTotalNonContacts,
                              receptorInterface.InterfaceDensity,
                              receptorInterface.InterfaceMininumDensity,
                              receptorInterface.InterfaceAllAminoAcids,
                              receptorInterface.InterfaceAllAminoAcidsSuper,
                              receptorInterface.InterfaceContactAminoAcids,
                              receptorInterface.InterfaceNoContactAminoAcids,
                              receptorInterface.InterfaceDsspSecondaryStructure,
                              receptorInterface.InterfaceStrideSecondaryStructure,
                              receptorInterface.SequenceFromSequenceFile,
                              receptorInterface.SequenceFromStructureFile,
                              receptorInterface.SequenceFromSequenceFileAligned,
                              receptorInterface.SequenceFromStructureFileAligned,
                              receptorInterface.SequenceSuper,
                              receptorInterface.SequenceMissingResidues,
                              receptorInterface.SequenceSuperMissingResidues,
                              receptorInterface.SequenceDssp,
                              receptorInterface.SequenceStride,

                              -1,
                              -1,
                              -1,
                              -1,
                              -1,
                              -1,
                              -1,
                              ligandInterface.MaxCluster,
                              ligandInterface.ReceptorInterfaceSitePositionLigandSpecificClusterIndex,
                              ligandInterface.UniqueIdentifier,
                              ligandInterface.PdbId,
                              ligandInterface.ReceptorChainId,
                              ligandInterface.ReceptorInterfaceIndex,
                              ligandInterface.InterfaceResSeqStart,
                              ligandInterface.InterfaceResSeqEnd,
                              ligandInterface.InterfaceResSeqLength,
                              ligandInterface.InterfaceSuperStart,
                              ligandInterface.InterfaceSuperEnd,
                              ligandInterface.InterfaceSuperLength,
                              ligandInterface.InterfaceTotalContacts,
                              ligandInterface.InterfaceTotalNonContacts,
                              ligandInterface.InterfaceDensity,
                              ligandInterface.InterfaceMininumDensity,
                              ligandInterface.InterfaceAllAminoAcids,
                              ligandInterface.InterfaceAllAminoAcidsSuper,
                              ligandInterface.InterfaceContactAminoAcids,
                              ligandInterface.InterfaceNoContactAminoAcids,
                              ligandInterface.InterfaceDsspSecondaryStructure,
                              ligandInterface.InterfaceStrideSecondaryStructure,
                              ligandInterface.SequenceFromSequenceFile,
                              ligandInterface.SequenceFromStructureFile,
                              ligandInterface.SequenceFromSequenceFileAligned,
                              ligandInterface.SequenceFromStructureFileAligned,
                              ligandInterface.SequenceSuper,
                              ligandInterface.SequenceMissingResidues,
                              ligandInterface.SequenceSuperMissingResidues,
                              ligandInterface.SequenceDssp,
                              ligandInterface.SequenceStride
                          ));
                }

            }

            receptorToLigandInterfaceList =
                receptorToLigandInterfaceList.OrderBy(a => a.ReceptorUniqueIdentifier)
                    .ThenBy(a => a.LigandUniqueIdentifier)
                    .ToList();

            var test = new List<string>();

            foreach (var t in receptorToLigandInterfaceList)
            {
                test.Add("PDB: " + t.ReceptorPdbId + ":" + t.ReceptorChainId);
                test.Add("");
                test.Add("Fasta file and PDB file");
                test.Add("Structure:\t" + t.ReceptorSequenceFromStructureFile.Trim('-'));
                test.Add("_Sequence:\t" + (t.ReceptorSequenceFromSequenceFile == null ? "" : t.ReceptorSequenceFromSequenceFile.Trim('-')));
                test.Add("");
                test.Add("Full sequence alignment");
                test.Add("Structure:\t" + t.ReceptorSequenceFromStructureFileAligned.Trim('-'));
                test.Add("_Sequence:\t" + t.ReceptorSequenceFromSequenceFileAligned.Trim('-'));
                test.Add("____Super:\t" + t.ReceptorSequenceSuper.Trim('-'));
                test.Add("");
                test.Add("Interface");
                test.Add("Structure:\t" + t.ReceptorInterfaceAllAminoAcids);//.Trim('-'));
                test.Add("____Super:\t" + t.ReceptorInterfaceAllAminoAcidsSuper);//.Trim('-'));
                test.Add("__SuperNG:\t" + t.ReceptorInterfaceAllAminoAcidsSuper);//.Trim('-'));
                test.Add("");
                test.Add("_Missing:\t" + t.ReceptorSequenceMissingResidues);
                test.Add("MissingS:\t" +t.ReceptorSequenceSuperMissingResidues);
                test.Add("");
            }

            File.WriteAllLines(outputFilename2 + ".log", test);

            var lines = receptorToLigandInterfaceList.Select(a => a.ToString()).ToList();
            //lines = lines.OrderBy(a => a).ToList();
            lines.Insert(0, InterfaceInterfaceData.Header());

            if (!string.IsNullOrWhiteSpace(outputFilename2))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFilename2));
                File.WriteAllLines(outputFilename2, lines);
            }
            else
            {
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }

}
