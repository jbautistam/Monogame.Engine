namespace Bau.BauEngine.Scenes;

/// <summary>
///     Manager de escenas
/// </summary>
public class SceneManager(Managers.EngineManager engineManager)
{
    // Variables privadas
    private AbstractScene? _currentScene;

    /// <summary>
    ///     Cambia la escena
    /// </summary>
    public void ChangeScene(NextSceneContextModel nextScene, Managers.GameContext gameContext)
    {
        AbstractScene? newScene = EngineManager.EngineGame.GetScene(nextScene);

            // Cambia la escena
            if (newScene is not null)
            {
                // Finaliza la escena
                _currentScene?.End(gameContext);
                // Cambia la escena actual
                _currentScene = newScene;
                // Inicia la escena
                _currentScene?.Start();
            }
    }

    /// <summary>
    ///     Actualiza la escena
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        NextSceneContextModel? nextScene = _currentScene?.Update(gameContext);

            // Cambia la escena
            if (nextScene is not null)
                ChangeScene(nextScene, gameContext);
    }

    /// <summary>
    ///     Dibuja la escena
    /// </summary>
    public void Draw(Managers.GameContext gameContext)
    {
        if (_currentScene is not null)
            _currentScene.Draw(gameContext);
    }

    /// <summary>
    ///     Manager del motor principal
    /// </summary>
    public Managers.EngineManager EngineManager { get; } = engineManager;
}
