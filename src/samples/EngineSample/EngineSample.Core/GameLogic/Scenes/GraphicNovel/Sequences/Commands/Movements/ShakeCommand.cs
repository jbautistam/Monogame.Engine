using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Movements;

/// <summary>
///     Shake: sacudida con amortiguación
/// </summary>
public class ShakeCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Vector2 _start;
    private bool _initialized;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        float currentIntensity = Intensity * (1 - Progress);
        float angle = Progress * MathHelper.TwoPi * Oscillations;
        float offset = MathF.Sin(angle) * currentIntensity;
        Vector2 position;

            // Obtiene el valor inicial
            if (!_initialized)
            {
                _start = actor.Transform.Bounds.Location;
                _initialized = true;
            }
            // Cambia la posición en horizontal y en vertical (con menos intensidad en la vertical)
            position = _start;
            if (Horizontal) 
                position.X += offset;
            if (Vertical) 
                position.Y += offset * 0.5f; // Menos intensidad vertical
            // Cambia la posición del actor
            actor.Transform.Bounds.Location = position;
    }

    /// <summary>
    ///     Intensidad del movimiento
    /// </summary>
    public float Intensity { get; set; } = 2;

    /// <summary>
    ///     Número de oscilaciones
    /// </summary>
    public int Oscillations { get; set; } = 3;

    /// <summary>
    ///     Indica si se aplica en el eje horizontal
    /// </summary>
    public bool Horizontal { get; set; } = true;

    /// <summary>
    ///     Indica si se aplica en el eje vertical
    /// </summary>
    public bool Vertical { get; set; } = true;
}
