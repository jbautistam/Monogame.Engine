using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Core.Actors;
using Bau.Libraries.BauGame.Engine.Core.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Core.Scenes.Layers;

namespace EngineSample.Core.GameLogic.Actors.Characters;

/// <summary>
///		Actor del manager de personajes
/// </summary>
public class CharacterManager(AbstractLayer layer) : AbstractActor(layer)
{
	/// <summary>
	///		Añade un personaje
	/// </summary>
	public CharacterActor Add(string name)
	{
		CharacterActor character = new(this)
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
	public override void Start()
	{
		foreach (CharacterActor character in Characters.Items.Values)
			character.Start();
	}

	/// <summary>
	///		Actualiza los personajes
	/// </summary>
	protected override void UpdateActor(GameTime gameTime)
	{
		foreach (CharacterActor character in Characters.Items.Values)
			character.Update(gameTime);
	}

	/// <summary>
	///		Dibuja los personajes
	/// </summary>
	protected override void DrawActor(Camera2D camera, GameTime gameTime)
	{
		foreach (CharacterActor character in Characters.Items.Values)
			if (character.Enabled)
				character.Draw(camera, gameTime);
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