using System;
using System.Linq;
using BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection;
using BioinformaticsHelperLibrary.Misc;

namespace BioinformaticsHelperLibrary.Dimers
{
    public class HomodimerSymmetry
    {

        public static ProteinInterfaceSymmetryModeValues CheckDimer2X2ProteinInterfaceSymmetry(InteractionBetweenProteinInterfacesListContainer interactionBetweenProteinInterfacesContainer)
        {
            if (ParameterValidation.IsInteractionBetweenProteinInterfacesListContainerNullOrEmpty(interactionBetweenProteinInterfacesContainer))
            {
                throw new ArgumentOutOfRangeException(nameof(interactionBetweenProteinInterfacesContainer));
            }

            //int? chainIdA = null;
            //int? chainIdB = null;

            //foreach (var interaction in interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList)
            //{
            //    if (chainIdA == null || interaction.Atom1.FullProteinInterfaceId.ChainId < chainIdA)
            //    {
            //        chainIdA = interaction.Atom1.FullProteinInterfaceId.ChainId;
            //    }

            //    if (chainIdA == null || interaction.Atom2.FullProteinInterfaceId.ChainId < chainIdA)
            //    {
            //        chainIdA = interaction.Atom2.FullProteinInterfaceId.ChainId;
            //    }

            //    if (chainIdB == null || interaction.Atom1.FullProteinInterfaceId.ChainId > chainIdB)
            //    {
            //        chainIdB = interaction.Atom1.FullProteinInterfaceId.ChainId;
            //    }

            //    if (chainIdB == null || interaction.Atom2.FullProteinInterfaceId.ChainId > chainIdB)
            //    {
            //        chainIdB = interaction.Atom2.FullProteinInterfaceId.ChainId;
            //    }
            //}

            int chainIdA = 0;
            int chainIdB = 1;

            //List<int> totalProteinInterfacesA = new List<int>();
            //List<int> totalProteinInterfacesB = new List<int>();

            int totalProteinInterfacesA = interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Where(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdA).Select(a => a.Atom1.FullProteinInterfaceId.ProteinInterfaceId).Distinct().Count() +
                              interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Where(a => a.Atom2.FullProteinInterfaceId.ChainId == chainIdA).Select(a => a.Atom2.FullProteinInterfaceId.ProteinInterfaceId).Distinct().Count();

            int totalProteinInterfacesB = interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Where(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdB).Select(a => a.Atom1.FullProteinInterfaceId.ProteinInterfaceId).Distinct().Count() +
                              interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Where(a => a.Atom2.FullProteinInterfaceId.ChainId == chainIdB).Select(a => a.Atom2.FullProteinInterfaceId.ProteinInterfaceId).Distinct().Count();


            //foreach (var interaction in interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList)
            //{
            //    if (interaction.Atom1.FullProteinInterfaceId.ChainId == chainIdA && (interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId != null && !totalProteinInterfacesA.Contains((int) interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId)))
            //    {
            //        totalProteinInterfacesA.Add((int) interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId);
            //    }

            //    if (interaction.Atom2.FullProteinInterfaceId.ChainId == chainIdA && (interaction.Atom2.FullProteinInterfaceId.ProteinInterfaceId != null && !totalProteinInterfacesA.Contains((int) interaction.Atom2.FullProteinInterfaceId.ProteinInterfaceId)))
            //    {
            //        totalProteinInterfacesA.Add((int) interaction.Atom2.FullProteinInterfaceId.ProteinInterfaceId);
            //    }

            //    //

            //    if (interaction.Atom1.FullProteinInterfaceId.ChainId == chainIdB && (interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId != null && !totalProteinInterfacesB.Contains((int) interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId)))
            //    {
            //        totalProteinInterfacesB.Add((int) interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId);
            //    }

            //    if (interaction.Atom2.FullProteinInterfaceId.ChainId == chainIdB && (interaction.Atom2.FullProteinInterfaceId.ProteinInterfaceId != null && !totalProteinInterfacesB.Contains((int) interaction.Atom2.FullProteinInterfaceId.ProteinInterfaceId)))
            //    {
            //        totalProteinInterfacesB.Add((int) interaction.Atom2.FullProteinInterfaceId.ProteinInterfaceId);
            //    }
            //}

            //var interactionsBetweenProteinInterfaces2 = new List<AtomPair>[totalProteinInterfacesA.Max() > totalProteinInterfacesA.Count ? totalProteinInterfacesA.Max() : totalProteinInterfacesA.Count, totalProteinInterfacesB.Max() > totalProteinInterfacesB.Count ? totalProteinInterfacesB.Max() : totalProteinInterfacesB.Count];

            //var interactionsBetweenProteinInterfaces2 = new List<AtomPair>[totalProteinInterfacesA, totalProteinInterfacesB];

            //foreach (var interaction in interactionsBetweenProteinInterfaces.InteractionBetweenProteinInterfacesList)
            //{
            //    if (interaction.Atom1.FullProteinInterfaceId.ChainId == chainIdA && interaction.Atom2.FullProteinInterfaceId.ChainId == chainIdB)
            //    {
            //        AtomPair atomPair = new AtomPair(interaction.Atom1.Atom, interaction.Atom2.Atom, 0);
            //        interactionsBetweenProteinInterfaces2[(int)interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId, (int) interaction.Atom2 .ProteinInterfaceId].Add(atomPair);
            //    }
            //    else if (interaction.Atom1.FullProteinInterfaceId.ChainId == chainIdB && interaction.Atom2.FullProteinInterfaceId.ChainId == chainIdA)
            //    {
            //        AtomPair atomPair = new AtomPair(interaction.Atom2.Atom, interaction.Atom1.Atom, 0);
            //        interactionsBetweenProteinInterfaces2[(int)interaction.Atom2.FullProteinInterfaceId.ProteinInterfaceId, (int) interaction.Atom1.FullProteinInterfaceId.ProteinInterfaceId].Add(atomPair);
            //    }

            //}

            // C0S0 to C1S0 plus C0S1 to C1S1
            //int Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A = interactionsBetweenProteinInterfaces2[0, 0].Count;
            //int Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B = interactionsBetweenProteinInterfaces2[1, 1].Count;

            const int proteinInterfaceIdA = StaticValues.ProteinInterfaceA;
            const int proteinInterfaceIdB = StaticValues.ProteinInterfaceB;

            int Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A = interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdA && a.Atom2.FullProteinInterfaceId.ChainId == chainIdB && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA) +
                                                   interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdB && a.Atom2.FullProteinInterfaceId.ChainId == chainIdA && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA);

