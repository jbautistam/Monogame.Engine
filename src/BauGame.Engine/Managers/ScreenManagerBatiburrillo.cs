### USER
Cuál es la mejor forma de configuración de la pantalla en monogame

### ASSISTANT
La **configuración óptima de la pantalla en MonoGame** depende del tipo de juego (2D/3D), la resolución objetivo, la compatibilidad multiplataforma y si deseas ventana fija, pantalla completa o escalado. A continuación te explico la **mejor práctica actual** para juegos 2D (como tu shooter espacial), con enfoque en **calidad visual, rendimiento y portabilidad**.

---

## ✅ Configuración recomendada para juegos 2D en MonoGame

### 1. **Usa `GraphicsDeviceManager` con ajustes explícitos**

En el constructor de tu clase `Game1`:

```csharp
public Game1()
{
    _graphics = new GraphicsDeviceManager(this);
    
    // --- Resolución lógica (virtual) ---
    // Define una resolución base de diseño (ej: 1280x720)
    const int VIRTUAL_WIDTH = 1280;
    const int VIRTUAL_HEIGHT = 720;

    // Aplica esa resolución lógica
    _graphics.PreferredBackBufferWidth = VIRTUAL_WIDTH;
    _graphics.PreferredBackBufferHeight = VIRTUAL_HEIGHT;
    
    // --- Modo de ventana ---
    _graphics.IsFullScreen = false; // o true si lo deseas
    _graphics.HardwareModeSwitch = false; // evita cambios drásticos de resolución
    
    // --- Calidad visual ---
    _graphics.PreferMultiSampling = false; // desactivado para 2D pixel art
    _graphics.SynchronizeWithVerticalRetrace = true; // evita tearing

    Content.RootDirectory = "Content";
    IsMouseVisible = true;
}
```

> 🔑 **Clave**: `PreferredBackBufferWidth/Height` define el **tamaño lógico del lienzo**, no la resolución física de la pantalla.

---

### 2. **Escalado automático con `SpriteBatch` y matriz de escala**

Para que tu juego se vea bien en **cualquier resolución de monitor**, usa una **matriz de escala** que ajuste el lienzo lógico al tamaño real de la ventana.

#### A. Crea una propiedad para la matriz de escala:

```csharp
private Matrix _scaleMatrix;
private Viewport _virtualViewport;

private void UpdateScaleMatrix()
{
    const int VIRTUAL_WIDTH = 1280;
    const int VIRTUAL_HEIGHT = 720;

    float scaleX = (float)GraphicsDevice.Viewport.Width / VIRTUAL_WIDTH;
    float scaleY = (float)GraphicsDevice.Viewport.Height / VIRTUAL_HEIGHT;

    // Opción 1: Escalado uniforme (sin deformar, con barras negras)
    float scale = Math.Min(scaleX, scaleY);
    var viewport = new Viewport(
        (int)((GraphicsDevice.Viewport.Width - VIRTUAL_WIDTH * scale) / 2),
        (int)((GraphicsDevice.Viewport.Height - VIRTUAL_HEIGHT * scale) / 2),
        (int)(VIRTUAL_WIDTH * scale),
        (int)(VIRTUAL_HEIGHT * scale)
    );
    _virtualViewport = viewport;
    _scaleMatrix = Matrix.CreateScale(scale) *
                   Matrix.CreateTranslation(viewport.X, viewport.Y, 0);
}
```

#### B. Llama a `UpdateScaleMatrix` cuando cambie la ventana:

```csharp
protected override void Initialize()
{
    Window.ClientSizeChanged += (s, e) => UpdateScaleMatrix();
    base.Initialize();
    UpdateScaleMatrix(); // inicial
}
```

#### C. Usa la matriz en `SpriteBatch.Begin`:

