using Microsoft.Xna.Framework;
using GameEngine.Cameras;
using GameEngine.Math;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Layers;

namespace GameEngine.Debugging;

public class BehaviorDebugger
{
    private readonly SpriteBatch _spriteBatch;
    private readonly SpriteFont _font;
    private readonly Texture2D _pixel;

    public bool ShowCameraBounds { get; set; } = true;
    public bool ShowLayerInfo { get; set; } = true;
    public bool ShowBehaviorInfo { get; set; } = true;

    public BehaviorDebugger(SpriteBatch spriteBatch, SpriteFont font, GraphicsDevice device)
    {
        _spriteBatch = spriteBatch;
        _font = font;
            
        _pixel = new Texture2D(device, 1, 1);
        _pixel.SetData([ Color.White ]);
    }

    public void DrawCameraDebug(CameraDirector director)
    {
        if (ShowCameraBounds == false) return;

        foreach (var camera in director.Cameras)
        {
            var viewport = camera.CameraViewport;

            var visible = camera.State.GetVisibleBounds(viewport.ToViewport(camera.Director.Viewport));
            DrawRectangle(visible, Color.Yellow * 0.3f);
            DrawRectangleOutline(visible, Color.Yellow);

            DrawString($"Camera: {camera.Name}", new Vector2(viewport.X + 5, viewport.Y + 5), Color.White);
            DrawString($"Zoom: {camera.State.Zoom:F2}", new Vector2(viewport.X + 5, viewport.Y + 25), Color.White);
        }
    }

    public void DrawLayerDebug(LayerDirector director)
    {
        if (ShowLayerInfo == false) return;

        int y = 100;
        foreach (var layer in director.Layers)
        {
            DrawString($"Layer: {layer.Name} (Order: {layer.DrawOrder})", new Vector2(5, y), Color.Cyan);
            y += 20;
            DrawString($"  Actors: {layer.Actors.Count}, Behaviors: {layer.Behaviors.Count}", new Vector2(5, y), Color.Gray);
            y += 20;
        }
    }

    private void DrawRectangle(RectangleF rect, Color color)
    {
        _spriteBatch.Draw(_pixel, rect.Position, null, color, 0f, Vector2.Zero, rect.Size, SpriteEffects.None, 0f);
    }

    private void DrawRectangleOutline(RectangleF rect, Color color)
    {
        float thickness = 2f;
            
        _spriteBatch.Draw(_pixel, new Vector2(rect.Left, rect.Top), null, color, 0f, Vector2.Zero, new Vector2(rect.Width, thickness), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_pixel, new Vector2(rect.Left, rect.Bottom - thickness), null, color, 0f, Vector2.Zero, new Vector2(rect.Width, thickness), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_pixel, new Vector2(rect.Left, rect.Top), null, color, 0f, Vector2.Zero, new Vector2(thickness, rect.Height), SpriteEffects.None, 0f);
        _spriteBatch.Draw(_pixel, new Vector2(rect.Right - thickness, rect.Top), null, color, 0f, Vector2.Zero, new Vector2(thickness, rect.Height), SpriteEffects.None, 0f);
    }

    private void DrawString(string text, Vector2 position, Color color)
    {
        _spriteBatch.DrawString(_font, text, position, color);
    }
}
