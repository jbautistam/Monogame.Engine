using Microsoft.Xna.Framework;

namespace GameEngine.Math;

public struct RectangleF
{
    public float X;
    public float Y;
    public float Width;
    public float Height;

    public RectangleF(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public RectangleF(Vector2 position, Vector2 size)
    {
        X = position.X;
        Y = position.Y;
        Width = size.X;
        Height = size.Y;
    }

    public float Left => X;
    public float Right => X + Width;
    public float Top => Y;
    public float Bottom => Y + Height;

    public Vector2 TopLeft => new Vector2(Left, Top);
    public Vector2 TopRight => new Vector2(Right, Top);
    public Vector2 BottomLeft => new Vector2(Left, Bottom);
    public Vector2 BottomRight => new Vector2(Right, Bottom);

    public Vector2 Center => new Vector2(X + Width * 0.5f, Y + Height * 0.5f);
    public Vector2 Size => new Vector2(Width, Height);
    public Vector2 Position => new Vector2(X, Y);

    public bool Contains(Vector2 point)
    {
        return point.X >= Left && point.X <= Right &&
                point.Y >= Top && point.Y <= Bottom;
    }

    public bool Contains(RectangleF other)
    {
        return Left <= other.Left && Right >= other.Right &&
                Top <= other.Top && Bottom >= other.Bottom;
    }

    public bool Intersects(RectangleF other)
    {
        return Left < other.Right && Right > other.Left &&
                Top < other.Bottom && Bottom > other.Top;
    }

    public RectangleF Expand(float amount)
    {
        return new RectangleF(
            X - amount,
            Y - amount,
            Width + amount * 2f,
            Height + amount * 2f
        );
    }

    public RectangleF Expand(Vector2 amount)
    {
        return new RectangleF(
            X - amount.X,
            Y - amount.Y,
            Width + amount.X * 2f,
            Height + amount.Y * 2f
        );
    }

    public static RectangleF Intersect(RectangleF a, RectangleF b)
    {
        float left = System.Math.Max(a.Left, b.Left);
        float top = System.Math.Max(a.Top, b.Top);
        float right = System.Math.Min(a.Right, b.Right);
        float bottom = System.Math.Min(a.Bottom, b.Bottom);

        if (right > left && bottom > top)
        {
            return new RectangleF(left, top, right - left, bottom - top);
        }

        return Empty;
    }

    public Rectangle ToRectangle()
    {
        return new Rectangle(
            (int)X,
            (int)Y,
            (int)Width,
            (int)Height
        );
    }

    public static RectangleF FromPoints(Vector2 a, Vector2 b)
    {
        float minX = System.Math.Min(a.X, b.X);
        float minY = System.Math.Min(a.Y, b.Y);
        float maxX = System.Math.Max(a.X, b.X);
        float maxY = System.Math.Max(a.Y, b.Y);

        return new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }

    public static RectangleF Empty => new RectangleF(0, 0, 0, 0);

    public bool IsEmpty => Width <= 0 || Height <= 0;

    public override string ToString()
    {
        return $"[{X}, {Y}, {Width}, {Height}]";
    }
}