```csharp
protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.Black);

    _spriteBatch.Begin(
        transformMatrix: _scaleMatrix,
        samplerState: SamplerState.PointClamp // ¡crucial para pixel art!
    );

    // Dibuja TODO tu juego en coordenadas lógicas (0-1280 x 0-720)
    _player.Draw(_spriteBatch);
    // ... enemigos, UI, etc ...

    _spriteBatch.End();
}
```

> ✅ **Ventajas**:
> - Tu juego siempre se ve en **1280x720 lógico**, sin importar la resolución real.
> - No hay deformación (usa `Math.Min` para mantener proporción).
> - Ideal para **pixel art** (usa `SamplerState.PointClamp` para evitar desenfoque).

---

### 3. **Alternativa: Pantalla completa "borderless" (más moderna)**

Si quieres **pantalla completa sin bordes** (como la mayoría de juegos actuales):

```csharp
public Game1()
{
    _graphics = new GraphicsDeviceManager(this);
    
    // Detecta resolución nativa del monitor
    var desktopBounds = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
    
    _graphics.PreferredBackBufferWidth = desktopBounds.Width;
    _graphics.PreferredBackBufferHeight = desktopBounds.Height;
    _graphics.IsFullScreen = false; // ¡no uses true!
    Window.IsBorderless = true; // ventana sin bordes = "fullscreen windowed"
    
    // Resto de ajustes...
}
```

> ✅ **Ventajas**:
> - Cambio instantáneo de ventana ↔ pantalla completa.
> - Alt+Tab funciona sin problemas.
> - Compatibilidad perfecta con multi-monitor.

---

### 4. **Configuraciones por plataforma (opcional)**

En `App.xaml.cs` (Windows) o `Program.cs` (otros), puedes ajustar más:

#### Windows (en `App.xaml.cs`):
```xml
<Application
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    x:Class="YourGame.App">
    <!-- Nada especial necesario -->
</Application>
```

#### DesktopGL (multiplataforma):
No requiere cambios; la configuración anterior funciona en Windows, Linux y macOS.

---

## 🚫 Qué evitar

| Práctica | Por qué evitarla |
|--------|------------------|
| `IsFullScreen = true` | Problemas con Alt+Tab, resoluciones no nativas, multi-monitor. |
| No usar `SamplerState.PointClamp` en pixel art | Los sprites se ven borrosos. |
| Dibujar en resolución física directamente | El juego se rompe en pantallas 4K o ultrapanorámicas. |
| Usar `ApplyChanges()` en cada frame | Solo llámalo si cambias resolución en tiempo real. |

---

## 🎮 Resumen: Configuración ideal para tu shooter

```csharp
public Game1()
{
    _graphics = new GraphicsDeviceManager(this);
    
    // Diseño en 1280x720 (HD)
    _graphics.PreferredBackBufferWidth = 1280;
    _graphics.PreferredBackBufferHeight = 720;
    
    // Modo ventana sin bordes (fullscreen windowed)
    _graphics.IsFullScreen = false;
    Window.IsBorderless = true;
    
    _graphics.SynchronizeWithVerticalRetrace = true;
    _graphics.PreferMultiSampling = false;
    
    Content.RootDirectory = "Content";
}

protected override void Initialize()
{
    Window.ClientSizeChanged += (s, e) => UpdateScaleMatrix();
    base.Initialize();
    UpdateScaleMatrix();
}
```

Y en `Draw`:

```csharp
_spriteBatch.Begin(
    transformMatrix: _scaleMatrix,
    samplerState: SamplerState.PointClamp
);
// ... dibuja en 1280x720 lógico ...
_spriteBatch.End();
```

---

## 🌐 Compatibilidad

- ✅ **Windows** (DX, GL)
- ✅ **Linux** (GL)
- ✅ **macOS** (GL)
- ✅ **Consolas** (con ajustes menores)

---

¿Te gustaría que te muestre cómo **cambiar dinámicamente entre ventana y pantalla completa** con una tecla (como F11), o cómo **soportar múltiples resoluciones de diseño** (720p, 1080p)?

