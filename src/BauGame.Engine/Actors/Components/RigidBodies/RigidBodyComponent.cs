using Bau.Libraries.BauGame.Engine.Scenes.Physics;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.RigidBodies;

/// <summary>
///     Componente para manejo de un rigidbody
/// </summary>
public class RigidBodyComponent(AbstractActor owner, RigidBodyComponent.BodyType type) : AbstractComponent(owner, false)
{
    /// <summary>
    ///     Tipo de movimiento asociado al componente
    /// </summary>
    public enum BodyType
    {
        /// <summary>Sin movimiento, no responde a las fíasicas</summary>
        Static,
        /// <summary>El movimiento lo controla el código. Ignora las fuerzas</summary>
        Kinematic,
        /// <summary>El movimiento lo controlan las fuerzas</summary>
        Dynamic
    }
    // Variables privadas
    private Vector2 _accumulatedForce = Vector2.Zero;

	/// <summary>
	///		Inicia el componente
	/// </summary>
	public override void Start()
	{
		// ... no hace nada
	}

	/// <summary>
	///		Actualiza los datos del componente para las físicas (si es necesario)
    ///		Integra las fuerzas definidas sobre el cuerpo
	/// </summary>
	public override void UpdatePhysics(Managers.GameContext gameContext)
    {
        // Sólo en los cuerpos que no sean estáticos
        if (Type == BodyType.Dynamic)
        {
            // Aplicar gravedad
            Vector2 totalForce = _accumulatedForce + Owner.Layer.Scene.PhysicsManager.WorldGravity * GravityScale * Mass;
            Vector2 acceleration = totalForce * InverseMass; // F = ma → a = F / m

                // Actualiza la velocidad con la aceleración
                Velocity += acceleration * gameContext.DeltaTime;
                // Aplica la fricción del aire
                Velocity *= 1f - LinearDamping * gameContext.DeltaTime;
        }
        // Vácía fuerzas acumuladas
        _accumulatedForce = Vector2.Zero;
    }

	/// <summary>
	///		Actualiza los datos del componente
	/// </summary>
	public override void Update(Managers.GameContext gameContext)
    {
        if (Type == BodyType.Dynamic)
            Owner.Transform.Bounds.Translate(Velocity * gameContext.DeltaTime);
    }

	/// <summary>
	///		Dibuja el componente
	/// </summary>
	public override void Draw(Scenes.Cameras.Camera2D camera, Managers.GameContext gameContext)
    {
        // ... no hace nada, sólo implementa la interface
    }

	/// <summary>
	///		Finaliza el trabajo con el componente
	/// </summary>
	public override void End()
    {
    }

    /// <summary>
    ///     Añade una fuerza al cuerpo (independientemente de la masa)
    /// </summary>
    public void AddForce(Vector2 force)
    {
        _accumulatedForce += force;
    }

    /// <summary>
    ///     Añade un impulso al cuerpo (depende de la masa)
    /// </summary>
    public void AddImpulse(Vector2 impulse)
    {
        if (Type != BodyType.Static)
            Velocity += impulse * InverseMass;
    }

    /// <summary>
    ///     Mueve el cuerpo deslizándolo y comprobando las colisiones
    /// </summary>
    public void MoveAndSlide(Managers.GameContext gameContext, Vector2 velocity, int slides = 4)
    {
        Vector2 motion = velocity * gameContext.DeltaTime;
        int slideCount = 0;

            // Inicializa las propiedades
            IsOnFloor = IsOnWall = IsOnCeiling = false;
            // Recorre la línea de movimiento por partes
            while (slideCount < slides && motion.LengthSquared() > 0.01f)
            {
                List<KinematicCollisionModel> contacts = Owner.Raycast(motion, 100, true);

                    if (contacts.Count > 0)
                    {
                        KinematicCollisionModel contact = contacts[0];
                        float dot = Vector2.Dot(contact.Normal, -Vector2.UnitY);
                        float projection = Vector2.Dot(motion, contact.Normal);

                            // Atendiendo al producto vectorial comprueba si el cuerpo está en contacto con el suelo, las paredes o el techo
                            if (dot > 0.7f) 
                                IsOnFloor = true;
                            else if (dot < -0.7f) 
                                IsOnCeiling = true;
                            else 
                                IsOnWall = true;
                            // Cancela el componente normal del movimiento si va hacia la superficie
                            if (projection < 0)
                                motion -= contact.Normal * projection;
                            // Mueve hasta la superficie añadiéndole un pequeño desplazamiento para que no quede pegado
                            Owner.Transform.Bounds.Translate(contact.Normal * (contact.Penetration - 0.01f));
                            // Pasa a la siguiente sección
                            slideCount++;
                    }
                    else
                    {
                        Owner.Transform.Bounds.Translate(motion);
                        motion = Vector2.Zero;
                    }
            }

    }

