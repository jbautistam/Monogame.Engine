using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Actors.Components.Health;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Actors.Components.Shooting;
using Bau.Libraries.BauGame.Engine.Scenes.Messages;

namespace EngineSample.Core.GameLogic.Actors.SpaceShips;

/// <summary>
///		Actor (nave espacial) del jugador 
/// </summary>
public class SpacePlayerActor : AbstractActorDrawable
{
	// Constantes públicas
	public const string PlayerName = nameof(PlayerName);
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
	private HealthComponent _health;
	private ShooterComponent _shooter;
	private int _score;

	public SpacePlayerActor(AbstractLayer layer, int physicsLayer) : base(layer, 0)
	{
		CollisionComponent collision = new(this, physicsLayer);

			// Guarda la capa de físicas (para que sea más rápido de localizar)
			PhysicsLayer = physicsLayer;
			// Inicializa la colisión
			collision.Colliders.Add(new CircleCollider(collision, null));
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
			// Inicializa las propiedades de animación
			Renderer.AnimatorBlenderProperties = new Bau.Libraries.BauGame.Engine.Actors.Components.Renderers.AnimatorBlenderProperties("SpacePlayer");
			// Añade los componentes creados a la lista
			Components.Add(collision);
			Components.Add(_health);
			Components.Add(_shooter);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected override void StartActor()
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
			builder.WithPistol(WeaponGun, "laser", string.Empty, 0, new Vector2(30, 30));
			// Añade el arma al slot principal
			_shooter.AddWeapons(SlotPrimary, builder.Build());
			// Crea una granada para el slot secundario
			builder = new WeaponBuilder();
			builder.WithGranade(WeaponGrenade, "laser", string.Empty, "explosion", string.Empty, "explosion-animation", 0, new Vector2(15, 0));
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
			List<AbstractCollider> colliders = Layer.Scene.PhysicsManager.MapManager.CollisionSpatialGrid.GetPotentialColliders(this);

				// Calcula la salud del jugador
				ComputeHealth(colliders, gameContext);
				// Mueve el actor
				if (!_health.IsDead)
				{
					// Mueve al jugador
					Move(gameContext);
					// Dispara
					Shoot(gameContext);
				}
				// Añade el actor a la lista de objetivos de la cámara
				Layer.Scene.Camera.TargetsManager.Add(this);
		}
		// Actualiza las propiedades de animación
		UpdateAnimation(Velocity, _health.IsDead);
		// Trata los mensajes recibidos
		TreatReceivedMessages();
		// Envía los mensajes al Hud
		SendHudMessages();
	}

	/// <summary>
	///		Calcula la salud del jugador
	/// </summary>
	private void ComputeHealth(List<AbstractCollider> colliders, GameContext gameContext)
	{
		foreach (AbstractCollider collider in colliders)
			if (collider.Collision.Owner.Tags.Contains(Constants.TagEnemy))
			{
				HealthComponent? health = Components.GetComponent<HealthComponent>();

					if (health is not null)
						health.ApplyDamage(10f);
			}
			else if (collider.Collision.Owner.Tags.Contains(Constants.TagPowerUp) && collider.Collision.Owner is PowerUpActor powerUp)
			{
				TreatPowerUp(powerUp);
				powerUp.Destroy(gameContext);
			}
	}

	/// <summary>
	///		Trata el powerup
	/// </summary>
	private void TreatPowerUp(PowerUpActor powerUp)
	{
	}

	/// <summary>
	///		Actualiza las propiedades de animación
	/// </summary>
	private void UpdateAnimation(Vector2 speed, bool isDead)
	{
		if (Renderer.AnimatorBlenderProperties is not null)
		{
			if (speed.X != 0 || speed.Y != 0)
				Renderer.AnimatorBlenderProperties.Add("speed", 1);
			else
				Renderer.AnimatorBlenderProperties.Add("speed", 0);
			Renderer.AnimatorBlenderProperties.Add("died", isDead);
		}
	}

	/// <summary>
	///		Mueve al jugador
	/// </summary>
	private void Move(GameContext gameContext)
	{
		// Cambia la rotación
		if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaultActionLeft))
			Transform.Rotation -= RotationSpeed * gameContext.DeltaTime;
		else if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaultActionRight))
			Transform.Rotation += RotationSpeed * gameContext.DeltaTime;
		// Normaliza la rotación para evitar la acumulación
		Transform.Rotation = MathHelper.WrapAngle(Transform.Rotation); //TODO ... ¿no se podría cambiar el field de la propiedad?
		// Acelera / decelera (después de rotar)
		if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaultActionUp))
		{
            // Acelera en la dirección a la que apunta el jugador
            Velocity += new Vector2((float) Math.Cos(Transform.Rotation), (float) Math.Sin(Transform.Rotation)) * Acceleration;
            // Limita la velocidad máxima
            if (Velocity.Length() > MaximumSpeed)
                Velocity = Vector2.Normalize(Velocity) * MaximumSpeed;
		}
		else if (GameEngine.Instance.InputManager.IsAction(Bau.Libraries.BauGame.Engine.Managers.Input.InputMappings.DefaultActionDown))
		{
            if (Velocity.Length() > Deceleration)
                Velocity -= new Vector2((float) Math.Cos(Transform.Rotation), (float) Math.Sin(Transform.Rotation)) * Deceleration;
            else
                Velocity = Vector2.Zero; // Detener completamente si es menor que la tasa de deceleración
		}
        else // ... aplica la fricción para el frenado
		{
            if (Velocity.Length() > Friction)
                Velocity -= new Vector2((float) Math.Cos(Transform.Rotation), (float) Math.Sin(Transform.Rotation)) * Friction;
            else
                Velocity = Vector2.Zero; // Detener completamente si es menor que la tasa de fricción
		}   
		// Coloca el jugador
		Transform.Bounds.Translate(Velocity * gameContext.DeltaTime);
		// Normaliza la posición
		Transform.Bounds.Clamp(Layer.Scene.WorldDefinition.WorldBounds);
	}

	/// <summary>
	///		Controla los disparos
	/// </summary>
	private void Shoot(GameContext gameContext)
	{
		if (GameEngine.Instance.InputManager.IsAction(Constants.InputShootAction))
			Shoot(SlotPrimary);
		else if (GameEngine.Instance.InputManager.IsAction(Constants.InputShootGrenadeAction))
			Shoot(SlotSecondary);
			
		// Lanza el proyectil
		void Shoot(string slot)
		{
			_shooter.Shoot(slot, Transform.Bounds.TopLeft, Transform.Rotation, Scenes.Games.GameScene.PhysicsPlayerProjectileLayer);
		}
	}

	/// <summary>
	///		Trata los mensajes recibidos
	/// </summary>
	private void TreatReceivedMessages()
	{
		List<MessageModel> messages = Layer.Scene.MessagesManager.GetReceived(PlayerName);

			// Trata los mensajes
			foreach (MessageModel message in messages)
				switch (message.Type)
				{
					case Constants.MessageEnemyKilled:
							if (message.Tag is int killScore)
								_score += killScore;
							else
								_score += 10;
						break;
				}
	}

	/// <summary>
	///		Envía los mensajes al hud
	/// </summary>
	private void SendHudMessages()
	{
		List<MessageModel> messages = [];

			// Crea los mensajes
			messages.Add(new MessageModel(this, "PlayerPosition", Constants.LayerHud)
								{
									Message = $"{Transform.Bounds.TopLeft.X:0}-{Transform.Bounds.TopLeft.Y:0}"
								}
						);
			messages.Add(new MessageModel(this, "Score", Constants.LayerHud)
								{
									Message = _score.ToString()
								}
						);
			messages.Add(new MessageModel(this, "Lives", Constants.LayerHud)
								{
									Message = Math.Clamp(_health.Lives, 0, _health.Lives).ToString()
								}
						);
			// Envía los mensajes
			if (messages.Count > 0)
				Layer.Scene.MessagesManager.AddRange(messages);
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawSelf(Bau.Libraries.BauGame.Engine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor(GameContext gameContext)
	{
	}

	/// <summary>
	///		Velocidad actual
	/// </summary>
	public Vector2 Velocity { get; private set; } = Vector2.Zero;

	/// <summary>
	///		Velocidad máxima
	/// </summary>
	public float MaximumSpeed { get; set; } = 250f;

	/// <summary>
	///		Aceleración
	/// </summary>
	public float Acceleration { get; set; } = 25f;

	/// <summary>
	///		Deceleración
	/// </summary>
	public float Deceleration { get; set; } = 12f;

	/// <summary>
	///		Fricción
	/// </summary>
	public float Friction { get; set; } = 5f;

	/// <summary>
	///		Velocidad de rotación
	/// </summary>
	public float RotationSpeed { get; set; } = 2.0f;

	/// <summary>
	///		Capa de físicas
	/// </summary>
	public int PhysicsLayer { get; }
}