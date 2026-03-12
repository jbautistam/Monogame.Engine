using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine.Shapes;

/// <summary>
///     Figura de emisión de tipo anillo
/// </summary>
public class RingShapeEmitter(float innerRadius, float outerRadius) : AbstractShapeEmitter
{
    /// <summary>
    ///     Obtiene los siguientes puntos de emisión
    /// </summary>
    public override EmissionData GetEmissionData(Random random, EmissionLocation location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        Vector2 position;
        float angle = angle = (float)(random.NextDouble() * MathHelper.TwoPi);
        Vector2 normal = Vector2.Zero;

            // Dependiendo del modo de emisión
            if (location == EmissionLocation.Border)
            {
                float radius = OuterRadius;
            
                    // Lanzamos el dado, si sale cruz, cogemos el radio interno
                    if (random.NextDouble() > 0.5f)
                        radius = InnerRadius;
                    // Calcula la posición
                    position = new Vector2((float) Math.Cos(angle) * radius, (float) Math.Sin(angle) * radius);
                    // La normal en el borde de un anillo es radial (desde el centro)
                    normal = Vector2.Normalize(position);
            }
            else
            {
                float innerSq = InnerRadius * InnerRadius;
                float outerSq = OuterRadius * OuterRadius;
                float randomArea = (float) (random.NextDouble() * (outerSq - innerSq) + innerSq);
                float radius = (float) Math.Sqrt(randomArea);

                    // Obtiene la posición            
                    // En superficie se utiliza una distribución uniforme en el área del anillo
                    // Fórmula: r = sqrt(random * (R² - r²) + r²)
                    position = new Vector2((float) Math.Cos(angle) * radius, (float) Math.Sin(angle) * radius);
                    // En superficie, la normal promedio es radial hacia afuera
                    normal = Vector2.Normalize(position);
            }
            // En caso que estemos justamente en el centro
            if (normal == Vector2.Zero) 
                normal = Vector2.UnitY;
            // Devuelve el punto de emisión
            return new EmissionData(position, ResolveDirection(directionMode, normal, fixedDirection, random));
    }

    /// <summary>
    ///     Radio interno
    /// </summary>
    public float InnerRadius { get; set; } = innerRadius;

    /// <summary>
    ///     Radio externo
    /// </summary>
    public float OuterRadius { get; set; } = outerRadius;
}
