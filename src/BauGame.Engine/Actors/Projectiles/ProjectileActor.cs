using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Projectiles;

/// <summary>
///     Clase con los datos de un proyectil
/// </summary>
public class ProjectileActor(AbstractLayer layer, int zOrder) : AbstractActor(layer, zOrder)
{
/*
    // Nuevas propiedades para explosiones
    public bool ExplodesOnImpact { get; set; }
    public float ExplosionRadius { get; set; }
    public int ExplosionDamage { get; set; }
    public bool ExplodesOnTimeout { get; set; }
    
    // Método para crear explosión
    public ExplosionComponent CreateExplosion()
    {
        if (ExplosionRadius > 0 && ExplosionDamage > 0)
        {
            return new ExplosionComponent(Position, ExplosionRadius, ExplosionDamage);
        }
        return null;
    }
*/

    /// <summary>
    ///     Arranca los datos del actor
    /// </summary>
    public override void Start()
    {
    }

    /// <summary>
    ///     Crea un proyectil
    /// </summary>
	public void Shoot(ProjectileProperties properties, Vector2 position, float rotation)
    {
        // Inicializa los datos de posición
        Transform.WorldBounds = new Models.RectangleF(position.X, position.Y, 0, 0);
        Transform.Rotation = rotation;
        // Inicializa los datos de dibujo
        Renderer.Texture = properties.Texture;
        Renderer.Region = properties.Region;
        // Inicializa el resto de propiedades
        Properties = properties;
        CurrentDistance = 0f;
        // Calcula la velocidad
        Velocity = new Vector2((float) Math.Cos(rotation), (float) Math.Sin(rotation)) * properties.Speed;
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
            Vector2 previousPosition = Transform.WorldBounds.TopLeft;
        
                // Actualizar posición
                Transform.WorldBounds.Translate(Velocity * gameContext.DeltaTime);
                // Calcula la distancia recorrida en este frame
                CurrentDistance += Vector2.Distance(previousPosition, Transform.WorldBounds.TopLeft);
                // Comprueba si se superó la distancia máxima
                if (CurrentDistance >= Properties.MaxDistance)
                    Enabled = false;
        }
    }

/*
    // proyectil que puede explotar
    public override void Update(GameTime gameTime)
    {
        if (!IsActive) return;

        Vector2 previousPosition = Position;
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Actualizar bounds
        int size = Bounds.Width;
        Bounds = new Rectangle((int)Position.X - size/2, (int)Position.Y - size/2, size, size);

        float distanceThisFrame = Vector2.Distance(previousPosition, Position);
        CurrentDistance += distanceThisFrame;

        // Verificar si se superó la distancia máxima
        if (CurrentDistance >= MaxDistance)
        {
            if (ExplodesOnTimeout)
            {
                // Crear explosión al terminar el alcance
                var explosion = CreateExplosion();
                if (explosion != null)
                {
                    // Aquí se debería notificar al ExplosionManager
                }
            }
            IsActive = false;
        }
    }
*/

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