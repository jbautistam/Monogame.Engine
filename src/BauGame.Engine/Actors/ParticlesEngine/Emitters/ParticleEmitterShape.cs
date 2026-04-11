using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.ParticlesEngine.Emitters;

/// <summary>
///		Datos de la forma de emisión de partículas
/// </summary>
public class ParticleEmitterShape
{
    /// <summary>
    ///     Clona el objeto
    /// </summary>
	public ParticleEmitterShape Clone()
	{
		ParticleEmitterShape cloned = new()
                                        {
                                            Shape = Shape.Clone()
                                        };

            // Asigna las propiedades
            cloned.Location = Location;
            cloned.DirectionMode = DirectionMode;
            if (FixedDirection is not null)
                cloned.FixedDirection = new Vector2(FixedDirection?.X ?? 0, FixedDirection?.Y ?? 0);
            // Devuelve el objeto clonado
            return cloned;
	}

    /// <summary>
    ///     Figura de emisión
    /// </summary>
    public required Shapes.AbstractShapeEmitter Shape { get; init; }

    /// <summary>
    ///     Ubicación de la emisión
    /// </summary>
    public Shapes.AbstractShapeEmitter.EmissionLocationMode Location { get; set; }

    /// <summary>
    ///     Dirección de emisión con respecto a la figura
    /// </summary>
    public Shapes.AbstractShapeEmitter.EmissionDirectionMode DirectionMode { get; set; }

    /// <summary>
    ///     Vector de dirección fija (si la dirección es <see cref="Shapes.AbstractShapeEmitter.EmissionDirectionMode.Fixed"/>
    /// </summary>
    public Vector2? FixedDirection { get; set; }
}