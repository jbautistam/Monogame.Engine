using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class InverterNode : BehaviorNode
{
    private BehaviorNode child;

    public InverterNode(string name = "Inverter") : base(name) { }

    public void SetChild(BehaviorNode child)
    {
        this.child = child;
    }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        if (child == null) return BehaviorStatus.Failure;

        var childStatus = child.Execute(npc, gameTime);

        switch (childStatus)
        {
            case BehaviorStatus.Success:
                return BehaviorStatus.Failure;
            case BehaviorStatus.Failure:
                return BehaviorStatus.Success;
            default:
                return childStatus;
        }
    }

    public override void Reset()
    {
        base.Reset();
        child?.Reset();
    }
}