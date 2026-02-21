namespace UI.CharactersEngine;

/// <summary>
///		Motor para controlar los fondos / personajes
/// </summary>
internal class CharactersEngine
{
}

/*
public class CharacterManager
{
    private readonly Dictionary<string, Actor> _actors = new();
    private Sequence _currentSequence;
    private float _sequenceTime;
    private bool _isPlaying;
    private int _nextZOrder = 0; // Auto-incremental para nuevos actores
    
    public bool IsPlaying => _isPlaying;
    public float CurrentTime => _sequenceTime;
    public float SequenceDuration => _currentSequence?.Duration ?? 0f;
    
    public event Action OnSequenceStarted;
    public event Action OnSequenceCompleted;
    public event Action<string> OnActorAdded; // id del actor
    
    // Iniciar secuencia
    public void PlaySequence(Sequence sequence)
    {
        _currentSequence = sequence;
        _currentSequence.Sort();
        _sequenceTime = 0f;
        _isPlaying = true;
        _nextZOrder = 0;
        OnSequenceStarted?.Invoke();
    }
    
    // Detener secuencia
    public void Stop()
    {
        _isPlaying = false;
    }
    
    // Saltar a tiempo específico
    public void Seek(float time)
    {
        _sequenceTime = MathHelper.Clamp(time, 0f, SequenceDuration);
        ApplyAllCommands();
    }
    
    // Update principal - llamar cada frame
    public void Update(float deltaTime)
    {
        if (!_isPlaying || _currentSequence == null) return;
        
        _sequenceTime += deltaTime;
        ApplyAllCommands();
        
        // Verificar si terminó
        if (_sequenceTime >= SequenceDuration)
        {
            _sequenceTime = SequenceDuration;
            _isPlaying = false;
            OnSequenceCompleted?.Invoke();
        }
    }
    
    // Aplica todos los comandos correspondientes al tiempo actual
    private void ApplyAllCommands()
    {
        foreach (var command in _currentSequence.Commands)
        {
            // Solo aplicar si el comando está activo o debe iniciarse
            if (_sequenceTime >= command.StartTime || IsCommandActive(command))
            {
                var actor = GetOrCreateActor(command.ActorId);
                command.Apply(actor, _sequenceTime);
            }
        }
    }
    
    // Determina si un comando debe seguir aplicándose (para valores que persisten)
    private bool IsCommandActive(Command cmd)
    {
        // Los comandos de duración 0 (instantáneos) solo se aplican una vez
        if (cmd.Duration <= 0) return _sequenceTime >= cmd.StartTime;
        
        // Los comandos con duración aplican durante su ventana de tiempo
        return _sequenceTime >= cmd.StartTime && _sequenceTime <= cmd.EndTime;
    }
    
    // Obtiene o crea actor, asignando ZOrder si es nuevo
    private Actor GetOrCreateActor(string id)
    {
        if (!_actors.TryGetValue(id, out var actor))
        {
            actor = new Actor(id);
            actor.Transform.ZOrder = _nextZOrder++;
            actor.IsVisible = true;
            _actors[id] = actor;
            OnActorAdded?.Invoke(id);
        }
        return actor;
    }
    
    // Obtener actor existente (null si no existe)
    public Actor GetActor(string id) => _actors.TryGetValue(id, out var a) ? a : null;
    
    // Verificar si actor existe
    public bool HasActor(string id) => _actors.ContainsKey(id);
    
    // Dibujar todos los actores ordenados por ZOrder
    public void Draw(SpriteBatch spriteBatch)
    {
        var sorted = _actors.Values
            .Where(a => a.IsVisible)
            .OrderBy(a => a.Transform.ZOrder);
            
        foreach (var actor in sorted)
        {
            actor.Draw(spriteBatch);
        }
    }
    
    // Limpiar todos los actores
    public void Clear()
    {
        _actors.Clear();
        _currentSequence = null;
        _isPlaying = false;
        _sequenceTime = 0f;
    }
    
    // Crear secuencia builder
    public SequenceBuilder CreateSequence() => new SequenceBuilder(this);
}

*/