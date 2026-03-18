using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Managers.Audio.Transitions;

/// <summary>
///     Sistema para un transiciones fade out: un sonido va del volumen inicial a cero
/// </summary>
public class FadeOutTransition(TransitionsQueue queue, float volume, float duration) : AbstractAudioTransition(queue, duration)
{
    /// <summary>
    ///     Actualiza la transición
    /// </summary>
	protected override void UpdateSelf(float progress)
	{
        // El origen cambia el volumen de 1 a 0
        Queue.AudioSystem.Current?.SetVolume(MathHelper.Lerp(Volume, 0f, progress));
        // Hasta que se detiene
        if (progress >= 0.9)
        {
            // Detiene el sonido actual
            Queue.AudioSystem.Current?.Stop();
            Queue.AudioSystem.Current = null;
            // Indica que la transición ha terminado
            IsActive = false;
        }
    }

    /// <summary>
    ///     Volumen actual
    /// </summary>
    public float Volume { get; } = volume;
}
