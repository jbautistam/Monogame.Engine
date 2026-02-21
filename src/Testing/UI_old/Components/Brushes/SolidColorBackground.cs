using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI.Brushes;

/// <summary>
/// Fondo de color sólido
/// </summary>
public class SolidColorBackground : IBackground, IEquatable<SolidColorBackground>
{
    public Color Color { get; set; }
        
    public bool HasContent => Color.A > 0;

    public SolidColorBackground(Color color)
    {
        Color = color;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle bounds, Rectangle contentBounds)
    {
        if (!HasContent) return;
            
        var pixel = TextureHelper.GetPixelTexture(spriteBatch.GraphicsDevice);
        spriteBatch.Draw(pixel, bounds, Color);
    }

    public bool Equals(SolidColorBackground other)
    {
        if (other is null) return false;
        return Color == other.Color;
    }

    public override bool Equals(object obj) => Equals(obj as SolidColorBackground);
    public override int GetHashCode() => Color.GetHashCode();
        
    public static bool operator ==(SolidColorBackground left, SolidColorBackground right) 
        => Equals(left, right);
    public static bool operator !=(SolidColorBackground left, SolidColorBackground right) 
        => !Equals(left, right);
}
