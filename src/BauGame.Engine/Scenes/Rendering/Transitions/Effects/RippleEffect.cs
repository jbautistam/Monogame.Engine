using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Transitions.Effects;

/// <summary>
///     Efecto de barras
/// </summary>
public class RippleEffect(TransitionManager manager) : AbstractTransitionEffect(manager, "Effects/Transitions/Ripple")
{
	/// <summary>
	///     Inicializa la transición (por ejemplo, carga los shaders)
	/// </summary>
	protected override void StartShelf() {}

    /// <summary>
    ///     Actualiza los parámetros particulares del efecto
    /// </summary>
    protected override void UpdateParameters(float deltaTime)
    {
        Shader?.Parameters["Center"]?.SetValue(Center);
        Shader?.Parameters["Frequency"]?.SetValue(Frequency);
        Shader?.Parameters["Amplitude"]?.SetValue(Amplitude);
        Shader?.Parameters["Speed"]?.SetValue(Speed);
    }

    /// <summary>
    ///     Centro del efecto
    /// </summary>
    public required Vector2 Center { get; init; }

    /// <summary>
    ///     Frecuencia del efecto
    /// </summary>
    public required float Frequency { get; init; }

    /// <summary>
    ///     Amplitud del efecto
    /// </summary>
    public required float Amplitude { get; init; }

    /// <summary>
    ///     Velocidad del efecto
    /// </summary>
    public required float Speed { get; init; }
}