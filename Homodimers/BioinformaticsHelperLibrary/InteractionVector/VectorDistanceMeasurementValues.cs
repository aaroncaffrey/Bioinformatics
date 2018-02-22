using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class VectorDistanceMeasurementValues
    {
        public double InteractionAndInteraction = 0;
        public double InteractionAndNonInteraction = 1;
        public double NonInteractionAndNonInteraction = 0;
        public double DifferentLengthProteinInterface = 0;

        public VectorDistanceMeasurementValues()
        {
            
        }

        public VectorDistanceMeasurementValues(double interactionAndInteraction, double interactionAndNonInteraction, double nonInteractionAndNonInteraction, double differentLengthProteinInterface)
        {
            InteractionAndInteraction = interactionAndInteraction;
            InteractionAndNonInteraction = interactionAndNonInteraction;
            NonInteractionAndNonInteraction = nonInteractionAndNonInteraction;
            DifferentLengthProteinInterface = differentLengthProteinInterface;
        }

        public VectorDistanceMeasurementValues(string interactionAndInteraction, string interactionAndNonInteraction, string nonInteractionAndNonInteraction, string differentLengthProteinInterface)
        {
            InteractionAndInteraction = double.Parse(interactionAndInteraction);
            InteractionAndNonInteraction = double.Parse(interactionAndNonInteraction);
            NonInteractionAndNonInteraction = double.Parse(nonInteractionAndNonInteraction);
            DifferentLengthProteinInterface = double.Parse(differentLengthProteinInterface);
        }
    }
}
