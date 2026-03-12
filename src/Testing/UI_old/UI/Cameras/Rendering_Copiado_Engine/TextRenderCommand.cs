using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering;

public class TextRenderCommand : RenderCommand
{
    public string Text { get; set; }
    public SpriteFont Font { get; set; }
    public Vector2 Position { get; set; }
    public Color Color { get; set; } = Color.White;
    public float Rotation { get; set; }
    public Vector2 Origin { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
    {
        SpriteFont fontToUse = Font ?? defaultFont;
            
        if (fontToUse == null) return;
        if (string.IsNullOrEmpty(Text)) return;
            
        spriteBatch.DrawString(
            fontToUse,
            Text,
            Position,
            Color,
            Rotation,
            Origin,
            Scale,
            Effects,
            0f
        );
    }
}
