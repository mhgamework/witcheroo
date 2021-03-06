﻿using System;
using System.Diagnostics;
using DirectX11;

namespace MHGameWork.TheWizards.SkyMerchant._Engine.DataStructures
{
    /// <summary>
    /// Represents a array that can be accessed using Point3
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Array3D<T>
    {
        public T[,,] arr;
        public Point3 Size { get; private set; }

        public Array3D(Point3 size)
        {
            arr = new T[size.X, size.Y, size.Z];
            Size = size;
        }
        public T this[Point3 pos]
        {
            get
            {
                if (!InArray(pos)) return default(T);
                return arr[pos.X, pos.Y, pos.Z];
            }
            set { arr[pos.X, pos.Y, pos.Z] = value; }
        }
        public T Get(Point3 pos)
        {
            return arr[pos.X, pos.Y, pos.Z];
        }
        /// <summary>
        /// Very fast get operation, no bounds checking
        /// </summary>
        public T GetFast(int x, int y, int z)
        {
            return arr[x, y, z];
        }


        public bool InArray(Point3 pos)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.Z < 0) return false;
            if (pos.X >= Size.X || pos.Y >= Size.Y || pos.Z >= Size.Z) return false;

            return true;
        }

        public void ForEach(Action<T, Point3> func)
        {
            for (int x = 0; x < Size.X; x++)
                for (int y = 0; y < Size.Y; y++)
                    for (int z = 0; z < Size.Z; z++)
                    {
                        var pos = new Point3(x, y, z);
                        func(this[pos], pos);
                    }
        }

        public T GetTiledFast(int x, int y, int z)
        {
            // Dont call the this[] directly, its about 20% slower when i tested it. Probably because of the out of bounds check
            // TODO: since this is always in bounds, maybe try using 'unsafe' and pointers to avoid the C# array bounds checking?
            Point3 point3 = Size; // Reading this out is a bit faster
            x = TWMath.nfmod(x, point3.X);
            y = TWMath.nfmod(y, point3.Y);
            z = TWMath.nfmod(z, point3.Z);
            return arr[x, y, z];
        }
        public T GetTiled(Point3 pos)
        {
            // Dont call the this[] directly, its about 20% slower when i tested it. Probably because of the out of bounds check
            // TODO: since this is always in bounds, maybe try using 'unsafe' and pointers to avoid the C# array bounds checking?
            Point3 point3 = Size; // Reading this out is a bit faster
            var x = TWMath.nfmod(pos.X, point3.X);
            var y = TWMath.nfmod(pos.Y, point3.Y);
            var z = TWMath.nfmod(pos.Z, point3.Z);
            return arr[x, y, z];
        }

        public T[] ToArray()
        {
            var data = new T[Size.X*Size.Y*Size.Z];

            int index = 0;

            for (int x = 0; x < Size.X; x++)
                for (int y = 0; y < Size.Y; y++)
                    for (int z = 0; z < Size.Z; z++)
                        data[index++] = GetFast(x,y,z);

            return data;

        }

        public static Array3D<T> FromFlattenedArray(T[] data, Point3 size)
        {
            var ret = new Array3D<T>(size);

            int index = 0;

            for (int x = 0; x < size.X; x++)
                for (int y = 0; y < size.Y; y++)
                    for (int z = 0; z < size.Z; z++)
                    { 
                        ret[new Point3(x,y,z)] = data[index++];
                    }

            return ret;

        }
    }
}