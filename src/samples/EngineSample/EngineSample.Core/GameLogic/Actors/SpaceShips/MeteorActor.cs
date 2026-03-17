using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Actors.Components.Health;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Games;
using Bau.Libraries.BauGame.Engine.Managers;

namespace EngineSample.Core.GameLogic.Actors.SpaceShips;

/// <summary>
///		Meteoro
/// </summary>
public class MeteorActor : AbstractActorDrawable
{
	// Enumerados públicos
	public enum MeteorSize
	{
		Big,
		Medium,
		Small
	}
	// Variables privadas
	private HealthComponent _health;
	private CollisionComponent _collision;

	public MeteorActor(AbstractLayer layer, string name, MeteorSize size, string texture, string region, int physicsPlayerLayer) : base(layer, null)
	{
		// Inicializa las propiedades
		Name = name;
		Size = size;
		Texture = texture; // ... almacena la textura para cuando se lancen meteoritos hijo al destruir este
		Region = region;
		// Configura el renderer
		Renderer.Sprite = new Bau.Libraries.BauGame.Engine.Entities.Common.Sprites.SpriteDefinition(texture, region);
		switch (size)
		{
			case MeteorSize.Medium:
					Renderer.Scale = new Vector2(0.6f, 0.6f);
				break;
			case MeteorSize.Small:
					Renderer.Scale = new Vector2(0.35f, 0.35f);
				break;
		}
		// Configura las colisiones
		_collision = new(this, physicsPlayerLayer);
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
	protected override void UpdateActor(GameContext gameContext)
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
	private void TreaActorIsDead(GameContext gameContext, bool killedByPlayer)
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
			Layer.Scene.MessagesManager.SendMessage(this, PlayerActor.PlayerName, Constants.MessageEnemyKilled, "Meteor eliminated", GetScore());
			// Genera los meteoros hijo
			for (int index = 0; index < 2; index++)
				switch (Size)
				{
					case MeteorSize.Big:
							SpawnMeteor(MeteorSize.Medium);
						break;
					case MeteorSize.Medium:
							SpawnMeteor(MeteorSize.Small);
						break;
				}
		}
		// Destruye el actor (cuando ya haya terminado la animación)
		Layer.Actors.MarkToDestroy(this, gameContext.GetTotalTime(TimeSpan.FromMilliseconds(1)));
		// Marca el actor como eliminado
		_health.MarkAsDead();
		// Si está en el tamaño más pequeño, lanza un powerup
		if (Size == MeteorSize.Small && killedByPlayer)
			SpawnPowerUp();

		// Obtiene la puntuación dependiendo del tamaño
		int GetScore()
		{
			return Size switch
					{
						MeteorSize.Big => 20,
						MeteorSize.Small => 30,
						_ => 40
					};
		}
	}

	/// <summary>
	///		Crea un meteorito cuando se destruye este
	/// </summary>
	private void SpawnMeteor(MeteorSize size)
	{
		MeteorActor meteor = new(Layer, "meteor", size, Texture, Region, _collision.PhysicLayerId);

			// Asigna la posición y la dirección
			meteor.Transform.Bounds.MoveTo(Transform.Bounds.Location);
			meteor.Direction = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandomDirection();
			meteor.RotationSpeed = Bau.Libraries.BauGame.Engine.Tools.Randomizer.GetRandom(0.3f, 0.7f);
			// Añade el meteoro al buffer de la pantalla
			Layer.Actors.Add(meteor);
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
				Layer.Actors.Add(powerUp);
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
	///		Tamaño del meteoro
	/// </summary>
	public MeteorSize Size { get; }

	/// <summary>
	///		Textura: en realidad, no se utiliza en el renderer si no al crear meteoros hijo cuando este explota
	/// </summary>
	public string Texture { get; }

	/// <summary>
	///		Región: en realidad, no se utiliza en el renderer si no al crear meteoros hijo cuando este explota
	/// </summary>
	public string Region { get; }

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