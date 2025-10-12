using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Particles.Emisors;

public class PointEmissionShape : AbstractEmissionShape
{
    public override Vector2 GetEmissionPosition() => Position;
}