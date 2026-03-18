using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Movements;

/// <summary>
///     Comando para un movimiento en forma de péndulo
/// </summary>
public class PendulumCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private float _startRotation;
    private bool _initialized;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        float currentOscillation = Progress * Oscillations;
        float angle = EndAngle * MathF.Cos(currentOscillation * MathHelper.TwoPi);

            // Inicializa los datos
            if (!_initialized)
            {
                // Obtiene los datos iniciales
                _startRotation = actor.Transform.Rotation;
                // Indica que ya se ha inicializado
                _initialized = true;
            }
            // Aplica la amortiguación al ángulo
            angle *= MathF.Pow(Damping, currentOscillation);
            // Cambia los datos
            actor.Transform.Bounds.Location = new Vector2(Pivot.X + MathF.Sin(angle) * Length, Pivot.Y + MathF.Cos(angle) * Length);
            actor.Transform.Rotation = _startRotation + angle;
    }

    /// <summary>
    ///     Pivote de la rotación
    /// </summary>
    public required Vector2 Pivot { get; init; }

    /// <summary>
    ///     Longitud del péndulo
    /// </summary>
    public required float Length { get; init; }

    /// <summary>
    ///     Angulo final del péndulo
    /// </summary>
    public required float EndAngle { get; init; }

    /// <summary>
    ///     Número de oscilaciones
    /// </summary>
    public required int Oscillations { get; init; }

    /// <summary>
    ///     Amortiguación
    /// </summary>
    public required float Damping { get; init; }
}
