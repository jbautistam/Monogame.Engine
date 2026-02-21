using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.PostProcess;

public interface IPostProcessEffect
{
    bool IsEnabled { get; set; }
    void Apply(SpriteBatch spriteBatch, RenderTarget2D source, RenderTarget2D destination);
}