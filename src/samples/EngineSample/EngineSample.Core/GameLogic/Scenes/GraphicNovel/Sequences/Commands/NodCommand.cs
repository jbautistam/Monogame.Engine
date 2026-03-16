using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

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
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        // Inicializa los datos
        if (!_initialized)
        {
            _start = actor.Transform.Rotation;
            _initialized = true;
        }
        // Asigna la nueva rotación
        actor.Transform.Rotation = _start + MathF.Sin(Progress * MathHelper.TwoPi * Nods) * Angle;
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
