using Bau.Libraries.BauGame.Engine.Managers.Resources;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering;

/// <summary>
///     Comando para dibujar un sprite
/// </summary>
public class SpriteRenderCommand : AbstractRenderCommand
{
    /// <summary>
    ///     Modo de dibujo
    /// </summary>
    public enum DrawType
    {
        /// <summary>Normal</summary>
        Normal,
        /// <summary>Textura repetida (por ejemplo, fondos)</summary>
        Tiled
    }

    /// <summary>
    ///     Ejecuta el comando
    /// </summary>
    public override void Execute(CameraDirector director, SpriteBatch spriteBatch)
    {
		TextureConfigurationManager.TextureResolved? textureConfiguration = LoadAsset(director);

			if (textureConfiguration is not null)
			{
				// Cambia el destino si no se ha creado
				if (Transform.Destination.IsEmpty)
					Transform.Destination = new Rectangle((int) Transform.Coordinates.Position.X, (int) Transform.Coordinates.Position.Y, 
														  textureConfiguration.Texture.Width, textureConfiguration.Texture.Height);
				// Dibuja la textura adecuada
				if (textureConfiguration.NineSliceConfiguration is not null)
					DrawNineSlice(textureConfiguration, textureConfiguration.NineSliceConfiguration, spriteBatch);
				else
					switch (DrawMode)
					{
						case DrawType.Normal:
								Draw(textureConfiguration, spriteBatch);
							break;
						case DrawType.Tiled:
								DrawTiled(director, textureConfiguration, spriteBatch);
							break;
					}
			}
    }

	/// <summary>
	///		Carga la textura si es la primera vez o ha habido modificaciones
	/// </summary>
	private TextureConfigurationManager.TextureResolved? LoadAsset(CameraDirector director)
	{
		return GameEngine.Instance.ResourcesManager.TextureConfigurationManager.GetTextureRegion(director.Scene, Sprite.Asset, Sprite.Region);
	}

	/// <summary>
	///		Dibuja la textura en modo normal
	/// </summary>
	private void Draw(TextureConfigurationManager.TextureResolved textureConfiguration, SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(textureConfiguration.Texture, Transform.Destination, textureConfiguration.Region,
						 Presentation.Color, Transform.Coordinates.Rotation, Transform.Coordinates.Origin, Presentation.Effect, 1);
	}

	/// <summary>
	///		Dibuja la textura por teselas
	/// </summary>
	private void DrawTiled(CameraDirector director, TextureConfigurationManager.TextureResolved textureConfiguration, 
						   SpriteBatch spriteBatch)
	{
		Cameras.Camera2D? camera = director.Scene.Camera;

			if (camera is not null)
			{
				float worldScreenWidth = camera.ScreenViewport.Width / camera.Zoom;
				float worldScreenHeight = camera.ScreenViewport.Height / camera.Zoom;
				int tileWidth = textureConfiguration.Texture.Width;
				int tileHeight = textureConfiguration.Texture.Height;
				int startX = (int) (camera.Position.X - worldScreenWidth / 2f) / tileWidth * tileWidth;
				int startY = (int) (camera.Position.Y - worldScreenHeight / 2f) / tileHeight * tileHeight;
				int endX = startX + (int) worldScreenWidth + tileWidth;
				int endY = startY + (int) worldScreenHeight + tileHeight;

					//TODO: esto no está bien, debería tener en cuenta que puede que estemos en una región no en toda la textura
					// Dibuja los diferentes tiles
					for (int x = startX; x < endX; x += tileWidth)
						for (int y = startY; y < endY; y += tileHeight)
							spriteBatch.Draw(textureConfiguration.Texture, new Vector2(x, y), Presentation.Color);
			}
	}

