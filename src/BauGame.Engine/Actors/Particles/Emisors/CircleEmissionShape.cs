using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Emisors;

public class CircleEmissionShape : AbstractEmissionShape
{
    public float Radius = 50f;
    public bool EdgeOnly = false;

    public override Vector2 GetEmissionPosition()
    {
        float angle = (float)(Tools.Randomizer.Random.NextDouble() * Math.PI * 2);
        float radius = EdgeOnly ? Radius : (float)(Tools.Randomizer.Random.NextDouble() * Radius);
        return Position + new Vector2(
            (float)Math.Cos(angle) * radius,
            (float)Math.Sin(angle) * radius
        );
    }
}