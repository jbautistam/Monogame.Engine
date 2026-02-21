User: No es necesario que repitas todo el código cada vez. Te iré pidiendo componente a componente que vayamos necesitando. Primero necesito crear un componente de scroll vertical que utilizaremos en un panel
Kimi: Entendido. Aquí el componente `VerticalScroll` diseñado para integrarse con cualquier componente que tenga contenido que exceda su área visible.

## Diseño del VerticalScroll

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGame.UI.Components
{
    /// <summary>
    /// Scrollbar vertical que puede anclarse a cualquier componente
    /// No tiene fondo ni borde por defecto - es funcionalidad pura
    /// </summary>
    public class VerticalScroll : UIComponent
    {
        // Configuración
        public float ScrollSpeed { get; set; } = 0.1f; // Porcentaje del contenido por click
        
        // Estado
        public float ScrollPercent { get; private set; } = 0f; // 0 = arriba, 1 = abajo
        public bool IsDragging { get; private set; }
        
        // Thumb (el control que se arrastra)
        public float ThumbSizePercent { get; set; } = 0.2f; // Tamaño del thumb relativo al track
        public float ThumbMinSize { get; set; } = 20f; // En píxeles
        
        // Opcional: puedes asignar estilos desde fuera
        public IBackground TrackBackground { get; set; } = EmptyBackground.Instance;
        public IBackground ThumbBackground { get; set; } = EmptyBackground.Instance;
        public IBorder TrackBorder { get; set; } = EmptyBorder.Instance;
        public IBorder ThumbBorder { get; set; } = EmptyBorder.Instance;

        // Áreas calculadas
        private Rectangle _trackBounds;
        private Rectangle _thumbBounds;
        
        // Input
        private MouseState _previousMouse;
        private Point _dragOffset;

        public event Action<float> OnScroll; // Notifica cambios de scroll (0-1)

        public VerticalScroll() : base()
        {
            // Por defecto ocupa todo el alto del padre y un ancho fijo proporcional
            // El padre debe asignar el ancho deseado (ej: 0.02f = 2% del ancho)
            RelativeBounds = new RelativeRect(0, 0, 1, 1);
        }

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();
            UpdateThumbPosition();
        }

        private void UpdateThumbPosition()
        {
            var bounds = AbsoluteBounds;
            
            // Track ocupa todo el espacio del componente
            _trackBounds = bounds;
            
            // Calcula tamaño del thumb basado en el contenido que representa
            float thumbHeight = Math.Max(
                bounds.Height * ThumbSizePercent, 
                ThumbMinSize
            );
            
            // Limita el thumb al tamaño disponible si es necesario
            thumbHeight = Math.Min(thumbHeight, bounds.Height);
            
            // Posición del thumb basada en ScrollPercent
            float availableSpace = bounds.Height - thumbHeight;
            int thumbY = bounds.Y + (int)(ScrollPercent * availableSpace);
            
            _thumbBounds = new Rectangle(
                bounds.X,
                thumbY,
                bounds.Width,
                (int)thumbHeight
            );
        }

        protected override void UpdateSelf(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var mousePos = new Point(mouse.X, mouse.Y);

            // Click en thumb
            if (mouse.LeftButton == ButtonState.Pressed && 
                _previousMouse.LeftButton == ButtonState.Released)
            {
                if (_thumbBounds.Contains(mousePos))
                {
                    IsDragging = true;
                    _dragOffset = new Point(mousePos.X - _thumbBounds.X, mousePos.Y - _thumbBounds.Y);
                }
                else if (_trackBounds.Contains(mousePos))
                {
                    // Click en track: avanza una página hacia arriba/abajo
                    if (mousePos.Y < _thumbBounds.Y)
                        Scroll(-ScrollSpeed); // Página arriba
                    else
                        Scroll(ScrollSpeed);  // Página abajo
                }
            }

            // Drag del thumb
            if (IsDragging)
            {
                if (mouse.LeftButton == ButtonState.Released)
                {
                    IsDragging = false;
                }
                else
                {
                    int availableSpace = _trackBounds.Height - _thumbBounds.Height;
                    if (availableSpace > 0)
                    {
                        int newY = mousePos.Y - _dragOffset.Y - _trackBounds.Y;
                        float newPercent = MathHelper.Clamp(newY / (float)availableSpace, 0f, 1f);
                        SetScroll(newPercent);
                    }
                }
            }

            // Wheel del ratón (si el mouse está sobre el padre)
            // Nota: El padre debe inyectar el delta del wheel aquí o capturarlo global
            
            _previousMouse = mouse;
        }

        /// <summary>
        /// Para scroll con rueda del ratón - llamar desde el componente padre
        /// </summary>
        public void HandleWheel(int delta)
        {
            float change = -delta * 0.001f; // Ajustar sensibilidad
            Scroll(change);
        }

        /// <summary>
        /// Mueve el scroll relativo al valor actual
        /// </summary>
        public void Scroll(float delta)
        {
            SetScroll(ScrollPercent + delta);
        }

        /// <summary>
        /// Establece el scroll a un valor absoluto (0-1)
        /// </summary>
        public void SetScroll(float percent)
        {
            float newPercent = MathHelper.Clamp(percent, 0f, 1f);
            if (Math.Abs(newPercent - ScrollPercent) > 0.0001f)
            {
                ScrollPercent = newPercent;
                UpdateThumbPosition();
                OnScroll?.Invoke(ScrollPercent);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Track
            if (TrackBackground.HasContent || TrackBorder.HasContent)
            {
                TrackBackground.Draw(spriteBatch, _trackBounds, _trackBounds);
                TrackBorder.Draw(spriteBatch, _trackBounds);
            }

            // Thumb
            if (ThumbBackground.HasContent || ThumbBorder.HasContent)
            {
                ThumbBackground.Draw(spriteBatch, _thumbBounds, _thumbBounds);
                ThumbBorder.Draw(spriteBatch, _thumbBounds);
            }
        }

        /// <summary>
        /// Calcula el offset en píxeles para aplicar a contenido scrolleable
        /// </summary>
        public int GetContentOffset(int contentHeight, int viewportHeight)
        {
            int maxScroll = Math.Max(0, contentHeight - viewportHeight);
            return (int)(ScrollPercent * maxScroll);
        }
    }
}
```

## Uso típico en un ScrollablePanel

```csharp
public class ScrollablePanel : StyledComponent
{
    private VerticalScroll _scrollbar;
    private float _contentHeight = 1000f; // Altura total del contenido virtual
    
    public ScrollablePanel()
    {
        Padding = new Spacing(0.02f, 0.02f, 0.05f, 0.02f); // Padding derecho para scrollbar
        
        _scrollbar = new VerticalScroll
        {
            RelativeBounds = new RelativeRect(0.95f, 0, 0.03f, 1f), // 3% ancho, al final
            TrackBackground = new SolidColorBackground(new Color(40, 40, 40)),
            ThumbBackground = new SolidColorBackground(new Color(100, 100, 100)),
            ThumbBorder = new SolidBorder(new Color(120, 120, 120), 1)
        };
        
        _scrollbar.OnScroll += (percent) => { /* invalidar render si es necesario */ };
        
        AddChild(_scrollbar);
    }

    protected override void UpdateSelf(GameTime gameTime)
    {
        base.UpdateSelf(gameTime);
        
        // Inyectar wheel del ratón al scrollbar
        var mouse = Mouse.GetState();
        if (AbsoluteBounds.Contains(mouse.Position))
        {
            var wheel = mouse.ScrollWheelValue - _previousWheel;
            if (wheel != 0) _scrollbar.HandleWheel(wheel);
        }
        _previousWheel = mouse.ScrollWheelValue;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, GameTime gameTime)
    {
        var content = ContentBounds;
        var offset = _scrollbar.GetContentOffset((int)_contentHeight, content.Height);
        
        // Aplicar transformación de scroll al dibujar contenido
        // (necesitarías un clipping rectangle o scissor rect aquí)
    }
}
```

¿Necesitas que añada:
- Soporte para scroll horizontal también?
- Auto-hide del scrollbar cuando el contenido cabe?
- Flechas arriba/abajo como botones?
User: Crea ahora un scroll horizontal. No hace falta por ahora que lo incluyas en un panel
Kimi: Aquí está el `HorizontalScroll`, simétrico al vertical pero adaptado para el eje X:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGame.UI.Components
{
    /// <summary>
    /// Scrollbar horizontal - funcionalidad pura, sin estilo por defecto
    /// </summary>
    public class HorizontalScroll : UIComponent
    {
        public float ScrollSpeed { get; set; } = 0.1f;
        
        public float ScrollPercent { get; private set; } = 0f;
        public bool IsDragging { get; private set; }
        
        public float ThumbSizePercent { get; set; } = 0.2f;
        public float ThumbMinSize { get; set; } = 20f;

        public IBackground TrackBackground { get; set; } = EmptyBackground.Instance;
        public IBackground ThumbBackground { get; set; } = EmptyBackground.Instance;
        public IBorder TrackBorder { get; set; } = EmptyBorder.Instance;
        public IBorder ThumbBorder { get; set; } = EmptyBorder.Instance;

        public event Action<float> OnScroll;

        private Rectangle _trackBounds;
        private Rectangle _thumbBounds;
        private MouseState _previousMouse;
        private Point _dragOffset;
        private int _previousWheel;

        public HorizontalScroll() : base()
        {
            RelativeBounds = new RelativeRect(0, 0, 1, 1);
        }

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();
            UpdateThumbPosition();
        }

        private void UpdateThumbPosition()
        {
            _trackBounds = AbsoluteBounds;
            
            float thumbWidth = Math.Max(
                _trackBounds.Width * ThumbSizePercent,
                ThumbMinSize
            );
            thumbWidth = Math.Min(thumbWidth, _trackBounds.Width);
            
            float availableSpace = _trackBounds.Width - thumbWidth;
            int thumbX = _trackBounds.X + (int)(ScrollPercent * availableSpace);
            
            _thumbBounds = new Rectangle(
                thumbX,
                _trackBounds.Y,
                (int)thumbWidth,
                _trackBounds.Height
            );
        }

        protected override void UpdateSelf(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var mousePos = new Point(mouse.X, mouse.Y);

            if (mouse.LeftButton == ButtonState.Pressed && 
                _previousMouse.LeftButton == ButtonState.Released)
            {
                if (_thumbBounds.Contains(mousePos))
                {
                    IsDragging = true;
                    _dragOffset = new Point(mousePos.X - _thumbBounds.X, mousePos.Y - _thumbBounds.Y);
                }
                else if (_trackBounds.Contains(mousePos))
                {
                    if (mousePos.X < _thumbBounds.X)
                        Scroll(-ScrollSpeed);
                    else
                        Scroll(ScrollSpeed);
                }
            }

            if (IsDragging)
            {
                if (mouse.LeftButton == ButtonState.Released)
                {
                    IsDragging = false;
                }
                else
                {
                    float availableSpace = _trackBounds.Width - _thumbBounds.Width;
                    if (availableSpace > 0)
                    {
                        int newX = mousePos.X - _dragOffset.X - _trackBounds.X;
                        SetScroll(newX / availableSpace);
                    }
                }
            }

            // Scroll horizontal con Shift+Wheel (convención estándar)
            if (_trackBounds.Contains(mousePos))
            {
                int wheel = mouse.ScrollWheelValue;
                int delta = wheel - _previousWheel;
                
                // Shift presionado = scroll horizontal
                if (delta != 0 && (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || 
                                   Keyboard.GetState().IsKeyDown(Keys.RightShift)))
                {
                    HandleWheel(delta);
                }
                _previousWheel = wheel;
            }
            else
            {
                _previousWheel = mouse.ScrollWheelValue;
            }

            _previousMouse = mouse;
        }

        public void HandleWheel(int delta)
        {
            Scroll(-delta * 0.001f);
        }

        public void Scroll(float delta)
        {
            SetScroll(ScrollPercent + delta);
        }

        public void SetScroll(float percent)
        {
            float newPercent = MathHelper.Clamp(percent, 0f, 1f);
            if (Math.Abs(newPercent - ScrollPercent) > 0.0001f)
            {
                ScrollPercent = newPercent;
                UpdateThumbPosition();
                OnScroll?.Invoke(ScrollPercent);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (TrackBackground.HasContent || TrackBorder.HasContent)
            {
                TrackBackground.Draw(spriteBatch, _trackBounds, _trackBounds);
                TrackBorder.Draw(spriteBatch, _trackBounds);
            }

            if (ThumbBackground.HasContent || ThumbBorder.HasContent)
            {
                ThumbBackground.Draw(spriteBatch, _thumbBounds, _thumbBounds);
                ThumbBorder.Draw(spriteBatch, _thumbBounds);
            }
        }

        public int GetContentOffset(int contentWidth, int viewportWidth)
        {
            int maxScroll = Math.Max(0, contentWidth - viewportWidth);
            return (int)(ScrollPercent * maxScroll);
        }
    }
}
```

## Diferencias clave con VerticalScroll:

| Aspecto | VerticalScroll | HorizontalScroll |
|---------|---------------|------------------|
| **Eje** | Y (arriba/abajo) | X (izquierda/derecha) |
| **Altura thumb** | Calculada | Fija (altura del track) |
| **Ancho thumb** | Fijo (ancho del track) | Calculado |
| **Drag** | `mouse.Y` | `mouse.X` |
| **Click track** | Arriba/abajo de thumb | Izquierda/derecha de thumb |
| **Wheel** | Directo | Requiere Shift |

## Ejemplo de uso:

```csharp
var hScroll = new HorizontalScroll
{
    RelativeBounds = new RelativeRect(0, 0.9f, 1f, 0.05f), // Abajo, 5% alto
    ThumbSizePercent = 0.3f,
    TrackBackground = new SolidColorBackground(Color.DarkGray),
    ThumbBackground = new SolidColorBackground(Color.LightGray),
    ThumbBorder = new SolidBorder(Color.White, 1)
};

hScroll.OnScroll += (percent) => 
{
    int offset = hScroll.GetContentOffset(2000, 800);
    // Desplazar contenido horizontalmente...
};
```

