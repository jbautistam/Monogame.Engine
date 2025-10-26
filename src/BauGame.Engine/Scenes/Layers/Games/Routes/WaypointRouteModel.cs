using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Games.Routes;

/// <summary>
///     Ruta de puntos
/// </summary>
public class WaypointRouteModel(string name)
{
    // Registros públicos
    public record Waypoint(Vector2 Position, float WaitTime, string? NextState);

    /// <summary>
    ///     Obtiene el siguiente <see cref="Waypoint"/>
    /// </summary>
    public Waypoint? GetNextWaypoint(Waypoint? actual, bool isLooping)
    {
        int nextIndex = -1;

            // Obtiene el siguiente punto
            if (Waypoints.Count > 0)
            {
                // Obtiene el índice del waypoint actual
                if (actual is null)
                    nextIndex = 0;
                else
                    nextIndex = Waypoints.IndexOf(actual) + 1;
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
    public float DistanceToWaypoint(Vector2 position, Waypoint waypoint) => Vector2.Distance(position, waypoint.Position);

    /// <summary>
    ///     Nombre de la ruta
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    ///     Puntos del recorrido
    /// </summary>
    public List<Waypoint> Waypoints { get; } = [];
}