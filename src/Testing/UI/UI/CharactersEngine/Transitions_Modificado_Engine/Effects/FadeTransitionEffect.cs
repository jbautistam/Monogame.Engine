using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.Transitions.Effects;

/// <summary>
///     Efecto de transición fade
/// </summary>
public class FadeTransitionEffect(TransitionManager manager) : AbstractTransitionEffect(manager, "Effects/Transitions/Fade")
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
        Shader?.Parameters["FadeColor"]?.SetValue(Color.ToVector4());
    }

    /// <summary>
    ///     Color final del efecto
    /// </summary>
    public required Color Color { get; init; }
}