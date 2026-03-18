using Microsoft.Xna.Framework;
using Bau.BauEngine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Cinematics;

/// <summary>
///     Comando para reacción de tipo impacto
/// </summary>
public class ImpactReactionCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Tipo de reacción
    /// </summary>
    public enum ReactionType
    {
        /// <summary>Tambaleo hacia atrás y recuperación</summary>
        Stagger,
        /// <summary>Empujón y recuperación lenta</summary>
        Knockback,
        /// <summary>Efecto de caida</summary>
        Collapse,
        /// <summary>Deslizamiento con fricción</summary>
        Slide,
        /// <summary>Giro por el impacto</summary>
        Spin,
        /// <summary>Vibración intensa que decae</summary>
        Shake
    }
    // Variables privadas
    private Vector2 _start;
    private float _startRotation;
    private bool _initialized;

    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        // Inicicializa los datos
        if (!_initialized)
        {
            _start = actor.Transform.Bounds.Location;
            _startRotation = actor.Transform.Rotation;
            _initialized = true;
        }
        // Aplica la reacción
        switch (Reaction)
        {
            case ReactionType.Stagger:
                    ApplyStagger(actor, GetImpactDirection());
                break;
            case ReactionType.Knockback:
                    ApplyKnockback(actor, GetImpactDirection());
                break;
            case ReactionType.Collapse:
                    ApplyCollapse(actor, GetImpactDirection());
                break;
            case ReactionType.Slide:
                    ApplySlide(actor, GetImpactDirection());
                break;
            case ReactionType.Spin:
                    ApplySpin(actor);
                break;
            case ReactionType.Shake:
                    ApplyShake(actor);
                break;
        }

        // Obtiene la dirección del impacto
        Vector2 GetImpactDirection() => Vector2.Normalize(_start - ImpactSource);
    }
    
    /// <summary>
    ///     Aplica un efecto de tambaleo hacia atrás y recuperación
    /// </summary>
    private void ApplyStagger(Bau.BauEngine.Actors.AbstractActorDrawable actor, Vector2 direction)
    {
        // Fuerza hacia atrás
        actor.Transform.Bounds.Location = _start + direction * MathF.Sin(Progress * MathF.PI) * Force * 0.3f;
        // Rotación de tambaleo
        actor.Transform.Rotation = _startRotation + MathF.Sin(Progress * MathF.PI * 2) * 0.1f * (1 - Progress);
    }

    /// <summary>
    ///     Aplica un efecto de empujón y recuperación lenta
    /// </summary>
    private void ApplyKnockback(Bau.BauEngine.Actors.AbstractActorDrawable actor, Vector2 direction)
    {
        if (Progress < 0.3f)
            actor.Transform.Bounds.Location = _start + direction * Force * EasingFunctions.ExpoOut(Progress / 0.3f);
        else
            actor.Transform.Bounds.Location = _start + direction * Force * (1 - EasingFunctions.QuadOut((Progress - 0.3f) / 0.7f));
    }
    
    /// <summary>
    ///     Aplica un efecto de caida
    /// </summary>
    private void ApplyCollapse(Bau.BauEngine.Actors.AbstractActorDrawable actor, Vector2 direction)
    {
        float fallProgress = EasingFunctions.QuadIn(Progress);

            // Aplica el efecto
            actor.Transform.Bounds.Location = _start + direction * Force * 0.5f * fallProgress;
            actor.Transform.Rotation = _startRotation + fallProgress * MathHelper.PiOver2;
            actor.Renderer.Scale = new Vector2(1 + fallProgress * 0.2f, 1 - fallProgress * 0.3f);
    }
    
    /// <summary>
    ///     Aplica un deslizamiento con fricción
    /// </summary>
    private void ApplySlide(Bau.BauEngine.Actors.AbstractActorDrawable actor, Vector2 direction)
    {
        float distance = Force * (1 - EasingFunctions.QuadOut(Progress));

            // Aplica el efecto
            actor.Transform.Bounds.Location = _start + direction * distance;
    }
    
    /// <summary>
    ///     Aplica un giro por el impacto
    /// </summary>
    private void ApplySpin(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        float rotations = Force / 100f;

            // Aplica el giro
            actor.Transform.Rotation = _startRotation + rotations * MathHelper.TwoPi * EasingFunctions.QuadOut(Progress);
    }
    
    /// <summary>
    ///     Aplica el efecto de vibración intensa que decae
    /// </summary>
    private void ApplyShake(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        float intensity = Force * (1 - Progress);
        float noise = Bau.BauEngine.GameEngine.Instance.MathFunctions.Noise.GetNoise(Progress * 20) * intensity;

            // Cambia la ubicación del actor
            actor.Transform.Bounds.Location = _start + new Vector2(noise, noise * 0.5f);
    }

    /// <summary>
    ///     Reacción
    /// </summary>
    public required ReactionType Reaction { get; init; }

    /// <summary>
    ///     Fuente del impacto
    /// </summary>
    public required Vector2 ImpactSource { get; init; }

    /// <summary>
    ///     Fuerza
    /// </summary>
    public required float Force { get; init; }
}
