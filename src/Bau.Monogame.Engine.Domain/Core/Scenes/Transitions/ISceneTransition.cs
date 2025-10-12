using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.Scenes.Transitions;

public interface ISceneTransition
{
    bool IsComplete { get; }
    float Duration { get; set; }
    void Begin(Scene currentScene, Scene nextScene, GraphicsDevice graphicsDevice);
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}