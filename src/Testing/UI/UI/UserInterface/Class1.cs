User: Qué otros componentes de este tipo son interesantes para una visual novel?
Kimi: Para una visual novel, hay muchos componentes de UI que mejoran la experiencia inmersiva y la narrativa. Aquí te propongo los más interesantes, todos con coordenadas relativas (0-1) y estilo coherente con lo que ya hemos construido:

## 1. **ChoiceWheel** - Selector de respuestas radial/temporal

```csharp
public class ChoiceWheel
{
    // Opciones distribuidas en círculo o lista vertical con temporizador opcional
    // Efecto de "time pressure" con barra decreciente
    // Animación de selección con íconos o previews de consecuencias
}
```

## 2. **CharacterPortrait** - Retrato animado de personajes

```csharp
public class CharacterPortrait
{
    // Soporte para múltiples capas: base + expresión + accesorios
    // Transiciones suaves (crossfade, slide, bounce)
    // Efectos de iluminación (sombreado cuando no hablan)
    // Sistema de "live2d" básico con deformación de malla simple
}
```

## 3. **NarrativeLog** - Historial de diálogos desplazable

```csharp
public class NarrativeLog
{
    // Lista scrollable de entradas pasadas con timestamp
    // Búsqueda por personaje o palabra clave
    // Marcadores de "visto" vs "no visto"
    // Exportable a formato "script" para revisión
}
```

## 4. **AffinityMeter** - Indicador de relación invisible/visible

```csharp
public class AffinityMeter
{
    // Barra sutil o corazones que flotan en decisiones clave
    // Notificación de "cambio de relación" con animación
    // Sistema de "flags" que desbloquean rutas
    // Opcional: completamente oculto (solo para debug)
}
```

## 5. **QuickTimeEvent** - Secuencias de acción interactivas

```csharp
public class QuickTimeEvent
{
    // Barra de timing con zona óptima
    // Secuencias de botones tipo "Dance Dance Revolution"
    // Diferentes dificultades según importancia de la escena
    // Consecuencias narrativas del éxito/fracaso
}
```

## 6. **PhoneInterface** - Simulación de smartphone

```csharp
public class PhoneInterface
{
    // Pantalla de inicio con apps (mensajes, fotos, notas, llamadas)
    // Notificaciones push que avanzan la trama
    // Galería de fotos desbloqueables
    // Logs de llamadas perdidas
    // Música player con playlist del juego
}
```

## 7. **MapNavigator** - Mapa para seleccionar ubicaciones

```csharp
public class MapNavigator
{
    // Nodos conectados que se desbloquean progresivamente
    // Indicadores de "nuevo contenido" o "incompleto"
    // Preview de escena al hover
    // Rutas alternativas visibles pero bloqueadas
}
```

## 8. **Inventory/ClueSystem** - Sistema de pistas/evidencias

```csharp
public class ClueSystem
{
    // Cuadrícula de items con zoom al seleccionar
    // Conexiones entre pistas (tablero de detective estilo "Baccano")
    // "Combinar" pistas para deducciones
    // Perfiles de personajes con información desbloqueada
}
```

## 9. **CinematicBars** - Barras de cine para momentos dramáticos

```csharp
public class CinematicBars
{
    // Barras negras superior/inferior que se cierran/abren
    // Zoom sutil de cámara simultáneo
    // Desactivación de UI durante momentos clave
    // Transición suave a gameplay normal
}
```

## 10. **EmotionParticleSystem** - Efectos atmosféricos sutiles

```csharp
public class EmotionParticleSystem
{
    // Petalos cayendo (romance), nieve (melancolía), chispas (tensión)
    // Integrado detrás de los retratos pero delante del fondo
    // Cambio dinámico según música/mood de la escena
    // No interactivo, puramente ambiental
}
```

## 11. **SaveSlotThumbnail** - Previsualización de partidas guardadas

```csharp
public class SaveSlotThumbnail
{
    // Captura de pantalla del momento exacto del guardado
    // Datos: Capítulo, tiempo de juego, decisiones clave tomadas
    // Mini-grafo de ruta actual visible
    // Sistema de "notas" personalizables por slot
}
```

## 12. **TypingIndicator** - Cursor de máquina de escribir

```csharp
public class TypingIndicator
{
    // Cursor parpadeante estilo terminal/consola
    // Sonido de tecla opcional por carácter
    // Velocidad ajustable (configuración de accesibilidad)
    // Opción de "saltar" vs "acelerar" vs "mostrar instantáneo"
}
```

---

## Implementación completa: **CharacterPortrait**

