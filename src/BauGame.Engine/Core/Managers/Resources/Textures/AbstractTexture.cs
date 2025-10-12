using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Core.Managers.Resources.Textures;

/// <summary>
///		Clase abstracta para la textura
/// </summary>
public abstract class AbstractTexture(TextureManager textureManager, string id, string asset)
{
	// Variables privadas
	private bool _isLoaded;
	private Texture2D? _texture;

	/// <summary>
	///		Obtiene la textura
	/// </summary>
	protected Texture2D? GetTexture() 
	{
		// Carga la textura
		if (!_isLoaded)
		{
			_texture = TextureManager.ResourcesManager.GlobalContentManager.LoadAsset<Texture2D>(Asset);
			_isLoaded = true;
		}
		// Devuelve la textura cargada
		return _texture;
	}

	/// <summary>
	///		Obtiene la <see cref="TextureRegion"/> asociada a un nombre
	/// </summary>
	public abstract TextureRegion? GetRegion(string name);

	/// <summary>
	///		Manager del motor
	/// </summary>
	public TextureManager TextureManager { get; } = textureManager;

	/// <summary>
	///		Identificador
	/// </summary>
	public string Id { get; } = id;

	/// <summary>
	///		Ruta de la textura
	/// </summary>
	public string Asset { get; } = asset;
}