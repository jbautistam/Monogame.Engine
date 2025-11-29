namespace Bau.Libraries.BauGame.Engine.Scenes.Timers;

/// <summary>
///     Manager para temporizadores
/// </summary>
public class TimerManager(AbstractScene scene)
{
    // Variables privadas
    private List<TimerModel> _timers = [];

    /// <summary>
    ///     Crea un temporizador
    /// </summary>
    public TimerModel Create(float duration, Action onComplete, bool repeat, bool start)
    {
        TimerModel timer = new(duration, onComplete, repeat);

            // Añade el temporizador a la lista
            _timers.Add(timer);
            // Arranca el temporizador si es necesario
            if (start)
                timer.Start();
            // Devuelve el temporizador creado
            return timer;
    }

    /// <summary>
    ///     Crea un temporizador para que se ejecute tras x segundos
    /// </summary>
    public TimerModel InvokeAfter(float delay, Action action, bool start) => Create(delay, action, false, true);

    /// <summary>
    ///     Crea un temporizador para que se ejecute repetidamente tras x segundos
    /// </summary>
    public TimerModel RepeatEvery(float interval, Action action) => Create(interval, action, true, true);

    /// <summary>
    ///     Actualiza los temporizadores
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        // Actualizamos todos los timers activos
        foreach (TimerModel timer in _timers)
            timer.Update(gameContext);
        // Elimina los temporizadores finalizados
        for (int index = _timers.Count - 1; index >= 0; index--)
            if (!_timers[index].IsRunning && !_timers[index].Repeat)
                _timers.RemoveAt(index);
    }

    /// <summary>
    ///     Escena a la que se asocia el manager
    /// </summary>
    public AbstractScene Scene { get; } = scene;
}
/*
## ✅ Ejemplos de uso

### Ejemplo 1: Delay simple
```csharp
TimerManager.InvokeAfter(2f, () => UIManager.ShowMessage("¡Listo!"));
```

### Ejemplo 2: Cooldown de habilidad
```csharp
private Timer _dashCooldown;

void Start()
{
    _dashCooldown = TimerManager.Create(1.0f, null, repeat: false);
}

void Update()
{
    if (InputManager.IsPressed("Dash") && !_dashCooldown.IsRunning)
    {
        Dash();
        _dashCooldown.Reset();
        _dashCooldown.Start();
    }
}
```

### Ejemplo 3: Slow motion
```csharp
// Activar slow motion al hacer daño
TimeManager.SetTimeScale(0.3f);
TimerManager.InvokeAfter(0.5f, () => TimeManager.SetTimeScale(1f));
*/
