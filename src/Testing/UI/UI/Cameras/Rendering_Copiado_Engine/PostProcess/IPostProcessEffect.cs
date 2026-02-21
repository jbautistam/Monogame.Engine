using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering.PostProcess;

public interface IPostProcessEffect
{
    bool IsEnabled { get; set; }
    void Apply(SpriteBatch spriteBatch, RenderTarget2D source, RenderTarget2D destination);
}