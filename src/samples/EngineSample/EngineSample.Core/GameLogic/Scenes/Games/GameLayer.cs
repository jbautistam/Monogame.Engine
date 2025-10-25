using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Actors.Particles;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Games;
using EngineSample.Core.GameLogic.Actors;
using EngineSample.Core.GameLogic.Actors.Characters;

namespace EngineSample.Core.GameLogic.Scenes.Games;

/// <summary>
///		Layer de la partida
/// </summary>
public class GameLayer(AbstractScene scene, string name, int sortOrder) : AbstractGameLayer(scene, name, sortOrder)
{
	// Variables privadas
	private CharacterManager? _characterManager;
	private ParticlesSystemActor? _particlesManager;

	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartGameLayer()
	{
		CreatePlayer();
		CreateEnemies();
		CreateCharacters();
		CreateParticles();
	}

	/// <summary>
	///		Crea los datos del jugador
	/// </summary>
	private void CreatePlayer()
	{	
		PlayerActor player = new(this, GameScene.PhysicsPlayerLayer);

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
		EnemyActor enemy = new(this, GameScene.PhysicsNpcLayer);

			// Posiciona el enemigo
			enemy.Transform.Bounds.MoveTo(0f, 200f);
			// Añade el jugador
			Actors.Add(enemy);
	}

	/// <summary>
	///		Crea los personajes
	/// </summary>
	private void CreateCharacters()
	{
		// Crea el manager de personajes
		_characterManager = new(this, 2);
		// Crea los personajes
		CreateCharacter(_characterManager, "sylvie");
		CreateCharacter(_characterManager, "james");
		CreateCharacter(_characterManager, "narrator");
		// Añade el personaje
		Actors.Add(_characterManager);
	}

	/// <summary>
	///		Crea un personaje
	/// </summary>
	private void CreateCharacter(CharacterManager manager, string name)
	{
		CharacterActor character = manager.Add(name);

			// Crea las definiciones
			character.AddDefinition(CharacterActorDefinition.DefaultType, $"{name}-default", name);
			character.AddDefinition("avatar", $"{name}-avatar", name);
			character.AddDefinition("sad", $"{name}-sad", name);
			character.AddDefinition("smile", $"{name}-smile", name);
	}

	/// <summary>
	///		Crea el sistema de partículas
	/// </summary>
	private void CreateParticles()
	{
		// Genera el actor del sistema de partículas
		_particlesManager = new(this, 3)
								{
									Position = new Vector2(0, 0),
									Texture = "particle",
									Region = "default"
								};
		// Añade el sistema de partículas a la lista de actores
		Actors.Add(_particlesManager);
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
		// Crea los personajes y sus acciones
		if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.S))
		{
			CreateShowAction("sylvie", "sad", 0.4f, 1);
			CreateMoveAction("sylvie", 0.8f);
			CreateMoveAction("sylvie", 0);
			CreateExpressionAction("sylvie", "happy");
			CreateHideAction("sylvie");
			CreateFadeInAction("sylvie");
		}
		else if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.J))
			CreateShowAction("james", "sad", 0.3f, 1);
		else if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.N))
			CreateShowAction("narrator", "happy", 0.6f, 1);
		// Emite partículas
		if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.P))
			EmitParticles();
	}

	/// <summary>
	///		Obtiene un actor
	/// </summary>
	private CharacterActor? GetActor(string name) => _characterManager?.GetActor(name);

	/// <summary>
	///		Cambia la expresión de un personaje
	/// </summary>
	private void CreateExpressionAction(string name, string definition)
	{
		GetActor(name)?.AddAction(new Actors.Characters.Actions.UpdateExpressionCharacterAction()
													{
														DefinitionId = definition,
														Duration = 5
													}
								);
	}

	/// <summary>
	///		Crea una acción para mostrar un personaje
	/// </summary>
	private void CreateShowAction(string name, string definition, float end, float duration)
	{
		GetActor(name)?.AddAction(new Actors.Characters.Actions.ShowCharacterAction()
													{
														DefinitionId = definition,
														StartPosition = Vector2.Zero,
														EndPosition = new Vector2(end, 0),
														StartOpacity = 0,
														EndOpacity = 1,
														Duration = duration
													}
								);
	}

	/// <summary>
	///		Crea una acción para mover un personaje
	/// </summary>
	private void CreateMoveAction(string name, float end)
	{
		GetActor(name)?.AddAction(new Actors.Characters.Actions.MoveCharacterAction()
												{
													EndPosition = new Vector2(end, 0),
													Duration = 1
												}
							   );
	}

	/// <summary>
	///		Crea una acción para ocultar un personaje
	/// </summary>
	private void CreateHideAction(string name)
	{
		GetActor(name)?.AddAction(new Actors.Characters.Actions.FadeCharacterAction()
												{
													EndOpacity = 0,
													Duration = 1
												}
							   );
	}

	/// <summary>
	///		Crea una acción para mostrar un personaje
	/// </summary>
	private void CreateFadeInAction(string name)
	{
		GetActor(name)?.AddAction(new Actors.Characters.Actions.FadeCharacterAction()
												{
													EndOpacity = 1,
													Duration = 3
												}
							   );
	}

	/// <summary>
	///		Emite una serie de partículas
	/// </summary>
	private void EmitParticles()
	{
		if (_particlesManager is not null)
		{
			Random random = new();
			int type = random.Next(1, 5);
			Vector2 position = new(random.Next(-200, 200), random.Next(-200, 200));

				// Cambia la posición del emisor
				_particlesManager.Position = position;
				// Dependiendo del valor aleatorio
				switch (type)
				{
					case 1:
							_particlesManager.Effect = new Bau.Libraries.BauGame.Engine.Actors.Particles.Effects.ParticlesConfettiEffect(1_000);
						break;
					case 2:
							_particlesManager.Effect = new Bau.Libraries.BauGame.Engine.Actors.Particles.Effects.ParticlesExplossionEffect(1_000);
						break;
					case 3:
							_particlesManager.Effect = new Bau.Libraries.BauGame.Engine.Actors.Particles.Effects.ParticlesFireworkEffect(1_000);
						break;
					case 4:
							_particlesManager.Effect = new Bau.Libraries.BauGame.Engine.Actors.Particles.Effects.ParticlesSparklesEffect(1_000);
						break;
				}
				// Emite las partículas
				_particlesManager.Emit();
		}
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
