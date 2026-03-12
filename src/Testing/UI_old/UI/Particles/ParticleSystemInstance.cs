namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

// ============================================================================
// RUNTIME / PARTICLE SYSTEM INSTANCE
// ============================================================================

public sealed class ParticleSystemInstance
{
    private readonly ParticleSystemDefinition _definition;
    private readonly List<ParticleEmitter> _emitters = new();
    private readonly Transform2D _transform = new();
    
    private readonly LodEvaluator? _lodEvaluator;
    private readonly SystemMovementRuntime? _movementRuntime;
    private readonly List<VectorFieldRuntime> _globalFields = new();
    
    private float _systemTime;
    private bool _isPlaying;
    private bool _isPaused;
    private Vector2 _spawnPosition;
    
    public Transform2D Transform => _transform;
    public bool IsPlaying => _isPlaying;
    public float SystemTime => _systemTime;
    public ParticleSystemDefinition Definition => _definition;
    public IReadOnlyList<ParticleEmitter> Emitters => _emitters;
    
    public ParticleSystemInstance(ParticleSystemDefinition definition,
        IEmissionShapeCompiler shapeCompiler,
        IModuleCompiler moduleCompiler,
        IContentManager content,
        ICurveCompiler curveCompiler)
    {
        _definition = definition;
        
        foreach (var emitterDef in definition.Emitters)
        {
            var emitter = new ParticleEmitter(emitterDef, shapeCompiler, moduleCompiler, 
                content, emitterDef.Emission.MaxParticles);
            _emitters.Add(emitter);
        }
        
        if (definition.EnableLod && definition.LodLevels.Count > 0)
        {
            _lodEvaluator = new LodEvaluator(definition.LodLevels.ToArray(), definition.LodBlendDistance);
        }
        
        if (definition.SystemMovement != null)
        {
            _movementRuntime = new SystemMovementRuntime(definition.SystemMovement, curveCompiler);
        }
    }
    
    public void Play()
    {
        _isPlaying = true;
        _isPaused = false;
        _systemTime = 0;
        _spawnPosition = _transform.Position;
        
        foreach (var emitter in _emitters)
            emitter.Play(_transform.Position, _transform.Rotation);
    }
    
    public void Stop(bool clearParticles = false)
    {
        _isPlaying = false;
    }
    
    public void Pause() => _isPaused = true;
    public void Resume() => _isPaused = false;
    
    public void Update(float deltaTime, Vector2 cameraPosition)
    {
        if (!_isPlaying || _isPaused) return;
        
        deltaTime *= _definition.SimulationSpeed;
        
        // Evaluar LOD
        if (_lodEvaluator != null)
        {
            _lodEvaluator.Evaluate(_transform.Position, cameraPosition);
            var settings = _lodEvaluator.GetEffectiveSettings();
            
            if (settings.DisableRendering && _definition.PauseWhenCulled)
            {
                if (_definition.SimulateWhenCulled && _systemTime % 0.2f < deltaTime)
                {
                    deltaTime *= 5; // Simular a 1/5 velocidad
                }
                else
                {
                    return;
                }
            }
            
            foreach (var emitter in _emitters)
                emitter.SetLodSettings(settings);
        }
        
        // Actualizar movimiento del sistema
        _movementRuntime?.Update(_transform, _systemTime, _spawnPosition, deltaTime);
        
        // Actualizar campos vectoriales globales
        foreach (var field in _globalFields)
            field.Update(deltaTime);
        
        // Actualizar emisores
        foreach (var emitter in _emitters)
        {
            var pos = new Vector2(_transform.GetAnimatedMatrix().M31, _transform.GetAnimatedMatrix().M32);
            emitter.Update(deltaTime, pos, _transform.Rotation);
        }
        
        _systemTime += deltaTime;
    }
    
    public void Render(ParticleRenderer renderer)
    {
        foreach (var emitter in _emitters.OrderBy(e => e.Definition.SortingOrder))
            emitter.Render(renderer);
    }
}
