using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine.Shapes;

/// <summary>
///     Emisor a partir de una ruta
/// </summary>
public class PathEmitter : AbstractShapeEmitter
{
    /// <summary>
    ///     Calcula los datos de emisión
    /// </summary>
    public override EmissionData GetEmissionData(Random random, EmissionLocation location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        if (Points.Count == 0)
            return new EmissionData(Vector2.Zero, Vector2.UnitY);
        else
        {
            int segmentIndex = random.Next(Points.Count - 1);
            Vector2 p1 = Points[segmentIndex];
            Vector2 p2 = Points[segmentIndex + 1];
            Vector2 tangent = Vector2.Normalize(p2 - p1);
            Vector2 normal = new(-tangent.Y, tangent.X);

                // Devuelve los datos
                return new EmissionData(Vector2.Lerp(p1, p2, (float) random.NextDouble()), ResolveDirection(directionMode, normal, fixedDirection, random));
        }
    }

    /// <summary>
    ///     Puntos de la ruta
    /// </summary>
    public List<Vector2> Points { get; } = [];
}
