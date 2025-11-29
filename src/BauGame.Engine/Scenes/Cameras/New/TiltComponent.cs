using Microsoft.Xna.Framework;

public class TiltComponent : ICamaraComponent
{
    private float _targetTilt;
    private float _currentTilt;
    private float _decay = 3f; // velocidad de retorno

    public void Tilt(float angleRadians, float duration = 0.5f)
    {
        _targetTilt = angleRadians;
        // Opcional: usar un temporizador; aquí usamos spring-back
    }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        // Mover hacia el objetivo
        _currentTilt = MathHelper.Lerp(_currentTilt, _targetTilt, 0.1f);
        // Decaer el objetivo hacia 0
        _targetTilt = MathHelper.Lerp(_targetTilt, 0f, dt * _decay);
        // Aplicar
        camera.Rotation = _currentTilt;
    }

    public void Reset() => _targetTilt = _currentTilt = 0f;
}

using Microsoft.Xna.Framework;

public class ZoomPulseComponent : ICamaraComponent
{
    private float _baseZoom;
    private float _amplitude = 0.1f;
    private float _frequency = 5f;
    private float _timer;
    private bool _active;

    public void Start(float amplitude = 0.1f, float frequency = 5f, float duration = 2f)
    {
        _amplitude = amplitude;
        _frequency = frequency;
        _timer = duration;
        _active = true;
    }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (!_active) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _timer -= dt;
        if (_timer <= 0)
        {
            _active = false;
            camera.Zoom = _baseZoom;
            return;
        }

        // Guardar zoom base la primera vez
        if (camera.Zoom != _baseZoom && _baseZoom == 0) _baseZoom = camera.Zoom;

        float offset = (float)Math.Sin(_timer * _frequency) * _amplitude;
        camera.Zoom = _baseZoom + offset;
    }

    public void Reset()
    {
        _active = false;
        _baseZoom = 0;
    }
}
using Microsoft.Xna.Framework;

public class RadialWobbleComponent : ICamaraComponent
{
    private Vector2 _center;
    private float _amplitude;
    private float _duration;
    private float _timer;
    private bool _active;
    private Vector2 _offset;

    public void Trigger(Vector2 worldCenter, float amplitude, float duration)
    {
        _center = worldCenter;
        _amplitude = amplitude;
        _duration = duration;
        _timer = 0f;
        _active = true;
    }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (!_active) return;

        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        float progress = MathHelper.Clamp(_timer / _duration, 0f, 1f);
        float currentAmp = _amplitude * (1f - progress);

        // Dirección desde el centro de la onda a la cámara
        Vector2 direction = camera.Position - _center;
        if (direction.LengthSquared() > 0.1f)
        {
            direction.Normalize();
            _offset = direction * currentAmp;
        }
        else
        {
            _offset = Vector2.Zero;
        }

        if (progress >= 1f) _active = false;
    }

    public Vector2 GetOffset() => _offset;
    public void Reset() => _active = false;
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ScreenOverlayComponent : ICamaraDrawableComponent
{
    public Color OverlayColor { get; set; } = Color.Black * 0.3f;
    public bool Active { get; set; } = false;

    public void Update(GameTime gameTime, Camera2D camera) { }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd)
    {
        if (!Active) return;
        var rect = new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height);
        spriteBatch.Draw(PixelTexture.Instance, rect, OverlayColor);
    }

    public void Reset() => Active = false;
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class VignetteComponent : ICamaraDrawableComponent
{
    public float Intensity { get; set; } = 0.7f;
    public bool Active { get; set; } = true;

    public void Update(GameTime gameTime, Camera2D camera) { }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd)
    {
        if (!Active) return;

        int w = gd.Viewport.Width;
        int h = gd.Viewport.Height;
        float centerX = w * 0.5f;
        float centerY = h * 0.5f;
        float maxDist = (float)Math.Sqrt(centerX * centerX + centerY * centerY);

        // Dibujar un círculo grande con alpha creciente hacia los bordes
        Color color = Color.Black * Intensity;
        spriteBatch.Draw(PixelTexture.Instance, new Vector2(centerX, centerY), null, color, 0f, Vector2.Zero, maxDist * 2, SpriteEffects.None, 0f);
    }

    public void Reset() => Active = false;
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ScanlineComponent : ICamaraDrawableComponent
{
    private Texture2D _scanlineTexture;
    private float _scroll;
    public float Speed { get; set; } = 50f;
    public float Alpha { get; set; } = 0.3f;
    public bool Active { get; set; } = true;

    public void LoadContent(GraphicsDevice gd)
    {
        // Generar textura de scanlines (4px alto: blanco, negro, blanco, negro)
        _scanlineTexture = new Texture2D(gd, 1, 4);
        var data = new Color[4];
        data[0] = Color.White * Alpha;
        data[1] = Color.Transparent;
        data[2] = Color.White * Alpha;
        data[3] = Color.Transparent;
        _scanlineTexture.SetData(data);
    }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (Active)
            _scroll += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd)
    {
        if (!Active || _scanlineTexture == null) return;
        spriteBatch.Draw(_scanlineTexture,
            new Rectangle(0, (int)(_scroll % gd.Viewport.Height), gd.Viewport.Width, gd.Viewport.Height),
            null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
    }

    public void Reset() => Active = false;
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NoiseComponent : ICamaraDrawableComponent
{
    private Texture2D _noiseTexture;
    private float _time;
    public float Speed { get; set; } = 10f;
    public float Alpha { get; set; } = 0.15f;
    public bool Active { get; set; } = true;

    public void LoadContent(GraphicsDevice gd)
    {
        // Generar textura de ruido procedural (64x64)
        var size = 64;
        _noiseTexture = new Texture2D(gd, size, size);
        var data = new Color[size * size];
        var rand = new System.Random();
        for (int i = 0; i < data.Length; i++)
        {
            float v = (float)rand.NextDouble();
            data[i] = new Color(v, v, v) * Alpha;
        }
        _noiseTexture.SetData(data);
    }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (Active)
            _time += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd)
    {
        if (!Active || _noiseTexture == null) return;
        spriteBatch.Draw(_noiseTexture,
            new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height),
            null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
    }

    public void Reset() => Active = false;
}
