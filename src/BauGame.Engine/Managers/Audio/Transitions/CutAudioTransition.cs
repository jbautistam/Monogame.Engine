using Bau.Libraries.BauGame.Engine.Managers.Audio.Sources;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.Transitions;

/// <summary>
///     Corte de audio. Se espera un tiempo y se pasa a la siguiente canción
/// </summary>
public class CutAudioTransition(TransitionsQueue queue, AbstractAudioSource? target, float volume, float duration) : AbstractAudioTransition(queue, duration)
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
            // Detiene el audio original
            Queue.AudioSystem.Current?.Stop();
            // Indica que se ha inicializado
            _initialized = true;
        }
        // Y al tiempo, inicia el siguiente
        if (progress >= 0.9)
        {
            // Inicia el siguiente sonido
            Queue.AudioSystem.Current = Target;
            if (Target is not null)
            {
                Queue.AudioSystem.Current?.SetVolume(Volume);
                Queue.AudioSystem.Current?.Play();
            }
            // Indica que se ha terminado
            IsActive = false;
        }
	}

    /// <summary>
    ///     Sonido sobre el que se ejecuta la transición
    /// </summary>
    public AbstractAudioSource? Target { get; } = target;

    /// <summary>
    ///     Volumen final
    /// </summary>
    public float Volume { get; } = volume;
}
