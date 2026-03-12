using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.ParticlesEngine.Shapes;

/// <summary>
///     Emisor desde una línea
/// </summary>
public class LineShapeEmitter(float length, float rotation = 0f) :  AbstractShapeEmitter
{
    /// <summary>
    ///     Obtiene los datos de emisión
    /// </summary>
    public override EmissionData GetEmissionData(EmissionLocation location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        Vector2 position;
        Vector2 normal = Vector2.Zero;

            // Obtiene la posición dependiendo de si es sobre el límite o en el interior
            if (location == EmissionLocation.Border)
            {
                int sign = Tools.Randomizer.Random.Next(2) > 1 ? -1 : 1;

                    // Crea la posición sobre uno de los extremos
                    position = new Vector2(sign * Length * 0.5f, 0);
                    // La normal en los bordes de una línea es perpendicular a la línea
                    normal = Vector2.UnitY;
            }
            else // Si es en el interior, recoge un punto aleatorio a lo largo de la línea
            {
                // Calcula la posición como un punto en la línea centrado en el 0
                position = new Vector2((float) (Tools.Randomizer.Random.NextDouble() * Length) - (0.5f * Length), 0);
                // Normal perpendicular a la línea
                normal = Vector2.UnitY;
            }
            // Aplica la rotación a la posición y la normal
            if (Rotation != 0)
            {
                position = Vector2.Transform(position, Matrix.CreateRotationZ(Rotation));
                normal = Vector2.Transform(normal, Matrix.CreateRotationZ(Rotation));
            }
            // Devuelve los datos
            return new EmissionData(position, ResolveDirection(directionMode, normal, fixedDirection));
    }

    /// <summary>
    ///     Longitud de la línea
    /// </summary>
    public float Length { get; set; } = length;

    /// <summary>
    ///     Rotación en radianes
    /// </summary>
    public float Rotation { get; set; } = rotation; // Radianes
}
