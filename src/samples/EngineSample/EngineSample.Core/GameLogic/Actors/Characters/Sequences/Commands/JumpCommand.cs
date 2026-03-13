using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

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
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        float t = Progress;
        Vector2 horizontal;

            // Recoge el valor inicial
            if (!_initialized)
            {
                _start = actor.Transform.Bounds.TopLeft;
                _initialized = true;
            }
            // Calcula la nueva posición horizontal
            horizontal = EasingFunctionsHelper.Interpolate(_start, new Vector2(_start.X + OffsetX, _start.Y), t, Easing);
            // Cambia la posición
            actor.Transform.Bounds.TopLeft = new Vector2(horizontal.X, horizontal.Y - 4 * t * (1 - t) * Height);
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
