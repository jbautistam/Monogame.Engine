using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Scenes;

/// <summary>
///     Manager de escenas
/// </summary>
public class SceneManager
{
    // Variables privadas
    private AbstractScene? _currentScene;
    private Base.DictionaryModel<AbstractScene> _scenes = new();

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
    public void ChangeScene(string name)
    {
        AbstractScene? newScene = _scenes.Get(name);

            // Cambia la escena
            if (newScene is not null)
                ChangeScene(newScene);
    }

    /// <summary>
    ///     Cambia la escena
    /// </summary>
    public void ChangeScene(AbstractScene? newScene)
    {
        // Finaliza la escena
        _currentScene?.End();
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
    public void Update(GameTime gameTime)
    {
        AbstractScene? nextScene = _currentScene?.Update(gameTime);

            // Cambia la escena
            if (nextScene != _currentScene)
                ChangeScene(nextScene);
        //TransitionManager.Update(gameTime);
        //if (!TransitionManager.IsTransitioning)
        //    _currentScene?.Update(gameTime);
    }

    /// <summary>
    ///     Dibuja la escena
    /// </summary>
    public void Draw(GameTime gameTime)
    {
        if (_currentScene is not null)
            _currentScene.Draw(gameTime);
        //if (TransitionManager.IsTransitioning)
        //{
        //    spriteBatch.Begin();
        //    TransitionManager.Draw(spriteBatch);
        //    spriteBatch.End();
        //}
        //else
        //{
        //    _currentScene?.Draw(spriteBatch, gameTime);
        //}
    }
}
