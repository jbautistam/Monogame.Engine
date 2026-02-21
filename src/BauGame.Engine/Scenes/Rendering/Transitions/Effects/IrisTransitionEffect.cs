using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Transitions.Effects;

/// <summary>
///     Efecto de transición de apertura de Iris a partir de un punto
/// </summary>
public class IrisTransitionEffect(TransitionManager manager) : AbstractTransitionEffect(manager, "Effects/Transitions/Iris")
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
    }

    /// <summary>
    ///     Centro del efecto
    /// </summary>
    public required Vector2 Center { get; init; }
}