namespace UI.CharactersEngine;

/// <summary>
///		Motor de la novela visual
/// </summary>
internal class VisualNovelEngine
{
	/// <summary>
	///		Motor para tratamiento de los personajes y fondos
	/// </summary>
	internal CharactersEngine CharactersEngine { get; } = new();
}