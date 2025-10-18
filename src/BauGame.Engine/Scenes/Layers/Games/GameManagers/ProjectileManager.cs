using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors.Projectiles;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Games.GameManagers;

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
    public ProjectileActor Create(ProjectileProperties properties, Vector2 position, float rotation)
    {
        ProjectileActor? projectile = _projectiles.GetFirstInactive();
        
            // Crea el proyectil si no se ha encontrado ninguno y lo añade al pool
            if (projectile is null)
            {
                // Crea el proyectil
                projectile = new ProjectileActor(Layer, properties.ZOrder);
                // Añade el proyectil al pool
                _projectiles.Add(projectile);
            }
            // Configura el proyectil
            projectile.Shoot(properties, position, rotation);
            // Devuelve el proyectil que se acaba de crear
            return projectile;
    }

    /// <summary>
    ///     Actualiza los proyectiles
    /// </summary>
    public void Update(GameTime gameTime)
    {
        foreach (ProjectileActor projectile in _projectiles.Enumerate())
            projectile.Update(gameTime);
    }

    /// <summary>
    ///     Dibuja los proyectiles
    /// </summary>
    public void Draw(Cameras.Camera2D camera, GameTime gameTime)
    {
        foreach (ProjectileActor projectile in _projectiles.Enumerate())
            projectile.Draw(camera, gameTime);
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