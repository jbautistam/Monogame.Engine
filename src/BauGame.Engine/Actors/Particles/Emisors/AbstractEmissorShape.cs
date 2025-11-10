using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Emisors;

/// <summary>
///		Clase base para los emisores de partículas
/// </summary>
public abstract class AbstractEmissorShape
{
    /// <summary>
    ///     Obtiene la posición de emisión
    /// </summary>
    public abstract Vector2 GetEmissionPosition(Vector2 systemPosition);
}
