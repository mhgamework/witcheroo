using System;
using System.Diagnostics;
using DirectX11;

namespace MHGameWork.TheWizards.SkyMerchant._Engine.DataStructures
{
    /// <summary>
    /// Represents a array that can be accessed using Point3
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Array2D<T>
    {
        public T[] arr;
        public Point2 Size { get; private set; }

        public Array2D(Point2 size)
        {
            arr = new T[size.X * size.Y];
            Size = size;
        }
        public T this[Point2 pos]
        {
            get
            {
                if (!InArray(pos)) return default(T);
                return arr[pos.Y * Size.X + pos.X];
            }
            set { arr[pos.Y * Size.X + pos.X] = value; }
        }
        public T this[int indexYThenX]
        {
            get
            {
                return arr[indexYThenX];
            }
            set { arr[indexYThenX] = value; }
        }
        public T Get(Point2 pos)
        {
            return arr[pos.Y * Size.X + pos.X];
        }
        /// <summary>
        /// Very fast get operation, no bounds checking
        /// </summary>
        public T GetFast(int x, int y)
        {
            return arr[y * Size.X + x];
        }

        public void SetFast(int x, int y, T val)
        {
            arr[y * Size.X + x] = val;
        }
      

        public bool InArray(Point2 pos)
        {
            if (pos.X < 0 || pos.Y < 0) return false;
            if (pos.X >= Size.X || pos.Y >= Size.Y) return false;

            return true;
        }

        public void ForEach(Action<T, Point2> func)
        {
            for (int x = 0; x < Size.X; x++)
                for (int y = 0; y < Size.Y; y++)
                {
                    var pos = new Point2(x, y);
                    func(this[pos], pos);
                }
        }

        public T GetTiledFast(int x, int y)
        {
            // Dont call the this[] directly, its about 20% slower when i tested it. Probably because of the out of bounds check
            // TODO: since this is always in bounds, maybe try using 'unsafe' and pointers to avoid the C# array bounds checking?
            Point2 point3 = Size; // Reading this out is a bit faster
            x = TWMath.nfmod(x, point3.X);
            y = TWMath.nfmod(y, point3.Y);
            return arr[y * Size.X + x];
        }
        public T GetTiled(Point2 pos)
        {
            // Dont call the this[] directly, its about 20% slower when i tested it. Probably because of the out of bounds check
            // TODO: since this is always in bounds, maybe try using 'unsafe' and pointers to avoid the C# array bounds checking?
            Point2 point3 = Size; // Reading this out is a bit faster
            var x = TWMath.nfmod(pos.X, point3.X);
            var y = TWMath.nfmod(pos.Y, point3.Y);
            return arr[y * Size.X + x];
        }

        /// <summary>
        /// i = y*size.x+x
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            var data = new T[Size.X * Size.Y];

            int index = 0;

            for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                    data[index++] = GetFast(x, y);

            return data;

        }

        public static Array2D<T> FromFlattenedArray(T[] data, Point2 size)
        {
            var ret = new Array2D<T>(size);

            int index = 0;

            for (int y = 0; y < size.Y; y++)
                for (int x = 0; x < size.X; x++)
                    {
                        ret[new Point2(x, y)] = data[index++];
                    }

            return ret;

        }

    }
}