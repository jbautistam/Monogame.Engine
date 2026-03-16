using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Jump - salto arqueado (funciona mejor con <see cref="MathTools.Easing.EasingFunctionsHelper.EasingType.QuadOut"/> )
/// </summary>
public class JumpCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private bool _initialized;
    private Vector2 _start;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    public override void Apply(Actor actor, float sequenceTime)
    {
        float t = GetProgress(sequenceTime);
        Vector2 horizontal;

            // Recoge el valor inicial
            if (!_initialized)
            {
                _start = actor.Position;
                _initialized = true;
            }
            // Calcula la nueva posición horizontal
            horizontal = Interpolate(_start, new Vector2(_start.X + OffsetX, _start.Y), sequenceTime);
            // Cambia la posición
            actor.Position = new Vector2(horizontal.X, horizontal.Y - 4 * t * (1 - t) * Height);
    }

    /// <summary>
    ///     Altura del salto
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    ///     Destino del salto
    /// </summary>
    public float OffsetX { get; set; }
}
