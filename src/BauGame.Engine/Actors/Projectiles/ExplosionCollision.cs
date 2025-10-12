using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Projectiles;

public class ExplosionCollision(ExplosionActor owner)
{
    public ExplosionActor Explosion { get; } = owner;
    public Rectangle TargetBounds { get; set; } = Rectangle.Empty; // Para colisiones con rectángulos
    public Vector2 TargetPosition { get; set; } = Vector2.Zero; // Para colisiones con puntos
    public int Damage { get; set; } = 5;
    public Vector2 Force { get; set; } = Vector2.Zero;
    public bool IsPointCollision => TargetBounds == Rectangle.Empty;
}
