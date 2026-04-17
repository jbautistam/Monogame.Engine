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
    public void ChangeScene(string name, Managers.GameContext gameContext)
    {
        AbstractScene? newScene = EngineManager.EngineGame.GetScene(name); // _scenes.Get(name);

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
        string? nextScene = _currentScene?.Update(gameContext);

            // Cambia la escena
            if (!string.IsNullOrWhiteSpace(nextScene) && !nextScene.Equals(_currentScene?.Name, StringComparison.CurrentCultureIgnoreCase))
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
