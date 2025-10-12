using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;

namespace EngineSample.Core.GameLogic.Actors.Characters;

/// <summary>
///		Definición de las secciones de un actor
/// </summary>
public class CharacterActorDefinition(CharacterActor parent, string type, string texture, string region)
{
	// Constantes públicas
	public const string DefaultType = "default";
	// Variables privadas
	private AbstractTexture? _texture;

	/// <summary>
	///		Arranca la carga de los datos de la definición
	/// </summary>
	public void Start()
	{
		_texture = GameEngine.Instance.ResourcesManager.TextureManager.Assets.Get(Texture);
	}

	/// <summary>
	///		Obtiene la región asociada a la textura
	/// </summary>
	internal TextureRegion? GetTextureRegion() => _texture?.GetRegion(Region);

	/// <summary>
	///		Actor padre
	/// </summary>
	public CharacterActor Parent { get; } = parent;

	/// <summary>
	///		Tipo de definición de la textura del actor (avatar, triste, predeterminado...)
	/// </summary>
	public string Type { get; } = type;

	/// <summary>
	///		Textura
	/// </summary>
	public string Texture { get; } = texture;

	/// <summary>
	///		Región
	/// </summary>
	public string Region { get; } = region;
}
