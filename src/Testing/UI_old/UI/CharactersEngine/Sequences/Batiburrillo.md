User: Eres un programador especializado en monogame. Estás programando un motor para visual novel, en concreto las partes de secuencias cinemática. Ya tienes una clase Actor con sus transformaciones de posición, rotación y escalada y una serie de comandos de movimiento. Necesitamos añadir un comando que haga zoom a una posición en concreto de un Actor, por ejemplo qué haga un zoom pero que el centro de ese zoom sea la cara del personaje.
Kimi: Entiendo perfectamente lo que necesitas. Para implementar un **comando de zoom centrado en un punto específico** de un Actor, necesitamos combinar el escalado con un desplazamiento de posición compensatorio. Esto es un problema clásico de transformaciones 2D/3D.

## Concepto Matemático

Cuando haces zoom (escalas) en un punto que no es el origen, ese punto se desplaza. Para mantenerlo fijo en la pantalla, debes compensar moviendo el Actor.

```
NuevaPosición = PuntoDeZoom - (PuntoDeZoom - PosiciónActual) * Escala
```

O simplificado:
```
Desplazamiento = PuntoDeZoom * (1 - Escala)
NuevaPosición = PosiciónActual + Desplazamiento
```

## Implementación en MonoGame

```csharp
using Microsoft.Xna.Framework;
using System;

namespace VisualNovelEngine.Cinematic
{
    /// <summary>
    /// Comando que realiza zoom centrado en un punto específico del Actor
    /// </summary>
    public class ZoomToPointCommand : CinematicCommand
    {
        private readonly Actor _actor;
        private readonly Vector2 _localPoint;      // Punto en coordenadas locales del Actor (ej: cara del personaje)
        private readonly float _targetScale;       // Escala objetivo
        private readonly float _duration;          // Duración en segundos
        private readonly EasingFunction _easing;   // Función de interpolación
        
        // Estado interno de la animación
        private float _elapsedTime;
        private float _startScale;
        private Vector2 _startPosition;
        private Vector2 _targetPosition;           // Posición calculada para mantener el punto fijo
        
        // Punto en coordenadas mundiales al inicio (se mantiene fijo durante el zoom)
        private Vector2 _worldAnchorPoint;

        public ZoomToPointCommand(
            Actor actor, 
            Vector2 localPoint, 
            float targetScale, 
            float duration,
            EasingFunction easing = null)
        {
            _actor = actor ?? throw new ArgumentNullException(nameof(actor));
            _localPoint = localPoint;
            _targetScale = targetScale;
            _duration = duration;
            _easing = easing ?? EasingFunctions.Linear;
        }

        public override void Start()
        {
            _elapsedTime = 0f;
            _startScale = _actor.Scale;
            _startPosition = _actor.Position;
            
            // Calcular dónde está el punto de anclaje en coordenadas mundiales ANTES de empezar
            _worldAnchorPoint = _actor.LocalToWorld(_localPoint);
            
            // Calcular la posición final necesaria para mantener ese punto fijo con la escala objetivo
            _targetPosition = CalculatePositionForZoom(_worldAnchorPoint, _actor.Position, _targetScale);
            
            IsPlaying = true;
        }

        public override void Update(float deltaTime)
        {
            if (!IsPlaying) return;

            _elapsedTime += deltaTime;
            float progress = Math.Min(_elapsedTime / _duration, 1f);
            float easedProgress = _easing(progress);

            // Interpolar escala
            float currentScale = MathHelper.Lerp(_startScale, _targetScale, easedProgress);
            _actor.Scale = currentScale;

            // Interpolar posición para mantener el punto de anclaje fijo en pantalla
            Vector2 currentPosition = Vector2.Lerp(_startPosition, _targetPosition, easedProgress);
            _actor.Position = currentPosition;

            if (progress >= 1f)
            {
                IsPlaying = false;
                IsComplete = true;
            }
        }

        /// <summary>
        /// Calcula la posición necesaria del Actor para que worldPoint quede fijo 
        /// en pantalla cuando se aplique targetScale
        /// </summary>
        private Vector2 CalculatePositionForZoom(Vector2 worldAnchorPoint, Vector2 currentPosition, float targetScale)
        {
            // Fórmula: NuevaPos = PuntoAnclaje - (PuntoAnclaje - PosActual) * Escala
            // Pero como el punto anclaje está en el Actor, necesitamos pensarlo diferente:
            
            // El offset desde el origen del Actor hasta el punto local
            Vector2 localOffset = _localPoint;
            
            // A escala 1, el punto local está en: Posición + LocalOffset
            // A escala S, queremos que siga en la misma posición mundial:
            // NuevaPosición + (LocalOffset * S) = MismoPuntoMundial
            
            // Despejando:
            // NuevaPosición = MismoPuntoMundial - (LocalOffset * S)
            
            return worldAnchorPoint - (localOffset * targetScale);
        }

        public override void SkipToEnd()
        {
            _actor.Scale = _targetScale;
            _actor.Position = _targetPosition;
            IsPlaying = false;
            IsComplete = true;
        }
    }

    // ============================================
    // EXTENSIÓN: Métodos helper para Actor
    // ============================================
    
    public static class ActorCinematicExtensions
    {
        /// <summary>
        /// Zoom a un punto local específico (ej: cabeza del personaje)
        /// </summary>
        public static ZoomToPointCommand ZoomToPoint(
            this Actor actor, 
            Vector2 localPoint, 
            float targetScale, 
            float duration,
            EasingFunction easing = null)
        {
            return new ZoomToPointCommand(actor, localPoint, targetScale, duration, easing);
        }

        /// <summary>
        /// Zoom predefinido a la "cabeza" del personaje
        /// Asume que el sprite tiene origen en la esquina inferior o centro
        /// </summary>
        public static ZoomToPointCommand ZoomToFace(
            this Actor actor, 
            float targetScale = 1.5f, 
            float duration = 1f,
            EasingFunction easing = null)
        {
            // Asumiendo que la cara está en la parte superior del sprite
            // Ajusta estos valores según tus sprites
            Vector2 facePosition = new Vector2(
                actor.Origin.X,           // Centro horizontal
                actor.Origin.Y - 100f       // 100px arriba del origen (ajustar)
            );
            
            return actor.ZoomToPoint(facePosition, targetScale, duration, easing);
        }

        /// <summary>
        /// Zoom a una región del Actor usando coordenadas normalizadas (0-1)
        /// </summary>
        public static ZoomToPointCommand ZoomToRegion(
            this Actor actor,
            float normalizedX,    // 0 = izquierda, 0.5 = centro, 1 = derecha
            float normalizedY,    // 0 = arriba, 0.5 = centro, 1 = abajo
            float targetScale = 2f,
            float duration = 1f,
            EasingFunction easing = null)
        {
            Vector2 localPoint = new Vector2(
                actor.Size.X * normalizedX,
                actor.Size.Y * normalizedY
            );
            
            return actor.ZoomToPoint(localPoint, targetScale, duration, easing);
        }

        /// <summary>
        /// Zoom suave que puede encadenarse (retorna el comando para fluent API)
        /// </summary>
        public static CinematicSequence ZoomAndReturn(
            this CinematicSequence sequence,
            Actor actor,
            Vector2 localPoint,
            float zoomScale,
            float zoomDuration,
            float holdDuration,
            float returnDuration)
        {
            return sequence
                .Add(actor.ZoomToPoint(localPoint, zoomScale, zoomDuration, EasingFunctions.EaseInOutQuad))
                .Wait(holdDuration)
                .Add(actor.ZoomToPoint(localPoint, 1f, returnDuration, EasingFunctions.EaseInOutQuad));
        }
    }
}
```

