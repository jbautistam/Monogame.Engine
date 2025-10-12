using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Monogame.Engine.Domain;
using Bau.Monogame.Engine.Domain.Core.Actors;
using Bau.Monogame.Engine.Domain.Core.Actors.Components.Health;
using Bau.Monogame.Engine.Domain.Core.Actors.Components.Physics;
using Bau.Monogame.Engine.Domain.Core.Models;
using Bau.Monogame.Engine.Domain.Core.Scenes.Cameras;
using Bau.Monogame.Engine.Domain.Core.Scenes.Layers;

namespace EngineSample.Core.GameLogic.Actors;

/// <summary>
///		Actor del jugador
/// </summary>
public class PlayerActor : AbstractActor
{
	// Variables privadas
	private Vector2 _speed = new();
	private HealthComponent _health;

	public PlayerActor(AbstractLayer layer, int physicsPlayerLayer) : base(layer)
	{
		CollisionComponent collision = new(this, physicsPlayerLayer);

			// Inicializa la colisión
			collision.Colliders.Add(new RectangleCollider(collision, new RectangleF(0, 0, 1, 1)));
			// Inicializa el componente de salud
			_health = new HealthComponent(this)
									{
										Health = 100,
										Lives = 3,
										InvulnerabilityTime = 3,
										InvulnerabilityEffect = new Bau.Monogame.Engine.Domain.Core.Actors.Components.Renderers.Effects.BlinkRendererEffect(Renderer, null)
																		{
																			Colors = [ Color.Green, Color.Red, Color.Navy ],
																			TimeBetweenColor = 0.5f
																		}
									};
			// Añade los componentes creados a la lista
			Components.Add(collision);
			Components.Add(_health);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public override void Start()
	{
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameTime gameTime)
	{
		List<AbstractCollider> colliders = Layer.Scene.PhysicsManager.CollisionSpatialGrid.GetPotentialColliders(this);

			// Ajusta el daño
			foreach (AbstractCollider collider in colliders)
			{
				if (collider.Collision.Owner is EnemyActor)
				{
					HealthComponent? health = Components.GetComponent<HealthComponent>();

						if (health is not null)
							health.ApplyDamage(10f);
				}
			}
			// Inicializa la velocidad
			_speed = new Vector2();
			// Mueve el jugador con el teclado
			if (GameEngine.Instance.InputManager.KeyboardManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Up))
				_speed.Y = -Velocity;
			if (GameEngine.Instance.InputManager.KeyboardManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Down))
				_speed.Y = Velocity;
			if (GameEngine.Instance.InputManager.KeyboardManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Left))
				_speed.X = -Velocity;
			if (GameEngine.Instance.InputManager.KeyboardManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Right))
				_speed.X = Velocity;
			// Coloca el jugador
			Transform.WorldBounds.Translate(_speed * (float) gameTime.ElapsedGameTime.TotalSeconds);
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
	///		Dibuja el actor
	/// </summary>
	protected override void DrawActor(Camera2D camera, GameTime gameTime)
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