using Bau.BauEngine.Scenes.Rendering.Postprocessing;
using Bau.BauEngine.Scenes.Rendering.Transition;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.New;

public class GraphicsCoordinator
{
    private GraphicsDevice _gd;
    private SpriteBatch _sb;
    private RenderManager _renderManager;
    private TransitionManager _transitionManager;
    private bool _isTransitioning;
    private Action _onTransitionCompleted;

    public GraphicsCoordinator(GraphicsDevice gd, int width, int height)
    {
        _gd = gd;
        _sb = new SpriteBatch(gd);
        _renderManager = new RenderManager(gd, width, height);
        _transitionManager = new TransitionManager(gd, _sb);
    }

    // Dibujo del mundo
    public void BeginDrawWorld() => _renderManager.BeginDrawWorld();
    public void EndDrawWorld() => _renderManager.EndDrawWorld();
    public SpriteBatch SpriteBatch => _renderManager.SpriteBatch;

    // Configurar efectos de post-proceso
    public void SetPostProcessEffects(params IPostProcessingEffect [] effects)
        => _renderManager.SetPostProcessingEffects(effects);

    // Iniciar transición con la textura de la nueva escena (debes generarla externamente)
    public void StartTransition(Texture2D newSceneTexture, ITransitionEffect effect,
                                float duration, Action onCompleted = null)
    {
        if (_isTransitioning) return;

        _renderManager.RenderScene();
        Texture2D currentTexture = _renderManager.GetFinalTexture();

        _transitionManager.StartTransition(currentTexture, newSceneTexture, effect, duration);
        _isTransitioning = true;
        _onTransitionCompleted = onCompleted;
    }

    // Actualizar (llamar en Update del juego)
    public void Update(GameTime gameTime)
    {
        if (_isTransitioning)
        {
            _transitionManager.Update(gameTime);
            if (!_transitionManager.IsTransitioning)
            {
                _isTransitioning = false;
                _onTransitionCompleted?.Invoke();
                _onTransitionCompleted = null;
            }
        }
    }

    // Dibujar el resultado final (llamar en Draw del juego)
    public void Draw()
    {
        if (!_isTransitioning)
        {
            _renderManager.RenderScene();
            Texture2D final = _renderManager.GetFinalTexture();
            _sb.Begin();
            _sb.Draw(final, new Rectangle(0, 0, final.Width, final.Height), Color.White);
            _sb.End();
        }
        else
        {
            _transitionManager.Draw();
        }
    }

    // Dibujar la UI siempre encima
    public void DrawUI(Action<SpriteBatch> uiDrawAction) => _renderManager.DrawUI(uiDrawAction);

    public void UpdateSize(int width, int height)
    {
        _renderManager.UpdateSize(width, height);
        _transitionManager.UpdateSize(width, height);
    }

    public void Dispose()
    {
        _renderManager.Dispose();
        _transitionManager.Dispose();
        _sb.Dispose();
    }
}