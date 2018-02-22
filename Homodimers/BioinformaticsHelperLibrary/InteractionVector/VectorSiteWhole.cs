using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;


namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class VectorProteinInterfaceWhole
    {
        public FullProteinInterfaceId FullProteinInterfaceId = null;

        public string SecondaryStructure;

        public int ProteinInterfaceLength = -1;

        public int FirstResidueSequenceIndex = -1;
        public int LastResidueSequenceIndex = -1;

        public long FullSequenceLength = -1;

        public List<VectorProteinInterfacePart> VectorProteinInterfacePartList = new List<VectorProteinInterfacePart>();

        public bool ReversedInteractions = false;
        public bool ReversedSequence = false;

        public VectorProteinInterfaceWhole()
        {
            
        }

        public string VectorString(int vectorType)
        {
            if (vectorType == 0)
            {
                return string.Join("", InteractionBools().Select(Convert.ToInt32)) + "+" + (VectorProteinInterfacePartList.Count(a => a.InteractionToNonProteinInterface) > 0 ? 1 : 0);
            }
            else if (vectorType == 1)
            {
                return string.Join(" ", VectorProteinInterfacePartList.Select(a => string.Join("", a.InteractionFlagBools.Select(Convert.ToInt32)) + "+" + Convert.ToInt32(a.InteractionToNonProteinInterface)).ToList());
            }
            else if (vectorType == 2)
            {
                return string.Join("", InteractionBools().Select(Convert.ToInt32));
            }
            else if (vectorType == 3)
            {
                return string.Join(" ", VectorProteinInterfacePartList.Select(a => string.Join("", a.InteractionFlagBools.Select(Convert.ToInt32))).ToList());
            }
            else
            {
                //throw new ArgumentOutOfRangeException(nameof(vectorType");
                return null;
            }
        }

        public static string VectorStringDescription(int vectorType)
        {
            if (vectorType == 0)
            {
                return "Short";
            }
            else if (vectorType == 1)
            {
                return "Long";
            }
            else if (vectorType == 2)
            {
                return "Short Ignore Outside";
            }
            else if (vectorType == 3)
            {
                return "Long Ignore Outside";
            }
            else if (vectorType == 4)
            {
                return "Secondary Structure";
            }
            else if (vectorType == 5)
            {
                return "ProteinInterface Length";
            }
            else
            {
                //throw new ArgumentOutOfRangeException(nameof(vectorType");
                return null;
            }
        }

        /// <summary>
        /// Get the short Interaction Vector for the whole proteinInterface (does not contain interaction positions in oppoproteinInterface proteinInterface)
        /// </summary>
        /// <returns></returns>
        public bool[] InteractionBools()
        {
            var result = new bool[VectorProteinInterfacePartList.Count];

            for (int index = 0; index < VectorProteinInterfacePartList.Count; index++)
            {
                var vectorProteinInterfacePart = VectorProteinInterfacePartList[index];
                result[index] = vectorProteinInterfacePart.InteractionFlagBools.Count(boolValue => boolValue) > 0 || vectorProteinInterfacePart.InteractionToNonProteinInterface;
            }

            return result;
        }

        public enum ProteinInterfaceReadOption
        {
            WholeProteinInterface,
            Interactions,
            NonInteractions    
        }

        public string ProteinInterfaceAminoAcids1L(ProteinInterfaceReadOption proteinInterfaceReadOption = ProteinInterfaceReadOption.WholeProteinInterface)
        {
            var result = "";

            var interactionBools = InteractionBools();

            for (int index = 0; index < VectorProteinInterfacePartList.Count; index++)
            {
                var vectorProteinInterfacePart = VectorProteinInterfacePartList[index];
                var isInteraction = interactionBools[index];

                if ((proteinInterfaceReadOption == ProteinInterfaceReadOption.WholeProteinInterface) ||
                    (proteinInterfaceReadOption == ProteinInterfaceReadOption.Interactions && isInteraction) ||
                    (proteinInterfaceReadOption == ProteinInterfaceReadOption.NonInteractions && !isInteraction))
                {
                    result += vectorProteinInterfacePart.SourceAminoAcid1L;
                }
            }

            return result;
        }

        public string ProteinInterfaceAminoAcids3L(ProteinInterfaceReadOption proteinInterfaceReadOption = ProteinInterfaceReadOption.WholeProteinInterface)
        {
            var result = "";

            var interactionBools = InteractionBools();

            for (int index = 0; index < VectorProteinInterfacePartList.Count; index++)
            {
                var vectorProteinInterfacePart = VectorProteinInterfacePartList[index];
                var isInteraction = interactionBools[index];

                if ((proteinInterfaceReadOption == ProteinInterfaceReadOption.WholeProteinInterface) ||
                    (proteinInterfaceReadOption == ProteinInterfaceReadOption.Interactions && isInteraction) ||
                    (proteinInterfaceReadOption == ProteinInterfaceReadOption.NonInteractions && !isInteraction))
                {
                    result += vectorProteinInterfacePart.SourceAminoAcid3L;
                }
            }

            return result;
        }

        public string FastaFormatProteinInterface()//(FullProteinInterfaceId fullProteinInterfaceId, string proteinInterfaceSequence)
        {
            return ">" + FullProteinInterfaceId.ProteinId + ":" + FullProteinInterfaceId.ChainId + ":" + FullProteinInterfaceId.ProteinInterfaceId + ":" + FullProteinInterfaceId.ProteinInterfaceStartIndex + ":" + FullProteinInterfaceId.ProteinInterfaceEndIndex + "|PDBID|CHAIN|SITE|START|END|SEQUENCE" + Environment.NewLine +
                   string.Join("", VectorProteinInterfacePartList.Select(a => a.SourceAminoAcid1L).ToList());// + Environment.NewLine;
        }
    }
}
