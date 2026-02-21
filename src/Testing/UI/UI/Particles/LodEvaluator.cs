namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class LodEvaluator
{
    private readonly LodDefinition[] _lods;
    private readonly float _blendDistance;
    
    public int CurrentLodIndex { get; private set; }
    public float BlendFactor { get; private set; }
    
    public LodEvaluator(LodDefinition[] lods, float blendDistance)
    {
        _lods = lods.OrderBy(l => l.Distance).ToArray();
        _blendDistance = blendDistance;
    }
    
    public void Evaluate(Vector2 systemPos, Vector2 cameraPos)
    {
        float distance = Vector2.Distance(systemPos, cameraPos);
        
        for (int i = 0; i < _lods.Length; i++)
        {
            if (distance < _lods[i].Distance)
            {
                CurrentLodIndex = i;
                
                if (i < _lods.Length - 1)
                {
                    float range = _lods[i + 1].Distance - _lods[i].Distance;
                    BlendFactor = Math.Clamp((distance - _lods[i].Distance + _blendDistance) / (range + _blendDistance), 0, 1);
                }
                else BlendFactor = 0;
                
                return;
            }
        }
        
        CurrentLodIndex = -1;
    }
    
    public LodSettings GetEffectiveSettings()
    {
        if (CurrentLodIndex == -1) return LodSettings.Culled;
        
        var current = _lods[CurrentLodIndex];
        var settings = new LodSettings
        {
            EmissionMultiplier = current.EmissionMultiplier,
            MaxParticles = current.MaxParticlesOverride,
            DisableTrails = current.DisableTrails,
            DisableCollision = current.DisableCollision,
            UpdateEveryNFrames = current.UpdateEveryNFrames,
            LifetimeMultiplier = current.LifetimeMultiplier,
            SizeMultiplier = current.SizeMultiplier,
            SimplifiedSystemPrefab = current.SimplifiedSystemPrefab
        };
        
        return settings;
    }
}
