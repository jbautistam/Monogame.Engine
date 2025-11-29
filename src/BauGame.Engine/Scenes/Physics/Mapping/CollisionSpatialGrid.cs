using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Actors.Components.Physics;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping;

/// <summary>
///     Grid espacial
/// </summary>
public class CollisionSpatialGrid(MapManager mapManager, int cellSize)
{
    // Variables privadas
    private Dictionary<Point, List<AbstractActor>> _grid = [];
    
    /// <summary>
    ///     Obtiene una posición del grid a partir de una posición del mundo
    /// </summary>
    public Point GetGridPosition(Vector2 worldPosition)
    {
        if (CellSize != 0)
            return new Point((int) (worldPosition.X / CellSize), (int) (worldPosition.Y / CellSize));
        else
            return new Point(0, 0);
    }
    
    /// <summary>
    ///     Añade un actor al grid espacial
    /// </summary>
    public void Add(AbstractActor actor)
    {
        (Point minCell, Point maxCell) = GetGridPositions(actor.Transform);
        
            // Elimina el actor de su posición inicial
            if (actor.PreviuosTransform.IsValid)
                Remove(actor, actor.PreviuosTransform);
            // Crea los apuntes en la lista
            for (int x = minCell.X; x <= maxCell.X; x++)
                for (int y = minCell.Y; y <= maxCell.Y; y++)
                {
                    Point cellKey = new(x, y);

                        // Crea la lista si no existía
                        if (!_grid.ContainsKey(cellKey))
                            _grid[cellKey] = new List<AbstractActor>();
                        // Añade el actor a la posición
                        _grid[cellKey].Add(actor);
                }
    }
    
    /// <summary>
    ///     Obtiene las colisiones potenciales sobre un actor
    /// </summary>
    public List<AbstractCollider> GetPotentialColliders(AbstractActor actor)
    {
        List<AbstractCollider> result = [];

            // Si realmente tiene ancho y alto
            if (actor.Transform.IsValid) 
            {
                CollisionComponent? collision = actor.Components.GetComponent<CollisionComponent>();

                    // Busca los actores que están en las mimas posiciones
                    if (collision is not null)
                    {
                        (Point minCell, Point maxCell) = GetGridPositions(actor.Transform);
        
                            for (int x = minCell.X; x <= maxCell.X; x++)
                                for (int y = minCell.Y; y <= maxCell.Y; y++)
                                {
                                    Point cellKey = new(x, y);

                                        if (_grid.TryGetValue(cellKey, out List<AbstractActor>? others))
                                            foreach (AbstractActor other in others)
                                                if (other != actor)
                                                {
                                                    AbstractCollider? collider = GetCollisionCollider(actor, other);

                                                        // Si ha encontrado una colisión
                                                        if (collider is not null && collider.Enabled && !result.Contains(collider))
                                                            result.Add(collider);
                                                }
                                }
                    }
            }
            // Devuelve los actores con colisiones potenciales
            return result;
    }

    /// <summary>
    ///     Comprueba si están colisionando dos actores
    /// </summary>
    private AbstractCollider? GetCollisionCollider(AbstractActor actor, AbstractActor other)
    {
        CollisionComponent? actorCollision = actor.Components.GetComponent<CollisionComponent>();
        CollisionComponent? otherCollision = other.Components.GetComponent<CollisionComponent>();

            // Comprueba las colisiones
            if (actorCollision is not null && otherCollision is not null && actorCollision.Enabled && otherCollision.Enabled)
                if (MapManager.PhysicsManager.LayersRelations.IsColliding(actorCollision.PhysicLayerId, otherCollision.PhysicLayerId))
                    foreach (AbstractCollider actorCollider in actorCollision.Colliders)
                        if (actorCollider.Enabled)
                            foreach (AbstractCollider otherCollider in otherCollision.Colliders)
                                if (otherCollider.Enabled && actorCollider.IsColliding(otherCollider))
                                    return otherCollider;
            // Si ha llegado hasta aquí es porque no había ninguna colisión
            return null;
    }

    /// <summary>
    ///     Obtiene las celdas mínima y máxima de un rectángulo
    /// </summary>
    private (Point minCell, Point maxCell) GetGridPositions(Actors.Components.Transforms.TransformComponent transform)
    {
        Point minCell = GetGridPosition(new Vector2(transform.Bounds.X, transform.Bounds.Y));
        Point maxCell = GetGridPosition(new Vector2(transform.Bounds.Right, transform.Bounds.Bottom));
        bool mustSwap = false;

            // Comprueba si se tienen que cambiar los puntos
            if (maxCell.X < minCell.X)
                mustSwap = true;
            else if (maxCell.X == minCell.X && maxCell.Y < minCell.Y)
                mustSwap = true;
            // Devuelve los valores
            if (mustSwap)
                return (maxCell, minCell);
            else
                return (minCell, maxCell);
    }

    /// <summary>
    ///     Elimina un actor
    /// </summary>
    public void Remove(AbstractActor actor)
    {
        Remove(actor, actor.PreviuosTransform);
        Remove(actor, actor.Transform);
    }

    /// <summary>
    ///     Elimina un actor en una posición
    /// </summary>
    private void Remove(AbstractActor actor, Actors.Components.Transforms.TransformComponent transform)
    {
        (Point minCell, Point maxCell) = GetGridPositions(actor.Transform);
        
            for (int x = minCell.X; x <= maxCell.X; x++)
                for (int y = minCell.Y; y <= maxCell.Y; y++)
                {
                    Point cellKey = new(x, y);

                        if (_grid.TryGetValue(cellKey, out List<AbstractActor>? others) && others.Count > 0)
                            others?.Remove(actor);
                }
    }
    
    /// <summary>
    ///     Limpia el grid
    /// </summary>
    public void Clear()
    {
        _grid.Clear();
    }

    /// <summary>
    ///     Manager del mapa
    /// </summary>
    public MapManager MapManager { get; } = mapManager;

    /// <summary>
    ///     Tamaño de la celda
    /// </summary>
    public int CellSize { get; } = cellSize;
}
