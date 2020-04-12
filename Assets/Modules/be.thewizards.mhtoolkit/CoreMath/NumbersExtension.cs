using System;
using UnityEngine;
using System.Linq;
using System.Text;

public static class NumbersExtension
{
	public static int Ceiling(this float f)
	{
		if ((int)f == f) return (int)f;
		if (f < 0) return (int)f;
		return (int)f + 1;
	}


	public static int Ceiling(this double d)
	{
		if ((int)d == d) return (int)d;
		if (d < 0) return (int)d;
		return (int)d + 1;
	}

	public static int Floor(this float f)
	{
		if ((int)f == f) return (int)f;
		if (f < 0) return (int)f - 1;
		return (int)f;
	}



	public static float AtLeast(this float f, float min)
	{
		return Mathf.Max(f, min);
	}

	public static float AtMost(this float f, float max)
	{
		return Mathf.Min(f, max);
	}

	public static int AtLeast(this int f, int min)
	{
		return Math.Max(f, min);
	}

	public static int AtMost(this int f, int max)
	{
		return Mathf.Min(f, max);
	}

	public static int Round(this float f)
	{
		return f - f.Floor() > f.Ceiling() - f ? f.Ceiling() : f.Floor();
	}

	public static bool IsOdd(this int i)
	{
		return i % 2 == 1;
	}

	public static bool IsEven(this int i)
	{
		return !i.IsOdd();
	}

	public static float DegToRad(this float f)
	{
		return Mathf.PI * f / 180;
	}

	public static float RadToDeg(this float f)
	{
		return 180 * f / Mathf.PI;
	}

	public static float PosMod(this float f, float m)
	{
		float result = f % m;
		return result < 0 ? result + m : result;
	}

	public static int PosMod(this int f, int m)
	{
		int result = f % m;
		return result < 0 ? result + m : result;
	}

	public static bool IsBetween(this int i, int min, int max, bool includeLowerBound = true, bool includeUpperBound = true)
	{
		return i > min && i < max || i == min && includeLowerBound || i == max && includeUpperBound;
	}

	public static bool IsBetween(this float f, float min, float max, bool includeLowerBound = true, bool includeUpperBound = true)
	{
		return f > min && f < max || f == min && includeLowerBound || f == max && includeUpperBound;
	}

	public static float ReverseOrZero(this float f)
	{
		if (f == 0) return 0;
		return 1 / f;
	}

	/// <param name="target">value to reach</param>
	/// <param name="step">value to add or remove from this, going towards <paramref name="target"/></param>
	public static float HeadFor(this float f, float target, float step)
	{
		if (f < target) return Mathf.Min(target, f + step);
		return Mathf.Max(target, f - step);
	}
}