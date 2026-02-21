public sealed record AttractorModuleDefinition : ModuleDefinition
{
    public required Vector2 Position { get; init; }
    public FloatRangeOrCurve Strength { get; init; } = new(10f, 10f);
    public float Radius { get; init; } = 0f;
    public FalloffMode Falloff { get; init; } = FalloffMode.Linear;
    public bool AttachToEmitter { get; init; } = false;
    public Vector2 LocalOffset { get; init; } = Vector2.Zero;
    public CurveDefinition? FalloffCurve { get; init; }
    
    public enum FalloffMode { None, Linear, InverseSquare, SmoothStep, Custom }
}
