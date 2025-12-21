using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Actors.Components.Health;
using Bau.Libraries.BauGame.Engine.Actors.Components.IA;
using Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

namespace EngineSample.Core.GameLogic.Actors.SpaceShips;

/// <summary>
///		Segundo actor enemigo
/// </summary>
public class SpaceEnemyFsmActor : AbstractActor
{
	// Variables privadas
	private HealthComponent _health;
	private CollisionComponent _collision;
	private BrainComponent _brain;

	public SpaceEnemyFsmActor(AbstractLayer layer, string name, int physicsPlayerLayer) : base(layer, null)
	{
		// Inicializa las propiedades
		Name = name;
		// Configura las colisiones
		_collision = new(this, physicsPlayerLayer);
		_collision.Colliders.Add(new RectangleCollider(_collision, null));
		// Inicializa el componente de salud
		_health = new HealthComponent(this)
								{
									Health = 100,
									Lives = 1,
									InvulnerabilityTime = 3,
									InvulnerabilityEffect = new Bau.Libraries.BauGame.Engine.Actors.Components.Renderers.Effects.BlinkRendererEffect(Renderer, null)
																	{
																		Colors = [ Color.Green, Color.Red, Color.Navy ],
																		TimeBetweenColor = 0.5f
																	}
								};
		// Crea el componente de IA
		_brain = new BrainComponent(this);
		// Añade los componentes a la lista
		Components.Add(_collision);
		Components.Add(_health);
		Components.Add(_brain);
		// Inicializa las propiedades de animación
		Renderer.AnimatorBlenderProperties = new Bau.Libraries.BauGame.Engine.Actors.Components.Renderers.AnimatorBlenderProperties(Name);
		// Añade la etiqueta
		Tags.Add(Constants.TagEnemy);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public override void StartActor()
	{
		ConfigureFiniteStateMachine();
	}

	/// <summary>
	///		Configura la máquina de estados
	/// </summary>
	private void ConfigureFiniteStateMachine()
	{
		// Añade los estados
		_brain.StatesMachineManager.States.Add(new IdleState("Idle", new PropertiesState()
																			{
																				BlenderGroup = Name,
																				Duration = 10,
																				SpeedMaximum = Velocity,
																				NextState = "Walk"
																			}
															)
												);
		_brain.StatesMachineManager.States.Add(new RandomWalkingState("Walk", new PropertiesState()
																			{
																				BlenderGroup = Name,
																				Duration = 10,
																				SpeedMaximum = Velocity,
																				NextState = "Idle"
																			}
															)
												);
		// Asigna el manejador de eventos
		_brain.IsDead += (sender, args) => TreaActorIsDead();
		// Arranca la IA (porque se ha configurado después de crearlo)
		_brain.Start();
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		// Actualiza la inteligencia artificial
		_brain.Update(gameContext);
		// Actualiza el blender de animaciones
		UpdateAnimation(_brain.AgentSteeringManager.Velocity, _health.JustDead || _health.IsDead);
	}

	/// <summary>
	///		Actualiza las propiedades de animación
	/// </summary>
	private void UpdateAnimation(Vector2 speed, bool isDead)
	{
		// Asigna las propiedades
		if (Renderer.AnimatorBlenderProperties is not null)
		{
			if (speed.X != 0 || speed.Y != 0)
				Renderer.AnimatorBlenderProperties.Add("speed", 1);
			else
				Renderer.AnimatorBlenderProperties.Add("speed", 0);
			Renderer.AnimatorBlenderProperties.Add("died", isDead);
		}
		// Cambia la orientación del sprite
		if (speed.X > 0)
			Renderer.SpriteEffects = SpriteEffects.FlipHorizontally;
		else
			Renderer.SpriteEffects = SpriteEffects.None;
	}

	/// <summary>
	///		Trata el resto de la muerte del actor
	/// </summary>
	private void TreaActorIsDead()
	{
		Layer.Scene.MessagesManager.SendMessage(PlayerActor.PlayerName, 
												new Bau.Libraries.BauGame.Engine.Scenes.Messages.MessageModel(this, Constants.MessageEnemyKilled)
															{
																Message = "I'm died",
																Tag = 90
															}
												);
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawActor(Camera2D camera, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor()
	{
	}

	/// <summary>
	///		Nombre del actor
	/// </summary>
	public string Name { get; }

	/// <summary>
	///		Velocidad
	/// </summary>
	public float Velocity { get; set; } = 150f;

	/// <summary>
	///		Valor mínimo de X
	/// </summary>
	public float MinimumX { get; set; } = 500f;
}