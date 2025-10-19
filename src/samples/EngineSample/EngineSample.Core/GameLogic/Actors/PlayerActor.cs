using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Actors.Components.Health;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Actors.Components.Shooting;

namespace EngineSample.Core.GameLogic.Actors;

/// <summary>
///		Actor del jugador
/// </summary>
public class PlayerActor : AbstractActor
{
	// Variables privadas
	private Vector2 _speed = new();
	private HealthComponent _health;
	private ShooterComponent _shooter;

	public PlayerActor(AbstractLayer layer, int physicsLayer) : base(layer, 0)
	{
		CollisionComponent collision = new(this, physicsLayer);

			// Inicializa la colisión
			collision.Colliders.Add(new RectangleCollider(collision, null));
			// Inicializa el componente de salud
			_health = new HealthComponent(this)
									{
										Health = 100,
										Lives = 3,
										InvulnerabilityTime = 3,
										InvulnerabilityEffect = new Bau.Libraries.BauGame.Engine.Actors.Components.Renderers.Effects.BlinkRendererEffect(Renderer, null)
																		{
																			Colors = [ Color.Green, Color.Red, Color.Navy ],
																			TimeBetweenColor = 0.5f
																		}
									};
			// Inicializa el shooter
			_shooter = new ShooterComponent(this);
			// Añade los componentes creados a la lista
			Components.Add(collision);
			Components.Add(_health);
			Components.Add(_shooter);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public override void Start()
	{
		CreateGun();
	}

	/// <summary>
	///		Crea un arma
	/// </summary>
	private void CreateGun()
	{
		WeaponBuilder builder = new();

			// Crea una pistola
			builder.WithPistol("Gun", "Gem", string.Empty, 0);
			// Añade el arma
			_shooter.Weapons.AddRange(builder.Build());
			// Selecciona el arma
			_shooter.EquipWeapon("Gun");
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
		if (!_health.IsDead)
		{
			List<AbstractCollider> colliders = Layer.Scene.PhysicsManager.CollisionSpatialGrid.GetPotentialColliders(this);

				// Calcula la salud del jugador
				ComputeHealth(colliders);
				// Mueve al jugador
				Move(gameContext);
				// Dispara
				Shoot(gameContext);
		}
	}

	/// <summary>
	///		Calcula la salud del jugador
	/// </summary>
	private void ComputeHealth(List<AbstractCollider> colliders)
	{
		foreach (AbstractCollider collider in colliders)
			if (collider.Collision.Owner is EnemyActor)
			{
				HealthComponent? health = Components.GetComponent<HealthComponent>();

					if (health is not null)
						health.ApplyDamage(10f);
			}
	}

	/// <summary>
	///		Mueve al jugador
	/// </summary>
	private void Move(GameContext gameContext)
	{
		// Inicializa la velocidad
		_speed = new Vector2();
		// Mueve el jugador con el teclado
		if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaultActionUp))
			_speed.Y = -Velocity;
		if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaultActionDown))
			_speed.Y = Velocity;
		if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaultActionLeft))
			_speed.X = -Velocity;
		if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaultActionRight))
			_speed.X = Velocity;
		// Coloca el jugador
		Transform.WorldBounds.Translate(_speed * gameContext.DeltaTime);
		// Normaliza la posición
		Transform.WorldBounds.Clamp(Layer.Scene.WorldBounds);
		// Asigna la animación
		if (_speed.X == 0 && _speed.Y == 0)
			Renderer.StartAnimation("player-celebrate", "player-celebrate-animation", false);
		else
			Renderer.StartAnimation("player-run", "player-run-animation", true);
		// Cambia la orientación del sprite
		if (_speed.X > 0)
			Renderer.SpriteEffects = SpriteEffects.FlipHorizontally;
		else
			Renderer.SpriteEffects = SpriteEffects.None;
	}

	/// <summary>
	///		Controla los disparos
	/// </summary>
	private void Shoot(GameContext gameContext)
	{
		// Inicializa la velocidad
		_speed = new Vector2();
		// Mueve el jugador con el teclado
		if (GameEngine.Instance.InputManager.IsAction(Constants.InputShootAction))
			_shooter.Shoot();
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawActor(Camera2D camera, GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor()
	{
	}

	/// <summary>
	///		Velocidad
	/// </summary>
	public float Velocity { get; set; } = 100f;
}