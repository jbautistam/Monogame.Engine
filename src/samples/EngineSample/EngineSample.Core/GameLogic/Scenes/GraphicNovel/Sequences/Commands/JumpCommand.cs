using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

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
        Vector2 target = ToWorld(new Vector2(OffsetX, Height));
        Vector2 horizontal;

            // Recoge el valor inicial
            if (!_initialized)
            {
                _start = actor.Transform.Bounds.Location;
                _initialized = true;
            }
            // Calcula la nueva posición horizontal
            horizontal = EasingFunctionsHelper.Interpolate(_start, new Vector2(_start.X + target.X, _start.Y), t, Easing);
            // Cambia la posición
            actor.Transform.Bounds.Location = new Vector2(horizontal.X, horizontal.Y - 4 * t * (1 - t) * target.Y);
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
