using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Managers;

/// <summary>
///     Manager del motor 
/// </summary>
public class EngineManager
{
    public EngineManager(Game game, Configuration.EngineSettings engineSettings)
    {
        EngineSettings = engineSettings;
        FilesManager = new Files.FilesManager(this);
        InputManager = new Input.InputManager();
        LocalizationManager = new Localization.LocalizationManager();
        MonogameServicesManager = new Services.MonogameServicesManager(game);
        ResourcesManager = new Resources.ResourcesManager(this);
        SceneManager = new Scenes.SceneManager();
    }

    /// <summary>
    ///     Inicializa el motor de juego
    /// </summary>
    public void Initialize()
    {
        MonogameServicesManager.Initialize(EngineSettings);
    }

	/// <summary>
	///		Actualiza los datos del motor
	/// </summary>
	public void Update(GameTime gameTime)
	{
		InputManager.Update(gameTime);
        SceneManager.Update(gameTime);
	}

    /// <summary>
    ///     Termina el juego
    /// </summary>
	public void Exit()
	{
        MonogameServicesManager.Game.Exit();
	}

	/// <summary>
	///		Configuración global
	/// </summary>
	public Configuration.EngineSettings EngineSettings { get; } 

    /// <summary>
    ///     Manager de archivos
    /// </summary>
    public Files.FilesManager FilesManager { get; }

    /// <summary>
    ///     Manager del teclado
    /// </summary>
    public Input.InputManager InputManager { get; }

    /// <summary>
    ///     Manager de cultura
    /// </summary>
    public Localization.LocalizationManager LocalizationManager { get; }

    /// <summary>
    ///     Manager de los servicios de monogame
    /// </summary>
    public Services.MonogameServicesManager MonogameServicesManager { get; }

    /// <summary>
    ///     Manager de recursos
    /// </summary>
    public Resources.ResourcesManager ResourcesManager { get; }

    /// <summary>
    ///     Manager de escenas
    /// </summary>
    public Scenes.SceneManager SceneManager { get; }
}