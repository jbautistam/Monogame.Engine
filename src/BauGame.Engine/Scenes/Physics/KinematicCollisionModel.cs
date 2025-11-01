using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics;

public class KinematicCollisionModel
{
    public Vector2 Position { get; internal set; }
    public Vector2 Normal { get; internal set; }
    public Vector2 Remainder { get; internal set; }
    public float Travel { get; internal set; }
    public float Penetration;
    public object? Collider { get; internal set; }
}