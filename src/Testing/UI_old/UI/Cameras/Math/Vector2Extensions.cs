using Microsoft.Xna.Framework;

namespace GameEngine.Math;

public static class Vector2Extensions
{
    public static Vector2 Perpendicular(this Vector2 vector)
    {
        return new Vector2(-vector.Y, vector.X);
    }

    public static Vector2 Rotate(this Vector2 vector, float radians)
    {
        float cos = (float)System.Math.Cos(radians);
        float sin = (float)System.Math.Sin(radians);
            
        return new Vector2(
            vector.X * cos - vector.Y * sin,
            vector.X * sin + vector.Y * cos
        );
    }

    public static Vector2 RotateAround(this Vector2 vector, Vector2 origin, float radians)
    {
        Vector2 diff = (vector - origin);

            diff.Rotate(radians);
            return origin + diff;
    }

    public static Vector2 Snap(this Vector2 vector, float gridSize)
    {
        return new Vector2(
            MathUtils.Snap(vector.X, gridSize),
            MathUtils.Snap(vector.Y, gridSize)
        );
    }

    public static Vector2 Snap(this Vector2 vector, Vector2 gridSize) => new (MathUtils.Snap(vector.X, gridSize.X), MathUtils.Snap(vector.Y, gridSize.Y));

    public static float Cross(this Vector2 a, Vector2 b)
    {
        return a.X * b.Y - a.Y * b.X;
    }

    public static Vector2 Abs(this Vector2 vector)
    {
        return new Vector2(
            System.Math.Abs(vector.X),
            System.Math.Abs(vector.Y)
        );
    }

    public static Vector2 Sign(this Vector2 vector)
    {
        return new Vector2(
            System.Math.Sign(vector.X),
            System.Math.Sign(vector.Y)
        );
    }

    public static Vector2 ComponentMul(this Vector2 a, Vector2 b)
    {
        return new Vector2(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2 ComponentDiv(this Vector2 a, Vector2 b)
    {
        return new Vector2(a.X / b.X, a.Y / b.Y);
    }
}
