using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Games.Routes;

/// <summary>
///     Ruta de puntos
/// </summary>
public class WaypointRouteModel(string name)
{
    /// <summary>
    ///     Obtiene el siguiente <see cref="Waypoint"/>
    /// </summary>
    public Vector2? GetNextWaypoint(Vector2? actual, bool isLooping)
    {
        int nextIndex = -1;

            // Obtiene el siguiente punto
            if (Waypoints.Count > 0)
            {
                // Obtiene el índice del waypoint actual
                if (actual is null)
                    nextIndex = 0;
                else
                    nextIndex = Waypoints.IndexOf(actual ?? Vector2.Zero) + 1;
                // Obtiene el siguiente índice
                if (nextIndex >= Waypoints.Count && isLooping)
                    nextIndex = 0;
            }
            // Devuelve el siguiente punto
            if (nextIndex >= 0 && nextIndex < Waypoints.Count)
                return Waypoints[nextIndex];
            else
                return null;
    }

    /// <summary>
    ///     Calcula la distancia a un punto
    /// </summary>
    public float DistanceToWaypoint(Vector2 position, Vector2 waypoint) => Vector2.Distance(position, waypoint);

    /// <summary>
    ///     Nombre de la ruta
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     Puntos del recorrido
    /// </summary>
    public List<Vector2> Waypoints { get; } = [];
}