using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Managers.Resources;
using Bau.BauEngine.Tools.Extensors;

namespace Bau.BauEngine.Entities.Sprites;

/// <summary>
///		Definición de textura
/// </summary>
public abstract class AbstractSpriteDefinition
{
	// Variables privadas
	private string _asset = string.Empty;
	private string? _region;
	// private TextureConfigurationManager.TextureResolved? _textureConfiguration;

	public AbstractSpriteDefinition(string asset, string? region)
	{
		Asset = asset;
		Region = region;
	}

	/// <summary>
	///		Actualiza los datos del sprite
	/// </summary>
	public abstract void Update(GameContext gameContext);

	/// <summary>
	///		Clona la definición del sprite
	/// </summary>
	public abstract AbstractSpriteDefinition Clone();
	
	/// <summary>
	///		Carga la textura si es la primera vez o ha habido modificaciones
	/// </summary>
	public abstract TextureConfigurationManager.TextureResolved? LoadAsset(Scenes.AbstractScene scene);

	/// <summary>
	///		Obtiene el tamaño
	/// </summary>
	public abstract Common.Size GetSize();

	/// <summary>
	///		Nombre del asset con la definición de la textura
	/// </summary>
	public string Asset
	{ 
		get { return _asset;}
		set 
		{ 
			if (!value.EqualsIgnoreNull(_asset, StringComparison.CurrentCultureIgnoreCase))
			{
				_asset = value;
				IsDirty = true; 
			}
		}
	}

	/// <summary>
	///		Región de la textura
	/// </summary>
	public string? Region
	{
		get { return _region; }
		set
		{
			if (!value.EqualsIgnoreNull(_region, StringComparison.CurrentCultureIgnoreCase))
			{
				_region = value;
				IsDirty = true;
			}
		}
	}

	/// <summary>
	///		Efecto aplicado al sprite
	/// </summary>
	public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

	/// <summary>
	///		Indica si se han modificado datos que impliquen cambiar la configuración
	/// </summary>
	protected bool IsDirty { get; set; } = true;
}
