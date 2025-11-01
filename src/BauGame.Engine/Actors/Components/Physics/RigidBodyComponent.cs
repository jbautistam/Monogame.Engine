using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Physics;

public class RigidbodyComponent(AbstractActor owner) : AbstractComponent(owner, false)
{
    // Variables privadas
    private Vector2 _forceAccumulator = Vector2.Zero;

	/// <summary>
	///		Inicia el componente
	/// </summary>
	public override void Start()
	{
		// ... no hace nada
	}

    /// <summary>
    ///     Actualiza las físicas
    /// </summary>
	public override void UpdatePhysics(Managers.GameContext gameContext)
	{
        if (!IsStatic && !IsKinematic)
        {
            float deltaTime = gameContext.DeltaTime;
        
                // Aplica la gravedad ajustada a píxels
                if (UseGravity)
                    Acceleration += new Vector2(0, 9.81f * 60);
                // Aplica las fuerzas acumuladas
                Acceleration += _forceAccumulator / Mass;
                _forceAccumulator = Vector2.Zero;
                // Aplica la resistencia
                Acceleration -= Velocity * Drag;
                // Integra el movimiento (Euler integration)
                Velocity += Acceleration * deltaTime;
                // Resetea la aceleración para el próximo frame
                Acceleration = Vector2.Zero;
                // Verifica si está sobre el suelo
                IsGrounded = CheckGrounded();
        }
	}

    /// <summary>
    ///     Actualiza el actor
    /// </summary>
    public override void Update(Managers.GameContext gameContext)
    {
        if (!IsStatic && !IsKinematic)
            Owner.Transform.Bounds += Velocity * gameContext.DeltaTime;
    }
    
    /// <summary>
    ///     Añade una fuerza al RigidBody
    /// </summary>
    public void AddForce(Vector2 force)
    {
        _forceAccumulator += force;
    }
    
    /// <summary>
    ///     Añade un impulso al RigidBody
    /// </summary>
    public void AddImpulse(Vector2 impulse)
    {
        Velocity += impulse / Mass;
    }
    
    /// <summary>
    ///     Detiene el movmiento
    /// </summary>
    public void Stop()
    {
        Velocity = Vector2.Zero;
        Acceleration = Vector2.Zero;
        _forceAccumulator = Vector2.Zero;
    }
    
    /// <summary>
    ///     Comprueba si está sobre el suelo
    /// </summary>
    private bool CheckGrounded()
    {
        CollisionComponent? collision = Owner.Components.GetComponent<CollisionComponent>();
        bool isGrounded = false;

            // Devuelve el valor que indica si está sobre el suelo
            if (collision is not null)
            {
            /*
                // Crear un pequeño rayo hacia abajo desde el centro del objeto
                Vector2 rayStart = collider.Center;
                Vector2 rayEnd = rayStart + new Vector2(0, collider.Size.Y / 2 + GroundCheckDistance);
        
                // Aquí iría la lógica de raycasting contra el suelo
                // Por simplicidad, asumimos que hay suelo en y = 400
                if (rayEnd.Y >= 400) // Altura del suelo
                {
                    isGrounded = true;
                    // Ajustar posición para que esté exactamente sobre el suelo
                    if (Owner.Position.Y + collider.Size.Y > 400)
                    {
                        Owner.Position = new Vector2(Owner.Position.X, 400 - collider.Size.Y);
                        Velocity = new Vector2(Velocity.X, 0); // Detener movimiento vertical
                    }
                }
                */
            }
            // Devuelve el valor que indica si está sobre un suelo
            return isGrounded;
    }

    /// <summary>
    ///     Dibuja el componente (en este caso no hace nada)
    /// </summary>
	public override void Draw(Camera2D camera, Managers.GameContext gameContext)
	{
	}

    /// <summary>
    ///     Termina la ejecución del componente (en este caso no hace nada
    /// </summary>
	public override void End()
	{
	}

    /// <summary>
    ///     Velocidad actual
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    ///     Aceleración actual
    /// </summary>
    public Vector2 Acceleration { get; set; }

    /// <summary>
    ///     Masa
    /// </summary>
    public float Mass { get; set; } = 1.0f;

    /// <summary>
    ///     Resistencia del aire
    /// </summary>
    public float Drag { get; set; } = 0.0f;

    /// <summary>
    ///     Indica si va a utilizar la gravedad
    /// </summary>
    public bool UseGravity { get; set; } = true;

    /// <summary>
    ///     Indica si se va a controlar directamente
    /// </summary>
    public bool IsKinematic { get; set; } = false;
    
    /// <summary>
    ///     Indica si es estático (sin movimiento)
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    ///     Elasticidad (0 = sin rebote, 1 = rebote completo)
    /// </summary>
    public float Bounciness { get; set; }

    /// <summary>
    ///     Fricción
    /// </summary>
    public float Friction { get; set; } = 0.1f;

    /// <summary>
    ///     Indica si está sobre el suelo
    /// </summary>
    public bool IsGrounded { get; set; }
}
