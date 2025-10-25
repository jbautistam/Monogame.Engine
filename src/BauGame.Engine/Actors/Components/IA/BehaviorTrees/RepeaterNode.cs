using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class RepeaterNode : BehaviorNode
{
    private BehaviorNode child;
    private int maxRepeats;
    private int currentRepeats;

    public RepeaterNode(int maxRepeats = -1, string name = "Repeater") : base(name)
    {
        this.maxRepeats = maxRepeats; // -1 para infinito
        currentRepeats = 0;
    }

    public void SetChild(BehaviorNode child)
    {
        this.child = child;
    }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        if (child == null) return BehaviorStatus.Failure;

        var childStatus = child.Execute(npc, gameTime);

        if (childStatus == BehaviorStatus.Success || childStatus == BehaviorStatus.Failure)
        {
            currentRepeats++;
            child.Reset();

            if (maxRepeats == -1 || currentRepeats < maxRepeats)
            {
                return BehaviorStatus.Running;
            }
            else
            {
                Reset();
                return BehaviorStatus.Success;
            }
        }

        return BehaviorStatus.Running;
    }

    public override void Reset()
    {
        base.Reset();
        currentRepeats = 0;
        child?.Reset();
    }
}