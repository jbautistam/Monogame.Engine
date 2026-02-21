namespace MonoGame.UI;

/// <summary>
///     Espaciado interno / externo para un componente
/// </summary>
public struct Spacing
{
    public Spacing(float uniform)
    {
        Left = Top = Right = Bottom = uniform;
    }

    public Spacing(float horizontal, float vertical)
    {
        Left = Right = horizontal;
        Top = Bottom = vertical;
    }

    public Spacing(float left, float top, float right, float bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    /// <summary>
    ///     Espaciado izquierdo
    /// </summary>
    public float Left { get; }

    /// <summary>
    ///     Espaciado superior
    /// </summary>
    public float Top { get; }

    /// <summary>
    ///     Espaciado derecho
    /// </summary>
    public float Right { get; }

    /// <summary>
    ///     Espaciado inferior
    /// </summary>
    public float Bottom { get; }

    /// <summary>
    ///     Espaciado horizontal
    /// </summary>
    public float Horizontal => Left + Right;

    /// <summary>
    ///     Espaciado vertical
    /// </summary>
    public float Vertical => Top + Bottom;
}