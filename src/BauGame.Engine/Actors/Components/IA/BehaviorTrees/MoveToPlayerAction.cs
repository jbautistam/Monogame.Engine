using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class MoveToPlayerAction : BehaviorNode
{
    private float moveSpeed;

    public MoveToPlayerAction(float speed = 100f, string name = "MoveToPlayer") : base(name)
    {
        moveSpeed = speed;
    }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        if (npc.Player == null)
            return BehaviorStatus.Failure;

        Vector2 direction = npc.Player.Position - npc.Position;
        float distance = direction.Length();

        if (distance < npc.ArrivalDistance)
        {
            npc.Velocity = Vector2.Zero;
            return BehaviorStatus.Success;
        }

        if (distance > 0)
        {
            direction.Normalize();
            npc.Velocity = direction * moveSpeed;
            npc.Position += npc.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            return BehaviorStatus.Running;
        }

        return BehaviorStatus.Failure;
    }

    public override void Reset()
    {
        base.Reset();
        // No hay estado que resetear en esta acción simple
    }
}