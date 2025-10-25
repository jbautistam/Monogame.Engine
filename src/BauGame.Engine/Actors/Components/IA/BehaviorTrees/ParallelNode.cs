using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class ParallelNode : BehaviorNode
{
    public enum Policy
    {
        RequireOne,  // Éxito/Fallo cuando uno lo consigue
        RequireAll   // Éxito/Fallo cuando todos lo consiguen
    }

    private List<BehaviorNode> children;
    private Policy successPolicy;
    private Policy failurePolicy;

    public ParallelNode(Policy successPolicy = Policy.RequireOne, 
                       Policy failurePolicy = Policy.RequireOne, 
                       string name = "Parallel") : base(name)
    {
        children = new List<BehaviorNode>();
        this.successPolicy = successPolicy;
        this.failurePolicy = failurePolicy;
    }

    public void AddChild(BehaviorNode child)
    {
        children.Add(child);
    }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        int successCount = 0;
        int failureCount = 0;

        foreach (var child in children)
        {
            // Solo ejecutar hijos que no han terminado
            if (child is not { } || child.status == BehaviorStatus.Running)
            {
                var childStatus = child.Execute(npc, gameTime);
                
                switch (childStatus)
                {
                    case BehaviorStatus.Success:
                        successCount++;
                        break;
                    case BehaviorStatus.Failure:
                        failureCount++;
                        break;
                }
            }
            else if (child.status == BehaviorStatus.Success)
            {
                successCount++;
            }
            else if (child.status == BehaviorStatus.Failure)
            {
                failureCount++;
            }
        }

        // Evaluar políticas
        if ((successPolicy == Policy.RequireOne && successCount > 0) ||
            (successPolicy == Policy.RequireAll && successCount == children.Count))
        {
            Reset();
            return BehaviorStatus.Success;
        }

        if ((failurePolicy == Policy.RequireOne && failureCount > 0) ||
            (failurePolicy == Policy.RequireAll && failureCount == children.Count))
        {
            Reset();
            return BehaviorStatus.Failure;
        }

        return BehaviorStatus.Running;
    }

    public override void Reset()
    {
        base.Reset();
        foreach (var child in children)
        {
            child.Reset();
        }
    }
}