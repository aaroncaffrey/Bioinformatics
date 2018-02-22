//-----------------------------------------------------------------------
// <copyright file="____________.cs" company="Aaron Caffrey">
//     Copyright (c) 2013-2014 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace BioinformaticsHelperLibrary.Misc
{
    public class ClusteringFullResultListContainer
    {
        public List<Chain> ChainList = new List<Chain>();

        /// <summary>
        ///     ResultList: A list of chains, containing a list of clustering process stages, containing a list of clustered
        ///     groups, containing a list of atoms in the group
        /// </summary>
        //public List<List<List<List<int>>>> ResultList = new List<List<List<List<int>>>>();
        public class Chain
        {
            public List<Stage> StageList = new List<Stage>();

            public class Stage
            {
                public List<Cluster> ClusterList = new List<Cluster>();

                public class Cluster : GeneralAtomIndexList
                {
                    //public List<ATOM_Record> AtomList = new List<ATOM_Record>();
                }
            }
        }
    }
}