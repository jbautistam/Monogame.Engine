using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
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
    protected override void DrawSelf(Camera2D camera, Managers.GameContext gameContext)
    {
        TextureRegion? region = GetTextureRegion("background");

            // Si tenemos una textura
            if (region is not null)
            {
                float worldScreenWidth = camera.ScreenViewport.Width / camera.Zoom;
                float worldScreenHeight = camera.ScreenViewport.Height / camera.Zoom;

                // La capa se mueve más lento → posición "atrasada"
                Vector2 layerPosition = camera.Position * SpeedMultiplier;

                int tileWidth = region.Texture.Width;
                int tileHeight = region.Texture.Height;

                int startX = (int) (layerPosition.X - worldScreenWidth / 2f) / tileWidth * tileWidth;
                int startY = (int) (layerPosition.Y - worldScreenHeight / 2f) / tileHeight * tileHeight;

                int endX = startX + (int) worldScreenWidth + tileWidth;
                int endY = startY + (int) worldScreenHeight + tileHeight;

                // Dibuja las partes del fondo
                for (int x = startX; x < endX; x += tileWidth)
                    for (int y = startY; y < endY; y += tileHeight)
                        camera.SpriteBatchController.Draw(region.Texture, new Vector2(x, y), Color);
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