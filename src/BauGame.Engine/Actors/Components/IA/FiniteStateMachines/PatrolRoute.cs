using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public class PatrolRoute
{
    public List<Waypoint> Waypoints { get; private set; }
    public bool IsLooping { get; set; } = true;
    public int CurrentWaypointIndex { get; private set; } = 0;

    public PatrolRoute()
    {
        Waypoints = new List<Waypoint>();
    }

    public PatrolRoute(List<Waypoint> waypoints, bool isLooping = true)
    {
        Waypoints = waypoints;
        IsLooping = isLooping;
    }

    public Waypoint GetCurrentWaypoint()
    {
        if (Waypoints.Count == 0) return null;
        return Waypoints[CurrentWaypointIndex];
    }

    public Waypoint GetNextWaypoint()
    {
        if (Waypoints.Count == 0) return null;

        CurrentWaypointIndex++;
        
        if (CurrentWaypointIndex >= Waypoints.Count)
        {
            if (IsLooping)
                CurrentWaypointIndex = 0;
            else
                CurrentWaypointIndex = Waypoints.Count - 1;
        }

        return Waypoints[CurrentWaypointIndex];
    }

    public void Reset()
    {
        CurrentWaypointIndex = 0;
    }

    public void AddWaypoint(Waypoint waypoint)
    {
        Waypoints.Add(waypoint);
    }

    public float DistanceToCurrentWaypoint(Vector2 npcPosition)
    {
        var current = GetCurrentWaypoint();
        if (current == null) return float.MaxValue;
        return Vector2.Distance(npcPosition, current.Position);
    }
}