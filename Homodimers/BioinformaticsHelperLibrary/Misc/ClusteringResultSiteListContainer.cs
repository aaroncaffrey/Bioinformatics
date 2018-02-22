//-----------------------------------------------------------------------
// <copyright file="____________.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace BioinformaticsHelperLibrary.Misc
{
    public class ClusteringResultProteinInterfaceListContainer
    {
        /// <summary>
        ///     ProteinInterfaceList: A list of chains, containing a list of proteinInterfaces, containing a list of atoms.
        /// </summary>
        // public List<List<List<ATOM_Record>>> ProteinInterfaceList = new List<List<List<ATOM_Record>>>();
        public List<Chain> ChainList = new List<Chain>();

        public class Chain
        {
            public List<ProteinInterface> ProteinInterfaceList = new List<ProteinInterface>();

            public class ProteinInterface : GeneralAtomList
            {
                //public List<ATOM_Record> AtomList = new List<ATOM_Record>();
            }
        }
    }
}