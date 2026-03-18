using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Managers;

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
        AudioManager = new Audio.AudioManager(this);
        SceneManager = new Scenes.SceneManager();
        MathFunctions = new Tools.MathTools.Precomputed.PrecomputedFunctions();
        DebugManager = new Debugger.DebugManager(this);
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
        // Actualiza el contexto de la partida
        GameContext.Update(gameTime);
        // Actualiza los managers adicionales
		InputManager.Update(GameContext);
        SceneManager.Update(GameContext);
        AudioManager.Update(GameContext);
        // Actualiza la información de depuración
        DebugManager.Update(GameContext);
	}

	/// <summary>
	///		Dibuja los datos del motor
	/// </summary>
	public void Draw(GameTime gameTime)
	{
        // Actualiza el contexto de la partida
        GameContext.Update(gameTime);
        // Actualiza los datos
        SceneManager.Draw(GameContext);
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
    ///     Funciones matemáticas
    /// </summary>
    public Tools.MathTools.Precomputed.PrecomputedFunctions MathFunctions { get; }

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
    ///     Manager de audio
    /// </summary>
    public Audio.AudioManager AudioManager { get; }

    /// <summary>
    ///     Manager de escenas
    /// </summary>
    public Scenes.SceneManager SceneManager { get; }

    /// <summary>
    ///     Manager para depuración
    /// </summary>
    public Debugger.DebugManager DebugManager { get; }

    /// <summary>
    ///     Contexto de la partida
    /// </summary>
    public GameContext GameContext { get; } = new();
}