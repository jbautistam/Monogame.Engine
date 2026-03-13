using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;

namespace EngineSample.Core.GameLogic.Actors.Characters;

/// <summary>
///		Definición de una expresión de un <see cref="CharacterDefinition"/>
/// </summary>
public class CharacterExpressionDefinition(string name)
{
	// Constantes públicas
	public const string DefaultType = "default";

	/// <summary>
	///		Nombre del personaje
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Sprite de la definición
	/// </summary>
	public required SpriteDefinition Sprite { get; init; }
}
