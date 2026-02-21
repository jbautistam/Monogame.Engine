User: Eres un programador especializado en monogame. Estás creando un motor para videojuegos. Tienes un sistema de escenas que controla las diferentes partes del juego y los actores. Tienes preparado componentes para la interface de usuario. Quieres añadir a la escena la posibilidad de abrir ventanas popup o cuadros de diálogo. Estos cuadros de diálogo pueden apilarse. Los actores de la escena deben detenerse si el popu que se abre es un cuadro de diálogo pero continuar actualizándose si no es así
Kimi: Voy a diseñar un sistema de gestión de ventanas popup/dialogos para tu motor de MonoGame. Este sistema permitirá apilar ventanas, pausar actores según el tipo de popup, y mantener una arquitectura limpia y extensible.

## Arquitectura del Sistema de Popups

```csharp
// ==================== ENUMS ====================

public enum PopupType
{
    /// <summary>Popup que pausa el juego (diálogos, menús de pausa)</summary>
    Modal,
    /// <summary>Popup que no pausa el juego (notificaciones, HUD flotante)</summary>
    NonModal
}

public enum PopupResult
{
    None,
    Confirmed,
    Cancelled,
    Closed
}

// ==================== INTERFACES ====================

public interface IPopup
{
    string Id { get; }
    PopupType Type { get; }
    bool IsActive { get; }
    bool IsClosing { get; }
    
    void Open();
    void Close(PopupResult result = PopupResult.Closed);
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    
    event EventHandler<PopupClosedEventArgs> OnClosed;
}

public interface IPopupManager
{
    IReadOnlyCollection<IPopup> ActivePopups { get; }
    bool HasModalPopup { get; }
    
    void Push(IPopup popup);
    bool Pop(string popupId = null);
    void ClearAll();
    
    IPopup GetTopPopup();
    IEnumerable<IPopup> GetPopupsOfType(PopupType type);
}

// ==================== EVENT ARGS ====================

public class PopupClosedEventArgs : EventArgs
{
    public string PopupId { get; set; }
    public PopupResult Result { get; set; }
    public object Data { get; set; }
}

// ==================== IMPLEMENTACIÓN BASE ====================

public abstract class PopupBase : IPopup
{
    protected readonly UIManager UIManager;
    protected readonly List<UIComponent> Components = new();
    
    public string Id { get; }
    public abstract PopupType Type { get; }
    public bool IsActive { get; protected set; }
    public bool IsClosing { get; protected set; }
    
    public event EventHandler<PopupClosedEventArgs> OnClosed;
    
    protected PopupBase(string id, UIManager uiManager)
    {
        Id = id ?? Guid.NewGuid().ToString();
        UIManager = uiManager;
    }
    
    public virtual void Open()
    {
        IsActive = true;
        IsClosing = false;
        OnOpened();
    }
    
    protected virtual void OnOpened() { }
    
    public virtual void Close(PopupResult result = PopupResult.Closed)
    {
        if (IsClosing) return;
        
        IsClosing = true;
        OnClosing(result);
        
        // Animación de cierre opcional aquí
        
        IsActive = false;
        OnClosed?.Invoke(this, new PopupClosedEventArgs 
        { 
            PopupId = Id, 
            Result = result 
        });
    }
    
    protected virtual void OnClosing(PopupResult result) { }
    
    public virtual void Update(GameTime gameTime)
    {
        if (!IsActive || IsClosing) return;
        
        // Actualizar componentes UI internos
        foreach (var component in Components)
        {
            component.Update(gameTime);
        }
    }
    
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;
        
        // Dibujar fondo oscurecido si es modal
        if (Type == PopupType.Modal)
        {
            DrawModalBackground(spriteBatch);
        }
        
        // Dibujar contenedor del popup
        DrawPopupContainer(spriteBatch);
        
        // Dibujar componentes
        foreach (var component in Components)
        {
            component.Draw(spriteBatch);
        }
    }
    
    protected virtual void DrawModalBackground(SpriteBatch spriteBatch)
    {
        // Dibujar overlay semitransparente
        var viewport = spriteBatch.GraphicsDevice.Viewport;
        var texture = UIManager.GetTexture("pixel"); // Textura 1x1 blanca
        spriteBatch.Draw(texture, new Rectangle(0, 0, viewport.Width, viewport.Height), 
            Color.Black * 0.6f);
    }
    
    protected abstract void DrawPopupContainer(SpriteBatch spriteBatch);
    
    protected void AddComponent(UIComponent component)
    {
        Components.Add(component);
    }
}

// ==================== GESTOR DE POPUPS ====================

public class PopupManager : IPopupManager, IUpdateable, IDrawable
{
    private readonly Stack<IPopup> _popupStack = new();
    private readonly List<IPopup> _pendingRemoval = new();
    private readonly Scene _ownerScene;
    
    public IReadOnlyCollection<IPopup> ActivePopups => _popupStack.ToList().AsReadOnly();
    public bool HasModalPopup => _popupStack.Any(p => p.Type == PopupType.Modal && p.IsActive);
    public int Count => _popupStack.Count;
    
    public event EventHandler<PopupStackChangedEventArgs> OnStackChanged;
    
    public PopupManager(Scene ownerScene)
    {
        _ownerScene = ownerScene;
    }
    
    /// <summary>
    /// Añade un popup a la pila y lo abre
    /// </summary>
    public void Push(IPopup popup)
    {
        if (popup == null) throw new ArgumentNullException(nameof(popup));
        
        // Si el popup en la cima es modal, no permitir abrir non-modal encima
        // (opcional, depende de tu diseño)
        var top = GetTopPopup();
        if (top?.Type == PopupType.Modal && popup.Type == PopupType.NonModal)
        {
            Debug.WriteLine("Warning: Intentando abrir non-modal sobre modal");
            // O lanzar excepción, o permitirlo según diseño
        }
        
        _popupStack.Push(popup);
        popup.OnClosed += OnPopupClosed;
        popup.Open();
        
        OnStackChanged?.Invoke(this, new PopupStackChangedEventArgs
        {
            Action = StackAction.Push,
            Popup = popup,
            StackSize = _popupStack.Count
        });
        
        // Notificar a la escena del cambio de estado modal
        _ownerScene.OnPopupStateChanged();
    }
    
    /// <summary>
    /// Cierra el popup superior o uno específico por ID
    /// </summary>
    public bool Pop(string popupId = null)
    {
        if (_popupStack.Count == 0) return false;
        
        IPopup target;
        
        if (popupId == null)
        {
            // Cerrar el más reciente
            target = _popupStack.Pop();
            target.OnClosed -= OnPopupClosed;
            target.Close();
        }
        else
        {
            // Buscar y cerrar específico (puede estar en cualquier posición)
            var tempStack = new Stack<IPopup>();
            target = null;
            
            while (_popupStack.Count > 0)
            {
                var popup = _popupStack.Pop();
                if (popup.Id == popupId)
                {
                    target = popup;
                    popup.OnClosed -= OnPopupClosed;
                    popup.Close();
                    break;
                }
                tempStack.Push(popup);
            }
            
            // Restaurar los que no eran el target
            while (tempStack.Count > 0)
            {
                _popupStack.Push(tempStack.Pop());
            }
            
            if (target == null) return false;
        }
        
        OnStackChanged?.Invoke(this, new PopupStackChangedEventArgs
        {
            Action = StackAction.Pop,
            Popup = target,
            StackSize = _popupStack.Count
        });
        
        _ownerScene.OnPopupStateChanged();
        return true;
    }
    
    /// <summary>
    /// Cierra todos los popups
    /// </summary>
    public void ClearAll()
    {
        while (_popupStack.Count > 0)
        {
            var popup = _popupStack.Pop();
            popup.OnClosed -= OnPopupClosed;
            popup.Close(PopupResult.Closed);
        }
        
        OnStackChanged?.Invoke(this, new PopupStackChangedEventArgs
        {
            Action = StackAction.Clear,
            StackSize = 0
        });
        
        _ownerScene.OnPopupStateChanged();
    }
    
    public IPopup GetTopPopup() => _popupStack.Count > 0 ? _popupStack.Peek() : null;
    
    public IEnumerable<IPopup> GetPopupsOfType(PopupType type) => 
        _popupStack.Where(p => p.Type == type);
    
    private void OnPopupClosed(object sender, PopupClosedEventArgs e)
    {
        // Limpiar referencias si es necesario
    }
    
    public void Update(GameTime gameTime)
    {
        // Actualizar desde el fondo hacia arriba
        // Los popups no-modales pueden actualizarse todos
        // Los modales solo el de arriba recibe input, pero todos se dibujan
        
        var popups = _popupStack.ToArray(); // Copia inversa (el primero es el fondo)
        
        for (int i = popups.Length - 1; i >= 0; i--)
        {
            var popup = popups[i];
            
            // Solo el popup superior recibe input si es modal
            if (popup.Type == PopupType.Modal && i < popups.Length - 1)
            {
                // Dibujar pero no actualizar input
                popup.Update(gameTime); // Actualización visual, no de input
            }
            else
            {
                popup.Update(gameTime);
            }
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Dibujar desde el fondo hacia arriba
        var popups = _popupStack.ToArray();
        for (int i = popups.Length - 1; i >= 0; i--)
        {
            popups[i].Draw(spriteBatch);
        }
    }
}

public enum StackAction { Push, Pop, Clear }
public class PopupStackChangedEventArgs : EventArgs
{
    public StackAction Action { get; set; }
    public IPopup Popup { get; set; }
    public int StackSize { get; set; }
}

// ==================== ESCENA CON SOPORTE DE POPUPS ====================

public abstract class Scene
{
    protected readonly Game Game;
    public PopupManager PopupManager { get; private set; }
    
    private readonly List<Actor> _actors = new();
    private readonly List<Actor> _pendingAdd = new();
    private readonly List<Actor> _pendingRemove = new();
    
    public bool IsPaused { get; private set; }
    public bool IsActive { get; set; }
    
    protected Scene(Game game)
    {
        Game = game;
        PopupManager = new PopupManager(this);
    }
    
    /// <summary>
    /// Llamado por PopupManager cuando cambia el estado de los popups
    /// </summary>
    internal void OnPopupStateChanged()
    {
        UpdatePauseState();
    }
    
    /// <summary>
    /// Determina si la escena debe pausarse según los popups activos
    /// </summary>
    private void UpdatePauseState()
    {
        bool shouldPause = PopupManager.HasModalPopup;
        
        if (shouldPause != IsPaused)
        {
            IsPaused = shouldPause;
            
            if (IsPaused)
                OnPaused();
            else
                OnResumed();
        }
    }
    
    protected virtual void OnPaused()
    {
        // Notificar a actores que el juego está pausado
        foreach (var actor in _actors)
        {
            actor.OnGamePaused();
        }
    }
    
    protected virtual void OnResumed()
    {
        foreach (var actor in _actors)
        {
            actor.OnGameResumed();
        }
    }
    
    public virtual void Update(GameTime gameTime)
    {
        // Procesar cambios pendientes de actores
        ProcessPendingChanges();
        
        // Actualizar popups primero (input priority)
        PopupManager.Update(gameTime);
        
        // Actualizar actores solo si no hay popup modal
        if (!IsPaused)
        {
            UpdateActors(gameTime);
        }
        else
        {
            // Opcional: actualizar actores con deltaTime = 0 o marcados como "updateWhenPaused"
            UpdateActorsPaused(gameTime);
        }
    }
    
    private void UpdateActors(GameTime gameTime)
    {
        foreach (var actor in _actors.Where(a => a.IsActive))
        {
            actor.Update(gameTime);
        }
    }
    
    private void UpdateActorsPaused(GameTime gameTime)
    {
        foreach (var actor in _actors.Where(a => a.IsActive && a.UpdateWhenPaused))
        {
            actor.Update(gameTime);
        }
    }
    
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // Dibujar escena (siempre, incluso con popups modales de fondo)
        DrawScene(spriteBatch);
        
        // Dibujar actores
        foreach (var actor in _actors.Where(a => a.IsVisible).OrderBy(a => a.DrawOrder))
        {
            actor.Draw(spriteBatch);
        }
        
        // Dibujar popups encima de todo
        PopupManager.Draw(spriteBatch);
    }
    
    protected abstract void DrawScene(SpriteBatch spriteBatch);
    
    // Gestión de actores...
    public void AddActor(Actor actor) => _pendingAdd.Add(actor);
    public void RemoveActor(Actor actor) => _pendingRemove.Add(actor);
    private void ProcessPendingChanges() { /* implementación */ }
}

// ==================== IMPLEMENTACIONES CONCRETAS DE POPUPS ====================

public class DialogBox : PopupBase
{
    public override PopupType Type => PopupType.Modal;
    
    private string _title;
    private string _message;
    private Rectangle _bounds;
    private Texture2D _backgroundTexture;
    
    public DialogBox(string id, UIManager uiManager, string title, string message) 
        : base(id, uiManager)
    {
        _title = title;
        _message = message;
        InitializeComponents();
    }
    
    private void InitializeComponents()
    {
        // Botón Aceptar
        var btnAccept = new Button("btnAccept", "Aceptar", UIManager);
        btnAccept.OnClick += () => Close(PopupResult.Confirmed);
        AddComponent(btnAccept);
        
        // Botón Cancelar
        var btnCancel = new Button("btnCancel", "Cancelar", UIManager);
        btnCancel.OnClick += () => Close(PopupResult.Cancelled);
        AddComponent(btnCancel);
        
        // Layout automático
        RecalculateLayout();
    }
    
    private void RecalculateLayout()
    {
        var viewport = UIManager.GraphicsDevice.Viewport;
        int width = 400;
        int height = 200;
        _bounds = new Rectangle(
            (viewport.Width - width) / 2,
            (viewport.Height - height) / 2,
            width, 
            height
        );
        
        // Posicionar botones...
    }
    
    protected override void DrawPopupContainer(SpriteBatch spriteBatch)
    {
        // Fondo del diálogo
        spriteBatch.Draw(_backgroundTexture ?? UIManager.GetTexture("dialog_bg"), 
            _bounds, Color.White);
        
        // Título
        UIManager.DrawString(_title, new Vector2(_bounds.X + 20, _bounds.Y + 20), 
            Color.Black, scale: 1.2f);
        
        // Mensaje
        UIManager.DrawString(_message, new Vector2(_bounds.X + 20, _bounds.Y + 60), 
            Color.DarkGray, wrapWidth: _bounds.Width - 40);
    }
}

public class NotificationPopup : PopupBase
{
    public override PopupType Type => PopupType.NonModal;
    
    private float _lifetime;
    private float _elapsed;
    private string _message;
    private Vector2 _position;
    
    public NotificationPopup(string id, UIManager uiManager, string message, float duration = 3f) 
        : base(id, uiManager)
    {
        _message = message;
        _lifetime = duration;
        _position = new Vector2(20, 20); // Esquina superior izquierda
    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Auto-cerrar después del tiempo
        if (_elapsed >= _lifetime)
        {
            Close(PopupResult.Closed);
        }
        
        // Animación de entrada/salida
        float alpha = 1f;
        if (_elapsed < 0.3f) alpha = _elapsed / 0.3f;
        else if (_elapsed > _lifetime - 0.3f) alpha = (_lifetime - _elapsed) / 0.3f;
    }
    
    protected override void DrawPopupContainer(SpriteBatch spriteBatch)
    {
        // Dibujar notificación pequeña sin fondo modal
        var size = UIManager.MeasureString(_message);
        var bgRect = new Rectangle((int)_position.X, (int)_position.Y, 
            (int)size.X + 20, (int)size.Y + 10);
        
        spriteBatch.Draw(UIManager.GetTexture("pixel"), bgRect, Color.DarkBlue * 0.8f);
        UIManager.DrawString(_message, _position + new Vector2(10, 5), Color.White);
    }
}

// ==================== ACTOR CON CONCIENCIA DE PAUSA ====================

public abstract class Actor
{
    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;
    public int DrawOrder { get; set; }
    
    /// <summary>
    /// Si true, este actor se actualiza incluso cuando hay un popup modal
    /// Útil para: efectos de fondo, música, animaciones ambientales
    /// </summary>
    public bool UpdateWhenPaused { get; set; }
    
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
    
    public virtual void OnGamePaused() 
    {
        // Por defecto, detener animaciones de gameplay
    }
    
    public virtual void OnGameResumed()
    {
        // Reanudar animaciones
    }
}

// ==================== USO EJEMPLO ====================

public class GameplayScene : Scene
{
    private PlayerActor _player;
    private UIManager _uiManager;
    
    public GameplayScene(Game game) : base(game)
    {
        _uiManager = new UIManager(game.GraphicsDevice);
    }
    
    public void ShowPauseMenu()
    {
        var pauseMenu = new PauseMenuPopup("pause_menu", _uiManager);
        PopupManager.Push(pauseMenu);
        // Automáticamente: IsPaused = true, actores se detienen
    }
    
    public void ShowDialog(string npcName, string text)
    {
        var dialog = new DialogBox($"dialog_{npcName}", _uiManager, npcName, text);
        PopupManager.Push(dialog);
        // Automáticamente: IsPaused = true, actores se detienen
    }
    
    public void ShowNotification(string text)
    {
        var notif = new NotificationPopup($"notif_{Guid.NewGuid()}", _uiManager, text);
        PopupManager.Push(notif);
        // NO pausa el juego, actores continúan
    }
    
    public void OnPlayerLevelUp()
    {
        // Apilar popups: primero notificación, luego menú de habilidades
        ShowNotification("¡Nivel completado!");
        
        var skillMenu = new SkillSelectionPopup("levelup_skills", _uiManager);
        PopupManager.Push(skillMenu);
        // Ahora sí pausa porque skillMenu es Modal
    }
}
```

