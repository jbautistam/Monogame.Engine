using Microsoft.Xna.Framework;

namespace GameEngine.Math;

public static class MathUtils
{
	public static float Lerp(float a, float b, float t)
	{
	return a + (b - a) * MathHelper.Clamp(t, 0, 1);
	}

	public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
	{
	return new Vector2(
		Lerp(a.X, b.X, t),
		Lerp(a.Y, b.Y, t)
	);
	}

	public static float Snap(float value, float gridSize) => (float) System.Math.Floor(value / gridSize) * gridSize;
}
