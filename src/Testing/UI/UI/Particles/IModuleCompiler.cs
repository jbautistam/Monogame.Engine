namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public interface IModuleCompiler
{
    IInitModule CompileInit(ModuleDefinition definition);
    IUpdateModule CompileUpdate(ModuleDefinition definition);
    IRenderModule CompileRender(ModuleDefinition definition, IContentManager content);
}