¿Lo añado ahora al `ScrollablePanel` para soportar ambos ejes simultáneamente?
User: Primero veamos un panel normal, tiene que controlar el clip de sus componentes hijo de forma que su contenido no se salga del panel
Kimi: Aquí está el `ClipPanel` - un panel que recorta el dibujo de sus hijos para que no se salgan de sus límites:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.UI.Components
{
    /// <summary>
    /// Panel que recorta (clip) el contenido de sus hijos a sus límites.
    /// No tiene scroll, solo clipping básico.
    /// </summary>
    public class ClipPanel : StyledComponent
    {
        // Guarda el scissor rectangle anterior para restaurarlo
        private Rectangle _previousScissor;

        public ClipPanel() : base() { }
        
        public ClipPanel(RelativeRect bounds) : base(bounds) { }

        protected override void DrawRecursive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var content = ContentBounds;
            
            // Si no hay contenido visible, no dibuja nada
            if (content.Width <= 0 || content.Height <= 0) return;

            // Guarda estado actual
            _previousScissor = spriteBatch.GraphicsDevice.ScissorRectangle;
            var previousRasterizer = spriteBatch.GraphicsDevice.RasterizerState;

            // Aplica clipping: el área de contenido es lo único visible
            var clipRasterizer = new RasterizerState
            {
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };

            spriteBatch.GraphicsDevice.RasterizerState = clipRasterizer;
            spriteBatch.GraphicsDevice.ScissorRectangle = content;

            // Dibuja fondo propio (si tiene)
            DrawSelf(spriteBatch, gameTime);

            // Dibuja hijos con clipping activo
            foreach (var child in Children)
                child.Draw(spriteBatch, gameTime);

            // Restaura estado anterior
            spriteBatch.GraphicsDevice.ScissorRectangle = _previousScissor;
            spriteBatch.GraphicsDevice.RasterizerState = previousRasterizer;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var bounds = AbsoluteBounds;
            
            // Fondo
            if (HasBackground)
                Background.Draw(spriteBatch, bounds, ContentBounds);
            
            // Borde (se dibuja FUERA del clip para que sea siempre visible)
            // Por eso no está en DrawContent
        }

        protected override void DrawContent(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // El contenido son los hijos, se dibujan en DrawRecursive con clip
        }

        // Opcional: método para verificar si un punto está dentro del área clipeada
        public bool ContainsClipped(Point point)
        {
            return ContentBounds.Contains(point);
        }
    }
}
```

## Características:

- **Clip estricto**: Los hijos no se dibujan fuera de `ContentBounds`
- **Fondo y borde**: Se comportan como `StyledComponent` normal
- **Anidación**: Los `ClipPanel` pueden anidarse (cada uno aplica su propio clip)
- **Eficiencia**: Usa `ScissorRectangle` de hardware (rápido, sin overhead de stencil)

## Limitación conocida:

`SpriteBatch` debe iniciarse con `RasterizerState` que permita cambiar el scissor. Si tu `UIManager` usa `spriteBatch.Begin()` sin parámetros, necesitarás modificarlo:

```csharp
// En UIManager.Draw():
spriteBatch.Begin(
    SpriteSortMode.Deferred, 
    BlendState.AlphaBlend,
    samplerState: SamplerState.LinearClamp,
    depthStencilState: null,
    rasterizerState: new RasterizerState { ScissorTestEnable = true } // Importante
);
```

¿Quieres que ahora integre los scrollbars (`VerticalScroll` + `HorizontalScroll`) con este `ClipPanel` para crear un `ScrollablePanel` completo?
User: Crea un scrollable panel. Debe controlar si se muestran o no las barras de scroll y por supuesto los movimientos de estos
Kimi: Aquí está el `ScrollablePanel` que combina el clipping con scrollbars verticales y horizontales, mostrándolos solo cuando son necesarios:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGame.UI.Components
{
    /// <summary>
    /// Panel con scroll vertical y/o horizontal opcional.
    /// Muestra barras solo cuando el contenido excede el viewport.
    /// </summary>
    public class ScrollablePanel : StyledComponent
    {
        // Scrollbars
        private readonly VerticalScroll _vScroll;
        private readonly HorizontalScroll _hScroll;
        
        // Configuración de visibilidad
        public ScrollbarMode VerticalMode { get; set; } = ScrollbarMode.Auto;
        public ScrollbarMode HorizontalMode { get; set; } = ScrollbarMode.Auto;
        public float ScrollbarSize { get; set; } = 0.025f; // Ancho/alto relativo del scrollbar
        
        // Contenido virtual (tamaño real del contenido a mostrar)
        private Vector2 _contentSize;
        public Vector2 ContentSize
        {
            get => _contentSize;
            set
            {
                if (_contentSize != value)
                {
                    _contentSize = value;
                    UpdateScrollbarVisibility();
                }
            }
        }

        // Estado de visibilidad calculado
        public bool ShowVertical => VerticalMode == ScrollbarMode.Always || 
                                    (VerticalMode == ScrollbarMode.Auto && NeedsVertical);
        public bool ShowHorizontal => HorizontalMode == ScrollbarMode.Always || 
                                      (HorizontalMode == ScrollbarMode.Auto && NeedsHorizontal);
        
        private bool NeedsVertical => _contentSize.Y > ContentBounds.Height;
        private bool NeedsHorizontal => _contentSize.X > ContentBounds.Width;

        // Guarda scissor anterior
        private Rectangle _previousScissor;

        public event Action OnScroll;

        public ScrollablePanel() : base()
        {
            // Inicializa scrollbars (se añaden como hijos según se necesiten)
            _vScroll = new VerticalScroll
            {
                RelativeBounds = new RelativeRect(1f - ScrollbarSize, 0, ScrollbarSize, 1f),
                OnScroll = (_) => OnScroll?.Invoke()
            };
            
            _hScroll = new HorizontalScroll
            {
                RelativeBounds = new RelativeRect(0, 1f - ScrollbarSize, 1f, ScrollbarSize),
                OnScroll = (_) => OnScroll?.Invoke()
            };
        }

        protected override void CalculateTransformations()
        {
            // Calcula primero bounds normales
            base.CalculateTransformations();
            
            // Ajusta ContentBounds restando espacio para scrollbars visibles
            var content = base.ContentBounds;
            int vBarWidth = ShowVertical ? (int)(ScrollbarSize * AbsoluteBounds.Width) : 0;
            int hBarHeight = ShowHorizontal ? (int)(ScrollbarSize * AbsoluteBounds.Height) : 0;
            
            _contentBounds = new Rectangle(
                content.X,
                content.Y,
                content.Width - vBarWidth,
                content.Height - hBarHeight
            );

            // Reposiciona scrollbars
            if (ShowVertical)
            {
                float height = ShowHorizontal ? 1f - ScrollbarSize : 1f;
                _vScroll.RelativeBounds = new RelativeRect(1f - ScrollbarSize, 0, ScrollbarSize, height);
                _vScroll.InvalidateTransform();
            }
            
            if (ShowHorizontal)
            {
                float width = ShowVertical ? 1f - ScrollbarSize : 1f;
                _hScroll.RelativeBounds = new RelativeRect(0, 1f - ScrollbarSize, width, ScrollbarSize);
                _hScroll.InvalidateTransform();
            }

            // Actualiza tamaño de thumbs basado en proporción visible
            UpdateThumbSizes();
        }

        private void UpdateThumbSizes()
        {
            if (ShowVertical && _contentSize.Y > 0)
            {
                float ratio = Math.Min(1f, _contentBounds.Height / _contentSize.Y);
                _vScroll.ThumbSizePercent = Math.Max(ratio, 0.1f); // Mínimo 10% para usabilidad
            }
            
            if (ShowHorizontal && _contentSize.X > 0)
            {
                float ratio = Math.Min(1f, _contentBounds.Width / _contentSize.X);
                _hScroll.ThumbSizePercent = Math.Max(ratio, 0.1f);
            }
        }

        private void UpdateScrollbarVisibility()
        {
            // Fuerza recálculo si cambia la necesidad de scrollbars
            bool needsRecalc = false;
            
            // Comprueba si cambió la visibilidad necesaria
            InvalidateTransform();
        }

        protected override void UpdateSelf(GameTime gameTime)
        {
            base.UpdateSelf(gameTime);
            
            // Input de rueda del ratón
            HandleWheelInput();
            
            // Actualiza scrollbars visibles
            if (ShowVertical) _vScroll.Update(gameTime);
            if (ShowHorizontal) _hScroll.Update(gameTime);
        }

        private void HandleWheelInput()
        {
            var mouse = Mouse.GetState();
            if (!ContentBounds.Contains(mouse.Position)) return;

            // Scroll vertical normal
            int vDelta = mouse.ScrollWheelValue - _previousWheelV;
            if (vDelta != 0 && ShowVertical)
            {
                _vScroll.HandleWheel(vDelta);
            }
            _previousWheelV = mouse.ScrollWheelValue;

            // Scroll horizontal con Shift
            var keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.LeftShift) || keys.IsKeyDown(Keys.RightShift))
            {
                if (vDelta != 0 && ShowHorizontal)
                {
                    _hScroll.HandleWheel(vDelta);
                }
            }
        }

        private int _previousWheelV;

        protected override void DrawRecursive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_contentBounds.Width <= 0 || _contentBounds.Height <= 0) return;

            // Guarda estado
            _previousScissor = spriteBatch.GraphicsDevice.ScissorRectangle;
            var previousRasterizer = spriteBatch.GraphicsDevice.RasterizerState;

            // 1. Dibuja fondo y borde del panel (sin clip)
            DrawSelf(spriteBatch, gameTime);

            // 2. Dibuja contenido con clip
            var clipRasterizer = new RasterizerState
            {
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };
            
            spriteBatch.GraphicsDevice.RasterizerState = clipRasterizer;
            spriteBatch.GraphicsDevice.ScissorRectangle = _contentBounds;

            // Aplica offset de scroll a la transformación
            var offset = new Vector2(
                ShowHorizontal ? -_hScroll.GetContentOffset((int)_contentSize.X, _contentBounds.Width) : 0,
                ShowVertical ? -_vScroll.GetContentOffset((int)_contentSize.Y, _contentBounds.Height) : 0
            );

            using (var transform = new TransformScope(spriteBatch, offset))
            {
                foreach (var child in Children)
                {
                    // No dibuja los scrollbars aquí (van encima sin clip)
                    if (child == _vScroll || child == _hScroll) continue;
                    
                    child.Draw(spriteBatch, gameTime);
                }
            }

            // 3. Dibuja scrollbars encima (sin clip, encima del contenido)
            spriteBatch.GraphicsDevice.ScissorRectangle = _previousScissor;
            
            if (ShowVertical) _vScroll.Draw(spriteBatch, gameTime);
            if (ShowHorizontal) _hScroll.Draw(spriteBatch, gameTime);

            // Restaura
            spriteBatch.GraphicsDevice.RasterizerState = previousRasterizer;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var bounds = AbsoluteBounds;
            
            if (HasBackground)
                Background.Draw(spriteBatch, bounds, _contentBounds);
            
            // Borde se dibuja al final en DrawContent o aquí si quieres que esté bajo los scrollbars
            if (HasBorder)
                Border.Draw(spriteBatch, bounds);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Contenido se maneja en DrawRecursive con transformación de scroll
        }

        // Helpers públicos
        public void ScrollToTop() => _vScroll?.SetScroll(0);
        public void ScrollToBottom() => _vScroll?.SetScroll(1);
        public void ScrollToLeft() => _hScroll?.SetScroll(0);
        public void ScrollToRight() => _hScroll?.SetScroll(1);
        
        public Vector2 ScrollOffset => new Vector2(
            ShowHorizontal ? _hScroll.GetContentOffset((int)_contentSize.X, _contentBounds.Width) : 0,
            ShowVertical ? _vScroll.GetContentOffset((int)_contentSize.Y, _contentBounds.Height) : 0
        );

        // Gestión de hijos: asegura que scrollbars estén en la lista cuando se necesiten
        public new void AddChild(UIComponent child)
        {
            if (child == _vScroll || child == _hScroll) return; // Ya gestionados internamente
            base.AddChild(child);
        }
    }

    public enum ScrollbarMode
    {
        Auto,    // Muestra solo si es necesario
        Always,  // Siempre visible
        Never    // Nunca visible (desactivado)
    }

    /// <summary>
    /// Helper para aplicar transformación de scroll temporalmente
    /// </summary>
    internal class TransformScope : IDisposable
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Matrix _previousTransform;

        public TransformScope(SpriteBatch spriteBatch, Vector2 offset)
        {
            _spriteBatch = spriteBatch;
            _previousTransform = spriteBatch.TransformMatrix;
            
            // Acumula la transformación existente con el offset de scroll
            spriteBatch.TransformMatrix = _previousTransform * Matrix.CreateTranslation(offset.X, offset.Y, 0);
        }

        public void Dispose()
        {
            _spriteBatch.TransformMatrix = _previousTransform;
        }
    }
}
```

## Características clave:

| Característica | Descripción |
|---------------|-------------|
| **Modos de visibilidad** | `Auto`, `Always`, `Never` para cada eje |
| **Thumb dinámico** | Tamaño proporcional al contenido visible vs total |
| **Wheel con Shift** | Vertical normal, horizontal con Shift presionado |
| **Esquina compartida** | Cuando ambos visibles, reservan espacio correctamente |
| **Transformación** | Aplica offset automáticamente a todos los hijos |

## Ejemplo de uso:

```csharp
var scrollPanel = new ScrollablePanel
{
    RelativeBounds = new RelativeRect(0.1f, 0.1f, 0.5f, 0.8f),
    Background = new SolidColorBackground(Color.DarkSlateGray),
    Border = new SolidBorder(Color.LightGray, 2),
    Padding = new Spacing(0.02f),
    ContentSize = new Vector2(2000, 3000), // Contenido grande
    VerticalMode = ScrollbarMode.Auto,
    HorizontalMode = ScrollbarMode.Auto
};

// Añade hijos normalmente
scrollPanel.AddChild(new Label { Text = "Item 1", RelativeBounds = new RelativeRect(0, 0, 1, 0.1f) });
scrollPanel.AddChild(new Label { Text = "Item 50", RelativeBounds = new RelativeRect(0, 4.9f, 1, 0.1f) });

// Eventos
scrollPanel.OnScroll += () => Console.WriteLine($"Scroll: {scrollPanel.ScrollOffset}");
```

