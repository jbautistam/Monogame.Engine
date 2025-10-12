using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Actors.Particles.Emisors;

public class ConeEmissionShape : AbstractEmissionShape
{
    public float Angle = MathHelper.PiOver4; // 45 grados
    public float Length = 100f;
    public bool EdgeOnly = false;

    public override Vector2 GetEmissionPosition()
    {
        float randomAngle = (float)(Tools.Randomizer.Random.NextDouble() * Angle) - Angle/2;
        float length = EdgeOnly ? Length : (float)(Tools.Randomizer.Random.NextDouble() * Length);
        
        return Position + new Vector2(
            (float)Math.Cos(randomAngle) * length,
            (float)Math.Sin(randomAngle) * length
        );
    }
}