using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.CharactersEngine.Sequences;


// Reacción de impacto/emoción
public class ImpactReactionCommand : AbstractSequenceCommand
{
    public enum ReactionType { Stagger, Knockback, Collapse, Slide, Spin, Shake }
    
    private readonly Vector2 _impactSource;
    private readonly float _force;
    private readonly ReactionType _reaction;
    private Vector2 _startPos;
    private float _startRotation;
    private bool _initialized;
    
    public ImpactReactionCommand(string actorId, float startTime, float duration,
        Vector2 impactSource, float force, ReactionType reaction)
        : base(actorId, startTime, duration)
    {
        _impactSource = impactSource;
        _force = force;
        _reaction = reaction;
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (!_initialized)
        {
            _startPos = actor.Transform.Position;
            _startRotation = actor.Transform.Rotation;
            _initialized = true;
        }
        
        if (sequenceTime < StartTime) return;
        if (sequenceTime >= EndTime)
        {
            actor.Transform.Position = _startPos;
            actor.Transform.Rotation = _startRotation;
            return;
        }
        
        float t = (sequenceTime - StartTime) / Duration;
        Vector2 impactDir = Vector2.Normalize(_startPos - _impactSource);
        
        switch (_reaction)
        {
            case ReactionType.Stagger:
                ApplyStagger(actor, t, impactDir);
                break;
            case ReactionType.Knockback:
                ApplyKnockback(actor, t, impactDir);
                break;
            case ReactionType.Collapse:
                ApplyCollapse(actor, t, impactDir);
                break;
            case ReactionType.Slide:
                ApplySlide(actor, t, impactDir);
                break;
            case ReactionType.Spin:
                ApplySpin(actor, t);
                break;
            case ReactionType.Shake:
                ApplyShake(actor, t);
                break;
        }
    }
    
    private void ApplyStagger(Actor actor, float t, Vector2 dir)
    {
        // Tambaleo hacia atrás y recuperación
        float backAmount = MathF.Sin(t * MathF.PI) * _force * 0.3f;
        actor.Transform.Position = _startPos + dir * backAmount;
        
        // Rotación de tambaleo
        float tilt = MathF.Sin(t * MathF.PI * 2) * 0.1f * (1 - t);
        actor.Transform.Rotation = _startRotation + tilt;
    }
    
    private void ApplyKnockback(Actor actor, float t, Vector2 dir)
    {
        // Empujón fuerte y recuperación lenta
        if (t < 0.3f)
        {
            float localT = t / 0.3f;
            actor.Transform.Position = _startPos + dir * _force * Easing.ExpoOut(localT);
        }
        else
        {
            float localT = (t - 0.3f) / 0.7f;
            actor.Transform.Position = _startPos + dir * _force * (1 - Easing.QuadOut(localT));
        }
    }
    
    private void ApplyCollapse(Actor actor, float t, Vector2 dir)
    {
        // Caída al suelo
        float fallProgress = Easing.QuadIn(t);
        actor.Transform.Position = _startPos + dir * _force * 0.5f * fallProgress;
        actor.Transform.Rotation = _startRotation + fallProgress * MathHelper.PiOver2;
        actor.Transform.Scale = new Vector2(1 + fallProgress * 0.2f, 1 - fallProgress * 0.3f);
    }
    
    private void ApplySlide(Actor actor, float t, Vector2 dir)
    {
        // Deslizamiento con fricción
        float distance = _force * (1 - Easing.QuadOut(t));
        actor.Transform.Position = _startPos + dir * distance;
    }
    
    private void ApplySpin(Actor actor, float t)
    {
        // Giro por el impacto
        float rotations = _force / 100f;
        actor.Transform.Rotation = _startRotation + rotations * MathHelper.TwoPi * Easing.QuadOut(t);
    }
    
    private void ApplyShake(Actor actor, float t)
    {
        // Vibración intensa que decae
        float intensity = _force * (1 - t);
        float noise = SimplexNoise.Noise(t * 20) * intensity;
        actor.Transform.Position = _startPos + new Vector2(noise, noise * 0.5f);
    }
}

