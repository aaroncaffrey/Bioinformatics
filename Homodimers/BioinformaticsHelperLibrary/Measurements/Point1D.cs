//-----------------------------------------------------------------------
// <copyright file="Points.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;

namespace BioinformaticsHelperLibrary.Measurements
{
    /// <summary>
    ///     This class represents a one dimensional point.
    /// </summary>
    public class Point1D : AbstractPoint
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Point1D" /> class.
        /// </summary>
        public Point1D()
            : this(0)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Point1D" /> class.
        /// </summary>
        /// <param name="p"></param>
        public Point1D(decimal p)
        {
            P = p;
            ParseOK = true;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Point1D" /> class.
        /// </summary>
        /// <param name="p"></param>
        public Point1D(string p)
        {
            decimal localP;

            if (decimal.TryParse(p, out localP))
            {
                P = localP;
                ParseOK = true;
            }
            else
            {
                P = 0;
                ParseOK = false;
            }
        }

        /// <summary>
        ///     Gets or sets P.  P is the stored point.
        /// </summary>
        public decimal P { get; set; }

        /// <summary>
        ///     Gets a value indicating whether or not the number was successfully parsed.
        /// </summary>
        public bool ParseOK { get; private set; }

        /// <summary>
        ///     Calculate the distance between two given points.
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="abs"></param>
        /// <returns></returns>
        public static decimal Distance1D(Point1D pointA, Point1D pointB, bool abs = true)
        {
            var result = new Point1D(pointA.P - pointB.P);
            decimal distance1D = result.P;

            if (abs)
            {
                distance1D = Math.Abs(distance1D);
            }

            return distance1D;
        }
    }
}