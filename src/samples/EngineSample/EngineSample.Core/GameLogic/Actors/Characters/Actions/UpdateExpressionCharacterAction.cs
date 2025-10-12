using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción para cambiar la expresión de un personaje
/// </summary>
public class UpdateExpressionCharacterAction : AbstractCharacterAction
{
	/// <summary>
	///		Actualiza el personaje
	/// </summary>
	protected override bool UpdateAction(CharacterActor actor, float elapsed, GameTime gameTime)
	{
		// Cambia la definición del personaje
		actor.SetDefinition(DefinitionId);
		// Devuelve el valor que indica si ha terminado la acción
		return true;
	}

	/// <summary>
	///		Id de la definición
	/// </summary>
	public required string DefinitionId { get; init; }
}