¿Necesitas que añada soporte para scroll táctil/drag directo sobre el contenido, o algún método para scroll suavizado (lerp)?
User: No. Vamos a crear un componente grid. Tendrá filas y columnas pero pueden ser superiores al número de celdas en pantalla, por tanto debe utilizar scroll si es necesario. Sólo debe dibujar lo que realmente se vea en pantalla.
Kimi: Aquí está el componente `GridView` con virtualización (solo dibuja lo visible) y scroll integrado cuando el contenido excede el área:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGame.UI.Components
{
    /// <summary>
    /// Grid con celdas virtuales. Solo dibuja las celdas visibles en pantalla.
    /// Soporta scroll cuando el contenido excede el viewport.
    /// </summary>
    public class GridView : StyledComponent
    {
        // Configuración del grid
        public int Columns { get; set; } = 3;
        public int Rows { get; set; } = 10;
        public int TotalItems { get; set; } = 100;
        
        // Layout
        public float ColumnSpacing { get; set; } = 0.02f;
        public float RowSpacing { get; set; } = 0.02f;
        
        // Scroll
        public bool AllowVerticalScroll { get; set; } = true;
        public bool AllowHorizontalScroll { get; set; } = false;
        
        // Scrollbar interno (solo vertical por defecto, horizontal opcional)
        private VerticalScroll _vScroll;
        private HorizontalScroll _hScroll;
        private float _scrollbarSize = 0.025f;
        
        // Cache de celdas visibles para rendimiento
        private Rectangle _previousScissor;
        private int _firstVisibleRow;
        private int _lastVisibleRow;
        private int _firstVisibleCol;
        private int _lastVisibleCol;

        // Factory para crear contenido de celdas bajo demanda
        public Func<int, int, int, UIComponent> CreateCellContent { get; set; }
        
        // Cache de celdas instanciadas (pooling opcional)
        private Dictionary<(int row, int col), UIComponent> _cellCache = new();

        public GridView() : base() { }
        
        public GridView(RelativeRect bounds) : base(bounds) { }

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();
            
            // Calcula tamaño total del contenido virtual
            float cellWidth = (1f - Padding.Horizontal - ColumnSpacing * (Columns - 1)) / Columns;
            float cellHeight = CalculateCellHeight();
            
            float totalWidth = Padding.Horizontal + Columns * cellWidth + (Columns - 1) * ColumnSpacing;
            float totalHeight = Padding.Vertical + Rows * cellHeight + (Rows - 1) * RowSpacing;
            
            // Inicializa/configura scrollbars si es necesario
            UpdateScrollbars(totalWidth, totalHeight);
        }

        private float CalculateCellHeight()
        {
            // Altura proporcional al ancho para mantener aspecto, o fija
            float cellWidth = (1f - Padding.Horizontal - ColumnSpacing * (Columns - 1)) / Columns;
            return cellWidth; // Cuadrado por defecto, overrideable
        }

        private void UpdateScrollbars(float contentWidth, float contentHeight)
        {
            var contentBounds = base.ContentBounds;
            
            // Scroll vertical
            bool needsV = AllowVerticalScroll && contentHeight * AbsoluteBounds.Height > contentBounds.Height;
            if (needsV && _vScroll == null)
            {
                _vScroll = new VerticalScroll
                {
                    RelativeBounds = new RelativeRect(1f - _scrollbarSize, 0, _scrollbarSize, 1f),
                    OnScroll = (_) => InvalidateTransform()
                };
            }
            
            // Scroll horizontal  
            bool needsH = AllowHorizontalScroll && contentWidth * AbsoluteBounds.Width > contentBounds.Width;
            if (needsH && _hScroll == null)
            {
                _hScroll = new HorizontalScroll
                {
                    RelativeBounds = new RelativeRect(0, 1f - _scrollbarSize, 1f, _scrollbarSize),
                    OnScroll = (_) => InvalidateTransform()
                };
            }

            // Ajusta ContentBounds restando scrollbars
            int vBarWidth = needsV ? (int)(_scrollbarSize * AbsoluteBounds.Width) : 0;
            int hBarHeight = needsH ? (int)(_scrollbarSize * AbsoluteBounds.Height) : 0;
            
            _contentBounds = new Rectangle(
                contentBounds.X,
                contentBounds.Y,
                contentBounds.Width - vBarWidth,
                contentBounds.Height - hBarHeight
            );

            // Actualiza posición de scrollbars
            if (needsV)
            {
                float h = needsH ? 1f - _scrollbarSize : 1f;
                _vScroll.RelativeBounds = new RelativeRect(1f - _scrollbarSize, 0, _scrollbarSize, h);
                _vScroll.InvalidateTransform();
                
                // Actualiza thumb size
                float ratio = Math.Min(1f, _contentBounds.Height / (contentHeight * AbsoluteBounds.Height));
                _vScroll.ThumbSizePercent = Math.Max(ratio, 0.05f);
            }
            
            if (needsH)
            {
                float w = needsV ? 1f - _scrollbarSize : 1f;
                _hScroll.RelativeBounds = new RelativeRect(0, 1f - _scrollbarSize, w, _scrollbarSize);
                _hScroll.InvalidateTransform();
                
                float ratio = Math.Min(1f, _contentBounds.Width / (contentWidth * AbsoluteBounds.Width));
                _hScroll.ThumbSizePercent = Math.Max(ratio, 0.05f);
            }
        }

        protected override void UpdateSelf(GameTime gameTime)
        {
            base.UpdateSelf(gameTime);
            _vScroll?.Update(gameTime);
            _hScroll?.Update(gameTime);
            
            HandleWheelInput();
        }

        private void HandleWheelInput()
        {
            var mouse = Mouse.GetState();
            if (!ContentBounds.Contains(mouse.Position)) return;
            
            int delta = mouse.ScrollWheelValue - _previousWheel;
            if (delta != 0)
            {
                if (_vScroll != null && AllowVerticalScroll)
                    _vScroll.HandleWheel(delta);
                else if (_hScroll != null && AllowHorizontalScroll)
                    _hScroll.HandleWheel(delta);
            }
            _previousWheel = mouse.ScrollWheelValue;
        }
        private int _previousWheel;

        protected override void DrawRecursive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (ContentBounds.Width <= 0 || ContentBounds.Height <= 0) return;

            // Guarda estado
            _previousScissor = spriteBatch.GraphicsDevice.ScissorRectangle;
            var prevRasterizer = spriteBatch.GraphicsDevice.RasterizerState;

            // 1. Fondo y borde del grid
            DrawSelf(spriteBatch, gameTime);

            // 2. Calcula celdas visibles (virtualización)
            CalculateVisibleRange();

            // 3. Dibuja celdas visibles con clip
            var clipRasterizer = new RasterizerState
            {
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };
            
            spriteBatch.GraphicsDevice.RasterizerState = clipRasterizer;
            spriteBatch.GraphicsDevice.ScissorRectangle = ContentBounds;

            // Offset de scroll
            float offsetX = _hScroll?.GetContentOffset(
                (int)(Columns * GetCellWidth() * AbsoluteBounds.Width), 
                ContentBounds.Width
            ) ?? 0;
            
            float offsetY = _vScroll?.GetContentOffset(
                (int)(Rows * GetCellHeight() * AbsoluteBounds.Height), 
                ContentBounds.Height
            ) ?? 0;

            DrawVisibleCells(spriteBatch, gameTime, offsetX, offsetY);

            // 4. Scrollbars encima
            spriteBatch.GraphicsDevice.ScissorRectangle = _previousScissor;
            _vScroll?.Draw(spriteBatch, gameTime);
            _hScroll?.Draw(spriteBatch, gameTime);

            // Restaura
            spriteBatch.GraphicsDevice.RasterizerState = prevRasterizer;
        }

        private void CalculateVisibleRange()
        {
            // Calcula qué filas y columnas están dentro del viewport
            float cellH = GetCellHeight() * AbsoluteBounds.Height;
            float cellW = GetCellWidth() * AbsoluteBounds.Width;
            
            float scrollY = _vScroll?.ScrollPercent ?? 0;
            float scrollX = _hScroll?.ScrollPercent ?? 0;
            
            int totalHeight = (int)(Rows * cellH + (Rows - 1) * RowSpacing * AbsoluteBounds.Height);
            int totalWidth = (int)(Columns * cellW + (Columns - 1) * ColumnSpacing * AbsoluteBounds.Width);
            
            int maxScrollY = Math.Max(0, totalHeight - ContentBounds.Height);
            int maxScrollX = Math.Max(0, totalWidth - ContentBounds.Width);
            
            int currentOffsetY = (int)(scrollY * maxScrollY);
            int currentOffsetX = (int)(scrollX * maxScrollX);
            
            // Filas visibles
            _firstVisibleRow = (int)(currentOffsetY / (cellH + RowSpacing * AbsoluteBounds.Height));
            _firstVisibleRow = Math.Max(0, _firstVisibleRow);
            
            float visibleHeight = ContentBounds.Height;
            _lastVisibleRow = _firstVisibleRow + (int)(visibleHeight / cellH) + 1;
            _lastVisibleRow = Math.Min(Rows - 1, _lastVisibleRow);
            
            // Columnas visibles (si hay scroll horizontal)
            _firstVisibleCol = (int)(currentOffsetX / (cellW + ColumnSpacing * AbsoluteBounds.Width));
            _firstVisibleCol = Math.Max(0, _firstVisibleCol);
            
            float visibleWidth = ContentBounds.Width;
            _lastVisibleCol = _firstVisibleCol + (int)(visibleWidth / cellW) + 1;
            _lastVisibleCol = Math.Min(Columns - 1, _lastVisibleCol);
        }

        private void DrawVisibleCells(SpriteBatch spriteBatch, GameTime gameTime, float offsetX, float offsetY)
        {
            float cellW = GetCellWidth() * AbsoluteBounds.Width;
            float cellH = GetCellHeight() * AbsoluteBounds.Height;
            float spacingX = ColumnSpacing * AbsoluteBounds.Width;
            float spacingY = RowSpacing * AbsoluteBounds.Height;

            for (int row = _firstVisibleRow; row <= _lastVisibleRow; row++)
            {
                for (int col = _firstVisibleCol; col <= _lastVisibleCol; col++)
                {
                    int itemIndex = row * Columns + col;
                    if (itemIndex >= TotalItems) continue;

                    // Calcula posición absoluta
                    float x = Padding.Left * AbsoluteBounds.Width + col * (cellW + spacingX) - offsetX;
                    float y = Padding.Top * AbsoluteBounds.Height + row * (cellH + spacingY) - offsetY;
                    
                    // Ajusta a coordenadas de pantalla
                    var cellBounds = new Rectangle(
                        (int)x + ContentBounds.X,
                        (int)y + ContentBounds.Y,
                        (int)cellW,
                        (int)cellH
                    );

                    // Clip manual por si acaso (celda parcialmente visible)
                    if (!cellBounds.Intersects(ContentBounds)) continue;

                    // Obtiene o crea contenido de celda
                    var cell = GetOrCreateCell(row, col, itemIndex);
                    if (cell != null)
                    {
                        // Forza posición temporal
                        var oldBounds = cell.RelativeBounds;
                        cell.RelativeBounds = AbsoluteToRelative(cellBounds);
                        cell.InvalidateTransform();
                        
                        cell.Draw(spriteBatch, gameTime);
                        
                        // Restaura (o marca para reciclaje)
                        // cell.RelativeBounds = oldBounds;
                    }
                    else
                    {
                        // Dibuja placeholder si no hay factory
                        DrawCellPlaceholder(spriteBatch, cellBounds, itemIndex);
                    }
                }
            }
        }

        private UIComponent GetOrCreateCell(int row, int col, int index)
        {
            var key = (row, col);
            
            if (!_cellCache.TryGetValue(key, out var cell))
            {
                if (CreateCellContent != null)
                {
                    cell = CreateCellContent(row, col, index);
                    if (cell != null)
                        _cellCache[key] = cell;
                }
            }
            
            return cell;
        }

        private void DrawCellPlaceholder(SpriteBatch spriteBatch, Rectangle bounds, int index)
        {
            // Dibujo mínimo si no hay factory configurado
            var pixel = TextureHelper.GetPixelTexture(spriteBatch.GraphicsDevice);
            spriteBatch.Draw(pixel, bounds, index % 2 == 0 ? Color.DarkGray : Color.Gray);
            
            // Número de índice centrado
            // (simplificado - en producción usarías un Label cacheado)
        }

        private RelativeRect AbsoluteToRelative(Rectangle abs)
        {
            var parent = Parent?.ContentBounds ?? 
                new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            
            return new RelativeRect(
                (abs.X - parent.X) / (float)parent.Width,
                (abs.Y - parent.Y) / (float)parent.Height,
                abs.Width / (float)parent.Width,
                abs.Height / (float)parent.Height
            );
        }

        private float GetCellWidth()
        {
            return (1f - Padding.Horizontal - ColumnSpacing * (Columns - 1)) / Columns;
        }

        private float GetCellHeight()
        {
            return CalculateCellHeight();
        }

        protected override void DrawContent(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // El contenido se maneja en DrawRecursive con virtualización
        }

        // Limpieza de cache cuando cambia el contenido
        public void ClearCache()
        {
            _cellCache.Clear();
        }

        public void RefreshCell(int row, int col)
        {
            _cellCache.Remove((row, col));
        }
    }
}
```

## Características clave:

| Característica | Implementación |
|---------------|--------------|
| **Virtualización** | Solo calcula y dibuja celdas en `_firstVisibleRow` a `_lastVisibleRow` |
| **Scroll opcional** | Vertical por defecto, horizontal configurable |
| **Factory pattern** | `CreateCellContent(row, col, index)` genera celdas bajo demanda |
| **Caching** | Pool de celdas para reutilizar instancias |
| **Clip automático** | `ScissorRectangle` en `ContentBounds` |

## Ejemplo de uso:

```csharp
var grid = new GridView
{
    RelativeBounds = new RelativeRect(0.05f, 0.1f, 0.9f, 0.8f),
    Columns = 4,
    Rows = 50,          // 50 filas virtuales
    TotalItems = 200,   // 200 items totales
    AllowVerticalScroll = true,
    Background = new SolidColorBackground(Color.Black),
    Padding = new Spacing(0.02f),
    
    // Factory: crea el contenido de cada celda
    CreateCellContent = (row, col, index) => new Panel
    {
        Background = new SolidColorBackground(
            (row + col) % 2 == 0 ? Color.DarkBlue : Color.DarkCyan
        ),
        Border = new SolidBorder(Color.White, 1),
        Children = 
        {
            new Label 
            { 
                Text = $"Item {index}",
                RelativeBounds = new RelativeRect(0.1f, 0.3f, 0.8f, 0.4f),
                HorizontalAlignment = TextAlignment.Center
            }
        }
    }
};
```

¿Necesitas que añada selección de celdas (highlight al hover/click) o celdas de tamaño variable (no uniforme)?
User: Ahora necesitamos un stackpanel horizontal o vertical con scroll horizontal o vertical según se necesite
Kimi: Aquí está el `StackView` con scroll automático según orientación y contenido:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGame.UI.Components
{
    /// <summary>
    /// StackPanel con virtualización y scroll automático según orientación.
    /// Solo dibuja los elementos visibles en el viewport.
    /// </summary>
    public class StackView : StyledComponent
    {
        public enum Orientation { Vertical, Horizontal }

        public Orientation Direction { get; set; } = Orientation.Vertical;
        public float ItemSpacing { get; set; } = 0.02f;
        public bool AutoSize { get; set; } = false; // Ajusta tamaño al contenido

        // Scroll
        private VerticalScroll _vScroll;
        private HorizontalScroll _hScroll;
        private float _scrollbarSize = 0.025f;

        // Medición del contenido
        private float _totalContentSize; // En coordenadas relativas (0-1)
        private readonly List<float> _itemSizes = new(); // Tamaño de cada item en 0-1
        private readonly Dictionary<int, UIComponent> _itemCache = new();

        // Factory para items bajo demanda
        public Func<int, UIComponent> CreateItem { get; set; }
        public int ItemCount { get; set; } = 0;

        // Rango visible calculado
        private int _firstVisibleIndex;
        private int _lastVisibleIndex;

        public StackView() : base() { }
        public StackView(RelativeRect bounds) : base(bounds) { }

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();
            
            // Calcula tamaño total del contenido
            CalculateContentSize();
            
            // Configura scroll según orientación y necesidad
            SetupScroll();
        }

        private void CalculateContentSize()
        {
            _itemSizes.Clear();
            _totalContentSize = 0f;

            float availableSpace = Direction == Orientation.Vertical
                ? 1f - Padding.Vertical
                : 1f - Padding.Horizontal;

            if (AutoSize && CreateItem != null)
            {
                // Mide cada item real (costoso, solo si es necesario)
                for (int i = 0; i < ItemCount; i++)
                {
                    var item = GetItem(i);
                    float size = MeasureItem(item);
                    _itemSizes.Add(size);
                    _totalContentSize += size;
                }
            }
            else
            {
                // Distribución uniforme o basada en preferencias
                float itemSize = ItemCount > 0 
                    ? (availableSpace - (ItemCount - 1) * ItemSpacing) / ItemCount 
                    : 0;
                
                for (int i = 0; i < ItemCount; i++)
                {
                    _itemSizes.Add(itemSize);
                    _totalContentSize += itemSize;
                }
                
                _totalContentSize += (ItemCount - 1) * ItemSpacing;
            }
        }

        private float MeasureItem(UIComponent item)
        {
            // Extrae tamaño preferido del item o usa default
            // En implementación real, podrías tener IMeasurable
            return Direction == Orientation.Vertical 
                ? item.RelativeBounds.Height 
                : item.RelativeBounds.Width;
        }

        private void SetupScroll()
        {
            bool isVertical = Direction == Orientation.Vertical;
            float viewportSize = isVertical 
                ? 1f - Padding.Vertical 
                : 1f - Padding.Horizontal;

            bool needsScroll = _totalContentSize > viewportSize && !AutoSize;

            // Limpia scroll opuesto
            if (isVertical)
                _hScroll = null;
            else
                _vScroll = null;

            // Configura scroll necesario
            if (isVertical && needsScroll)
            {
                if (_vScroll == null)
                {
                    _vScroll = new VerticalScroll
                    {
                        OnScroll = (_) => InvalidateTransform()
                    };
                }
                
                float ratio = viewportSize / _totalContentSize;
                _vScroll.ThumbSizePercent = Math.Max(ratio, 0.05f);
            }
            else if (!isVertical && needsScroll)
            {
                if (_hScroll == null)
                {
                    _hScroll = new HorizontalScroll
                    {
                        OnScroll = (_) => InvalidateTransform()
                    };
                }
                
                float ratio = viewportSize / _totalContentSize;
                _hScroll.ThumbSizePercent = Math.Max(ratio, 0.05f);
            }

            // Ajusta ContentBounds por scrollbar
            var content = base.ContentBounds;
            int sbSize = (int)(_scrollbarSize * (isVertical ? AbsoluteBounds.Width : AbsoluteBounds.Height));
            
            if (isVertical && needsScroll)
            {
                _contentBounds = new Rectangle(content.X, content.Y, content.Width - sbSize, content.Height);
                _vScroll.RelativeBounds = new RelativeRect(1f - _scrollbarSize, 0, _scrollbarSize, 1f);
                _vScroll.InvalidateTransform();
            }
            else if (!isVertical && needsScroll)
            {
                _contentBounds = new Rectangle(content.X, content.Y, content.Width, content.Height - sbSize);
                _hScroll.RelativeBounds = new RelativeRect(0, 1f - _scrollbarSize, 1f, _scrollbarSize);
                _hScroll.InvalidateTransform();
            }
        }

        protected override void UpdateSelf(GameTime gameTime)
        {
            base.UpdateSelf(gameTime);
            
            _vScroll?.Update(gameTime);
            _hScroll?.Update(gameTime);
            
            HandleInput();
        }

        private void HandleInput()
        {
            var mouse = Mouse.GetState();
            if (!ContentBounds.Contains(mouse.Position)) return;

            int delta = mouse.ScrollWheelValue - _previousWheel;
            if (delta != 0)
            {
                if (Direction == Orientation.Vertical && _vScroll != null)
                    _vScroll.HandleWheel(delta);
                else if (Direction == Orientation.Horizontal && _hScroll != null)
                    _hScroll.HandleWheel(delta);
            }
            _previousWheel = mouse.ScrollWheelValue;
        }
        private int _previousWheel;

        protected override void DrawRecursive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (ContentBounds.Width <= 0 || ContentBounds.Height <= 0) return;

            // Guarda estado
            var prevScissor = spriteBatch.GraphicsDevice.ScissorRectangle;
            var prevRasterizer = spriteBatch.GraphicsDevice.RasterizerState;

            // 1. Fondo y borde
            DrawSelf(spriteBatch, gameTime);

            // 2. Calcula ítems visibles (virtualización)
            CalculateVisibleRange();

            // 3. Dibuja ítems visibles con clip
            var clipRasterizer = new RasterizerState
            {
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };
            
            spriteBatch.GraphicsDevice.RasterizerState = clipRasterizer;
            spriteBatch.GraphicsDevice.ScissorRectangle = ContentBounds;

            float offset = GetScrollOffset();
            DrawVisibleItems(spriteBatch, gameTime, offset);

            // 4. Scrollbar encima
            spriteBatch.GraphicsDevice.ScissorRectangle = prevScissor;
            _vScroll?.Draw(spriteBatch, gameTime);
            _hScroll?.Draw(spriteBatch, gameTime);

            // Restaura
            spriteBatch.GraphicsDevice.RasterizerState = prevRasterizer;
        }

        private void CalculateVisibleRange()
        {
            if (ItemCount == 0)
            {
                _firstVisibleIndex = 0;
                _lastVisibleIndex = -1;
                return;
            }

            float offset = GetScrollOffset();
            float viewportStart = -offset;
            float viewportEnd = viewportStart + (Direction == Orientation.Vertical 
                ? 1f - Padding.Vertical 
                : 1f - Padding.Horizontal);

            _firstVisibleIndex = FindIndexAtPosition(viewportStart);
            _lastVisibleIndex = FindIndexAtPosition(viewportEnd);

            // Margen de seguridad
            _firstVisibleIndex = Math.Max(0, _firstVisibleIndex - 1);
            _lastVisibleIndex = Math.Min(ItemCount - 1, _lastVisibleIndex + 1);
        }

        private int FindIndexAtPosition(float position)
        {
            float current = 0f;
            float spacing = ItemSpacing;
            
            for (int i = 0; i < ItemCount; i++)
            {
                float size = i < _itemSizes.Count ? _itemSizes[i] : 0f;
                
                if (current + size >= position)
                    return i;
                
                current += size + spacing;
            }
            
            return ItemCount - 1;
        }

        private float GetScrollOffset()
        {
            if (Direction == Orientation.Vertical && _vScroll != null)
            {
                float maxScroll = Math.Max(0, _totalContentSize - (1f - Padding.Vertical));
                return -_vScroll.ScrollPercent * maxScroll;
            }
            
            if (Direction == Orientation.Horizontal && _hScroll != null)
            {
                float maxScroll = Math.Max(0, _totalContentSize - (1f - Padding.Horizontal));
                return -_hScroll.ScrollPercent * maxScroll;
            }
            
            return 0f;
        }

        private void DrawVisibleItems(SpriteBatch spriteBatch, GameTime gameTime, float scrollOffset)
        {
            float currentPos = Padding.Top;
            if (Direction == Orientation.Horizontal)
                currentPos = Padding.Left;

            // Avanza hasta el primer visible
            for (int i = 0; i < _firstVisibleIndex && i < _itemSizes.Count; i++)
                currentPos += _itemSizes[i] + ItemSpacing;

            // Dibuja visibles
            for (int i = _firstVisibleIndex; i <= _lastVisibleIndex && i < ItemCount; i++)
            {
                float size = i < _itemSizes.Count ? _itemSizes[i] : 0.1f;
                
                // Calcula bounds absolutos
                Rectangle itemBounds;
                if (Direction == Orientation.Vertical)
                {
                    float y = (currentPos + scrollOffset) * AbsoluteBounds.Height + ContentBounds.Y;
                    itemBounds = new Rectangle(
                        ContentBounds.X,
                        (int)y,
                        ContentBounds.Width,
                        (int)(size * AbsoluteBounds.Height)
                    );
                }
                else
                {
                    float x = (currentPos + scrollOffset) * AbsoluteBounds.Width + ContentBounds.X;
                    itemBounds = new Rectangle(
                        (int)x,
                        ContentBounds.Y,
                        (int)(size * AbsoluteBounds.Width),
                        ContentBounds.Height
                    );
                }

                // Obtiene o crea item
                var item = GetItem(i);
                if (item != null)
                {
                    // Posición temporal
                    var oldParent = item.Parent;
                    item.Parent = this; // Temporal para cálculo de transformación
                    
                    // Fuerza bounds absolutos calculados
                    ForceItemBounds(item, itemBounds);
                    item.Draw(spriteBatch, gameTime);
                    
                    item.Parent = oldParent;
                }
                else
                {
                    DrawPlaceholder(spriteBatch, itemBounds, i);
                }

                currentPos += size + ItemSpacing;
            }
        }

        private UIComponent GetItem(int index)
        {
            if (!_itemCache.TryGetValue(index, out var item))
            {
                if (CreateItem != null)
                {
                    item = CreateItem(index);
                    if (item != null)
                        _itemCache[index] = item;
                }
            }
            return item;
        }

        private void ForceItemBounds(UIComponent item, Rectangle absoluteBounds)
        {
            // Convierte a relativos respecto al StackView
            float relX = (absoluteBounds.X - AbsoluteBounds.X) / (float)AbsoluteBounds.Width;
            float relY = (absoluteBounds.Y - AbsoluteBounds.Y) / (float)AbsoluteBounds.Height;
            float relW = absoluteBounds.Width / (float)AbsoluteBounds.Width;
            float relH = absoluteBounds.Height / (float)AbsoluteBounds.Height;

            item.RelativeBounds = new RelativeRect(relX, relY, relW, relH);
            item.InvalidateTransform();
        }

        private void DrawPlaceholder(SpriteBatch spriteBatch, Rectangle bounds, int index)
        {
            var pixel = TextureHelper.GetPixelTexture(spriteBatch.GraphicsDevice);
            spriteBatch.Draw(pixel, bounds, index % 2 == 0 ? Color.DarkSlateGray : Color.SlateGray);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, GameTime gameTime) { }

        // API pública
        public void ScrollToIndex(int index)
        {
            if (index < 0 || index >= ItemCount) return;
            
            float pos = GetItemPosition(index);
            float size = index < _itemSizes.Count ? _itemSizes[index] : 0.1f;
            float viewport = Direction == Orientation.Vertical 
                ? 1f - Padding.Vertical 
                : 1f - Padding.Horizontal;

            if (pos < 0)
            {
                // Item está arriba/izquierda, scrollear hasta él
                float percent = GetPositionPercent(pos);
                if (Direction == Orientation.Vertical) _vScroll?.SetScroll(percent);
                else _hScroll?.SetScroll(percent);
            }
            else if (pos + size > viewport)
            {
                // Item está abajo/derecha
                float targetPos = pos + size - viewport;
                float percent = GetPositionPercent(targetPos);
                if (Direction == Orientation.Vertical) _vScroll?.SetScroll(percent);
                else _hScroll?.SetScroll(percent);
            }
        }

        private float GetItemPosition(int index)
        {
            float pos = Direction == Orientation.Vertical ? Padding.Top : Padding.Left;
            for (int i = 0; i < index && i < _itemSizes.Count; i++)
                pos += _itemSizes[i] + ItemSpacing;
            return pos;
        }

        private float GetPositionPercent(float position)
        {
            float maxScroll = Math.Max(0, _totalContentSize - (Direction == Orientation.Vertical 
                ? 1f - Padding.Vertical 
                : 1f - Padding.Horizontal));
            
            return maxScroll > 0 ? MathHelper.Clamp(position / maxScroll, 0, 1) : 0;
        }

        public void RefreshItem(int index)
        {
            _itemCache.Remove(index);
        }

        public void ClearCache()
        {
            _itemCache.Clear();
            _itemSizes.Clear();
            InvalidateTransform();
        }
    }
}
```

