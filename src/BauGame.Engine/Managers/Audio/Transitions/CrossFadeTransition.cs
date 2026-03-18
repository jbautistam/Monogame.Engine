using Microsoft.Xna.Framework;
using Bau.BauEngine.Managers.Audio.Sources;

namespace Bau.BauEngine.Managers.Audio.Transitions;

/// <summary>
///     Transición de fade simultáneo de A a B
/// </summary>
public class CrossFadeTransition(TransitionsQueue queue, AbstractAudioSource? target, float maxVolume, float duration) : AbstractAudioTransition(queue, duration)
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
            // Arranca el audio objetivo sin volumn
            Target?.SetVolume(0f);
            Target?.Play();
            // Indica que se ha inicializado
            _initialized = true;
        }
        // Al llegar al final, transforma el sonido destino en el actual
        if (progress > 0.99f)
        {
            // El origen cambia el volumen de 1 a 0 mientras que el destino sube el volumen de 0 a 1
            Queue.AudioSystem.Current?.SetVolume(MathHelper.Lerp(MaxVolume, 0f, progress));
            Target?.SetVolume(MathHelper.Lerp(0f, MaxVolume, progress));
        }
        else
        {
            Queue.AudioSystem.Current = Target;
            IsActive = false;
        }
    }

    /// <summary>
    ///     Canción destino
    /// </summary>
    private AbstractAudioSource? Target { get; } = target;

    /// <summary>
    ///     Volumen máximo
    /// </summary>
    public float MaxVolume { get; } = maxVolume;
}
