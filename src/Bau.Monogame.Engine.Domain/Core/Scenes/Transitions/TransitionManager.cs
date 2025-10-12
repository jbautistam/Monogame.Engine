using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.Scenes.Transitions;

public class TransitionManager
{
    private ISceneTransition _currentTransition;
    private Scene _nextScene;
    private Action _onCompleteCallback;

    public bool IsTransitioning => _currentTransition != null && !_currentTransition.IsComplete;

    public void StartTransition(ISceneTransition transition, Scene currentScene, Scene nextScene, Action onComplete = null)
    {
        _nextScene = nextScene;
        _onCompleteCallback = onComplete;
        _currentTransition = transition;
        _currentTransition.Begin(currentScene, nextScene, Game1.Instance.GraphicsDevice);
    }

    public void Update(GameTime gameTime)
    {
        if (_currentTransition != null && !IsTransitioning)
        {
            // Transición terminada → cambiar escena
            Game1.Instance.SceneManager.ChangeSceneWithoutTransition(_nextScene);
            _onCompleteCallback?.Invoke();
            _currentTransition.Dispose();
            _currentTransition = null;
        }

        _currentTransition?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _currentTransition?.Draw(spriteBatch);
    }
}