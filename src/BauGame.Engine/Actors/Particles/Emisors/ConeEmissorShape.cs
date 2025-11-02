using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Emisors;

/// <summary>
///     Emisor representado en un cono
/// </summary>
public class ConeEmissorShape : AbstractEmissorShape
{
    /// <summary>
    ///     Obtiene la posición de emisión
    /// </summary>
    public override Vector2 GetEmissionPosition()
    {
        float randomAngle = (float) (Tools.Randomizer.Random.NextDouble() * Angle) - 0.5f * Angle;
        float length = EdgeOnly ? Length : (float) (Tools.Randomizer.Random.NextDouble() * Length);
        
            return Position + new Vector2((float) Math.Cos(randomAngle) * length, (float) Math.Sin(randomAngle) * length);
    }

    /// <summary>
    ///     Angulo de emisión del cono
    /// </summary>
    public float Angle { get; set; } = MathHelper.PiOver4; // 45 grados

    /// <summary>
    ///     Longitud del cono
    /// </summary>
    public float Length { get; set; }= 100f;

    /// <summary>
    ///     Indica si se debe emitir sólo en el borde
    /// </summary>
    public bool EdgeOnly { get; set; }
}