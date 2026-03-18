using Microsoft.Xna.Framework;
using Bau.BauEngine.Actors.Components.Transforms;
using Bau.BauEngine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Movements;

/// <summary>
///     Rotación
/// </summary>
public class RotateCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private float _startRotation, _targetRotation;
    private bool _initialized;

    /// <summary>
    ///     Aplica el comando
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        // Recoge los valores iniciales
        if (!_initialized)
        {
            // Guarda los valores del actor
            _startRotation = actor.Transform.Rotation;
            _targetRotation = MathHelper.ToRadians(Rotation);
            // Indica que se ha inicializado
            _initialized = true;
        }
        // Actualiza los datos
        actor.Transform.OriginPoint = OriginPoint;
        if (Origin is not null)
            actor.Transform.Center = Origin ?? Vector2.Zero;
        actor.Transform.Rotation = EasingFunctionsHelper.Interpolate(_startRotation, _targetRotation, Progress, Easing);
    }

    /// <summary>
    ///     Punto origen de la rotación
    /// </summary>
    public required TransformComponent.OriginPointType OriginPoint { get; init; }

    /// <summary>
    ///     Punto de origen
    /// </summary>
    public required Vector2? Origin { get; init; }

    /// <summary>
    ///     Rotación final
    /// </summary>
    public required float Rotation { get; init; }
}