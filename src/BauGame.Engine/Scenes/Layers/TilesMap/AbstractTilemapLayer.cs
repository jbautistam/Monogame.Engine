using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.TilesMap;

/// <summary>
///		Layer de fondo para <see cref="MapDefinition"/>
/// </summary>
public abstract class AbstractTilemapLayer(AbstractScene scene, string name, int physicsLayer, int sortOrder) : AbstractLayer(scene, name, LayerType.Game, sortOrder)
{
	/// <summary>
	///		Actualiza los datos de la capa
	/// </summary>
	protected override void UpdateLayer(GameTime gameTime)
	{
	}

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	protected override void DrawLayer(Camera2D camera, GameTime gameTime)
	{
        if (Map.TileWidth > 0 && Map.TileHeight > 0)
        {
            (int startX, int endX, int startY, int endY) = GetVisibleTileRange(camera);

                // Dibuja cada una de las capa
                for (int y = startY; y < endY; y++)
                    for (int x = startX; x < endX; x++)
                    {
                        int index = y * Map.Width + x;
                        int tileId = Map.Tiles[index];
                        TileDefinition? tile = Tiles.FirstOrDefault(item => item.Id == tileId);

/*                                
                            if (tile is not null)
                            {
                                int drawTileId = tileId;

                                if (def.IsAnimated && def.AnimationFrames?.Count > 0)
                                {
                                    float timeOffset = (x * 7 + y * 13) * 0.1f;
                                    int frameIndex = (int)((Game1.TotalGameTime + timeOffset) / def.FrameDuration) % def.AnimationFrames.Count;
                                    drawTileId = def.AnimationFrames[frameIndex];
                                }

                                int tilesPerRow = _texture.Width / tileW;
                                int srcX = (drawTileId % tilesPerRow) * tileW;
                                int srcY = (drawTileId / tilesPerRow) * tileH;
                                var sourceRect = new Rectangle(srcX, srcY, tileW, tileH);

                                // Posición EXACTA en pantalla (con fracciones)
                                Vector2 screenPos = new Vector2(
                                    x * tileW - cameraPosition.X,
                                    y * tileH - cameraPosition.Y
                                );

                                spriteBatch.Draw(_texture, screenPos, sourceRect, Color.White);
                            }
*/
                    }
        }
	}

	/// <summary>
	///		Obtiene las coordenadas de los patrones visibles del mapa
	/// </summary>
    private (int startX, int endX, int startY, int endY) GetVisibleTileRange(Camera2D camera)
    {
        float worldVisibleWidth = camera.ScreenViewport.Width / camera.Zoom;
        float worldVisibleHeight = camera.ScreenViewport.Height / camera.Zoom;
        float worldLeft = camera.Position.X - worldVisibleWidth / 2f;
        float worldRight = camera.Position.X + worldVisibleWidth / 2f;
        float worldTop = camera.Position.Y - worldVisibleHeight / 2f;
        float worldBottom = camera.Position.Y + worldVisibleHeight / 2f;

			// Devuelve las coordenadas
			return (Math.Max(0, (int) Math.Floor(worldLeft / Map.TileWidth)), 
					Math.Min(Map.Width, (int) Math.Ceiling(worldRight / Map.TileWidth)), 
					Math.Max(0, (int) Math.Floor(worldTop / Map.TileHeight)), 
					Math.Min(Map.Height, (int) Math.Ceiling(worldBottom / Map.TileHeight)));
    }

	/// <summary>
	///		Finaliza la capa
	/// </summary>
	protected override void EndLayer()
	{
	}

	/// <summary>
	///		Código de la capa de físicas (para colisiones, por ejemplo)
	/// </summary>
	public int PhysicsLayer { get; } = physicsLayer;

	/// <summary>
	///		Patrones
	/// </summary>
	public List<TileDefinition> Tiles { get; } = [];

	/// <summary>
	///		Mapa
	/// </summary>
	public MapDefinition Map { get; protected set; } = new();
}