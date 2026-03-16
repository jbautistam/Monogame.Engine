using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Managers.Resources;

namespace Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;

/// <summary>
///		Definición de textura
/// </summary>
public class SpriteDefinition
{
	// Variables privadas
	private string _asset = string.Empty;
	private TextureConfigurationManager.TextureResolved? _textureConfiguration;
	private bool _isDirty;

	public SpriteDefinition(string asset, string? region)
	{
		Asset = asset;
		Region = region;
	}

	/// <summary>
	///		Actualiza los datos del sprite
	/// </summary>
	public void Update(GameContext gameContext)
	{
	}

	/// <summary>
	///		Carga la textura si es la primera vez o ha habido modificaciones
	/// </summary>
	public TextureConfigurationManager.TextureResolved? LoadAsset(Scenes.AbstractScene scene)
	{
		// Carga el asset si no estaba ya en memoria o se ha modificado
		if (_isDirty)
		{
			// Carga la configuración del asset
			_textureConfiguration = GameEngine.Instance.ResourcesManager.TextureConfigurationManager.GetTextureRegion(scene, Asset, Region);
			// Indica que se ha cargado con las últimas modificaciones
			_isDirty = false;
		}
		// Devuelve los datos de la textura
		return _textureConfiguration;
	}

	/// <summary>
	///		Obtiene el tamaño
	/// </summary>
	public Size GetSize()
	{
		if (_textureConfiguration is null)
			return new Size(0, 0);
		else
			return new Size(_textureConfiguration.Region.Width, _textureConfiguration.Region.Height);
	}

	/// <summary>
	///		Nombre del asset con la definición de la textura
	/// </summary>
	public string Asset
	{ 
		get { return _asset;}
		set
		{
			if (!string.IsNullOrWhiteSpace(value) && !value.Equals(_asset, StringComparison.CurrentCultureIgnoreCase))
			{
				_asset = value;
				_isDirty = true;
			}
		}
	}

	/// <summary>
	///		Región de la textura
	/// </summary>
	public string? Region { get; set; }

	/// <summary>
	///		Efecto aplicado al sprite
	/// </summary>
	public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
}
