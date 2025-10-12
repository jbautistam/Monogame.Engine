using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Actors.Particles.Emisors;

public class PointEmissionShape : AbstractEmissionShape
{
    public override Vector2 GetEmissionPosition() => Position;
}