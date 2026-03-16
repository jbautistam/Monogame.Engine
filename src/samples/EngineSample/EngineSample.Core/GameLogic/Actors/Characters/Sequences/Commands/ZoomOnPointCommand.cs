using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

/// <summary>
///     Comando que realiza zoom centrado en un punto específico del actor
/// </summary>
public class ZoomOnPointCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private bool _initialized;
    private Vector2 _startScale, _start, _target;

    /// <summary>
    ///     Aplica el comando
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        // Guarda los datos iniciales
        if (!_initialized)
        {
            // Guarda los valores iniciales
            _startScale = actor.Renderer.Scale;
            // Punto en coordenadas mundiales al inicio (se mantiene fijo durante el zoom)
            _start = actor.Transform.Bounds.TopLeft;
            // Calcula la posición final necesaria para mantener ese punto fijo con la escala objetivo
            _target = CalculatePositionForZoom(_start, Scale);
            // Indica que se ha inicializado
            _initialized = true;
        }
        // Interpolar la escala y la posición
        actor.Renderer.Scale = EasingFunctionsHelper.Interpolate(_startScale, Scale, Progress, Easing);
        actor.Transform.Bounds.TopLeft = EasingFunctionsHelper.Interpolate(_start, _target, Progress, Easing);
    }

    /// <summary>
    ///     Calcula la posición necesaria del actor para que el punto en el mundo quede fijo en pantalla cuando se aplique la escala necesaria
    /// </summary>
    /// <remarks>
    /// A escala 1, el punto local está en: Posición + LocalOffset
    /// A escala S, queremos que siga en la misma posición mundial:
    /// NuevaPosición + (LocalOffset * S) = PuntoMundo
    /// Simplificando: NuevaPosición = PuntoMundo - (LocalOffset * S)
    /// </remarks>
    private Vector2 CalculatePositionForZoom(Vector2 worldAnchorPoint, Vector2 targetScale)
    {
        return worldAnchorPoint - LocalPoint * targetScale;
    }

    /// <summary>
    ///     Punto en coordenadas relativas del actor sobre el que se va a enfocar
    /// </summary>
    public required Vector2 LocalPoint { get; init; }

    /// <summary>
    ///     Escala final
    /// </summary>
    public required Vector2 Scale { get; init; }
}