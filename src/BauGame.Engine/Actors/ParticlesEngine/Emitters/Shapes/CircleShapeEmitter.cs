using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.ParticlesEngine.Emitters.Shapes;

/// <summary>
///     Emisor sobre un círculo
/// </summary>
public class CircleShapeEmitter(float radius) : AbstractShapeEmitter
{
    /// <summary>
    ///     Clona el objeto
    /// </summary>
	public override AbstractShapeEmitter Clone()
	{
		return new CircleShapeEmitter(Radius)
                        {
                            EmissionLocation = EmissionLocation,
                            EmissionDirection = EmissionDirection
                        };
	}

    /// <summary>
    ///     Obtiene los datos de emisión de una partícula
    /// </summary>
    public override EmissionData GetEmissionData(EmissionLocationMode location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        Vector2 position = GetPosition();

            // Devuelve los datos de la emisión
            return new EmissionData(position, GetDirection(position));

        // Obtiene la ubicación de la partícula
        Vector2 GetPosition()
        {
            float angle = (float) (Tools.Randomizer.Random.NextDouble() * MathHelper.TwoPi);

                if (location == EmissionLocationMode.Border)
                    return new Vector2((float) Math.Cos(angle) * Radius, (float) Math.Sin(angle) * Radius);
                else
                {
                    float radius = Radius * (float) Math.Sqrt(Tools.Randomizer.Random.NextDouble());
            
                        // Utiliza Sqrt para una distribución uniforme y que no se agolpen las partículas en el centro
                        return new Vector2((float) Math.Cos(angle) * radius, (float) Math.Sin(angle) * radius);
                }
        }

        // Obtiene la dirección de lanzamiento de la partícula
        Vector2 GetDirection(Vector2 position)
        {
            Vector2 normal = Vector2.Normalize(position);

                // En un círculo, la normal en el borde es simplemente el vector posición normalizado
                if (normal == Vector2.Zero) 
                    normal = Vector2.UnitY; // estamos en el centro exacto
                // Calcula la dirección
                return ResolveDirection(directionMode, normal, fixedDirection);
        }
    }

    /// <summary>
    ///     Radio del círculo
    /// </summary>
    public float Radius { get; set; } = radius;
}