## Características Clave del Sistema

### 1. **Gestión de Pila (Stack)**
- Los popups se apilan LIFO (Last In, First Out)
- Se pueden cerrar el superior o uno específico por ID
- Eventos `OnStackChanged` para UI que reacciona a cambios

### 2. **Control de Pausa Automático**
- `HasModalPopup` determina si la escena debe pausarse
- `IsPaused` se actualiza automáticamente al abrir/cerrar popups
- Los actores pueden marcar `UpdateWhenPaused = true` para excepciones (partículas, música)

### 3. **Jerarquía de Renderizado**
1. Escena y actores (siempre visibles)
2. Popups apilados (del fondo hacia arriba)
3. Popup superior recibe input (si es modal)

### 4. **Extensibilidad**
- Hereda de `PopupBase` para nuevos tipos
- Implementa `IPopup` para comportamientos especiales
- Sistema de componentes UI reutilizable

¿Necesitas que profundice en algún aspecto específico, como el sistema de input (para que solo el popup superior reciba clicks), transiciones animadas, o serialización del estado de la pila?
User: Veamos el sistema de input
Kimi: Vamos a implementar un sistema de input robusto que maneje correctamente la jerarquía de popups, asegurando que solo el popup superior reciba interacciones cuando sea modal, pero permitiendo passthrough cuando sea necesario.

## Sistema de Input Jerárquico para Popups

