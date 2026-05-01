using Bau.BauEngine.Scenes.Rendering.Transition;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.New;

public class TransitionManager
{
    private GraphicsDevice _gd;
    private SpriteBatch _sb;
    private ITransitionEffect _activeEffect;
    private float _progress;
    private float _duration;
    private bool _isTransitioning;
    private Texture2D _currentSceneTexture;
    private Texture2D _newSceneTexture;

    public bool IsTransitioning => _isTransitioning;

    public TransitionManager(GraphicsDevice gd, SpriteBatch sb)
    {
        _gd = gd;
        _sb = sb;
    }

    public void StartTransition(Texture2D currentScene, Texture2D newScene,
                                ITransitionEffect effect, float duration)
    {
        _currentSceneTexture = currentScene;
        _newSceneTexture = newScene;
        _activeEffect = effect;
        _duration = duration;
        _progress = 0f;
        _isTransitioning = true;
        _activeEffect.Progress = 0f;
    }

    public void Update(GameTime gameTime)
    {
        if (!_isTransitioning) return;
        _progress += (float)gameTime.ElapsedGameTime.TotalSeconds / _duration;
        if (_progress >= 1f)
        {
            _progress = 1f;
            _isTransitioning = false;
        }
        if (_activeEffect != null)
            _activeEffect.Progress = _progress;
    }

    public void Draw()
    {
        if (!_isTransitioning || _activeEffect == null) return;
        _gd.SetRenderTarget(null);
        _sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        _activeEffect.Apply(_currentSceneTexture, _newSceneTexture, _sb, _gd);
        _sb.End();
    }

    public void UpdateSize(int width, int height) { /* opcional */ }
    public void Dispose() { }
}