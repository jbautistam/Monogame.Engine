using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Backgrounds;

/// <summary>
///     Fondo avanzado con textura
/// </summary>
public class UiTextureBackground(Styles.UiStyle style) : UiAbstractBackground(style)
{
    /// <summary>
    ///     Modos de escalado para la textura
    /// </summary>
    public enum TextureScaleMode
    {
        /// <summary>Estira para llenar</summary>
        Stretch,
        /// <summary>Repite en mosaico</summary>
        Tile,
        /// <summary>Centra sin escalar</summary>
        Center,
        /// <summary>Escala manteniendo aspect ratio para que quepa completamente</summary>
        Fit,
        /// <summary>Escala manteniendo aspect ratio cubriendo todo el área (puede recortar)</summary>
        Fill,
        /// <summary>Divide en 9 partes para UI escalable</summary>
        NineSlice
    }

    /// <summary>
	///		Actualiza el control
	/// </summary>
	public override void Update(Managers.GameContext gameContext)
	{
        if (!IsInitialized)
        {
            // Carga la textura
            if (!string.IsNullOrWhiteSpace(Texture))
                Texture2D = Style.Layer.Scene.LoadSceneAsset<Texture2D>(Texture);
            // Indica que ya está inicializado
           IsInitialized = true;
        }
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, Rectangle position, Managers.GameContext gameContext)
	{
		if (Texture2D is not null)
            switch (ScaleMode)
            {
                case TextureScaleMode.Stretch:
                        camera.SpriteBatchController.Draw(Texture2D, position, Color * Opacity);
                    break;
                case TextureScaleMode.Tile:
                        DrawTiled(camera, Texture2D, position);
                    break;
                case TextureScaleMode.Center:
                        DrawCentered(camera, Texture2D, position);
                    break;
                case TextureScaleMode.Fit:
                        DrawFitted(camera, Texture2D, position, true);
                    break;
                case TextureScaleMode.Fill:
                        DrawFitted(camera, Texture2D, position, false);
                    break;
                case TextureScaleMode.NineSlice:
                        DrawNineSliced(camera, Texture2D, position);
                    break;
            }
	}

    /// <summary>
    ///     Dibuja el fondo y lo repite si es necesario
    /// </summary>
    private void DrawTiled(Camera2D camera, Texture2D texture, Rectangle bounds)
    {
        for (int y = bounds.Top; y < bounds.Bottom; y += texture.Height)
            for (int x = bounds.Left; x < bounds.Right; x += texture.Width)
            {
                int width = Math.Min(texture.Width, bounds.Right - x);
                int height = Math.Min(texture.Height, bounds.Bottom - y);
                    
                    // Dibuja la textura
                    camera.SpriteBatchController.Draw(texture, new Rectangle(x, y, width, height), 
                                                      new Rectangle(0, 0, width, height), Color * Opacity);
            }
    }

    /// <summary>
    ///     Dibuja la textura centrada
    /// </summary>
    private void DrawCentered(Camera2D camera, Texture2D texture, Rectangle bounds)
    {
        float x = bounds.X + 0.5f * (bounds.Width - texture.Width);
        float y = bounds.Y + 0.5f * (bounds.Height - texture.Height);
            
            // Dibua la textura
            camera.SpriteBatchController.Draw(texture, new Vector2(x, y), Color * Opacity);
    }

    /// <summary>
    ///     Dibuja manteniendo aspect ratio para que quepa completamente (con o sin recorte
    /// </summary>
    private void DrawFitted(Camera2D camera, Texture2D texture, Rectangle bounds, bool fitInside)
    {
        float scaleX = (float) bounds.Width / texture.Width;
        float scaleY = (float) bounds.Height / texture.Height;
        float scale = fitInside ? Math.Min(scaleX, scaleY) : Math.Max(scaleX, scaleY);
        int newWidth = (int) (texture.Width * scale);
        int newHeight = (int) (texture.Height * scale);
        int x = bounds.X + (bounds.Width - newWidth) / 2;
        int y = bounds.Y + (bounds.Height - newHeight) / 2;
            
            // Dibuja el fondo
            camera.SpriteBatchController.Draw(texture, new Rectangle(x, y, newWidth, newHeight), Color * Opacity);
    }
    
