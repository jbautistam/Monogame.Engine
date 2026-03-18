using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Scenes.Layers.Backgrounds;

/// <summary>
///     Parámetros del fondo dinámico
/// </summary>
public class BackgroundState
{
    /// <summary>
    ///     Combina los datos de este estado con otro
    /// </summary>
    public BackgroundState Combine(BackgroundState delta)
    {
        return new BackgroundState
                        {
                            ViewCenter = ViewCenter + delta.ViewCenter,
                            Zoom = Zoom * delta.Zoom,
                            ScreenOffset = ScreenOffset + delta.ScreenOffset,
                            Rotation = Rotation + delta.Rotation,
                            Tint = new Color((int) (Tint.R * delta.Tint.R / 255f),
                                             (int) (Tint.G * delta.Tint.G / 255f),
                                             (int) (Tint.B * delta.Tint.B / 255f),
                                             255)
                        };
    }

    /// <summary>
    ///     Centro de la vista
    /// </summary>
    public Vector2 ViewCenter { get; set; } = Vector2.Zero;

    /// <summary>
    ///     Zoom de la vista
    /// </summary>
    public float Zoom { get; set; } = 1;

    /// <summary>
    ///     Offset en la pantalla
    /// </summary>
    public Vector2 ScreenOffset { get; set; } = Vector2.Zero;

    /// <summary>
    ///     Rotación
    /// </summary>
    public float Rotation { get; set; } = 0;

    /// <summary>
    ///     Color
    /// </summary>
    public Color Tint { get; set; } = Color.White;
}
