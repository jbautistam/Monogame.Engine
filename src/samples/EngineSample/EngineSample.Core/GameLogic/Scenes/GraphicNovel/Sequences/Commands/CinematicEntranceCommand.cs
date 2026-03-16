using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

/// <summary>
///     Comando para una entrada cinematográfica en la escena
/// </summary>
public class CinematicEntranceCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Tipo de entrada en la escena
    /// </summary>
    public enum EntranceType 
    {
        /// <summary>Movimiento rápido, impacto, pausa</summary>
        Slam,
        /// <summary>Entrada rápida y frenado con fricción</summary>
        Slide,
        /// <summary>Caída acelerada por la gravedad</summary>
        Drop,
        /// <summary>Parábola de entrada</summary>
        Leap,
        /// <summary>Teleportación de un punto a otro</summary>
        Teleport, 
        /// <summary>Sube desde un punto inferior con opacidad creciente</summary>
        Emerging, 
        /// <summary>Baja con oscilación</summary>
        Descending
    }
    // Variables privadas
    private Vector2 _start, _to;
    private bool _initialized;

    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
	protected override void ApplySelf(AbstractActorDrawable actor)
    {
        // Inicializa los valores
        if (!_initialized)
        {
            _start = actor.Transform.Bounds.TopLeft;
            _to = ToWorld(To);
            _initialized = true;
        }
        // Ejecuta el efecto
        switch (Type)
        {
            case EntranceType.Slide:
                    ApplySlide(actor, Progress);
                break;
            case EntranceType.Drop:
                    ApplyDrop(actor, Progress);
                break;
            case EntranceType.Leap:
                    ApplyLeap(actor, Progress);
                break;
            case EntranceType.Teleport:
                    ApplyTeleport(actor, Progress);
                break;
            case EntranceType.Emerging:
                    ApplyEmerging(actor, Progress);
                break;
            case EntranceType.Descending:
                    ApplyDescending(actor, Progress);
                break;
            default:
                    ApplySlam(actor, Progress);
                break;
        }
    }

    /// <summary>
    ///     Entrada de tipo slam: Anticipación (retrocede ligeramente) -> movimiento rápido -> impacto -> pausa
    /// </summary>
    private Vector2 ApplySlam(AbstractActorDrawable actor, float t)
    {
        float anticipationEnd = Anticipation;
        float moveEnd = 1 - ImpactPause / Duration;
    
            if (t < anticipationEnd)
                return EasingFunctionsHelper.Interpolate(_start, _start - new Vector2(50, 0), 
                                                         t / anticipationEnd, 
                                                         EasingFunctionsHelper.EasingType.BackIn);
            else if (t < moveEnd)
                return EasingFunctionsHelper.Interpolate(_start - new Vector2(50, 0), _to, 
                                                         (t - anticipationEnd) / (moveEnd - anticipationEnd), 
                                                         EasingFunctionsHelper.EasingType.ExpoOut);
            else
                return EasingFunctionsHelper.Interpolate(_start, _to, t, EasingFunctionsHelper.EasingType.Linear);
    }

    /// <summary>
    ///     Entrada de tipo slide: entra rápido, frena con fricción
    /// </summary>
    private void ApplySlide(AbstractActorDrawable actor, float t)
    {
        // Entra rápido, frena con fricción
        if (t < 0.7f)
            actor.Transform.Bounds.TopLeft = EasingFunctionsHelper.Interpolate(_start, _to, t / 0.7f, EasingFunctionsHelper.EasingType.QuadOut);
        else
        {
            float localT = (t - 0.7f) / 0.3f;
            float overshoot = MathF.Sin(localT * MathF.PI) * 20 * (1 - localT);

                // Cambia la posición
                actor.Transform.Bounds.TopLeft = _to + new Vector2(overshoot, 0);
        }
    }

    /// <summary>
    ///     Entrada de tipo caída acelerada por la gravedad
    /// </summary>
    private void ApplyDrop(AbstractActorDrawable actor, float t)
    {
        float xOffset = EasingFunctions.Linear(t) * (_to.X - _start.X);
        float yOffset = EasingFunctions.QuadIn(t) * (_to.Y - _start.Y);

            // Mueve el actor
            actor.Transform.Bounds.TopLeft = new Vector2(_start.X + xOffset, _start.Y + yOffset);
            // Escala al impactar (achatamiento)
            if (t > 0.9f)
            {
                float squash = 1 - (t - 0.9f) * 10 * 0.2f;

                    // Cambia la escala
                    actor.Renderer.Scale = new Vector2(1.1f, squash);
            }
    }

    /// <summary>
    ///     Aplica una parábola de entrada
    /// </summary>
    private void ApplyLeap(AbstractActorDrawable actor, float t)
    {
        Vector2 midPoint = 0.5f * (_start + _to) + new Vector2(0, -200);

            // Aplica la parábolva    
            if (t < 0.5f)
            {
                float localT = t * 2;

                    // Cambia la posición
                    actor.Transform.Bounds.TopLeft = Vector2.Lerp(Vector2.Lerp(_start, midPoint, localT),
                                                                  Vector2.Lerp(midPoint, _to, localT),
                                                                  EasingFunctions.QuadOut(localT));
            }
            else
            {
                float localT = (t - 0.5f) * 2;

                    // Cambia la posición
                    actor.Transform.Bounds.TopLeft = Vector2.Lerp(Vector2.Lerp(midPoint, _to, localT),
                                                                  _to,
                                                                  EasingFunctions.QuadIn(localT));
            }
    }

    /// <summary>
    ///     Desaparece del punto de origen y aparece en destino con efecto
    /// </summary>
    private void ApplyTeleport(AbstractActorDrawable actor, float t)
    {
        float fadeOutEnd = 0.2f;
        float fadeInStart = 0.4f;

            // Aplica el efecto    
            if (t < fadeOutEnd)
            {
                actor.Renderer.Opacity = 1 - t / fadeOutEnd;
                actor.Transform.Bounds.TopLeft = _start;
            }
            else if (t < fadeInStart)
                actor.Renderer.Opacity = 0;
            else
            {
                float localT = (t - fadeInStart) / (1 - fadeInStart);
                float scale = 1 + MathF.Sin(localT * MathF.PI) * 0.3f;

                    // Cambia opacidad y posición
                    actor.Renderer.Opacity = localT;
                    actor.Transform.Bounds.TopLeft = _to;
                    // Escala para materializarse en el destino
                    actor.Renderer.Scale = new Vector2(scale, scale);
            }
    }

    /// <summary>
    ///     Sube con opacidad creciente
    /// </summary>
    private void ApplyEmerging(AbstractActorDrawable actor, float t)
    {
        actor.Transform.Bounds.TopLeft = Vector2.Lerp(_start, _to, EasingFunctions.BackOut(t));
        actor.Renderer.Opacity = MathHelper.Clamp(t * 2, 0, 1);
    }

    /// <summary>
    ///     Baja con oscilación de cuerda
    /// </summary>
    private void ApplyDescending(AbstractActorDrawable actor, float t)
    {
        float sway = MathF.Sin(t * MathF.PI * 3) * 30 * (1 - t);
        Vector2 basePos = Vector2.Lerp(_start, _to, EasingFunctions.QuadIn(t));

            // Cambia posición y rotación
            actor.Transform.Bounds.TopLeft = basePos + new Vector2(sway, 0);
            actor.Transform.Rotation = sway * 0.02f;
    }

    /// <summary>
    ///     Tipo de entrada que se va a ejecutar
    /// </summary>
    public required EntranceType Type { get; init; }

    /// <summary>
    ///     Punto destino del movimiento
    /// </summary>
    public required Vector2 To { get; init; }

    /// <summary>
    ///     Tiempo de pausa tras el impacto
    /// </summary>
    public float ImpactPause { get; set; }= 0.1f;

    /// <summary>
    ///     Anticipación
    /// </summary>
    public float Anticipation { get; set; } = 0.2f;
}