using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.Common;

/// <summary>
///		Definición de textura
/// </summary>
public class SpriteDefinition
{
	// Variables privadas
	private string _asset = default!;
	private AbstractTexture? _textureSprite;
	private bool _isDirty;

	public SpriteDefinition(Scenes.Layers.AbstractLayer layer, string asset, string? region)
	{
		Layer = layer;
		Asset = asset;
		Region = region;
	}

	/// <summary>
	///		Arranca la carga de los datos de la definición
	/// </summary>
	public void Update(GameContext gameContext)
	{
		LoadAsset();
	}

	/// <summary>
	///		Carga la textura si es la primera vez o ha habido modificaciones
	/// </summary>
	private void LoadAsset()
	{
		if (_isDirty)
		{
			// Carga la textura
			if (string.IsNullOrWhiteSpace(Asset))
				_textureSprite = null;
			else
				_textureSprite = GameEngine.Instance.ResourcesManager.TextureManager.Assets.Get(Asset);
			// Indica que se ha cargado con las últimas modificaciones
			_isDirty = false;
		}
	}

	/// <summary>
	///		Obtiene el tamaño
	/// </summary>
	public Size GetSize()
	{
		TextureRegion? region = GetRegion(Region);

			if (region is null)
				return new Size(0, 0);
			else
				return new Size(region.Region.Width, region.Region.Height);
	}

	/// <summary>
	///		Obtiene la región de la textura para dibujarla
	/// </summary>
	private TextureRegion? GetRegion(string? region)
	{
		// Carga la textura si es necesario
		LoadAsset();
		// Devuelve la región
		if (_textureSprite is null)
			return null;
		//else if (Animator.IsPlaying || Animator.HasEndLoop)
		//	return Animator.GetTexture(_textureSprite);
		else
			return _textureSprite.GetRegion(region);
	}

	/// <summary>
	///		Obtiene la textura
	/// </summary>
	public Texture2D? GetTexture()
	{
		if (_textureSprite is null)
			return null;
		else
		{
			TextureRegion? region = GetRegion(Region);

				if (region is not null)
					return region.Texture;
				else
					return null;
		}
	}

	/// <summary>
	///		Dibuja el sprite
	/// </summary>
    public void Draw(Camera2D camera, Point position, Point center, Vector2 scale, float rotation, Color color)
    {
		Draw(camera, new Vector2(position.X, position.Y), new Vector2(center.X, center.Y), scale, rotation, color);
    }

	/// <summary>
	///		Dibuja el sprite
	/// </summary>
    public void Draw(Camera2D camera, Vector2 position, Vector2 center, Vector2 scale, float rotation, Color color)
    {
		TextureRegion? region = GetRegion(Region);

			if (region is not null && region.Texture is not null)
				region.Draw(camera, position, center, scale, SpriteEffect, color, rotation);
    }

	/// <summary>
	///		Dibuja el sprite en un rectángulo
	/// </summary>
    public void Draw(Camera2D camera, Rectangle destination, Vector2 center, float rotation, Color color)
    {
		TextureRegion? region = GetRegion(Region);

			if (region is not null && region.Texture is not null)
				region.Draw(camera, destination, center, SpriteEffect, color, rotation);
    }

	/// <summary>
	///		Capa a la que se asocia el sprite
	/// </summary>
	public Scenes.Layers.AbstractLayer Layer { get; }

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
