using GameEngine.Math;
using Microsoft.Xna.Framework;

namespace GameEngine.Cameras.Behaviors;

public class ShakeBehavior : AbstractCameraBehaviorBase
{
    public override int Priority => 90;
        
    public float Intensity { get; set; } = 10f;
    public float Frequency { get; set; } = 20f;
        
    private float _currentTrauma;
    private Vector2 _originalPosition;
    private Random _random;
    private float _seed;

    public ShakeBehavior(AbstractCameraBase camera, float duration, float intensity) : base(camera)
    {
        Duration = duration;
        Intensity = intensity;
        _random = new Random();
        _seed = (float)_random.NextDouble() * 100f;
        _originalPosition = camera.State.Transform.Position;
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (ElapsedTime == 0f)
        {
            _originalPosition = Camera.State.Transform.Position;
        }
            
        float t = ElapsedTime / Duration.Value;
        _currentTrauma = MathHelper.Clamp(1f - t, 0, 1);
        _currentTrauma = _currentTrauma * _currentTrauma;
            
        float shake = _currentTrauma * Intensity;
            
        float noiseX = PerlinNoise(_seed + ElapsedTime * Frequency);
        float noiseY = PerlinNoise(_seed + 100f + ElapsedTime * Frequency);
            
        float offsetX = noiseX * shake;
        float offsetY = noiseY * shake;
            
        Camera.SetPosition(_originalPosition + new Vector2(offsetX, offsetY));
    }

    protected override void OnComplete()
    {
        Camera.SetPosition(_originalPosition);
    }

    private float PerlinNoise(float x)
    {
        return (float)(System.Math.Sin(x) * 0.5 + System.Math.Sin(x * 2.3) * 0.25 + System.Math.Sin(x * 4.7) * 0.125);
    }

    public override void Reset()
    {
        base.Reset();
        _seed = (float)_random.NextDouble() * 100f;
    }
}