## Ejemplo de Uso en tu Sistema de Cinemáticas

```csharp
// En tu script de cinemática:
var sequence = new CinematicSequence();

// 1. Zoom a la cara del personaje (coordenadas locales)
sequence.Add(character.ZoomToPoint(
    localPoint: new Vector2(200, 150),  // Posición de la cara en el sprite
    targetScale: 2.0f,                   // Zoom 2x
    duration: 1.5f,
    easing: EasingFunctions.EaseInOutQuad
));

// 2. Usando el helper predefinido para caras
sequence.Add(character.ZoomToFace(
    targetScale: 1.8f,
    duration: 1f
));

// 3. Zoom a región usando coordenadas normalizadas (centro superior = cara)
sequence.Add(character.ZoomToRegion(
    normalizedX: 0.5f,   // Centro horizontal
    normalizedY: 0.3f,   // 30% desde arriba (probablemente la cara)
    targetScale: 2.5f,
    duration: 2f
));

// 4. Zoom y vuelta (efecto dramático)
sequence.ZoomAndReturn(
    character,
    new Vector2(200, 150),  // Punto
    2.5f,                 // Zoom in
    1f,                   // Tiempo de zoom
    2f,                   // Mantener zoom
    0.8f                  // Zoom out
);
```

## Consideraciones Importantes

