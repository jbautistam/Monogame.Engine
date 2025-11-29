// En Camera2D.cs

// Hacer _desiredPosition accesible internamente (o mediante método protegido)
internal Vector2 _desiredPosition; // ya existía, pero asegúrate de que sea internal

// Nuevo: modo actual
private ICameraMode _currentMode;
private bool _usePhysics = true; // si false, la cámara se mueve instantáneamente

public void SetMode(ICameraMode mode, bool usePhysics = true)
{
    _currentMode?.Reset();
    _currentMode = mode;
    _usePhysics = usePhysics;
}

public ICameraMode CurrentMode => _currentMode;

// En Update, reemplaza toda la lógica de seguimiento por:
public void Update(GameTime gameTime)
{
    float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
    dt = MathHelper.Clamp(dt, 0f, 1f / 10f);

    // Delegar en el modo actual
    _currentMode?.Update(gameTime, this);

    // Aplicar física solo si está habilitada
    if (_usePhysics)
    {
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
    }
    else
    {
        // Movimiento instantáneo (útil en cinemáticas precisas)
        _position = _desiredPosition;
        _velocity = Vector2.Zero;
    }

    // Actualizar componentes
    foreach (var comp in _components.ToList())
        comp.Update(gameTime, this);
}
