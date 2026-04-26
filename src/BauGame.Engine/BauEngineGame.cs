using Bau.BauEngine.Scenes;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine;

/// <summary>
///		Clase base para los juegos del motor
/// </summary>
public abstract class BauEngineGame : Game
{
	public BauEngineGame(string contentRoot)
	{
		EngineManager = new Managers.EngineManager(this, contentRoot);
	}

	/// <summary>
	///		Inicializa el juego incluyendo la configuración de la localización y añadiendo las pantallas iniciales al ScreenManager.
	/// </summary>
	protected override void Initialize()
	{
		// Configura el juego
		InitializeGame();
		// Inicializa el juego
		base.Initialize();
	}

	/// <summary>
	///		Inicializa el juego
	/// </summary>
	protected abstract void InitializeGame();

	/// <summary>
	///		Carga el contenido del juego
	/// </summary>
	protected override void LoadContent()
	{
		// Carga el contenido del juego
		LoadContentGame();
		// Llama al método base
		base.LoadContent();
	}

	/// <summary>
	///		Carga el contenido del juego
	/// </summary>
	protected abstract void LoadContentGame();

	/// <summary>
	///		Actualiza la lógica del juego. Se le llama una vez por frame
	/// </summary>
	protected override void Update(GameTime gameTime)
	{
		// Actualiza los controladores del motor
		EngineManager.Update(gameTime);
		// Llama al método base de modificación
		base.Update(gameTime);
	}

	/// <summary>
	///		Dibuja los gráficos del juego. Se le llama una vez por frame
	/// </summary>
	protected override void Draw(GameTime gameTime)
	{
		// Dibuja la escena actual
		EngineManager.Draw(gameTime);
		// Llama al método base de dibujo
		base.Draw(gameTime);
	}

	/// <summary>
	///		Obtiene la escena que se corresponde a un nombre
	/// </summary>
	public abstract AbstractScene GetScene(string name);

	/// <summary>
	///		Graba la configuración de la partida
	/// </summary>
	public abstract void SaveConfiguration();

	/// <summary>
	///		Manager del motor
	/// </summary>
	public Managers.EngineManager EngineManager { get; }
}