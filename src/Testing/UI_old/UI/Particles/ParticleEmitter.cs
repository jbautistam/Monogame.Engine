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
// RUNTIME / PARTICLE EMITTER
// ============================================================================

public sealed class ParticleEmitter
{
    private readonly EmitterDefinition _definition;
    private readonly EmissionController _controller;
    private readonly ParticleDataArrays _data;
    private readonly int _capacity;
    
    private readonly List<IInitModule> _initModules = new();
    private readonly List<IUpdateModule> _updateModules = new();
    private readonly List<IRenderModule> _renderModules = new();
    
    private int _aliveCount;
    private int _deadCount;
    private int[] _deadIndices;
    private EmissionPoint[] _emissionBuffer;
    
    private LodSettings _lodSettings = LodSettings.FullQuality;
    private int _frameCounter;
    private Vector2 _parentOffset;
    
    public int Capacity => _capacity;
    public int AliveCount => _aliveCount;
    public ParticleDataArrays Data => _data;
    public EmitterDefinition Definition => _definition;
    
    public ParticleEmitter(EmitterDefinition definition, IEmissionShapeCompiler shapeCompiler,
        IModuleCompiler moduleCompiler, IContentManager content, int capacity)
    {
        _definition = definition;
        _capacity = capacity;
        
        var shape = shapeCompiler.Compile(definition.Emission.Shape);
        _controller = new EmissionController(definition.Emission, shape);
        
        _data = new ParticleDataArrays();
        _data.Initialize(capacity);
        
        _deadIndices = new int[capacity];
        for (int i = 0; i < capacity; i++) _deadIndices[i] = capacity - 1 - i;
        _deadCount = capacity;
        
        _emissionBuffer = new EmissionPoint[64];
        
        foreach (var mod in definition.InitModules)
            _initModules.Add(moduleCompiler.CompileInit(mod));
        foreach (var mod in definition.UpdateModules)
            _updateModules.Add(moduleCompiler.CompileUpdate(mod));
        foreach (var mod in definition.RenderModules)
            _renderModules.Add(moduleCompiler.CompileRender(mod, content));
    }
    
    public void SetParentOffset(Vector2 offset) => _parentOffset = offset;
    
    public void SetLodSettings(LodSettings lod)
    {
        _lodSettings = lod;
        
        if (lod.LifetimeMultiplier < 1f)
        {
            for (int i = 0; i < _capacity; i++)
            {
                if (_data.IsAlive[i])
                    _data.Lifetimes[i] *= lod.LifetimeMultiplier;
            }
        }
    }
    
    public void Play(Vector2 position, float rotation)
    {
        _controller.Play();
    }
    
    public void Update(float deltaTime, Vector2 systemPosition, float systemRotation)
    {
        var position = systemPosition + _parentOffset;
        
        int toSpawn = _controller.Update(deltaTime, position, _aliveCount);
        
        if (toSpawn > 0 && _deadCount > 0)
        {
            EnsureBufferSize(toSpawn);
            _controller.GetEmissionPoints(_emissionBuffer.AsSpan(0, Math.Min(toSpawn, _deadCount)));
            
            for (int i = 0; i < Math.Min(toSpawn, _deadCount); i++)
                SpawnParticle(_emissionBuffer[i]);
        }
        
        _frameCounter++;
        if (_frameCounter % _lodSettings.UpdateEveryNFrames != 0)
        {
            EssentialUpdate(deltaTime);
            return;
        }
        
        UpdateParticles(deltaTime);
    }
    
    private void SpawnParticle(in EmissionPoint emissionPoint)
    {
        if (_deadCount == 0) return;
        
        int index = _deadIndices[--_deadCount];
        
        _data.IsAlive[index] = true;
        _data.Ages[index] = 0;
        _data.NormalizedTimes[index] = 0;
        _data.Positions[index] = emissionPoint.Position;
        _data.Velocities[index] = emissionPoint.Direction;
        
        var particleRef = new ParticleRef(_data, index);
        foreach (var module in _initModules)
            module.Initialize(particleRef, emissionPoint, _controller.EmitterTime);
        
        _aliveCount++;
    }
    
    private void UpdateParticles(float deltaTime)
    {
        for (int i = 0; i < _capacity; i++)
        {
            if (!_data.IsAlive[i]) continue;
            
            _data.Ages[i] += deltaTime;
            float lifetime = _data.Lifetimes[i];
            
            if (lifetime > 0)
            {
                _data.NormalizedTimes[i] = Math.Clamp(_data.Ages[i] / lifetime, 0f, 1f);
                
                if (_data.Ages[i] >= lifetime)
                {
                    KillParticle(i);
                    continue;
                }
            }
            
            var particleRef = new ParticleRef(_data, i);
            foreach (var module in _updateModules)
            {
                if (ShouldSkipModule(module)) continue;
                module.Update(particleRef, deltaTime, _controller.EmitterTime);
            }
        }
    }
    
    private void EssentialUpdate(float deltaTime)
    {
        // Solo actualizar edad y matar
        for (int i = 0; i < _capacity; i++)
        {
            if (!_data.IsAlive[i]) continue;
            
            _data.Ages[i] += deltaTime;
            if (_data.Ages[i] >= _data.Lifetimes[i])
                KillParticle(i);
        }
    }
    
    private bool ShouldSkipModule(IUpdateModule module) => module switch
    {
        _ when _lodSettings.DisableTrails => module is TrailRenderModule,
        _ when _lodSettings.DisableCollision => module is CollisionModule,
        _ => false
    };
    
    private void KillParticle(int index)
    {
        if (!_data.IsAlive[index]) return;
        _data.IsAlive[index] = false;
        _deadIndices[_deadCount++] = index;
        _aliveCount--;
    }
    
    private void EnsureBufferSize(int required)
    {
        if (_emissionBuffer.Length >= required) return;
        Array.Resize(ref _emissionBuffer, Math.Max(_emissionBuffer.Length * 2, required));
    }
    
    public void Render(ParticleRenderer renderer)
    {
        if (_aliveCount == 0 || _lodSettings.DisableRendering) return;
        
        foreach (var module in _renderModules)
            module.Render(renderer, _data, _aliveCount);
    }
}
