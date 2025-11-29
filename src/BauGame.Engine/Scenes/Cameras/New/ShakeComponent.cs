using Microsoft.Xna.Framework;
using System;

public class ShakeComponent : ICamaraComponent
{
    private float _intensity;
    private float _duration;
    private float _timer;
    private Vector2 _offset = Vector2.Zero;
    private readonly Random _random = new Random();

    public bool IsActive => _timer < _duration;

    public void Start(float intensity, float duration)
    {
        _intensity = intensity;
        _duration = duration;
        _timer = 0f;
    }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (_timer >= _duration) return;

        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        float progress = MathHelper.Clamp(_timer / _duration, 0f, 1f);
        float currentIntensity = _intensity * (1f - progress);

        _offset.X = (float)(_random.NextDouble() * 2.0 - 1.0) * currentIntensity;
        _offset.Y = (float)(_random.NextDouble() * 2.0 - 1.0) * currentIntensity;
    }

    public Vector2 GetOffset() => _offset;

    public void Reset()
    {
        _timer = _duration;
        _offset = Vector2.Zero;
    }
}
