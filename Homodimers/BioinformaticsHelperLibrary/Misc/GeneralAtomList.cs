using System.Collections.Generic;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.Misc
{
    public class GeneralAtomList
    {
        /// <summary>
        ///     AtomList: A list of atoms which are part of a chain, proteinInterface, group or cluster
        /// </summary>
        public List<ATOM_Record> AtomList = new List<ATOM_Record>();
    }
}