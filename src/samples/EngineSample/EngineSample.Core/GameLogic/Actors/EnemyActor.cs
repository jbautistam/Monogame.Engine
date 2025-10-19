using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;

namespace EngineSample.Core.GameLogic.Actors;

/// <summary>
///		Actor del enemigo
/// </summary>
public class EnemyActor : AbstractActor
{
	// Variables privadas
	private Vector2 _speed = new();

	public EnemyActor(AbstractLayer layer, int physicsPlayerLayer) : base(layer, 1)
	{
		CollisionComponent collision = new(this, physicsPlayerLayer);

			// Configura las colisiones
			collision.Colliders.Add(new CircleCollider(collision, null));
			// Añade la colisión a los componentes
			Components.Add(collision);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public override void Start()
	{
		_speed = new Vector2(Velocity, 0);
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		// Mueve el enemigo a la izquierda / derecha
		if (Transform.WorldBounds.X < 0)
		{
			Transform.WorldBounds.X = 0;
			Transform.WorldBounds.Y = 200;
			_speed = new Vector2(Velocity, 0);
		}
		else if (Transform.WorldBounds.X > MinimumX)
		{
			Transform.WorldBounds.X = MinimumX;
			Transform.WorldBounds.Y = 200;
			_speed = new Vector2(-1 * Velocity, 0);
		}
		// Coloca el jugador
		Transform.WorldBounds.Translate(_speed * gameContext.DeltaTime);
		// Asigna la animación
		if (_speed.X == 0 && _speed.Y == 0)
			Renderer.StartAnimation("monsterA-idle", "monsterA-idle-animation", false);
		else
			Renderer.StartAnimation("monsterA-run", "monsterA-run-animation", true);
		// Cambia la orientación del sprite
		if (_speed.X > 0)
			Renderer.SpriteEffects = SpriteEffects.FlipHorizontally;
		else
			Renderer.SpriteEffects = SpriteEffects.None;
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
	///		Velocidad
	/// </summary>
	public float Velocity { get; set; } = 150f;

	/// <summary>
	///		Valor mínimo de X
	/// </summary>
	public float MinimumX { get; set; } = 500f;
}