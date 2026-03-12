using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine.Shapes;

/// <summary>
///     Figura de emisión sobre un rectángulo
/// </summary>
public class RectangleShapeEmitter(float width, float height) : AbstractShapeEmitter
{
    /// <summary>
    ///     Obtiene los datos de emisión de la siguiente partícula
    /// </summary>
    public override EmissionData GetEmissionData(Random random, EmissionLocation location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        Vector2 position;
        Vector2 normal = Vector2.Zero;
        float halfWidth = 0.5f * Width;
        float halfHeight = 0.5f * Height;

            // Calcula la posición y la normal dependiendo de si se debe emitir sobre el borde o sobre la superficie
            if (location == EmissionLocation.Border)
                switch (random.Next(0, 4))
                {
                    case 0: // Lado inferior
                            position = new Vector2((float) (random.NextDouble() * Width) - halfWidth, halfHeight);
                            normal = Vector2.UnitY;
                        break;
                    case 1: // Lado derecho
                            position = new Vector2(halfWidth, (float) (random.NextDouble() * Height) - halfHeight);
                            normal = Vector2.UnitX;
                        break;
                    case 2: // Lado superior
                            position = new Vector2((float) (random.NextDouble() * Width) - halfWidth, -halfHeight);
                            normal = -Vector2.UnitY;
                        break;
                    default: // Lado izquierdo
                            position = new Vector2(-halfWidth, (float) (random.NextDouble() * Height) - halfHeight);
                            normal = -Vector2.UnitX;
                        break;
                }
            else
            {
                // Calcula una posición aleatoria dentro del rectángulo
                position = new Vector2((float) (random.NextDouble() * Width) - halfWidth, 
                                       (float) (random.NextDouble() * Height) - halfHeight);
                // En superficie, la normal por defecto es hacia arriba
                normal = Vector2.UnitY; 
            }
            // Devuelve los datos de emisión
            return new EmissionData(position, ResolveDirection(directionMode, normal, fixedDirection, random));
    }

    /// <summary>
    ///     Ancho del rectángulo
    /// </summary>
    public float Width { get; set; } = width;

    /// <summary>
    ///     Altura del rectángulo
    /// </summary>
    public float Height { get; set; } = height;
}
