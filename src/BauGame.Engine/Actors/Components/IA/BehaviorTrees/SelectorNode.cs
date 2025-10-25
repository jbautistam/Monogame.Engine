using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class SelectorNode : BehaviorNode
{
    private List<BehaviorNode> children;
    private int currentChildIndex;

    public SelectorNode(string name = "Selector") : base(name)
    {
        children = new List<BehaviorNode>();
        currentChildIndex = 0;
    }

    public void AddChild(BehaviorNode child)
    {
        children.Add(child);
    }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        // Ejecutar hijos secuencialmente hasta que uno tenga éxito
        while (currentChildIndex < children.Count)
        {
            var child = children[currentChildIndex];
            var childStatus = child.Execute(npc, gameTime);

            switch (childStatus)
            {
                case BehaviorStatus.Success:
                    Reset(); // Resetear para próxima ejecución
                    return BehaviorStatus.Success;
                    
                case BehaviorStatus.Running:
                    return BehaviorStatus.Running;
                    
                case BehaviorStatus.Failure:
                    currentChildIndex++;
                    break;
            }
        }

        // Todos los hijos fallaron
        Reset();
        return BehaviorStatus.Failure;
    }

    public override void Reset()
    {
        base.Reset();
        currentChildIndex = 0;
        foreach (var child in children)
        {
            child.Reset();
        }
    }
}