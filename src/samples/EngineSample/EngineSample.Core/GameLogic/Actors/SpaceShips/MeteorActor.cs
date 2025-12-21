using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Actors.Components.Health;

namespace EngineSample.Core.GameLogic.Actors.SpaceShips;

/// <summary>
///		Meteoro
/// </summary>
public class MeteorActor : AbstractActor
{
	// Variables privadas
	private HealthComponent _health;
	private CollisionComponent _collision;

	public MeteorActor(AbstractLayer layer, string name, string texture, string region, int physicsPlayerLayer) : base(layer, null)
	{
		// Inicializa las propiedades
		Name = name;
		// Configura el renderer
		Renderer.Texture = texture;
		Renderer.Region = region;
		// Configura las colisiones
		_collision = new(this, physicsPlayerLayer);
		_collision.Colliders.Add(new RectangleCollider(_collision, null));
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
	public override void StartActor()
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
	private void TreaActorIsDead(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext, bool killedByPlayer)
	{
		// Desactiva la colisión
		_collision.ToggleEnabled(false);
		// Cambia la animación
		if (killedByPlayer)
		{
			// Añade una explosión
			Renderer.StartAnimation("explosion", "explosion-animation", false);
			// Envía el mensaje de fin para añadir la puntuación
			Layer.Scene.MessagesManager.SendMessage(PlayerActor.PlayerName, 
													new Bau.Libraries.BauGame.Engine.Scenes.Messages.MessageModel(this, Constants.MessageEnemyKilled)
																{
																	Message = "Meteor eliminated",
																	Tag = 20
																}
													);
		}
		// Destruye el actor (cuando ya haya terminado la animación)
		Layer.Actors.Destroy(this, gameContext.GetTotalTime(TimeSpan.FromSeconds(1)));
		// Marca el actor como eliminado
		_health.MarkAsDead();
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

	/// <summary>
	///		Valor mínimo de X
	/// </summary>
	public float MinimumX { get; set; } = 500f;
}