| Aspecto | Solución |
|---------|----------|
| **Origen del sprite** | Asegúrate de que `Actor.Origin` esté correctamente definido. El zoom funciona respecto al origen de transformación. |
| **Punto local vs mundial** | `_localPoint` es relativo al sprite sin escalar. Si tu sprite es 400x600, la cara podría estar en (200, 150). |
| **Interpolación suave** | Usa `EaseInOutQuad` para movimientos naturales de cámara. |
| **Zoom negativo** | El código soporta zoom out (`targetScale < 1`) manteniendo el punto fijo. |

¿Necesitas que añada soporte para **zoom con rotación** (ej: zoom a la cara mientras el personaje está inclinado) o **zoom en coordenadas de pantalla** (independiente de la posición del Actor)?
User: Las coordenadas de todo el comando y del actor son relativas de tipo 0 a 1. Es decir, la posición 0 del zoom es la esquina superior izquierda del Sprite y la coordenadas 1,1 es la esquina interior derecha. Lo mismo ocurre con las coor del actor en el juego, son relativas al ancho y alto del viewport
Kimi: Perfecto, entiendo completamente. Tienes un sistema **fully normalized** (0-1) tanto para coordenadas de Actor en viewport como para puntos locales dentro del sprite. Esto es muy elegante para visual novels responsive.

Aquí está la implementación adaptada:

