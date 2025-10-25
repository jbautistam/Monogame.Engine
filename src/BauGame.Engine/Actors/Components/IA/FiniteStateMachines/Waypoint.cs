using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public class Waypoint
{
    public Vector2 Position { get; set; }
    public float WaitTime { get; set; } // Tiempo de espera en este punto
    public string Animation { get; set; } // Animación opcional para este waypoint

    public Waypoint(Vector2 position, float waitTime = 0f, string animation = "walk")
    {
        Position = position;
        WaitTime = waitTime;
        Animation = animation;
    }
}