Voy a crear uno de los más útiles - el retrato de personaje con sistema de capas y transiciones:

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace VisualNovel.UI
{
    public class CharacterPortrait
    {
        // Sistema de capas: Base + Expresión + Accesorios + Efectos
        private Dictionary<string, Texture2D> _layers;
        private List<string> _activeLayers;
        
        // Transformaciones
        private Vector2 _position;          // Posición relativa (0-1)
        private float _scale;
        private float _rotation;
        private Color _tint;
        private float _opacity;
        
        // Animación
        private PortraitState _currentState;
        private PortraitState _targetState;
        private float _transitionSpeed;
        private float _bouncePhase;
        
        // Efectos
        private bool _isSpeaking;
        private float _speakingIntensity;
        private bool _isHighlighted;
        private float _silhouetteDarkness;  // 0 = normal, 1 = completamente oscuro
        
        // Configuración
        private SpriteEffects _facing;
        private string _characterId;
        
        public enum SlideDirection { Left, Right, Up, Down, None }
        
        public struct PortraitState
        {
            public Vector2 Position;
            public float Scale;
            public float Rotation;
            public Color Tint;
            public float Opacity;
            public SlideDirection EntryDirection;
        }
        
        public CharacterPortrait(string characterId, Vector2 initialPosition)
        {
            _characterId = characterId;
            _position = initialPosition;
            _layers = new Dictionary<string, Texture2D>();
            _activeLayers = new List<string>();
            
            _scale = 1f;
            _opacity = 1f;
            _tint = Color.White;
            _transitionSpeed = 5f;
            _facing = SpriteEffects.None;
            
            _currentState = new PortraitState
            {
                Position = initialPosition,
                Scale = 1f,
                Opacity = 0f,
                Tint = Color.White
            };
            _targetState = _currentState;
        }
        
        public void AddLayer(string layerName, Texture2D texture)
        {
            _layers[layerName] = texture;
        }
        
        public void SetExpression(string expressionName)
        {
            // Remover capas de expresión anteriores (que no sean "base")
            _activeLayers.RemoveAll(l => l != "base" && !_layers[l].Name.Contains("accessory"));
            
            // Añadir nueva expresión si existe
            string layerKey = $"expr_{expressionName}";
            if (_layers.ContainsKey(layerKey))
            {
                _activeLayers.Add(layerKey);
            }
        }
        
        public void SetBase(Texture2D baseTexture)
        {
            _layers["base"] = baseTexture;
            if (!_activeLayers.Contains("base"))
                _activeLayers.Insert(0, "base");
        }
        
        public void Enter(SlideDirection from, float duration = 0.5f)
        {
            Vector2 startPos = _position;
            
            switch (from)
            {
                case SlideDirection.Left: startPos.X -= 0.3f; break;
                case SlideDirection.Right: startPos.X += 0.3f; break;
                case SlideDirection.Up: startPos.Y -= 0.3f; break;
                case SlideDirection.Down: startPos.Y += 0.3f; break;
            }
            
            _currentState.Position = startPos;
            _currentState.Opacity = 0f;
            _targetState.Position = _position;
            _targetState.Opacity = 1f;
            _transitionSpeed = 1f / duration;
        }
        
        public void Exit(SlideDirection to, float duration = 0.5f)
        {
            Vector2 endPos = _position;
            
            switch (to)
            {
                case SlideDirection.Left: endPos.X -= 0.3f; break;
                case SlideDirection.Right: endPos.X += 0.3f; break;
                case SlideDirection.Up: endPos.Y -= 0.3f; break;
                case SlideDirection.Down: endPos.Y += 0.3f; break;
            }
            
            _targetState.Position = endPos;
            _targetState.Opacity = 0f;
            _transitionSpeed = 1f / duration;
        }
        
        public void SetSpeaking(bool speaking)
        {
            _isSpeaking = speaking;
        }
        
        public void SetHighlighted(bool highlighted)
        {
            _isHighlighted = highlighted;
            _silhouetteDarkness = highlighted ? 0f : 0.6f;
        }
        
        public void Bounce(float intensity = 5f)
        {
            _bouncePhase = intensity;
        }
        
        public void FaceLeft() => _facing = SpriteEffects.FlipHorizontally;
        public void FaceRight() => _facing = SpriteEffects.None;
        
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Interpolación suave de estado
            _currentState.Position = Vector2.Lerp(_currentState.Position, _targetState.Position, _transitionSpeed * dt);
            _currentState.Opacity = MathHelper.Lerp(_currentState.Opacity, _targetState.Opacity, _transitionSpeed * dt);
            _currentState.Scale = MathHelper.Lerp(_currentState.Scale, _targetState.Scale, _transitionSpeed * dt);
            
            // Animación de hablar (bounce sutil)
            if (_isSpeaking)
            {
                _speakingIntensity = MathHelper.Lerp(_speakingIntensity, 1f, 10f * dt);
            }
            else
            {
                _speakingIntensity = MathHelper.Lerp(_speakingIntensity, 0f, 10f * dt);
            }
            
            // Decay del bounce
            if (_bouncePhase > 0)
            {
                _bouncePhase -= dt * 10f;
                if (_bouncePhase < 0) _bouncePhase = 0;
            }
            
            _opacity = _currentState.Opacity;
        }
        
        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            if (_opacity <= 0.01f) return;
            
            // Calcular posición en píxeles
            int x = (int)(_currentState.Position.X * screenWidth);
            int y = (int)(_currentState.Position.Y * screenHeight);
            
            // Añadir bounce de hablar
            float speakBounce = (float)Math.Sin(_bouncePhase + (float)gameTime.TotalGameTime.TotalSeconds * 15) 
                * 3f * _speakingIntensity;
            float impactBounce = (float)Math.Sin(_bouncePhase) * _bouncePhase * 2f;
            
            y += (int)(speakBounce + impactBounce);
            
            // Calcular escala base
            float baseScale = _currentState.Scale * Math.Min(screenWidth, screenHeight) * 0.001f;
            
            // Dibujar capas de abajo hacia arriba
            foreach (var layerName in _activeLayers)
            {
                if (!_layers.ContainsKey(layerName)) continue;
                
                var texture = _layers[layerName];
                
                // Calcular origen para centrar
                Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
                
                // Color con tinte y oscurecimiento
                Color drawColor = _currentState.Tint;
                drawColor = Color.Lerp(drawColor, Color.Black, _silhouetteDarkness);
                drawColor.A = (byte)(_opacity * 255);
                
                // Efecto de highlight (brillo sutil cuando habla)
                if (_isHighlighted && _isSpeaking)
                {
                    drawColor = Color.Lerp(drawColor, Color.White, 0.1f);
                }
                
                spriteBatch.Draw(
                    texture,
                    new Vector2(x, y),
                    null,
                    drawColor,
                    _currentState.Rotation,
                    origin,
                    baseScale,
                    _facing,
                    0f
                );
            }
        }
    }
}
```

## Implementación: **ChoiceWheel** (selector de decisiones)

```csharp
public class ChoiceWheel
{
    private List<ChoiceOption> _options;
    private int _selectedIndex;
    private float _timer;  // Para time pressure
    private float _maxTime;
    
