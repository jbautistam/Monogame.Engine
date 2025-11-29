using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class CameraRenderer : IDisposable
{
    private SpriteBatch _spriteBatch;
    private Camera2D _camera;
    private GraphicsDevice _graphicsDevice;
    private Texture2D _whitePixel;

    public CameraRenderer(SpriteBatch spriteBatch, Camera2D camera, GraphicsDevice graphicsDevice)
    {
        _spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
        _camera = camera ?? throw new ArgumentNullException(nameof(camera));
        _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));

        _whitePixel = new Texture2D(_graphicsDevice, 1, 1);
        _whitePixel.SetData(new Color[] { Color.White });
    }

    // Mundo
    public void BeginWorld() => _spriteBatch.Begin(transformMatrix: _camera.GetFinalTransformMatrix());
    public void DrawWorld(Texture2D texture, Vector2 worldPosition, Color color) => _spriteBatch.Draw(texture, worldPosition, color);
    public void EndWorld() => _spriteBatch.End();

    // UI (píxeles)
    public void BeginUI() => _spriteBatch.Begin();
    public void DrawUI(Texture2D texture, Vector2 screenPosition, Color color) => _spriteBatch.Draw(texture, screenPosition, color);

    // UI (normalizado 0–1)
    public Vector2 NormalizeToScreen(Vector2 normalized) =>
        new Vector2(normalized.X * _graphicsDevice.Viewport.Width, normalized.Y * _graphicsDevice.Viewport.Height);

    public void DrawUINormalized(Texture2D texture, Vector2 normalizedPosition, Color color)
    {
        var pos = NormalizeToScreen(normalizedPosition);
        _spriteBatch.Draw(texture, pos, color);
    }
    // Mundo (afectado por cámara)
    public void BeginWorld() => _spriteBatch.Begin(transformMatrix: _camera.GetFinalTransformMatrix());
    public void DrawWorld(Texture2D texture, Vector2 worldPosition, Color color) => _spriteBatch.Draw(texture, worldPosition, color);
    public void DrawWorld(Texture2D texture, Rectangle worldDest, Color color) => _spriteBatch.Draw(texture, worldDest, color);
    public void EndWorld() => _spriteBatch.End();

    // UI (píxeles)
    public void BeginUI() => _spriteBatch.Begin();
    public void DrawUI(Texture2D texture, Vector2 screenPosition, Color color) => _spriteBatch.Draw(texture, screenPosition, color);
    public void DrawUI(Texture2D texture, Rectangle screenDest, Color color) => _spriteBatch.Draw(texture, screenDest, color);

    // UI (normalizado 0–1)
    public Vector2 NormalizeToScreen(Vector2 normalized) =>
        new Vector2(normalized.X * _graphicsDevice.Viewport.Width, normalized.Y * _graphicsDevice.Viewport.Height);

    public void DrawUINormalized(Texture2D texture, Vector2 normalizedPosition, Color color)
    {
        var pos = NormalizeToScreen(normalizedPosition);
        _spriteBatch.Draw(texture, pos, color);
    }

    public void DrawUINormalizedRectangle(Vector2 minNorm, Vector2 maxNorm, Color color, float thickness = 1f)
    {
        var min = NormalizeToScreen(minNorm);
        var max = NormalizeToScreen(maxNorm);
        var rect = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        DrawUIRectangle(rect, color, thickness);
    }

    public void DrawUIRectangle(Rectangle rect, Color color, float thickness = 1f)
    {
        var t = (int)thickness;
        _spriteBatch.Draw(_whitePixel, new Rectangle(rect.X, rect.Y, rect.Width, t), color);
        _spriteBatch.Draw(_whitePixel, new Rectangle(rect.X, rect.Bottom - t, rect.Width, t), color);
        _spriteBatch.Draw(_whitePixel, new Rectangle(rect.X, rect.Y, t, rect.Height), color);
        _spriteBatch.Draw(_whitePixel, new Rectangle(rect.Right - t, rect.Y, t, rect.Height), color);
    }
    public void EndUI() => _spriteBatch.End();

    public void Dispose() => _whitePixel?.Dispose();
}
