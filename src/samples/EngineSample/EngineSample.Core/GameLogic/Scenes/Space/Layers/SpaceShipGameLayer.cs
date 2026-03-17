using Bau.Libraries.BauGame.Engine.Actors.Spawners;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Games;
using Bau.Libraries.BauGame.Engine.Tools.Extensors;
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
	}

	/// <summary>
	///		Crea los datos del jugador
	/// </summary>
	private void CreatePlayer()
	{	
		SpacePlayerActor player = new(this, SpaceShipsScene.PhysicsPlayerLayer);

			// Posiciona al jugador
			player.Transform.Bounds.MoveTo(0.5f * Scene.WorldDefinition.WorldBounds.Width, 0.5f * Scene.WorldDefinition.WorldBounds.Height);
			Scene.Camera.Position = player.Transform.Bounds.TopLeft;
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
					.WithCircleSpawner(0, 0, MathF.Max(Scene.WorldDefinition.WorldBounds.Width, Scene.WorldDefinition.WorldBounds.Height), true, 1)
						.WithWave("enemy", CreateEnemy)
					.WithCircleSpawner(0, 0, MathF.Max(Scene.WorldDefinition.WorldBounds.Width, Scene.WorldDefinition.WorldBounds.Height), true, 1)
						.WithWave("meteors", CreateEnemy);
			// Añade el spawner a la capa
			Actors.Add(builder.Build());
	}

	/// <summary>
	///		Crea un enemigo
	/// </summary>
	private void CreateEnemy(SpawnerWaveModel.FactoryParameters parameters)
	{
		switch (parameters.Name)
		{
			case "enemy":
					SpawnEnemySpaceShip(parameters);
				break;
			case "meteors":
					SpawnMeteor(parameters);
				break;
		}
	}

	/// <summary>
	///		Crea una nave enemiga
	/// </summary>
	private void SpawnEnemySpaceShip(SpawnerWaveModel.FactoryParameters parameters)
	{
		int random = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(1, 11);
		SpaceShipEnemyActor meteor = new(this, "spaceship");

			// Asigna la posición y la dirección
			meteor.Transform.Bounds.MoveTo(parameters.Position);
			meteor.Direction = parameters.Position.DirectionTo(Scene.WorldDefinition.WorldBounds.Center.X, Scene.WorldDefinition.WorldBounds.Center.Y);
			meteor.RotationSpeed = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(0.3f, 0.7f);
			// Añade el enemigo al buffer de la pantalla
			Actors.Add(meteor);
	}

	/// <summary>
	///		Crea un meteoro
	/// </summary>
	private void SpawnMeteor(SpawnerWaveModel.FactoryParameters parameters)
	{
		int random = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(1, 11);
		MeteorActor meteor = new(this, "meteor", GetMeteorSize(), "meteors", $"meteor_{random:00}", SpaceShipsScene.PhysicsNpcLayer);

			// Asigna la posición y la dirección
			meteor.Transform.Bounds.MoveTo(parameters.Position);
			meteor.Direction = parameters.Position.DirectionTo(Scene.WorldDefinition.WorldBounds.Center.X, Scene.WorldDefinition.WorldBounds.Center.Y);
			meteor.RotationSpeed = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(0.3f, 0.7f);
			// Añade el meteoro al buffer de la pantalla
			Actors.Add(meteor);

		// Obtiene el tamaño del meteoro
		MeteorActor.MeteorSize GetMeteorSize()
		{
			return Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(1, 10) switch
						{
							> 7 => MeteorActor.MeteorSize.Big,
							> 3 => MeteorActor.MeteorSize.Medium,
							_ => MeteorActor.MeteorSize.Small
						};
		}
	}

	/// <summary>
	///		Actualiza la capa (los actores se actualizan por separado)
	/// </summary>
	protected override void UpdateGameLayer(GameContext gameContext)
	{
		TreatInputs(gameContext);
	}

	/// <summary>
	///		Trata las entradas
	/// </summary>
	private void TreatInputs(GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja la capa (los actores se dibujan por separado)
	/// </summary>
	protected override void DrawGameLayer(Bau.Libraries.BauGame.Engine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
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