```

## Comandos de Órbita

```csharp

// Espiral de Arquímedes
public class SpiralOrbitCommand : AbstractSequenceCommand
{
    private readonly Vector2 _center;
    private readonly float _startRadius;
    private readonly float _endRadius;
    private readonly float _turns;
    private readonly bool _inward;
    private readonly IEasingDefinition _easing;
    
    public SpiralOrbitCommand(string actorId, float startTime, float duration,
        Vector2 center, float startRadius, float endRadius, float turns,
        bool inward = true, IEasingDefinition easing = null)
        : base(actorId, startTime, duration)
    {
        _center = center;
        _startRadius = startRadius;
        _endRadius = endRadius;
        _turns = turns;
        _inward = inward;
        _easing = easing ?? new LambdaDefinition(Easing.Linear);
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (sequenceTime < StartTime) return;
        
        float t = MathHelper.Clamp((sequenceTime - StartTime) / Duration, 0, 1);
        float easedT = _easing.Evaluate(t);
        
        float angle = easedT * _turns * MathHelper.TwoPi;
        if (!_inward) angle = -angle;
        
        float radius = MathHelper.Lerp(_startRadius, _endRadius, easedT);
        
        actor.Transform.Position = new Vector2(
            _center.X + MathF.Cos(angle) * radius,
            _center.Y + MathF.Sin(angle) * radius
        );
        
        // Rotación acumulativa
        actor.Transform.Rotation = angle;
    }
}

// Pendulo
public class PendulumCommand : AbstractSequenceCommand
{
    private readonly Vector2 _pivot;
    private readonly float _length;
    private readonly float _maxAngle;
    private readonly int _oscillations;
    private readonly float _damping;
    private float _baseRotation;
    private bool _initialized;
    
    public PendulumCommand(string actorId, float startTime, float duration,
        Vector2 pivot, float length, float maxAngleDegrees, int oscillations = 3, float damping = 0.8f)
        : base(actorId, startTime, duration)
    {
        _pivot = pivot;
        _length = length;
        _maxAngle = MathHelper.ToRadians(maxAngleDegrees);
        _oscillations = oscillations;
        _damping = damping;
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (!_initialized)
        {
            _baseRotation = actor.Transform.Rotation;
            _initialized = true;
        }
        
        if (sequenceTime < StartTime) return;
        if (sequenceTime >= EndTime)
        {
            actor.Transform.Position = CalculatePosition(0);
            return;
        }
        
        float t = (sequenceTime - StartTime) / Duration;
        
        // Oscilación amortiguada
        float currentOscillation = t * _oscillations;
        float angle = _maxAngle * MathF.Cos(currentOscillation * MathHelper.TwoPi);
        
        // Aplicar damping
        float dampingFactor = MathF.Pow(_damping, currentOscillation);
        angle *= dampingFactor;
        
        actor.Transform.Position = CalculatePosition(angle);
        actor.Transform.Rotation = _baseRotation + angle;
    }
    
    private Vector2 CalculatePosition(float angle)
    {
        return new Vector2(
            _pivot.X + MathF.Sin(angle) * _length,
            _pivot.Y + MathF.Cos(angle) * _length
        );
    }
}
```

## Comandos de Física

```csharp
// Caída libre con gravedad
public class FallCommand : AbstractSequenceCommand
{
    private readonly float _gravity;
    private readonly float _groundY;
    private readonly bool _bounce;
    private readonly float _restitution;
    private Vector2 _startPos;
    private float _velocityY;
    private bool _grounded;
    private bool _initialized;
    
