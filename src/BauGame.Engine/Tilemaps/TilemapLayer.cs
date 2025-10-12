using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Bau.Monogame.Engine.Domain.Tilemaps;

public enum TilemapLayerType
{
    Background = 0,
    Midground = 1,
    Foreground = 2
}

public class TilemapLayer : Layer
{
    public List<Tilemap> Tilemaps { get; } = new();
    private TilemapRenderer _renderer;
    private GameTime _currentGameTime;

    public TilemapLayer(string name, int sortOrder, GraphicsDevice graphicsDevice)
        : base(name, sortOrder, graphicsDevice)
    {
        _renderer = new TilemapRenderer(new List<Tilemap>());
    }

    public void AddTilemap(Tilemap tilemap)
    {
        Tilemaps.Add(tilemap);
        _renderer = new TilemapRenderer(Tilemaps);
    }

    public override void Update(GameTime gameTime)
    {
        _currentGameTime = gameTime;
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive || Tilemaps.Count == 0 || Camera == null) return;

        if (Effect != null && Effect.IsEnabled)
        {
            SetupRenderTarget();
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _graphicsDevice.Clear(Color.Transparent);

            var layerSpriteBatch = new SpriteBatch(_graphicsDevice);
            DrawTilemaps(layerSpriteBatch);
            layerSpriteBatch.End();

            _graphicsDevice.SetRenderTarget(null);
            Effect.Apply(spriteBatch, _renderTarget);
        }
        else
        {
            DrawTilemaps(spriteBatch);
        }
    }

    private void DrawTilemaps(SpriteBatch spriteBatch)
    {
        if (_currentGameTime == null) return;

        spriteBatch.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);
        _renderer.Draw(spriteBatch, Camera, _currentGameTime);
        spriteBatch.End();
    }

    private void SetupRenderTarget()
    {
        if (_renderTarget == null ||
            _renderTarget.Width != _graphicsDevice.Viewport.Width ||
            _renderTarget.Height != _graphicsDevice.Viewport.Height)
        {
            _renderTarget?.Dispose();
            _renderTarget = new RenderTarget2D(_graphicsDevice,
                _graphicsDevice.Viewport.Width,
                _graphicsDevice.Viewport.Height);
        }
    }
}
