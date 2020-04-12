using System;
using UnityEngine;

namespace DirectX11
{
    /// <summary>
    /// This struct represents a discrete vector
    /// </summary>
    [Serializable]
    public struct Point2 : IEquatable<Point2>
    {
        public int X;
        public int Y;


        public Point2(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// This uses a vector3 as a Point2, note that this uses Math.round to convert the coords
        /// </summary>
        /// <param name="v"></param>
        public Point2(Vector2 v)
        {
            X = (int)Math.Round(v.x);
            Y = (int)Math.Round(v.y);
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    default:
                        throw new ArgumentException("index");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    default:
                        throw new ArgumentException("index");
                }
            }
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
        public static implicit operator Vector2(Point2 p)
        {
            return p.ToVector2();
        }
        public static Boolean operator ==(Point2 p, Point2 p2)
        {
            return p.hasSameValue(p2);
        }
        public static Boolean operator !=(Point2 p, Point2 p2)
        {
            return !p.hasSameValue(p2);
        }

        public Boolean hasSameValue(Point2 pos)
        {
            if (pos.X == X && pos.Y == Y)
                return true;
            return false;
        }
        public static Point2 operator -(Point2 p, Point2 p2)
        {
            return p + -p2;
        }
        public static Point2 operator +(Point2 p, Point2 p2)
        {
            return new Point2(p.X + p2.X, p.Y + p2.Y);
        }
        public static Point2 operator -(Point2 p)
        {
            return new Point2(-p.X, -p.Y);
        }

        public static Point2 operator *(Point2 a, int factor)
        {
            return new Point2(a.X * factor, a.Y * factor);
        }
        public static Point2 operator /(Point2 a, int factor)
        {
            return new Point2(a.X / factor, a.Y / factor);
        }

        public override string ToString()
        {
            return string.Format("X: {0},Y: {1}", X, Y);
        }

        public static Point2 UnitX()
        {
            return new Point2(1, 0);
        }
        public static Point2 UnitY()
        {
            return new Point2(0, 1);
        }

        public static Point2 Floor(Vector2 v)
        {
            return new Point2((int)Math.Floor(v.x), (int)Math.Floor(v.y));
        }
        public static Point2 Ceiling(Vector2 v)
        {
            return new Point2((int)Math.Ceiling(v.x), (int)Math.Ceiling(v.y));
        }


        /// <summary>
        /// Returns the magnitude of the vector. The magnitude is the 'length' of the vector from 0,0,0 to this point. Can be used for distance calculations:
        /// <code> Debug.Log ("Distance between 3,4,5 and 6,7,8 is: "+(new Int3(3,4,5) - new Int3(6,7,8)).magnitude); </code>
        /// </summary>
        public float magnitude
        {
            get
            {
                //It turns out that using doubles is just as fast as using ints with Mathf.Sqrt. And this can also handle larger numbers (possibly with small errors when using huge numbers)!

                double _x = X;
                double _y = Y;

                return (float)System.Math.Sqrt(_x * _x + _y * _y );
            }
        }

        public bool Equals(Point2 other)
        {
            return other.X == X && other.Y == Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Point2)) return false;
            return Equals((Point2)obj);
        }



        public override int GetHashCode()
        {
            unchecked
            {
                int result = X;
                result = (result * 397) ^ Y;
                return result;
            }
        }

        public float GetLength()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }


        public void ForEach(Action<Point2> func)
        {
            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                {
                    var pos = new Point2(x, y);
                    func(pos);
                }
        }

        public static readonly Point2[] DirectionsStraight = new Point2[]
            {new Point2(1, 0), new Point2(0, 1), new Point2(-1, 0), new Point2(0, -1)};

        public static readonly Point2[] DirectionsDiagonal = new Point2[]
            {
                new Point2(1, 1),
                new Point2(-1, 1),
                new Point2(-1, -1),
                new Point2(1, -1)

            };
        public static readonly Point2[] DirectionsStraightDiagonal = new Point2[]
        {
            new Point2(1, 0),new Point2(1, 1),
            new Point2(0, 1),new Point2(-1, 1),
            new Point2(-1, 0),new Point2(-1, -1),
            new Point2(0, -1),new Point2(1, -1)

        };

        public int Max()
        {
            return Math.Max(X, Y);
        }

        public static Point2 Clamp(Point2 value, Point2 min, Point2 max)
        {
            return new Point2(Mathf.Clamp(value.X,min.X,max.X), Mathf.Clamp(value.Y, min.Y, max.Y));
        }
        public static Point2 Min(Point2 a, Point2 b)
        {
            return new Point2(Mathf.Min(a.X,b.X), Mathf.Min(a.Y, b.Y));
        }
        public static Point2 Max(Point2 a, Point2 b)
        {
            return new Point2(Mathf.Max(a.X,b.X), Mathf.Max(a.Y, b.Y));
        }

        /// <summary>
        /// Componentwise minmax
        /// </summary>
        public static (Point2, Point2) MinMax(Point2 a, Point2 b)
        {
            return (Min(a, b), Max(a, b));
        }
    }
}