    public FallCommand(string actorId, float startTime, float duration,
        float gravity, float groundY, bool bounce = false, float restitution = 0.6f)
        : base(actorId, startTime, duration)
    {
        _gravity = gravity;
        _groundY = groundY;
        _bounce = bounce;
        _restitution = restitution;
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (!_initialized)
        {
            _startPos = actor.Transform.Position;
            _velocityY = 0;
            _initialized = true;
        }
        
        if (sequenceTime < StartTime) return;
        
        float dt = 0.016f; // Asumimos 60fps, en realidad usar deltaTime
        float elapsed = sequenceTime - StartTime;
        
        // Simulación física simple
        if (!_grounded)
        {
            _velocityY += _gravity * dt;
            float newY = actor.Transform.Position.Y + _velocityY * dt;
            
            if (newY >= _groundY)
            {
                // Impacto con suelo
                if (_bounce && MathF.Abs(_velocityY) > 50)
                {
                    newY = _groundY;
                    _velocityY = -_velocityY * _restitution;
                    
                    // Efecto de squash
                    float squash = 1 - MathF.Abs(_velocityY) / 1000f * 0.3f;
                    actor.Transform.Scale = new Vector2(1.2f, squash);
                }
                else
                {
                    newY = _groundY;
                    _grounded = true;
                    actor.Transform.Scale = Vector2.One;
                }
            }
            
            actor.Transform.Position = new Vector2(actor.Transform.Position.X, newY);
        }
    }
}

// Proyectil balístico
public class ProjectileCommand : AbstractSequenceCommand
{
    private readonly Vector2 _velocity;
    private readonly float _gravity;
    private readonly float _drag;
    private Vector2 _startPos;
    private Vector2 _currentVelocity;
    private bool _initialized;
    
    public ProjectileCommand(string actorId, float startTime, float duration,
        Vector2 initialVelocity, float gravity = 980f, float drag = 0.99f)
        : base(actorId, startTime, duration)
    {
        _velocity = initialVelocity;
        _gravity = gravity;
        _drag = drag;
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (!_initialized)
        {
            _startPos = actor.Transform.Position;
            _currentVelocity = _velocity;
            _initialized = true;
        }
        
        if (sequenceTime < StartTime) return;
        
        float dt = 0.016f;
        float elapsed = sequenceTime - StartTime;
        
        // Integración de Euler
        _currentVelocity.Y += _gravity * dt;
        _currentVelocity *= _drag;
        
        actor.Transform.Position += _currentVelocity * dt;
        
        // Rotar según velocidad
        actor.Transform.Rotation = MathF.Atan2(_currentVelocity.Y, _currentVelocity.X);
    }
}

// Flotación en líquido
public class BuoyancyCommand : AbstractSequenceCommand
{
    private readonly float _baseHeight;
    private readonly float _amplitude;
    private readonly float _frequency;
    private readonly float _drift;
    private readonly float _phase;
    
    public BuoyancyCommand(string actorId, float startTime, float duration,
        float baseHeight, float amplitude = 10f, float frequency = 2f, float drift = 20f)
        : base(actorId, startTime, duration)
    {
        _baseHeight = baseHeight;
        _amplitude = amplitude;
        _frequency = frequency;
        _drift = drift;
        _phase = Random.Shared.NextSingle() * MathHelper.TwoPi;
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (sequenceTime < StartTime) return;
        
        float t = sequenceTime - StartTime;
        
        // Onda senoidal con fase aleatoria
        float yOffset = MathF.Sin(t * _frequency + _phase) * _amplitude;
        
        // Deriva horizontal lenta
        float xOffset = MathF.Sin(t * 0.5f + _phase) * _drift;
        
        actor.Transform.Position = new Vector2(
            actor.Transform.Position.X + xOffset * 0.016f,
            _baseHeight + yOffset
        );
        
        // Rotación suave de balanceo
        actor.Transform.Rotation = MathF.Sin(t * _frequency * 0.5f + _phase) * 0.05f;
    }
}

// Viento que empuja
public class WindPushCommand : AbstractSequenceCommand
{
    private readonly Vector2 _direction;
    private readonly float _strength;
    private readonly float _resistance;
    private readonly float _turbulence;
    private Vector2 _startPos;
    private Vector2 _currentVelocity;
    private bool _initialized;
    
