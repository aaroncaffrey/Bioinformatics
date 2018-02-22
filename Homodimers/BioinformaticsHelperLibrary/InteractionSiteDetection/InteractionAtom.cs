using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public class InteractionAtom
    {
        public ATOM_Record Atom = null;
        public FullProteinInterfaceId FullProteinInterfaceId = new FullProteinInterfaceId();

        public InteractionAtom()
        {
            
        }

        public InteractionAtom(ATOM_Record atom)
        {
            Atom = atom;
        }

        public InteractionAtom(ATOM_Record atom, FullProteinInterfaceId fullProteinInterfaceId)
        {
            Atom = atom;
            FullProteinInterfaceId = fullProteinInterfaceId;
        }
    }
}