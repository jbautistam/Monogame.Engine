using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.Scenes.Transitions;

public abstract class SceneTransitionBase : ISceneTransition
{
    public bool IsComplete { get; protected set; }
    public float Duration { get; set; } = 1f;
    protected float _elapsedTime;
    protected Scene _currentScene;
    protected Scene _nextScene;
    protected GraphicsDevice _graphicsDevice;
    protected RenderTarget2D _currentSceneTarget;
    protected RenderTarget2D _nextSceneTarget;

    public virtual void Begin(Scene currentScene, Scene nextScene, GraphicsDevice graphicsDevice)
    {
        IsComplete = false;
        _elapsedTime = 0f;
        _currentScene = currentScene;
        _nextScene = nextScene;
        _graphicsDevice = graphicsDevice;

        var pp = _graphicsDevice.PresentationParameters;
        _currentSceneTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
        _nextSceneTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

        RenderSceneToTarget(_currentScene, _currentSceneTarget);
        RenderSceneToTarget(_nextScene, _nextSceneTarget);
    }

    protected void RenderSceneToTarget(Scene scene, RenderTarget2D target)
    {
        _graphicsDevice.SetRenderTarget(target);
        _graphicsDevice.Clear(Color.Transparent);
        var spriteBatch = new SpriteBatch(_graphicsDevice);
        spriteBatch.Begin();
        scene.Draw(spriteBatch, new GameTime());
        spriteBatch.End();
    }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);

    public virtual void Dispose()
    {
        _currentSceneTarget?.Dispose();
        _nextSceneTarget?.Dispose();
    }
}

