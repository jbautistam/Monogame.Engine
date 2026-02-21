using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;

/// <summary>
///     Fondo fijo
/// </summary>
public class FixedBackgroundLayer(AbstractScene scene, string name, string texture, int sortOrder) : AbstractBackgroundLayer(scene, name, texture, sortOrder)
{
    /// <summary>
    ///     Actualiza la capa para físicas
    /// </summary>
	protected override void UpdatePhysicsLayer(Managers.GameContext gameContext)
	{
	}

    /// <summary>
    ///     Actualiza la capa de fondo
    /// </summary>
	protected override void UpdateLayer(Managers.GameContext gameContext)
	{
	}

    /// <summary>
    ///     Dibuja la capa
    /// </summary>
    protected override void DrawLayer(Cameras.Camera2D camera, Managers.GameContext gameContext)
    {
        TextureRegion? region = GetTextureRegion("background");

            if (region is not null)
            {
                float worldScreenWidth = camera.ScreenViewport.Width / camera.Zoom;
                float worldScreenHeight = camera.ScreenViewport.Height / camera.Zoom;

                    if (!Tiled)
                    {
                        Vector2 scale = new(worldScreenWidth / region.Texture.Width, worldScreenHeight / region.Texture.Height);
                        Vector2 backgroundPosition = camera.Position - new Vector2(worldScreenWidth / 2f, worldScreenHeight / 2f);

                            // Dibujamos el fondo escalado para cubrir toda la pantalla visible
                            camera.SpriteBatchController.Draw(region.Texture, backgroundPosition, null, Vector2.Zero, scale, 
                                                              Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
                                                              Color, 0f, 0);
                    }
                    else
                    {
                        // Calculamos cuántos tiles necesitamos
                        int tileWidth = region.Texture.Width;
                        int tileHeight = region.Texture.Height;

                        int startX = (int) (camera.Position.X - worldScreenWidth / 2f) / tileWidth * tileWidth;
                        int startY = (int) (camera.Position.Y - worldScreenHeight / 2f) / tileHeight * tileHeight;

                        int endX = startX + (int) worldScreenWidth + tileWidth;
                        int endY = startY + (int) worldScreenHeight + tileHeight;

                            // Dibuja los diferentes tiles
                            for (int x = startX; x < endX; x += tileWidth)
                                for (int y = startY; y < endY; y += tileHeight)
                                    camera.SpriteBatchController.Draw(region.Texture, new Vector2(x, y), Color);
                    }
            }
    }

    /// <summary>
    ///     Finaliza el dibujo del fondo
    /// </summary>
	protected override void EndLayer()
	{
	}

    /// <summary>
    ///     Indica si se debe repetir
    /// </summary>
    public bool Tiled { get; set; }
}
