using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.Common;

/// <summary>
///     Rectángulo con valores decimales
/// </summary>
[System.Diagnostics.DebuggerDisplay("({X},{Y}) ({Width},{Height})")]
public class RectangleF(float x, float y, float width, float height)
{
    /// <summary>
    ///     Crea un rectángulo vacío
    /// </summary>
    public static RectangleF Empty => new RectangleF(0, 0, 0, 0);

    /// <summary>
    ///     Crea un rectángulo a partir de puntos
    /// </summary>
    public static RectangleF FromPoints(Vector2 topLeft, Vector2 bottomRight)
    {
        float minX = Math.Min(topLeft.X, bottomRight.X);
        float minY = Math.Min(topLeft.Y, bottomRight.Y);
        float maxX = Math.Max(topLeft.X, bottomRight.X);
        float maxY = Math.Max(topLeft.Y, bottomRight.Y);

            // Devuelve el rectángulo generado
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }

    /// <summary>
    ///     Crea el rectángulo que intersecta con otro rectángulo
    /// </summary>
    public static RectangleF Intersect(RectangleF source, Rectangle target) => Intersect(source, new RectangleF(target.X, target.Y, target.Width, target.Height));

    /// <summary>
    ///     Crea el rectángulo que intersecta con otro rectángulo
    /// </summary>
    public static RectangleF Intersect(RectangleF source, RectangleF target)
    {
        float left = Math.Max(source.Left, target.Left);
        float top = Math.Max(source.Top, target.Top);
        float right = Math.Min(source.Right, target.Right);
        float bottom = Math.Min(source.Bottom, target.Bottom);

            // Si realmente tienen una intersección
            if (right > left && bottom > top)
                return new RectangleF(left, top, right - left, bottom - top);
            else
                return Empty;
    }

    /// <summary>
    ///     Posiciona el rectángulo
    /// </summary>
	public void MoveTo(Vector2 position)
	{
	    MoveTo(position.X, position.Y);
	}

    /// <summary>
    ///     Posiciona el rectángulo
    /// </summary>
	public void MoveTo(float x, float y)
	{
	    X = x;
        Y = y;
	}

    /// <summary>
    ///     Desplaza el rectángulo
    /// </summary>
	public void Translate(Vector2 offset)
	{
	    X += offset.X;
        Y += offset.Y;
	}

    /// <summary>
    ///     Cambia el tamaño del rectángulo
    /// </summary>
	public void Resize(Size size)
	{
		Width = size.Width;
        Height = size.Height;
	}

    /// <summary>
    ///     Comprueba si el rectángulo contiene un punto
    /// </summary>
    public bool Contains(float x, float y) => x >= Left && x <= Right && y >= Top && y <= Bottom;

    /// <summary>
    ///     Comprueba si el rectángulo contiene un punto
    /// </summary>
    public bool Contains(Vector2 point) => Contains(point.X, point.Y);

    /// <summary>
    ///     Comprueba si el rectángulo colisiona con otro
    /// </summary>
    public bool Intersects(RectangleF other) => Left < other.Right && Right > other.Left && Top < other.Bottom && Bottom > other.Top;

    /// <summary>
    ///     Comprueba si el rectángulo colisiona con otro
    /// </summary>
    public bool Intersects(Rectangle other) => Left < other.Right && Right > other.Left && Top < other.Bottom && Bottom > other.Top;

    /// <summary>
    ///     Limita las posiciones del rectángulo a los límites
    /// </summary>
	public void Clamp(Rectangle bounds)
	{
	    X = MathHelper.Clamp(X, bounds.X, bounds.Width - Width);
        Y = MathHelper.Clamp(Y, bounds.Y, bounds.Height - Height);
	}

    /// <summary>
    ///     Sobrecarga el operador ==
    /// </summary>
    public static bool operator ==(RectangleF? left, RectangleF? right)
    {
        if (ReferenceEquals(left, right)) 
            return true;
        else if (left is null || right is null) 
            return false;
        else
            return left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
    }

    /// <summary>
    ///     Sobrecarga el operador +
    /// </summary>
    public static RectangleF operator +(RectangleF? left, RectangleF? right)
    {
        if (left is null && right is null)
            return new RectangleF(0, 0, 0, 0);
        else if (left is null)
            return right!;
        else if (right is null)
            return left!;
        else
            return new RectangleF(left.X + right.X, left.Y + right.Y, left.Width + right.Width, left.Height + right.Height);
    }

    /// <summary>
    ///     Sobrecarga el operador +
    /// </summary>
    public static RectangleF operator +(RectangleF rectangle, Vector2 vector)
    {
        return new RectangleF(rectangle.X + vector.X, rectangle.Y + vector.Y, rectangle.Width, rectangle.Height);
    }

    /// <summary>
    ///     Sobrecarga el operador !=
    /// </summary>
    public static bool operator !=(RectangleF? left, RectangleF? right) => !(left == right);

    /// <summary>
    ///     Sobrescribe el método Equals(object)
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is null) 
            return false;
        else if (ReferenceEquals(this, obj)) 
            return true;
        else if (obj.GetType() != GetType()) 
            return false;
        else if (obj is RectangleF rectangle)
            return this == rectangle;
        else
            return false;
    }

    /// <summary>
    ///     Sobrescribe el método GetHashCode
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    /// <summary>
    ///     Sobrescribe el método ToString
    /// </summary>
    public override string ToString() => $"{nameof(RectangleF)} (X={X}, Y={Y}, Width={Width}, Height={Height})";

    /// <summary>
    ///     Convierte a <see cref="Rectangle"/>
    /// </summary>
    public Rectangle ToRectangle() => new((int) X, (int) Y, (int) Width, (int) Height);

    /// <summary>
    ///     Clona un <see cref="RectangleF"/>
    /// </summary>
	public RectangleF Clone() => new(X, Y, Width, Height);

	/// <summary>
	///     Coordenada X
	/// </summary>
	public float X { get; set; } = x;

    /// <summary>
    ///     Coordenada Y
    /// </summary>
    public float Y { get; set; } = y;

    /// <summary>
    ///     Ancho
    /// </summary>
    public float Width { get; set; } = width;

    /// <summary>
    ///     Altura
    /// </summary>
    public float Height { get; set; } = height;

    /// <summary>
    ///     Devuelve la posición superior / izquierda
    /// </summary>
    public Vector2 TopLeft => new(X, Y);

    /// <summary>
    ///     Posición izquierda
    /// </summary>
    public float Left => X;

    /// <summary>
    ///     Posición derecha
    /// </summary>
    public float Right => X + Width;

    /// <summary>
    ///     Posición superior
    /// </summary>
    public float Top => Y;

    /// <summary>
    ///     Posición inferior
    /// </summary>
    public float Bottom => Y + Height;

    /// <summary>
    ///     Centro del rectángulo
    /// </summary>
    public Vector2 Center => new Vector2(X + Width * 0.5f, Y + Height * 0.5f);

    /// <summary>
    ///     Comprueba si el rectángulo está vacío (no tiene tamaño)
    /// </summary>
    public bool IsEmpty => Width <= 0 || Height <= 0;
}