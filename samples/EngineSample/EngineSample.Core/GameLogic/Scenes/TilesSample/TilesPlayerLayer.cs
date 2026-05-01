using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Cameras;
using Bau.BauEngine.Scenes.Layers.Games;
using EngineSample.Core.GameLogic.Actors;

namespace EngineSample.Core.GameLogic.Scenes.TilesSample;

/// <summary>
///		Layer de la partida
/// </summary>
public class TilesPlayerLayer(AbstractScene scene, string name, int sortOrder) : AbstractGameLayer(scene, name, sortOrder)
{
	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartGameLayer()
	{
		CreatePlayer();
		CreateEnemies();
	}

	/// <summary>
	///		Crea los datos del jugador
	/// </summary>
	private void CreatePlayer()
	{	
		PlayerActor player = new(this, TilesScene.PhysicsPlayerLayer);

			// Posiciona al jugador
			player.Transform.Bounds.MoveTo(100, 75);
			// Añade el jugador a la capa
			Actors.Add(player);
	}

	/// <summary>
	///		Crea los datos de los enemigos
	/// </summary>
	private void CreateEnemies()
	{	
		EnemyActor enemy = new(this, TilesScene.PhysicsNpcLayer);

			// Posiciona el enemigo
			enemy.Transform.Bounds.MoveTo(0f, 200f);
			// Añade el jugador
			Actors.Add(enemy);
	}

	/// <summary>
	///		Actualiza la capa (los actores se actualizan por separado)
	/// </summary>
	protected override void UpdateGameLayer(GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja la capa (los actores se dibujan por separado)
	/// </summary>
	protected override void DrawGameLayer(Bau.BauEngine.Scenes.Rendering.AbstractRenderingManager renderingManager, GameContext gameContext)
	{
		// ... no hace nada, los actores ya se han modificado y esta capa no necesita nada más
	}

	/// <summary>
	///		Finaliza la capa
	/// </summary>
	protected override void EndGameLayer()
	{
	}
}
