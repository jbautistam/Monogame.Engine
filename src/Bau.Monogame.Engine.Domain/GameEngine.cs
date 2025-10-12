using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain;

/// <summary>
///     Clase principal del motor
/// </summary>
public static class GameEngine
{
	/// <summary>
	///		Crea la clase principal del motor
	/// </summary>
	public static void Instantiate(Game game, Core.Configuration.EngineSettings settings)
	{
		Instance = new Core.Managers.EngineManager(game, settings);
	}

	/// <summary>
	///		Manager principal
	/// </summary>
	public static Core.Managers.EngineManager Instance { get; private set; } = default!;
}