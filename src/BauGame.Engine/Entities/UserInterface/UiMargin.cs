namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Datos del margen
/// </summary>
public struct UiMargin
{
    public UiMargin(float all)
    {
        Left = Top = Right = Bottom = all;
    }

    public UiMargin(float left, float top, float right, float bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    /// <summary>
    ///     Margen izquierdo
    /// </summary>
    public float Left { get; set; }

    /// <summary>
    ///     Margen superior
    /// </summary>
    public float Top { get; set; }

    /// <summary>
    ///     Margen izquierdo
    /// </summary>
    public float Right { get; set; }

    /// <summary>
    ///     Margen superior
    /// </summary>
    public float Bottom { get; set; }
}
