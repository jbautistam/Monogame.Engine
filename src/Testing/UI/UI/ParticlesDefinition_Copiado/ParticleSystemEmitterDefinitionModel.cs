using Microsoft.Xna.Framework;
using UI.CharactersEngine.ParticlesEngine.Intervals;
using UI.CharactersEngine.ParticlesEngine.Shapes;

namespace UI.ParticlesDefinition;

/// <summary>
///     Definición de la emisión de partículas
/// </summary>
public class ParticleSystemEmitterDefinitionModel(ParticleSystemEmitterDefinitionModel.EmitterShapeType shape)
{
    /// <summary>
    ///     Tipo de emisor
    /// </summary>
    public enum EmitterShapeType
    {
        /// <summary>Punto</summary>
        Point,
        /// <summary>Círculo</summary>
        Circle,
        /// <summary>Cono</summary>
        Cone,
        /// <summary>Línea</summary>
        Line,
        /// <summary>Ruta</summary>
        Path,
        /// <summary>Rectángulo</summary>
        Rectangle,
        /// <summary>Anillo</summary>
        Ring,
        /// <summary>Textura</summary>
        Texture
    }

    /// <summary>
    ///     Figura del emisor
    /// </summary>
    public EmitterShapeType Shape { get; } = shape;

    /// <summary>
    ///     Posición del emisor
    /// </summary>
    public Vector2 Position { get; set; }

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
    ///     Ubicación de la emisión
    /// </summary>
    public AbstractShapeEmitter.EmissionLocation Location { get; set; }

    /// <summary>
    ///     Dirección de emisión con respecto a la figura
    /// </summary>
    public AbstractShapeEmitter.EmissionDirectionMode DirectionMode { get; set; }

    /// <summary>
    ///     Vector de dirección fija (si la dirección es <see cref="AbstractShapeEmitter.EmissionDirectionMode.Fixed"/>
    /// </summary>
    public Vector2? FixedDirection { get; set; }

    /// <summary>
    ///     Parámetros del emisor
    /// </summary>
    public Dictionary<string, object> Parameters { get; } = new(StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    ///     Modificadores
    /// </summary>
    public List<ParticleSystemModifierDefinitionModel> Modifiers { get; } = [];
}
