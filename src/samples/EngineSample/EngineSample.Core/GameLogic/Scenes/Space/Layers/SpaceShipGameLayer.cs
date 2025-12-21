using Bau.Libraries.BauGame.Engine.Actors.Spawners;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Games;
using Bau.Libraries.BauGame.Engine.Tools.Extensors;
using EngineSample.Core.GameLogic.Actors;
using EngineSample.Core.GameLogic.Actors.SpaceShips;

namespace EngineSample.Core.GameLogic.Scenes.Space.Layers;

/// <summary>
///		Layer de la partida
/// </summary>
public class SpaceShipGameLayer(AbstractScene scene, string name, int sortOrder) : AbstractGameLayer(scene, name, sortOrder)
{
	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartGameLayer()
	{
		CreatePlayer();
		CreateSpawner();
		CreateEnemies();
		CreateFsmEnemies();
	}

	/// <summary>
	///		Crea los datos del jugador
	/// </summary>
	private void CreatePlayer()
	{	
		SpacePlayerActor player = new(this, SpaceShipsScene.PhysicsPlayerLayer);

			// Posiciona al jugador
			player.Transform.Bounds.MoveTo(32, 32);
			// Añade el jugador a la capa
			Actors.Add(player);
	}

	/// <summary>
	///		Crea el actor que genera los enemigos y lo posiciona
	/// </summary>
	private void CreateSpawner()
	{
		SpawnerActorBuilder builder = new(this);

			// Crea las diferentes olas
			builder
					//.WithSpawner(-20, -20, 50, 5)
					//	.WithWave("enemy", CreateEnemySpaceShip)
					.WithSpawner(-100, -100, 200, 5)
						.WithWave("meteors", CreateEnemySpaceShip);
			// Añade el spawner a la capa
			Actors.Add(builder.Build());
	}

	/// <summary>
	///		Crea un enemigo
	/// </summary>
	private void CreateEnemySpaceShip(SpawnerWaveModel.FactoryParameters parameters)
	{
		switch (parameters.Name)
		{
			case "enemy":
					SpaceEnemyFsmActor enemy = new(this, "monsterA", SpaceShipsScene.PhysicsNpcLayer);

						// Asigna la posición
						enemy.Transform.Bounds.MoveTo(parameters.Position);
						// Añade el enemigo al buffer de la pantalla
						Actors.AddNext(enemy);
				break;
			case "meteors":
					int random = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(1, 11);
					MeteorActor meteor = new(this, "meteor", "meteors", $"meteor_{random:00}", SpaceShipsScene.PhysicsNpcLayer);

						// Asigna la posición y la dirección
						meteor.Transform.Bounds.MoveTo(parameters.Position);
						meteor.Direction = parameters.Position.DirectionTo(Scene.WorldDefinition.WorldBounds.Center.X, Scene.WorldDefinition.WorldBounds.Center.Y);
						meteor.RotationSpeed = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(0.1f, 0.5f);
						// Añade el meteoro al buffer de la pantalla
						Actors.AddNext(meteor);
				break;
		}
	}

	/// <summary>
	///		Crea los datos de los enemigos
	/// </summary>
	private void CreateEnemies()
	{	
		EnemyActor enemy = new(this, SpaceShipsScene.PhysicsNpcLayer);

			// Posiciona el enemigo
			enemy.Transform.Bounds.MoveTo(0f, 200f);
			// Añade el enemigo
			Actors.Add(enemy);
	}

	/// <summary>
	///		Crea los datos de los enemigos
	/// </summary>
	private void CreateFsmEnemies()
	{	
		CreateEnemy("monsterB", 300);
		CreateEnemy("monsterC", 400);
		CreateEnemy("monsterD", 500);

		// Crea un enemigo
		void CreateEnemy(string name, float y)
		{
			EnemyFsmActor enemy = new(this, name, SpaceShipsScene.PhysicsNpcLayer);

				// Posiciona el enemigo
				enemy.Transform.Bounds.MoveTo(0f, y);
				// Añade el jugador
				Actors.Add(enemy);
		}
	}

	/// <summary>
	///		Actualiza la capa (los actores se actualizan por separado)
	/// </summary>
	protected override void UpdateGameLayer(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		TreatInputs(gameContext);
	}

	/// <summary>
	///		Trata las entradas
	/// </summary>
	private void TreatInputs(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja la capa (los actores se dibujan por separado)
	/// </summary>
	protected override void DrawGameLayer(Camera2D camera, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
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
