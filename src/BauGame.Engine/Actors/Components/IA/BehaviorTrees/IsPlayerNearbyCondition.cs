using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class IsPlayerNearbyCondition : BehaviorNode
{
    private float detectionRadius;

    public IsPlayerNearbyCondition(float radius = 100f, string name = "IsPlayerNearby") : base(name)
    {
        detectionRadius = radius;
    }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        if (npc.IsPlayerNearby(detectionRadius))
        {
            return BehaviorStatus.Success;
        }
        return BehaviorStatus.Failure;
    }
}