    public struct ChoiceOption
    {
        public string Text;
        public string Preview;      // Consecuencia hint
        public Action OnSelect;
        public Color AccentColor;   // Color temático de la opción
        public bool IsUnlocked;     // Si requiere prerequisitos
        public string LockReason;   // Por qué está bloqueado
    }
    
    // Distribución radial o vertical
    // Animación de entrada escalonada
    // Sonido de "ticking" si hay timer
    // Efecto de "destino" al seleccionar (zoom + fade)
}
```

## ¿Cuál te interesa desarrollar primero?

Recomiendo priorizar:
1. **CharacterPortrait** - Esencial para cualquier VN
2. **ChoiceWheel** - Si tu VN tiene decisiones significativas
3. **PhoneInterface** - Si quieres una estética moderna/inyo

¿Cuál prefieres que implemente completamente?
User: Me gustaría saber más sobre el carácter portrait. Para que sirve. No implementar código aún
Kimi: El **CharacterPortrait** es uno de los componentes más importantes en una visual novel porque cumple múltiples funciones narrativas y técnicas simultáneamente. Te explico para qué sirve y por qué es esencial:

## Funciones principales

### 1. **Identificación del hablante**
- Muestra quién está hablando en cada momento
- Evita la necesidad de escribir "—dijo María—" constantemente
- Permite diálogos rápidos entre múltiples personajes sin confusión

### 2. **Expresión emocional**
- Un mismo personaje puede mostrar diferentes emociones: feliz, triste, enojado, sorprendido, avergonzado
- La expresión facial complementa el texto y refuerza el tono emocional
- Permite ironía dramática (personaje sonriendo mientras dice algo triste, por ejemplo)

### 3. **Estado de la conversación**
- **Destacado/brillante**: el personaje está hablando activamente
- **Oscurecido/atenuado**: el personaje está en escena pero no habla (escuchando)
- **Ausente**: no está en la escena actual

### 4. **Posicionamiento espacial**
- Izquierda/derecha: indica posición relativa en la escena imaginaria
- Múltiples personajes pueden mostrarse simultáneamente en diferentes posiciones
- Transiciones suaves simulan movimiento o entrada/salida de personajes

### 5. **Dinamismo visual**
- Evita que la pantalla sea texto estático sobre fondo
- Animaciones sutiles (respiración, parpadeo, bounce al hablar) dan vida al personaje
- Cambios de expresión mantienen la atención del jugador

## Ventajas narrativas específicas

| Aspecto | Cómo lo logra el portrait |
|---------|---------------------------|
| **Empatía** | Ver la cara del personaje genera conexión emocional más fuerte que texto solo |
| **Comedia** | Las reacciones exageradas o timing de cambios de expresión crean humor |
| **Tensión** | Una expresión seria o preocupada prepara al jugador para malas noticias |
| **Romance** | Miradas, sonrojos, sonrisas tímidas son difíciles de describir solo con texto |
| **Misterio** | Ocultar expresiones o mostrarlas parcialmente genera incertidumbre |

## Variantes según estilo de VN

- **VN clásica japonesa**: Sprites 2D estáticos con múltiples expresiones predefinidas, posiciones fijas (izquierda/centro/derecha)
- **VN occidental moderna**: Sprites con más animación, posiciones libres, posiblemente con técnicas de Live2D o skeletal animation
- **VN minimalista**: Siluetas o avatares pequeños, enfocadas en el texto
- **VN con personajes propios del jugador**: El portrait puede cambiar según decisiones previas (ropa, peinado, expresión dominante)

## Integración con otros sistemas

El portrait no trabaja solo. Se conecta con:
- El sistema de diálogo (sabe quién habla)
- El sistema de audio (sincroniza con voces o efectos de sonido)
- El sistema de afecto/relación (expresiones cambian según cómo te trata el personaje)
- El sistema de guardado (recuerda qué sprite mostrar al cargar)

## Ejemplo de flujo típico

1. Entra personaje A por la izquierda (slide) con expresión "neutral"
2. Habla A: portrait se ilumina, bounce sutil, cambia a "sonrisa"
3. Entra personaje B por la derecha: A se oscurece parcialmente
4. Habla B: B se ilumina, A se oscurece completamente
5. Evento sorprendente: ambos cambian a "shock" simultáneamente
6. B sale (slide derecha), A vuelve a iluminarse y cambia a "preocupado"

Sin portraits, esto requeriría descripciones extensas que rompen el ritmo.

¿Te gustaría que profundice en algún aspecto específico, como las transiciones entre expresiones o el sistema de capas para vestimenta/accesorios?
User: Vamos a implementar primero un tipo de actor que nos permita definir un Sprite de personajes. Un Sprite de personaje tiene una serie de texturas con posiciones relativas entre si. Están colocadas por capas y pueden llegar a tener animación, por ejemplo, los ojos tienen varias texturas para el parpadeo. Las animaciones pueden no ser continuas. Es decir el personaje parpadea y espera un tiempo hasta el siguiente parpadea
Kimi: Voy a diseñar un sistema de Actor para personajes con sprites por capas y animaciones eventuales (no continuas) como el parpadeo. Este será el núcleo sobre el que construiremos el CharacterPortrait posteriormente.

## Diseño del sistema Actor

### Conceptos clave:

1. **Actor**: Contenedor principal del personaje
2. **Layer**: Capa visual (base, ojos, boca, pelo, ropa, accesorios)
3. **AnimationState**: Estados animables dentro de una capa (ojos: abierto, cerrado, medio_cerrado)
4. **AnimationTrigger**: Sistema de eventos temporizados para animaciones no continuas

### Características:

- **Jerarquía de capas**: Se renderizan en orden definido (de fondo a frente)
- **Posiciones relativas**: Cada capa tiene offset respecto al Actor
- **Animaciones frame-based**: Secuencias de texturas con timing
- **Sistema de blink/idle**: Timers aleatorios para comportamientos orgánicos
- **Transiciones suaves**: Crossfade entre estados cuando sea necesario

¿Quieres que proceda con la implementación completa en código?
User: No escribas las transiciones pero si el resto
Kimi: Entendido. Aquí está la implementación completa del sistema **Actor** con capas, animaciones frame-based y sistema de triggers temporizados, sin transiciones suaves entre estados.

## Sistema Actor - Implementación

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace VisualNovel.Characters
{
    public class Actor
    {
        // Identificación
        public string Id { get; private set; }
        public string Name { get; set; }
        
        // Transformación global del actor (coordenadas 0-1)
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public SpriteEffects Facing { get; set; }
        public Color Tint { get; set; }
        public float Opacity { get; set; }
        
        // Capas del personaje (ordenadas de fondo a frente)
        private List<ActorLayer> _layers;
        private Dictionary<string, ActorLayer> _layerIndex;
        
        // Animaciones y triggers
        private List<AnimationTrigger> _activeTriggers;
        private Random _random;
        
        // Estado global
        public bool IsVisible { get; set; }
        public bool IsPaused { get; set; }
        
        public Actor(string id, Vector2 initialPosition)
        {
            Id = id;
            Position = initialPosition;
            Scale = 1f;
            Rotation = 0f;
            Facing = SpriteEffects.None;
            Tint = Color.White;
            Opacity = 1f;
            
            _layers = new List<ActorLayer>();
            _layerIndex = new Dictionary<string, ActorLayer>();
            _activeTriggers = new List<AnimationTrigger>();
            _random = new Random();
            
            IsVisible = true;
            IsPaused = false;
        }
        
        // Gestión de capas
        public void AddLayer(string layerId, string layerName, int drawOrder)
        {
            var layer = new ActorLayer(layerId, layerName, drawOrder);
            _layerIndex[layerId] = layer;
            _layers.Add(layer);
            _layers.Sort((a, b) => a.DrawOrder.CompareTo(b.DrawOrder));
        }
        
        public void RemoveLayer(string layerId)
        {
            if (_layerIndex.ContainsKey(layerId))
            {
                _layers.Remove(_layerIndex[layerId]);
                _layerIndex.Remove(layerId);
            }
        }
        
        public ActorLayer GetLayer(string layerId)
        {
            return _layerIndex.ContainsKey(layerId) ? _layerIndex[layerId] : null;
        }
        
        // Configuración rápida de capas comunes
        public void SetupStandardLayers()
        {
            AddLayer("base", "Cuerpo base", 0);
            AddLayer("face", "Rostro", 10);
            AddLayer("eyes", "Ojos", 20);
            AddLayer("mouth", "Boca", 30);
            AddLayer("hair_front", "Pelo delantero", 40);
            AddLayer("clothes", "Ropa", 50);
            AddLayer("accessories", "Accesorios", 60);
        }
        
        // Actualización
        public void Update(GameTime gameTime)
        {
            if (IsPaused || !IsVisible) return;
            
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Actualizar todas las capas
            foreach (var layer in _layers)
            {
                layer.Update(deltaTime);
            }
            
            // Procesar triggers de animación
            UpdateTriggers(deltaTime);
        }
        
        private void UpdateTriggers(float deltaTime)
        {
            for (int i = _activeTriggers.Count - 1; i >= 0; i--)
            {
                var trigger = _activeTriggers[i];
                trigger.TimeRemaining -= deltaTime;
                
                if (trigger.TimeRemaining <= 0)
                {
                    // Ejecutar acción del trigger
                    ExecuteTrigger(trigger);
                    _activeTriggers.RemoveAt(i);
                }
            }
        }
        
        private void ExecuteTrigger(AnimationTrigger trigger)
        {
            var layer = GetLayer(trigger.LayerId);
            if (layer == null) return;
            
            switch (trigger.Type)
            {
                case TriggerType.PlayAnimation:
                    layer.PlayAnimation(trigger.AnimationName, trigger.LoopCount);
                    break;
                    
                case TriggerType.SetFrame:
                    layer.SetFrame(trigger.FrameIndex);
                    break;
                    
                case TriggerType.RandomFrame:
                    layer.SetRandomFrame();
                    break;
                    
                case TriggerType.ScheduleNext:
                    // Reprogramar otro trigger (como parpadeos consecutivos)
                    ScheduleTrigger(trigger.NextTrigger);
                    break;
            }
        }
        
        // Sistema de scheduling para animaciones eventuales
        public void ScheduleTrigger(AnimationTrigger trigger)
        {
            _activeTriggers.Add(trigger);
        }
        
        public void ScheduleBlink(string eyesLayerId = "eyes", 
            float minInterval = 2f, float maxInterval = 5f,
            float blinkDuration = 0.15f)
        {
            float nextBlink = minInterval + (float)_random.NextDouble() * (maxInterval - minInterval);
            
            var trigger = new AnimationTrigger
            {
                LayerId = eyesLayerId,
                Type = TriggerType.PlayAnimation,
                AnimationName = "blink",
                LoopCount = 1,
                TimeRemaining = nextBlink,
                OnComplete = () => {
                    // Después del parpadeo, programar el siguiente
                    ScheduleBlink(eyesLayerId, minInterval, maxInterval, blinkDuration);
                }
            };
            
            ScheduleTrigger(trigger);
        }
        
        public void ScheduleIdleAnimation(string layerId, string animationName,
            float minInterval, float maxInterval)
        {
            float delay = minInterval + (float)_random.NextDouble() * (maxInterval - minInterval);
            
            var trigger = new AnimationTrigger
            {
                LayerId = layerId,
                Type = TriggerType.PlayAnimation,
                AnimationName = animationName,
                LoopCount = 1,
                TimeRemaining = delay,
                OnComplete = () => {
                    ScheduleIdleAnimation(layerId, animationName, minInterval, maxInterval);
                }
            };
            
            ScheduleTrigger(trigger);
        }
        
        // Control directo de capas
        public void SetLayerVisibility(string layerId, bool visible)
        {
            var layer = GetLayer(layerId);
            if (layer != null) layer.IsVisible = visible;
        }
        
        public void SetLayerPosition(string layerId, Vector2 relativePosition)
        {
            var layer = GetLayer(layerId);
            if (layer != null) layer.RelativePosition = relativePosition;
        }
        
        public void PlayLayerAnimation(string layerId, string animationName, int loopCount = 1)
        {
            var layer = GetLayer(layerId);
            if (layer != null) layer.PlayAnimation(animationName, loopCount);
        }
        
        public void StopLayerAnimation(string layerId)
        {
            var layer = GetLayer(layerId);
            if (layer != null) layer.StopAnimation();
        }
        
        public void SetLayerFrame(string layerId, int frameIndex)
        {
            var layer = GetLayer(layerId);
            if (layer != null) layer.SetFrame(frameIndex);
        }
        
        // Renderizado
        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            if (!IsVisible || Opacity <= 0.01f) return;
            
            // Calcular posición en píxeles
            Vector2 pixelPosition = new Vector2(
                Position.X * screenWidth,
                Position.Y * screenHeight
            );
            
            // Calcular escala base (referencia: altura de pantalla)
            float baseScale = Scale * (screenHeight * 0.001f);
            
            foreach (var layer in _layers)
            {
                if (!layer.IsVisible) continue;
                
                layer.Draw(spriteBatch, pixelPosition, baseScale, Rotation, Facing, Tint, Opacity);
            }
        }
        
        // Limpieza
        public void ClearTriggers()
        {
            _activeTriggers.Clear();
        }
        
        public void ResetAllLayers()
        {
            foreach (var layer in _layers)
            {
                layer.Reset();
            }
        }
    }
    
    // Capa individual del actor
    public class ActorLayer
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public int DrawOrder { get; private set; }
        
        // Transformación relativa al actor
        public Vector2 RelativePosition { get; set; }
        public float RelativeScale { get; set; }
        public float RelativeRotation { get; set; }
        
        // Visibilidad
        public bool IsVisible { get; set; }
        
        // Frames/Animaciones
        private Dictionary<string, Animation> _animations;
        private Animation _currentAnimation;
        private int _currentFrameIndex;
        private float _frameTimer;
        private bool _isPlaying;
        private int _loopsRemaining;
        private Action _onAnimationComplete;
        
        // Frame estático (cuando no hay animación)
        private Texture2D _staticFrame;
        
        public ActorLayer(string id, string name, int drawOrder)
        {
            Id = id;
            Name = name;
            DrawOrder = drawOrder;
            
            RelativePosition = Vector2.Zero;
            RelativeScale = 1f;
            RelativeRotation = 0f;
            IsVisible = true;
            
            _animations = new Dictionary<string, Animation>();
            _currentFrameIndex = 0;
            _frameTimer = 0f;
            _isPlaying = false;
        }
        
        // Gestión de animaciones
        public void AddAnimation(string name, Animation animation)
        {
            _animations[name] = animation;
        }
        
        public void SetStaticFrame(Texture2D texture)
        {
            _staticFrame = texture;
            StopAnimation();
        }
        
        // Control de reproducción
        public void PlayAnimation(string name, int loopCount = 1, Action onComplete = null)
        {
            if (!_animations.ContainsKey(name)) return;
            
            _currentAnimation = _animations[name];
            _currentFrameIndex = 0;
            _frameTimer = 0f;
            _loopsRemaining = loopCount;
            _isPlaying = true;
            _onAnimationComplete = onComplete;
        }
        
        public void StopAnimation()
        {
            _isPlaying = false;
            _currentAnimation = null;
            _currentFrameIndex = 0;
        }
        
        public void SetFrame(int index)
        {
            if (_currentAnimation != null && index >= 0 && index < _currentAnimation.Frames.Count)
            {
                _currentFrameIndex = index;
                _frameTimer = 0f;
            }
        }
        
        public void SetRandomFrame()
        {
            if (_currentAnimation != null && _currentAnimation.Frames.Count > 0)
            {
                var random = new Random();
                _currentFrameIndex = random.Next(_currentAnimation.Frames.Count);
            }
        }
        
        // Actualización
        public void Update(float deltaTime)
        {
            if (!_isPlaying || _currentAnimation == null) return;
            
            _frameTimer += deltaTime;
            
            // Tiempo del frame actual
            float currentFrameDuration = _currentAnimation.GetFrameDuration(_currentFrameIndex);
            
            if (_frameTimer >= currentFrameDuration)
            {
                _frameTimer -= currentFrameDuration;
                _currentFrameIndex++;
                
                // Fin de animación
                if (_currentFrameIndex >= _currentAnimation.Frames.Count)
                {
                    _currentFrameIndex = 0;
                    
                    if (_loopsRemaining > 0)
                    {
                        _loopsRemaining--;
                        if (_loopsRemaining == 0)
                        {
                            _isPlaying = false;
                            _onAnimationComplete?.Invoke();
                        }
                    }
                }
            }
        }
        
        // Renderizado
        public void Draw(SpriteBatch spriteBatch, Vector2 actorPosition, float actorScale,
            float actorRotation, SpriteEffects actorFacing, Color actorTint, float actorOpacity)
        {
            Texture2D textureToDraw = null;
            
            if (_isPlaying && _currentAnimation != null)
            {
                textureToDraw = _currentAnimation.Frames[_currentFrameIndex];
            }
            else if (_staticFrame != null)
            {
                textureToDraw = _staticFrame;
            }
            
            if (textureToDraw == null) return;
            
            // Calcular transformación final
            Vector2 finalPosition = actorPosition + (RelativePosition * actorScale);
            float finalScale = actorScale * RelativeScale;
            float finalRotation = actorRotation + RelativeRotation;
            
            // Origen centrado
            Vector2 origin = new Vector2(textureToDraw.Width / 2f, textureToDraw.Height / 2f);
            
            // Color final
            Color finalColor = actorTint;
            finalColor.A = (byte)(actorOpacity * 255);
            
            spriteBatch.Draw(
                textureToDraw,
                finalPosition,
                null,
                finalColor,
                finalRotation,
                origin,
                finalScale,
                actorFacing,
                0f
            );
        }
        
        public void Reset()
        {
            StopAnimation();
            _currentFrameIndex = 0;
            _frameTimer = 0f;
            IsVisible = true;
        }
    }
    
    // Definición de animación
    public class Animation
    {
        public string Name { get; set; }
        public List<Texture2D> Frames { get; private set; }
        public float FrameDuration { get; set; }  // Duración base por frame
        public List<float> FrameDurations { get; private set; }  // Duraciones específicas por frame
        
        public Animation(string name, float defaultDuration = 0.1f)
        {
            Name = name;
            Frames = new List<Texture2D>();
            FrameDuration = defaultDuration;
            FrameDurations = new List<float>();
        }
        
        public void AddFrame(Texture2D frame, float? specificDuration = null)
        {
            Frames.Add(frame);
            FrameDurations.Add(specificDuration ?? FrameDuration);
        }
        
        public float GetFrameDuration(int index)
        {
            if (index >= 0 && index < FrameDurations.Count)
                return FrameDurations[index];
            return FrameDuration;
        }
    }
    
    // Trigger para animaciones temporizadas
    public class AnimationTrigger
    {
        public string LayerId { get; set; }
        public TriggerType Type { get; set; }
        public float TimeRemaining { get; set; }
        
        // Para PlayAnimation
        public string AnimationName { get; set; }
        public int LoopCount { get; set; }
        
        // Para SetFrame
        public int FrameIndex { get; set; }
        
        // Para encadenamiento
        public AnimationTrigger NextTrigger { get; set; }
        
        // Callback
        public Action OnComplete { get; set; }
    }
    
    public enum TriggerType
    {
        PlayAnimation,
        SetFrame,
        RandomFrame,
        ScheduleNext
    }
}
```

