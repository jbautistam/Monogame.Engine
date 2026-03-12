using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.ParticlesEngine.Shapes;

/// <summary>
///     Emisor a partir de una ruta
/// </summary>
public class PathEmitter : AbstractShapeEmitter
{
    /// <summary>
    ///     Calcula los datos de emisión
    /// </summary>
    public override EmissionData GetEmissionData(EmissionLocation location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        if (Points.Count == 0)
            return new EmissionData(Vector2.Zero, Vector2.UnitY);
        else
        {
            int segmentIndex = Tools.Randomizer.Random.Next(Points.Count - 1);
            Vector2 p1 = Points[segmentIndex];
            Vector2 p2 = Points[segmentIndex + 1];
            Vector2 tangent = Vector2.Normalize(p2 - p1);
            Vector2 normal = new(-tangent.Y, tangent.X);

                // Devuelve los datos
                return new EmissionData(Vector2.Lerp(p1, p2, (float) Tools.Randomizer.Random.NextDouble()), 
                                        ResolveDirection(directionMode, normal, fixedDirection));
        }
    }

    /// <summary>
    ///     Puntos de la ruta
    /// </summary>
    public List<Vector2> Points { get; } = [];
}
