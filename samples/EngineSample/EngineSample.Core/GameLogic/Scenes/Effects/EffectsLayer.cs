using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers.Games;
using Bau.BauEngine.Scenes.Rendering.Postprocessing;
using EngineSample.Core.GameLogic.Actors;
using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.Effects;

/// <summary>
///		Layer de la partida
/// </summary>
public class EffectsLayer(AbstractScene scene, string name, int sortOrder) : AbstractGameLayer(scene, name, sortOrder)
{
	// Constantes públicas
	public const int PhysicsPlayerLayer = 1;
	public const int PhysicsPlayerProjectileLayer = 2;
	public const int PhysicsNpcLayer = 3;
	public const int PhysicsNpcProjectileLayer = 4;

	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartGameLayer()
	{
		CreatePlayer();
		CreateEnemies();
		CreateFsmEnemies();
	}

	/// <summary>
	///		Crea los datos del jugador
	/// </summary>
	private void CreatePlayer()
	{	
		PlayerActor player = new(this, PhysicsPlayerLayer);

			// Posiciona al jugador
			player.Transform.Bounds.MoveTo(32, 32);
			// Añade el jugador a la capa
			Actors.Add(player);
	}

	/// <summary>
	///		Crea los datos de los enemigos
	/// </summary>
	private void CreateEnemies()
	{	
		EnemyActor enemy = new(this, PhysicsNpcLayer);

			// Posiciona el enemigo
			enemy.Transform.Bounds.MoveTo(0f, 200f);
			// Añade el jugador
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
			EnemyFsmActor enemy = new(this, name, PhysicsNpcLayer);

				// Posiciona el enemigo
				enemy.Transform.Bounds.MoveTo(0f, y);
				// Añade el jugador
				Actors.Add(enemy);
		}
	}

	/// <summary>
	///		Actualiza la capa (los actores se actualizan por separado)
	/// </summary>
	protected override void UpdateGameLayer(GameContext gameContext)
	{
	}

	/// <summary>
	///		Crea un efecto de tipo Wipe
	/// </summary>
    public void CreateEffectWipe()
	{
		TintEffect effect = new(Scene, Color.Red, 3);

			// Añade el efecto a la lista
			Scene.RenderingManager.PostprocessingEffects.Add(effect);
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
