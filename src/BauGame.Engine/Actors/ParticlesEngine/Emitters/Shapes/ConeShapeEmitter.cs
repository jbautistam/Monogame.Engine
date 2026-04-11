using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.ParticlesEngine.Emitters.Shapes;

/// <summary>
///     Figura de tipo cono para emisión
/// </summary>
public class ConeShapeEmitter(float radius, float startAngle, float endAngle) : AbstractShapeEmitter
{
    /// <summary>
    ///     Clona el objeto
    /// </summary>
	public override AbstractShapeEmitter Clone()
	{
		return new ConeShapeEmitter(Radius, StartAngle, EndAngle)
                        {
                            EmissionLocation = EmissionLocation,
                            EmissionDirection = EmissionDirection
                        };
	}

    /// <summary>
    ///     Obtiene los datos de la siguiente emisión
    /// </summary>
    public override EmissionData GetEmissionData(EmissionLocationMode location, EmissionDirectionMode directionMode, Vector2? fixedDirection)
    {
        Vector2 position;
        float angle = 0f;
        Vector2 normal = Vector2.Zero;
        float angleRange = EndAngle - StartAngle;

            // Calcula la posición sobre el borde o sobre la superficie
            if (location == EmissionLocationMode.Border)
            {
                switch (Tools.Randomizer.Random.Next(3))
                {
                    case 0: // arco exterior
                            // Obtiene la posición (utiliza angle como valor aleatorio temporal)
                            angle = (float) (Tools.Randomizer.Random.NextDouble() * angleRange + StartAngle);
                            position = new Vector2((float) Math.Cos(angle) * Radius, (float)Math.Sin(angle) * Radius);
                            // Obtiene la normal: radio hacia afuera
                            normal = Vector2.Normalize(position);
                        break;
                    case 1: // línea del ángulo inicial
                            // Obtiene la posición (utiliza angle como valor aleatorio temporal)
                            angle = (float) (Tools.Randomizer.Random.NextDouble() * Radius);
                            position = new Vector2((float) Math.Cos(StartAngle) * angle, (float) Math.Sin(StartAngle) * angle);
                            // Normal perpendicular a la línea del radio
                            normal = new Vector2(-(float) Math.Sin(StartAngle), (float) Math.Cos(StartAngle));
                        break;
                    default: // línea del ángulo final
                            // Obtiene la posición (utiliza angle como valor aleatorio temporal)
                            angle = (float) (Tools.Randomizer.Random.NextDouble() * Radius);
                            position = new Vector2((float) Math.Cos(EndAngle) * angle, (float) Math.Sin(EndAngle) * angle);
                            // Normal perpendicular a la línea del radio
                            normal = new Vector2(-(float) Math.Sin(EndAngle), (float) Math.Cos(EndAngle));
                        break;
                }
            }
            else
            {
                float radius = Radius * (float) Math.Sqrt(Tools.Randomizer.Random.NextDouble());

                    // Calcula una posición en un ángulo aleatorio
                    angle = (float) (Tools.Randomizer.Random.NextDouble() * angleRange + StartAngle);
                    position = new Vector2((float) Math.Cos(angle) * radius, (float) Math.Sin(angle) * radius);
                    // La normal es hacia afuera
                    normal = Vector2.Normalize(position);
                    if (normal == Vector2.Zero) 
                        normal = Vector2.UnitY;
            }
            // Devuelve los datos de emisión
            return new EmissionData(position, ResolveDirection(directionMode, normal, fixedDirection));
    }

    /// <summary>
    ///     Radio del cono
    /// </summary>
    public float Radius { get; set; } = radius;

    /// <summary>
    ///     Ángulo de inicio del cono (radianes)
    /// </summary>
    public float StartAngle { get; set; } = startAngle;

    /// <summary>
    ///     Ángulo de fin del cono (radianes)
    /// </summary>
    public float EndAngle { get; set; } = endAngle;
}
