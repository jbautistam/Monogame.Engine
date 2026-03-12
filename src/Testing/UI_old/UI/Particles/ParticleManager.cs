namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public sealed class ParticleManager : IDisposable
{
    private static ParticleManager? _instance;
    public static ParticleManager Instance => _instance!;
    
    private readonly GraphicsDevice _device;
    private readonly IContentManager _content;
    private readonly List<ParticleSystemInstance> _activeSystems = new();
    
    private readonly CurveCompiler _curveCompiler;
    private readonly GradientCompiler _gradientCompiler;
    private readonly EmissionShapeCompiler _shapeCompiler;
    private readonly ModuleCompiler _moduleCompiler;
    
    public ParticleManager(GraphicsDevice device, IContentManager content)
    {
        _device = device;
        _content = content;
        
        _curveCompiler = new CurveCompiler();
        _gradientCompiler = new GradientCompiler(_curveCompiler);
        _shapeCompiler = new EmissionShapeCompiler();
        _moduleCompiler = new ModuleCompiler(_curveCompiler, _gradientCompiler);
        
        _instance = this;
    }
    
    public ParticleSystemInstance Spawn(ParticleSystemDefinition definition, Vector2 position)
    {
        var system = new ParticleSystemInstance(definition, _shapeCompiler, _moduleCompiler, 
            _content, _curveCompiler);
        
        system.Transform.Position = position;
        system.Play();
        
        _activeSystems.Add(system);
        return system;
    }
    
    public void Update(float deltaTime, Vector2 cameraPosition)
    {
        foreach (var system in _activeSystems.Where(s => s.IsPlaying))
            system.Update(deltaTime, cameraPosition);
        
        _activeSystems.RemoveAll(s => !s.IsPlaying && s.Emitters.All(e => e.AliveCount == 0));
    }
    
    public void Render(ParticleRenderer renderer, Matrix viewProjection)
    {
        renderer.Begin(viewProjection);
        
        foreach (var system in _activeSystems)
            system.Render(renderer);
        
        renderer.End();
    }
    
    public void Dispose()
    {
        _instance = null;
    }
}
