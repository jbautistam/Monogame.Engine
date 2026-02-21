using Microsoft.Xna.Framework;

namespace GameEngine.Math;

public static class MathUtils
{
	public static float Lerp(float start, float end, float t)
	{
	return start + (end - start) * MathHelper.Clamp(t, 0, 1);
	}

	public static Vector2 Lerp(Vector2 start, Vector2 end, float t)
	{
	return new Vector2(
		Lerp(start.X, end.X, t),
		Lerp(start.Y, end.Y, t)
	);
	}

	public static float Snap(float value, float gridSize) => (float) System.Math.Floor(value / gridSize) * gridSize;
}