```csharp
// ==================== ENUMS Y ESTADOS DE INPUT ====================

public enum InputLayerPriority
{
    Scene = 0,      // Escena base (gameplay)
    Background = 1, // Popups no modales en fondo
    Normal = 2,     // Popups estándar
    Modal = 3,      // Popups modales (bloquean inferior)
    System = 4,     // Siempre activo (atajos de teclado, debug)
    Debug = 5       // Herramientas de desarrollo
}

public enum InputConsumeType
{
    /// <summary>No consume el input, pasa al siguiente layer</summary>
    PassThrough,
    /// <summary>Consume el input, no pasa a layers inferiores</summary>
    Block,
    /// <summary>Consume solo si el input fue sobre este elemento</summary>
    BlockOnHit
}

// ==================== INTERFACES DE INPUT ====================

public interface IInputHandler
{
    InputLayerPriority Priority { get; }
    InputConsumeType ConsumeType { get; }
    bool IsActive { get; }
    Rectangle? InputBounds { get; } // Null = toda la pantalla
    
    /// <summary>Procesa input. Retorna true si consume el evento.</summary>
    bool HandleInput(InputEvent inputEvent);
}

public interface IInputManager
{
    void RegisterHandler(IInputHandler handler);
    void UnregisterHandler(IInputHandler handler);
    void PushModalContext(IInputHandler owner);
    void PopModalContext(IInputHandler owner);
    bool IsBlockedByModal(IInputHandler handler);
}

// ==================== EVENTOS DE INPUT UNIFICADOS ====================

public class InputEvent
{
    public InputType Type { get; }
    public Vector2 Position { get; } // Para mouse/touch
    public Keys? Key { get; }        // Para teclado
    public Buttons? Button { get; }  // Para gamepad
    public float Value { get; }      // Para ejes (triggers, sticks)
    public bool IsPressed { get; }
    public bool IsReleased { get; }
    public bool IsHeld { get; }
    public double Timestamp { get; }
    
    // Propiedades calculadas
    public bool IsMouse => Type == InputType.MouseLeft || Type == InputType.MouseRight || Type == InputType.MouseMiddle;
    public bool IsKeyboard => Key.HasValue;
    
    public InputEvent(InputType type, Vector2 position = default, Keys? key = null, 
        bool pressed = false, bool released = false)
    {
        Type = type;
        Position = position;
        Key = key;
        IsPressed = pressed;
        IsReleased = released;
        Timestamp = DateTime.UtcNow.Ticks;
    }
}

public enum InputType
{
    MouseLeft, MouseRight, MouseMiddle,
    MouseMove, MouseScroll,
    KeyDown, KeyUp, KeyHeld,
    GamepadA, GamepadB, GamepadX, GamepadY,
    GamepadStart, GamepadBack,
    LeftStick, RightStick,
    LeftTrigger, RightTrigger,
    DPadUp, DPadDown, DPadLeft, DPadRight,
    TouchTap, TouchDoubleTap, TouchDrag
}

// ==================== GESTOR DE INPUT CENTRALIZADO ====================

public class InputManager : IInputManager, IUpdateable
{
    private readonly List<IInputHandler> _handlers = new();
    private readonly Stack<IInputHandler> _modalStack = new();
    private readonly Game _game;
    private MouseState _lastMouseState;
    private KeyboardState _lastKeyboardState;
    private GamePadState _lastGamepadState;
    
    // Configuración
    public bool EnableDebugInput { get; set; } = false;
    public bool BlockSceneInputWhenModal { get; set; } = true;
    
    public InputManager(Game game)
    {
        _game = game;
    }
    
    public void Update(GameTime gameTime)
    {
        var currentMouse = Mouse.GetState();
        var currentKeyboard = Keyboard.GetState();
        var currentGamepad = GamePad.GetState(PlayerIndex.One);
        
        ProcessMouse(currentMouse, currentMouseState);
        ProcessKeyboard(currentKeyboard, currentKeyboardState);
        ProcessGamepad(currentGamepad, currentGamepadState);
        
        _lastMouseState = currentMouse;
        _lastKeyboardState = currentKeyboard;
        _lastGamepadState = currentGamepad;
    }
    
    private void ProcessMouse(MouseState current, MouseState last)
    {
        var position = new Vector2(current.X, current.Y);
        
        // Movimiento
        if (current.X != last.X || current.Y != last.Y)
        {
            PropagateInput(new InputEvent(InputType.MouseMove, position));
        }
        
        // Scroll
        if (current.ScrollWheelValue != last.ScrollWheelValue)
        {
            var scrollEvent = new InputEvent(InputType.MouseScroll, position, 
                value: (current.ScrollWheelValue - last.ScrollWheelValue) / 120f);
            PropagateInput(scrollEvent);
        }
        
        // Botones
        CheckButton(current.LeftButton, last.LeftButton, InputType.MouseLeft, position);
        CheckButton(current.RightButton, last.RightButton, InputType.MouseRight, position);
        CheckButton(current.MiddleButton, last.MiddleButton, InputType.MouseMiddle, position);
    }
    
    private void CheckButton(ButtonState current, ButtonState last, InputType type, Vector2 pos)
    {
        if (current == ButtonState.Pressed && last == ButtonState.Released)
            PropagateInput(new InputEvent(type, pos, pressed: true));
        else if (current == ButtonState.Released && last == ButtonState.Pressed)
            PropagateInput(new InputEvent(type, pos, released: true));
    }
    
    private void ProcessKeyboard(KeyboardState current, KeyboardState last)
    {
        // Teclas presionadas
        foreach (Keys key in Enum.GetValues(typeof(Keys)))
        {
            bool wasDown = last.IsKeyDown(key);
            bool isDown = current.IsKeyDown(key);
            
            if (!wasDown && isDown)
                PropagateInput(new InputEvent(InputType.KeyDown, key: key, pressed: true));
            else if (wasDown && !isDown)
                PropagateInput(new InputEvent(InputType.KeyUp, key: key, released: true));
            else if (wasDown && isDown)
                PropagateInput(new InputEvent(InputType.KeyHeld, key: key, isHeld: true));
        }
        
        // Atajos globales (System priority)
        HandleGlobalShortcuts(current, last);
    }
    
    private void HandleGlobalShortcuts(KeyboardState current, KeyboardState last)
    {
        // Ejemplo: F12 para screenshot, Escape para cerrar popup
        if (WasPressed(current, last, Keys.F12))
        {
            TakeScreenshot();
        }
    }
    
    private bool WasPressed(KeyboardState current, KeyboardState last, Keys key) =>
        current.IsKeyDown(key) && !last.IsKeyDown(key);
    
    // ==================== SISTEMA DE PROPAGACIÓN JERÁRQUICA ====================
    
    private void PropagateInput(InputEvent evt)
    {
        // Ordenar handlers por prioridad (mayor primero)
        var sortedHandlers = _handlers
            .Where(h => h.IsActive)
            .OrderByDescending(h => h.Priority)
            .ToList();
        
        // Verificar si hay contexto modal activo
        var modalOwner = _modalStack.Count > 0 ? _modalStack.Peek() : null;
        
        foreach (var handler in sortedHandlers)
        {
            // Si hay un modal activo, solo procesar:
            // 1. El propio modal
            // 2. Handlers de prioridad System o Debug
            // 3. Handlers explicitamente permitidos por el modal
            if (modalOwner != null && !CanProcessInModalContext(handler, modalOwner))
                continue;
            
            // Verificar bounds para input posicional
            if (evt.IsMouse && handler.InputBounds.HasValue)
            {
                if (!handler.InputBounds.Value.Contains(evt.Position))
                    continue; // Input fuera del área, probar siguiente handler
            }
            
            // Intentar manejar el input
            bool consumed = handler.HandleInput(evt);
            
            if (consumed && handler.ConsumeType == InputConsumeType.Block)
                return; // Input consumido, detener propagación
            
            if (consumed && handler.ConsumeType == InputConsumeType.BlockOnHit && 
                (!evt.IsMouse || IsOverBounds(evt.Position, handler.InputBounds)))
                return;
        }
    }
    
    private bool CanProcessInModalContext(IInputHandler handler, IInputHandler modalOwner)
    {
        // Siempre permitir system y debug
        if (handler.Priority >= InputLayerPriority.System)
            return true;
        
        // Permitir el propio modal
        if (handler == modalOwner)
            return true;
        
        // Verificar si el modal permite passthrough específico
        if (modalOwner is IModalInputController modalController)
        {
            return modalController.AllowsPassthrough(handler);
        }
        
        // Por defecto, bloquear todo excepto el modal
        return false;
    }
    
    private bool IsOverBounds(Vector2 position, Rectangle? bounds) =>
        bounds?.Contains(position) ?? true;
    
    // ==================== API PÚBLICA ====================
    
    public void RegisterHandler(IInputHandler handler)
    {
        if (!_handlers.Contains(handler))
            _handlers.Add(handler);
    }
    
    public void UnregisterHandler(IInputHandler handler)
    {
        _handlers.Remove(handler);
        // Limpiar de modal stack si estaba
        if (_modalStack.Contains(handler))
        {
            var temp = _modalStack.ToList();
            temp.Remove(handler);
            _modalStack.Clear();
            foreach (var h in temp) _modalStack.Push(h);
        }
    }
    
    public void PushModalContext(IInputHandler owner)
    {
        if (!_handlers.Contains(owner))
            throw new InvalidOperationException("Handler must be registered before pushing modal context");
        
        _modalStack.Push(owner);
    }
    
    public void PopModalContext(IInputHandler owner)
    {
        if (_modalStack.Count > 0 && _modalStack.Peek() == owner)
            _modalStack.Pop();
    }
    
    public bool IsBlockedByModal(IInputHandler handler) => !CanProcessInModalContext(handler, _modalStack.Peek());
}

// ==================== INTERFAZ PARA MODALES AVANZADOS ====================

public interface IModalInputController
{
    /// <summary>Determina si un handler específico puede recibir input debajo de este modal</summary>
    bool AllowsPassthrough(IInputHandler handler);
    
    /// <summary>Lista de handlers específicos permitidos (whitelist)</summary>
    IEnumerable<IInputHandler> AllowedHandlers { get; }
}

// ==================== IMPLEMENTACIÓN EN POPUPS ====================

public abstract class PopupBase : IPopup, IInputHandler, IModalInputController
{
    // ... (campos anteriores) ...
    
    // IInputHandler implementation
    public virtual InputLayerPriority Priority => Type == PopupType.Modal 
        ? InputLayerPriority.Modal 
        : InputLayerPriority.Normal;
    
    public virtual InputConsumeType ConsumeType => Type == PopupType.Modal 
        ? InputConsumeType.Block 
        : InputConsumeType.BlockOnHit;
    
    public bool IsActive => _isActive;
    public virtual Rectangle? InputBounds => _bounds;
    
    // IModalInputController (solo para modales)
    public virtual IEnumerable<IInputHandler> AllowedHandlers => Enumerable.Empty<IInputHandler>();
    
    private InputManager _inputManager;
    protected bool _isActive;
    protected Rectangle _bounds;
    
    public virtual bool AllowsPassthrough(IInputHandler handler)
    {
        // Por defecto, los modales no permiten passthrough
        // Override en clases derivadas para comportamiento específico
        return AllowedHandlers.Contains(handler);
    }
    
    public virtual bool HandleInput(InputEvent evt)
    {
        if (!IsActive) return false;
        
        // Delegar a componentes UI
        foreach (var component in Components.OfType<IInputHandler>())
        {
            if (component.HandleInput(evt))
                return true;
        }
        
        // Comportamiento por defecto: cerrar con Escape si es modal
        if (Type == PopupType.Modal && evt.IsKeyboard && evt.Key == Keys.Escape && evt.IsPressed)
        {
            Close(PopupResult.Cancelled);
            return true;
        }
        
        // Cerrar al hacer click fuera si es non-modal
        if (Type == PopupType.NonModal && evt.IsMouse && evt.IsPressed)
        {
            if (!InputBounds.Value.Contains(evt.Position))
            {
                Close();
                return true;
            }
        }
        
        return false;
    }
    
    public override void Open()
    {
        base.Open();
        _inputManager?.RegisterHandler(this);
        
        if (Type == PopupType.Modal)
        {
            _inputManager?.PushModalContext(this);
        }
    }
    
    public override void Close(PopupResult result = PopupResult.Closed)
    {
        if (Type == PopupType.Modal)
        {
            _inputManager?.PopModalContext(this);
        }
        _inputManager?.UnregisterHandler(this);
        
        base.Close(result);
    }
}

// ==================== COMPONENTES UI CON INPUT ====================

public abstract class UIComponent : IInputHandler
{
    public string Id { get; }
    public virtual InputLayerPriority Priority => InputLayerPriority.Normal;
    public virtual InputConsumeType ConsumeType => InputConsumeType.BlockOnHit;
    public bool IsActive { get; set; } = true;
    public virtual Rectangle? InputBounds => Bounds;
    
    protected Rectangle Bounds;
    protected bool IsHovered;
    protected bool IsPressed;
    
    public event Action OnClick;
    public event Action OnHoverStart;
    public event Action OnHoverEnd;
    
    public virtual bool HandleInput(InputEvent evt)
    {
        if (!IsActive) return false;
        
        bool isOver = InputBounds?.Contains(evt.Position) ?? false;
        
        // Hover detection
        if (evt.Type == InputType.MouseMove)
        {
            if (isOver && !IsHovered)
            {
                IsHovered = true;
                OnHoverStart?.Invoke();
                OnHover();
            }
            else if (!isOver && IsHovered)
            {
                IsHovered = false;
                IsPressed = false;
                OnHoverEnd?.Invoke();
                OnHoverEnd();
            }
        }
        
        // Click handling
        if (evt.Type == InputType.MouseLeft)
        {
            if (evt.IsPressed && isOver)
            {
                IsPressed = true;
                OnPressed();
                return ConsumeType != InputConsumeType.PassThrough;
            }
            else if (evt.IsReleased && IsPressed)
            {
                IsPressed = false;
                if (isOver)
                {
                    OnClick?.Invoke();
                    OnClicked();
                }
                OnReleased();
                return isOver; // Consume solo si fue sobre el elemento
            }
        }
        
        return false;
    }
    
    protected virtual void OnHover() { }
    protected virtual void OnHoverEnd() { }
    protected virtual void OnPressed() { }
    protected virtual void OnReleased() { }
    protected virtual void OnClicked() { }
}

// ==================== BOTÓN ESPECÍFICO ====================

public class Button : UIComponent
{
    public string Text { get; set; }
    public Color NormalColor { get; set; } = Color.Gray;
    public Color HoverColor { get; set; } = Color.LightGray;
    public Color PressedColor { get; set; } = Color.DarkGray;
    public Color TextColor { get; set; } = Color.White;
    
    private readonly UIManager _uiManager;
    
    public Button(string id, string text, UIManager uiManager, Rectangle bounds) : base(id)
    {
        Text = text;
        _uiManager = uiManager;
        Bounds = bounds;
    }
    
    protected override void OnClicked()
    {
        // Efecto de sonido
        _uiManager.PlaySound("button_click");
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        Color bgColor = IsPressed ? PressedColor : (IsHovered ? HoverColor : NormalColor);
        spriteBatch.Draw(_uiManager.GetTexture("pixel"), Bounds, bgColor);
        
        var textSize = _uiManager.MeasureString(Text);
        var textPos = new Vector2(
            Bounds.Center.X - textSize.X / 2,
            Bounds.Center.Y - textSize.Y / 2
        );
        _uiManager.DrawString(Text, textPos, TextColor);
    }
}

// ==================== SCROLLVIEW (EJEMPLO AVANZADO) ====================

public class ScrollView : UIComponent, IEnumerable<UIComponent>
{
    private readonly List<UIComponent> _children = new();
    private Vector2 _scrollOffset;
    private Vector2 _contentSize;
    private bool _isDragging;
    private Vector2 _dragStart;
    private Vector2 _scrollStart;
    
    public override InputConsumeType ConsumeType => InputConsumeType.BlockOnHit;
    
    public void Add(UIComponent child)
    {
        _children.Add(child);
        RecalculateContentSize();
    }
    
    public override bool HandleInput(InputEvent evt)
    {
        // Primero intentar scroll
        if (HandleScrollInput(evt))
            return true;
        
        // Luego delegar a hijos con offset aplicado
        var offsetEvent = new InputEvent(
            evt.Type,
            evt.Position - _scrollOffset, // Transformar coordenadas
            evt.Key,
            evt.IsPressed,
            evt.IsReleased
        );
        
        foreach (var child in _children)
        {
            if (child.HandleInput(offsetEvent))
                return true;
        }
        
        return base.HandleInput(evt);
    }
    
    private bool HandleScrollInput(InputEvent evt)
    {
        if (!IsHovered) return false;
        
        // Mouse wheel
        if (evt.Type == InputType.MouseScroll)
        {
            _scrollOffset.Y = MathHelper.Clamp(
                _scrollOffset.Y - evt.Value * 50,
                0,
                Math.Max(0, _contentSize.Y - Bounds.Height)
            );
            return true;
        }
        
        // Drag scrolling (touch/mouse)
        if (evt.Type == InputType.MouseLeft)
        {
            if (evt.IsPressed && InputBounds.Value.Contains(evt.Position))
            {
                _isDragging = true;
                _dragStart = evt.Position;
                _scrollStart = _scrollOffset;
                return true;
            }
            else if (evt.IsReleased && _isDragging)
            {
                _isDragging = false;
                return true;
            }
        }
        
        if (evt.Type == InputType.MouseMove && _isDragging)
        {
            var delta = evt.Position - _dragStart;
            _scrollOffset = _scrollStart - delta;
            _scrollOffset.X = MathHelper.Clamp(_scrollOffset.X, 0, 
                Math.Max(0, _contentSize.X - Bounds.Width));
            _scrollOffset.Y = MathHelper.Clamp(_scrollOffset.Y, 0, 
                Math.Max(0, _contentSize.Y - Bounds.Height));
            return true;
        }
        
        return false;
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        // Dibujar viewport con scissor rectangle
        var originalScissor = spriteBatch.GraphicsDevice.ScissorRectangle;
        spriteBatch.GraphicsDevice.ScissorRectangle = Bounds;
        
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
            null, null, new RasterizerState { ScissorTestEnable = true });
        
        foreach (var child in _children)
        {
            // Aplicar offset temporalmente
            var originalBounds = child.Bounds;
            child.Bounds = new Rectangle(
                originalBounds.X + (int)_scrollOffset.X,
                originalBounds.Y + (int)_scrollOffset.Y,
                originalBounds.Width,
                originalBounds.Height
            );
            
            child.Draw(spriteBatch);
            
            child.Bounds = originalBounds; // Restaurar
        }
        
        spriteBatch.End();
        spriteBatch.Begin();
        
        spriteBatch.GraphicsDevice.ScissorRectangle = originalScissor;
    }
    
    // IEnumerable implementation...
    public IEnumerator<UIComponent> GetEnumerator() => _children.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

// ==================== ESCENA CON INPUT INTEGRADO ====================

public abstract class Scene : IInputHandler
{
    // IInputHandler implementation
    public InputLayerPriority Priority => InputLayerPriority.Scene;
    public InputConsumeType ConsumeType => InputConsumeType.BlockOnHit;
    public bool IsActive { get; set; }
    public virtual Rectangle? InputBounds => null; // Toda la pantalla
    
    protected readonly InputManager InputManager;
    protected readonly List<Actor> _actors = new();
    
    protected Scene(Game game, InputManager inputManager)
    {
        InputManager = inputManager;
    }
    
    public virtual bool HandleInput(InputEvent evt)
    {
        if (!IsActive) return false;
        
        // Solo procesar input de escena si no está pausada por modal
        if (IsPaused) return false;
        
        // Delegar a actores
        foreach (var actor in _actors.OfType<IInputHandler>().OrderByDescending(a => a.Priority))
        {
            if (actor.HandleInput(evt))
                return true;
        }
        
        // Input de escena por defecto
        return HandleSceneInput(evt);
    }
    
    protected virtual bool HandleSceneInput(InputEvent evt)
    {
        // Override en escenas derivadas
        return false;
    }
}

// ==================== ACTOR CON INPUT ====================

public abstract class Actor : IInputHandler
{
    public virtual InputLayerPriority Priority => InputLayerPriority.Scene;
    public virtual InputConsumeType ConsumeType => InputConsumeType.BlockOnHit;
    public bool IsActive { get; set; } = true;
    public virtual Rectangle? InputBounds => null;
    
    public bool HandleInput(InputEvent evt)
    {
        if (!IsActive) return false;
        return OnHandleInput(evt);
    }
    
    protected abstract bool OnHandleInput(InputEvent evt);
}

// ==================== EJEMPLO: DRAG AND DROP ====================

public class DraggableItem : UIComponent
{
    private Vector2 _dragOffset;
    private bool _isDragging;
    private readonly Action<Vector2> _onPositionChanged;
    
    public DraggableItem(string id, Rectangle bounds, Action<Vector2> onPositionChanged) : base(id)
    {
        Bounds = bounds;
        _onPositionChanged = onPositionChanged;
    }
    
    public override bool HandleInput(InputEvent evt)
    {
        if (evt.Type == InputType.MouseLeft)
        {
            if (evt.IsPressed && InputBounds.Value.Contains(evt.Position))
            {
                _isDragging = true;
                _dragOffset = new Vector2(
                    evt.Position.X - Bounds.X,
                    evt.Position.Y - Bounds.Y
                );
                Priority = InputLayerPriority.System; // Elevar prioridad durante drag
                return true;
            }
            else if (evt.IsReleased && _isDragging)
            {
                _isDragging = false;
                Priority = InputLayerPriority.Normal;
                return true;
            }
        }
        
        if (evt.Type == InputType.MouseMove && _isDragging)
        {
            var newPos = evt.Position - _dragOffset;
            Bounds = new Rectangle(
                (int)newPos.X,
                (int)newPos.Y,
                Bounds.Width,
                Bounds.Height
            );
            _onPositionChanged?.Invoke(newPos);
            return true;
        }
        
        return base.HandleInput(evt);
    }
}

// ==================== USO EN GAME1 ====================

public class Game1 : Game
{
    private InputManager _inputManager;
    private SceneManager _sceneManager;
    
    protected override void Initialize()
    {
        _inputManager = new InputManager(this);
        _sceneManager = new SceneManager(_inputManager);
        
        // Registrar input manager en componentes de MonoGame
        Components.Add(_inputManager);
        
        base.Initialize();
    }
    
    protected override void Update(GameTime gameTime)
    {
        // El InputManager se actualiza automáticamente como GameComponent
        
        _sceneManager.Update(gameTime);
        
        base.Update(gameTime);
    }
}
```