	/// <summary>
	///		Dibuja una textura definida como NineSlice
	/// </summary>
	private void DrawNineSlice(TextureConfigurationManager.TextureResolved textureConfiguration, TextureRegionNineSliceConfiguration nineSlice, SpriteBatch spriteBatch)
	{
		Rectangle[] slices = GenerateSlices(textureConfiguration.Region.Width, textureConfiguration.Region.Height, nineSlice);
        int dx0 = Transform.Destination.X;
        int dx1 = Transform.Destination.X + nineSlice.TopLeftWidth;
        int dx2 = Transform.Destination.Right - nineSlice.TopRightWidth;
        int dy0 = Transform.Destination.Y;
        int dy1 = Transform.Destination.Y + nineSlice.TopLeftHeight;
        int dy2 = Transform.Destination.Bottom - nineSlice.BottomLeftHeight;
        int centerW = Transform.Destination.Width - nineSlice.TopLeftWidth - nineSlice.TopRightWidth;
        int centerH = Transform.Destination.Height - nineSlice.TopLeftHeight - nineSlice.BottomLeftHeight;
        
			// TODO: debería tener en cuenta que puede estar en una región
			// Escala las esquinas proporcionalmente o las limita si el destino es más pequeño que el tamaño de las esquinas
			if (centerW < 0)
			{
				float scale = (float) Transform.Destination.Width / (nineSlice.TopLeftWidth + nineSlice.TopRightWidth);

					dx1 = Transform.Destination.X + (int) (nineSlice.TopLeftWidth * scale);
					dx2 = dx1;
					centerW = 0;
			}
			if (centerH < 0)
			{
				float scale = (float) Transform.Destination.Height / (nineSlice.TopLeftHeight + nineSlice.BottomLeftHeight);

					dy1 = Transform.Destination.Y + (int) (nineSlice.TopLeftHeight * scale);
					dy2 = dy1;
					centerH = 0;
			}
			// Dibuja la fila superior
			DrawSlice(spriteBatch, textureConfiguration.Texture, slices[0], dx0, dy0, nineSlice.TopLeftWidth, nineSlice.TopLeftHeight, Presentation.Color);
			DrawSlice(spriteBatch, textureConfiguration.Texture, slices[1], dx1, dy0, centerW, nineSlice.TopEdgeHeight, Presentation.Color);
			DrawSlice(spriteBatch, textureConfiguration.Texture, slices[2], dx2, dy0, nineSlice.TopRightWidth, nineSlice.TopRightHeight, Presentation.Color);
			// Dibuja la fila central
			DrawSlice(spriteBatch, textureConfiguration.Texture, slices[3], dx0, dy1, nineSlice.LeftEdgeWidth, centerH, Presentation.Color);
			DrawSlice(spriteBatch, textureConfiguration.Texture, slices[4], dx1, dy1, centerW, centerH, Presentation.Color);
			DrawSlice(spriteBatch, textureConfiguration.Texture, slices[5], dx2, dy1, nineSlice.RightEdgeWidth, centerH, Presentation.Color);
			// Dibuja la fila inferior
			DrawSlice(spriteBatch, textureConfiguration.Texture, slices[6], dx0, dy2, nineSlice.BottomLeftWidth, nineSlice.BottomLeftHeight, Presentation.Color);
			DrawSlice(spriteBatch, textureConfiguration.Texture, slices[7], dx1, dy2, centerW, nineSlice.BottomEdgeHeight, Presentation.Color);
			DrawSlice(spriteBatch, textureConfiguration.Texture, slices[8], dx2, dy2, nineSlice.BottomRightWidth, nineSlice.BottomRightHeight, Presentation.Color);
			// Dibuja el relleno
			if (nineSlice.FillBackground)
				DrawSlice(spriteBatch, textureConfiguration.Texture, slices[9], dx1, dy1, slices[9].Width, slices[9].Height, Presentation.Color);

		// Dibuja una sección de la imagen
		void DrawSlice(SpriteBatch spriteBatch, Texture2D texture, Rectangle source, int x, int y, int width, int height, Color color)
		{
			spriteBatch.Draw(texture, new Rectangle(x, y, width, height), source, color);
		}
    }    

	/// <summary>
	///		Genera los trozos correspondientes a una textura de 9 secciones
	/// </summary>
    private Rectangle[] GenerateSlices(int width, int height, TextureRegionNineSliceConfiguration nineSlice)
    {
        int x0 = 0;
        int x1 = nineSlice.TopLeftWidth; // También BottomLeft.Width si son diferentes, asumimos consistencia
        int x2 = width - nineSlice.TopRightWidth;
        int y0 = 0;
        int y1 = nineSlice.TopLeftHeight; // También TopRight.Height
        int y2 = height - nineSlice.BottomLeftHeight;
        
			// Devuelve los rectángulos origen de las esquinas / bordes
			return [
						CreateSlice(x0, y0, nineSlice.TopLeftWidth, nineSlice.TopLeftHeight),
						CreateSlice(x1, y0, width - nineSlice.EdgesWidth, nineSlice.TopEdgeHeight),
						CreateSlice(x2, y0, nineSlice.TopRightWidth, nineSlice.TopRightHeight),
						CreateSlice(x0, y1, nineSlice.LeftEdgeWidth, height - nineSlice.EdgesHeight),
						CreateSlice(x1, y1, width - nineSlice.EdgesWidth, height - nineSlice.EdgesHeight),
						CreateSlice(x2, y1, nineSlice.RightEdgeWidth, height - nineSlice.EdgesHeight),
						CreateSlice(x0, y2, nineSlice.BottomLeftWidth, nineSlice.BottomLeftHeight),
						CreateSlice(x1, y2, width - nineSlice.EdgesWidth, nineSlice.BottomEdgeHeight),
						CreateSlice(x2, y2, nineSlice.BottomRightWidth, nineSlice.BottomRightHeight),
						CreateSlice(x1, y1, width - nineSlice.EdgesWidth, height - nineSlice.EdgesHeight)
					];

		// Crea un rectángulo
		Rectangle CreateSlice(int x, int y, int width, int height) => new(x, y, Math.Max(0, width), Math.Max(0, height));
    }

    /// <summary>
    ///     Sprite a dibujar
    /// </summary>
    public required Entities.Common.SpriteDefinition Sprite { get; init; }

    /// <summary>
    ///     Transformación
    /// </summary>
    public TransformRenderModel Transform { get; } = new();

    /// <summary>
    ///     Datos de presentación
    /// </summary>
    public PresentationRenderModel Presentation { get; } = new();

    /// <summary>
    ///     Modo de dibujo
    /// </summary>
    public DrawType DrawMode { get; set; } = DrawType.Normal;
}
