namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// DEFINITIONS / MODULES
// ============================================================================

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(LifetimeModuleDefinition), "lifetime")]
[JsonDerivedType(typeof(InitPositionModuleDefinition), "initPosition")]
[JsonDerivedType(typeof(InitVelocityModuleDefinition), "initVelocity"))
[JsonDerivedType(typeof(InitRotationModuleDefinition), "initRotation"))
[JsonDerivedType(typeof(InitSizeModuleDefinition), "initSize"))
[JsonDerivedType(typeof(InitColorModuleDefinition), "initColor"))
[JsonDerivedType(typeof(InitFrameModuleDefinition), "initFrame"))
[JsonDerivedType(typeof(GravityModuleDefinition), "gravity"))
[JsonDerivedType(typeof(VelocityOverLifetimeModuleDefinition), "velocityOverLifetime"))
[JsonDerivedType(typeof(AccelerationModuleDefinition), "acceleration"))
[JsonDerivedType(typeof(DragModuleDefinition), "drag"))
[JsonDerivedType(typeof(RotationOverLifetimeModuleDefinition), "rotationOverLifetime"))
[JsonDerivedType(typeof(SizeOverLifetimeModuleDefinition), "sizeOverLifetime"))
[JsonDerivedType(typeof(ColorOverLifetimeModuleDefinition), "colorOverLifetime"))
[JsonDerivedType(typeof(NoiseModuleDefinition), "noise"))
[JsonDerivedType(typeof(AttractorModuleDefinition), "attractor"))
[JsonDerivedType(typeof(VortexModuleDefinition), "vortex"))
[JsonDerivedType(typeof(VectorFieldModuleDefinition), "vectorField"))
[JsonDerivedType(typeof(LimitVelocityModuleDefinition), "limitVelocity"))
[JsonDerivedType(typeof(CollisionModuleDefinition), "collision"))
[JsonDerivedType(typeof(SubEmitterModuleDefinition), "subEmitter"))
[JsonDerivedType(typeof(SpriteRenderModuleDefinition), "spriteRender"))
[JsonDerivedType(typeof(FlipbookRenderModuleDefinition), "flipbookRender"))
[JsonDerivedType(typeof(TrailRenderModuleDefinition), "trailRender"))
public abstract record ModuleDefinition
{
    public bool Enabled { get; init; } = true;
    public int Priority { get; init; } = 0;
}
