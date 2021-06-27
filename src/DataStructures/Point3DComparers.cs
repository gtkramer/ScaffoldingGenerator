using System.Collections.Generic;
using MathNet.Spatial.Euclidean;

namespace ScaffoldingGenerator.DataStructures
{
    public abstract class Point3DComparer : IComparer<Point3D>, IEqualityComparer<Point3D> {
        public bool Equals(Point3D a, Point3D b)
        {
            return Compare(a, b) == 0;
        }

        public int GetHashCode(Point3D p)
        {
            return p.GetHashCode();
        }

        public abstract int Compare(Point3D a, Point3D b);
    }

    public class Point3DXComparer : Point3DComparer
    {
        public override int Compare(Point3D a, Point3D b)
        {
            if (a.X < b.X)
            {
                return -1;
            }
            else if (a.X > b.X)
            {
                return 1;
            }
            else
            {
                if (a.Y < b.Y)
                {
                    return -1;
                }
                else if (a.Y > b.Y)
                {
                    return 1;
                }
                else
                {
                    if (a.Z < b.Z)
                    {
                        return -1;
                    }
                    else if (a.Z > b.Z)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }

    public class Point3DYComparer : Point3DComparer
    {
        public override int Compare(Point3D a, Point3D b)
        {
            if (a.Y < b.Y)
            {
                return -1;
            }
            else if (a.Y > b.Y)
            {
                return 1;
            }
            else
            {
                if (a.Z < b.Z)
                {
                    return -1;
                }
                else if (a.Z > b.Z)
                {
                    return 1;
                }
                else
                {
                    if (a.X < b.X)
                    {
                        return -1;
                    }
                    else if (a.X > b.X)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }

    public class Point3DZComparer : Point3DComparer
    {
        public override int Compare(Point3D a, Point3D b)
        {
            if (a.Z < b.Z)
            {
                return -1;
            }
            else if (a.Z > b.Z)
            {
                return 1;
            }
            else
            {
                if (a.X < b.X)
                {
                    return -1;
                }
                else if (a.X > b.X)
                {
                    return 1;
                }
                else
                {
                    if (a.Y < b.Y)
                    {
                        return -1;
                    }
                    else if (a.Y > b.Y)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}