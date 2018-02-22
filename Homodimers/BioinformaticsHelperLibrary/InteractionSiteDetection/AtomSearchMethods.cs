using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.Misc;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    public class AtomSearchMethods
    {
        public static ATOM_Record FindAtomInsideSingularInteractionsChain(ProteinChainListContainer singularAaToAaInteractions, int chainIndex, int residueSequenceIndex)
        {
            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(singularAaToAaInteractions))
            {
                return null;
            }

            if (ParameterValidation.IsChainIndexInvalid(chainIndex))
            {
                throw new ArgumentOutOfRangeException(nameof(chainIndex));
            }

            if (ParameterValidation.IsResidueSequenceIndexInvalid(residueSequenceIndex, true))
            {
                throw new ArgumentOutOfRangeException(nameof(residueSequenceIndex));
            }

            // Loop through atoms in specified chain to find atom with given residue sequence index
            for (int atomIndex = 0; atomIndex < singularAaToAaInteractions.ChainList[chainIndex].AtomList.Count; atomIndex++)
            {
                ATOM_Record atom = singularAaToAaInteractions.ChainList[chainIndex].AtomList[atomIndex];

                if (ProteinDataBankFileOperations.NullableTryParseInt32(atom.resSeq.FieldValue) == residueSequenceIndex)
                {
                    return atom;
                }
            }

            return null;
        }


        public static ATOM_Record FindAtomInsidePdbFileChain(ProteinChainListContainer pdbFileChains, int chainIndex, int residueSequenceIndex)
        {
            if (ParameterValidation.IsProteinChainListContainerNullOrEmpty(pdbFileChains))
            {
                return null;
            }

            if (ParameterValidation.IsChainIndexInvalid(chainIndex))
            {
                throw new ArgumentOutOfRangeException(nameof(chainIndex));
            }

            if (ParameterValidation.IsResidueSequenceIndexInvalid(residueSequenceIndex, true))
            {
                throw new ArgumentOutOfRangeException(nameof(residueSequenceIndex));
            }

            for (int memberIndex = 0; memberIndex < pdbFileChains.ChainList[chainIndex].AtomList.Count; memberIndex++)
            {
                ATOM_Record atom = pdbFileChains.ChainList[chainIndex].AtomList[memberIndex];

                if (ProteinDataBankFileOperations.NullableTryParseInt32(atom.resSeq.FieldValue) == residueSequenceIndex)
                {
                    return atom;
                }
            }

            return null;
        }

        public static List<ATOM_Record> FindAtomInteractingWithOtherProteinInterfaces(ATOM_Record atom, InteractionBetweenProteinInterfacesListContainer interactionBetweenProteinInterfacesContainer, FindAtomInteractingWithAnotherProteinInterfaceOptions findAtomInteractingWithAnotherProteinInterfaceOptions)
        {
            if (ParameterValidation.IsAtomNullOrEmpty(atom))
            {
                throw new ArgumentNullException(nameof(atom));
            }

            if (ParameterValidation.IsInteractionBetweenProteinInterfacesListContainerNullOrEmpty(interactionBetweenProteinInterfacesContainer))
            {
                ////////Console.WriteLine("empty");
                return null;
            }

            List<InteractionBetweenProteinInterfaces> searchList = null;

            if (findAtomInteractingWithAnotherProteinInterfaceOptions == FindAtomInteractingWithAnotherProteinInterfaceOptions.FindAtomsInteractingWithOtherProteinInterfaces)
            {
                searchList = interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList;
            }
            else if (findAtomInteractingWithAnotherProteinInterfaceOptions == FindAtomInteractingWithAnotherProteinInterfaceOptions.FindAtomsInteractingWithNonProteinInterfaces)
            {
                searchList = interactionBetweenProteinInterfacesContainer.InteractionBetweenNonProteinInterfacesList;
            }
            else
            {
                throw new NotImplementedException();
            }

            if (searchList == null)
            {
                ////////Console.WriteLine("returning null");
                return null;
            }

            var result = new List<ATOM_Record>();

            foreach (InteractionBetweenProteinInterfaces interaction in searchList)
            {
                if (interaction.Atom1.Atom == atom && interaction.Atom2.Atom != atom)
                {
                    //return interaction.Atom1.Atom;
                    result.Add(interaction.Atom2.Atom);
                }

                if (interaction.Atom1.Atom != atom && interaction.Atom2.Atom == atom)
                {
                    //return interaction.Atom2.Atom;
                    result.Add(interaction.Atom1.Atom);
                }
            }

            //if (result.Count == 0)
            //{
            //    //////Console.WriteLine("returning empty list");
            //}

            return result;
        }
    }
}
