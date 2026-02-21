using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering;

public class TextureRenderCommand : RenderCommand
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public Rectangle? SourceRectangle { get; set; }
    public Rectangle? DestinationRectangle { get; set; }
    public Color Color { get; set; } = Color.White;
    public float Rotation { get; set; }
    public Vector2 Origin { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
    {
        if (Texture == null) return;
            
        if (DestinationRectangle.HasValue)
        {
            Rectangle dest = DestinationRectangle.Value;
                
            if (SourceRectangle.HasValue)
            {
                spriteBatch.Draw(
                    Texture,
                    dest,
                    SourceRectangle.Value,
                    Color,
                    Rotation,
                    Origin,
                    Effects,
                    0f
                );
            }
            else
            {
                spriteBatch.Draw(
                    Texture,
                    dest,
                    null,
                    Color,
                    Rotation,
                    Origin,
                    Effects,
                    0f
                );
            }
        }
        else
        {
            spriteBatch.Draw(
                Texture,
                Position,
                SourceRectangle,
                Color,
                Rotation,
                Origin,
                Scale,
                Effects,
                0f
            );
        }
    }
}