## Características Avanzadas del Sistema

### 1. **Jerarquía de Prioridades**
- **Scene**: Gameplay base
- **Background**: Popups no modales
- **Normal**: UI estándar
- **Modal**: Bloquea todo debajo
- **System**: Atajos globales (F12, Escape)
- **Debug**: Herramientas de desarrollo

### 2. **Tipos de Consumo**
- `PassThrough`: Siempre pasa al siguiente (ej: visualizadores)
- `Block`: Siempre bloquea (ej: modales)
- `BlockOnHit`: Solo bloquea si el input fue sobre el elemento

### 3. **Contextos Modales Apilados**
```csharp
// Apilar múltiples modales
popupManager.Push(dialog1);    // Bloquea escena
popupManager.Push(dialog2);    // Bloquea dialog1 y escena
popupManager.Pop();            // Vuelve a dialog1
```

### 4. **Passthrough Selectivo**
```csharp
public class InventoryPopup : PopupBase, IModalInputController
{
    // Permitir que el jugador siga moviéndose con WASD
    public override IEnumerable<IInputHandler> AllowedHandlers => 
        new[] { _movementHandler };
    
    public override bool AllowsPassthrough(IInputHandler handler)
    {
        // Permitir input de movimiento pero no de ataque
        return handler is MovementController;
    }
}
```

