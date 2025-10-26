using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors.Projectiles;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Games.Pools;

/// <summary>
///     Manager de proyectiles
/// </summary>
public class ProjectileManager(AbstractLayer layer)
{
    // Variables privadas
    private Pool.ObjectPool<ProjectileActor> _projectiles = new();

    /// <summary>
    ///     Crea un proyectil
    /// </summary>
    public ProjectileActor Create(ProjectileProperties properties, Vector2 position, Vector2 direction, float rotation, int physicsLayer)
    {
        ProjectileActor? projectile = _projectiles.GetFirstInactive();
        
            // Crea el proyectil si no se ha encontrado ninguno y lo añade al pool
            if (projectile is null)
            {
                // Crea el proyectil
                projectile = new ProjectileActor(Layer, properties.ZOrder, physicsLayer);
                // Añade el proyectil al pool
                _projectiles.Add(projectile);
            }
            // Configura el proyectil
            projectile.Shoot(properties, position, direction * properties.Speed, rotation, physicsLayer);
            // Devuelve el proyectil que se acaba de crear
            return projectile;
    }

    /// <summary>
    ///     Actualiza las físicas de los proyectiles
    /// </summary>
    public void UpdatePhysics(Managers.GameContext gameContext)
    {
        foreach (ProjectileActor projectile in _projectiles.Enumerate())
            projectile.UpdatePhysics(gameContext);
    }

    /// <summary>
    ///     Actualiza los proyectiles
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        foreach (ProjectileActor projectile in _projectiles.Enumerate())
            projectile.Update(gameContext);
    }

    /// <summary>
    ///     Dibuja los proyectiles
    /// </summary>
    public void Draw(Cameras.Camera2D camera, Managers.GameContext gameContext)
    {
        foreach (ProjectileActor projectile in _projectiles.Enumerate())
            projectile.Draw(camera, gameContext);
    }

    /// <summary>
    ///     Limpia los proyectiles
    /// </summary>
    public void Clear()
    {
        _projectiles.Clear();
    }

    /// <summary>
    ///     Capa
    /// </summary>
    public AbstractLayer Layer { get; } = layer;
}