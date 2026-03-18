using Bau.BauEngine.Managers;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Actors;

/// <summary>
///		Actor que representa un personaje
/// </summary>
public class CharacterActor(Bau.BauEngine.Scenes.Layers.AbstractLayer layer, CharacterDefinition definition, int logicalLayer, int logicalZOrder) 
					: AbstractCharacterActor(layer, definition, logicalLayer, logicalZOrder)
{
	/// <summary>
	///		Arranca el actor
	/// </summary>
	protected override void StartCharacter()
	{
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el actor
	/// </summary>
	protected override void EndActor(GameContext gameContext)
	{
	}
}
