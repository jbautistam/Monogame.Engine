using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine.Shapes;

/// <summary>
///     Emisión a partir de un punto
/// </summary>
public class PointShapeEmitter(float spread = 0) : AbstractShapeEmitter
{
    /// <summary>
    ///     Obtiene el punto de emisión
    /// </summary>
    public override EmissionData GetEmissionData(Random random, EmissionLocation location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        Vector2 position = Vector2.Zero;

            // Añadimos la variacion al punto si es necesario
            if (Spread > 0)
            {
                float angle = (float) (random.NextDouble() * MathHelper.TwoPi);
                float radius = Spread * (float) Math.Sqrt(random.NextDouble());
            
                    // Calcula la posición
                    position = new Vector2((float) Math.Cos(angle) * radius, (float) Math.Sin(angle) * radius);
            }
            // Devuelve los datos de emisión
            return new EmissionData(position, ResolveDirection(directionMode, Vector2.UnitY, fixedDirection, random));
    }

    /// <summary>
    ///     Variación para evitar que todas las partículas se creen sobre el punto exacto
    /// </summary>
    public float Spread { get; set; } = spread;
}
