using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Actors;
using Bau.BauEngine.Actors.Components.Physics;
using Bau.BauEngine.Scenes.Cameras;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Actors.Components.Health;
using Bau.BauEngine.Actors.Components.IA;
using Bau.BauEngine.Actors.Components.IA.FiniteStateMachines;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Actors.Components.Renderers;

namespace EngineSample.Core.GameLogic.Actors;

/// <summary>
///		Actor con máquina de estados
/// </summary>
public class EnemyFsmActor : AbstractActorDrawable
{
	// Variables privadas
	private HealthComponent _health;
	private CollisionComponent _collision;
	private BrainComponent _brain;

	public EnemyFsmActor(AbstractLayer layer, string name, int physicsPlayerLayer) : base(layer, 1)
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
									InvulnerabilityEffect = new Bau.BauEngine.Actors.Components.Renderers.Effects.BlinkRendererEffect(Renderer, null)
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
		if (Renderer is RendererAnimatorComponent animator)
			animator.AnimatorBlenderProperties = new Bau.BauEngine.Actors.Components.Renderers.AnimatorBlenderProperties(Name);
		// Añade la etiqueta
		Tags.Add(Constants.TagEnemy);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected override void StartActor()
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
	protected override void UpdateActor(GameContext gameContext)
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
		if (Renderer is RendererAnimatorComponent animator && animator.AnimatorBlenderProperties is not null)
		{
			if (speed.X != 0 || speed.Y != 0)
				animator.AnimatorBlenderProperties.Add("speed", 1);
			else
				animator.AnimatorBlenderProperties.Add("speed", 0);
			animator.AnimatorBlenderProperties.Add("died", isDead);
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
		Layer.Scene.MessagesManager.SendMessage(this, PlayerActor.PlayerName, Constants.MessageEnemyKilled, "I'm died", 90);
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawSelf(Bau.BauEngine.Scenes.Rendering.AbstractRenderingManager renderingManager, GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor(GameContext gameContext)
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