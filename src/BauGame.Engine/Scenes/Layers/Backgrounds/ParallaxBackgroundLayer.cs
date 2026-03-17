using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;

/// <summary>
///     Background con movimiento parallax
/// </summary>
public class ParallaxBackgroundLayer(AbstractScene scene, string name, string texture, int sortOrder, float speedMultiplier) : AbstractBackgroundLayer(scene, name, texture, sortOrder)
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
	protected override void UpdateSelf(Managers.GameContext gameContext)
	{
	}

    /// <summary>
    ///     Dibuja la capa
    /// </summary>
    protected override void DrawSelf(Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        TextureRegion? region = GetTextureRegion("background");

            // Si tenemos una textura
            if (region is not null)
            {
                Entities.Common.Sprites.SpriteDefinition sprite = new(region.Texture.Id, region.Name);
                float worldScreenWidth = renderingManager.Scene.Camera.ScreenViewport.Width / renderingManager.Scene.Camera.Zoom;
                float worldScreenHeight = renderingManager.Scene.Camera.ScreenViewport.Height / renderingManager.Scene.Camera.Zoom;
                Vector2 layerPosition = renderingManager.Scene.Camera.Position * SpeedMultiplier;
                int tileWidth = region.Region.Width;
                int tileHeight = region.Region.Height;
                int startX = (int) (layerPosition.X - 0.5f * worldScreenWidth) / tileWidth * tileWidth;
                int startY = (int) (layerPosition.Y - 0.5f * worldScreenHeight) / tileHeight * tileHeight;
                int endX = startX + (int) worldScreenWidth + tileWidth;
                int endY = startY + (int) worldScreenHeight + tileHeight;

                    // Dibuja las partes del fondo
                    for (int x = startX; x < endX; x += tileWidth)
                        for (int y = startY; y < endY; y += tileHeight)
                            renderingManager.SpriteRenderer.Draw(sprite, new Vector2(x, y), new Vector2(region.Region.Center.X, region.Region.Center.Y), 
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