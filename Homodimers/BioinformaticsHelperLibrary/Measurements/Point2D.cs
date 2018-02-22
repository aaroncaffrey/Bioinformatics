//-----------------------------------------------------------------------
// <copyright file="Points.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using BioinformaticsHelperLibrary.InteractionDetection;

namespace BioinformaticsHelperLibrary.Measurements
{
    /// <summary>
    ///     /// This class represents a two dimensional point.
    /// </summary>
    public class Point2D : AbstractPoint
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Point2D" /> class.
        /// </summary>
        public Point2D()
            : this(0, 0)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Point2D" /> class.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point2D(decimal x, decimal y)
        {
            X = x;
            Y = y;
            ParseOK = true;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Point2D" /> class.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point2D(string x, string y)
        {
            ParseOK = true;

            decimal localX;
            if (decimal.TryParse(x, out localX))
            {
                X = localX;
            }
            else
            {
                X = 0;
                ParseOK = false;
            }

            decimal localY;
            if (decimal.TryParse(y, out localY))
            {
                Y = localY;
            }
            else
            {
                Y = 0;
                ParseOK = false;
            }
        }

        /// <summary>
        ///     Gets or sets X.  X is the point's position on the X axis.
        /// </summary>
        public decimal X { get; set; }

        /// <summary>
        ///     Gets or sets Y.  Y is the point's position on the Y axis.
        /// </summary>
        public decimal Y { get; set; }

        /// <summary>
        ///     Gets a value indicating whether or not the numbers were successfully parsed.
        /// </summary>
        public bool ParseOK { get; private set; }

        /// <summary>
        ///     Calculate the distance between two given points.
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="abs"></param>
        /// <returns></returns>
        public static decimal Distance2D(Point2D pointA, Point2D pointB, bool abs = true, Dictionary<double, double> sqrtCache = null)
        {
            if (sqrtCache == null)
            {
                sqrtCache = new Dictionary<double, double>();
            }

            var result = new Point2D(pointA.X - pointB.X, pointA.Y - pointB.Y);
            var distance2D = (decimal)SqrtCache.CachedSqrt((double)((result.X * result.X) + (result.Y * result.Y)), sqrtCache);

            if (abs)
            {
                distance2D = Math.Abs(distance2D);
            }

            return distance2D;
        }
    }
}