### 5. **Drag & Drop entre Popups**
- El sistema eleva automáticamente la prioridad durante el drag
- Mantiene el input incluso si el mouse sale del bounds original
- Soporta drop en elementos de diferentes popups

¿Necesitas que profundice en algún aspecto específico como gesture recognition (pinch, swipe), input de gamepad navegable (foco entre botones), o virtual input para mobile?
User: Y no sería mejor desactivar los popup que estén por debajo del cuadro de diálogo activo?
Kimi: Tienes toda la razón. Desactivar los popups inferiores es mucho más limpio, eficiente y menos propenso a errores que el sistema de filtrado que propuse. Es la arquitectura estándar en motores profesionales (Unity, Unreal, Godot).

Voy a rediseñar el sistema con esta aproximación:

1. **Solo el popup superior está activo** (recibe input y se actualiza)
2. **Los inferiores quedan "congelados"** en su estado visual pero inactivos
3. **Los non-modal permiten que la escena continúe** pero no se actualizan entre sí
4. **Transparencia opcional**: se puede ver el popup de abajo pero no interactuar con él

Esta es una corrección arquitectónica importante. El sistema anterior tenía complejidad innecesaria y potenciales bugs de input. La nueva versión será más simple, más rápida y más predecible.

Necesito:
1. Modificar PopupManager para que solo actualice el top de la pila
2. Simplificar InputManager - ya no necesita filtrar por prioridades complejas
3. Ajustar el comportamiento de NonModal vs Modal
4. Añadir opciones de "background dimming" para indicar visualmente que los de abajo están inactivos
5. Mantener la capacidad de que NonModal no pause la escena pero sí pause otros popups

Esto resuelve el problema fundamental: si hay un modal arriba, todo lo demás (escena y otros popups) se pausa. Si hay un non-modal arriba, la escena continúa pero otros popups no.

Voy a implementar esta versión corregida. </think>  Tienes absolutamente razón. Desactivar los popups inferiores es **arquitectónicamente superior**: más simple, eficiente y menos propenso a bugs. Es el patrón estándar en motores profesionales (Unity, Unreal, Godot).

Aquí está el rediseño con **solo el popup superior activo**:

