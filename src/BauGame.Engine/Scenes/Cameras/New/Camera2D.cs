using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

public interface ICamaraComponent
{
    void Update(GameTime gameTime, Camera2D camera);
    void Reset();
}

public class Camera2D
{
    private readonly GraphicsDevice _graphicsDevice;
    private Vector2 _position;
    private Vector2 _velocity;
    private Vector2 _desiredPosition;
    private readonly List<Vector2> _targets = new List<Vector2>();
    private readonly List<ICamaraComponent> _components = new List<ICamaraComponent>();

    public float Acceleration = 80f;
    public float Damping = 0.88f;
    public bool UseClamp = true;
    public Rectangle? WorldBounds { get; set; } = null;
    public Rectangle? DeadZone { get; set; } = null;

    private float _zoom = 1f;
    public float Zoom
    {
        get => _zoom;
        set => _zoom = MathHelper.Max(value, 0.1f);
    }
    public float Rotation { get; set; } = 0f;

    public Vector2 Position => _position;
    public Vector2 ViewportCenter => new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
    public int ViewportWidth => _graphicsDevice.Viewport.Width;
    public int ViewportHeight => _graphicsDevice.Viewport.Height;

    public Camera2D(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
        _position = Vector2.Zero;
        _velocity = Vector2.Zero;
        _desiredPosition = Vector2.Zero;
    }

    public void AddTarget(Vector2 target) => _targets.Add(target);
    public void ClearTargets() => _targets.Clear();

    public T AddComponent<T>() where T : ICamaraComponent, new()
    {
        var comp = new T();
        _components.Add(comp);
        return comp;
    }

    public T GetComponent<T>() where T : ICamaraComponent => _components.OfType<T>().FirstOrDefault();

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        dt = MathHelper.Clamp(dt, 0f, 1f / 10f);

        if (_targets.Count == 0)
        {
            _desiredPosition = _position;
        }
        else
        {
            Vector2 targetCenter = _targets.Aggregate(Vector2.Zero, (sum, t) => sum + t) / _targets.Count;

            if (DeadZone.HasValue)
            {
                Vector2 screenTarget = WorldToScreenPure(targetCenter);
                Rectangle deadRect = DeadZone.Value;
                Vector2 deadCenter = new Vector2(deadRect.Center.X, deadRect.Center.Y);
                Vector2 offset = screenTarget - deadCenter;

                if (Math.Abs(offset.X) > deadRect.Width * 0.5f || Math.Abs(offset.Y) > deadRect.Height * 0.5f)
                {
                    _desiredPosition = targetCenter - ScreenToWorldPure(deadCenter);
                }
                else
                {
                    _desiredPosition = _position;
                }
            }
            else
            {
                _desiredPosition = targetCenter;
            }
        }

        if (UseClamp && WorldBounds.HasValue)
        {
            var bounds = WorldBounds.Value;
            var halfView = new Vector2(ViewportWidth / (2f * Zoom), ViewportHeight / (2f * Zoom));

            if (bounds.Width > ViewportWidth / Zoom)
                _desiredPosition.X = MathHelper.Clamp(_desiredPosition.X, bounds.Left + halfView.X, bounds.Right - halfView.X);
            else
                _desiredPosition.X = bounds.Center.X;

            if (bounds.Height > ViewportHeight / Zoom)
                _desiredPosition.Y = MathHelper.Clamp(_desiredPosition.Y, bounds.Top + halfView.Y, bounds.Bottom - halfView.Y);
            else
                _desiredPosition.Y = bounds.Center.Y;
        }

        Vector2 error = _desiredPosition - _position;
        Vector2 acceleration = error * Acceleration;
        _velocity += acceleration * dt;
        _velocity *= MathHelper.Clamp(Damping, 0f, 0.99f);
        _position += _velocity * dt;

        if (error.LengthSquared() < 0.1f && _velocity.LengthSquared() < 0.01f)
        {
            _position = _desiredPosition;
            _velocity = Vector2.Zero;
        }

        foreach (var comp in _components.ToList())
            comp.Update(gameTime, this);
    }

    private Matrix GetPureTransformMatrix() =>
        Matrix.CreateTranslation(-_position.X, -_position.Y, 0f) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(Zoom, Zoom, 1f) *
        Matrix.CreateTranslation(ViewportCenter.X, ViewportCenter.Y, 0f);

    public Vector2 WorldToScreenPure(Vector2 world) => Vector2.Transform(world, GetPureTransformMatrix());
    public Vector2 ScreenToWorldPure(Vector2 screen) => Vector2.Transform(screen, Matrix.Invert(GetPureTransformMatrix()));

    public Matrix GetFinalTransformMatrix()
    {
        Vector2 offset = Vector2.Zero;
        foreach (var comp in _components)
        {
            if (comp is ShakeComponent shake)
                offset += shake.GetOffset();
        }

        return Matrix.CreateTranslation(-_position.X - offset.X, -_position.Y - offset.Y, 0f) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(Zoom, Zoom, 1f) *
               Matrix.CreateTranslation(ViewportCenter.X, ViewportCenter.Y, 0f);
    }

    public bool IsPointInView(Vector2 worldPoint)
    {
        var screen = WorldToScreenPure(worldPoint);
        return new Rectangle(0, 0, ViewportWidth, ViewportHeight).Contains((int)screen.X, (int)screen.Y);
    }
}
