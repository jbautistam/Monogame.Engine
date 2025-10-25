using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Agent1
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float MaxSpeed = 100f;
    public float MaxForce = 10f;

    private Texture2D sprite;
    private FSM fsm;

    public Agent1(Vector2 position, Texture2D sprite)
    {
        Position = position;
        Velocity = Vector2.Zero;
        Acceleration = Vector2.Zero;
        this.sprite = sprite;

        // Iniciar con un estado por defecto
        fsm = new FSM(new WanderState(this));
    }

    public void ChangeState(SteeringState state)
    {
        fsm.ChangeState(state);
    }

    public void Update(float deltaTime)
    {
        // Actualizar estado actual
        fsm.Update(deltaTime);

        // Obtener comportamientos y pesos del estado actual
        var behaviors = fsm.GetCurrentState().GetBehaviors();

        Vector2 force = Vector2.Zero;

        foreach (var (behavior, weight) in behaviors)
        {
            Vector2 calculatedForce = behavior.Calculate(this);
            force += calculatedForce * weight;
        }

        // Limitar fuerza
        if (force.Length() > MaxForce)
        {
            force = Vector2.Normalize(force) * MaxForce;
        }

        Acceleration = force;

        Velocity += Acceleration * deltaTime;
        if (Velocity.Length() > MaxSpeed)
        {
            Velocity = Vector2.Normalize(Velocity) * MaxSpeed;
        }

        Position += Velocity * deltaTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, Position, null, Color.Red, 0f, new Vector2(0.5f), 1f, SpriteEffects.None, 0f);
    }
}