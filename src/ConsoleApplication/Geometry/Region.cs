using System.Collections.Generic;
using MathNet.Spatial.Euclidean;

namespace AdditiveManufacturing.Geometry
{
    public class Region
    {
        public List<Facet> Facets;
        public Point3D MinPoint;
        public Point3D MaxPoint;
        public Point3D CenterPoint;

        public Region(List<Facet> facets)
        {
            Facets = facets;

            double minX, minY, minZ;
            double maxX, maxY, maxZ;
            minX = minY = minZ = double.MaxValue;
            maxX = maxY = maxZ = double.MinValue;
            foreach (Facet facet in facets)
            {
                foreach (Point3D vertex in facet.Vertices)
                {
                    if (vertex.X < minX)
                    {
                        minX = vertex.X;
                    }
                    if (vertex.Y < minY)
                    {
                        minY = vertex.Y;
                    }
                    if (vertex.Z < minZ)
                    {
                        minZ = vertex.Z;
                    }

                    if (vertex.X > maxX)
                    {
                        maxX = vertex.X;
                    }
                    if (vertex.Y > maxY)
                    {
                        maxY = vertex.Y;
                    }
                    if (vertex.Z > maxZ)
                    {
                        maxZ = vertex.Z;
                    }
                }
            }
            MinPoint = new Point3D(minX, minY, minZ);
            MaxPoint = new Point3D(maxX, maxY, maxZ);
            CenterPoint = Point3D.MidPoint(MinPoint, MaxPoint);
        }

        public bool ContainsPoint(Point3D point) {
            return point.X >= MinPoint.X && point.X <= MaxPoint.X &&
            point.Y >= MinPoint.Y && point.Y <= MaxPoint.Y &&
            point.Z >= MinPoint.Z && point.Z <= MaxPoint.Z;
        }
    }
}
