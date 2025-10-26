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
	// Constantes privadas
	private const string SlotPrimary = "Primary";
	private const string SlotSecondary = "Secondary";
	private const string WeaponGun = "Gun";
	private const string WeaponGrenade = "Grenade";
	// Enumerados públicos
	public enum MoveMode
	{
		LeftToRight,
		RightToLeft
	}
	// Variables privadas
	private Vector2 _speed = new();
	private HealthComponent _health;
	private ShooterComponent _shooter;

	public PlayerActor(AbstractLayer layer, int physicsLayer) : base(layer, 0)
	{
		CollisionComponent collision = new(this, physicsLayer);

			// Guarda la capa de físicas (para que sea más rápido de localizar)
			PhysicsLayer = physicsLayer;
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
		CreateWeapons();
	}

	/// <summary>
	///		Crea las armas
	/// </summary>
	private void CreateWeapons()
	{
		WeaponBuilder builder = new();

			// Crea una pistola
			builder.WithPistol(WeaponGun, "laser", string.Empty, 0);
			// Añade el arma al slot principal
			_shooter.AddWeapons(SlotPrimary, builder.Build());
			// Crea una granda para el slot secundario
			builder = new WeaponBuilder();
			builder.WithGranade(WeaponGrenade, "laser", string.Empty, "explosion", string.Empty, "explosion-animation", 0);
			// Selecciona el arma
			_shooter.AddWeapons(SlotSecondary, builder.Build());
			// Equipas las armas principal y secundaria
			_shooter.EquipWeapon(SlotPrimary, WeaponGun);
			_shooter.EquipWeapon(SlotSecondary, WeaponGrenade);
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
				// Si acaba de morir, cambia la animación
				if (_health.IsDead)
					Renderer.StartAnimation("player-die", "player-die-animation", false);
				else
				{
					// Mueve al jugador
					Move(gameContext);
					// Dispara
					Shoot(gameContext);
				}
		}
	}

	/// <summary>
	///		Calcula la salud del jugador
	/// </summary>
	private void ComputeHealth(List<AbstractCollider> colliders)
	{
		foreach (AbstractCollider collider in colliders)
			if (collider.Collision.Owner.Tags.Contains(Constants.TagEnemy))
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
		Transform.Bounds.Translate(_speed * gameContext.DeltaTime);
		// Normaliza la posición
		Transform.Bounds.Clamp(Layer.Scene.WorldBounds);
		// Asigna la animación
		if (_speed.X == 0 && _speed.Y == 0)
			Renderer.StartAnimation("player-celebrate", "player-celebrate-animation", false);
		else
			Renderer.StartAnimation("player-run", "player-run-animation", true);
		// Cambia el modo de movimiento
		if (_speed.X < 0)
			Moving = MoveMode.RightToLeft;
		else if (_speed.X > 0)
			Moving = MoveMode.LeftToRight;
		// Cambia la orientación del sprite
		if (Moving == MoveMode.LeftToRight)
			Renderer.SpriteEffects = SpriteEffects.FlipHorizontally;
		else
			Renderer.SpriteEffects = SpriteEffects.None;
	}

	/// <summary>
	///		Controla los disparos
	/// </summary>
	private void Shoot(GameContext gameContext)
	{
		if (GameEngine.Instance.InputManager.IsAction(Constants.InputShootAction))
			Shoot(SlotPrimary);
		if (GameEngine.Instance.InputManager.IsAction(Constants.InputShootGrenadeAction))
			Shoot(SlotSecondary);

		// Dispara el arma activa en un slot
		void Shoot(string slot)
		{
			Vector2 address = new(1, 0);

				// Cambia la dirección en la que se dispara
				if (Moving == MoveMode.RightToLeft)
					address = new Vector2(-1, 0);
				// Dispara el arma
				_shooter.Shoot(slot, Transform.Bounds.TopLeft, address, 0, Scenes.Games.GameScene.PhysicsPlayerProjectileLayer);
		}
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
	///		Forma de movimiento
	/// </summary>
	public MoveMode Moving { get; private set; } = MoveMode.LeftToRight;

	/// <summary>
	///		Velocidad
	/// </summary>
	public float Velocity { get; set; } = 100f;

	/// <summary>
	///		Capa de físicas
	/// </summary>
	public int PhysicsLayer { get; }
}