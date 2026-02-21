using Microsoft.Xna.Framework;

namespace MonoGame.UI;

/// <summary>
///     Rectángulo en coordenadas relativas (0-1)
/// </summary>
public struct RectangleRelative(float x, float y, float width, float height)
{
    /// <summary>
    ///     Convierte las coordenadas relativas a absolutas
    /// </summary>
    public Rectangle ToAbsolute(int width, int height)
    {
        return new Rectangle((int) (X * width), (int) (Y * height), 
                             (int) (Width * width), (int) (Height * height));
    }

    /// <summary>
    ///     Aplica el espaciado interior a un rectángulo
    /// </summary>
    public RectangleRelative ApplySpacing(Spacing spacing)
    {
        return new RectangleRelative(X + spacing.Left * Width, Y + spacing.Top * Height, 
                                    Width * (1f - spacing.Horizontal), Height * (1f - spacing.Vertical));
    }

    /// <summary>
    ///     Coordenada X
    /// </summary>
    public float X { get; } = x;

    /// <summary>
    ///     Coordenada Y
    /// </summary>
    public float Y { get; } = y;

    /// <summary>
    ///     Anchura
    /// </summary>
    public float Width { get; } = width;

    /// <summary>
    ///     Altura
    /// </summary>
    public float Height { get; } = height;

    /// <summary>
    ///     Posición derecha
    /// </summary>
    public float Right => X + Width;

    /// <summary>
    ///     Posición inferior
    /// </summary>
    public float Bottom => Y + Height;

    /// <summary>
    ///     Posición de la coordenada superior izquierda
    /// </summary>
    public Vector2 Position => new(X, Y);

    /// <summary>
    ///     Tamaño
    /// </summary>
    public Vector2 Size => new(Width, Height);
}
