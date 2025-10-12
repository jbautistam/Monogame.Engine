using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine;

/// <summary>
///     Clase principal del motor
/// </summary>
public static class GameEngine
{
	/// <summary>
	///		Crea la clase principal del motor
	/// </summary>
	public static void Instantiate(Game game, Configuration.EngineSettings settings)
	{
		Instance = new Managers.EngineManager(game, settings);
	}

	/// <summary>
	///		Manager principal
	/// </summary>
	public static Managers.EngineManager Instance { get; private set; } = default!;
}