    public WindPushCommand(string actorId, float startTime, float duration,
        Vector2 direction, float strength, float resistance = 0.95f, float turbulence = 0.2f)
        : base(actorId, startTime, duration)
    {
        _direction = Vector2.Normalize(direction);
        _strength = strength;
        _resistance = resistance;
        _turbulence = turbulence;
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (!_initialized)
        {
            _startPos = actor.Transform.Position;
            _currentVelocity = Vector2.Zero;
            _initialized = true;
        }
        
        if (sequenceTime < StartTime) return;
        
        float t = sequenceTime - StartTime;
        
        // Fuerza del viento con turbulencia
        float noise = SimplexNoise.Noise(t * 3) * _turbulence;
        Vector2 windForce = _direction * _strength * (1 + noise);
        
        // Aplicar fuerza y resistencia
        _currentVelocity += windForce * 0.016f;
        _currentVelocity *= _resistance;
        
        actor.Transform.Position += _currentVelocity * 0.016f;
        
        // Rotación según fuerza del viento
        actor.Transform.Rotation = MathF.Atan2(_currentVelocity.Y, _currentVelocity.X) * 0.1f;
    }
}
```

## Comandos de Seguimiento de Rutas

```csharp
// Patrulla entre waypoints
public class PatrolCommand : AbstractSequenceCommand
{
    public enum PatrolType { Loop, PingPong, Random, Once }
    
    private readonly Vector2[] _waypoints;
    private readonly PatrolType _type;
    private readonly float _waitTime;
    private readonly float _moveSpeed;
    private readonly IEasingDefinition _easing;
    private int _currentIndex;
    private int _direction;
    private float _waitTimer;
    private bool _waiting;
    private Vector2 _currentTarget;
    private Vector2 _previousPosition;
    private float _segmentProgress;
    
    public PatrolCommand(string actorId, float startTime, float duration,
        Vector2[] waypoints, PatrolType type = PatrolType.Loop, 
        float waitTime = 0.5f, float moveSpeed = 200f, IEasingDefinition easing = null)
        : base(actorId, startTime, duration)
    {
        _waypoints = waypoints;
        _type = type;
        _waitTime = waitTime;
        _moveSpeed = moveSpeed;
        _easing = easing ?? new LambdaDefinition(Easing.Linear);
        _direction = 1;
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (sequenceTime < StartTime) return;
        
        if (_waiting)
        {
            _waitTimer += 0.016f;
            if (_waitTimer >= _waitTime)
            {
                _waiting = false;
                SelectNextWaypoint();
            }
            return;
        }
        
        // Mover hacia waypoint actual
        Vector2 toTarget = _currentTarget - actor.Transform.Position;
        float distance = toTarget.Length();
        
        if (distance < 5f)
        {
            // Llegó al waypoint
            _waiting = true;
            _waitTimer = 0;
            actor.Transform.Position = _currentTarget;
            return;
        }
        
        // Movimiento con easing por segmento
        Vector2 moveDir = Vector2.Normalize(toTarget);
        float moveAmount = Math.Min(_moveSpeed * 0.016f, distance);
        
        actor.Transform.Position += moveDir * moveAmount;
        
        // Rotar hacia dirección de movimiento
        if (toTarget != Vector2.Zero)
            actor.Transform.Rotation = MathF.Atan2(moveDir.Y, moveDir.X);
    }
    
    private void SelectNextWaypoint()
    {
        switch (_type)
        {
            case PatrolType.Loop:
                _currentIndex = (_currentIndex + 1) % _waypoints.Length;
                break;
            case PatrolType.PingPong:
                _currentIndex += _direction;
                if (_currentIndex >= _waypoints.Length - 1 || _currentIndex <= 0)
                    _direction *= -1;
                break;
            case PatrolType.Random:
                _currentIndex = Random.Shared.Next(_waypoints.Length);
                break;
            case PatrolType.Once:
                if (_currentIndex < _waypoints.Length - 1)
                    _currentIndex++;
                break;
        }
        
        _previousPosition = _currentTarget;
        _currentTarget = _waypoints[_currentIndex];
    }
}

