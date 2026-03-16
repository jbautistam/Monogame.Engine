using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

/// <summary>
///     Comando de flash: destello blanco momentáneo
/// </summary>
public class FlashCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        float intensity;
        
            // Cambia la intensidad según pasa el tiempo
            if (Progress < 0.5f)
                intensity = Progress * 2 * PeakIntensity;
            else
                intensity = (1 - Progress) * 2 * PeakIntensity;
            // Cambia el color
            actor.Renderer.Color = Color.Lerp(actor.Renderer.Color, Color.White, intensity);
    }

    /// <summary>
    ///     Momento de máximo brillo (0..1)
    /// </summary>
    public float PeakIntensity { get; set; } = 0.5f;
}
