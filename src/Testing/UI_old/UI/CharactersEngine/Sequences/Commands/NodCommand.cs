using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Nod - asentimiento (rotación oscilante)
/// </summary>
public class NodCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private float _start;
    private bool _initialized;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    public override void Apply(Actor actor, float sequenceTime)
    {
        // Inicializa los datos
        if (!_initialized)
        {
            _start = actor.Rotation;
            _initialized = true;
        }
        // Asigna la nueva rotación
        actor.Rotation = _start + MathF.Sin(GetProgress(sequenceTime) * MathHelper.TwoPi * Nods) * Angle;
    }

    /// <summary>
    ///     Ángulo máximo de inclinación
    /// </summary>
    public float Angle { get; set; } = 10;

    /// <summary>
    ///     Número de rotaciones
    /// </summary>
    public int Nods { get; set; } = 2;
}