```csharp
using Microsoft.Xna.Framework;
using System;

namespace VisualNovelEngine.Cinematic
{
    /// <summary>
    /// Comando de zoom centrado en un punto normalizado (0-1) del Actor.
    /// Todas las coordenadas son relativas: Actor (viewport 0-1), punto local (sprite 0-1)
    /// </summary>
    public class ZoomToPointCommand : CinematicCommand
    {
        private readonly Actor _actor;
        private readonly Vector2 _normalizedLocalPoint;  // 0,0 = esquina sup-izq del sprite | 1,1 = esquina inf-der
        private readonly float _targetScale;
        private readonly float _duration;
        private readonly EasingFunction _easing;
        
        private float _elapsedTime;
        private float _startScale;
        private Vector2 _startViewportPos;               // Posición 0-1 del actor en viewport al inicio
        private Vector2 _targetViewportPos;              // Posición calculada para mantener punto fijo
        
        // Viewport dimensions (se actualizan en Start para soportar resize)
        private int _viewportW, _viewportH;
        private Vector2 _spriteSize;

        public ZoomToPointCommand(
            Actor actor, 
            Vector2 normalizedLocalPoint,  // 0-1 dentro del sprite
            float targetScale, 
            float duration,
            EasingFunction easing = null)
        {
            _actor = actor ?? throw new ArgumentNullException(nameof(actor));
            
            // Clamp a 0-1 para seguridad
            _normalizedLocalPoint = new Vector2(
                MathHelper.Clamp(normalizedLocalPoint.X, 0f, 1f),
                MathHelper.Clamp(normalizedLocalPoint.Y, 0f, 1f)
            );
            
            _targetScale = targetScale;
            _duration = duration;
            _easing = easing ?? EasingFunctions.Linear;
        }

        public override void Start()
        {
            _elapsedTime = 0f;
            _startScale = _actor.Scale;
            _startViewportPos = _actor.ViewportPosition;  // Tu propiedad 0-1 del actor
            
            // Cachear dimensiones actuales
            _viewportW = _actor.ViewportWidth;   // o desde tu sistema de cámara/viewport
            _viewportH = _actor.ViewportHeight;
            _spriteSize = _actor.Size;            // Tamaño del sprite en píxeles (para calcular offset visual)
            
            // Calcular posición viewport objetivo que mantenga el punto local fijo en pantalla
            _targetViewportPos = CalculateViewportPositionForZoom(
                _startViewportPos,
                _startScale,
                _targetScale,
                _normalizedLocalPoint
            );
            
            IsPlaying = true;
        }

        public override void Update(float deltaTime)
        {
            if (!IsPlaying) return;

            _elapsedTime += deltaTime;
            float t = Math.Min(_elapsedTime / _duration, 1f);
            float easedT = _easing(t);

            // Interpolar escala
            float currentScale = MathHelper.Lerp(_startScale, _targetScale, easedT);
            _actor.Scale = currentScale;

            // Interpolar posición viewport (0-1)
            Vector2 currentViewportPos = Vector2.Lerp(_startViewportPos, _targetViewportPos, easedT);
            _actor.ViewportPosition = currentViewportPos;

            if (t >= 1f)
            {
                IsPlaying = false;
                IsComplete = true;
            }
        }

        /// <summary>
        /// Calcula la nueva posición viewport (0-1) del Actor para que el punto normalizado 
        /// del sprite permanezca en la misma posición de pantalla tras el zoom.
        /// </summary>
        private Vector2 CalculateViewportPositionForZoom(
            Vector2 currentViewportPos,
            float currentScale,
            float targetScale,
            Vector2 normalizedLocalPoint)
        {
            // Convertir todo a coordenadas de pantalla (píxeles) para el cálculo
            // Esto hace la matemática más intuitiva y evita errores de aspect ratio
            
            // Tamaño visual del sprite en pantalla (píxeles) a escala actual
            Vector2 currentVisualSize = new Vector2(
                _spriteSize.X * currentScale,
                _spriteSize.Y * currentScale
            );
            
            Vector2 targetVisualSize = new Vector2(
                _spriteSize.X * targetScale,
                _spriteSize.Y * targetScale
            );

            // Posición actual del actor en píxeles (centro del sprite, asumiendo origen centrado)
            // Si tu actor usa esquina superior izquierda como origen, ajusta esto
            Vector2 currentScreenPos = new Vector2(
                currentViewportPos.X * _viewportW,
                currentViewportPos.Y * _viewportH
            );

            // Offset desde el centro del actor hasta el punto de interés (píxeles, escala actual)
            // normalizedLocalPoint 0.5,0.5 = centro del sprite
            Vector2 currentOffset = new Vector2(
                (normalizedLocalPoint.X - 0.5f) * currentVisualSize.X,
                (normalizedLocalPoint.Y - 0.5f) * currentVisualSize.Y
            );

            // Posición en pantalla del punto de interés (MUNDIAL) - ¡ESTO NO DEBE CAMBIAR!
            Vector2 anchorScreenPos = currentScreenPos + currentOffset;

            // A la nueva escala, el offset al punto de interés será diferente
            Vector2 targetOffset = new Vector2(
                (normalizedLocalPoint.X - 0.5f) * targetVisualSize.X,
                (normalizedLocalPoint.Y - 0.5f) * targetVisualSize.Y
            );

            // Nueva posición del actor para compensar: Anchor - TargetOffset
            Vector2 targetScreenPos = anchorScreenPos - targetOffset;

            // Convertir de vuelta a viewport 0-1
            return new Vector2(
                targetScreenPos.X / _viewportW,
                targetScreenPos.Y / _viewportH
            );
        }

        // Versión alternativa si tu Actor usa esquina superior-izquierda como origen
        private Vector2 CalculateViewportPositionForZoomTopLeftOrigin(
            Vector2 currentViewportPos,
            float currentScale,
            float targetScale,
            Vector2 normalizedLocalPoint)
        {
            Vector2 currentVisualSize = new Vector2(
                _spriteSize.X * currentScale,
                _spriteSize.Y * currentScale
            );
            
            Vector2 targetVisualSize = new Vector2(
                _spriteSize.X * targetScale,
                _spriteSize.Y * targetScale
            );

            Vector2 currentScreenPos = new Vector2(
                currentViewportPos.X * _viewportW,
                currentViewportPos.Y * _viewportH
            );

            // Offset desde esquina sup-izq (0,0 en local = esquina sup-izq del sprite)
            Vector2 currentOffset = new Vector2(
                normalizedLocalPoint.X * currentVisualSize.X,
                normalizedLocalPoint.Y * currentVisualSize.Y
            );

            Vector2 anchorScreenPos = currentScreenPos + currentOffset;

            Vector2 targetOffset = new Vector2(
                normalizedLocalPoint.X * targetVisualSize.X,
                normalizedLocalPoint.Y * targetVisualSize.Y
            );

            Vector2 targetScreenPos = anchorScreenPos - targetOffset;

            return new Vector2(
                targetScreenPos.X / _viewportW,
                targetScreenPos.Y / _viewportH
            );
        }

        public override void SkipToEnd()
        {
            _actor.Scale = _targetScale;
            _actor.ViewportPosition = _targetViewportPos;
            IsPlaying = false;
            IsComplete = true;
        }
    }

    // ============================================
    // EXTENSIONES FLUENT API - Sistema Normalizado
    // ============================================
    
    public static class ActorCinematicExtensions
    {
        /// <summary>
        /// Zoom a punto normalizado del sprite (0-1)
        /// </summary>
        public static ZoomToPointCommand ZoomToPoint(
            this Actor actor, 
            Vector2 normalizedPoint,  // 0,0 = esquina sup-izq | 1,1 = esquina inf-der
            float targetScale, 
            float duration,
            EasingFunction easing = null)
        {
            return new ZoomToPointCommand(actor, normalizedPoint, targetScale, duration, easing);
        }

        /// <summary>
        /// Zoom a punto con coordenadas individuales (más conveniente)
        /// </summary>
        public static ZoomToPointCommand ZoomToPoint(
            this Actor actor, 
            float normalizedX,
            float normalizedY,
            float targetScale, 
            float duration,
            EasingFunction easing = null)
        {
            return new ZoomToPointCommand(actor, new Vector2(normalizedX, normalizedY), targetScale, duration, easing);
        }

        /// <summary>
        /// Zoom predefinido a la cara del personaje.
        /// Asume que la cara está en la parte superior-central del sprite.
        /// </summary>
        public static ZoomToPointCommand ZoomToFace(
            this Actor actor, 
            float targetScale = 1.8f, 
            float duration = 1f,
            EasingFunction easing = null,
            float faceY = 0.25f)  // 25% desde arriba por defecto
        {
            return actor.ZoomToPoint(0.5f, faceY, targetScale, duration, easing);
        }

        /// <summary>
        /// Zoom a ojos (más arriba que la cara general)
        /// </summary>
        public static ZoomToPointCommand ZoomToEyes(
            this Actor actor,
            float targetScale = 2.2f,
            float duration = 0.8f,
            EasingFunction easing = null)
        {
            return actor.ZoomToPoint(0.5f, 0.20f, targetScale, duration, easing);
        }

        /// <summary>
        /// Zoom a boca (para diálogos íntimos)
        /// </summary>
        public static ZoomToPointCommand ZoomToMouth(
            this Actor actor,
            float targetScale = 2.0f,
            float duration = 0.8f,
            EasingFunction easing = null)
        {
            return actor.ZoomToPoint(0.5f, 0.65f, targetScale, duration, easing);
        }

        /// <summary>
        /// Zoom a mano/objeto que sostiene el personaje (lateral)
        /// </summary>
        public static ZoomToPointCommand ZoomToHand(
            this Actor actor,
            bool rightHand = true,
            float targetScale = 1.6f,
            float duration = 1f)
        {
            float x = rightHand ? 0.85f : 0.15f;
            return actor.ZoomToPoint(x, 0.75f, targetScale, duration, EasingFunctions.EaseInOutCubic);
        }

        /// <summary>
        /// Efecto "dramático": Zoom rápido a cara y vuelta lenta
        /// </summary>
        public static CinematicSequence ZoomDramatic(
            this CinematicSequence sequence,
            Actor actor,
            Vector2? point = null,
            float zoomScale = 2.0f)
        {
            var target = point ?? new Vector2(0.5f, 0.3f);
            
            return sequence
                .Add(actor.ZoomToPoint(target, zoomScale, 0.4f, EasingFunctions.EaseOutQuad))
                .Wait(1.5f)
                .Add(actor.ZoomToPoint(target, 1.0f, 1.2f, EasingFunctions.EaseInOutQuad));
        }

        /// <summary>
        /// Zoom suave "respiración" - zoom in/out sutil en loop
        /// </summary>
        public static CinematicSequence ZoomBreathing(
            this CinematicSequence sequence,
            Actor actor,
            Vector2 point,
            float baseScale = 1.0f,
            float breathScale = 1.15f,
            float cycleDuration = 3f)
        {
            return sequence
                .Add(actor.ZoomToPoint(point, breathScale, cycleDuration * 0.4f, EasingFunctions.EaseInOutSine))
                .Add(actor.ZoomToPoint(point, baseScale, cycleDuration * 0.6f, EasingFunctions.EaseInOutSine));
        }
    }
}
```