## Características clave:

| Característica | Implementación |
|--------------|---------------|
| **Orientación** | `Vertical` (scroll vertical) o `Horizontal` (scroll horizontal) |
| **Virtualización** | Calcula `_firstVisibleIndex` / `_lastVisibleIndex` según scroll |
| **Auto-size** | Si `true`, ajusta tamaño del panel al contenido (sin scroll) |
| **ItemSpacing** | Espacio entre elementos en coordenadas relativas |
| **Factory** | `CreateItem(index)` genera elementos bajo demanda |
| **Cache** | Pool de elementos para reutilización |

## Ejemplos de uso:

```csharp
// Lista vertical de 1000 items
var list = new StackView
{
    RelativeBounds = new RelativeRect(0.1f, 0.1f, 0.4f, 0.8f),
    Direction = StackView.Orientation.Vertical,
    ItemCount = 1000,
    ItemSpacing = 0.01f,
    Background = new SolidColorBackground(Color.Black),
    CreateItem = (index) => new Label
    {
        Text = $"Item {index}",
        RelativeBounds = new RelativeRect(0, 0, 1, 0.08f), // 8% de alto cada uno
        Background = new SolidColorBackground(index % 2 == 0 ? Color.DarkGray : Color.Gray)
    }
};

// Barra horizontal de iconos
var toolbar = new StackView
{
    RelativeBounds = new RelativeRect(0, 0, 1, 0.1f),
    Direction = StackView.Orientation.Horizontal,
    ItemCount = 50,
    ItemSpacing = 0.01f,
    Padding = new Spacing(0.01f),
    CreateItem = (index) => new Image
    {
        Texture = iconTexture,
        RelativeBounds = new RelativeRect(0, 0, 0.08f, 1) // 8% de ancho, alto completo
    }
};
```

