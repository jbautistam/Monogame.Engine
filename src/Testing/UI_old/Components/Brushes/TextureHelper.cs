using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Helper para texturas compartidas
/// </summary>
public static class TextureHelper
{
    private static Texture2D _pixelTexture;

    public static Texture2D GetPixelTexture(GraphicsDevice device)
    {
        if (_pixelTexture == null || _pixelTexture.IsDisposed)
        {
            _pixelTexture = new Texture2D(device, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });
        }
        return _pixelTexture;
    }

    public static void Dispose()
    {
        _pixelTexture?.Dispose();
        _pixelTexture = null;
    }
}
