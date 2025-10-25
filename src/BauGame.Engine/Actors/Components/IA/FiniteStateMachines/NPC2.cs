using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public class NPC
{
    // ... propiedades existentes ...

    public BehaviorTree BehaviorTree { get; set; }

    public void Update(GameTime gameTime)
    {
        // Ejecutar el behavior tree
        BehaviorTree?.Update(gameTime);
        
        // También puedes mantener compatibilidad con la máquina de estados
        // StateMachine?.Update(gameTime);
    }

    // ... resto de métodos ...
}
```

## 7. **Ejemplo de árbol de comportamiento**
// Crear un árbol de comportamiento complejo
public static BehaviorTree CreateGuardBehaviorTree(NPC npc)
{
    /*
    Árbol de comportamiento para un guardia:
    
    Selector Principal
    ├── Secuencia: Interacción con jugador
    │   ├── Condición: ¿Jugador cerca?
    │   └── Acción: Hablar con jugador
    └── Secuencia: Comportamiento normal
        ├── Repetidor: Patrullar indefinidamente
        │   └── Acción: Patrullar
        └── Acción: Esperar
    */

    // Selector principal
    var mainSelector = new SelectorNode("Main Behavior");

    // Secuencia de interacción
    var interactionSequence = new SequenceNode("Player Interaction");
    interactionSequence.AddChild(new IsPlayerNearbyCondition(80f));
    interactionSequence.AddChild(new TalkToPlayerAction());

    // Secuencia de comportamiento normal
    var normalBehaviorSequence = new SequenceNode("Normal Behavior");
    
    // Crear ruta de patrulla
    var waypoints = new List<Waypoint>
    {
        new Waypoint(new Vector2(100, 100)),
        new Waypoint(new Vector2(200, 100)),
        new Waypoint(new Vector2(200, 200)),
        new Waypoint(new Vector2(100, 200))
    };
    var patrolRoute = new PatrolRoute(waypoints);
    
    // Repetidor de patrulla
    var patrolRepeater = new RepeaterNode(-1, "Infinite Patrol");
    var patrolAction = new PatrolAction(patrolRoute);
    patrolRepeater.SetChild(patrolAction);
    
    normalBehaviorSequence.AddChild(patrolRepeater);
    normalBehaviorSequence.AddChild(new WaitAction(2f)); // Esperar 2 segundos

    // Construir árbol
    mainSelector.AddChild(interactionSequence);
    mainSelector.AddChild(normalBehaviorSequence);

    return new BehaviorTree(mainSelector, npc);
}

// Uso:
var behaviorTree = CreateGuardBehaviorTree(npc);
npc.BehaviorTree = behaviorTree;
```

## 8. **Ejemplo avanzado: Árbol con comportamiento paralelo**

```csharp
public static BehaviorTree CreateAdvancedNPCBehavior(NPC npc)
{
    /*
    Árbol avanzado:
    
    Paralelo
    ├── Selector: Comportamiento principal
    │   ├── Secuencia: Emergencia (jugador muy cerca)
    │   │   ├── Condición: ¿Jugador muy cerca?
    │   │   └── Acción: Moverse hacia jugador
    │   └── Secuencia: Comportamiento normal
    │       ├── Repetidor: Patrullar
    │       └── Esperar
    └── Secuencia: Verificaciones constantes
        ├── Condición: ¿Bajo ataque?
        └── Acción: Huir
    */

    var parallel = new ParallelNode(ParallelNode.Policy.RequireOne, 
                                   ParallelNode.Policy.RequireOne, 
                                   "Main Parallel");

    // Comportamiento principal
    var mainSelector = new SelectorNode("Main Selector");
    
    // Emergencia: jugador muy cerca
    var emergencySequence = new SequenceNode("Emergency");
    emergencySequence.AddChild(new IsPlayerNearbyCondition(50f)); // Muy cerca
    emergencySequence.AddChild(new MoveToPlayerAction(150f)); // Moverse rápido

    // Comportamiento normal
    var normalSequence = new SequenceNode("Normal Behavior");
    var patrolRoute = new PatrolRoute(/* waypoints */);
    var patrolRepeater = new RepeaterNode(-1);
    patrolRepeater.SetChild(new PatrolAction(patrolRoute));
    normalSequence.AddChild(patrolRepeater);
    normalSequence.AddChild(new WaitAction(1f));

    mainSelector.AddChild(emergencySequence);
    mainSelector.AddChild(normalSequence);

    // Verificaciones constantes
    var constantChecks = new SequenceNode("Constant Checks");
    constantChecks.AddChild(new IsUnderAttackCondition());
    constantChecks.AddChild(new FleeAction());

    parallel.AddChild(mainSelector);
    parallel.AddChild(constantChecks);

    return new BehaviorTree(parallel, npc);
}
```