// Ruta con checkpoints y callbacks
public class PathCommand : AbstractSequenceCommand
{
    private readonly Vector2[] _checkpoints;
    private readonly float _checkpointRadius;
    private readonly Action<int> _onCheckpoint;
    private readonly IEasingDefinition _easing;
    private int _currentCheckpoint;
    private bool[] _reached;
    
    public PathCommand(string actorId, float startTime, float duration,
        Vector2[] checkpoints, float checkpointRadius = 10f, 
        Action<int> onCheckpointReached = null, IEasingDefinition easing = null)
        : base(actorId, startTime, duration)
    {
        _checkpoints = checkpoints;
        _checkpointRadius = checkpointRadius;
        _onCheckpoint = onCheckpointReached;
        _easing = easing ?? new LambdaDefinition(Easing.Linear);
        _reached = new bool[checkpoints.Length];
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (sequenceTime < StartTime) return;
        
        float t = MathHelper.Clamp((sequenceTime - StartTime) / Duration, 0, 1);
        float easedT = _easing.Evaluate(t);
        
        // Encontrar segmento actual
        float totalLength = CalculateTotalLength();
        float targetDistance = easedT * totalLength;
        
        float accumulated = 0;
        for (int i = 0; i < _checkpoints.Length - 1; i++)
        {
            float segmentLength = Vector2.Distance(_checkpoints[i], _checkpoints[i + 1]);
            
            if (accumulated + segmentLength >= targetDistance)
            {
                // En este segmento
                float localT = (targetDistance - accumulated) / segmentLength;
                actor.Transform.Position = Vector2.Lerp(_checkpoints[i], _checkpoints[i + 1], localT);
                
                // Notificar checkpoint si es nuevo
                if (!_reached[i])
                {
                    _reached[i] = true;
                    _onCheckpoint?.Invoke(i);
                }
                
                break;
            }
            
            accumulated += segmentLength;
        }
    }
    
    private float CalculateTotalLength()
    {
        float length = 0;
        for (int i = 0; i < _checkpoints.Length - 1; i++)
            length += Vector2.Distance(_checkpoints[i], _checkpoints[i + 1]);
        return length;
    }
}

// Seguimiento con retardo (cola/snake)
public class FollowCommand : AbstractSequenceCommand
{
    private readonly string _targetActorId;
    private readonly float _delay;
    private readonly float _smoothness;
    private readonly float _minDistance;
    private Queue<(float time, Vector2 pos)> _history;
    private Vector2 _lastTargetPos;
    
    public FollowCommand(string actorId, float startTime, float duration,
        string targetActorId, float delay = 0.3f, float smoothness = 0.1f, float minDistance = 5f)
        : base(actorId, startTime, duration)
    {
        _targetActorId = targetActorId;
        _delay = delay;
        _smoothness = smoothness;
        _minDistance = minDistance;
        _history = new Queue<(float, Vector2)>();
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (sequenceTime < StartTime) return;
        
        // Obtener posición del target (asumiendo que tenemos referencia al manager)
        // En implementación real, pasar referencia al CharacterManager
        Vector2 targetPos = GetTargetPosition(); // Placeholder
        
        // Guardar historial de posiciones
        _history.Enqueue((sequenceTime, targetPos));
        
        // Remover posiciones viejas
        while (_history.Count > 0 && sequenceTime - _history.Peek().time > _delay)
            _history.Dequeue();
        
        // Seguir posición retrasada
        if (_history.Count > 0)
        {
            Vector2 targetDelayedPos = _history.Peek().pos;
            Vector2 toTarget = targetDelayedPos - actor.Transform.Position;
            float distance = toTarget.Length();
            
            if (distance > _minDistance)
            {
                // Movimiento suave hacia posición retrasada
                actor.Transform.Position = Vector2.Lerp(
                    actor.Transform.Position, 
                    targetDelayedPos, 
                    _smoothness
                );
            }
        }
    }
    
    private Vector2 GetTargetPosition()
    {
        // Implementar con referencia al manager
        return Vector2.Zero;
    }
}