    /// <summary>
    ///     Dibuja en nueva partes para un fondo escalable
    /// </summary>
    private void DrawNineSliced(Camera2D camera, Texture2D texture, Rectangle bounds)
    {
        int sliceWidth = Math.Min(texture.Width, texture.Height) / 3;
        int sliceHeight = Math.Min(texture.Width, texture.Height) / 3;

            // Cambia el tamaño de los trozos si se ha definido alguno
            if (SliceSize.X > 0 && SliceSize.Y > 0)
            {
                sliceWidth = (int) SliceSize.X;
                sliceHeight = (int) SliceSize.Y;
            }
            // Dibuja las esquinas (sin escalar)
            DrawCorners();
            // Dibuja los bordes (escalando)
            DrawBorders();

        // Dibuja las esquinas (sin escalar)
        void DrawCorners()
        {
            // Superior izquierda
            camera.SpriteBatchController.Draw(texture, new Rectangle(bounds.X, bounds.Y, sliceWidth, sliceHeight),
                                              new Rectangle(0, 0, sliceWidth, sliceHeight), 
                                              Color * Opacity);
            // Superior derecha
            camera.SpriteBatchController.Draw(texture, new Rectangle(bounds.Right - sliceWidth, bounds.Y, sliceWidth, sliceHeight),
                                              new Rectangle(texture.Width - sliceWidth, 0, sliceWidth, sliceHeight), 
                                              Color * Opacity);
            // Inferior izquierda
            camera.SpriteBatchController.Draw(texture, new Rectangle(bounds.X, bounds.Bottom - sliceHeight, sliceWidth, sliceHeight),
                                              new Rectangle(0, texture.Height - sliceHeight, sliceWidth, sliceHeight), 
                                              Color * Opacity);
            // Inferior derecha
            camera.SpriteBatchController.Draw(texture, 
                                              new Rectangle(bounds.Right - sliceWidth, bounds.Bottom - sliceHeight, sliceWidth, sliceHeight),
                                              new Rectangle(texture.Width - sliceWidth, texture.Height - sliceHeight, sliceWidth, sliceHeight), 
                                              Color * Opacity);
        }

        // Dibuja los bordes escalando a la posición en pantalla
        void DrawBorders()
        {   
            int centerWidth = bounds.Width - sliceWidth * 2;
            int centerHeight = bounds.Height - sliceHeight * 2;
            
                // Superior
                camera.SpriteBatchController.Draw(texture,
                                                  new Rectangle(bounds.X + sliceWidth, bounds.Y, centerWidth, sliceHeight),
                                                  new Rectangle(sliceWidth, 0, texture.Width - sliceWidth * 2, sliceHeight), 
                                                  Color * Opacity);
                // Inferior
                camera.SpriteBatchController.Draw(texture,
                                                  new Rectangle(bounds.X + sliceWidth, bounds.Bottom - sliceWidth, centerWidth, sliceHeight),
                                                  new Rectangle(sliceWidth, texture.Height - sliceWidth, texture.Width - sliceWidth * 2, sliceHeight), 
                                                  Color * Opacity);
                // Izquierda
                camera.SpriteBatchController.Draw(texture,
                                                  new Rectangle(bounds.X, bounds.Y + sliceWidth, sliceWidth, centerHeight),
                                                  new Rectangle(0, sliceWidth, sliceWidth, texture.Height - sliceHeight * 2), 
                                                  Color * Opacity);
                // Derecha
                camera.SpriteBatchController.Draw(texture,
                                                  new Rectangle(bounds.Right - sliceWidth, bounds.Y + sliceWidth, sliceWidth, centerHeight),
                                                  new Rectangle(texture.Width - sliceWidth, sliceWidth, sliceWidth, texture.Height - sliceHeight * 2), 
                                                  Color * Opacity);
                // Relleno
                camera.SpriteBatchController.Draw(texture,
                                                  new Rectangle(bounds.X + sliceWidth, bounds.Y + sliceWidth, centerWidth, centerHeight),
                                                  new Rectangle(sliceWidth, sliceWidth, texture.Width - sliceWidth * 2, texture.Height - sliceHeight * 2), 
                                                  Color * Opacity);
        }
    }

    /// <summary>
    ///     Nombre de la textura
    /// </summary>
    public string? Texture { get; set; }

    /// <summary>
    ///     Textura
    /// </summary>
    private Texture2D? Texture2D { get; set; }
        
    /// <summary>
    ///     Modo de escalado de la textura
    /// </summary>
    public TextureScaleMode ScaleMode { get; set; } = TextureScaleMode.Stretch;

    /// <summary>
    ///     Tamaño para los fondos que se dibujan como Slice 9
    /// </summary>
    public Vector2 SliceSize { get; set; } = Vector2.Zero;
}
