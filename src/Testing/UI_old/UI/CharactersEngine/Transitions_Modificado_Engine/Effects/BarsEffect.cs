namespace UI.CharactersEngine.Transitions.Effects;

/// <summary>
///     Efecto de barras
/// </summary>
public class BarsEffect(TransitionManager manager) : AbstractTransitionEffect(manager, "Effects/Transitions/Bars")
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
        Shader?.Parameters["BarCount"]?.SetValue(8);
        Shader?.Parameters["AlternateDirection"]?.SetValue(AlternateDirection);
        Shader?.Parameters["Smoothness"]?.SetValue(Smoothness);
    }

    /// <summary>
    ///     Dirección del efecto
    /// </summary>
    public required DirectionType Direction { get; init; }

    /// <summary>
    ///     Número de barras
    /// </summary>
    public required int Bars { get; init; }

    /// <summary>
    ///     Si se establece a <see cref="true"/> barras alternas se abren en direcciones opuestas
    /// </summary>
    public required bool AlternateDirection { get; init; }

    /// <summary>
    ///     Suavizado de los bordes (0.0 a 0.5)
    /// </summary>
    public required float Smoothness { get; init; }
}