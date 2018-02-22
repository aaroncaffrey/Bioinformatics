using System;
using BioinformaticsHelperLibrary.Misc;

namespace BioinformaticsHelperLibrary.Measurements
{
    public class DistanceMatrixCalculations
    {
        /// <summary>
        ///     This method finds the smallest value in the given distance array.  If the smallest value appears more than once,
        ///     then the lower index is returned.
        /// </summary>
        /// <param name="distances">The pre-calculated distance matrix.</param>
        /// <returns>The first smallest distance index.</returns>
        public static int[] FirstSmallestDistanceIndex(decimal[,] distances)
        {
            if (ParameterValidation.IsDecimalArrayNullOrEmpty(distances))
            {
                return null;
            }

            var indexSmallest = new[] {0, 0};
            //var indexSmallestA = 0;
            //var indexSmallestB = 0;

            for (int indexA = distances.GetLowerBound(0); indexA <= distances.GetUpperBound(0); indexA++)
            {
                for (int indexB = distances.GetLowerBound(1); indexB <= distances.GetUpperBound(1); indexB++)
                {
                    if ((indexA != indexB) && ((indexSmallest[0] == indexSmallest[1]) || (Math.Abs(distances[indexA, indexB]) < Math.Abs(distances[indexSmallest[0], indexSmallest[1]]))))
                    {
                        indexSmallest[0] = indexA;
                        indexSmallest[1] = indexB;
                    }
                }
            }

            return indexSmallest; // new int[] { indexSmallestA, indexSmallestB };
        }


        /// <summary>
        ///     This method finds the next smallest value in the array, after the value found in the index specified.  If the same
        ///     value exists multiple times, the lower index is returned.
        /// </summary>
        /// <param name="distances">The pre-calculated distance matrix.</param>
        /// <param name="indexLast"></param>
        /// <returns>The next smallest distance index from the last indexes.</returns>
        public static int[] NextSmallestDistanceIndex(decimal[,] distances, int[] indexLast)
        {
            if (ParameterValidation.IsDecimalArrayNullOrEmpty(distances))
            {
                throw new ArgumentOutOfRangeException(nameof(distances));
            }

            if (ParameterValidation.IsIntArrayNullOrEmpty(indexLast))
            {
                throw new ArgumentOutOfRangeException(nameof(indexLast));
            }

            var indexBest = new[] {-1, -1};
            //var indexBestA = -1;
            //var indexBestB = -1;

            if ((indexLast[0] == -1) || (indexLast[1] == -1))
            {
                return FirstSmallestDistanceIndex(distances);
            }

            for (int indexA = distances.GetLowerBound(0); indexA <= distances.GetUpperBound(0); indexA++)
            {
                for (int indexB = distances.GetLowerBound(1); indexB <= distances.GetUpperBound(1); indexB++)
                {
                    if
                        //// Not the same object twice / no distance measurement ... zero.    
                        ((indexA != indexB)
                         &&
                         //// Not the same indexes the other way around as last time (at least one value changed).
                         (indexLast[0] != indexB || indexLast[1] != indexA)
                         &&
                         //// Not the same indexes the same way around as last time (at least one value changed).
                         (indexLast[0] != indexA || indexLast[1] != indexB)
                         &&
                         //// Distance can be the same as the last one - if the index is higher.    
                         ((Math.Abs(distances[indexA, indexB]) == Math.Abs(distances[indexLast[0], indexLast[1]]) && (indexA > indexLast[0] || (indexA >= indexLast[0] && indexB > indexLast[1])))
                          ||
                          //// Distance can be more, but only if it is less than the best found so far.
                          (Math.Abs(distances[indexA, indexB]) > Math.Abs(distances[indexLast[0], indexLast[1]]) && (indexBest[0] == -1 || indexBest[1] == -1 || Math.Abs(distances[indexA, indexB]) < Math.Abs(distances[indexBest[0], indexBest[1]])))))
                    {
                        indexBest[0] = indexA;
                        indexBest[1] = indexB;

                        if (distances[indexA, indexB] == distances[indexLast[0], indexLast[1]])
                        {
                            // If the distance is the same, then it is impossible to find a lower one, so break out, also, if no break, indexes may be skipped.
                            break;
                        }
                    }
                }
            }

            return indexBest; //new int[] { indexBestA, indexBestB };
        }
    }
}