```csharp
// ==================== ESTADOS DE POPUP SIMPLIFICADOS ====================

public enum PopupState
{
    Inactive,       // No visible, no actualiza
    Background,     // Visible pero congelado (render only)
    Active          // Visible, actualiza y recibe input
}

// ==================== POPUP BASE REDISEÑADO ====================

public abstract class PopupBase : IPopup
{
    public string Id { get; }
    public abstract PopupType Type { get; }
    public PopupState State { get; private set; } = PopupState.Inactive;
    
    // Referencia al manager para auto-registro
    protected PopupManager Manager { get; private set; }
    protected InputManager Input { get; private set; }
    
    // Visual
    protected Rectangle Bounds;
    protected float BackgroundDimAmount => Type == PopupType.Modal ? 0.7f : 0.3f;
    private bool _wasScenePaused;
    
    public event EventHandler<PopupClosedEventArgs> OnClosed;
    
    protected PopupBase(string id)
    {
        Id = id ?? Guid.NewGuid().ToString();
    }
    
    // Llamado por el manager al añadir a la pila
    internal void Initialize(PopupManager manager, InputManager input)
    {
        Manager = manager;
        Input = input;
    }
    
    // ==================== CICLO DE VIDA ====================
    
    public void Open()
    {
        if (State != PopupState.Inactive) return;
        
        OnOpening();
        State = PopupState.Active;
        
        // Registrar en input
        Input?.RegisterHandler(this);
        
        // Si es modal, pausar escena (guardar estado previo)
        if (Type == PopupType.Modal)
        {
            _wasScenePaused = Manager.OwnerScene.IsPaused;
            Manager.OwnerScene.SetPaused(true, source: this);
        }
    }
    
    // Llamado por el manager cuando otro popup se pone encima
    internal void PushToBackground()
    {
        if (State == PopupState.Active)
        {
            State = PopupState.Background;
            Input?.UnregisterHandler(this);
            OnSentToBackground();
        }
    }
    
    // Llamado por el manager cuando vuelve a ser el top
    internal void BringToFront()
    {
        if (State == PopupState.Background)
        {
            State = PopupState.Active;
            Input?.RegisterHandler(this);
            OnBroughtToFront();
        }
    }
    
    public void Close(PopupResult result = PopupResult.Closed)
    {
        if (State == PopupState.Inactive) return;
        
        OnClosing(result);
        
        // Restaurar estado de pausa si éramos nosotros quienes lo cambiamos
        if (Type == PopupType.Modal && Manager.OwnerScene.IsPaused)
        {
            Manager.OwnerScene.SetPaused(_wasScenePaused, source: this);
        }
        
        Input?.UnregisterHandler(this);
        State = PopupState.Inactive;
        
        OnClosed?.Invoke(this, new PopupClosedEventArgs 
        { 
            PopupId = Id, 
            Result = result,
            Data = GetCloseData()
        });
        
        // Notificar al manager para que actualice la pila
        Manager.OnPopupClosed(this);
    }
    
    // ==================== UPDATE/DRAW ====================
    
    public void Update(GameTime gameTime)
    {
        // SOLO actualizar si estamos activos (top de la pila)
        if (State != PopupState.Active) return;
        
        OnUpdate(gameTime);
        
        // Actualizar componentes UI
        foreach (var component in Components.Where(c => c.IsActive))
        {
            component.Update(gameTime);
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Dibujar si estamos visibles (Active o Background)
        if (State == PopupState.Inactive) return;
        
        // Si somos el top y hay algo debajo, oscurecer fondo
        if (State == PopupState.Active && Manager.Count > 1)
        {
            DrawBackgroundDim(spriteBatch);
        }
        
        OnDraw(spriteBatch);
        
        foreach (var component in Components)
        {
            component.Draw(spriteBatch);
        }
    }
    
    protected virtual void DrawBackgroundDim(SpriteBatch spriteBatch)
    {
        var viewport = spriteBatch.GraphicsDevice.Viewport;
        var overlay = Input.GetTexture("pixel");
        
        // Opacidad proporcional a cuántos popups hay debajo
        float alpha = BackgroundDimAmount * (1f - 1f / Manager.Count);
        
        spriteBatch.Draw(overlay, new Rectangle(0, 0, viewport.Width, viewport.Height), 
            Color.Black * alpha);
    }
    
    // ==================== INPUT ====================
    
    public bool HandleInput(InputEvent evt)
    {
        if (State != PopupState.Active) return false;
        
        // Delegar a componentes de abajo hacia arriba (Z-order inverso)
        foreach (var component in Components.OfType<IInputHandler>().Reverse())
        {
            if (component.HandleInput(evt))
                return true;
        }
        
        return OnHandleInput(evt);
    }
    
    // ==================== VIRTUAL METHODS ====================
    
    protected virtual void OnOpening() { }
    protected virtual void OnSentToBackground() { }
    protected virtual void OnBroughtToFront() { }
    protected virtual void OnClosing(PopupResult result) { }
    protected virtual void OnUpdate(GameTime gameTime) { }
    protected virtual void OnDraw(SpriteBatch spriteBatch) { }
    protected virtual bool OnHandleInput(InputEvent evt) { return false; }
    protected virtual object GetCloseData() { return null; }
    
    protected List<UIComponent> Components = new();
}

// ==================== POPUP MANAGER SIMPLIFICADO ====================

public class PopupManager : IUpdateable, IDrawable
{
    private readonly Stack<PopupBase> _stack = new();
    private readonly List<PopupBase> _pendingRemoval = new();
    
    public Scene OwnerScene { get; }
    public InputManager Input { get; }
    
    public int Count => _stack.Count;
    public bool HasAnyOpen => _stack.Count > 0;
    public bool HasModalOpen => _stack.Any(p => p.Type == PopupType.Modal);
    
    // Solo el top de la pila está activo
    public PopupBase ActivePopup => _stack.Count > 0 ? _stack.Peek() : null;
    
    public PopupManager(Scene ownerScene, InputManager input)
    {
        OwnerScene = ownerScene;
        Input = input;
    }
    
    // ==================== GESTIÓN DE PILA ====================
    
    public void Push(PopupBase popup)
    {
        if (popup == null) throw new ArgumentNullException(nameof(popup));
        
        // 1. Congelar el popup actual (si existe)
        var currentTop = ActivePopup;
        currentTop?.PushToBackground();
        
        // 2. Añadir nuevo a la pila
        popup.Initialize(this, Input);
        _stack.Push(popup);
        
        // 3. Abrir (ahora es el activo)
        popup.Open();
        
        Debug.WriteLine($"[Popup] Push: {popup.Id} (Stack: {_stack.Count})");
    }
    
    public bool Pop()
    {
        if (_stack.Count == 0) return false;
        
        var popup = _stack.Pop();
        popup.Close(); // Esto llamará a OnPopupClosed -> Reactivar el siguiente
        
        return true;
    }
    
    public bool Pop(string popupId)
    {
        // Si es el top, pop normal
        if (ActivePopup?.Id == popupId)
            return Pop();
        
        // Si está en medio de la pila, remover sin activar otros
        // (caso edge: cerrar popup no-top)
        var temp = new Stack<PopupBase>();
        bool found = false;
        
        while (_stack.Count > 0)
        {
            var p = _stack.Pop();
            if (p.Id == popupId)
            {
                p.Close();
                found = true;
                break;
            }
            temp.Push(p);
        }
        
        // Restaurar pila
        while (temp.Count > 0)
            _stack.Push(temp.Pop());
        
        return found;
    }
    
    public void ClearAll()
    {
        while (_stack.Count > 0)
        {
            _stack.Pop().Close(PopupResult.Closed);
        }
    }
    
    // Llamado por popup.Close()
    internal void OnPopupClosed(PopupBase closedPopup)
    {
        // Si cerramos el top, el nuevo top debe activarse
        if (ActivePopup == closedPopup && _stack.Count > 0)
        {
            // El Pop ya lo quitó, así que Peek() da el nuevo top
            var newTop = _stack.Peek();
            newTop.BringToFront();
            
            Debug.WriteLine($"[Popup] New active: {newTop.Id}");
        }
    }
    
    // ==================== CICLO DE VIDA ====================
    
    public void Update(GameTime gameTime)
    {
        // SOLO actualizar el popup activo (top)
        ActivePopup?.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Dibujar de abajo hacia arriba (el fondo primero)
        // Convertir stack a array para iterar en orden inverso
        var popups = _stack.ToArray().Reverse();
        
        foreach (var popup in popups)
        {
            popup.Draw(spriteBatch);
        }
    }
}

// ==================== ESCENA CON CONTROL DE PAUSA ====================

public abstract class Scene
{
    public PopupManager PopupManager { get; private set; }
    public InputManager Input { get; }
    
    private readonly List<Actor> _actors = new();
    private HashSet<object> _pauseSources = new(); // Quién pidió la pausa
    
    public bool IsPaused { get; private set; }
    public bool IsActive { get; set; }
    
    // Los actores pueden elegir actualizarse en pausa (UI, efectos)
    public bool ShouldPauseActors => IsPaused && !PopupManager.HasModalOpen;
    
    protected Scene(Game game, InputManager input)
    {
        Input = input;
        PopupManager = new PopupManager(this, input);
    }
    
    // ==================== CONTROL DE PAUSA ROBUSTO ====================
    
    public void SetPaused(bool paused, object source)
    {
        if (paused)
        {
            _pauseSources.Add(source);
        }
        else
        {
            _pauseSources.Remove(source);
        }
        
        bool shouldBePaused = _pauseSources.Count > 0;
        if (shouldBePaused != IsPaused)
        {
            IsPaused = shouldBePaused;
            
            if (IsPaused)
                OnPaused();
            else
                OnResumed();
        }
    }
    
    // Forzar pausa (útil para menús de sistema)
    public void ForcePause(object source) => SetPaused(true, source);
    public void ForceResume(object source) => SetPaused(false, source);
    
    // ==================== UPDATE ====================
    
    public void Update(GameTime gameTime)
    {
        // Actualizar popups (solo el activo se actualizará)
        PopupManager.Update(gameTime);
        
        // Actualizar escena según estado
        if (!IsPaused)
        {
            UpdateScene(gameTime);
            UpdateActors(gameTime, force: false);
        }
        else if (!ShouldPauseActors) // Pausado pero por non-modal
        {
            // Escena pausada, pero actores pueden actualizarse si quieren
            UpdateActors(gameTime, force: false);
        }
        else // Pausado por modal
        {
            UpdateScenePaused(gameTime);
            UpdateActors(gameTime, force: true); // Solo los que ignoran pausa
        }
    }
    
    private void UpdateActors(GameTime gameTime, bool force)
    {
        foreach (var actor in _actors.Where(a => a.IsActive))
        {
            if (!force || a.UpdateWhenPaused)
            {
                actor.Update(gameTime);
            }
        }
    }
    
    protected virtual void UpdateScene(GameTime gameTime) { }
    protected virtual void UpdateScenePaused(GameTime gameTime) { }
    protected virtual void OnPaused() { }
    protected virtual void OnResumed() { }
    
    // ==================== DRAW ====================
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // 1. Escena (siempre, aunque esté pausada)
        DrawScene(spriteBatch);
        
        // 2. Actores
        foreach (var actor in _actors.Where(a => a.IsVisible).OrderBy(a => a.DrawOrder))
        {
            actor.Draw(spriteBatch);
        }
        
        // 3. Popups (el manager maneja el orden y quién dibuja)
        PopupManager.Draw(spriteBatch);
    }
    
    protected abstract void DrawScene(SpriteBatch spriteBatch);
}

// ==================== IMPLEMENTACIONES CONCRETAS ====================

public class DialogBox : PopupBase
{
    public override PopupType Type => PopupType.Modal;
    
    private string _title;
    private string _message;
    private Action<PopupResult> _callback;
    
    public DialogBox(string id, string title, string message, Action<PopupResult> callback = null) 
        : base(id)
    {
        _title = title;
        _message = message;
        _callback = callback;
        SetupUI();
    }
    
    private void SetupUI()
    {
        // Panel central
        var screen = Manager.OwnerScene.GraphicsDevice.Viewport;
        Bounds = new Rectangle(
            screen.Width / 2 - 200,
            screen.Height / 2 - 150,
            400, 300
        );
        
        // Botón Aceptar
        var btnOk = new Button("ok", "Aceptar", new Rectangle(Bounds.X + 50, Bounds.Bottom - 60, 100, 40));
        btnOk.OnClick += () => Close(PopupResult.Confirmed);
        Components.Add(btnOk);
        
        // Botón Cancelar
        var btnCancel = new Button("cancel", "Cancelar", new Rectangle(Bounds.Right - 150, Bounds.Bottom - 60, 100, 40));
        btnCancel.OnClick += () => Close(PopupResult.Cancelled);
        Components.Add(btnCancel);
    }
    
    protected override void OnDraw(SpriteBatch spriteBatch)
    {
        // Fondo del diálogo
        var bgTex = Input.GetTexture("dialog_bg");
        spriteBatch.Draw(bgTex, Bounds, Color.White);
        
        // Título
        Input.DrawString(_title, new Vector2(Bounds.X + 20, Bounds.Y + 20), Color.Black, scale: 1.2f);
        
        // Mensaje
        Input.DrawString(_message, new Vector2(Bounds.X + 20, Bounds.Y + 70), Color.DarkGray, 
            wrapWidth: Bounds.Width - 40);
    }
    
    protected override bool OnHandleInput(InputEvent evt)
    {
        // Cerrar con Escape
        if (evt.Key == Keys.Escape && evt.IsPressed)
        {
            Close(PopupResult.Cancelled);
            return true;
        }
        
        // Cerrar con Enter
        if (evt.Key == Keys.Enter && evt.IsPressed)
        {
            Close(PopupResult.Confirmed);
            return true;
        }
        
        return false;
    }
    
    protected override void OnClosing(PopupResult result)
    {
        _callback?.Invoke(result);
    }
}

public class NotificationPopup : PopupBase
{
    public override PopupType Type => PopupType.NonModal;
    
    private string _message;
    private float _duration;
    private float _elapsed;
    
    public NotificationPopup(string id, string message, float duration = 3f) : base(id)
    {
        _message = message;
        _duration = duration;
        
        // Posición fija: esquina superior derecha
        Bounds = new Rectangle(0, 0, 300, 80); // Posición calculada en Draw
    }
    
    protected override void OnUpdate(GameTime gameTime)
    {
        _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (_elapsed >= _duration)
        {
            Close();
        }
    }
    
    protected override void OnDraw(SpriteBatch spriteBatch)
    {
        var screen = spriteBatch.GraphicsDevice.Viewport;
        var rect = new Rectangle(screen.Width - 320, 20, 300, 80);
        
        // Fondo
        spriteBatch.Draw(Input.GetTexture("pixel"), rect, Color.DarkBlue * 0.9f);
        
        // Texto
        Input.DrawString(_message, new Vector2(rect.X + 15, rect.Y + 25), Color.White);
    }
}

// ==================== USO EJEMPLO ====================

public class GameplayScene : Scene
{
    public GameplayScene(Game game, InputManager input) : base(game, input) { }
    
    public void ShowInventory()
    {
        // Non-modal: escena sigue, otros popups se pausan
        var inv = new InventoryPopup("inv");
        PopupManager.Push(inv);
    }
    
    public void ShowConfirmDialog(string text, Action<bool> onResult)
    {
        // Modal: todo se pausa
        var dialog = new DialogBox("confirm", "Confirmar", text, result => 
        {
            onResult?.Invoke(result == PopupResult.Confirmed);
        });
        PopupManager.Push(dialog);
    }
    
    public void ShowNestedMenu()
    {
        // Apilar: primero inventario (non-modal), luego opciones sobre él (modal)
        ShowInventory(); // Non-modal, escena activa
        
        // Si abrimos opciones desde el inventario:
        var options = new OptionsPopup("options"); // Modal
        PopupManager.Push(options);
        
        // Ahora: Options activo (modal), Inventory en background, Escena pausada
        // Al cerrar Options, Inventory vuelve a activo, escena sigue pausada (porque Inventory es non-modal? No...)
        
        // Corrección: Si Inventory es non-modal, la escena NO debería pausarse
        // Pero si abrimos un modal encima, sí se pausa.
    }
}
```

