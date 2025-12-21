using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors.Projectiles;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Games.Pools;

/// <summary>
///     Managers para explosiones
/// </summary>
public class ExplosionsManager(AbstractLayer layer)
{
    // Variables privadas
    private Pool.ObjectPool<ExplosionActor> _explossions = new();

    /// <summary>
    ///     Crea una explosión
    /// </summary>
    public ExplosionActor Create(ExplosionProperties properties, Vector2 position)
    {
        ExplosionActor? explosion = _explossions.GetFirstInactive();
        
            // Crea la explosión si no se ha encontrado ninguna y la añade al pool
            if (explosion is null)
            {
                // Crea la explosión
                explosion = new ExplosionActor(Layer);
                // Añade la explosión al pool
                _explossions.Add(explosion);
            }
            // Configura la explosión
            explosion.Shoot(properties, position);
            // Devuelve la explosión que se acaba de crear
            return explosion;
    }

    /// <summary>
    ///     Actualiza las explosiones
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        foreach (ExplosionActor explosion in _explossions.Enumerate())
            explosion.Update(gameContext);
    }

    /// <summary>
    ///     Dibuja las explosiones
    /// </summary>
    public void Draw(Cameras.Camera2D camera, Managers.GameContext gameContext)
    {
        foreach (ExplosionActor explosion in _explossions.Enumerate())
            explosion.Draw(camera, gameContext);
    }

    /// <summary>
    ///     Verifica las colisiones con entidades
    /// </summary>
    public List<ExplosionCollision> CheckCollisions(IEnumerable<Rectangle> targets)
    {
        List<ExplosionCollision> collisions = [];
        
        foreach (var explosion in _explossions.Enumerate())
        {
            foreach (var target in targets)
            {
/*
                if (explosion.GetBounds().Intersects(target))
                {
                    // Calcular punto central del target para daño preciso
                    Vector2 targetCenter = new Vector2(
                        target.X + target.Width / 2f,
                        target.Y + target.Height / 2f
                    );
                    
                    if (explosion.IsInRange(targetCenter))
                    {
                        int damage = explosion.GetDamageAtPosition(targetCenter);
                        if (damage > 0)
                        {
                            collisions.Add(new ExplosionCollision
                            {
                                Explosion = explosion,
                                TargetBounds = target,
                                Damage = damage,
                                Force = explosion.GetForceAtPosition(targetCenter)
                            });
                        }
                    }
                }
*/
            }
        }
        
        return collisions;
    }

    /// <summary>
    ///     Verifica las colisiones con puntos específicos
    /// </summary>
    public List<ExplosionCollision> CheckCollisions(IEnumerable<Vector2> targetPositions)
    {
        List<ExplosionCollision> collisions = [];
       
/*
        foreach (var explosion in ActiveExplosions.Where(e => e.IsActive))
        {
            foreach (var position in targetPositions)
            {
                if (explosion.IsInRange(position))
                {
                    int damage = explosion.GetDamageAtPosition(position);
                    if (damage > 0)
                    {
                        collisions.Add(new ExplosionCollision
                        {
                            Explosion = explosion,
                            TargetPosition = position,
                            Damage = damage,
                            Force = explosion.GetForceAtPosition(position)
                        });
                    }
                }
            }
        }
*/
        return collisions;
    }

    /// <summary>
    ///     Limpia el pool
    /// </summary>
    public void Clear()
    {
        _explossions.Clear();
    }

    /// <summary>
    ///     Capa
    /// </summary>
    public AbstractLayer Layer { get; } = layer;
}
