using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.Transitions;

/// <summary>
///		Clase base para las transiciones de audio
/// </summary>
public abstract class AbstractAudioTransition(TransitionsQueue queue, float duration)
{
    // Variables privadas
    private float _elapsed;

    /// <summary>
    ///     Actualiza la transición
    /// </summary>
    public void Update(GameContext gameContext)
    {
        if (IsActive)
        {
            float progress = MathHelper.Clamp(_elapsed / Math.Min(0.01f, Duration), 0f, 1f);

                // Actualiza la transición
                UpdateSelf(progress);
                // Acumula el tiempo
                _elapsed += gameContext.DeltaTime;
        }
    }

    /// <summary>
    ///     Actualiza la transición concreta
    /// </summary>
    protected abstract void UpdateSelf(float progress);

    /// <summary>
    ///     Detiene la transición
    /// </summary>
    public void Stop()
    {
        IsActive = false;
    }

    /// <summary>
    ///     Cola de transiciones a la que pertenece la transición
    /// </summary>
    public TransitionsQueue Queue { get; } = queue;

    /// <summary>
    ///     Duración del efecto
    /// </summary>
    public float Duration { get; } = duration;

    /// <summary>
    ///     Indica si está activo
    /// </summary>
    public bool IsActive { get; protected set; } = true;
}