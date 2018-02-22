using System;
using System.Linq;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class VectorProteinInterfacePart
    {
        public FullProteinInterfaceId FullProteinInterfaceId = new FullProteinInterfaceId();
        
        public string InteractionAminoAcids1L = "";
        public bool[] InteractionFlagBools = null;

        public string InteractionNonProteinInterfaceAminoAcids1L = "";
        public bool InteractionToNonProteinInterface = false;
        
        public int ResidueId = -1;

        public string SourceAminoAcid1L = "";
        public string SourceAminoAcid3L = "";

        public VectorProteinInterfacePart()
        {

        }

        public VectorProteinInterfacePart(int interactionVectorSize) : this()
        {
            InteractionFlagBools = new bool[interactionVectorSize];
        }

        public VectorProteinInterfacePart(FullProteinInterfaceId fullProteinInterfaceId, string interactionAminoAcids, bool[] interactionBools, string interactionNonProteinInterfaceAminoAcids, bool interactionToNonProteinInterface,  int residueId, string sourceAminoAcid1L, string sourceAminoAcid3L) : this()
        {
            if (fullProteinInterfaceId == null) throw new ArgumentNullException(nameof(fullProteinInterfaceId));
            if (interactionBools == null) throw new ArgumentNullException(nameof(interactionBools));
            if (interactionNonProteinInterfaceAminoAcids == null) throw new ArgumentNullException(nameof(interactionNonProteinInterfaceAminoAcids));
            if (sourceAminoAcid1L == null) throw new ArgumentNullException(nameof(sourceAminoAcid1L));
            if (sourceAminoAcid3L == null) throw new ArgumentNullException(nameof(sourceAminoAcid3L));

            FullProteinInterfaceId = new FullProteinInterfaceId(fullProteinInterfaceId);
            
            InteractionAminoAcids1L = interactionAminoAcids;
            InteractionFlagBools = interactionBools.ToArray();
            InteractionNonProteinInterfaceAminoAcids1L = interactionNonProteinInterfaceAminoAcids;
            InteractionToNonProteinInterface = interactionToNonProteinInterface;
            
            ResidueId = residueId;

            SourceAminoAcid1L = sourceAminoAcid1L;

            SourceAminoAcid3L = sourceAminoAcid3L;
        }

        public VectorProteinInterfacePart(VectorProteinInterfacePart vectorProteinInterfacePart)
            : this(
            vectorProteinInterfacePart.FullProteinInterfaceId,
            vectorProteinInterfacePart.InteractionAminoAcids1L,
            vectorProteinInterfacePart.InteractionFlagBools,
            vectorProteinInterfacePart.InteractionNonProteinInterfaceAminoAcids1L,
            vectorProteinInterfacePart.InteractionToNonProteinInterface,
            vectorProteinInterfacePart.ResidueId,
            vectorProteinInterfacePart.SourceAminoAcid1L,
            vectorProteinInterfacePart.SourceAminoAcid3L
            )
        {
            if (vectorProteinInterfacePart == null) throw new ArgumentNullException(nameof(vectorProteinInterfacePart));
        }

        public bool[] GetOriginalVectorCopy()
        {
            var result = new bool[InteractionFlagBools.Length];
            Array.Copy(InteractionFlagBools, result, InteractionFlagBools.Length);
            return result;
        }
    }
}