## Ejemplo de uso:

```csharp
// Crear actor
var maria = new Actor("maria", new Vector2(0.3f, 0.8f));

// Configurar capas
maria.SetupStandardLayers();

// Configurar capa base (cuerpo)
var baseLayer = maria.GetLayer("base");
baseLayer.SetStaticFrame(Content.Load<Texture2D>("Characters/Maria/Base")));

// Configurar ojos con animación de parpadeo
var eyesLayer = maria.GetLayer("eyes");
var blinkAnim = new Animation("blink", 0.05f);
blinkAnim.AddFrame(Content.Load<Texture2D>("Characters/Maria/Eyes_Open")));
blinkAnim.AddFrame(Content.Load<Texture2D>("Characters/Maria/Eyes_Half")));
blinkAnim.AddFrame(Content.Load<Texture2D>("Characters/Maria/Eyes_Closed")));
blinkAnim.AddFrame(Content.Load<Texture2D>("Characters/Maria/Eyes_Half")));
blinkAnim.AddFrame(Content.Load<Texture2D>("Characters/Maria/Eyes_Open")));
eyesLayer.AddAnimation("blink", blinkAnim);
eyesLayer.SetStaticFrame(blinkAnim.Frames[0]); // Frame inicial

// Iniciar sistema de parpadeo automático
maria.ScheduleBlink("eyes", minInterval: 2f, maxInterval: 6f);

// Configurar boca con expresiones
var mouthLayer = maria.GetLayer("mouth");
mouthLayer.AddAnimation("talk", new Animation("talk", 0.15f));
// ... añadir frames de boca hablando

// En el juego
maria.Update(gameTime);
maria.Draw(spriteBatch, screenWidth, screenHeight);

// Eventos
maria.PlayLayerAnimation("mouth", "talk", loopCount: 3); // Hablar 3 ciclos
maria.SetLayerFrame("eyes", 0); // Forzar ojos abiertos
```