¿Necesitas que añada selección de items (single/multiple) o animaciones de entrada/salida al hacer scroll?
User: Ahora necesitamos un control que muestre un texto con el efecto de máquina de escribir, es decir letra a letra o palabra a palabra configurable. Además debe permitir desactivar el efecto, es decir, tendrá una propiedad que si se pone a falso, dibujará el texto completo. Se debe poder configurar la velocidad. El texto debe partirse por palabra y admitir saltos de linea físicos y tabulador. Además, el texto puede tener cambios de color y negrita de tipo bbcode o similar incluyendo esperas de tiempo antes de presentar la siguiente palabra
Kimi: Aquí está el `TypewriterLabel` con todas las características solicitadas:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MonoGame.UI.Components
{
    /// <summary>
    /// Label con efecto máquina de escribir. Soporta BBCode básico para colores, estilos y pausas.
    /// </summary>
    public class TypewriterLabel : StyledComponent
    {
        // Configuración
        public SpriteFont Font { get; set; }
        public Color DefaultColor { get; set; } = Color.White;
        
        /// <summary>
        /// Velocidad en caracteres por segundo (0 = instantáneo)
        /// </summary>
        public float Speed { get; set; } = 20f;
        
        /// <summary>
        /// Modo de revelado: Character o Word
        /// </summary>
        public RevealMode Mode { get; set; } = RevealMode.Character;
        
        /// <summary>
        /// Activa/desactiva el efecto typewriter
        /// </summary>
        public bool IsTypewriterEnabled { get; set; } = true;
        
        /// <summary>
        /// Saltos de línea automáticos si el texto excede el ancho
        /// </summary>
        public bool WordWrap { get; set; } = true;
        
        // Estado del typewriter
        public bool IsComplete => _currentIndex >= _segments.Count;
        public bool IsPlaying { get; private set; }
        public float Progress => _segments.Count > 0 ? _currentIndex / (float)_segments.Count : 1f;
        
        // Texto procesado
        private string _rawText = "";
        private readonly List<TextSegment> _segments = new();
        private int _currentIndex;
        private float _timer;
        private float _pauseTimer;
        
        // Formato actual durante parsing
        private Color _currentColor;
        private bool _currentBold;
        private float _currentScale = 1f;
        
        // Layout calculado
        private readonly List<RenderedLine> _lines = new();
        private float _scrollY;
        private bool _needsLayout = true;

        public event Action OnComplete;
        public event Action OnCharacterRevealed;

        public enum RevealMode { Character, Word }
        
        public TypewriterLabel() : base() { }
        public TypewriterLabel(RelativeRect bounds) : base(bounds) { }

        /// <summary>
        /// Establece el texto con tags BBCode soportados:
        /// [color=red] [color=#FF0000] [b] [i] [size=1.5] [wait=0.5] [pause] [/color] [/b] [/i] [/size]
        /// </summary>
        public void SetText(string text)
        {
            _rawText = text ?? "";
            ParseText();
            Reset();
            _needsLayout = true;
        }

        public void AppendText(string text)
        {
            _rawText += text;
            ParseText();
            _needsLayout = true;
        }

        public void Reset()
        {
            _currentIndex = 0;
            _timer = 0;
            _pauseTimer = 0;
            _scrollY = 0;
            IsPlaying = IsTypewriterEnabled;
            
            if (!IsTypewriterEnabled)
                _currentIndex = _segments.Count;
        }

        public void SkipToEnd()
        {
            _currentIndex = _segments.Count;
            IsPlaying = false;
            OnComplete?.Invoke();
        }

        public void Start()
        {
            if (IsComplete) Reset();
            IsPlaying = true;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Resume()
        {
            IsPlaying = !IsComplete;
        }

        private void ParseText()
        {
            _segments.Clear();
            _currentColor = DefaultColor;
            _currentBold = false;
            _currentScale = 1f;

            var regex = new Regex(@"\[(\w+)(?:=([^\]]+))?\]|\[/(\w+)\]|(\n)|(\t)|([^\[\n\t]+)");
            var matches = regex.Matches(_rawText);
            
            foreach (Match match in matches)
            {
                if (match.Groups[4].Success) // Newline \n
                {
                    _segments.Add(new TextSegment { Type = SegmentType.Newline });
                }
                else if (match.Groups[5].Success) // Tab \t
                {
                    _segments.Add(new TextSegment 
                    { 
                        Type = SegmentType.Tab,
                        Text = "\t"
                    });
                }
                else if (match.Groups[6].Success) // Texto plano
                {
                    AddTextSegments(match.Groups[6].Value);
                }
                else if (match.Groups[1].Success) // Tag de apertura
                {
                    ProcessOpenTag(match.Groups[1].Value, match.Groups[2].Value);
                }
                else if (match.Groups[3].Success) // Tag de cierre
                {
                    ProcessCloseTag(match.Groups[3].Value);
                }
            }
        }

        private void AddTextSegments(string text)
        {
            if (Mode == RevealMode.Word)
            {
                var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {
                    _segments.Add(new TextSegment
                    {
                        Type = SegmentType.Word,
                        Text = words[i] + (i < words.Length - 1 ? " " : ""),
                        Color = _currentColor,
                        IsBold = _currentBold,
                        Scale = _currentScale
                    });
                }
            }
            else // Character
            {
                foreach (char c in text)
                {
                    _segments.Add(new TextSegment
                    {
                        Type = SegmentType.Character,
                        Text = c.ToString(),
                        Color = _currentColor,
                        IsBold = _currentBold,
                        Scale = _currentScale
                    });
                }
            }
        }

        private void ProcessOpenTag(string tag, string value)
        {
            switch (tag.ToLower())
            {
                case "color":
                case "c":
                    _currentColor = ParseColor(value);
                    break;
                case "b":
                case "bold":
                    _currentBold = true;
                    break;
                case "i":
                case "italic":
                    // Italic no implementado en SpriteFont básico, marcar para shader futuro
                    break;
                case "size":
                case "s":
                    if (float.TryParse(value?.Replace('.', ','), out float scale))
                        _currentScale = MathHelper.Clamp(scale, 0.5f, 3f);
                    break;
                case "wait":
                case "w":
                    if (float.TryParse(value?.Replace('.', ','), out float waitTime))
                        _segments.Add(new TextSegment { Type = SegmentType.Wait, Delay = waitTime });
                    break;
                case "pause":
                case "p":
                    _segments.Add(new TextSegment { Type = SegmentType.Wait, Delay = float.MaxValue });
                    break;
            }
        }

        private void ProcessCloseTag(string tag)
        {
            switch (tag.ToLower())
            {
                case "color":
                case "c":
                    _currentColor = DefaultColor;
                    break;
                case "b":
                case "bold":
                    _currentBold = false;
                    break;
                case "size":
                case "s":
                    _currentScale = 1f;
                    break;
            }
        }

        private Color ParseColor(string value)
        {
            if (string.IsNullOrEmpty(value)) return DefaultColor;
            
            // Hex #RRGGBB o #AARRGGBB
            if (value.StartsWith("#") && (value.Length == 7 || value.Length == 9))
            {
                try
                {
                    byte a = value.Length == 9 ? Convert.ToByte(value.Substring(1, 2), 16) : (byte)255;
                    int start = value.Length == 9 ? 3 : 1;
                    byte r = Convert.ToByte(value.Substring(start, 2), 16);
                    byte g = Convert.ToByte(value.Substring(start + 2, 2), 16);
                    byte b = Convert.ToByte(value.Substring(start + 4, 2), 16);
                    return new Color(r, g, b, a);
                }
                catch { return DefaultColor; }
            }
            
            // Nombres de colores comunes
            return value.ToLower() switch
            {
                "red" => Color.Red,
                "green" => Color.Green,
                "blue" => Color.Blue,
                "yellow" => Color.Yellow,
                "white" => Color.White,
                "black" => Color.Black,
                "gray" or "grey" => Color.Gray,
                "cyan" => Color.Cyan,
                "magenta" => Color.Magenta,
                "orange" => Color.Orange,
                "purple" => Color.Purple,
                _ => DefaultColor
            };
        }

        protected override void UpdateSelf(GameTime gameTime)
        {
            if (!IsPlaying || IsComplete || Speed <= 0) return;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Maneja pausas
            if (_pauseTimer > 0)
            {
                _pauseTimer -= delta;
                if (_pauseTimer <= 0)
                {
                    Advance();
                }
                return;
            }

            // Avanza según velocidad
            _timer += delta * Speed;
            
            while (_timer >= 1f && !IsComplete && _pauseTimer <= 0)
            {
                _timer -= 1f;
                Advance();
            }
        }

        private void Advance()
        {
            if (_currentIndex >= _segments.Count) return;

            var segment = _segments[_currentIndex];
            
            if (segment.Type == SegmentType.Wait)
            {
                if (segment.Delay == float.MaxValue)
                {
                    // Pausa manual, requiere Resume()
                    Pause();
                    _currentIndex++; // Consume el tag
                    return;
                }
                
                _pauseTimer = segment.Delay;
                _currentIndex++; // Consume el tag de espera
                return;
            }

            _currentIndex++;
            OnCharacterRevealed?.Invoke();

            if (IsComplete)
            {
                IsPlaying = false;
                OnComplete?.Invoke();
            }
        }

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();
            
            if (_needsLayout || WordWrap)
            {
                PerformLayout();
                _needsLayout = false;
            }
        }

        private void PerformLayout()
        {
            _lines.Clear();
            if (Font == null || _segments.Count == 0) return;

            var content = ContentBounds;
            float maxWidth = content.Width;
            
            var currentLine = new RenderedLine();
            float currentX = 0;
            float lineHeight = Font.LineSpacing;

            for (int i = 0; i < _segments.Count; i++)
            {
                var seg = _segments[i];
                
                if (seg.Type == SegmentType.Newline)
                {
                    FinalizeLine(currentLine, ref currentX, ref lineHeight);
                    currentLine = new RenderedLine();
                    currentX = 0;
                }
                else if (seg.Type == SegmentType.Tab)
                {
                    float tabSize = Font.MeasureString("    ").X * seg.Scale;
                    float alignedX = (float)Math.Floor((currentX + tabSize) / tabSize) * tabSize;
                    currentLine.Spacing.Add((alignedX - currentX, null));
                    currentX = alignedX;
                }
                else if (seg.Type == SegmentType.Wait)
                {
                    // Los waits no ocupan espacio visual
                }
                else // Character o Word
                {
                    var size = Font.MeasureString(seg.Text) * seg.Scale;
                    
                    // Word wrap
                    if (WordWrap && currentX + size.X > maxWidth && currentLine.Segments.Count > 0)
                    {
                        FinalizeLine(currentLine, ref currentX, ref lineHeight);
                        currentLine = new RenderedLine();
                        currentX = 0;
                    }
                    
                    // Si una sola palabra es más ancha que la línea, la partimos (overflow)
                    if (WordWrap && size.X > maxWidth)
                    {
                        // Simplificación: truncar o forzar overflow
                        // En implementación completa: partir la palabra
                    }

                    currentLine.Segments.Add(new RenderedSegment
                    {
                        SourceIndex = i,
                        Text = seg.Text,
                        Color = seg.Color,
                        IsBold = seg.IsBold,
                        Scale = seg.Scale,
                        Position = new Vector2(currentX, 0),
                        Size = size
                    });
                    
                    currentX += size.X;
                    lineHeight = Math.Max(lineHeight, size.Y);
                }
            }
            
            if (currentLine.Segments.Count > 0)
                FinalizeLine(currentLine, ref currentX, ref lineHeight);
        }

        private void FinalizeLine(RenderedLine line, ref float currentX, ref float lineHeight)
        {
            line.Width = currentX;
            line.Height = lineHeight;
            _lines.Add(line);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Font == null || _lines.Count == 0) return;

            var content = ContentBounds;
            float startY = content.Y + _scrollY;
            float currentY = startY;

            // Calcula qué líneas están visibles (optimización)
            for (int lineIdx = 0; lineIdx < _lines.Count; lineIdx++)
            {
                var line = _lines[lineIdx];
                
                // Skip líneas fuera de pantalla
                if (currentY + line.Height < content.Y)
                {
                    currentY += line.Height;
                    continue;
                }
                if (currentY > content.Bottom)
                    break;

                // Dibuja segmentos de esta línea que estén revelados
                DrawLine(spriteBatch, line, content.X, currentY);
                
                currentY += line.Height;
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, RenderedLine line, float startX, float y)
        {
            float x = startX;
            
            // Alineación horizontal (opcional: agregar propiedad TextAlignment)
            if (HorizontalAlignment == TextAlignment.Center)
                x += (ContentBounds.Width - line.Width) / 2;
            else if (HorizontalAlignment == TextAlignment.Right)
                x += ContentBounds.Width - line.Width;

            foreach (var seg in line.Segments)
            {
                if (seg.SourceIndex >= _currentIndex)
                    break; // No revelado aún

                var color = seg.Color;
                
                // Efecto cursor parpadeante en el último carácter visible
                if (IsPlaying && seg.SourceIndex == _currentIndex - 1)
                {
                    color = Color.Lerp(color, Color.White, 0.3f + (float)Math.Sin(_timer * MathHelper.Pi) * 0.3f);
                }

                // Bold simulado con múltiple dibujo levemente offset (fake bold)
                if (seg.IsBold)
                {
                    spriteBatch.DrawString(Font, seg.Text, new Vector2(x + 1, y), color, 0, Vector2.Zero, seg.Scale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(Font, seg.Text, new Vector2(x, y + 1), color, 0, Vector2.Zero, seg.Scale, SpriteEffects.None, 0);
                }
                
                spriteBatch.DrawString(Font, seg.Text, new Vector2(x, y), color, 0, Vector2.Zero, seg.Scale, SpriteEffects.None, 0);
                
                x += seg.Size.X;
            }
        }

        // Estructuras internas
        
        private enum SegmentType { Character, Word, Newline, Tab, Wait }
        
        private class TextSegment
        {
            public SegmentType Type;
            public string Text;
            public Color Color;
            public bool IsBold;
            public float Scale = 1f;
            public float Delay; // Para Wait
        }

        private class RenderedLine
        {
            public List<RenderedSegment> Segments = new();
            public List<(float spacing, object marker)> Spacing = new();
            public float Width;
            public float Height;
        }

        private class RenderedSegment
        {
            public int SourceIndex;
            public string Text;
            public Color Color;
            public bool IsBold;
            public float Scale;
            public Vector2 Position;
            public Vector2 Size;
        }
    }
}
```

## Características:

| Feature | Implementación |
|---------|---------------|
| **Modos** | `Character` (letra a letra) o `Word` (palabra a palabra) |
| **Velocidad** | Caracteres/palabras por segundo (`Speed`) |
| **Activación** | `IsTypewriterEnabled` - `false` muestra todo instantáneamente |
| **BBCode** | `[color]`, `[b]`, `[size]`, `[wait=segundos]`, `[pause]` |
| **Saltos** | `\n` (físico), `\t` (tabulador), word-wrap automático |
| **Eventos** | `OnCharacterRevealed`, `OnComplete` |
| **Control** | `Start()`, `Pause()`, `Resume()`, `SkipToEnd()`, `Reset()` |

## Ejemplo de uso:

```csharp
var dialog = new TypewriterLabel
{
    RelativeBounds = new RelativeRect(0.1f, 0.6f, 0.8f, 0.3f),
    Font = Content.Load<SpriteFont>("Fonts/Dialog"),
    Speed = 15f, // 15 caracteres por segundo
    Mode = TypewriterLabel.RevealMode.Character,
    Background = new SolidColorBackground(new Color(0, 0, 0, 200)),
    Padding = new Spacing(0.05f),
    WordWrap = true
};

// Texto con formato
dialog.SetText(
    "[color=yellow]Narrador:[/color]\n" +
    "Bienvenido al [b]sistema[/b] de [color=#FF6B6B]diálogos[/color].\n" +
    "[wait=0.5]Esto es una pausa...[wait=0.3] ¿Listo?\n" +
    "[pause]" // Pausa manual, requiere Resume()
);

dialog.Start();
dialog.OnComplete += () => Console.WriteLine("Texto completado");

// Controles
if (keyboard.IsKeyDown(Keys.Space))
    dialog.SkipToEnd(); // O dialog.Resume() si está en [pause]
```

¿Necesitas que añada soporte para efectos de sonido por carácter, o variaciones de velocidad por tag (`[speed=0.5]`)?
User: Separa el intérprete del componente en una clase diferente. Añade la velocidad y una etiqueta para llamar a un evento externo con el contenido de la etiqueta cuando se llegue a esa etiqueta
Kimi: Aquí está el `TypewriterLabel` refactorizado con el intérprete separado en `TypewriterInterpreter`:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MonoGame.UI.Components
{
    #region Interpreter

    /// <summary>
    /// Intérprete de texto con efecto máquina de escribir y BBCode.
    /// Procesa el texto en segmentos ejecutables independientemente del renderizado.
    /// </summary>
    public class TypewriterInterpreter
    {
        // Configuración de parsing
        public RevealMode DefaultMode { get; set; } = RevealMode.Character;
        public Color DefaultColor { get; set; } = Color.White;
        public float DefaultSpeed { get; set; } = 20f;
        
        // Eventos
        public event Action<string> OnTagTriggered; // [trigger=nombre_evento]
        public event Action OnComplete;
        public event Action OnSegmentRevealed;

        // Estado
        public bool IsComplete => _currentIndex >= _segments.Count;
        public bool IsPlaying { get; private set; }
        public float Progress => _segments.Count > 0 ? _currentIndex / (float)_segments.Count : 1f;
        public int CurrentIndex => _currentIndex;
        public IReadOnlyList<TextSegment> Segments => _segments;

        // Interno
        private readonly List<TextSegment> _segments = new();
        private int _currentIndex;
        private float _timer;
        private float _pauseTimer;
        private float _currentSpeed;
        private RevealMode _currentMode;

        public void Parse(string text, RevealMode? mode = null)
        {
            _segments.Clear();
            _currentSpeed = DefaultSpeed;
            _currentMode = mode ?? DefaultMode;

            var context = new ParseContext
            {
                Color = DefaultColor,
                Bold = false,
                Italic = false,
                Scale = 1f,
                Speed = DefaultSpeed
            };

            // Regex: [tag=value] [/tag] \n \t texto
            var regex = new Regex(@"\[(\w+)(?:=([^\]]+))?\]|\[/(\w+)\]|(\\n)|(\\t)|([^\[\\\n\t]+)");
            var matches = regex.Matches(text ?? "");

            foreach (Match match in matches)
            {
                if (match.Groups[4].Success) // \n
                {
                    _segments.Add(new TextSegment { Type = SegmentType.Newline });
                }
                else if (match.Groups[5].Success) // \t
                {
                    _segments.Add(new TextSegment { Type = SegmentType.Tab });
                }
                else if (match.Groups[6].Success) // Texto plano
                {
                    AddText(match.Groups[6].Value, context);
                }
                else if (match.Groups[1].Success) // Tag apertura
                {
                    ProcessOpenTag(match.Groups[1].Value, match.Groups[2].Value, context);
                }
                else if (match.Groups[3].Success) // Tag cierre
                {
                    ProcessCloseTag(match.Groups[3].Value, context);
                }
            }
        }

        private void AddText(string text, ParseContext ctx)
        {
            if (_currentMode == RevealMode.Word)
            {
                var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {
                    bool last = i == words.Length - 1;
                    AddSegment(new TextSegment
                    {
                        Type = SegmentType.Word,
                        Text = words[i] + (last ? "" : " "),
                        Color = ctx.Color,
                        Bold = ctx.Bold,
                        Italic = ctx.Italic,
                        Scale = ctx.Scale,
                        Speed = ctx.Speed
                    });
                }
            }
            else
            {
                foreach (char c in text)
                {
                    AddSegment(new TextSegment
                    {
                        Type = SegmentType.Character,
                        Text = c.ToString(),
                        Color = ctx.Color,
                        Bold = ctx.Bold,
                        Italic = ctx.Italic,
                        Scale = ctx.Scale,
                        Speed = ctx.Speed
                    });
                }
            }
        }

        private void AddSegment(TextSegment seg)
        {
            seg.Index = _segments.Count;
            _segments.Add(seg);
        }

        private void ProcessOpenTag(string tag, string value, ParseContext ctx)
        {
            switch (tag.ToLower())
            {
                case "color":
                case "c":
                    ctx.ColorStack.Push(ctx.Color);
                    ctx.Color = ParseColor(value);
                    break;
                    
                case "b":
                case "bold":
                    ctx.BoldStack.Push(ctx.Bold);
                    ctx.Bold = true;
                    break;
                    
                case "i":
                case "italic":
                    ctx.ItalicStack.Push(ctx.Italic);
                    ctx.Italic = true;
                    break;
                    
                case "size":
                case "s":
                    ctx.ScaleStack.Push(ctx.Scale);
                    if (float.TryParse(NormalizeFloat(value), out float scale))
                        ctx.Scale = MathHelper.Clamp(scale, 0.5f, 3f);
                    break;
                    
                case "speed":
                    ctx.SpeedStack.Push(ctx.Speed);
                    if (float.TryParse(NormalizeFloat(value), out float speed))
                        ctx.Speed = Math.Max(0.1f, speed);
                    break;
                    
                case "wait":
                case "w":
                    if (float.TryParse(NormalizeFloat(value), out float wait))
                    {
                        _segments.Add(new TextSegment 
                        { 
                            Type = SegmentType.Wait, 
                            Delay = wait,
                            Index = _segments.Count
                        });
                    }
                    break;
                    
                case "pause":
                case "p":
                    _segments.Add(new TextSegment 
                    { 
                        Type = SegmentType.Pause,
                        Index = _segments.Count
                    });
                    break;
                    
                case "trigger":
                case "t":
                case "event":
                case "e":
                    _segments.Add(new TextSegment 
                    { 
                        Type = SegmentType.Trigger,
                        Text = value ?? "",
                        Index = _segments.Count
                    });
                    break;
            }
        }

        private void ProcessCloseTag(string tag, ParseContext ctx)
        {
            switch (tag.ToLower())
            {
                case "color":
                case "c":
                    if (ctx.ColorStack.Count > 0) ctx.Color = ctx.ColorStack.Pop();
                    break;
                case "b":
                case "bold":
                    if (ctx.BoldStack.Count > 0) ctx.Bold = ctx.BoldStack.Pop();
                    break;
                case "i":
                case "italic":
                    if (ctx.ItalicStack.Count > 0) ctx.Italic = ctx.ItalicStack.Pop();
                    break;
                case "size":
                case "s":
                    if (ctx.ScaleStack.Count > 0) ctx.Scale = ctx.ScaleStack.Pop();
                    break;
                case "speed":
                    if (ctx.SpeedStack.Count > 0) ctx.Speed = ctx.SpeedStack.Pop();
                    break;
            }
        }

        private Color ParseColor(string value)
        {
            if (string.IsNullOrEmpty(value)) return DefaultColor;
            
            if (value.StartsWith("#"))
            {
                try
                {
                    byte a = value.Length == 9 ? Convert.ToByte(value.Substring(1, 2), 16) : (byte)255;
                    int start = value.Length == 9 ? 3 : 1;
                    byte r = Convert.ToByte(value.Substring(start, 2), 16);
                    byte g = Convert.ToByte(value.Substring(start + 2, 2), 16);
                    byte b = Convert.ToByte(value.Substring(start + 4, 2), 16);
                    return new Color(r, g, b, a);
                }
                catch { return DefaultColor; }
            }
            
            return value.ToLower() switch
            {
                "red" => Color.Red,
                "green" => Color.Green,
                "blue" => Color.Blue,
                "yellow" => Color.Yellow,
                "white" => Color.White,
                "black" => Color.Black,
                "gray" or "grey" => Color.Gray,
                "cyan" => Color.Cyan,
                "magenta" => Color.Magenta,
                "orange" => Color.Orange,
                "purple" => Color.Purple,
                _ => DefaultColor
            };
        }

        private string NormalizeFloat(string value) => 
            value?.Replace('.', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) 
            ?? "1";

        // Control de reproducción

        public void Start()
        {
            if (IsComplete) Reset();
            IsPlaying = true;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Resume()
        {
            IsPlaying = !IsComplete;
        }

        public void Reset()
        {
            _currentIndex = 0;
            _timer = 0;
            _pauseTimer = 0;
            IsPlaying = false;
        }

        public void SkipToEnd()
        {
            _currentIndex = _segments.Count;
            IsPlaying = false;
            OnComplete?.Invoke();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPlaying || IsComplete) return;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Pausa activa
            if (_pauseTimer > 0)
            {
                _pauseTimer -= delta;
                if (_pauseTimer > 0) return;
                // Continúa al terminar pausa
            }

            // Obtiene velocidad actual del segmento o default
            float speed = _currentIndex < _segments.Count 
                ? _segments[_currentIndex].Speed 
                : DefaultSpeed;

            if (speed <= 0) speed = float.MaxValue; // Instantáneo

            _timer += delta * speed;

            while (_timer >= 1f && !IsComplete && _pauseTimer <= 0)
            {
                _timer -= 1f;
                Advance();
            }
        }

        private void Advance()
        {
            if (_currentIndex >= _segments.Count) return;

            var seg = _segments[_currentIndex];

            switch (seg.Type)
            {
                case SegmentType.Wait:
                    _pauseTimer = seg.Delay;
                    _currentIndex++;
                    return;

                case SegmentType.Pause:
                    Pause();
                    _currentIndex++;
                    return;

                case SegmentType.Trigger:
                    OnTagTriggered?.Invoke(seg.Text);
                    _currentIndex++;
                    break;

                default:
                    _currentIndex++;
                    OnSegmentRevealed?.Invoke();
                    break;
            }

            if (IsComplete)
            {
                IsPlaying = false;
                OnComplete?.Invoke();
            }
        }

        // Contexto de parsing con stacks para anidamiento
        private class ParseContext
        {
            public Color Color;
            public bool Bold;
            public bool Italic;
            public float Scale;
            public float Speed;
            
            public Stack<Color> ColorStack = new();
            public Stack<bool> BoldStack = new();
            public Stack<bool> ItalicStack = new();
            public Stack<float> ScaleStack = new();
            public Stack<float> SpeedStack = new();
        }
    }

    public enum RevealMode { Character, Word }

    public enum SegmentType { Character, Word, Newline, Tab, Wait, Pause, Trigger }

    public class TextSegment
    {
        public int Index;
        public SegmentType Type;
        public string Text;
        public Color Color;
        public bool Bold;
        public bool Italic;
        public float Scale = 1f;
        public float Speed = 20f;
        public float Delay;
    }

    #endregion

    #region Componente Visual

    /// <summary>
    /// Componente visual que usa TypewriterInterpreter para mostrar texto con efecto máquina de escribir.
    /// </summary>
    public class TypewriterLabel : StyledComponent
    {
        public SpriteFont Font { get; set; }
        public Color DefaultColor { get; set; } = Color.White;
        
        /// <summary>
        /// Activa/desactiva el efecto typewriter. Si es false, muestra todo el texto instantáneamente.
        /// </summary>
        public bool IsTypewriterEnabled { get; set; } = true;
        
        /// <summary>
        /// Saltos de línea automáticos si el texto excede el ancho disponible.
        /// </summary>
        public bool WordWrap { get; set; } = true;
        
        /// <summary>
        /// Alineación horizontal del texto.
        /// </summary>
        public TextAlignment HorizontalAlignment { get; set; } = TextAlignment.Left;

        // Intérprete interno
        public TypewriterInterpreter Interpreter { get; } = new();
        
        // Eventos reexpuestos
        public event Action<string> OnTagTriggered
        {
            add => Interpreter.OnTagTriggered += value;
            remove => Interpreter.OnTagTriggered -= value;
        }
        
        public event Action OnComplete
        {
            add => Interpreter.OnComplete += value;
            remove => Interpreter.OnComplete -= value;
        }
        
        public event Action OnCharacterRevealed
        {
            add => Interpreter.OnSegmentRevealed += value;
            remove => Interpreter.OnSegmentRevealed -= value;
        }

        // Estado visual
        private readonly List<RenderedLine> _lines = new();
        private bool _needsLayout = true;
        private string _pendingText;

        public TypewriterLabel() : base()
        {
            Interpreter.OnSegmentRevealed += () => _needsLayout = true;
        }

        public void SetText(string text, RevealMode? mode = null)
        {
            _pendingText = text;
            Interpreter.Parse(text, mode);
            
            if (!IsTypewriterEnabled)
            {
                // Modo instantáneo: avanza hasta el final sin animar
                while (!Interpreter.IsComplete)
                {
                    // Hack: fuerza avance sin timing
                    typeof(TypewriterInterpreter).GetMethod("Advance", 
                        System.Reflection.BindingFlags.NonPublic | 
                        System.Reflection.BindingFlags.Instance)?.Invoke(Interpreter, null);
                }
            }
            else
            {
                Interpreter.Reset();
            }
            
            _needsLayout = true;
        }

        public void AppendText(string text)
        {
            SetText(_pendingText + text);
        }

        public void Start() => Interpreter.Start();
        public void Pause() => Interpreter.Pause();
        public void Resume() => Interpreter.Resume();
        public void SkipToEnd() => Interpreter.SkipToEnd();
        public void Reset() 
        { 
            Interpreter.Reset();
            _needsLayout = true;
        }

        protected override void UpdateSelf(GameTime gameTime)
        {
            if (IsTypewriterEnabled)
            {
                Interpreter.Update(gameTime);
            }
        }

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();
            
            if (_needsLayout && Font != null)
            {
                PerformLayout();
                _needsLayout = false;
            }
        }

        private void PerformLayout()
        {
            _lines.Clear();
            var segments = Interpreter.Segments;
            if (segments.Count == 0 || Font == null) return;

            var content = ContentBounds;
            float maxWidth = Math.Max(1, content.Width);
            
            var currentLine = new RenderedLine();
            float currentX = 0;
            float lineHeight = Font.LineSpacing;

            for (int i = 0; i < segments.Count; i++)
            {
                var seg = segments[i];
                
                if (seg.Type == SegmentType.Newline)
                {
                    FinalizeLine(currentLine, ref currentX, ref lineHeight);
                    currentLine = new RenderedLine();
                    currentX = 0;
                    lineHeight = Font.LineSpacing;
                }
                else if (seg.Type == SegmentType.Tab)
                {
                    float tabWidth = Font.MeasureString("    ").X * seg.Scale;
                    float nextTab = (float)Math.Floor((currentX + tabWidth) / tabWidth) * tabWidth;
                    currentX = nextTab;
                }
                else if (seg.Type == SegmentType.Character || seg.Type == SegmentType.Word)
                {
                    var size = Font.MeasureString(seg.Text) * seg.Scale;
                    
                    // Word wrap
                    if (WordWrap && currentX + size.X > maxWidth && currentLine.Segments.Count > 0)
                    {
                        FinalizeLine(currentLine, ref currentX, ref lineHeight);
                        currentLine = new RenderedLine();
                        currentX = 0;
                        lineHeight = Font.LineSpacing;
                    }
                    
                    // Si palabra muy larga, fuerza corte (simplificación)
                    if (WordWrap && size.X > maxWidth)
                    {
                        size.X = maxWidth;
                    }

                    currentLine.Segments.Add(new RenderedSegment
                    {
                        SourceIndex = i,
                        Text = seg.Text,
                        Color = seg.Color,
                        Bold = seg.Bold,
                        Scale = seg.Scale,
                        Size = size,
                        X = currentX
                    });
                    
                    currentX += size.X;
                    lineHeight = Math.Max(lineHeight, size.Y);
                }
                // Wait, Pause, Trigger no ocupan espacio visual
            }
            
            if (currentLine.Segments.Count > 0)
                FinalizeLine(currentLine, ref currentX, ref lineHeight);
        }

        private void FinalizeLine(RenderedLine line, ref float currentX, ref float lineHeight)
        {
            line.Width = currentX;
            line.Height = lineHeight;
            _lines.Add(line);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Font == null || _lines.Count == 0) return;

            var content = ContentBounds;
            float currentY = content.Y;

            foreach (var line in _lines)
            {
                if (currentY > content.Bottom) break;

                DrawLine(spriteBatch, line, content, currentY);
                currentY += line.Height;
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, RenderedLine line, Rectangle content, float y)
        {
            float x = content.X;
            
            // Alineación
            if (HorizontalAlignment == TextAlignment.Center)
                x += (content.Width - line.Width) / 2;
            else if (HorizontalAlignment == TextAlignment.Right)
                x += content.Width - line.Width;

            foreach (var seg in line.Segments)
            {
                // Solo dibuja si ya está revelado
                if (seg.SourceIndex >= Interpreter.CurrentIndex)
                    return;

                var color = seg.Color;
                
                // Efecto cursor en último carácter visible durante reproducción
                if (Interpreter.IsPlaying && seg.SourceIndex == Interpreter.CurrentIndex - 1)
                {
                    float pulse = 0.5f + (float)Math.Sin(Environment.TickCount / 200f) * 0.5f;
                    color = Color.Lerp(color, Color.White, pulse * 0.5f);
                }

                Vector2 pos = new Vector2(x + seg.X, y);
                
                // Bold simulado
                if (seg.Bold)
                {
                    spriteBatch.DrawString(Font, seg.Text, pos + Vector2.One, color * 0.7f, 
                        0, Vector2.Zero, seg.Scale, SpriteEffects.None, 0);
                }
                
                spriteBatch.DrawString(Font, seg.Text, pos, color, 
                    0, Vector2.Zero, seg.Scale, SpriteEffects.None, 0);
            }
        }

        // Estructuras de layout
        private class RenderedLine
        {
            public List<RenderedSegment> Segments = new();
            public float Width;
            public float Height;
        }

        private class RenderedSegment
        {
            public int SourceIndex;
            public string Text;
            public Color Color;
            public bool Bold;
            public float Scale;
            public float X;
            public Vector2 Size;
        }
    }

    public enum TextAlignment { Left, Center, Right }
    
    #endregion
}
```

## Ejemplo de uso con eventos:

```csharp
var dialog = new TypewriterLabel
{
    RelativeBounds = new RelativeRect(0.1f, 0.6f, 0.8f, 0.3f),
    Font = Content.Load<SpriteFont>("Fonts/Dialog"),
    IsTypewriterEnabled = true,
    WordWrap = true
};

// Evento externo configurado
dialog.OnTagTriggered += (tagContent) =>
{
    switch (tagContent)
    {
        case "shake_camera":
            camera.Shake(0.5f);
            break;
        case "play_sound_thunder":
            audio.Play("thunder");
            break;
        case "spawn_enemy":
            spawner.Spawn("goblin");
            break;
        default:
            Console.WriteLine($"Tag desconocido: {tagContent}");
            break;
    }
};

dialog.OnComplete += () => Console.WriteLine("Diálogo terminado");

// Texto con velocidad variable y triggers
dialog.SetText(
    "[speed=30]¡Alerta![/speed] [wait=0.3]\n" +
    "[color=red]Enemigos[/color] detectados al [b]norte[/b].\n" +
    "[trigger=shake_camera][trigger=play_sound_thunder]\n" +
    "[speed=10]Prepárate...[pause]"
);

dialog.Start();
```

## BBCode soportado:

| Tag | Descripción |
|-----|-------------|
| `[speed=chars/s]` | Cambia velocidad temporalmente |
| `[trigger=nombre]` | Dispara `OnTagTriggered("nombre")` |
| `[wait=segundos]` | Pausa temporal |
| `[pause]` | Pausa hasta `Resume()` |
| `[color=nombre/#RRGGBB]` | Color del texto |
| `[b]...[/b]` | Negrita |
| `[i]...[/i]` | Itálica (placeholder) |
| `[size=multiplicador]` | Escala del texto |

¿Necesitas que añada soporte para sonidos por carácter o variaciones de pitch?
User: No utilices sentencias break o continue
Kimi: Aquí está el código refactorizado sin `break` ni `continue`:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MonoGame.UI.Components
{
    #region Interpreter

    public class TypewriterInterpreter
    {
        public RevealMode DefaultMode { get; set; } = RevealMode.Character;
        public Color DefaultColor { get; set; } = Color.White;
        public float DefaultSpeed { get; set; } = 20f;
        
        public event Action<string> OnTagTriggered;
        public event Action OnComplete;
        public event Action OnSegmentRevealed;

        public bool IsComplete => _currentIndex >= _segments.Count;
        public bool IsPlaying { get; private set; }
        public float Progress => _segments.Count > 0 ? _currentIndex / (float)_segments.Count : 1f;
        public int CurrentIndex => _currentIndex;
        public IReadOnlyList<TextSegment> Segments => _segments;

        private readonly List<TextSegment> _segments = new();
        private int _currentIndex;
        private float _timer;
        private float _pauseTimer;
        private float _currentSpeed;
        private RevealMode _currentMode;

        public void Parse(string text, RevealMode? mode = null)
        {
            _segments.Clear();
            _currentSpeed = DefaultSpeed;
            _currentMode = mode ?? DefaultMode;

            var context = new ParseContext
            {
                Color = DefaultColor,
                Bold = false,
                Italic = false,
                Scale = 1f,
                Speed = DefaultSpeed
            };

            var regex = new Regex(@"\[(\w+)(?:=([^\]]+))?\]|\[/(\w+)\]|(\\n)|(\\t)|([^\[\\\n\t]+)");
            var matches = regex.Matches(text ?? "");

            foreach (Match match in matches)
            {
                ProcessMatch(match, context);
            }
        }

        private void ProcessMatch(Match match, ParseContext ctx)
        {
            if (match.Groups[4].Success)
            {
                _segments.Add(new TextSegment { Type = SegmentType.Newline });
                return;
            }

            if (match.Groups[5].Success)
            {
                _segments.Add(new TextSegment { Type = SegmentType.Tab });
                return;
            }

            if (match.Groups[6].Success)
            {
                AddText(match.Groups[6].Value, ctx);
                return;
            }

            if (match.Groups[1].Success)
            {
                ProcessOpenTag(match.Groups[1].Value, match.Groups[2].Value, ctx);
                return;
            }

            if (match.Groups[3].Success)
            {
                ProcessCloseTag(match.Groups[3].Value, ctx);
            }
        }

        private void AddText(string text, ParseContext ctx)
        {
            if (_currentMode == RevealMode.Word)
            {
                var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {
                    bool last = i == words.Length - 1;
                    AddSegment(new TextSegment
                    {
                        Type = SegmentType.Word,
                        Text = words[i] + (last ? "" : " "),
                        Color = ctx.Color,
                        Bold = ctx.Bold,
                        Italic = ctx.Italic,
                        Scale = ctx.Scale,
                        Speed = ctx.Speed
                    });
                }
            }
            else
            {
                foreach (char c in text)
                {
                    AddSegment(new TextSegment
                    {
                        Type = SegmentType.Character,
                        Text = c.ToString(),
                        Color = ctx.Color,
                        Bold = ctx.Bold,
                        Italic = ctx.Italic,
                        Scale = ctx.Scale,
                        Speed = ctx.Speed
                    });
                }
            }
        }

        private void AddSegment(TextSegment seg)
        {
            seg.Index = _segments.Count;
            _segments.Add(seg);
        }

        private void ProcessOpenTag(string tag, string value, ParseContext ctx)
        {
            string t = tag.ToLower();

            if (t == "color" || t == "c")
            {
                ctx.ColorStack.Push(ctx.Color);
                ctx.Color = ParseColor(value);
                return;
            }

            if (t == "b" || t == "bold")
            {
                ctx.BoldStack.Push(ctx.Bold);
                ctx.Bold = true;
                return;
            }

            if (t == "i" || t == "italic")
            {
                ctx.ItalicStack.Push(ctx.Italic);
                ctx.Italic = true;
                return;
            }

            if (t == "size" || t == "s")
            {
                ctx.ScaleStack.Push(ctx.Scale);
                if (float.TryParse(NormalizeFloat(value), out float scale))
                {
                    ctx.Scale = MathHelper.Clamp(scale, 0.5f, 3f);
                }
                return;
            }

            if (t == "speed")
            {
                ctx.SpeedStack.Push(ctx.Speed);
                if (float.TryParse(NormalizeFloat(value), out float speed))
                {
                    ctx.Speed = Math.Max(0.1f, speed);
                }
                return;
            }

            if ((t == "wait" || t == "w") && float.TryParse(NormalizeFloat(value), out float wait))
            {
                _segments.Add(new TextSegment 
                { 
                    Type = SegmentType.Wait, 
                    Delay = wait,
                    Index = _segments.Count
                });
                return;
            }

            if (t == "pause" || t == "p")
            {
                _segments.Add(new TextSegment 
                { 
                    Type = SegmentType.Pause,
                    Index = _segments.Count
                });
                return;
            }

            if (t == "trigger" || t == "t" || t == "event" || t == "e")
            {
                _segments.Add(new TextSegment 
                { 
                    Type = SegmentType.Trigger,
                    Text = value ?? "",
                    Index = _segments.Count
                });
            }
        }

        private void ProcessCloseTag(string tag, ParseContext ctx)
        {
            string t = tag.ToLower();

            if ((t == "color" || t == "c") && ctx.ColorStack.Count > 0)
            {
                ctx.Color = ctx.ColorStack.Pop();
                return;
            }

            if ((t == "b" || t == "bold") && ctx.BoldStack.Count > 0)
            {
                ctx.Bold = ctx.BoldStack.Pop();
                return;
            }

            if ((t == "i" || t == "italic") && ctx.ItalicStack.Count > 0)
            {
                ctx.Italic = ctx.ItalicStack.Pop();
                return;
            }

            if ((t == "size" || t == "s") && ctx.ScaleStack.Count > 0)
            {
                ctx.Scale = ctx.ScaleStack.Pop();
                return;
            }

            if (t == "speed" && ctx.SpeedStack.Count > 0)
            {
                ctx.Speed = ctx.SpeedStack.Pop();
            }
        }

        private Color ParseColor(string value)
        {
            if (string.IsNullOrEmpty(value)) return DefaultColor;
            
            if (value.StartsWith("#"))
            {
                try
                {
                    byte a = value.Length == 9 ? Convert.ToByte(value.Substring(1, 2), 16) : (byte)255;
                    int start = value.Length == 9 ? 3 : 1;
                    byte r = Convert.ToByte(value.Substring(start, 2), 16);
                    byte g = Convert.ToByte(value.Substring(start + 2, 2), 16);
                    byte b = Convert.ToByte(value.Substring(start + 4, 2), 16);
                    return new Color(r, g, b, a);
                }
                catch { return DefaultColor; }
            }
            
            return value.ToLower() switch
            {
                "red" => Color.Red,
                "green" => Color.Green,
                "blue" => Color.Blue,
                "yellow" => Color.Yellow,
                "white" => Color.White,
                "black" => Color.Black,
                "gray" or "grey" => Color.Gray,
                "cyan" => Color.Cyan,
                "magenta" => Color.Magenta,
                "orange" => Color.Orange,
                "purple" => Color.Purple,
                _ => DefaultColor
            };
        }

        private string NormalizeFloat(string value) => 
            value?.Replace('.', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) 
            ?? "1";

        public void Start()
        {
            if (IsComplete) Reset();
            IsPlaying = true;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Resume()
        {
            IsPlaying = !IsComplete;
        }

        public void Reset()
        {
            _currentIndex = 0;
            _timer = 0;
            _pauseTimer = 0;
            IsPlaying = false;
        }

        public void SkipToEnd()
        {
            _currentIndex = _segments.Count;
            IsPlaying = false;
            OnComplete?.Invoke();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPlaying || IsComplete) return;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_pauseTimer > 0)
            {
                _pauseTimer -= delta;
                if (_pauseTimer > 0) return;
            }

            float speed = _currentIndex < _segments.Count 
                ? _segments[_currentIndex].Speed 
                : DefaultSpeed;

            if (speed <= 0) speed = float.MaxValue;

            _timer += delta * speed;

            bool shouldContinue = _timer >= 1f && !IsComplete && _pauseTimer <= 0;
            while (shouldContinue)
            {
                _timer -= 1f;
                Advance();
                shouldContinue = _timer >= 1f && !IsComplete && _pauseTimer <= 0;
            }
        }

        private void Advance()
        {
            if (_currentIndex >= _segments.Count) return;

            var seg = _segments[_currentIndex];

            if (seg.Type == SegmentType.Wait)
            {
                _pauseTimer = seg.Delay;
                _currentIndex++;
                return;
            }

            if (seg.Type == SegmentType.Pause)
            {
                Pause();
                _currentIndex++;
                return;
            }

            if (seg.Type == SegmentType.Trigger)
            {
                OnTagTriggered?.Invoke(seg.Text);
                _currentIndex++;
            }
            else
            {
                _currentIndex++;
                OnSegmentRevealed?.Invoke();
            }

            if (IsComplete)
            {
                IsPlaying = false;
                OnComplete?.Invoke();
            }
        }

        private class ParseContext
        {
            public Color Color;
            public bool Bold;
            public bool Italic;
            public float Scale;
            public float Speed;
            
            public Stack<Color> ColorStack = new();
            public Stack<bool> BoldStack = new();
            public Stack<bool> ItalicStack = new();
            public Stack<float> ScaleStack = new();
            public Stack<float> SpeedStack = new();
        }
    }

    public enum RevealMode { Character, Word }

    public enum SegmentType { Character, Word, Newline, Tab, Wait, Pause, Trigger }

    public class TextSegment
    {
        public int Index;
        public SegmentType Type;
        public string Text;
        public Color Color;
        public bool Bold;
        public bool Italic;
        public float Scale = 1f;
        public float Speed = 20f;
        public float Delay;
    }

    #endregion

    #region Componente Visual

    public class TypewriterLabel : StyledComponent
    {
        public SpriteFont Font { get; set; }
        public Color DefaultColor { get; set; } = Color.White;
        public bool IsTypewriterEnabled { get; set; } = true;
        public bool WordWrap { get; set; } = true;
        public TextAlignment HorizontalAlignment { get; set; } = TextAlignment.Left;

        public TypewriterInterpreter Interpreter { get; } = new();
        
        public event Action<string> OnTagTriggered
        {
            add => Interpreter.OnTagTriggered += value;
            remove => Interpreter.OnTagTriggered -= value;
        }
        
        public event Action OnComplete
        {
            add => Interpreter.OnComplete += value;
            remove => Interpreter.OnComplete -= value;
        }
        
        public event Action OnCharacterRevealed
        {
            add => Interpreter.OnSegmentRevealed += value;
            remove => Interpreter.OnSegmentRevealed -= value;
        }

        private readonly List<RenderedLine> _lines = new();
        private bool _needsLayout = true;
        private string _pendingText;

        public TypewriterLabel() : base()
        {
            Interpreter.OnSegmentRevealed += () => _needsLayout = true;
        }

        public void SetText(string text, RevealMode? mode = null)
        {
            _pendingText = text;
            Interpreter.Parse(text, mode);
            
            if (!IsTypewriterEnabled)
            {
                ForceComplete();
            }
            else
            {
                Interpreter.Reset();
            }
            
            _needsLayout = true;
        }

        private void ForceComplete()
        {
            while (!Interpreter.IsComplete)
            {
                AdvanceInterpreter();
            }
        }

        private void AdvanceInterpreter()
        {
            var method = typeof(TypewriterInterpreter).GetMethod("Advance", 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance);
            method?.Invoke(Interpreter, null);
        }

        public void AppendText(string text)
        {
            SetText(_pendingText + text);
        }

        public void Start() => Interpreter.Start();
        public void Pause() => Interpreter.Pause();
        public void Resume() => Interpreter.Resume();
        public void SkipToEnd() => Interpreter.SkipToEnd();
        
        public void Reset() 
        { 
            Interpreter.Reset();
            _needsLayout = true;
        }

        protected override void UpdateSelf(GameTime gameTime)
        {
            if (IsTypewriterEnabled)
            {
                Interpreter.Update(gameTime);
            }
        }

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();
            
            if (_needsLayout && Font != null)
            {
                PerformLayout();
                _needsLayout = false;
            }
        }

        private void PerformLayout()
        {
            _lines.Clear();
            var segments = Interpreter.Segments;
            
            if (segments.Count == 0 || Font == null) return;

            var content = ContentBounds;
            float maxWidth = Math.Max(1, content.Width);
            
            var currentLine = new RenderedLine();
            float currentX = 0;
            float lineHeight = Font.LineSpacing;

            for (int i = 0; i < segments.Count; i++)
            {
                ProcessSegmentForLayout(segments[i], ref currentLine, ref currentX, ref lineHeight, maxWidth);
            }
            
            if (currentLine.Segments.Count > 0)
            {
                FinalizeLine(currentLine, currentX, lineHeight);
            }
        }

        private void ProcessSegmentForLayout(TextSegment seg, ref RenderedLine currentLine, 
            ref float currentX, ref float lineHeight, float maxWidth)
        {
            if (seg.Type == SegmentType.Newline)
            {
                FinalizeLine(currentLine, currentX, lineHeight);
                currentLine = new RenderedLine();
                currentX = 0;
                lineHeight = Font.LineSpacing;
                return;
            }

            if (seg.Type == SegmentType.Tab)
            {
                float tabWidth = Font.MeasureString("    ").X * seg.Scale;
                float nextTab = (float)Math.Floor((currentX + tabWidth) / tabWidth) * tabWidth;
                currentX = nextTab;
                return;
            }

            if (seg.Type != SegmentType.Character && seg.Type != SegmentType.Word)
            {
                return;
            }

            var size = Font.MeasureString(seg.Text) * seg.Scale;
            
            bool needsWrap = WordWrap && currentX + size.X > maxWidth && currentLine.Segments.Count > 0;
            if (needsWrap)
            {
                FinalizeLine(currentLine, currentX, lineHeight);
                currentLine = new RenderedLine();
                currentX = 0;
                lineHeight = Font.LineSpacing;
            }
            
            if (WordWrap && size.X > maxWidth)
            {
                size.X = maxWidth;
            }

            currentLine.Segments.Add(new RenderedSegment
            {
                SourceIndex = seg.Index,
                Text = seg.Text,
                Color = seg.Color,
                Bold = seg.Bold,
                Scale = seg.Scale,
                Size = size,
                X = currentX
            });
            
            currentX += size.X;
            lineHeight = Math.Max(lineHeight, size.Y);
        }

        private void FinalizeLine(RenderedLine line, float currentX, float lineHeight)
        {
            line.Width = currentX;
            line.Height = lineHeight;
            _lines.Add(line);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Font == null || _lines.Count == 0) return;

            var content = ContentBounds;
            float currentY = content.Y;

            for (int i = 0; i < _lines.Count && currentY <= content.Bottom; i++)
            {
                DrawLine(spriteBatch, _lines[i], content, currentY);
                currentY += _lines[i].Height;
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, RenderedLine line, Rectangle content, float y)
        {
            float x = content.X;
            
            if (HorizontalAlignment == TextAlignment.Center)
            {
                x += (content.Width - line.Width) / 2;
            }
            else if (HorizontalAlignment == TextAlignment.Right)
            {
                x += content.Width - line.Width;
            }

            for (int i = 0; i < line.Segments.Count; i++)
            {
                var seg = line.Segments[i];
                
                if (seg.SourceIndex >= Interpreter.CurrentIndex)
                {
                    return;
                }

                var color = seg.Color;
                
                bool isLastRevealed = seg.SourceIndex == Interpreter.CurrentIndex - 1;
                if (Interpreter.IsPlaying && isLastRevealed)
                {
                    float pulse = 0.5f + (float)Math.Sin(Environment.TickCount / 200f) * 0.5f;
                    color = Color.Lerp(color, Color.White, pulse * 0.5f);
                }

                Vector2 pos = new Vector2(x + seg.X, y);
                
                if (seg.Bold)
                {
                    spriteBatch.DrawString(Font, seg.Text, pos + Vector2.One, color * 0.7f, 
                        0, Vector2.Zero, seg.Scale, SpriteEffects.None, 0);
                }
                
                spriteBatch.DrawString(Font, seg.Text, pos, color, 
                    0, Vector2.Zero, seg.Scale, SpriteEffects.None, 0);
            }
        }

        private class RenderedLine
        {
            public List<RenderedSegment> Segments = new();
            public float Width;
            public float Height;
        }

        private class RenderedSegment
        {
            public int SourceIndex;
            public string Text;
            public Color Color;
            public bool Bold;
            public float Scale;
            public float X;
            public Vector2 Size;
        }
    }

    public enum TextAlignment { Left, Center, Right }
    
    #endregion
}
```

## Cambios realizados sin `break`/`continue`:

| Antes | Después |
|-------|---------|
| `switch` con `break` | `if` anidados con `return` temprano |
| `while` con `break` | `while` con condición booleana |
| `foreach` con `continue` | Múltiples `if` con `return` temprano |
| Loop de layout con `break` | Condición en el `for` |
| Búsqueda con `break` | Retorno temprano cuando se encuentra |

La lógica se mantiene idéntica, solo cambia la estructura de control.
User: El intérprete debe devolver una lista de secciones o partes de la cadena. Es el componente quien debe llevar la cuenta de hasta donde ha llegado la presentación, es decir, debemos eliminar los métodos avance y similares del intérprete
Kimi: Aquí está el código refactorizado con el intérprete como generador puro de secciones y el componente gestionando todo el estado de presentación:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace MonoGame.UI.Components
{
    #region Data Structures

    public enum RevealMode { Character, Word }

    public enum SegmentType { Character, Word, Newline, Tab, Wait, Pause, Trigger }

    public class TextSegment
    {
        public int Index;
        public SegmentType Type;
        public string Text;
        public Color Color;
        public bool Bold;
        public bool Italic;
        public float Scale = 1f;
        public float Speed = 20f;
        public float Delay;
    }

    public class TypewriterSection
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public float BaseSpeed { get; set; }
        public IReadOnlyList<TextSegment> Segments { get; set; }
    }

    #endregion

    #region Interpreter (Pure Generator)

    /// <summary>
    /// Intérprete puro: recibe texto con BBCode y devuelve lista de secciones procesadas.
    /// No mantiene estado de ejecución.
    /// </summary>
    public class TypewriterInterpreter
    {
        public RevealMode DefaultMode { get; set; } = RevealMode.Character;
        public Color DefaultColor { get; set; } = Color.White;
        public float DefaultSpeed { get; set; } = 20f;

        /// <summary>
        /// Parsea el texto y devuelve secciones listas para ser presentadas.
        /// Cada sección representa una unidad lógica de presentación (por ejemplo, entre pausas).
        /// </summary>
        public List<TypewriterSection> Parse(string text, RevealMode? mode = null)
        {
            var sections = new List<TypewriterSection>();
            var allSegments = new List<TextSegment>();
            
            var ctx = new ParseContext
            {
                Color = DefaultColor,
                Bold = false,
                Italic = false,
                Scale = 1f,
                Speed = DefaultSpeed
            };

            RevealMode currentMode = mode ?? DefaultMode;
            int currentIndex = 0;

            var regex = new Regex(@"\[(\w+)(?:=([^\]]+))?\]|\[/(\w+)\]|(\\n)|(\\t)|([^\[\\\n\t]+)");
            var matches = regex.Matches(text ?? "");

            var currentSectionSegments = new List<TextSegment>();
            float sectionBaseSpeed = DefaultSpeed;

            foreach (Match match in matches)
            {
                var result = ProcessMatch(match, ctx, currentMode, currentIndex);
                
                foreach (var seg in result.Segments)
                {
                    seg.Index = currentIndex++;
                    currentSectionSegments.Add(seg);
                    allSegments.Add(seg);

                    if (seg.Type == SegmentType.Pause || seg.Type == SegmentType.Wait)
                    {
                        // Finaliza sección actual
                        sections.Add(new TypewriterSection
                        {
                            StartIndex = currentSectionSegments[0].Index,
                            EndIndex = seg.Index,
                            BaseSpeed = sectionBaseSpeed,
                            Segments = new ReadOnlyCollection<TextSegment>(currentSectionSegments.ToArray())
                        });

                        currentSectionSegments = new List<TextSegment>();
                        sectionBaseSpeed = seg.Type == SegmentType.Wait ? DefaultSpeed : ctx.Speed;
                    }

                    if (seg.Type == SegmentType.Trigger)
                    {
                        // Los triggers terminan sección para permitir sincronización
                        sections.Add(new TypewriterSection
                        {
                            StartIndex = currentSectionSegments[0].Index,
                            EndIndex = seg.Index,
                            BaseSpeed = sectionBaseSpeed,
                            Segments = new ReadOnlyCollection<TextSegment>(currentSectionSegments.ToArray())
                        });

                        currentSectionSegments = new List<TextSegment>();
                    }
                }

                if (result.NewSectionSpeed.HasValue)
                {
                    sectionBaseSpeed = result.NewSectionSpeed.Value;
                }
            }

            // Añade sección final si queda contenido
            if (currentSectionSegments.Count > 0)
            {
                sections.Add(new TypewriterSection
                {
                    StartIndex = currentSectionSegments[0].Index,
                    EndIndex = currentSectionSegments[currentSectionSegments.Count - 1].Index,
                    BaseSpeed = sectionBaseSpeed,
                    Segments = new ReadOnlyCollection<TextSegment>(currentSectionSegments.ToArray())
                });
            }

            return sections;
        }

        private ProcessResult ProcessMatch(Match match, ParseContext ctx, RevealMode mode, int baseIndex)
        {
            var result = new ProcessResult();

            if (match.Groups[4].Success)
            {
                result.Segments.Add(new TextSegment { Type = SegmentType.Newline });
                return result;
            }

            if (match.Groups[5].Success)
            {
                result.Segments.Add(new TextSegment { Type = SegmentType.Tab });
                return result;
            }

            if (match.Groups[6].Success)
            {
                result.Segments.AddRange(CreateTextSegments(match.Groups[6].Value, ctx, mode));
                return result;
            }

            if (match.Groups[1].Success)
            {
                ProcessOpenTag(match.Groups[1].Value, match.Groups[2].Value, ctx, result);
                return result;
            }

            if (match.Groups[3].Success)
            {
                ProcessCloseTag(match.Groups[3].Value, ctx);
                return result;
            }

            return result;
        }

        private List<TextSegment> CreateTextSegments(string text, ParseContext ctx, RevealMode mode)
        {
            var segments = new List<TextSegment>();

            if (mode == RevealMode.Word)
            {
                var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++)
                {
                    bool last = i == words.Length - 1;
                    segments.Add(new TextSegment
                    {
                        Type = SegmentType.Word,
                        Text = words[i] + (last ? "" : " "),
                        Color = ctx.Color,
                        Bold = ctx.Bold,
                        Italic = ctx.Italic,
                        Scale = ctx.Scale,
                        Speed = ctx.Speed
                    });
                }
            }
            else
            {
                foreach (char c in text)
                {
                    segments.Add(new TextSegment
                    {
                        Type = SegmentType.Character,
                        Text = c.ToString(),
                        Color = ctx.Color,
                        Bold = ctx.Bold,
                        Italic = ctx.Italic,
                        Scale = ctx.Scale,
                        Speed = ctx.Speed
                    });
                }
            }

            return segments;
        }

        private void ProcessOpenTag(string tag, string value, ParseContext ctx, ProcessResult result)
        {
            string t = tag.ToLower();

            if (t == "color" || t == "c")
            {
                ctx.ColorStack.Push(ctx.Color);
                ctx.Color = ParseColor(value);
                return;
            }

            if (t == "b" || t == "bold")
            {
                ctx.BoldStack.Push(ctx.Bold);
                ctx.Bold = true;
                return;
            }

            if (t == "i" || t == "italic")
            {
                ctx.ItalicStack.Push(ctx.Italic);
                ctx.Italic = true;
                return;
            }

            if (t == "size" || t == "s")
            {
                ctx.ScaleStack.Push(ctx.Scale);
                if (float.TryParse(NormalizeFloat(value), out float scale))
                {
                    ctx.Scale = MathHelper.Clamp(scale, 0.5f, 3f);
                }
                return;
            }

            if (t == "speed")
            {
                ctx.SpeedStack.Push(ctx.Speed);
                if (float.TryParse(NormalizeFloat(value), out float speed))
                {
                    ctx.Speed = Math.Max(0.1f, speed);
                    result.NewSectionSpeed = speed;
                }
                return;
            }

            if ((t == "wait" || t == "w") && float.TryParse(NormalizeFloat(value), out float wait))
            {
                result.Segments.Add(new TextSegment 
                { 
                    Type = SegmentType.Wait, 
                    Delay = wait
                });
                return;
            }

            if (t == "pause" || t == "p")
            {
                result.Segments.Add(new TextSegment 
                { 
                    Type = SegmentType.Pause
                });
                return;
            }

            if (t == "trigger" || t == "t" || t == "event" || t == "e")
            {
                result.Segments.Add(new TextSegment 
                { 
                    Type = SegmentType.Trigger,
                    Text = value ?? ""
                });
            }
        }

        private void ProcessCloseTag(string tag, ParseContext ctx)
        {
            string t = tag.ToLower();

            if ((t == "color" || t == "c") && ctx.ColorStack.Count > 0)
            {
                ctx.Color = ctx.ColorStack.Pop();
                return;
            }

            if ((t == "b" || t == "bold") && ctx.BoldStack.Count > 0)
            {
                ctx.Bold = ctx.BoldStack.Pop();
                return;
            }

            if ((t == "i" || t == "italic") && ctx.ItalicStack.Count > 0)
            {
                ctx.Italic = ctx.ItalicStack.Pop();
                return;
            }

            if ((t == "size" || t == "s") && ctx.ScaleStack.Count > 0)
            {
                ctx.Scale = ctx.ScaleStack.Pop();
                return;
            }

            if (t == "speed" && ctx.SpeedStack.Count > 0)
            {
                ctx.Speed = ctx.SpeedStack.Pop();
            }
        }

        private Color ParseColor(string value)
        {
            if (string.IsNullOrEmpty(value)) return DefaultColor;
            
            if (value.StartsWith("#"))
            {
                try
                {
                    byte a = value.Length == 9 ? Convert.ToByte(value.Substring(1, 2), 16) : (byte)255;
                    int start = value.Length == 9 ? 3 : 1;
                    byte r = Convert.ToByte(value.Substring(start, 2), 16);
                    byte g = Convert.ToByte(value.Substring(start + 2, 2), 16);
                    byte b = Convert.ToByte(value.Substring(start + 4, 2), 16);
                    return new Color(r, g, b, a);
                }
                catch { return DefaultColor; }
            }
            
            return value.ToLower() switch
            {
                "red" => Color.Red,
                "green" => Color.Green,
                "blue" => Color.Blue,
                "yellow" => Color.Yellow,
                "white" => Color.White,
                "black" => Color.Black,
                "gray" or "grey" => Color.Gray,
                "cyan" => Color.Cyan,
                "magenta" => Color.Magenta,
                "orange" => Color.Orange,
                "purple" => Color.Purple,
                _ => DefaultColor
            };
        }

        private string NormalizeFloat(string value) => 
            value?.Replace('.', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]) 
            ?? "1";

        private class ParseContext
        {
            public Color Color;
            public bool Bold;
            public bool Italic;
            public float Scale;
            public float Speed;
            
            public Stack<Color> ColorStack = new();
            public Stack<bool> BoldStack = new();
            public Stack<bool> ItalicStack = new();
            public Stack<float> ScaleStack = new();
            public Stack<float> SpeedStack = new();
        }

        private class ProcessResult
        {
            public List<TextSegment> Segments = new();
            public float? NewSectionSpeed;
        }
    }

    #endregion

    #region Componente Visual (State Manager)

    /// <summary>
    /// Componente que gestiona la presentación progresiva de secciones generadas por TypewriterInterpreter.
    /// Mantiene todo el estado de reproducción.
    /// </summary>
    public class TypewriterLabel : StyledComponent
    {
        public SpriteFont Font { get; set; }
        public Color DefaultColor { get; set; } = Color.White;
        public bool IsTypewriterEnabled { get; set; } = true;
        public bool WordWrap { get; set; } = true;
        public TextAlignment HorizontalAlignment { get; set; } = TextAlignment.Left;

        public event Action<string> OnTagTriggered;
        public event Action OnComplete;
        public event Action<int> OnCharacterRevealed;
        public event Action<int> OnSectionStarted;
        public event Action<int> OnSectionComplete;

        // Intérprete y datos parseados
        public TypewriterInterpreter Interpreter { get; } = new();
        private List<TypewriterSection> _sections = new();
        private List<TextSegment> _allSegments = new();

        // Estado de presentación
        private int _currentSectionIndex;
        private int _currentSegmentIndex;
        private float _timer;
        private float _pauseTimer;
        private bool _isPaused;
        
        public bool IsPlaying { get; private set; }
        public bool IsComplete => _currentSectionIndex >= _sections.Count;
        public int CurrentGlobalIndex => _currentSegmentIndex;
        public float Progress => _allSegments.Count > 0 ? _currentSegmentIndex / (float)_allSegments.Count : 1f;
        public int CurrentSection => _currentSectionIndex;

        // Layout
        private readonly List<RenderedLine> _lines = new();
        private bool _needsLayout = true;

        public void SetText(string text, RevealMode? mode = null)
        {
            _sections = Interpreter.Parse(text, mode);
            
            // Reconstruye lista completa de segmentos para referencia rápida
            _allSegments = new List<TextSegment>();
            foreach (var section in _sections)
            {
                foreach (var seg in section.Segments)
                {
                    _allSegments.Add(seg);
                }
            }

            ResetPresentation();
            
            if (!IsTypewriterEnabled)
            {
                SkipToEnd();
            }
            
            _needsLayout = true;
        }

        public void AppendText(string text)
        {
            // Guarda estado actual
            int oldGlobalIndex = _currentSegmentIndex;
            bool wasPlaying = IsPlaying;

            // Re-parse completo (simplificación; optimizable)
            var currentText = ReconstructText();
            SetText(currentText + text);

            // Restaura posición aproximada
            _currentSegmentIndex = Math.Min(oldGlobalIndex, _allSegments.Count - 1);
            UpdateSectionFromGlobalIndex();
            
            if (wasPlaying) Start();
        }

        private string ReconstructText()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var seg in _allSegments)
            {
                if (seg.Type == SegmentType.Character || seg.Type == SegmentType.Word)
                {
                    sb.Append(seg.Text);
                }
            }
            return sb.ToString();
        }

        public void Start()
        {
            if (IsComplete)
            {
                ResetPresentation();
            }
            IsPlaying = true;
            _isPaused = false;
        }

        public void Pause()
        {
            _isPaused = true;
            IsPlaying = false;
        }

        public void Resume()
        {
            if (!IsComplete && !_isPaused) return;
            
            _isPaused = false;
            IsPlaying = true;
            
            if (_pauseTimer > 0)
            {
                _pauseTimer = 0;
            }
        }

        public void ResetPresentation()
        {
            _currentSectionIndex = 0;
            _currentSegmentIndex = 0;
            _timer = 0;
            _pauseTimer = 0;
            _isPaused = false;
            IsPlaying = false;
        }

        public void SkipToEnd()
        {
            _currentSectionIndex = _sections.Count;
            _currentSegmentIndex = _allSegments.Count;
            IsPlaying = false;
            OnComplete?.Invoke();
        }

        public void SkipSection()
        {
            if (IsComplete) return;
            
            var section = _sections[_currentSectionIndex];
            _currentSegmentIndex = section.EndIndex + 1;
            _currentSectionIndex++;
            
            OnSectionComplete?.Invoke(_currentSectionIndex - 1);
            
            if (_currentSectionIndex < _sections.Count)
            {
                OnSectionStarted?.Invoke(_currentSectionIndex);
            }
            else
            {
                IsPlaying = false;
                OnComplete?.Invoke();
            }
        }

        protected override void UpdateSelf(GameTime gameTime)
        {
            if (!IsPlaying || IsComplete) return;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_pauseTimer > 0)
            {
                _pauseTimer -= delta;
                if (_pauseTimer > 0) return;
            }

            if (_currentSectionIndex >= _sections.Count) return;

            var section = _sections[_currentSectionIndex];
            float speed = section.BaseSpeed;

            if (_currentSegmentIndex < _allSegments.Count)
            {
                speed = _allSegments[_currentSegmentIndex].Speed;
            }

            if (speed <= 0) speed = float.MaxValue;

            _timer += delta * speed;

            while (_timer >= 1f && IsPlaying && !IsComplete && _pauseTimer <= 0)
            {
                _timer -= 1f;
                Advance();
            }
        }

        private void Advance()
        {
            if (_currentSegmentIndex >= _allSegments.Count) return;

            var segment = _allSegments[_currentSegmentIndex];

            if (segment.Type == SegmentType.Wait)
            {
                _pauseTimer = segment.Delay;
                _currentSegmentIndex++;
                return;
            }

            if (segment.Type == SegmentType.Pause)
            {
                Pause();
                _currentSegmentIndex++;
                return;
            }

            if (segment.Type == SegmentType.Trigger)
            {
                OnTagTriggered?.Invoke(segment.Text);
            }

            _currentSegmentIndex++;
            OnCharacterRevealed?.Invoke(_currentSegmentIndex);

            CheckSectionBoundary();
            CheckCompletion();
        }

        private void CheckSectionBoundary()
        {
            if (_currentSectionIndex >= _sections.Count) return;

            var section = _sections[_currentSectionIndex];
            
            if (_currentSegmentIndex > section.EndIndex)
            {
                OnSectionComplete?.Invoke(_currentSectionIndex);
                _currentSectionIndex++;

                if (_currentSectionIndex < _sections.Count)
                {
                    OnSectionStarted?.Invoke(_currentSectionIndex);
                }
            }
        }

        private void CheckCompletion()
        {
            if (_currentSegmentIndex >= _allSegments.Count)
            {
                IsPlaying = false;
                OnComplete?.Invoke();
            }
        }

        private void UpdateSectionFromGlobalIndex()
        {
            for (int i = 0; i < _sections.Count; i++)
            {
                if (_currentSegmentIndex <= _sections[i].EndIndex)
                {
                    _currentSectionIndex = i;
                    return;
                }
            }
            _currentSectionIndex = _sections.Count;
        }

        protected override void CalculateTransformations()
        {
            base.CalculateTransformations();
            
            if (_needsLayout && Font != null)
            {
                PerformLayout();
                _needsLayout = false;
            }
        }

        private void PerformLayout()
        {
            _lines.Clear();
            
            if (_allSegments.Count == 0 || Font == null) return;

            var content = ContentBounds;
            float maxWidth = Math.Max(1, content.Width);
            
            var currentLine = new RenderedLine();
            float currentX = 0;
            float lineHeight = Font.LineSpacing;

            foreach (var seg in _allSegments)
            {
                ProcessSegmentForLayout(seg, ref currentLine, ref currentX, ref lineHeight, maxWidth);
            }
            
            if (currentLine.Segments.Count > 0)
            {
                FinalizeLine(currentLine, currentX, lineHeight);
            }
        }

        private void ProcessSegmentForLayout(TextSegment seg, ref RenderedLine currentLine, 
            ref float currentX, ref float lineHeight, float maxWidth)
        {
            if (seg.Type == SegmentType.Newline)
            {
                FinalizeLine(currentLine, currentX, lineHeight);
                currentLine = new RenderedLine();
                currentX = 0;
                lineHeight = Font.LineSpacing;
                return;
            }

            if (seg.Type == SegmentType.Tab)
            {
                float tabWidth = Font.MeasureString("    ").X * seg.Scale;
                float nextTab = (float)Math.Floor((currentX + tabWidth) / tabWidth) * tabWidth;
                currentX = nextTab;
                return;
            }

            if (seg.Type != SegmentType.Character && seg.Type != SegmentType.Word)
            {
                return;
            }

            var size = Font.MeasureString(seg.Text) * seg.Scale;
            
            bool needsWrap = WordWrap && currentX + size.X > maxWidth && currentLine.Segments.Count > 0;
            if (needsWrap)
            {
                FinalizeLine(currentLine, currentX, lineHeight);
                currentLine = new RenderedLine();
                currentX = 0;
                lineHeight = Font.LineSpacing;
            }
            
            if (WordWrap && size.X > maxWidth)
            {
                size.X = maxWidth;
            }

            currentLine.Segments.Add(new RenderedSegment
            {
                SourceIndex = seg.Index,
                Text = seg.Text,
                Color = seg.Color,
                Bold = seg.Bold,
                Scale = seg.Scale,
                Size = size,
                X = currentX
            });
            
            currentX += size.X;
            lineHeight = Math.Max(lineHeight, size.Y);
        }

        private void FinalizeLine(RenderedLine line, float currentX, float lineHeight)
        {
            line.Width = currentX;
            line.Height = lineHeight;
            _lines.Add(line);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Font == null || _lines.Count == 0) return;

            var content = ContentBounds;
            float currentY = content.Y;

            for (int i = 0; i < _lines.Count && currentY <= content.Bottom; i++)
            {
                DrawLine(spriteBatch, _lines[i], content, currentY);
                currentY += _lines[i].Height;
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, RenderedLine line, Rectangle content, float y)
        {
            float x = content.X;
            
            if (HorizontalAlignment == TextAlignment.Center)
            {
                x += (content.Width - line.Width) / 2;
            }
            else if (HorizontalAlignment == TextAlignment.Right)
            {
                x += content.Width - line.Width;
            }

            for (int i = 0; i < line.Segments.Count; i++)
            {
                var seg = line.Segments[i];
                
                if (seg.SourceIndex >= _currentSegmentIndex)
                {
                    return;
                }

                var color = seg.Color;
                
                bool isLastRevealed = seg.SourceIndex == _currentSegmentIndex - 1;
                if (IsPlaying && isLastRevealed)
                {
                    float pulse = 0.5f + (float)Math.Sin(Environment.TickCount / 200f) * 0.5f;
                    color = Color.Lerp(color, Color.White, pulse * 0.5f);
                }

                Vector2 pos = new Vector2(x + seg.X, y);
                
                if (seg.Bold)
                {
                    spriteBatch.DrawString(Font, seg.Text, pos + Vector2.One, color * 0.7f, 
                        0, Vector2.Zero, seg.Scale, SpriteEffects.None, 0);
                }
                
                spriteBatch.DrawString(Font, seg.Text, pos, color, 
                    0, Vector2.Zero, seg.Scale, SpriteEffects.None, 0);
            }
        }

        private class RenderedLine
        {
            public List<RenderedSegment> Segments = new();
            public float Width;
            public float Height;
        }

        private class RenderedSegment
        {
            public int SourceIndex;
            public string Text;
            public Color Color;
            public bool Bold;
            public float Scale;
            public float X;
            public Vector2 Size;
        }
    }

    public enum TextAlignment { Left, Center, Right }

    #endregion
}
```

