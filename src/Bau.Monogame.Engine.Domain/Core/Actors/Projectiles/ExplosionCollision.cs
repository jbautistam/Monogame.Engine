using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Projectiles;

public class ExplosionCollision
{
    public ExplosionActor Explosion { get; set; }
    public Rectangle TargetBounds { get; set; } // Para colisiones con rectángulos
    public Vector2 TargetPosition { get; set; } // Para colisiones con puntos
    public int Damage { get; set; }
    public Vector2 Force { get; set; }
    public bool IsPointCollision => TargetBounds == Rectangle.Empty;
    
    public ExplosionCollision()
    {
        TargetBounds = Rectangle.Empty;
        TargetPosition = Vector2.Zero;
        Force = Vector2.Zero;
    }
}
