using GameEngine.Math;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering;

public abstract class RenderCommand
{
    public RectangleF Bounds { get; set; }
    public Effect Effect { get; set; }
        
    public abstract void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont);
}