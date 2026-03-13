namespace EngineSample.Core.GameLogic.Actors.Characters.Actions;

/// <summary>
///		Acción para cambiar la expresión de un personaje
/// </summary>
public class UpdateExpressionCharacterAction : AbstractCharacterAction
{
	/// <summary>
	///		Actualiza el personaje
	/// </summary>
	protected override void UpdateActionSelf(CharacterActor actor, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		actor.SetDefinition(DefinitionId);
		Progress = 1;
	}

	/// <summary>
	///		Id de la definición
	/// </summary>
	public required string DefinitionId { get; init; }
}
