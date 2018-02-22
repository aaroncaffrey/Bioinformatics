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
    ///     This class represents a three dimensional point.
    /// </summary>
    public class Point3D : AbstractPoint
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Point3D" /> class.
        /// </summary>
        public Point3D()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Point3D" /> class.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point3D(decimal x, decimal y, decimal z)
        {
            X = x;
            Y = y;
            Z = z;
            ParseOK = true;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Point3D" /> class.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point3D(string x, string y, string z)
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

            decimal localZ;
            if (decimal.TryParse(z, out localZ))
            {
                Z = localZ;
            }
            else
            {
                Z = 0;
                ParseOK = false;
            }
        }

        /// <summary>
        ///     Gets or sets X. X is the point's position on the X axis.
        /// </summary>
        public decimal X { get; set; }

        /// <summary>
        ///     Gets or sets Y. Y is the point's position on the Y axis.
        /// </summary>
        public decimal Y { get; set; }

        /// <summary>
        ///     Gets or sets Z.  Z is the point's position on the Z axis.
        /// </summary>
        public decimal Z { get; set; }

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
        public static decimal Distance3D(Point3D pointA, Point3D pointB, bool abs = true)
        {
            var result = new Point3D(pointA.X - pointB.X, pointA.Y - pointB.Y, pointA.Z - pointB.Z);
            var distance3D = (decimal) Math.Sqrt(((double) ((result.X*result.X) + (result.Y*result.Y) + (result.Z*result.Z))));

            if (abs)
            {
                distance3D = Math.Abs(distance3D);
            }

            return distance3D;
        }
    }
}