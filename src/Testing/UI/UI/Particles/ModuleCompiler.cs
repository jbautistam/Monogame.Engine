namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public sealed class ModuleCompiler : IModuleCompiler
{
    private readonly ICurveCompiler _curveCompiler;
    private readonly IGradientCompiler _gradientCompiler;
    
    public ModuleCompiler(ICurveCompiler curveCompiler, IGradientCompiler gradientCompiler)
    {
        _curveCompiler = curveCompiler;
        _gradientCompiler = gradientCompiler;
    }
    
    public IInitModule CompileInit(ModuleDefinition definition) => definition switch
    {
        LifetimeModuleDefinition d => new LifetimeModule(d, _curveCompiler),
        InitVelocityModuleDefinition d => new InitVelocityModule(d, _curveCompiler),
        InitColorModuleDefinition d => new InitColorModule(d, _gradientCompiler),
        _ => throw new NotSupportedException()
    };
    
    public IUpdateModule CompileUpdate(ModuleDefinition definition) => definition switch
    {
        GravityModuleDefinition d => new GravityModule(d, _curveCompiler),
        ColorOverLifetimeModuleDefinition d => new ColorOverLifetimeModule(d, _gradientCompiler),
        _ => throw new NotSupportedException()
    };
    
    public IRenderModule CompileRender(ModuleDefinition definition, IContentManager content) => definition switch
    {
        SpriteRenderModuleDefinition d => new SpriteRenderModule(d, content.Load<Texture2D>(d.TexturePath)),
        _ => throw new NotSupportedException()
    };
}
