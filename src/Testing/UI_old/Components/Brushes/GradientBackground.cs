using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Fondo con degradado
/// </summary>
public class GradientBackground : IBackground
{
    public enum GradientType { Vertical, Horizontal, Radial }

    public GradientType Type { get; set; } = GradientType.Vertical;
    public Color StartColor { get; set; }
    public Color EndColor { get; set; }
        
    // Caché de textura de degradado (se regenera si cambian colores o tamaño)
    private Texture2D? _cachedGradient;
    private int _cachedWidth;
    private int _cachedHeight;
    private Color _cachedStart;
    private Color _cachedEnd;
    private GradientType _cachedType;

    public bool HasContent => StartColor.A > 0 || EndColor.A > 0;

    public GradientBackground(Color startColor, Color endColor, GradientType type = GradientType.Vertical)
    {
        StartColor = startColor;
        EndColor = endColor;
        Type = type;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle bounds, Rectangle contentBounds)
    {
        if (!HasContent) return;

        // Regenera la textura si es necesario
        if (_cachedGradient == null || 
            _cachedGradient.IsDisposed ||
            _cachedWidth != bounds.Width || 
            _cachedHeight != bounds.Height ||
            _cachedStart != StartColor || 
            _cachedEnd != EndColor ||
            _cachedType != Type)
        {
            GenerateGradient(spriteBatch.GraphicsDevice, bounds.Width, bounds.Height);
        }

        spriteBatch.Draw(_cachedGradient, bounds, Color.White);
    }

    private void GenerateGradient(GraphicsDevice device, int width, int height)
    {
        _cachedGradient?.Dispose();
            
        _cachedWidth = width;
        _cachedHeight = height;
        _cachedStart = StartColor;
        _cachedEnd = EndColor;
        _cachedType = Type;

        Color[] data = new Color[width * height];
            
        if (Type == GradientType.Vertical)
        {
            for (int y = 0; y < height; y++)
            {
                float t = y / (float)(height - 1);
                Color color = Color.Lerp(StartColor, EndColor, t);
                    
                for (int x = 0; x < width; x++)
                    data[y * width + x] = color;
            }
        }
        else if (Type == GradientType.Horizontal)
        {
            for (int x = 0; x < width; x++)
            {
                float t = x / (float)(width - 1);
                Color color = Color.Lerp(StartColor, EndColor, t);
                    
                for (int y = 0; y < height; y++)
                    data[y * width + x] = color;
            }
        }
        else // Radial
        {
            Vector2 center = new Vector2(width / 2f, height / 2f);
            float maxDist = Math.Min(width, height) / 2f;
                
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), center);
                    float t = MathHelper.Clamp(dist / maxDist, 0, 1);
                    data[y * width + x] = Color.Lerp(StartColor, EndColor, t);
                }
            }
        }

        _cachedGradient = new Texture2D(device, width, height);
        _cachedGradient.SetData(data);
    }

    public void Dispose()
    {
        _cachedGradient?.Dispose();
    }
}
