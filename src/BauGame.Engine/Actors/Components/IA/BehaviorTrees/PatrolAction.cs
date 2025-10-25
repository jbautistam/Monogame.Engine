using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class PatrolAction : BehaviorNode
{
    private PatrolRoute patrolRoute;
    private Vector2 targetPosition;
    private bool initialized = false;

    public PatrolAction(PatrolRoute route, string name = "Patrol") : base(name)
    {
        patrolRoute = route;
    }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        if (patrolRoute == null || patrolRoute.Waypoints.Count == 0)
            return BehaviorStatus.Failure;

        if (!initialized)
        {
            var currentWaypoint = patrolRoute.GetCurrentWaypoint();
            if (currentWaypoint != null)
            {
                targetPosition = currentWaypoint.Position;
            }
            initialized = true;
        }

        float distanceToTarget = Vector2.Distance(npc.Position, targetPosition);

        if (distanceToTarget < npc.ArrivalDistance)
        {
            // Llegó al waypoint
            npc.Position = targetPosition;
            npc.Velocity = Vector2.Zero;

            // Ir al siguiente waypoint
            var nextWaypoint = patrolRoute.GetNextWaypoint();
            if (nextWaypoint != null)
            {
                targetPosition = nextWaypoint.Position;
                return BehaviorStatus.Running;
            }
            else
            {
                return BehaviorStatus.Success;
            }
        }
        else
        {
            // Moverse hacia el waypoint
            Vector2 direction = targetPosition - npc.Position;
            if (direction.Length() > 0)
            {
                direction.Normalize();
                npc.Velocity = direction * npc.Speed;
                npc.Position += npc.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            return BehaviorStatus.Running;
        }
    }

    public override void Reset()
    {
        base.Reset();
        initialized = false;
        patrolRoute?.Reset();
    }
}