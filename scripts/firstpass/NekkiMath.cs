using System;

/// <summary>
/// Nekki math utility — random helpers and other numeric helpers.
/// </summary>
public static class NekkiMath
{
	private static readonly Random _rng = new Random();

	public static float randomFloat(float min, float max)
		=> (float)(_rng.NextDouble() * (max - min) + min);

	public static float randomFloat(float max) => randomFloat(0f, max);

	public static int randomInt(int min, int max) => _rng.Next(min, max + 1);
	public static int randomInt(int max) => _rng.Next(0, max + 1);

	public static float Clamp(float value, float min, float max)
		=> value < min ? min : value > max ? max : value;

	public static int Clamp(int value, int min, int max)
		=> value < min ? min : value > max ? max : value;
}
