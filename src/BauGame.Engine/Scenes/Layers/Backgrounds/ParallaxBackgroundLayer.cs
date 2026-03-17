using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;

/// <summary>
///     Background con movimiento parallax
/// </summary>
public class ParallaxBackgroundLayer(AbstractScene scene, string name, string asset, string? region, int sortOrder, float speedMultiplier) 
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
                Vector2 layerPosition = renderingManager.Scene.Camera.Position * SpeedMultiplier;
                int tileWidth = (int) size.Width;
                int tileHeight = (int) size.Height;
                int startX = (int) (layerPosition.X - 0.5f * worldScreenWidth) / tileWidth * tileWidth;
                int startY = (int) (layerPosition.Y - 0.5f * worldScreenHeight) / tileHeight * tileHeight;
                int endX = startX + (int) worldScreenWidth + tileWidth;
                int endY = startY + (int) worldScreenHeight + tileHeight;

                    // Dibuja las partes del fondo
                    for (int x = startX; x < endX; x += tileWidth)
                        for (int y = startY; y < endY; y += tileHeight)
                            renderingManager.SpriteRenderer.Draw(Sprite, new Vector2(x, y), new Vector2(0.5f * tileWidth, 0.5f * tileHeight),
                                                                 Vector2.One, 0, Color);
            }
    }

    /// <summary>
    ///     Finaliza el dibujo del fondo
    /// </summary>
	protected override void EndLayer()
	{
	}

    /// <summary>
    ///     Multiplicador para la velocidad del movimiento
    /// </summary>
    public float SpeedMultiplier { get; set; } = speedMultiplier;
}