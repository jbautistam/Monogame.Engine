using Bau.Libraries.BauGame.Engine.Actors.Components.Health;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Projectiles;

/// <summary>
///     Clase con los datos de un proyectil
/// </summary>
public class ProjectileActor : AbstractActor
{
    // Variables privadas
    private CollisionComponent _collision;

    public ProjectileActor(AbstractLayer layer, int zOrder, int physicsLayer) : base(layer, null)
    {
		// Inicializa el componente de la colisión
		_collision = new(this, physicsLayer);
		_collision.Colliders.Add(new RectangleCollider(_collision, null));
		// Añade los componentes creados a la lista
		Components.Add(_collision);
    }

    /// <summary>
    ///     Arranca los datos del actor
    /// </summary>
    public override void StartActor()
    {
    }

    /// <summary>
    ///     Crea un proyectil
    /// </summary>
	public void Shoot(ProjectileProperties properties, Vector2 position, Vector2 velocity, float rotation, int physicsLayer)
    {
        // Actualiza las propiedades
        Properties = properties;
        // Inicializa los datos de posición
        Transform.Bounds = new Models.RectangleF(position.X, position.Y, 0, 0);
        Transform.Rotation = rotation;
        // Inicializa los datos de dibujo
        Renderer.Texture = Properties.Texture;
        Renderer.Region = Properties.Region;
        if (!string.IsNullOrWhiteSpace(Properties.Animation))
        {
            Renderer.Animator.Reset();
            Renderer.StartAnimation(Renderer.Texture, Properties.Animation, false);
        }
        // Inicializa el resto de propiedades
        CurrentDistance = 0f;
        // Calcula la velocidad
        Velocity = velocity;
        // Cambia la capa física
        _collision.PhysicLayerId = physicsLayer;
        // Indica que está activo
        Enabled = true;
    }

    /// <summary>
    ///     Actualiza los datos del proyectil
    /// </summary>
    protected override void UpdateActor(Managers.GameContext gameContext)
    {
        if (Enabled && Properties is not null)
        {
            if (CheckCollision(gameContext))
                RemoveProjectile(true);
            else
            {
                Vector2 previousPosition = Transform.Bounds.TopLeft;
        
                    // Actualizar posición
                    Transform.Bounds.Translate(Velocity * gameContext.DeltaTime);
                    // Calcula la distancia recorrida en este frame
                    CurrentDistance += Vector2.Distance(previousPosition, Transform.Bounds.TopLeft);
                    // Comprueba si se superó la distancia máxima
                    if (CurrentDistance >= Properties.MaxDistance)
                        RemoveProjectile(Properties.ShowExplosionWhenEndDistance);
            }
        }
    }

    /// <summary>
    ///     Comprueba las colisiones
    /// </summary>
    private bool CheckCollision(Managers.GameContext gameContext)
    {
        List<AbstractCollider> colliders = Layer.Scene.PhysicsManager.MapManager.CollisionSpatialGrid.GetPotentialColliders(this);

            // Comprueba los elementos con los que colisiones
            if (Properties is not null)
                foreach (AbstractCollider collider in colliders)
                {
                    HealthComponent? health = collider.Collision.Owner.Components.GetComponent<HealthComponent>();

                        if (health is not null)
                            health.ApplyDamage(Properties.Damage);
                }
            // Devuelve el valor que indica si se ha colisionado
            return colliders.Count > 0;
    }

    /// <summary>
    ///     Elimina el proyectil
    /// </summary>
    private void RemoveProjectile(bool createExplosion)
    {
        // Quita el proyectil de la capa de colisiones
        _collision.End();
        // Desactiva el proyectil
        Enabled = false;
        // Si tiene que crear una explosión, la crea
        if (createExplosion && Properties?.Explosion is not null && Layer is Scenes.Layers.Games.AbstractGameLayer gameLayer)
            gameLayer.ExplosionsManager.Create(Properties.Explosion, new Vector2(Transform.Bounds.X, Transform.Bounds.Y - 2 * Transform.Bounds.Height));
    }

    /// <summary>
    ///     Dibuja datos adicionales del actor
    /// </summary>
    protected override void DrawActor(Scenes.Cameras.Camera2D camera, Managers.GameContext gameContext)
    {
        // ... no hace nada, sólo implementa la interface
    }

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor()
	{
        // ... no hace nada, sólo implementa la interface
	}

    /// <summary>
    ///     Vector de velocidad
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    ///     Propiedades del proyectil
    /// </summary>
    public ProjectileProperties? Properties { get; private set; }

    /// <summary>
    ///     Distancia recorrida por el proyectil
    /// </summary>
    public float CurrentDistance { get; private set; }
}