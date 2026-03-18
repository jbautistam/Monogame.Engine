using Microsoft.Xna.Framework;
using Bau.BauEngine.Managers.Audio.Sources;

namespace Bau.BauEngine.Managers.Audio.Transitions;

/// <summary>
///     Sistema para un transiciones fade In: comienza a tocar un sonido poco a poco
/// </summary>
public class FadeInTransition(TransitionsQueue queue, AbstractAudioSource target, float maxVolume, float duration) : AbstractAudioTransition(queue, duration)
{
    // Variables privadas
    private bool _initialized;

    /// <summary>
    ///     Actualiza la transición
    /// </summary>
	protected override void UpdateSelf(float progress)
	{
        // Inicializa los datos
        if (!_initialized)
        {
            // Asigna el audio objetivo
            Queue.AudioSystem.Current = Target;
            // Arranca el audio objetivo sin volumen
            Queue.AudioSystem.Current.SetVolume(0f);
            Queue.AudioSystem.Current.Play();
            // Indica que se ha inicializado
            _initialized = true;
        }
        // Y sube el volumen de 0 a 1
        Queue.AudioSystem.Current?.SetVolume(MathHelper.Lerp(0f, MaxVolume, progress));
    }

    /// <summary>
    ///     Sonido sobre el que se ejecuta la transición
    /// </summary>
    public AbstractAudioSource Target { get; } = target;

    /// <summary>
    ///     Volumen máximo
    /// </summary>
    public float MaxVolume { get; } = maxVolume;
}
