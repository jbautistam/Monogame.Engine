using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Actors.Particles.Emisors;

/// <summary>
///		Clase base para las figuras de emisión
/// </summary>
public abstract class AbstractEmissionShape
{
    public abstract Vector2 GetEmissionPosition();
    public Vector2 Position;
}
