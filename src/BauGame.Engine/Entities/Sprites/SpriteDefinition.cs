using Bau.BauEngine.Managers;
using Bau.BauEngine.Managers.Resources;

namespace Bau.BauEngine.Entities.Sprites;

/// <summary>
///		Definición de textura
/// </summary>
public class SpriteDefinition(string asset, string? region) : AbstractSpriteDefinition(asset, region)
{
	// Variables privadas
	private TextureConfigurationManager.TextureResolved? _textureConfiguration;

	/// <summary>
	///		Carga la textura si es la primera vez o ha habido modificaciones
	/// </summary>
	public override TextureConfigurationManager.TextureResolved? LoadAsset(Scenes.AbstractScene scene)
	{
		// Carga el asset si no estaba ya en memoria o se ha modificado
		if (IsDirty)
		{
			// Carga la configuración del asset
			_textureConfiguration = scene.SceneManager.EngineManager.ResourcesManager.TextureConfigurationManager.GetTextureRegion(scene, Asset, Region);
			// Indica que se ha cargado con las últimas modificaciones
			IsDirty = false;
		}
		// Devuelve los datos de la textura
		return _textureConfiguration;
	}

	/// <summary>
	///		Obtiene el tamaño
	/// </summary>
	public override Common.Size GetSize()
	{
		if (_textureConfiguration is null)
			return new Common.Size(0, 0);
		else
			return new Common.Size(_textureConfiguration.Region.Width, _textureConfiguration.Region.Height);
	}

	/// <summary>
	///		Actualiza los datos del sprite
	/// </summary>
	public override void Update(GameContext gameContext)
	{
	}

	/// <summary>
	///		Clona el objeto
	/// </summary>
	public override AbstractSpriteDefinition Clone() => new SpriteDefinition(Asset, Region);
}
