//-----------------------------------------------------------------------
// <copyright file="AtomPair.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using BioinformaticsHelperLibrary.ProteinDataBankFormat;
using BioinformaticsHelperLibrary.ProteinDataBankFormat.RecordTypes;

namespace BioinformaticsHelperLibrary.Misc
{
    /// <summary>
    ///     This class is used to store two related ATOM records.  This is useful for storing two interacting atoms together.
    /// </summary>
    [Serializable]
    public class AtomPair
    {
        /// <summary>
        ///     Atom1 property.
        /// </summary>
        public ATOM_Record Atom1 = null;

        /// <summary>
        ///     Atom2 property.
        /// </summary>
        public ATOM_Record Atom2 = null;


        public FullProteinInterfaceId Atom1FullProteinInterfaceId = new FullProteinInterfaceId();

        public FullProteinInterfaceId Atom2FullProteinInterfaceId = new FullProteinInterfaceId();


        /// <summary>
        ///     The calculated distance between Atom1 and Atom2.
        /// </summary>
        public decimal Distance;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomPair" /> class.
        /// </summary>
        /// <param name="atom1">The first atom.</param>
        /// <param name="atom2">The second atom.</param>
        /// <param name="distance">
        ///     The distance between atom1 and atom2 (presumed already calculated as it is expensive to
        ///     calculate 3d distance).
        /// </param>
        public AtomPair(ATOM_Record atom1, ATOM_Record atom2, decimal distance = 0)
        {
            //PdbIdAtom1 = null;
            Atom1 = atom1;
            //PdbIdAtom2 = null;
            Atom2 = atom2;
            Distance = distance;
        }

        public AtomPair(string pdbIdAtom1, ATOM_Record atom1, string pdbIdAtom2, ATOM_Record atom2, decimal distance = 0)
        {
            Atom1 = atom1;
            Atom2 = atom2;
            Distance = distance;

            Atom1FullProteinInterfaceId.ProteinId = pdbIdAtom1;

            Atom2FullProteinInterfaceId.ProteinId = pdbIdAtom2;
        }


        public AtomPair(string pdbIdAtom1, ATOM_Record atom1, int chainIndexAtom1, string pdbIdAtom2, int chainIndexAtom2, ATOM_Record atom2, decimal distance = 0)
        {
            Atom1 = atom1;
            Atom2 = atom2;
            Distance = distance;

            Atom1FullProteinInterfaceId.ProteinId = pdbIdAtom1;
            Atom1FullProteinInterfaceId.ChainId = chainIndexAtom1;

            Atom2FullProteinInterfaceId.ProteinId = pdbIdAtom2;
            Atom2FullProteinInterfaceId.ChainId = chainIndexAtom2;
        }
    }
}