using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Emisors;

/// <summary>
///     Emisor desde un punto
/// </summary>
public class PointEmissorShape : AbstractEmissorShape
{
    /// <summary>
    ///     Obtiene la posición de emisión
    /// </summary>
    public override Vector2 GetEmissionPosition(Vector2 systemPosition) => systemPosition;
}