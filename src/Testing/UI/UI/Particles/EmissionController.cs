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
// RUNTIME / EMISSION CONTROLLER
// ============================================================================

internal sealed class EmissionController
{
    private readonly EmissionSettingsDefinition _def;
    private readonly EmissionShape _shape;
    private readonly ParticleRandom _random;
    
    private float _emitterTime;
    private float _delayTimer;
    private float _rateAccumulator;
    private float _distanceAccumulator;
    private Vector2 _lastPosition;
    private int _loopCount;
    private bool _isPlaying;
    private bool _isPaused;
    
    private readonly List<BurstState> _burstStates = new();
    private LodSettings _lodSettings = LodSettings.FullQuality;
    
    public EmissionController(EmissionSettingsDefinition definition, EmissionShape shape, int seed = 0)
    {
        _def = definition;
        _shape = shape;
        _random = new ParticleRandom(seed);
        InitializeBurstStates();
    }
    
    public bool IsPlaying => _isPlaying;
    public float EmitterTime => _emitterTime;
    
    public void Play()
    {
        _isPlaying = true;
        _isPaused = false;
        _delayTimer = _def.StartDelay;
        _emitterTime = 0;
        _rateAccumulator = 0;
        ResetBurstStates();
    }
    
    public void Stop() => _isPlaying = false;
    public void Pause() => _isPaused = true;
    public void Resume() => _isPaused = false;
    
    public void SetLodSettings(LodSettings lod) => _lodSettings = lod;
    
    public int Update(float deltaTime, Vector2 currentPosition, int currentParticleCount)
    {
        if (!_isPlaying || _isPaused) return 0;
        
        deltaTime *= _def.SimulationSpeed;
        
        if (_delayTimer > 0)
        {
            _delayTimer -= deltaTime;
            if (_delayTimer > 0) return 0;
            deltaTime = -_delayTimer;
            _delayTimer = 0;
        }
        
        _emitterTime += deltaTime;
        _shape.Update(deltaTime, _emitterTime);
        
        if (_def.Duration > 0 && _emitterTime >= _def.Duration)
        {
            if (_def.Looping)
            {
                _loopCount++;
                _emitterTime %= _def.Duration;
                ResetBurstStates();
            }
            else
            {
                _isPlaying = false;
                return 0;
            }
        }
        
        int count = _def.Mode switch
        {
            EmissionMode.Constant => CalculateConstantEmission(deltaTime, currentPosition, currentParticleCount),
            EmissionMode.Burst => CalculateBurstEmission(),
            _ => 0
        };
        
        count = (int)(count * _lodSettings.EmissionMultiplier);
        
        var effectiveMax = _lodSettings.MaxParticles > 0 
            ? Math.Min(_def.MaxParticles, _lodSettings.MaxParticles) 
            : _def.MaxParticles;
        
        int available = effectiveMax - currentParticleCount;
        return Math.Min(count, available);
    }
    
    private int CalculateConstantEmission(float deltaTime, Vector2 currentPosition, int currentCount)
    {
        int count = 0;
        
        if (_def.RateOverTime > 0)
        {
            _rateAccumulator += _def.RateOverTime * deltaTime;
            int whole = (int)_rateAccumulator;
            _rateAccumulator -= whole;
            count += whole;
        }
        
        if (_def.RateOverDistance > 0)
        {
            float dist = Vector2.Distance(currentPosition, _lastPosition);
            _distanceAccumulator += _def.RateOverDistance * dist;
            int whole = (int)_distanceAccumulator;
            _distanceAccumulator -= whole;
            count += whole;
        }
        
        _lastPosition = currentPosition;
        return count;
    }
    
    private int CalculateBurstEmission()
    {
        int count = 0;
        
        foreach (var burst in _burstStates.Where(b => !b.IsComplete))
        {
            float checkTime = burst.Def.TimeRelativeToEmitter ? _emitterTime : _emitterTime + _loopCount * _def.Duration;
            float cycleTime = burst.Def.Time + burst.CurrentCycle * burst.Def.Interval;
            
            if (!burst.HasTriggered && checkTime >= cycleTime)
            {
                if (_random.NextFloat() * 100 < burst.Def.Probability)
                    count += burst.Def.Count.GetValue(_random);
                
                burst.HasTriggered = true;
                burst.CurrentCycle++;
                if (burst.CurrentCycle >= burst.Def.Cycles) burst.IsComplete = true;
                else burst.HasTriggered = false;
            }
        }
        
        return count;
    }
    
    public void GetEmissionPoints(Span<EmissionPoint> output)
    {
        for (int i = 0; i < output.Length; i++)
            output[i] = _shape.Sample(_random);
    }
    
    private void InitializeBurstStates() => 
        _burstStates.AddRange(_def.Bursts.Select(b => new BurstState(b)));
    
    private void ResetBurstStates() => _burstStates.ForEach(s => s.Reset());
    
    private class BurstState
    {
        public BurstDefinition Def;
        public int CurrentCycle;
        public bool HasTriggered;
        public bool IsComplete;
        
        public BurstState(BurstDefinition def) { Def = def; Reset(); }
        
        public void Reset()
        {
            CurrentCycle = 0;
            HasTriggered = false;
            IsComplete = false;
        }
    }
}