El sistema permite animaciones complejas por capas con timing independiente, perfecto para personajes vivos con comportamientos orgánicos como parpadeos aleatorios o reacciones eventuales.
User: Para manejar mejor las capas, las coordenadas de una capa son absolutas, es decir píxeles. Esto nos indica la posición del hueso dentro de la imagen. Al colocarse debe tener en cuenta que el actor se posiciona en coordenadas relativas de la pantalla y ajustarse a ellas. No hacen falta las animaciones
Kimi: Entendido. Simplifico el sistema: coordenadas de capas en **píxeles absolutos** (posición del "hueso" dentro de la imagen del personaje), mientras el Actor mantiene posición relativa en pantalla. Sin sistema de animación.

## Actor Simplificado - Capas con Posición Absoluta

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace VisualNovel.Characters
{
    public class Actor
    {
        public string Id { get; private set; }
        public string Name { get; set; }
        
        // Posición y escala del actor en pantalla (relativo 0-1)
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public SpriteEffects Facing { get; set; }
        public Color Tint { get; set; }
        public float Opacity { get; set; }
        
        // Capas ordenadas por profundidad
        private List<ActorLayer> _layers;
        private Dictionary<string, ActorLayer> _layerIndex;
        
        public bool IsVisible { get; set; }
        
        public Actor(string id, Vector2 initialPosition)
        {
            Id = id;
            Position = initialPosition;
            Scale = 1f;
            Rotation = 0f;
            Facing = SpriteEffects.None;
            Tint = Color.White;
            Opacity = 1f;
            
            _layers = new List<ActorLayer>();
            _layerIndex = new Dictionary<string, ActorLayer>();
            IsVisible = true;
        }
        
        // Gestión de capas
        public void AddLayer(string layerId, string layerName, int drawOrder)
        {
            var layer = new ActorLayer(layerId, layerName);
            layer.DrawOrder = drawOrder;
            _layerIndex[layerId] = layer;
            _layers.Add(layer);
            _layers.Sort((a, b) => a.DrawOrder.CompareTo(b.DrawOrder));
        }
        
        public void RemoveLayer(string layerId)
        {
            if (_layerIndex.TryGetValue(layerId, out var layer))
            {
                _layers.Remove(layer);
                _layerIndex.Remove(layerId);
            }
        }
        
        public ActorLayer GetLayer(string layerId)
        {
            return _layerIndex.TryGetValue(layerId, out var layer) ? layer : null;
        }
        
        // Configuración rápida estándar
        public void SetupStandardLayers()
        {
            AddLayer("body", "Cuerpo", 0);
            AddLayer("face", "Rostro", 10);
            AddLayer("eyes", "Ojos", 20);
            AddLayer("mouth", "Boca", 30);
            AddLayer("hair_back", "Pelo trasero", 5);
            AddLayer("hair_front", "Pelo delantero", 40);
            AddLayer("clothes", "Ropa", 25);
        }
        
        // Control de capas
        public void SetLayerVisibility(string layerId, bool visible)
        {
            var layer = GetLayer(layerId);
            if (layer != null) layer.IsVisible = visible;
        }
        
        public void SetLayerTexture(string layerId, Texture2D texture)
        {
            var layer = GetLayer(layerId);
            if (layer != null) layer.Texture = texture;
        }
        
        public void SetLayerPosition(string layerId, Vector2 pixelPosition)
        {
            var layer = GetLayer(layerId);
            if (layer != null) layer.PixelPosition = pixelPosition;
        }
        
        public void SetLayerOffset(string layerId, Vector2 pixelOffset)
        {
            var layer = GetLayer(layerId);
            if (layer != null) layer.PixelOffset = pixelOffset;
        }
        
        // Renderizado
        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            if (!IsVisible || Opacity <= 0.01f) return;
            
            // Posición base del actor en píxeles de pantalla
            Vector2 actorPixelPos = new Vector2(
                Position.X * screenWidth,
                Position.Y * screenHeight
            );
            
            // Factor de escala global (referencia: 1000px = escala 1.0)
            float globalScale = Scale * (screenHeight / 1000f);
            
            foreach (var layer in _layers)
            {
                if (!layer.IsVisible || layer.Texture == null) continue;
                
                layer.Draw(spriteBatch, actorPixelPos, globalScale, Rotation, Facing, Tint, Opacity);
            }
        }
    }
    
    public class ActorLayer
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public int DrawOrder { get; set; }
        
        // Posición del "hueso" en la imagen original (píxeles absolutos)
        // Por ejemplo: si los ojos están en (120, 80) del sprite base
        public Vector2 PixelPosition { get; set; }
        
        // Offset adicional para ajustes finos (píxeles)
        public Vector2 PixelOffset { get; set; }
        
        // Escala relativa a la capa base
        public float LayerScale { get; set; }
        
        // Rotación relativa
        public float LayerRotation { get; set; }
        
        public Texture2D Texture { get; set; }
        public bool IsVisible { get; set; }
        
        // Origen para rotación (relativo a la textura, 0-1)
        // Por defecto centro: (0.5, 0.5)
        public Vector2 Pivot { get; set; }
        
        public ActorLayer(string id, string name)
        {
            Id = id;
            Name = name;
            PixelPosition = Vector2.Zero;
            PixelOffset = Vector2.Zero;
            LayerScale = 1f;
            LayerRotation = 0f;
            IsVisible = true;
            Pivot = new Vector2(0.5f, 0.5f);
        }
        
        public void Draw(SpriteBatch spriteBatch, Vector2 actorPixelPos, float globalScale,
            float actorRotation, SpriteEffects facing, Color tint, float opacity)
        {
            if (Texture == null) return;
            
            // Calcular posición final:
            // 1. Posición del hueso escalada por el factor global
            // 2. Sumar offset de ajuste
            // 3. Aplicar rotación del actor si existe
            
            Vector2 scaledBonePos = PixelPosition * globalScale;
            Vector2 scaledOffset = PixelOffset * globalScale;
            
            Vector2 finalPos = actorPixelPos + scaledBonePos + scaledOffset;
            
            // Si hay rotación del actor, rotar la posición relativa al centro del actor
            if (actorRotation != 0)
            {
                // Crear matriz de rotación alrededor del actor
                Matrix rotationMatrix = Matrix.CreateRotationZ(actorRotation);
                Vector2 rotatedPos = Vector2.Transform(finalPos - actorPixelPos, rotationMatrix);
                finalPos = actorPixelPos + rotatedPos;
            }
            
            // Calcular escala final de esta capa
            float finalScale = globalScale * LayerScale;
            
            // Calcular rotación final
            float finalRotation = actorRotation + LayerRotation;
            
            // Origen basado en pivot
            Vector2 origin = new Vector2(
                Texture.Width * Pivot.X,
                Texture.Height * Pivot.Y
            );
            
            // Color final
            Color finalColor = tint;
            finalColor.A = (byte)(opacity * 255);
            
            spriteBatch.Draw(
                Texture,
                finalPos,
                null,
                finalColor,
                finalRotation,
                origin,
                finalScale,
                facing,
                0f
            );
        }
    }
}
```

## Ejemplo de uso práctico:

```csharp
// Crear personaje centrado abajo
var maria = new Actor("maria", new Vector2(0.5f, 0.85f));
maria.Scale = 1.2f;

