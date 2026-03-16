using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Actors.Components.Health;
using Bau.Libraries.BauGame.Engine.Managers;

namespace EngineSample.Core.GameLogic.Actors.SpaceShips;

/// <summary>
///		Bono / Power up
/// </summary>
public class PowerUpActor : AbstractActorDrawable
{
	// Enumerados públicos
	/// <summary>
	///		Tipos de powerup
	/// </summary>
	public enum PowerUpType
	{
		Armor, // Armor_Bonus,0,0,75,75
		Barrier, // Barrier_Bonus,0,76,75,75
		Damage, // Damage_Bonus,76,0,75,75
		EnemyDestroy, // Enemy_Destroy_Bonus,76,76,75,75
		EnemySpeedDebuff, // Enemy_Speed_Debuff,0,152,75,75
		HeroMovementDebuff, // Hero_Movement_Debuff,76,152,75,75
		HeroSpeedDebuff, // Hero_Speed_Debuff,152,0,75,75
		Live, // HP_Bonus,152,76,75,75
		Magnet, // Magnet_Bonus,152,152,75,75
		Rocket // Rockets_Bonus,0,228,75,75
	}
	// Variables privadas
	private HealthComponent _health;
	private CollisionComponent _collision;

	public PowerUpActor(AbstractLayer layer, string name) : base(layer, null)
	{
		// Inicializa las propiedades
		Name = name;
		Type = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom<PowerUpType>();
		// Configura el renderer
		Renderer.Texture = "bonus";
		Renderer.Region = Type switch
								{
									PowerUpType.Armor => "Armor_Bonus",
									PowerUpType.Barrier => "Barrier_Bonus",
									PowerUpType.Damage => "Damage_Bonus",
									PowerUpType.EnemyDestroy => "Enemy_Destroy_Bonus",
									PowerUpType.EnemySpeedDebuff => "Enemy_Speed_Debuff",
									PowerUpType.HeroMovementDebuff => "Hero_Movement_Debuff",
									PowerUpType.HeroSpeedDebuff => "Hero_Speed_Debuff",
									PowerUpType.Live => "HP_Bonus",
									PowerUpType.Magnet => "Magnet_Bonus",
									_ => "Rockets_Bonus"
								};
		// Configura las colisiones
		_collision = new(this, Scenes.Space.SpaceShipsScene.PhysicsPowerUpLayer);
		_collision.Colliders.Add(new CircleCollider(_collision, null));
		// Inicializa el componente de salud
		_health = new HealthComponent(this)
								{
									Health = 100,
									Lives = 1,
									InvulnerabilityTime = 0
								};
		// Añade los componentes a la lista
		Components.Add(_collision);
		Components.Add(_health);
		// Añade la etiqueta
		Tags.Add(Constants.TagPowerUp);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected override void StartActor()
	{
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
		if (_health.JustDead || IsOutLimits())
			Destroy(gameContext);
		else if (!_health.IsDead)
		{
			Transform.Bounds.Translate(Direction * Velocity * gameContext.DeltaTime);
			Transform.Rotation = MathHelper.WrapAngle(Transform.Rotation + RotationSpeed * gameContext.DeltaTime);
		}
	}

	/// <summary>
	///		Comprueba si ha pasado los límites
	/// </summary>
	private bool IsOutLimits()
	{
		if (Direction.X < 0 && Transform.Bounds.Right < 0)
			return true;
		else if (Direction.X > 0 && Transform.Bounds.Right > Layer.Scene.WorldDefinition.WorldBounds.Right + Transform.Bounds.Width)
			return true;
		else if (Direction.Y < 0 && Transform.Bounds.Bottom < 0)
			return true;
		else if (Direction.Y > 0 && Transform.Bounds.Bottom > Layer.Scene.WorldDefinition.WorldBounds.Bottom + Transform.Bounds.Height)
			return true;
		else
			return false;
	}

	/// <summary>
	///		Trata el resto de la muerte del actor
	/// </summary>
	public void Destroy(GameContext gameContext)
	{
		// Desactiva la colisión
		_collision.ToggleEnabled(false);
		// Destruye el actor (cuando ya haya terminado la animación)
		Layer.Actors.MarkToDestroy(this, gameContext.GetTotalTime(TimeSpan.FromMilliseconds(1)));
		// Marca el actor como eliminado
		_health.MarkAsDead();
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
	///		Nombre del actor
	/// </summary>
	public string Name { get; }

	/// <summary>
	///		Tipo de powerUp
	/// </summary>
	public PowerUpType Type { get; }

	/// <summary>
	///		Dirección de movimiento
	/// </summary>
	public Vector2 Direction { get; set; }

	/// <summary>
	///		Velocidad
	/// </summary>
	public float Velocity { get; set; } = 150f;

	/// <summary>
	///		Velocidad de rotación
	/// </summary>
	public float RotationSpeed { get; set; } = 0.1f;
}