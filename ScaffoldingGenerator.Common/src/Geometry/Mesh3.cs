namespace ScaffoldingGenerator.Geometry
{
    public class Mesh3
    {
        public Polygon3[] Facets;
        public Point3 MinPoint;
        public Point3 MaxPoint;
        public Point3 CenterPoint;

        public Mesh3(Polygon3[] facets)
        {
            Facets = facets;

            float minX, minY, minZ;
            float maxX, maxY, maxZ;
            minX = minY = minZ = float.MaxValue;
            maxX = maxY = maxZ = float.MinValue;
            foreach (Polygon3 facet in facets)
            {
                foreach (Point3 vertex in facet.Vertices)
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
            MinPoint = new Point3(minX, minY, minZ);
            MaxPoint = new Point3(maxX, maxY, maxZ);
            CenterPoint = Point3.MidPoint(MinPoint, MaxPoint);
        }
    }
}