## Ventajas de esta Arquitectura

### 1. **Simplicidad Radical**
- Solo un `Update()` por frame (el popup activo)
- No hay filtrado complejo de input
- Estados claros: Inactive | Background | Active

### 2. **Eficiencia**
- Menos código ejecutándose por frame
- No se procesan inputs de popups inactivos
- Garbage collector menos estresado

### 3. **Predictibilidad**
- Comportamiento determinista: si no es el top, no responde
- Fácil de debuggear y razonar sobre el estado

### 4. **Flexibilidad Conservada**
- **NonModal**: Pausa popups pero no la escena
- **Modal**: Pausa todo (escena + popups)
- Transiciones visuales suaves (el de abajo se ve atenuado)

### 5. **Casos Edge Resueltos**
```csharp
// Cerrar popup del medio de la pila
PopupManager.Pop("inventory"); 
// El resto de la pila se mantiene, el nuevo top se activa automáticamente

// Prevenir apertura si ya existe
if (PopupManager.ActivePopup?.Id != "pause_menu")
    PopupManager.Push(new PauseMenu());
```

¿Quieres que añada **transiciones animadas** (fade in/out, slide) o **navegación por teclado/mando** (foco entre botones) para completar el sistema?
User: El gran problema de este código es que estás utilizando una pila para mantener el stack de popup. Puedes utilizar una lista normal y así se puede recorrer sin necesidad de ir recreando la pila constantemente. Puedes añadir una propiedad Enabled a los popup. Sí no está activo, simplemente no tratas los update de sus componentes y por tanto no tratas la pulsación de teclado o ratón sobre los componentes
Kimi: Tienes toda la razón. Usar una `Stack` obliga a recrear arrays constantemente para iterar, y complica operaciones como remover del medio o acceder por índice. Una `List` es más flexible y eficiente para este caso.

Además, añadir una propiedad `Enabled` explícita hace el estado más transparente y evita la complejidad de los estados `Background`/`Active`. Si `Enabled == false`, simplemente no se actualiza ni procesa input, pero sigue dibujándose.

Voy a refactorizar:

1. **Cambiar `Stack<PopupBase>` por `List<PopupBase>`**
2. **Añadir propiedad `Enabled`** al popup
3. **El último elemento de la lista es siempre el activo** (top)
4. **Simplificar el ciclo de vida**: al abrir uno nuevo, desactivar el anterior; al cerrar, activar el que queda al final
5. **Iteración directa** sin conversiones ni reversos innecesarios
6. **Mantener la separación Modal/NonModal** pero con la lógica más limpia

Esto elimina:
- `Stack.ToArray().Reverse()`
- Estados internos complejos del popup
- Métodos `PushToBackground`/`BringToFront`
- Lógica de "congelamiento" confusa

Y añade:
- Control explícito via `Enabled`
- Acceso O(1) al popup activo (`Last()`)
- Posibilidad futura de reordenar popups si se necesita </think>  Tienes toda la razón. La `Stack` obliga a conversiones constantes y complica el acceso. Una `List` es más eficiente y clara. Aquí está la versión refactorizada con `Enabled` y lista plana:

