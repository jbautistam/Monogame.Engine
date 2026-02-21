using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Datos del margen
/// </summary>
public struct UiMargin
{
    public UiMargin(int all)
    {
        Left = Top = Right = Bottom = all;
    }

    public UiMargin(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    /// <summary>
    ///     Aplica el margen a un rectángulo
    /// </summary>
    public Rectangle Apply(Rectangle bounds)
    {
        return new Rectangle(bounds.X + Left, bounds.Y + Top,
                             Math.Max(0, bounds.Width - Horizontal),
                             Math.Max(0, bounds.Height - Vertical));
    }

    /// <summary>
    ///     Margen izquierdo
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    ///     Margen superior
    /// </summary>
    public int Top { get; set; }

    /// <summary>
    ///     Margen izquierdo
    /// </summary>
    public int Right { get; set; }

    /// <summary>
    ///     Margen superior
    /// </summary>
    public int Bottom { get; set; }

    /// <summary>
    ///     Espaciado horizontal
    /// </summary>
    public int Horizontal => Left + Right;

    /// <summary>
    ///     Espaciado vertical
    /// </summary>
    public int Vertical => Top + Bottom;
}
