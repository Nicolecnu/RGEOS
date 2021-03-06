using System;

namespace RGeos.Geometries
{
    /// <summary>
    /// A Point3D is a 0-dimensional geometry and represents a single location in 3D coordinate space. A Point3D has a x coordinate
    /// value, a y-coordinate value and a z-coordinate value. The boundary of a Point3D is the empty set.
    /// </summary>
    [Serializable]
    public class Point3D : RgPoint
    {
        private double _Z;

        /// <summary>
        /// Initializes a new Point
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Point3D(double x, double y, double z)
            : base(x, y)
        {
            _Z = z;
        }

        /// <summary>
        /// Initializes a new Point
        /// </summary>
        /// <param name="p">2D Point</param>
        /// <param name="z">Z coordinate</param>
        public Point3D(RgPoint p, double z)
            : base(p.X, p.Y)
        {
            _Z = z;
        }
        public Point3D(Vector3d v)
            : base(v.X, v.Y)
        {
            _Z = v.Z;
        }
        /// <summary>
        /// Initializes a new Point at (0,0)
        /// </summary>
        public Point3D()
            : this(0, 0, 0)
        {
            SetIsEmpty = true;
        }

        /// <summary>
        /// Create a new point by a douuble[] array
        /// </summary>
        /// <param name="point"></param>
        public Point3D(double[] point)
            : base(point[0], point[1])
        {
            if (point.Length != 3)
                throw new Exception("Only 3 dimensions are supported for points");

            _Z = point[2];
        }

        /// <summary>
        /// Gets or sets the Z coordinate of the point
        /// </summary>
        public double Z
        {
            get
            {
                if (!IsEmpty())
                    return _Z;
                else throw new ApplicationException("Point is empty");
            }
            set
            {
                _Z = value;
                SetIsEmpty = false;
            }
        }

        /// <summary>
        /// Returns part of coordinate. Index 0 = X, Index 1 = Y, , Index 2 = Z
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override double this[uint index]
        {
            get
            {
                if (index == 2)
                {
                    if (IsEmpty())
                        throw new ApplicationException("Point is empty");
                    return Z;
                }
                else
                    return base[index];
            }
            set
            {
                if (index == 2)
                {
                    Z = value;
                    SetIsEmpty = false;
                }
                else base[index] = value;
            }
        }

        /// <summary>
        /// Returns the number of ordinates for this point
        /// </summary>
        public override int NumOrdinates
        {
            get { return 3; }
        }

        #region Operators

        ///// <summary>
        ///// Vector + Vector
        ///// </summary>
        ///// <param name="v1">Vector</param>
        ///// <param name="v2">Vector</param>
        ///// <returns></returns>
        //public static Point3D operator +(Point3D v1, Point3D v2)
        //{
        //    return new Point3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        //}


        ///// <summary>
        ///// Vector - Vector
        ///// </summary>
        ///// <param name="v1">Vector</param>
        ///// <param name="v2">Vector</param>
        ///// <returns>Cross product</returns>
        //public static Point3D operator -(Point3D v1, Point3D v2)
        //{
        //    return new Point3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        //}

        /// <summary>
        /// Vector * Scalar
        /// </summary>
        /// <param name="m">Vector</param>
        /// <param name="d">Scalar (double)</param>
        /// <returns></returns>
        public static Point3D operator *(Point3D m, double d)
        {
            return new Point3D(m.X * d, m.Y * d, m.Z * d);
        }

        #endregion

        #region "Inherited methods from abstract class Geometry"

        /// <summary>
        /// Checks whether this instance is spatially equal to the Point 'o'
        /// </summary>
        /// <param name="p">Point to compare to</param>
        /// <returns></returns>
        public bool Equals(Point3D p)
        {
            return base.Equals(p) && p.Z == _Z;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="GetHashCode"/> is suitable for use 
        /// in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>A hash code for the current <see cref="GetHashCode"/>.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ _Z.GetHashCode();
        }

        /// <summary>
        /// Returns the distance between this geometry instance and another geometry, as
        /// measured in the spatial reference system of this instance.
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override double Distance(Geometry geom)
        {
            if (geom.GetType() == typeof(Point3D))
            {
                Point3D p = geom as Point3D;
                return Math.Sqrt(Math.Pow(X - p.X, 2) + Math.Pow(Y - p.Y, 2) + Math.Pow(Z - p.Z, 2));
            }
            else
                return base.Distance(geom);
        }

        #endregion

        /// <summary>
        /// exports a point into a 3-dimensional double array
        /// </summary>
        /// <returns></returns>
        public new double[] ToDoubleArray()
        {
            return new double[3] { X, Y, Z };
        }

        /// <summary>
        /// This method must be overridden using 'public new [derived_data_type] Clone()'
        /// </summary>
        /// <returns>Clone</returns>
        public new Point3D Clone()
        {
            return new Point3D(X, Y, Z);
        }

        /// <summary>
        /// Checks whether the two points are spatially equal
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>true if the points a spatially equal</returns>
        public bool Equals(Point3D p1, Point3D p2)
        {
            return (p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z);
        }

        /// <summary>
        /// Comparator used for ordering point first by ascending X, then by ascending Y and then by ascending Z.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual int CompareTo(Point3D other)
        {
            if (X < other.X || X == other.X && Y < other.Y || X == other.X && Y == other.Y && Z < other.Z)
                return -1;
            else if (X > other.X || X == other.X && Y > other.Y || X == other.X && Y == other.Y && Z > other.Z)
                return 1;
            else // (this.X == other.X && this.Y == other.Y && this.Z == other.Z)
                return 0;
        }


        public void Move(double dx, double dy, double dz)
        {
            this.X += dx;
            this.Y += dy;
            this.Z += dz;
        }
        /// <summary>
        /// 点相减的一个向量
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Vector3d operator -(Point3D p1, Point3D p2)
        {
            Vector3d Vec = new Vector3d();
            Vec.X = (p1.X - p2.X);
            Vec.Y = (p1.Y - p2.Y);
            Vec.Z = (p1.Z - p2.Z);
            return Vec;
        }
        //public static bool operator ==(RPoint p1, RPoint p2)
        //{
        //    return (p1.x == p2.x) && (p1.y == p2.y) && (p1.z == p2.z);
        //}
        //public static bool operator !=(RPoint p1, RPoint p2)
        //{
        //    return (p1.x != p2.x) || (p1.y != p2.y) || (p1.z != p2.z);

        //}
        public static Point3D operator +(Point3D Pts, Vector3d V3)//向量除以数量
        {
            Point3D temp = new Point3D();
            temp.X = Pts.X + V3.X;
            temp.Y = Pts.Y + V3.Y;
            temp.Z = Pts.Z + V3.Z;
            return temp;
        }
    }
}