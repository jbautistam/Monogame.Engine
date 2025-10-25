using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

	public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<Agent> agents;
        private List<Obstacle> obstacles;
        private Texture2D pixel;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            agents = new List<Agent>();
            obstacles = new List<Obstacle>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            // Crear obstáculos
            obstacles.Add(new Obstacle(new Vector2(400, 300), 50f));

            // Crear un agente que persigue al mouse
            var pursuer = new Agent(new Vector2(100, 100), pixel);
            pursuer.AddBehavior(new Pursue(() => new Vector2(Mouse.GetState().X, Mouse.GetState().Y)), 1.0f);
            agents.Add(pursuer);

            // Crear un grupo de agentes que se mueven como un flock
            for (int i = 0; i < 5; i++)
            {
                var agent = new Agent(new Vector2(200 + i * 20, 200 + i * 20), pixel);
                agent.AddBehavior(new Flock(agents, agent), 1.0f); // FlockBehaviour maneja Separation, Alignment, Cohesion
                agent.AddBehavior(new ObstacleAvoidance(obstacles), 1.5f);
                agents.Add(agent);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var agent in agents)
            {
                agent.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (var obstacle in obstacles)
            {
                obstacle.Draw(_spriteBatch, pixel);
            }

            foreach (var agent in agents)
            {
                agent.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
```

---

## 🧱 Clase `Obstacle.cs`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Obstacle
{
    public Vector2 Position;
    public float Radius;

    public Obstacle(Vector2 position, float radius)
    {
        Position = position;
        Radius = radius;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        // Dibuja un círculo simple (muy básico, puedes mejorar con geometría o un sprite)
        int segments = 32;
        for (int i = 0; i < segments; i++)
        {
            float angle = (float)(i * (2 * Math.PI / segments));
            float nextAngle = (float)((i + 1) * (2 * Math.PI / segments));

            Vector2 p1 = Position + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Radius;
            Vector2 p2 = Position + new Vector2((float)Math.Cos(nextAngle), (float)Math.Sin(nextAngle)) * Radius;

            // Dibuja una línea entre p1 y p2
            // (MonoGame no tiene primitivas directas, así que dibujamos una imagen estirada)
            float length = Vector2.Distance(p1, p2);
            float rotation = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);

            spriteBatch.Draw(pixel, p1, null, Color.Gray, rotation, Vector2.Zero, new Vector2(length, 2f), SpriteEffects.None, 0f);
        }
    }
}
```

---

## 🎯 Comportamientos nuevos

### Separation

```csharp
public class Separation : ISteeringBehavior
{
    private List<Agent> agents;
    private Agent self;
    private float separationDistance = 50f;

    public Separation(List<Agent> agents, Agent self, float separationDistance = 50f)
    {
        this.agents = agents;
        this.self = self;
        this.separationDistance = separationDistance;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 steering = Vector2.Zero;
        int count = 0;

        foreach (var other in agents)
        {
            if (other == self) continue;

            float distance = Vector2.Distance(agent.Position, other.Position);
            if (distance > 0 && distance < separationDistance)
            {
                Vector2 diff = agent.Position - other.Position;
                diff.Normalize();
                diff /= distance; // Fuerza inversa a la distancia
                steering += diff;
                count++;
            }
        }

        if (count > 0)
        {
            steering /= count;
            if (steering.Length() > 0)
            {
                steering = Vector2.Normalize(steering) * agent.MaxSpeed;
                steering -= agent.Velocity;
                steering = Vector2.Clamp(steering, -agent.MaxForce, agent.MaxForce);
            }
        }

        return steering;
    }
}
```

### Alignment

```csharp
public class Alignment : ISteeringBehavior
{
    private List<Agent> agents;
    private Agent self;
    private float neighborRadius = 50f;

    public Alignment(List<Agent> agents, Agent self, float neighborRadius = 50f)
    {
        this.agents = agents;
        this.self = self;
        this.neighborRadius = neighborRadius;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 steering = Vector2.Zero;
        int count = 0;

        foreach (var other in agents)
        {
            if (other == self) continue;

            float distance = Vector2.Distance(agent.Position, other.Position);
            if (distance > 0 && distance < neighborRadius)
            {
                steering += other.Velocity;
                count++;
            }
        }

        if (count > 0)
        {
            steering /= count;
            if (steering.Length() > 0)
            {
                steering = Vector2.Normalize(steering) * agent.MaxSpeed;
                steering -= agent.Velocity;
                steering = Vector2.Clamp(steering, -agent.MaxForce, agent.MaxForce);
            }
        }

        return steering;
    }
}
```

### Cohesion

```csharp
public class Cohesion : ISteeringBehavior
{
    private List<Agent> agents;
    private Agent self;
    private float neighborRadius = 50f;

    public Cohesion(List<Agent> agents, Agent self, float neighborRadius = 50f)
    {
        this.agents = agents;
        this.self = self;
        this.neighborRadius = neighborRadius;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 steering = Vector2.Zero;
        int count = 0;

        foreach (var other in agents)
        {
            if (other == self) continue;

            float distance = Vector2.Distance(agent.Position, other.Position);
            if (distance > 0 && distance < neighborRadius)
            {
                steering += other.Position;
                count++;
            }
        }

        if (count > 0)
        {
            steering /= count; // Centro de masa
            return new Seek(() => steering).Calculate(agent); // Usamos Seek para ir hacia el centro
        }

        return Vector2.Zero;
    }
}
```

### Flock (Combinación de Separation, Alignment, Cohesion)

```csharp
public class Flock : ISteeringBehavior
{
    private List<Agent> agents;
    private Agent self;

    public Flock(List<Agent> agents, Agent self)
    {
        this.agents = agents;
        this.self = self;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 separation = new Separation(agents, self).Calculate(agent) * 1.5f;
        Vector2 alignment = new Alignment(agents, self).Calculate(agent) * 1.0f;
        Vector2 cohesion = new Cohesion(agents, self).Calculate(agent) * 1.0f;

        return separation + alignment + cohesion;
    }
}
```

### Obstacle Avoidance

```csharp
public class ObstacleAvoidance : ISteeringBehavior
{
    private List<Obstacle> obstacles;
    private float avoidDistance = 100f;

    public ObstacleAvoidance(List<Obstacle> obstacles, float avoidDistance = 100f)
    {
        this.obstacles = obstacles;
        this.avoidDistance = avoidDistance;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 force = Vector2.Zero;

        foreach (var obstacle in obstacles)
        {
            float distance = Vector2.Distance(agent.Position, obstacle.Position);
            if (distance < avoidDistance + obstacle.Radius)
            {
                Vector2 direction = agent.Position - obstacle.Position;
                direction.Normalize();
                direction *= (avoidDistance + obstacle.Radius - distance); // Cuanto más cerca, más fuerte
                force += direction;
            }
        }

        return force;
    }
}
```

### Pursue (perseguir un objetivo móvil)

```csharp
public class Pursue : ISteeringBehavior
{
    private System.Func<Vector2> targetFunc;

    public Pursue(System.Func<Vector2> targetFunc)
    {
        this.targetFunc = targetFunc;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 targetPos = targetFunc();
        Vector2 toTarget = targetPos - agent.Position;
        float distance = toTarget.Length();
        float lookAheadTime = distance / agent.MaxSpeed;

        // Predecir la posición futura del objetivo
        Vector2 predictedPos = targetPos + agent.Velocity * lookAheadTime;

        return new Seek(() => predictedPos).Calculate(agent);
    }
}
```

### Evade (evadir un objetivo móvil)

```csharp
public class Evade : ISteeringBehavior
{
    private System.Func<Vector2> targetFunc;

    public Evade(System.Func<Vector2> targetFunc)
    {
        this.targetFunc = targetFunc;
    }

    public Vector2 Calculate(Agent agent)
    {
        Vector2 targetPos = targetFunc();
        Vector2 toTarget = targetPos - agent.Position;
        float distance = toTarget.Length();
        float lookAheadTime = distance / agent.MaxSpeed;

        Vector2 predictedPos = targetPos + agent.Velocity * lookAheadTime;

        return new Flee(() => predictedPos).Calculate(agent);
    }
}
```

---

## ✅ Resultado

- Ahora tienes agentes que:
  - Se mueven como un **flock** (separación, alineación, cohesión).
  - **Evitan obstáculos**.
  - **Persiguen** al cursor del mouse.
- Puedes combinar más comportamientos, ajustar pesos, y crear movimientos complejos.

---

¿Quieres que ahora añadamos una interfaz para cambiar comportamientos en tiempo real con teclas o mouse?

### USER
Cómo se pueden modificar los pesos de los tipos de movimientos dinámicamente simulando inteligencia y combinando con un sistema de estados finitos 

### ASSISTANT
¡Perfecto! Combinar **pesos dinámicos** de los comportamientos con un **sistema de estados finitos (FSM)** es una forma poderosa de simular **inteligencia** en el movimiento de los agentes. Por ejemplo:

- Un agente puede estar en estado `Wander` (explorando) con un peso alto en `Wander` y `Seek`.
- Si detecta un enemigo, cambia al estado `Flee`, aumentando el peso de `Flee` y reduciendo otros.
- Si está hambriento, puede ir al estado `SeekFood`, aumentando el peso de `Seek`.

---

## 🧠 Arquitectura propuesta

### 1. `SteeringState`: Clase base para estados de movimiento
Define los pesos de los comportamientos y lógica de transición.

### 2. `FSM`: Máquina de estados que gestiona el estado actual y transiciones.

### 3. `Agent`: Ahora tiene un `FSM` y puede cambiar comportamientos según el estado.

---

## 🧩 Paso 1: Clase base `SteeringState`

```csharp
public abstract class SteeringState
{
    protected Agent agent;
    protected Dictionary<ISteeringBehavior, float> behaviorWeights;

    public SteeringState(Agent agent)
    {
        this.agent = agent;
        behaviorWeights = new Dictionary<ISteeringBehavior, float>();
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public abstract void Update(float deltaTime);

    public Dictionary<ISteeringBehavior, float> GetBehaviors() => behaviorWeights;

    protected void SetBehaviorWeight(ISteeringBehavior behavior, float weight)
    {
        if (behaviorWeights.ContainsKey(behavior))
        {
            behaviorWeights[behavior] = weight;
        }
        else
        {
            behaviorWeights.Add(behavior, weight);
        }
    }
}
```

---

## 🧩 Paso 2: Estados concretos

### Estado `WanderState`

```csharp
public class WanderState : SteeringState
{
    public WanderState(Agent agent) : base(agent) { }

    public override void Enter()
    {
        SetBehaviorWeight(new Wander(), 1.0f);
        SetBehaviorWeight(new Seek(() => new Vector2(500, 300)), 0.3f); // Ir a un punto aleatorio
    }

    public override void Update(float deltaTime)
    {
        // Lógica de transición
        if (ShouldFlee())
        {
            agent.ChangeState(new FleeState(agent));
        }
    }

    private bool ShouldFlee()
    {
        // Detectar si hay un enemigo cerca
        // Ejemplo: si hay un punto "enemigo" en Vector2(400, 300) y está cerca
        return Vector2.Distance(agent.Position, new Vector2(400, 300)) < 100f;
    }
}
```

### Estado `FleeState`

```csharp
public class FleeState : SteeringState
{
    private Vector2 fleeTarget = new Vector2(400, 300); // posición del enemigo

    public FleeState(Agent agent) : base(agent) { }

    public override void Enter()
    {
        SetBehaviorWeight(new Flee(() => fleeTarget), 2.0f);
        SetBehaviorWeight(new Wander(), 0.2f);
    }

    public override void Update(float deltaTime)
    {
        if (Vector2.Distance(agent.Position, fleeTarget) > 200f)
        {
            agent.ChangeState(new WanderState(agent)); // vuelve a explorar
        }
    }
}
```

### Estado `SeekState`

```csharp
public class SeekState : SteeringState
{
    private Vector2 target;

    public SeekState(Agent agent, Vector2 target) : base(agent)
    {
        this.target = target;
    }

    public override void Enter()
    {
        SetBehaviorWeight(new Seek(() => target), 1.5f);
        SetBehaviorWeight(new Wander(), 0.1f);
    }

    public override void Update(float deltaTime)
    {
        if (Vector2.Distance(agent.Position, target) < 10f)
        {
            agent.ChangeState(new WanderState(agent));
        }
    }
}
```

---

## 🧩 Paso 3: Máquina de estados `FSM`

```csharp
public class FSM
{
    private SteeringState currentState;

    public FSM(SteeringState initialState)
    {
        currentState = initialState;
        currentState.Enter();
    }

    public void Update(float deltaTime)
    {
        currentState?.Update(deltaTime);
    }

    public void ChangeState(SteeringState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public SteeringState GetCurrentState() => currentState;
}
```

---

## 🧩 Paso 4: Modificar `Agent` para usar FSM

```csharp
public class Agent
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float MaxSpeed = 100f;
    public float MaxForce = 10f;

    private Texture2D sprite;
    private FSM fsm;

    public Agent(Vector2 position, Texture2D sprite)
    {
        Position = position;
        Velocity = Vector2.Zero;
        Acceleration = Vector2.Zero;
        this.sprite = sprite;

        // Iniciar con un estado por defecto
        fsm = new FSM(new WanderState(this));
    }

    public void ChangeState(SteeringState state)
    {
        fsm.ChangeState(state);
    }

    public void Update(float deltaTime)
    {
        // Actualizar estado actual
        fsm.Update(deltaTime);

        // Obtener comportamientos y pesos del estado actual
        var behaviors = fsm.GetCurrentState().GetBehaviors();

        Vector2 force = Vector2.Zero;

        foreach (var (behavior, weight) in behaviors)
        {
            Vector2 calculatedForce = behavior.Calculate(this);
            force += calculatedForce * weight;
        }

        // Limitar fuerza
        if (force.Length() > MaxForce)
        {
            force = Vector2.Normalize(force) * MaxForce;
        }

        Acceleration = force;

        Velocity += Acceleration * deltaTime;
        if (Velocity.Length() > MaxSpeed)
        {
            Velocity = Vector2.Normalize(Velocity) * MaxSpeed;
        }

        Position += Velocity * deltaTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, Position, null, Color.Red, 0f, new Vector2(0.5f), 1f, SpriteEffects.None, 0f);
    }
}
```

---

## 🧩 Paso 5: Cambios en `Game1.cs`

```csharp
protected override void LoadContent()
{
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    pixel = new Texture2D(GraphicsDevice, 1, 1);
    pixel.SetData(new[] { Color.White });

    // Agente con FSM
    var agent = new Agent(new Vector2(100, 100), pixel);
    agents.Add(agent);
}