using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public abstract class BehaviorNode
{
    public enum BehaviorStatus
    {
        Running,    // El comportamiento está en ejecución
        Success,    // El comportamiento completó exitosamente
        Failure     // El comportamiento falló
    }
    public string Name { get; set; }
    protected BehaviorStatus status = BehaviorStatus.Failure;

    public BehaviorNode(string name = "")
    {
        Name = name;
    }

    public abstract BehaviorStatus Execute(NPC npc, GameTime gameTime);
    
    public virtual void Reset()
    {
        status = BehaviorStatus.Failure;
    }
}