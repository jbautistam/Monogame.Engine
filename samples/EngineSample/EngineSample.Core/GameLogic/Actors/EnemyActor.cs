using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Actors;
using Bau.BauEngine.Actors.Components.Physics;
using Bau.BauEngine.Scenes.Cameras;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Actors.Components.Health;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Actors.Components.Renderers;

namespace EngineSample.Core.GameLogic.Actors;

/// <summary>
///		Actor del enemigo
/// </summary>
public class EnemyActor : AbstractActorDrawable
{
	// Variables privadas
	private Vector2 _speed = new();
	private HealthComponent _health;
	private CollisionComponent _collision;

	public EnemyActor(AbstractLayer layer, int physicsPlayerLayer) : base(layer, 1)
	{
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
		// Añade los componentes a la lista
		Components.Add(_collision);
		Components.Add(_health);
		// Añade la etiqueta
		Tags.Add(Constants.TagEnemy);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected override void StartActor()
	{
		_speed = new Vector2(Velocity, 0);
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
		if (_health.JustDead)
		{
			// Desactiva la colisión
			_collision.ToggleEnabled(false);
			// Cambia la animación
			if (Renderer is RendererAnimatorComponent animator)
				animator.StartAnimation("monsterA-die", "monsterA-die-animation", false);
			// Indica que ya se ha tratado el "justdead"
			_health.JustDead = false;
			// Destruye el actor
			Layer.Actors.MarkToDestroy(this, gameContext.GetTotalTime(TimeSpan.FromSeconds(5)));
			// Manda un mensaje indicando que se ha matado un enemigo
			Layer.Scene.MessagesManager.SendMessage(this, PlayerActor.PlayerName, Constants.MessageEnemyKilled, "I'm died", 30);
		}
		else if (!_health.IsDead)
			Move(gameContext);
	}

	/// <summary>
	///		Mueve el actor
	/// </summary>
	private void Move(GameContext gameContext)
	{
		// Mueve el actor a la izquierda / derecha
		if (Transform.Bounds.X < 0)
			_speed = new Vector2(Velocity, 0);
		else if (Transform.Bounds.X > MinimumX)
			_speed = new Vector2(-1 * Velocity, 0);
		// Mueve el actor
		Transform.Bounds.Translate(_speed * gameContext.DeltaTime);
		// Asigna la animación
		if (Renderer is RendererAnimatorComponent animator)
		{
			if (_speed.X == 0 && _speed.Y == 0)
				animator.StartAnimation("monsterA-idle", "monsterA-idle-animation", false);
			else
				animator.StartAnimation("monsterA-run", "monsterA-run-animation", true);
		}
		// Cambia la orientación del sprite
		if (_speed.X > 0)
			Renderer.SpriteEffects = SpriteEffects.FlipHorizontally;
		else
			Renderer.SpriteEffects = SpriteEffects.None;
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawSelf(Bau.BauEngine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor(GameContext gameContext)
	{
	}

	/// <summary>
	///		Velocidad
	/// </summary>
	public float Velocity { get; set; } = 150f;

	/// <summary>
	///		Valor mínimo de X
	/// </summary>
	public float MinimumX { get; set; } = 500f;
}