### USER
Resume todas las opciones de configuración en una sola clase para que no se presenten bandas negras y se pueda utilizar un mundo superior al viewport 

### ASSISTANT
¡Perfecto! A continuación te presento una **clase integral de configuración de pantalla** para MonoGame que:

✅ **Elimina las bandas negras** (escala al máximo manteniendo proporción).  
✅ **Soporta un mundo virtual más grande que el viewport** (coordenadas de mundo ilimitadas).  
✅ **Funciona en cualquier resolución de monitor** (720p, 1080p, 4K, ultrapanorámica).  
✅ **Permite pantalla completa sin bordes** (modo "borderless windowed").  
✅ **Integra una cámara para el mundo virtual** y una **matriz de UI fija**.  
✅ **Ideal para shooters espaciales 2D con scroll y mundo grande**.

---

## 🧩 Clase `ScreenManager`

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class ScreenManager
{
    // --- Configuración de diseño ---
    public readonly int DesignWidth = 1280;
    public readonly int DesignHeight = 720;

    // --- Estado ---
    public bool IsBorderlessFullscreen { get; private set; } = true;
    public GraphicsDevice GraphicsDevice { get; }
    public GraphicsDeviceManager GraphicsDeviceManager { get; }

    // --- Matrices ---
    public Matrix UIScaleMatrix { get; private set; }      // Para UI fija (menús, HUD)
    public Matrix WorldViewMatrix { get; private set; }    // Para el mundo virtual (jugador, enemigos)

    // --- Cámara del mundo ---
    public Vector2 WorldCameraPosition { get; set; } = Vector2.Zero;
    public float WorldZoom { get; set; } = 1f;

    // --- Internos ---
    private Viewport _uiViewport;
    private float _scaleFactor;

    public ScreenManager(GraphicsDeviceManager gdm)
    {
        GraphicsDeviceManager = gdm;
        GraphicsDevice = gdm.GraphicsDevice;

        // Configuración inicial de pantalla
        SetupDisplayMode();
    }

    private void SetupDisplayMode()
    {
        if (IsBorderlessFullscreen)
        {
            var mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            GraphicsDeviceManager.PreferredBackBufferWidth = mode.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = mode.Height;
            GraphicsDeviceManager.IsFullScreen = false;
            GraphicsDeviceManager.ApplyChanges();
            Game.Window.IsBorderless = true;
        }
        else
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = DesignWidth;
            GraphicsDeviceManager.PreferredBackBufferHeight = DesignHeight;
            GraphicsDeviceManager.IsFullScreen = false;
            GraphicsDeviceManager.ApplyChanges();
            Game.Window.IsBorderless = false;
        }

        RecalculateMatrices();
    }

    public void ToggleFullscreen()
    {
        IsBorderlessFullscreen = !IsBorderlessFullscreen;
        SetupDisplayMode();
    }

    public void RecalculateMatrices()
    {
        int screenWidth = GraphicsDevice.Viewport.Width;
        int screenHeight = GraphicsDevice.Viewport.Height;

        // Escalado uniforme SIN bandas negras → ajusta al mínimo que cubra toda la pantalla
        float scaleX = (float)screenWidth / DesignWidth;
        float scaleY = (float)screenHeight / DesignHeight;
        _scaleFactor = Math.Max(scaleX, scaleY); // ¡Math.Max elimina bandas negras!

        // Viewport lógico escalado (puede ser más grande que la pantalla)
        int scaledWidth = (int)(DesignWidth * _scaleFactor);
        int scaledHeight = (int)(DesignHeight * _scaleFactor);

        // Matriz para UI/HUD: se dibuja en coordenadas de diseño (1280x720), escalado a pantalla
        UIScaleMatrix = Matrix.CreateScale(_scaleFactor);

        // Centro del mundo en la cámara
        Vector2 cameraScreenCenter = new Vector2(screenWidth / 2f, screenHeight / 2f);

        // Matriz de vista del mundo: traslada el mundo para que WorldCameraPosition esté centrado
        WorldViewMatrix =
            Matrix.CreateTranslation(new Vector3(-WorldCameraPosition.X, -WorldCameraPosition.Y, 0)) *
            Matrix.CreateScale(WorldZoom) *
            Matrix.CreateTranslation(new Vector3(cameraScreenCenter, 0));
    }

    // Llamar cuando cambie el tamaño de la ventana o al inicializar
    public void OnClientSizeChanged()
    {
        RecalculateMatrices();
    }

    // Método de conveniencia para SpriteBatch
    public void BeginWorld(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(
            transformMatrix: WorldViewMatrix,
            samplerState: SamplerState.PointClamp,
            blendState: BlendState.AlphaBlend
        );
    }

    public void BeginUI(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(
            transformMatrix: UIScaleMatrix,
            samplerState: SamplerState.PointClamp,
            blendState: BlendState.AlphaBlend
        );
    }
}
```

---

## 🎮 Integración en `Game1.cs`

```csharp
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ScreenManager _screenManager;

    private PlayerShip _player;
    private List<Enemy> _enemies = new List<Enemy>();
    // ... otras entidades ...

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.PreferMultiSampling = false;
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        _screenManager = new ScreenManager(_graphics);
        
        // Suscribirse a cambios de tamaño
        Window.ClientSizeChanged += (s, e) => _screenManager.OnClientSizeChanged();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Cargar texturas y crear entidades en coordenadas de mundo
        var playerTex = Content.Load<Texture2D>("player");
        _player = new PlayerShip(playerTex, Vector2.Zero); // mundo: (0,0)
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.F11))
        {
            _screenManager.ToggleFullscreen();
        }

        // Actualizar lógica del juego en coordenadas de mundo
        _player.Update(gameTime);

        // La cámara sigue al jugador en el mundo
        _screenManager.WorldCameraPosition = _player.Position;

        // Actualizar enemigos, balas, colisiones, etc. (en mundo)
        // ...

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // 1. Dibujar el MUNDO (con scroll, cámara, mundo grande)
        _screenManager.BeginWorld(_spriteBatch);
        _player.Draw(_spriteBatch);
        foreach (var enemy in _enemies)
            enemy.Draw(_spriteBatch);
        // ... balas, partículas del mundo ...
        _spriteBatch.End();

        // 2. Dibujar UI/HUD (fijo en pantalla, en coordenadas de diseño 1280x720)
        _screenManager.BeginUI(_spriteBatch);
        // Ej: vida, puntuación, menús
        // spriteBatch.DrawString(font, "SCORE: 1000", new Vector2(50, 50), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
