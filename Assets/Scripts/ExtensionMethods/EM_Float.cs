using System;

public static class EM_Float
{
	static float max = Convert.ToSingle(int.MaxValue);
	static float min = Convert.ToSingle(int.MinValue);

	public static int AsPosInt (this float value)//as positive integer
	{
		if (float.IsNaN(value) || value <= 0.0f) return 0;
		if (value >= max) return int.MaxValue;
		return Convert.ToInt32(Math.Truncate(value));
	}
}