## Ejemplos de Uso (Sistema 0-1)

```csharp
// Setup básico
var hero = new Actor { ViewportPosition = new Vector2(0.5f, 0.8f), Scale = 1f };

var sequence = new CinematicSequence();

// 1. Zoom a la cara (0.5, 0.25 = centro-arriba del sprite)
sequence.Add(hero.ZoomToPoint(0.5f, 0.25f, 2.0f, 1.5f));

// 2. Usando helpers semánticos
sequence.Add(hero.ZoomToFace(targetScale: 1.8f));        // Default: 0.5, 0.25
sequence.Add(hero.ZoomToEyes(targetScale: 2.5f));        // 0.5, 0.20 - para primer plano intenso
sequence.Add(hero.ZoomToMouth());                         // 0.5, 0.65 - para susurros

// 3. Zoom a objeto en la mano derecha (0.85, 0.75)
sequence.Add(hero.ZoomToHand(rightHand: true, 2.0f, 1f));

// 4. Secuencia dramática completa
sequence
    .Add(hero.ZoomToPoint(0.5f, 0.3f, 2.2f, 0.5f))      // Zoom rápido a cara
    .Wait(2f)                                             // Sostener
    .Add(hero.MoveTo(0.3f, 0.8f, 1f))                    // Mover actor (coords viewport 0-1)
    .Add(hero.ZoomToPoint(0.5f, 0.3f, 1.0f, 0.8f));      // Zoom out suave

// 5. Pan + Zoom combinado (mover mientras haces zoom a punto diferente)
sequence.Parallel(
    hero.MoveTo(0.7f, 0.8f, 2f),
    hero.ZoomToPoint(0.3f, 0.4f, 1.8f, 2f)              // Zoom a hombro/pezón mientras se mueve
);

// 6. Zoom "respiración" para tensión romántica
sequence.ZoomBreathing(hero, new Vector2(0.5f, 0.25f), 1.0f, 1.2f, 4f);
```

