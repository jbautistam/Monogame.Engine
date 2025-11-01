using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;

namespace EngineSample.Core.GameLogic.Actors.Characters;

/// <summary>
///		Actor del manager de personajes
/// </summary>
public class CharacterManager(AbstractLayer layer, int zOrder) : AbstractActor(layer, zOrder)
{
	/// <summary>
	///		Añade un personaje
	/// </summary>
	public CharacterActor Add(string name)
	{
		CharacterActor character = new(this, ZOrder)
										{
											Enabled = false
										};

			// Añade el personaje al diccionario
			Characters.Add(name, character);
			// Devuelve el personaje
			return character;
	}

	/// <summary>
	///		Obtiene un actor
	/// </summary>
	public CharacterActor? GetActor(string name) => Characters.Get(name);

	/// <summary>
	///		Inicializa los personajes
	/// </summary>
	public override void StartActor()
	{
		foreach (CharacterActor character in Characters.Items.Values)
			character.StartActor();
	}

	/// <summary>
	///		Actualiza los personajes
	/// </summary>
	protected override void UpdateActor(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		foreach (CharacterActor character in Characters.Items.Values)
			character.Update(gameContext);
	}

	/// <summary>
	///		Dibuja los personajes
	/// </summary>
	protected override void DrawActor(Camera2D camera, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		foreach (CharacterActor character in Characters.Items.Values)
			if (character.Enabled)
				character.Draw(camera, gameContext);
	}

	/// <summary>
	///		Finaliza el trabajo con los personajes
	/// </summary>
	protected override void EndActor()
	{
		foreach (CharacterActor character in Characters.Items.Values)
			character.End();
	}

	/// <summary>
	///		Personajes
	/// </summary>
	private Bau.Libraries.BauGame.Engine.Base.DictionaryModel<CharacterActor> Characters { get; } = new();
}