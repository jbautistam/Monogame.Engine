using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Scenes.Layers.Backgrounds;

/// <summary>
///     Fondo fijo
/// </summary>
public class FixedBackgroundLayer(AbstractScene scene, string name, string asset, string? region, int sortOrder) 
                    : AbstractBackgroundLayer(scene, name, asset, region, sortOrder)
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
    protected override void DrawSelf(Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        Entities.Common.Size size = Sprite.GetSize();

            if (size.Width > 0 && size.Height > 0)
            {
                float worldScreenWidth = renderingManager.Scene.Camera.ScreenViewport.Width / renderingManager.Scene.Camera.Zoom;
                float worldScreenHeight = renderingManager.Scene.Camera.ScreenViewport.Height / renderingManager.Scene.Camera.Zoom;

                    if (!Tiled)
                    {
                        Vector2 scale = new(worldScreenWidth / size.Width, worldScreenHeight / size.Height);
                        Vector2 backgroundPosition = renderingManager.Scene.Camera.Position - new Vector2(worldScreenWidth / 2f, worldScreenHeight / 2f);

                            // Dibujamos el fondo escalado para cubrir toda la pantalla visible
                            renderingManager.SpriteRenderer.Draw(Sprite, backgroundPosition, Vector2.Zero, scale, 0, Color);
                    }
                    else
                    {
                        int tileWidth = (int) size.Width;
                        int tileHeight = (int) size.Height;
                        int startX = (int) (renderingManager.Scene.Camera.Position.X - worldScreenWidth / 2f) / tileWidth * tileWidth;
                        int startY = (int) (renderingManager.Scene.Camera.Position.Y - worldScreenHeight / 2f) / tileHeight * tileHeight;
                        int endX = startX + (int) worldScreenWidth + tileWidth;
                        int endY = startY + (int) worldScreenHeight + tileHeight;

                            // Dibuja los diferentes tiles
                            for (int x = startX; x < endX; x += tileWidth)
                                for (int y = startY; y < endY; y += tileHeight)
                                    renderingManager.SpriteRenderer.Draw(Sprite, new Vector2(x, y), Vector2.Zero, Vector2.One, 0, Color);
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
