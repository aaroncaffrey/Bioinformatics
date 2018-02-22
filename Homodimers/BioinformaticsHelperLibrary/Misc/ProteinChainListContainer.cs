using System.Collections.Generic;

namespace BioinformaticsHelperLibrary.Misc
{
    public class ProteinChainListContainer
    {
        /// <summary>
        ///     ChainList: A list of chains, containing a container class with a list of atoms
        /// </summary>
        public List<ProteinAtomListContainer> ChainList = new List<ProteinAtomListContainer>();
    }
}