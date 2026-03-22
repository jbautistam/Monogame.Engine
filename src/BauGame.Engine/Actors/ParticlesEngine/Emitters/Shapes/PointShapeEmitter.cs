using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.ParticlesEngine.Emitters.Shapes;

/// <summary>
///     Emisión a partir de un punto
/// </summary>
public class PointShapeEmitter(float spread = 0) : AbstractShapeEmitter
{
    /// <summary>
    ///     Obtiene el punto de emisión
    /// </summary>
    public override EmissionData GetEmissionData(EmissionLocationMode location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        Vector2 position = Vector2.Zero;

            // Añadimos la variacion al punto si es necesario
            if (Spread > 0)
            {
                float angle = (float) (Tools.Randomizer.Random.NextDouble() * MathHelper.TwoPi);
                float radius = Spread * (float) Math.Sqrt(Tools.Randomizer.Random.NextDouble());
            
                    // Calcula la posición
                    position = new Vector2((float) Math.Cos(angle) * radius, (float) Math.Sin(angle) * radius);
            }
            // Devuelve los datos de emisión
            return new EmissionData(position, ResolveDirection(directionMode, Vector2.UnitY, fixedDirection));
    }

    /// <summary>
    ///     Variación para evitar que todas las partículas se creen sobre el punto exacto
    /// </summary>
    public float Spread { get; set; } = spread;
}