```csharp
// ==================== POPUP BASE SIMPLIFICADO ====================

public abstract class PopupBase : IPopup
{
    public string Id { get; }
    public abstract PopupType Type { get; }
    
    // Estado explícito y simple
    public bool Enabled { get; set; }
    public bool IsOpen { get; private set; }
    
    protected PopupManager Manager { get; private set; }
    protected InputManager Input { get; private set; }
    protected Rectangle Bounds;
    
    public event EventHandler<PopupClosedEventArgs> OnClosed;
    
    protected PopupBase(string id)
    {
        Id = id ?? Guid.NewGuid().ToString();
    }
    
    internal void Initialize(PopupManager manager, InputManager input)
    {
        Manager = manager;
        Input = input;
    }
    
    public void Open()
    {
        if (IsOpen) return;
        
        IsOpen = true;
        Enabled = true;
        OnOpening();
        
        // Registrar para recibir input
        Input?.RegisterHandler(this);
    }
    
    public void Close(PopupResult result = PopupResult.Closed)
    {
        if (!IsOpen) return;
        
        OnClosing(result);
        Input?.UnregisterHandler(this);
        
        IsOpen = false;
        Enabled = false;
        
        OnClosed?.Invoke(this, new PopupClosedEventArgs 
        { 
            PopupId = Id, 
            Result = result 
        });
        
        Manager?.OnPopupClosed(this);
    }
    
    // Solo se actualiza si está habilitado (es el top o un non-modal especial)
    public void Update(GameTime gameTime)
    {
        if (!IsOpen || !Enabled) return;
        
        OnUpdate(gameTime);
        
        foreach (var component in Components.Where(c => c.IsActive))
        {
            component.Update(gameTime);
        }
    }
    
    // Se dibuja si está abierto, esté habilitado o no
    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsOpen) return;
        
        OnDraw(spriteBatch);
        
        foreach (var component in Components)
        {
            component.Draw(spriteBatch);
        }
    }
    
    // Input solo si está habilitado
    public bool HandleInput(InputEvent evt)
    {
        if (!IsOpen || !Enabled) return false;
        
        // Delegar a componentes (de arriba hacia abajo)
        foreach (var component in Components.OfType<IInputHandler>().Reverse())
        {
            if (component.HandleInput(evt))
                return true;
        }
        
        return OnHandleInput(evt);
    }
    
    protected virtual void OnOpening() { }
    protected virtual void OnClosing(PopupResult result) { }
    protected virtual void OnUpdate(GameTime gameTime) { }
    protected virtual void OnDraw(SpriteBatch spriteBatch) { }
    protected virtual bool OnHandleInput(InputEvent evt) { return false; }
    
    protected List<UIComponent> Components = new();
}

// ==================== POPUP MANAGER CON LISTA ====================

public class PopupManager
{
    private readonly List<PopupBase> _popups = new();
    
    public Scene OwnerScene { get; }
    public InputManager Input { get; }
    
    // El último es siempre el activo (top)
    public IReadOnlyList<PopupBase> AllPopups => _popups.AsReadOnly();
    public PopupBase ActivePopup => _popups.Count > 0 ? _popups[^1] : null;
    public int Count => _popups.Count;
    
    public bool HasModalOpen => _popups.Any(p => p.Type == PopupType.Modal && p.IsOpen);
    
    public PopupManager(Scene ownerScene, InputManager input)
    {
        OwnerScene = ownerScene;
        Input = input;
    }
    
    // ==================== GESTIÓN ====================
    
    public void Push(PopupBase popup)
    {
        if (popup == null) throw new ArgumentNullException(nameof(popup));
        
        // 1. Desactivar el actual (si existe)
        if (ActivePopup != null)
        {
            ActivePopup.Enabled = false;
        }
        
        // 2. Añadir y configurar nuevo
        popup.Initialize(this, Input);
        _popups.Add(popup);
        
        // 3. Abrir (ya está activo por defecto)
        popup.Open();
        
        // 4. Actualizar pausa de escena
        UpdateScenePause();
        
        Debug.WriteLine($"[Popup] Push: {popup.Id} (Total: {_popups.Count})");
    }
    
    public bool Pop()
    {
        if (_popups.Count == 0) return false;
        
        var popup = _popups[^1];
        popup.Close(); // Esto llamará a OnPopupClosed
        
        return true;
    }
    
    public bool Pop(string popupId)
    {
        var index = _popups.FindIndex(p => p.Id == popupId);
        if (index == -1) return false;
        
        // Si es el último, pop normal
        if (index == _popups.Count - 1)
            return Pop();
        
        // Si está en medio, cerrar sin activar otros (caso raro)
        var popup = _popups[index];
        _popups.RemoveAt(index);
        popup.Close(PopupResult.Closed);
        
        return true;
    }
    
    public void ClearAll()
    {
        // Cerrar en orden inverso para mantener consistencia
        for (int i = _popups.Count - 1; i >= 0; i--)
        {
            _popups[i].Close(PopupResult.Closed);
        }
    }
    
    // Llamado por popup.Close()
    internal void OnPopupClosed(PopupBase closedPopup)
    {
        _popups.Remove(closedPopup);
        
        // Activar el nuevo top si existe
        if (_popups.Count > 0)
        {
            var newTop = _popups[^1];
            newTop.Enabled = true;
            Debug.WriteLine($"[Popup] Activated: {newTop.Id}");
        }
        
        UpdateScenePause();
    }
    
    // ==================== CICLO DE VIDA ====================
    
    public void Update(GameTime gameTime)
    {
        // Solo el último (activo) se actualiza realmente
        // Los demás están Enabled=false, así que su Update es no-op
        foreach (var popup in _popups)
        {
            popup.Update(gameTime);
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Dibujar todos los abiertos, en orden (el activo se dibuja último = encima)
        foreach (var popup in _popups)
        {
            // Opcional: oscurecer los que no son el top
            if (popup != ActivePopup && popup.Type == PopupType.Modal)
            {
                DrawDimOverlay(spriteBatch, 0.3f);
            }
            
            popup.Draw(spriteBatch);
        }
    }
    
    private void DrawDimOverlay(SpriteBatch spriteBatch, float alpha)
    {
        var viewport = spriteBatch.GraphicsDevice.Viewport;
        var tex = Input.GetTexture("pixel");
        spriteBatch.Draw(tex, new Rectangle(0, 0, viewport.Width, viewport.Height), 
            Color.Black * alpha);
    }
    
    private void UpdateScenePause()
    {
        bool shouldPause = HasModalOpen;
        OwnerScene.SetPausedByPopup(shouldPause);
    }
}

// ==================== ESCENA SIMPLIFICADA ====================

public abstract class Scene
{
    public PopupManager PopupManager { get; }
    public bool IsPausedByPopup { get; private set; }
    
    private readonly List<Actor> _actors = new();
    private bool _manuallyPaused;
    
    protected Scene(Game game, InputManager input)
    {
        PopupManager = new PopupManager(this, input);
    }
    
    // Pausa manual (por código del juego) + pausa por popup
    public bool IsPaused => _manuallyPaused || IsPausedByPopup;
    
    public void SetPaused(bool paused) => _manuallyPaused = paused;
    internal void SetPausedByPopup(bool paused) => IsPausedByPopup = paused;
    
    public void Update(GameTime gameTime)
    {
        // Actualizar popups (solo el activo hace trabajo real)
        PopupManager.Update(gameTime);
        
        // Actualizar escena y actores según pausa
        if (!IsPaused)
        {
            UpdateScene(gameTime);
            UpdateActors(gameTime, paused: false);
        }
        else if (!IsPausedByPopup) // Pausa manual, no por popup
        {
            UpdateScenePaused(gameTime);
            UpdateActors(gameTime, paused: true);
        }
        else // Pausado por popup modal
        {
            UpdateScenePaused(gameTime);
            // Solo actores con UpdateWhenPaused
            UpdateActors(gameTime, paused: true);
        }
    }
    
    private void UpdateActors(GameTime gameTime, bool paused)
    {
        foreach (var actor in _actors.Where(a => a.IsActive))
        {
            if (!paused || a.UpdateWhenPaused)
            {
                actor.Update(gameTime);
            }
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        DrawScene(spriteBatch);
        
        foreach (var actor in _actors.Where(a => a.IsVisible).OrderBy(a => a.DrawOrder))
        {
            actor.Draw(spriteBatch);
        }
        
        PopupManager.Draw(spriteBatch);
    }
    
    protected virtual void UpdateScene(GameTime gameTime) { }
    protected virtual void UpdateScenePaused(GameTime gameTime) { }
    protected abstract void DrawScene(SpriteBatch spriteBatch);
}

// ==================== EJEMPLO: INVENTARIO CON DRAG ====================

public class InventoryPopup : PopupBase
{
    public override PopupType Type => PopupType.NonModal; // No pausa escena
    
    private List<InventorySlot> _slots = new();
    private DraggableItem _draggedItem;
    
    public InventoryPopup(string id) : base(id)
    {
        SetupUI();
    }
    
    private void SetupUI()
    {
        Bounds = new Rectangle(100, 100, 600, 400);
        
        // Crear slots
        for (int i = 0; i < 20; i++)
        {
            var slot = new InventorySlot(i);
            slot.OnBeginDrag += OnSlotBeginDrag;
            Components.Add(slot);
        }
    }
    
    protected override void OnUpdate(GameTime gameTime)
    {
        // Solo se ejecuta si Enabled (somos el top)
        if (_draggedItem != null)
        {
            _draggedItem.Position = Input.MousePosition;
        }
    }
    
    protected override bool OnHandleInput(InputEvent evt)
    {
        // Solo recibimos input si somos el top (Enabled)
        if (evt.Type == InputType.MouseLeft && evt.IsReleased && _draggedItem != null)
        {
            DropItem();
            return true;
        }
        
        return false;
    }
    
    protected override void OnDraw(SpriteBatch spriteBatch)
    {
        // Fondo del inventario
        spriteBatch.Draw(Input.GetTexture("panel"), Bounds, Color.White);
        
        // Slots se dibujan automáticamente via Components
        
        // Item arrastrado (encima de todo)
        _draggedItem?.Draw(spriteBatch);
    }
    
    private void OnSlotBeginDrag(InventorySlot slot)
    {
        if (slot.HasItem)
        {
            _draggedItem = slot.TakeItem();
        }
    }
    
    private void DropItem()
    {
        // Lógica de soltar...
        _draggedItem = null;
    }
}

// ==================== EJEMPLO: DIÁLOGO MODAL ====================

public class DialogPopup : PopupBase
{
    public override PopupType Type => PopupType.Modal;
    
    private string _text;
    private Action _onConfirm;
    private Action _onCancel;
    
    public DialogPopup(string id, string text, Action onConfirm, Action onCancel = null) 
        : base(id)
    {
        _text = text;
        _onConfirm = onConfirm;
        _onCancel = onCancel;
        
        SetupUI();
    }
    
    private void SetupUI()
    {
        var screen = Manager.OwnerScene.GraphicsDevice.Viewport;
        Bounds = new Rectangle(screen.Width/2 - 200, screen.Height/2 - 100, 400, 200);
        
        var btnYes = new Button("yes", "Sí", new Rectangle(Bounds.X + 50, Bounds.Bottom - 50, 100, 30));
        btnYes.OnClick += () => { _onConfirm?.Invoke(); Close(PopupResult.Confirmed); };
        Components.Add(btnYes);
        
        if (_onCancel != null)
        {
            var btnNo = new Button("no", "No", new Rectangle(Bounds.Right - 150, Bounds.Bottom - 50, 100, 30));
            btnNo.OnClick += () => { _onCancel?.Invoke(); Close(PopupResult.Cancelled); };
            Components.Add(btnNo);
        }
    }
    
    protected override bool OnHandleInput(InputEvent evt)
    {
        // Bloquear todo el input que no sea para nosotros
        if (evt.Key == Keys.Escape)
        {
            Close(PopupResult.Cancelled);
            return true;
        }
        
        // Si no procesamos el input, lo consumimos igual (somos modal)
        return true;
    }
}

// ==================== USO ====================

public class GameplayScene : Scene
{
    public void OpenInventory()
    {
        // Non-modal: escena sigue, pero si abrimos otro popup, este se desactiva
        PopupManager.Push(new InventoryPopup("inv"));
    }
    
    public void AskSaveBeforeExit()
    {
        // Modal: pausa todo
        PopupManager.Push(new DialogPopup("save_dialog", "¿Guardar antes de salir?",
            onConfirm: () => { SaveGame(); Exit(); },
            onCancel: () => { Exit(); }
        ));
    }
    
    public void ComplexStack()
    {
        // 1. Abrir inventario (non-modal, activo)
        OpenInventory(); 
        // Estado: [Inventory(Enabled=true)], Escena activa
        
        // 2. Abrir diálogo desde inventario (modal, activo)
        PopupManager.Push(new DialogPopup("drop_confirm", "¿Tirar objeto?", 
            onConfirm: () => { /* ... */ }));
        // Estado: [Inventory(Enabled=false), Dialog(Enabled=true)], Escena pausada
        
        // 3. Cerrar diálogo
        // Estado: [Inventory(Enabled=true)], Escena activa
    }
}
```

## Mejoras Clave

| Aspecto | Antes (Stack) | Ahora (List) |
|---------|--------------|--------------|
| **Acceso al top** | `Stack.Peek()` | `List[^1]` (index from end) |
| **Iteración** | `Stack.ToArray().Reverse()` | `foreach` directo |
| **Remover del medio** | Complejo (pop temporal) | `RemoveAt(index)` O(n) pero simple |
| **Estado** | Enum `Active/Background/Inactive` | Bool `Enabled` |
| **Claridad** | Métodos `PushToBackground` | `Enabled = false` |

## Comportamiento del Sistema

```
Abrir Inventory (NonModal):
  List: [Inventory(Enabled=true)]
  Escena: ACTIVA

Abrir Dialog desde Inventory:
  List: [Inventory(Enabled=false), Dialog(Enabled=true)]
  Escena: PAUSADA (por modal)

Cerrar Dialog:
  List: [Inventory(Enabled=true)]
  Escena: ACTIVA (vuelve a activarse)
```

¿Necesitas que añada **reordenamiento de popups** (traer al frente al hacer click) o **agrupación por tipo** (que varios non-modal estén activos simultáneamente)?
