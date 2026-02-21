namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class AnalyticGradient : ParticleGradient
{
    private readonly GradientDefinition.ColorStop[] _colorStops;
    private readonly GradientDefinition.AlphaStop[]? _alphaStops;
    private readonly GradientDefinition.ColorSpace _colorSpace;
    private readonly bool _premultiply;
    
    public AnalyticGradient(
        GradientDefinition.ColorStop[] colorStops,
        GradientDefinition.AlphaStop[]? alphaStops,
        GradientDefinition.ColorSpace colorSpace,
        bool premultiply)
    {
        _colorStops = colorStops;
        _alphaStops = alphaStops;
        _colorSpace = colorSpace;
        _premultiply = premultiply;
    }
    
    public override Color Evaluate(float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        
        int colorIdx = FindStopIndex(_colorStops, t);
        var color = InterpolateColor(colorIdx, t);
        
        float alpha = _alphaStops != null ? InterpolateAlpha(FindStopIndex(_alphaStops, t), t) : color.A / 255f;
        color = new Color(color.R, color.G, color.B, (byte)(alpha * 255));
        
        return _premultiply ? Color.PremultiplyAlpha(color) : color;
    }
    
    private int FindStopIndex<T>(T[] stops, float t) where T : IComparableTime
    {
        for (int i = 0; i < stops.Length - 1; i++)
            if (t >= stops[i].Time && t <= stops[i + 1].Time) return i;
        return stops.Length - 2;
    }
    
    private Color InterpolateColor(int idx, float t)
    {
        var s0 = _colorStops[idx];
        var s1 = _colorStops[idx + 1];
        float localT = (t - s0.Time) / (s1.Time - s0.Time);
        
        return _colorSpace switch
        {
            GradientDefinition.ColorSpace.Linear => Color.Lerp(s0.Color.ToColor(), s1.Color.ToColor(), localT),
            _ => Color.Lerp(s0.Color.ToColor(), s1.Color.ToColor(), localT) // Simplificado
        };
    }
    
    private float InterpolateAlpha(int idx, float t)
    {
        var s0 = _alphaStops![idx];
        var s1 = _alphaStops[idx + 1];
        float localT = (t - s0.Time) / (s1.Time - s0.Time);
        if (s0.Easing.HasValue) localT = EasingFunctions.Apply(localT, s0.Easing.Value);
        return float.Lerp(s0.Alpha, s1.Alpha, localT);
    }
    
    public override (Color min, Color max) GetApproximateRange() => (Color.Black, Color.White);
    
    public override Texture2D GenerateLutTexture(GraphicsDevice device, int width)
    {
        var texture = new Texture2D(device, width, 1);
        var data = new Color[width];
        for (int i = 0; i < width; i++)
            data[i] = Evaluate(i / (float)(width - 1));
        texture.SetData(data);
        return texture;
    }
}
