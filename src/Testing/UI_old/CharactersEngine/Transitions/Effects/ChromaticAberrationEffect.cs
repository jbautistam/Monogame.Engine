namespace UI.CharactersEngine.Transitions.Effects;

/// <summary>
///     Efecto de aberración cromática
/// </summary>
public class ChromaticAberrationEffect(TransitionManager manager) : AbstractTransitionEffect(manager, "Effects/Transitions/ChromaticAberration")
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
        Shader?.Parameters["Direction"]?.SetValue(GetDirection(Direction));
    }

    /// <summary>
    ///     Dirección del efecto
    /// </summary>
    public required DirectionType Direction { get; init; }
}