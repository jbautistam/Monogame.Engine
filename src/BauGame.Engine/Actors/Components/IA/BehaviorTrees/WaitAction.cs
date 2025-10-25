using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class WaitAction : BehaviorNode
{
    private float duration;
    private float elapsed;

    public WaitAction(float seconds, string name = "Wait") : base(name)
    {
        duration = seconds;
        elapsed = 0f;
    }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        npc.Velocity = Vector2.Zero; // Detener movimiento

        if (elapsed >= duration)
        {
            Reset();
            return BehaviorStatus.Success;
        }

        return BehaviorStatus.Running;
    }

    public override void Reset()
    {
        base.Reset();
        elapsed = 0f;
    }
}