```

---

## 🔑 Características clave

| Característica | Cómo se logra |
|---------------|----------------|
| **Sin bandas negras** | Usa `Math.Max(scaleX, scaleY)` → escala hasta cubrir toda la pantalla. |
| **Mundo > viewport** | La `WorldViewMatrix` permite coordenadas de mundo arbitrarias (ej: `(10000, -5000)`). |
| **UI fija en pantalla** | `UIScaleMatrix` mantiene menús en coordenadas de diseño (1280x720). |
| **Pantalla completa moderna** | `Window.IsBorderless = true` + resolución nativa. |
| **Pixel art nítido** | `SamplerState.PointClamp` evita el desenfoque. |
| **Cámara flexible** | `WorldCameraPosition` y `WorldZoom` controlan la vista del mundo. |

---

## 🌐 Compatibilidad

- ✅ Windows (DX/GL)
- ✅ Linux (GL)
- ✅ macOS (GL)
- ✅ Fácilmente adaptable a consolas

---

Con esta clase, tienes una **base sólida, escalable y profesional** para tu shooter espacial. Puedes moverte libremente por un mundo infinito, y la UI siempre se verá correctamente en cualquier monitor.

¿Te gustaría añadir ahora **soporte para múltiples cámaras** (para splitscreen) o **efectos post-procesamiento** (como CRT scanlines)?