    /// <summary>
    ///     Mueve el cuerpo una cantidad dada y se detiene en la primera colisión
    /// </summary>
    public KinematicCollisionModel? MoveAndCollide(Vector2 motion)
    {
        KinematicCollisionModel? collision = null;
        List<KinematicCollisionModel> contacts = Owner.Raycast(motion, 100, true);

            // Intenta mover el cuerpo una cantidad de movimiento, si encuentra una colisión, // mueve el cuerpo al punto de contacto
            if (contacts.Count > 0)
            {
                KinematicCollisionModel contact = contacts[0];
                Vector2 safeMotion = contact.Normal * (contact.Penetration - 0.01f);

                    // Mueve el cuerpo a la posición segura
                    Owner.Transform.Bounds.Translate(safeMotion);
                    // Crea la colisión de salida
                    collision = new KinematicCollisionModel
                                        {
                                            Position = Owner.Transform.Bounds.TopLeft,
                                            Normal = contact.Normal,
                                            Travel = safeMotion.Length(),
                                            Remainder = motion - safeMotion,
                                            Collider = contact.Collider
                                        };
            }
            else
                Owner.Transform.Bounds.Translate(motion);
            // Devuelve la colisión
            return collision;
    }

    /// <summary>
    ///     Tipo de cuerpo
    /// </summary>
    public BodyType Type { get; } = type;

    // --- Propiedades físicas ---
    public Vector2 Velocity { get; set; }
    public float Mass { get; set; } = 1f;
    public float InverseMass => Mass <= 0 ? 0f : 1f / Mass;

    // --- Parámetros de simulación ---
    public Vector2 GravityScale { get; set; } = Vector2.One;
    public float LinearDamping { get; set; } = 0.1f; // Amortiguación del aire
    public float Friction { get; set; } = 0.2f;
    public float Bounce { get; set; } = 0f; // Coeficiente de restitución

    /// <summary>
    ///     Indica si el cuerpo está sobre el suelo
    /// </summary>
    public bool IsOnFloor { get; private set; }

    /// <summary>
    ///     Indica si el cuerpo está sobre una pared
    /// </summary>
    public bool IsOnWall { get; private set; }

    /// <summary>
    ///     Indica si el cuerpo está tocando el techo
    /// </summary>
    public bool IsOnCeiling { get; private set; }
}

/*

private void ResolveCollision(Rigidbody2D a, Rigidbody2D b, Contact contact)
    {
        // Solo los cuerpos dinámicos responden físicamente
        if (a.Type != BodyType.Dynamic && b.Type != BodyType.Dynamic)
            return;

        // Separar penetración
        Vector2 correction = contact.Normal * contact.Penetration * 0.5f;
        if (a.Type == BodyType.Dynamic) a.Position -= correction;
        if (b.Type == BodyType.Dynamic) b.Position += correction;

        // Resolver velocidad (impulso)
        Vector2 relVel = b.Velocity - a.Velocity;
        float velAlongNormal = Vector2.Dot(relVel, contact.Normal);

        if (velAlongNormal > 0) return; // Ya se están separando

        float restitution = Math.Min(a.Bounce, b.Bounce);
        float impulseScalar = -(1 + restitution) * velAlongNormal;
        impulseScalar /= a.InverseMass + b.InverseMass;

        Vector2 impulse = impulseScalar * contact.Normal;

        if (a.Type == BodyType.Dynamic) a.Velocity -= impulse * a.InverseMass;
        if (b.Type == BodyType.Dynamic) b.Velocity += impulse * b.InverseMass;

        // Opcional: fricción tangencial (más avanzado)
    }

void ResolveCollision(Rigidbody2D a, Rigidbody2D b, Vector2 normal, float depth)
{
    // Separar los cuerpos
    Vector2 correction = normal * depth * 0.5f;
    a.Position -= correction;
    b.Position += correction;

    // Calcular velocidad relativa
    Vector2 relativeVel = b.Velocity - a.Velocity;
    float velAlongNormal = Vector2.Dot(relativeVel, normal);

    // No colisionar si se están separando
    if (velAlongNormal > 0) return;

    // Coeficiente de restitución
    float e = Math.Min(a.Bounce, b.Bounce);

    // Impulso escalar
    float j = -(1 + e) * velAlongNormal;
    j /= a.InverseMass + b.InverseMass;

    Vector2 impulse = j * normal;

    // Aplicar impulso
    a.Velocity -= impulse * a.InverseMass;
    b.Velocity += impulse * b.InverseMass;
}

// Horizontal con fuerza, pero con "arrastre" para evitar aceleración infinita
float maxSpeed = 200f;
float acceleration = 800f;
float groundDrag = 8f; // fricción en tierra

Vector2 force = Vector2.Zero;

if (keyState.IsKeyDown(Keys.A)) force.X = -acceleration;
else if (keyState.IsKeyDown(Keys.D)) force.X = acceleration;
else
{
    // Aplicar "drag" para frenar
    Rigidbody.AddForce(-Rigidbody.Velocity.X * groundDrag * Vector2.UnitX);
}

// Limitar velocidad horizontal
if (Math.Abs(Rigidbody.Velocity.X) > maxSpeed)
{
    Rigidbody.Velocity = new Vector2(
        Math.Sign(Rigidbody.Velocity.X) * maxSpeed,
        Rigidbody.Velocity.Y
    );
}

Rigidbody.AddForce(force);

*/