## Diagrama del Sistema de Coordenadas

```
VIEWPORT (0-1)                    SPRITE DEL ACTOR (0-1 local)
┌─────────────────────┐          ┌─────────────────────┐
│0,0             1,0  │          │0,0             1,0  │
│  ┌───────────┐      │          │  ╔═══════════╗      │
│  │   ACTOR   │      │          │  ║  ^ 0.25   ║      │
│  │  (0.5,0.8)│      │    →     │  ║  | CARA   ║      │
│  │     ▼     │      │          │  ║ 0.5,0.25  ║      │
│  └───────────┘      │          │  ╚═══════════╝      │
│0,1             1,1  │          │0,1             1,1  │
└─────────────────────┘          └─────────────────────┘

ZoomToPoint(0.5f, 0.25f, 2.0f):
• El punto (0.5, 0.25) del sprite se mantiene en la misma posición viewport
• El actor se desplaza automáticamente para compensar el escalado
• Resultado: zoom a la cara, centrado perfectamente
```

## Nota sobre Origen del Sprite

El código asume que el **origen de transformación** del Actor es el **centro** del sprite (común en VNs). Si usas esquina superior-izquierda, cambia `CalculateViewportPositionForZoom` por `CalculateViewportPositionForZoomTopLeftOrigin` o ajusta tu clase `Actor` para que exponga el origen:

```csharp
// En tu clase Actor
public Vector2 GetOriginNormalized() => _originType == OriginType.Center ? new Vector2(0.5f, 0.5f) : Vector2.Zero;
```

¿Necesitas que soporte **múltiples resoluciones dinámicas** (recálculo automático si cambia el viewport durante el zoom) o **zoom con parallax** (diferentes capas a diferentes velocidades)?
