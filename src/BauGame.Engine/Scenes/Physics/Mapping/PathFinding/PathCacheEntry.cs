using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping.PathFinding;

/// <summary>
///     Entrada de una ruta en la caché
/// </summary>
public class PathCacheEntry(List<Point> points, int currentFrame)
{
    /// <summary>
    ///     Indica que se ha tocado el elemento de la caché
    /// </summary>
    public void Touch(int currentFrame)
    {
        LastAccessFrame = currentFrame;
        AccessCount++;
    }

    /// <summary>
    ///     Puntos de la ruta
    /// </summary>
    public List<Point> GridPath { get; } = points;

    /// <summary>
    ///     Frame del último acceso
    /// </summary>
    public int LastAccessFrame { get; private set; } = currentFrame;

    /// <summary>
    ///     Número de accesos
    /// </summary>
    public int AccessCount { get; private set; } = 1;
}
