using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Components.Transforms;

public class RigidbodyComponent(AbstractActor owner) : AbstractComponent(owner, false)
{
    // Propiedades físicas básicas
    public Vector2 Velocity { get; set; }           // Velocidad actual
    public Vector2 Acceleration { get; set; }       // Aceleración actual
    public float Mass { get; set; } = 1.0f;         // Masa del objeto
    public float Drag { get; set; } = 0.0f;         // Resistencia del aire
    public bool UseGravity { get; set; } = true;    // ¿Se ve afectado por gravedad?
    public bool IsKinematic { get; set; } = false;  // ¿Controlado por física o directamente?
    
    // Propiedades de colisión
    public bool IsStatic { get; set; } = false;     // ¿Objeto estático (no se mueve)?
    public float Bounciness { get; set; } = 0.0f;   // Elasticidad (0 = sin rebote, 1 = rebote completo)
    public float Friction { get; set; } = 0.1f;     // Fricción
    public bool IsGrounded { get; set; }
    
    // Fuerzas aplicadas
    private Vector2 forceAccumulator = Vector2.Zero;
    
    public override void Update(GameTime gameTime)
    {
        if (IsStatic || IsKinematic) return;
        
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Aplicar gravedad
        if (UseGravity)
        {
            Acceleration += new Vector2(0, 9.81f * 60); // Gravedad ajustada para pixeles
        }
        
        // Aplicar fuerzas acumuladas
        Acceleration += forceAccumulator / Mass;
        forceAccumulator = Vector2.Zero;
        
        // Aplicar drag (resistencia)
        Acceleration -= Velocity * Drag;
        
        // Integrar movimiento (Euler integration)
        Velocity += Acceleration * deltaTime;
        Owner.Position += Velocity * deltaTime;
        
        // Resetear aceleración para el próximo frame
        Acceleration = Vector2.Zero;
        // Verificar si está en el suelo
        CheckGrounded();

    }
    
    // Métodos para aplicar fuerzas
    public void AddForce(Vector2 force)
    {
        forceAccumulator += force;
    }
    
    public void AddImpulse(Vector2 impulse)
    {
        Velocity += impulse / Mass;
    }
    
    // Método para detener el movimiento
    public void Stop()
    {
        Velocity = Vector2.Zero;
        Acceleration = Vector2.Zero;
        forceAccumulator = Vector2.Zero;
    }
    
    private void CheckGrounded()
    {
        IsGrounded = false;
        
        var collider = Owner.Components.GetComponent<ColliderComponent>();
        if (collider == null) return;
        
        // Crear un pequeño rayo hacia abajo desde el centro del objeto
        Vector2 rayStart = collider.Center;
        Vector2 rayEnd = rayStart + new Vector2(0, collider.Size.Y / 2 + GroundCheckDistance);
        
        // Aquí iría la lógica de raycasting contra el suelo
        // Por simplicidad, asumimos que hay suelo en y = 400
        if (rayEnd.Y >= 400) // Altura del suelo
        {
            IsGrounded = true;
            // Ajustar posición para que esté exactamente sobre el suelo
            if (Owner.Position.Y + collider.Size.Y > 400)
            {
                Owner.Position = new Vector2(Owner.Position.X, 400 - collider.Size.Y);
                Velocity = new Vector2(Velocity.X, 0); // Detener movimiento vertical
            }
        }
    }
}
