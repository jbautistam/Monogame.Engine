using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Games;

/// <summary>
///		Clase abstracta de definición de partidas
/// </summary>
public abstract class AbstractGameLayer : AbstractLayer
{
	public AbstractGameLayer(AbstractScene scene, string name, int sortOrder) : base(scene, name, LayerType.Game, sortOrder)
	{
		ProjectileManager = new GameManagers.ProjectileManager(this);
		ExplosionsManager = new GameManagers.ExplosionsManager(this);
	}

	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartLayer()
	{
		// Inicia la capa de la partida
		StartGameLayer();
	}

	/// <summary>
	///		Inicia la capa de partida
	/// </summary>
	protected abstract void StartGameLayer();

	/// <summary>
	///		Actualiza la capa (los actores se actualizan por separado)
	/// </summary>
	protected override void UpdateLayer(Managers.GameContext gameContext)
	{
		// Actualiza los datos de los managers
		ProjectileManager.Update(gameContext);
		ExplosionsManager.Update(gameContext);
		// Actualiza la capa de la partida
		UpdateGameLayer(gameContext);
	}

	/// <summary>
	///		Actualiza los datos particulares de la capa de juego
	/// </summary>
	protected abstract void UpdateGameLayer(Managers.GameContext gameContext);

	/// <summary>
	///		Dibuja la capa (los actores se dibujan por separado)
	/// </summary>
	protected override void DrawLayer(Camera2D camera, Managers.GameContext gameContext)
	{
		// Dibuja los datos de los managers
		ProjectileManager.Draw(camera, gameContext);
		ExplosionsManager.Draw(camera, gameContext);
		// Dibuja la capa de partida
		DrawGameLayer(camera, gameContext);
	}

	/// <summary>
	///		Dibuja los datos de la partida
	/// </summary>
	protected abstract void DrawGameLayer(Camera2D camera, Managers.GameContext gameContext);

	/// <summary>
	///		Finaliza la capa
	/// </summary>
	protected override void EndLayer()
	{
		// Limipia los datos
		ProjectileManager.Clear();
		ExplosionsManager.Clear();
		// Finaliza la capa
		EndGameLayer();
	}

	/// <summary>
	///		Finaliza la capa de la partida
	/// </summary>
	protected abstract void EndGameLayer();

	/// <summary>
	///		Manager para proyectiles
	/// </summary>
	public GameManagers.ProjectileManager ProjectileManager { get; }

	/// <summary>
	///		Manager para explosioens
	/// </summary>
	public GameManagers.ExplosionsManager ExplosionsManager { get; }
}
