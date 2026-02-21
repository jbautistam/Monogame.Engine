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
// RUNTIME / VECTOR FIELDS
// ============================================================================

internal sealed class VectorFieldRuntime
{
    private readonly VectorFieldDefinition _def;
    private readonly Texture2D _texture;
    private readonly Color[]? _cachedData;
    private readonly int _width, _height;
    private float _animationTime;
    
    public VectorFieldRuntime(VectorFieldDefinition def, Texture2D texture, bool cacheOnCpu = true)
    {
        _def = def;
        _texture = texture;
        
        if (cacheOnCpu)
        {
            _cachedData = new Color[texture.Width * texture.Height];
            texture.GetData(_cachedData);
            _width = texture.Width;
            _height = texture.Height;
        }
    }
    
    public void Update(float deltaTime) => _animationTime += deltaTime;
    
    public Vector2 Sample(Vector2 worldPosition)
    {
        Vector2 uv = CalculateUV(worldPosition);
        
        Vector2 fieldValue = _cachedData != null ? SampleCpu(uv) : SampleGpu(uv);
        
        fieldValue *= _def.Strength;
        
        if (_def.Animation == VectorFieldDefinition.AnimationMode.Rotate)
        {
            float angle = _animationTime * _def.RotationSpeed * MathHelper.Pi / 180f;
            float cos = MathF.Cos(angle), sin = MathF.Sin(angle);
            fieldValue = new Vector2(
                fieldValue.X * cos - fieldValue.Y * sin,
                fieldValue.X * sin + fieldValue.Y * cos);
        }
        
        return fieldValue;
    }
    
    private Vector2 CalculateUV(Vector2 worldPos)
    {
        Vector2 uv = new Vector2(worldPos.X / _def.Tiling.X, worldPos.Y / _def.Tiling.Y) + _def.Offset;
        
        if (_def.Animation == VectorFieldDefinition.AnimationMode.Scroll)
            uv.X += _animationTime * _def.ScrollSpeed;
        
        return new Vector2(uv.X % 1f, uv.Y % 1f);
    }
    
    private Vector2 SampleCpu(Vector2 uv)
    {
        int x = (int)(uv.X * _width) % _width;
        int y = (int)(uv.Y * _height) % _height;
        var color = _cachedData![y * _width + x];
        
        return new Vector2(
            (color.R / 255f) * 2 - 1,
            (color.G / 255f) * 2 - 1);
    }
    
    private Vector2 SampleGpu(Vector2 uv) => Vector2.Zero; // Placeholder
}