            int Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B = interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdA && a.Atom2.FullProteinInterfaceId.ChainId == chainIdB && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB) +
                                                   interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdB && a.Atom2.FullProteinInterfaceId.ChainId == chainIdA && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB);

            int categoryStraight = Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A + Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B;

            // C0S0 to C1S1 and C0S1 to C1S0
            //int Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B = interactionsBetweenProteinInterfaces2[0, 1].Count;
            //int Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A = interactionsBetweenProteinInterfaces2[1, 0].Count;

            int Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B = interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdA && a.Atom2.FullProteinInterfaceId.ChainId == chainIdB && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB) +
                                                   interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdB && a.Atom2.FullProteinInterfaceId.ChainId == chainIdA && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA);

            int Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A = interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdA && a.Atom2.FullProteinInterfaceId.ChainId == chainIdB && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA) +
                                                   interactionBetweenProteinInterfacesContainer.InteractionBetweenProteinInterfacesList.Count(a => a.Atom1.FullProteinInterfaceId.ChainId == chainIdB && a.Atom2.FullProteinInterfaceId.ChainId == chainIdA && a.Atom1.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdA && a.Atom2.FullProteinInterfaceId.ProteinInterfaceId == proteinInterfaceIdB);

            int categoryDiagonal = Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B + Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A;

            var proteinInterfaceSymmetryModeEnum = ProteinInterfaceSymmetryModeEnum.Unknown;

            // Top straight line ----------------------- Bottom straight line ------------------ Top to bottom diagonal line ----------- Bottom to top diagonal line
            if (Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A == 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B == 0 &&
                Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B == 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A == 0)
            {
                // Interaction Mode 0
                proteinInterfaceSymmetryModeEnum = ProteinInterfaceSymmetryModeEnum.Unknown;
            }
            else if (Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B > 0 &&
                     Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B == 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A == 0)
            {
                // Interaction Mode 1
                proteinInterfaceSymmetryModeEnum = ProteinInterfaceSymmetryModeEnum.Straight;
            }
            else if (Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A == 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B == 0 &&
                     Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A > 0)
            {
                // Interaction Mode 2
                proteinInterfaceSymmetryModeEnum = ProteinInterfaceSymmetryModeEnum.Diagonal;
            }
            else if (Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B > 0 &&
                     Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A > 0)
            {
                // Interaction Mode 3
                proteinInterfaceSymmetryModeEnum = ProteinInterfaceSymmetryModeEnum.StraightAndDiagonal;
            }
            else if (Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B == 0 &&
                     Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A > 0)
            {
                // Interaction Mode 4
                proteinInterfaceSymmetryModeEnum = ProteinInterfaceSymmetryModeEnum.DiagonalAndTopStraight;
            }
            else if (Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A == 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B > 0 &&
                     Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A > 0)
            {
                // Interaction Mode 5
                proteinInterfaceSymmetryModeEnum = ProteinInterfaceSymmetryModeEnum.DiagonalAndBottomStraight;
            }
            else if (Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B > 0 &&
                     Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A == 0)
            {
                // Interaction Mode 6
                proteinInterfaceSymmetryModeEnum = ProteinInterfaceSymmetryModeEnum.StraightAndTopToBottomDiagonal;
            }
            else if (Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_A > 0 && Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_B > 0 &&
                     Chain_A_ProteinInterface_A_to_Chain_B_ProteinInterface_B == 0 &&
                     Chain_A_ProteinInterface_B_to_Chain_B_ProteinInterface_A > 0)
            {
                // Interaction Mode 7
                proteinInterfaceSymmetryModeEnum = ProteinInterfaceSymmetryModeEnum.StraightAndBottomToTopDiagonal;
            }

            return new ProteinInterfaceSymmetryModeValues(categoryDiagonal, categoryStraight, proteinInterfaceSymmetryModeEnum);
        }
    }
}