## Cambios clave:

| Antes | Después |
|-------|---------|
| `TypewriterInterpreter` con estado (`_currentIndex`, `Advance`, `Update`) | `TypewriterInterpreter` puro: solo método `Parse()` que devuelve `List<TypewriterSection>` |
| `TextSegment.Index` asignado durante parsing | `TextSegment.Index` asignado en el componente |
| Eventos en intérprete | Eventos en el componente |
| Lógica de pausa/wait en intérprete | Lógica de presentación en `TypewriterLabel.UpdateSelf()` |
| `IsPlaying` en intérprete | `IsPlaying` en componente |

## Estructura de datos:

```
Texto BBCode
    ↓
TypewriterInterpreter.Parse()
    ↓
List<TypewriterSection>
    ├── Section 0: Segments [0..15]  (hasta [pause])
    ├── Section 1: Segments [16..42] (hasta [trigger])
    └── Section 2: Segments [43..N]  (resto)
    ↓
TypewriterLabel gestiona:
    - _currentSectionIndex
    - _currentSegmentIndex (global)
    - _timer, _pauseTimer
    - Eventos por sección
```

## Ejemplo de uso:

```csharp
var label = new TypewriterLabel();
label.SetText(
    "[speed=30]¡Alerta![/speed] [wait=0.3]\n" +
    "Enemigos detectados.[pause]\n" +
    "[trigger=spawn_enemies]Prepárate..."
);

label.OnSectionStarted += (sectionIdx) => 
    Console.WriteLine($"Sección {sectionIdx} iniciada");

label.OnSectionComplete += (sectionIdx) => 
    Console.WriteLine($"Sección {sectionIdx} terminada");

label.OnTagTriggered += (tag) => 
    Console.WriteLine($"Tag: {tag}");

label.Start(); // Inicia sección 0
label.SkipSection(); // Salta a sección 1
label.Resume(); // Después de [pause]
```