// Capas con posiciones de huesos medidas en el sprite original
maria.SetupStandardLayers();

// Cuerpo base (origen del sistema de coordenadas)
maria.SetLayerTexture("body", Content.Load<Texture2D>("Maria/Body"));
maria.SetLayerPosition("body", Vector2.Zero); // (0,0) = centro del actor

// Ojos: en el sprite original están en (45, -60) respecto al centro del body
maria.SetLayerTexture("eyes", Content.Load<Texture2D>("Maria/Eyes_Open"));
maria.SetLayerPosition("eyes", new Vector2(45f, -60f));

// Boca: en (0, -20)
maria.SetLayerTexture("mouth", Content.Load<Texture2D>("Maria/Mouth_Smile"));
maria.SetLayerPosition("mouth", new Vector2(0f, -20f));

// Pelo delantero: cubre parte de la cara, posición (10, -80)
maria.SetLayerTexture("hair_front", Content.Load<Texture2D>("Maria/Hair_Front"));
maria.SetLayerPosition("hair_front", new Vector2(10f, -80f));

// Ajuste fino en tiempo real (ej: respiración)
maria.SetLayerOffset("body", new Vector2(0, -2f)); // Subir 2px

// Dibujar
maria.Draw(spriteBatch, screenWidth, screenHeight);
```

## Características clave:

1. **Coordenadas de hueso absolutas**: Definidas en píxeles del arte original, no relativas
2. **Escalado automático**: Las posiciones de hueso se multiplican por el factor global del actor
3. **Jerarquía implícita**: Todas las capas son hijas del actor, heredan su posición/rotación/escala
4. **Pivot configurable**: Cada capa puede rotar alrededor de su centro o cualquier punto
5. **Sin animaciones**: Cambios de frame manuales cambiando la `Texture` de la capa
6. **Offset temporal**: `PixelOffset` permite micro-ajustes sin cambiar la posición base del hueso
