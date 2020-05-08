using System.Collections.Generic;
using MathNet.Spatial.Euclidean;

namespace AdditiveManufacturing.DataStructures {
    public class Point3DComparer : IEqualityComparer<Point3D> {
        public bool Equals(Point3D a, Point3D b) {
            return a == b;
        }

        public int GetHashCode(Point3D obj) {
            return obj.GetHashCode();
        }
    }

    public class Point3DXComparer : IComparer<Point3D> {
        public int Compare(Point3D a, Point3D b) {
            if (a.X < b.X) {
                return -1;
            } else if (a.X > b.X) {
                return 1;
            } else {
                if (a.Y < b.Y) {
                    return -1;
                } else if (a.Y > b.Y) {
                    return 1;
                } else {
                    if (a.Z < b.Z) {
                        return -1;
                    } else if (a.Z > b.Z) {
                        return 1;
                    } else {
                        return 0;
                    }
                }
            }
        }
    }

    public class Point3DYComparer : IComparer<Point3D> {
        public int Compare(Point3D a, Point3D b) {
            if (a.Y < b.Y) {
                return -1;
            } else if (a.Y > b.Y) {
                return 1;
            } else {
                if (a.Z < b.Z) {
                    return -1;
                } else if (a.Z > b.Z) {
                    return 1;
                } else {
                    if (a.X < b.X) {
                        return -1;
                    } else if (a.X > b.X) {
                        return 1;
                    } else {
                        return 0;
                    }
                }
            }
        }
    }

    public class Point3DZComparer : IComparer<Point3D> {
        public int Compare(Point3D a, Point3D b) {
            if (a.Z < b.Z) {
                return -1;
            } else if (a.Z > b.Z) {
                return 1;
            } else {
                if (a.X < b.X) {
                    return -1;
                } else if (a.X > b.X) {
                    return 1;
                } else {
                    if (a.Y < b.Y) {
                        return -1;
                    } else if (a.Y > b.Y) {
                        return 1;
                    } else {
                        return 0;
                    }
                }
            }
        }
    }
}
