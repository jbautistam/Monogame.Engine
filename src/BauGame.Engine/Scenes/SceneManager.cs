namespace Bau.Libraries.BauGame.Engine.Scenes;

/// <summary>
///     Manager de escenas
/// </summary>
public class SceneManager
{
    // Variables privadas
    private AbstractScene? _currentScene;
    private Entities.Common.DictionaryModel<AbstractScene> _scenes = new();

    /// <summary>
    ///     Añade una escena
    /// </summary>
    public AbstractScene AddScene(AbstractScene scene)
    {
        // Añade la escena
        _scenes.Add(scene.Name, scene);
        // Si era la primera, la asigna
        if (_scenes.Items.Count == 1)
            _currentScene = scene;
        // Devuelve la escena añadida
        return scene;
    }

    /// <summary>
    ///     Cambia la escena
    /// </summary>
    public void ChangeScene(string name, Managers.GameContext gameContext)
    {
        AbstractScene? newScene = _scenes.Get(name);

            // Cambia la escena
            if (newScene is not null)
                ChangeScene(newScene, gameContext);
    }

    /// <summary>
    ///     Cambia la escena
    /// </summary>
    public void ChangeScene(AbstractScene? newScene, Managers.GameContext gameContext)
    {
        // Finaliza la escena
        _currentScene?.End(gameContext);
        // Cambia la escena actual
        _currentScene = newScene;
        // Inicia la escena
        _currentScene?.Start();
    }

    /// <summary>
    ///     Obtiene una escena por su nombre
    /// </summary>
	public AbstractScene? GetScene(string scene) => _scenes.Get(scene);

    /// <summary>
    ///     Actualiza la escena
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        AbstractScene? nextScene = _currentScene?.Update(gameContext);

            // Cambia la escena
            if (nextScene != _currentScene)
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
}
