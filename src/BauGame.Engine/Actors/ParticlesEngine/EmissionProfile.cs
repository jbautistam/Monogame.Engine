using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Intervals;

namespace Bau.Libraries.BauGame.Engine.Actors.ParticlesEngine;

/// <summary>
///     Perfil de emisión de partículas
/// </summary>
public class EmissionProfile
{
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
    ///     Número de partículas por segundo
    /// </summary>
    public FloatRange EmissionRate { get; set; }

    /// <summary>
    ///     Segundos de vida de la partícula (ej: 1.0 a 3.0)
    /// </summary>
    public FloatRange Lifetime { get; set; }
    
    /// <summary>
    ///     Tamaño inicial (ej: 0.5 a 1.5)
    /// </summary>
    public FloatRange StartScale { get; set; }

    /// <summary>
    ///     Tamaño final (para efecto de desvanecimiento)
    /// </summary>
    public FloatRange EndScale { get; set; }

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
    ///     Color
    /// </summary>
    public ColorRange Color { get; set; }

    /// <summary>
    ///     Figura de emisión
    /// </summary>
    public required Shapes.AbstractShapeEmitter Shape { get; init; } 

    /// <summary>
    ///     Ubicación de la emisión
    /// </summary>
    public Shapes.AbstractShapeEmitter.EmissionLocation Location { get; set; }

    /// <summary>
    ///     Dirección de emisión con respecto a la figura
    /// </summary>
    public Shapes.AbstractShapeEmitter.EmissionDirectionMode DirectionMode { get; set; }

    /// <summary>
    ///     Vector de dirección fija (si la dirección es <see cref="Shapes.AbstractShapeEmitter.EmissionDirectionMode.Fixed"/>
    /// </summary>
    public Vector2? FixedDirection { get; set; }

    /// <summary>
    ///     Modificadores
    /// </summary>
    public List<Modifiers.AbstractParticleModifier> Modifiers { get; } = [];
}
