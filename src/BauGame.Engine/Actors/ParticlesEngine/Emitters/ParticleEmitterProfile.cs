using Bau.BauEngine.Tools.MathTools.Intervals;

namespace Bau.BauEngine.Actors.ParticlesEngine.Emitters;

/// <summary>
///     Perfil de emisión de partículas
/// </summary>
public class ParticleEmitterProfile
{
    /// <summary>
    ///     Clona el objeto
    /// </summary>
	public ParticleEmitterProfile Clone()
	{
		ParticleEmitterProfile cloned = new();

			// Asigna las propiedades
			cloned.MaximumParticles = MaximumParticles;
			cloned.Start = Start;
			cloned.Duration = Duration;
			cloned.EmissionRate = EmissionRate;
			cloned.ParticlesPerEmission = ParticlesPerEmission;
			cloned.Lifetime = Lifetime;
			cloned.Scale = Scale;
			cloned.Rotation = Rotation;
			cloned.RotationSpeed = RotationSpeed;
			cloned.Speed = Speed;
			cloned.Color = Color;
			cloned.Opacity = Opacity;
            // Asigna los datos del sprite
            if (Sprite is not null)
			    cloned.Sprite = Sprite.Clone();
			// Devuelve el perfil
			return cloned;
	}

    /// <summary>
    ///     Número máximo de partículas
    /// </summary>
    public int MaximumParticles { get; set; } = 10_000;
    
    /// <summary>
    ///     Momento de inicio de la emisión
    /// </summary>
    public float Start { get; set; }

    /// <summary>
    ///     Duración de la emisión si es temporal o 0 para infinito
    /// </summary>
    public float Duration { get; set; }

    /// <summary>
    ///     Ratio de emisión
    /// </summary>
    public FloatRange EmissionRate { get; set; }

    /// <summary>
    ///     Número de partículas por emisión
    /// </summary>
    public IntRange ParticlesPerEmission { get; set; }

    /// <summary>
    ///     Segundos de vida de la partícula (ej: 1.0 a 3.0)
    /// </summary>
    public FloatRange Lifetime { get; set; }
    
    /// <summary>
    ///     Escala inicial (ej: 0.5 a 1.5)
    /// </summary>
    public FloatRange Scale { get; set; }

    /// <summary>
    ///     Rotación en radianes (ej: 0 a 6.28)
    /// </summary>
    public FloatRange Rotation { get; set; }
    
    /// <summary>
    ///     Velocidad de rotación en radianes por segundo
    /// </summary>
    public FloatRange RotationSpeed { get; set; }
        
    /// <summary>
    ///     Magnitud del vector de velocidad
    /// </summary>
    public FloatRange Speed { get; set; }
    
    /// <summary>
    ///     Sprite
    /// </summary>
    public Entities.Sprites.AbstractSpriteDefinition? Sprite { get; set; }

    /// <summary>
    ///     Color
    /// </summary>
    public ColorRange Color { get; set; }

    /// <summary>
    ///     Opacidad
    /// </summary>
    public FloatRange Opacity { get; set; }
}
