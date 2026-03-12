using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Managers.Resources;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures.Configuration;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

namespace Bau.Libraries.BauGame.Engine.Entities.Common;

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
	private TextureConfigurationManager.TextureResolved? LoadAsset(Camera2D camera)
	{
		// Carga el asset si no estaba ya en memoria o se ha modificado
		if (_isDirty)
		{
			// Carga la configuración del asset
			_textureConfiguration = GameEngine.Instance.ResourcesManager.TextureConfigurationManager.GetTextureRegion(camera, Asset, Region);
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
	///		Dibuja el sprite
	/// </summary>
    public void Draw(Camera2D camera, Point position, Point center, Vector2 scale, float rotation, Color color)
    {
		Draw(camera, new Vector2(position.X, position.Y), new Vector2(center.X, center.Y), scale, rotation, color);
    }

	/// <summary>
	///		Dibuja la textura en una posición con escala
	/// </summary>
	public void Draw(Camera2D camera, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color)
	{
		TextureConfigurationManager.TextureResolved? textureConfiguration = LoadAsset(camera);

			// Dibuja la textura
			if (textureConfiguration is not null)
			{
				if (textureConfiguration.NineSliceConfiguration is not null)
					DrawNineSlice(camera, textureConfiguration.Texture, textureConfiguration.Region, textureConfiguration.NineSliceConfiguration,
								  new Rectangle((int) position.X, (int) position.Y, textureConfiguration.Region.Width, textureConfiguration.Region.Height),
								  origin, scale, rotation, color);
				else
					camera.SpriteBatchController.Draw(textureConfiguration.Texture, position, textureConfiguration.Region, 
													  origin, scale, SpriteEffect, color, rotation, 1);
			}
	}

	/// <summary>
	///		Dibuja la textura en un rectángulo concreto (ajusta al ancho y alto del rectángulo)
	/// </summary>
	public void Draw(Camera2D camera, Rectangle destination, Vector2 origin, float rotation, Color color)
	{
		TextureConfigurationManager.TextureResolved? textureConfiguration = LoadAsset(camera);

			// Dibuja la textura
			if (textureConfiguration is not null)
			{
				if (textureConfiguration.NineSliceConfiguration is not null)
					DrawNineSlice(camera, textureConfiguration.Texture, textureConfiguration.Region, textureConfiguration.NineSliceConfiguration,
								  destination, origin, new Vector2(1, 1), rotation, color);
				else
					camera.SpriteBatchController.Draw(textureConfiguration.Texture, destination, textureConfiguration.Region, 
													  origin, color, rotation, SpriteEffect, 1);
			}
	}

	/// <summary>
	///		Dibuja una textura definida como NineSlice
	/// </summary>
	private void DrawNineSlice(Camera2D camera, Texture2D texture, Rectangle region, TextureRegionNineSliceConfiguration nineSlice,
							   Rectangle destination, Vector2 origin, Vector2 vector2, float rotation, Color color)
	{
		Rectangle[] slices = GenerateSlices(texture, nineSlice);
        int dx0 = destination.X;
        int dx1 = destination.X + nineSlice.TopLeftWidth;
        int dx2 = destination.Right - nineSlice.TopRightWidth;
        int dy0 = destination.Y;
        int dy1 = destination.Y + nineSlice.TopLeftHeight;
        int dy2 = destination.Bottom - nineSlice.BottomLeftHeight;
        int centerW = destination.Width - nineSlice.TopLeftWidth - nineSlice.TopRightWidth;
        int centerH = destination.Height - nineSlice.TopLeftHeight - nineSlice.BottomLeftHeight;
        
			// Escala las esquinas proporcionalmente o las limita si el destino es más pequeño que el tamaño de las esquinas
			if (centerW < 0)
			{
				float scale = (float) destination.Width / (nineSlice.TopLeftWidth + nineSlice.TopRightWidth);

					dx1 = destination.X + (int) (nineSlice.TopLeftWidth * scale);
					dx2 = dx1;
					centerW = 0;
			}
			if (centerH < 0)
			{
				float scale = (float) destination.Height / (nineSlice.TopLeftHeight + nineSlice.BottomLeftHeight);

					dy1 = destination.Y + (int) (nineSlice.TopLeftHeight * scale);
					dy2 = dy1;
					centerH = 0;
			}
			// Dibuja la fila superior
			DrawSlice(camera, texture, slices[0], dx0, dy0, nineSlice.TopLeftWidth, nineSlice.TopLeftHeight, color);
			DrawSlice(camera, texture, slices[1], dx1, dy0, centerW, nineSlice.TopEdgeHeight, color);
			DrawSlice(camera, texture, slices[2], dx2, dy0, nineSlice.TopRightWidth, nineSlice.TopRightHeight, color);
			// Dibuja la fila central
			DrawSlice(camera, texture, slices[3], dx0, dy1, nineSlice.LeftEdgeWidth, centerH, color);
			DrawSlice(camera, texture, slices[4], dx1, dy1, centerW, centerH, color);
			DrawSlice(camera, texture, slices[5], dx2, dy1, nineSlice.RightEdgeWidth, centerH, color);
			// Dibuja la fila inferior
			DrawSlice(camera, texture, slices[6], dx0, dy2, nineSlice.BottomLeftWidth, nineSlice.BottomLeftHeight, color);
			DrawSlice(camera, texture, slices[7], dx1, dy2, centerW, nineSlice.BottomEdgeHeight, color);
			DrawSlice(camera, texture, slices[8], dx2, dy2, nineSlice.BottomRightWidth, nineSlice.BottomRightHeight, color);

		// Dibuja una sección de la imagen
		void DrawSlice(Camera2D camera2D, Texture2D texture, Rectangle source, int x, int y, int width, int height, Color color)
		{
			camera.SpriteBatchController.Draw(texture, new Rectangle(x, y, width, height), source, color);
		}
    }    

	/// <summary>
	///		Genera los trozos correspondientes a una textura de 9 secciones
	/// </summary>
    private Rectangle[] GenerateSlices(Texture2D texture, TextureRegionNineSliceConfiguration nineSlice)
    {
        int width = texture.Width;
        int height = texture.Height;
        int x0 = 0;
        int x1 = nineSlice.TopLeftWidth; // También BottomLeft.Width si son diferentes, asumimos consistencia
        int x2 = width - nineSlice.TopRightWidth;
        int y0 = 0;
        int y1 = nineSlice.TopLeftHeight; // También TopRight.Height
        int y2 = height - nineSlice.BottomLeftHeight;
        
			// Devuelve los rectángulos origen de las esquinas / bordes
			return [
						CreateSlice(x0, y0, nineSlice.TopLeftWidth, nineSlice.TopLeftHeight),
						CreateSlice(x1, y0, width - nineSlice.TopLeftWidth - nineSlice.TopRightWidth, nineSlice.TopEdgeHeight),
						CreateSlice(x2, y0, nineSlice.TopRightWidth, nineSlice.TopRightHeight),
						CreateSlice(x0, y1, nineSlice.LeftEdgeWidth, height - nineSlice.TopLeftHeight - nineSlice.BottomLeftHeight),
						CreateSlice(x1, y1, width - nineSlice.LeftEdgeWidth - nineSlice.RightEdgeWidth, height - nineSlice.TopEdgeHeight - nineSlice.BottomEdgeHeight),
						CreateSlice(x2, y1, nineSlice.RightEdgeWidth, height - nineSlice.TopRightHeight - nineSlice.BottomRightHeight),
						CreateSlice(x0, y2, nineSlice.BottomLeftWidth, nineSlice.BottomLeftHeight),
						CreateSlice(x1, y2, width - nineSlice.BottomLeftWidth - nineSlice.BottomRightWidth, nineSlice.BottomEdgeHeight),
						CreateSlice(x2, y2, nineSlice.BottomRightWidth, nineSlice.BottomRightHeight)
					];

		// Crea un rectángulo
		Rectangle CreateSlice(int x, int y, int width, int height) => new Rectangle(x, y, Math.Max(0, width), Math.Max(0, height));
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
