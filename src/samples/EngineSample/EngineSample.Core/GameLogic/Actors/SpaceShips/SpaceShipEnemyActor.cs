using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Actors.Components.Health;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Games;
using Bau.Libraries.BauGame.Engine.Tools.Extensors;
using Bau.Libraries.BauGame.Engine.Managers;

namespace EngineSample.Core.GameLogic.Actors.SpaceShips;

/// <summary>
///		Nave enemiga
/// </summary>
public class SpaceShipEnemyActor : AbstractActorDrawable
{
	// Variables privadas
	private HealthComponent _health;
	private CollisionComponent _collision;

	public SpaceShipEnemyActor(AbstractLayer layer, string name) : base(layer, null)
	{
		// Inicializa las propiedades
		Name = name;
		// Configura el renderer
		Renderer.Texture = "enemies";
		Renderer.Region = $"Ship {Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(1, 14):00}";
		// Configura las colisiones
		_collision = new(this, Scenes.Space.SpaceShipsScene.PhysicsNpcLayer);
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
		Tags.Add(Constants.TagEnemy);
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
	protected override void UpdateActor(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		if (_health.JustDead)
			TreaActorIsDead(gameContext, true);
		else if (IsOutLimits())
			TreaActorIsDead(gameContext, false);
		else if (!_health.IsDead)
		{
			Transform.Bounds.Translate(Direction * Velocity * gameContext.DeltaTime);
			Transform.Rotation = Direction.Angle();
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
	private void TreaActorIsDead(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext, bool killedByPlayer)
	{
		// Desactiva la colisión
		_collision.ToggleEnabled(false);
		// Cambia la animación
		if (killedByPlayer)
		{
			// Añade una explosión
			if (Layer is AbstractGameLayer gameLayer)
				gameLayer.ExplosionsManager.Create(new Bau.Libraries.BauGame.Engine.Actors.Projectiles.ExplosionProperties()
																{
																	Texture = "explosion",
																	Region = "default",
																	Animation = "explosion-animation",
																	ZOrder = ZOrder
																},
												   Transform.Bounds.Location);
			// Envía el mensaje de fin para añadir la puntuación
			Layer.Scene.MessagesManager.SendMessage(this, PlayerActor.PlayerName, Constants.MessageEnemyKilled, "Meteor eliminated", 50);
			// Lanza un powerup
			SpawnPowerUp();
		}
		// Destruye el actor (cuando ya haya terminado la animación)
		Layer.Actors.MarkToDestroy(this, gameContext.GetTotalTime(TimeSpan.FromMilliseconds(1)));
		// Marca el actor como eliminado
		_health.MarkAsDead();
	}

	/// <summary>
	///		Lanza un bono si es necesario
	/// </summary>
	private void SpawnPowerUp()
	{
		if (Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(0, 10) > 2)
		{
			PowerUpActor powerUp = new(Layer, "powerup");

				// Asigna la posición y la dirección
				powerUp.Transform.Bounds.MoveTo(Transform.Bounds.Location);
				powerUp.Direction = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandomDirection();
				powerUp.RotationSpeed = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(0.3f, 0.7f);
				// Añade el meteoro al buffer de la pantalla
				Layer.Actors.MarkToDestroy(powerUp, TimeSpan.FromMilliseconds(1));
		}
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