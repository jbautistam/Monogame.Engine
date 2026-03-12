namespace UI.ParticlesDefinition;

/// <summary>
///     Modificador de un emisor de partículas
/// </summary>
public class ParticleSystemModifierDefinitionModel(ParticleSystemModifierDefinitionModel.ModifierType type)
{
	/// <summary>
	///		Tipo de modificador
	/// </summary>
	public enum ModifierType
	{
		/// <summary>Opacidad</summary>
		Opacity,
		/// <summary>Atracción</summary>
		Attraction,
		/// <summary>Color</summary>
		Color,
		/// <summary>Rozamiento</summary>
		Drag,
		/// <summary>Gravedad</summary>
		Gravity,
		/// <summary>Rotación</summary>
		Rotation,
		/// <summary>Viento</summary>
		Wind
	}

	/// <summary>
	///		Tipo de modificador
	/// </summary>
	public ModifierType Type { get; } = type;

	/// <summary>
	///		Parámetros
	/// </summary>
	public Dictionary<string, object> Parameters { get; } = new(StringComparer.CurrentCultureIgnoreCase);
}
