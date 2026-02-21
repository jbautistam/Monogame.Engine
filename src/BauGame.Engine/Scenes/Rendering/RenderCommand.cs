using Bau.Libraries.BauGame.Engine.Entities.Common;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering;

public abstract class RenderCommand
{
    public RectangleF Bounds { get; set; }
    public Effect Effect { get; set; }
        
    public abstract void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont);
}