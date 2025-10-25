using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Agent
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float MaxSpeed = 100f;
    public float MaxForce = 10f;
    public float Orientation = 0f;

    private Texture2D sprite;
    private List<(ISteeringBehavior behavior, float weight)> behaviors;

    public Agent(Vector2 position, Texture2D sprite)
    {
        Position = position;
        Velocity = Vector2.Zero;
        Acceleration = Vector2.Zero;
        this.sprite = sprite;
        behaviors = new List<(ISteeringBehavior, float)>();
    }

    public void AddBehavior(ISteeringBehavior behavior, float weight)
    {
        behaviors.Add((behavior, weight));
    }

    public void Update(float deltaTime)
    {
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
        spriteBatch.Draw(sprite, Position, null, Color.Red, Orientation, new Vector2(0.5f), 1f, SpriteEffects.None, 0f);
    }
}
