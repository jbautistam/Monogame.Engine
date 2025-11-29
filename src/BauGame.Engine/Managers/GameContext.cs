using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Managers;

/// <summary>
///     Contexto de la partida
/// </summary>
public class GameContext
{
    /// <summary>
    ///     Actualiza el contexto
    /// </summary>
    public void Update(GameTime gameTime)
    {
        // Guarda el contexto
        GameTime = gameTime;
        // Normaliza la escala de tiempo
        if (TimeScale == 0)
            TimeScale = 1;
        TimeScale = Math.Clamp(TimeScale, 0, 1);
        // Actualiza las propiedades (para sólo hacer los cálculos una vez)
        TotalTime = (float) gameTime.TotalGameTime.TotalSeconds;
        UnscaledDeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        if (Paused)
            DeltaTime = 0;
        else
            DeltaTime = UnscaledDeltaTime * TimeScale;
    }

    /// <summary>
    ///     Detiene la partida
    /// </summary>
    public void Pause()
    {
        Paused = true;
    }

    /// <summary>
    ///     Reinicia la partida
    /// </summary>
    public void Resume()
    {
        Paused = false;
    }

    /// <summary>
    ///     Cambia el modo de pausa
    /// </summary>
    public void TogglePause()
    {
        Paused = !Paused;
    }

    /// <summary>
    ///     Obtiene el tiempo total
    /// </summary>
	public TimeSpan GetTotalTime(TimeSpan timeSpan) => GameTime.TotalGameTime.Add(timeSpan);

	/// <summary>
	///     Datos del GameTime normal
	/// </summary>
	public GameTime GameTime { get; private set; } = default!;

    /// <summary>
    ///     Tiempo desde el último Update (escalado por <see cref="TimeScale"/>)
    /// </summary>
    public float DeltaTime { get; private set; }

    /// <summary>
    ///     Tiempo desde el último Update (sin escalar por <see cref="TimeScale"/>)
    /// </summary>
    public float UnscaledDeltaTime { get; private set; }

    /// <summary>
    ///     Tiempo total
    /// </summary>
    public float TotalTime { get; private set; }

    /// <summary>
    ///     Escala de tiempo
    /// </summary>
    public float TimeScale { get; set; }

    /// <summary>
    ///     Indica si el juego está detenido
    /// </summary>
    public bool Paused { get; private set; }

    /// <summary>
    ///     Indica si se está ejecutando el juego
    /// </summary>
    public bool IsRunning => !Paused;

    /// <summary>
    ///     Indica si el juego se está moviendo a menor velocidad
    /// </summary>
    public bool IsSlowMotion => TimeScale < 1.0f;

    /// <summary>
    ///     Indica si el juego se está moviendo a mayor velocidad
    /// </summary>
    public bool IsFastForward => TimeScale > 1.0f;
}