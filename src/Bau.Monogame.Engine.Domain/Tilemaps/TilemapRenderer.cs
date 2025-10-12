using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Bau.Monogame.Engine.Domain.Tilemaps;

public class TilemapRenderer
{
    private List<Tilemap> _tilemaps;
    private TileAnimationManager _animationManager;

    public TilemapRenderer(IEnumerable<Tilemap> tilemaps)
    {
        _tilemaps = tilemaps.OrderBy(t => t.Layer).ToList();
        _animationManager = new TileAnimationManager();
    }

    public void Draw(SpriteBatch spriteBatch, Camera2D camera, GameTime gameTime)
    {
        var layers = _tilemaps.GroupBy(t => t.Layer).OrderBy(g => g.Key);

        foreach (var layerGroup in layers)
        {
            spriteBatch.Begin(transformMatrix: camera.Transform, samplerState: SamplerState.PointClamp);

            foreach (var tilemap in layerGroup)
            {
                DrawTilemap(spriteBatch, tilemap, camera, gameTime);
            }

            spriteBatch.End();
        }
    }

    private void DrawTilemap(SpriteBatch spriteBatch, Tilemap tilemap, Camera2D camera, GameTime gameTime)
    {
        var camWorldMin = camera.ScreenToWorld(Vector2.Zero);
        var camWorldMax = camera.ScreenToWorld(new Vector2(camera.Viewport.Width, camera.Viewport.Height));

        int startX = Math.Max(0, (int)Math.Floor(camWorldMin.X / tilemap.TileSize));
        int startY = Math.Max(0, (int)Math.Floor(camWorldMin.Y / tilemap.TileSize));
        int endX = Math.Min(tilemap.Width, (int)Math.Ceiling(camWorldMax.X / tilemap.TileSize));
        int endY = Math.Min(tilemap.Height, (int)Math.Ceiling(camWorldMax.Y / tilemap.TileSize));

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                int tileIndex = tilemap.GetTileIndex(x, y);
                if (tileIndex <= 0) continue;

                if (tilemap.HasAnimationAt(x, y))
                {
                    var animatedTile = tilemap.GetAnimatedTile(x, y);
                    tileIndex = animatedTile.GetCurrentFrameIndex(gameTime);
                }

                var sourceRect = tilemap.Tileset.GetSourceRect(tileIndex);
                if (!sourceRect.HasValue) continue;

                var worldPos = tilemap.GetWorldPosition(x, y);

                spriteBatch.Draw(
                    tilemap.Tileset.Texture,
                    worldPos,
                    sourceRect.Value,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
