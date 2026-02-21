using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Cameras
{
	internal class Class1
	{
	}
}

User: Eres un programador especializado en monogame. Estás programando un motor genérico para videojuegos. Quieres programar un sistema de cámaras 2d. Qué detalles tiene que tener. No muestres el código aún
Kimi: Excelente, vamos a diseñar un sistema de cámaras 2D robusto para MonoGame. Aquí están los detalles clave que debe tener:

## 🎯 Funcionalidades Core

### **Transformaciones Básicas**
- **Posición** (Vector2) - punto focal de la cámara
- **Zoom/escala** - factor de ampliación/reducción
- **Rotación** - ángulo de rotación alrededor del punto focal
- **Origen/offset** - punto de anclaje para rotaciones y escalado

### **Viewport y Resolución**
- Gestión del viewport de renderizado
- Soporte para **múltiples resoluciones** y aspect ratios
- Sistema de **letterboxing/pillarboxing** o stretching controlado
- **Virtual resolution** - renderizar a resolución fija y escalar

## 🎮 Comportamientos Avanzados

### **Seguimiento de Objetivos**
- **Target following** - suavizado con velocidad configurable (lerp/smooth damp)
- **Dead zone** - área central donde el target puede moverse sin mover cámara
- **Look ahead** - anticipación al movimiento del jugador
- **Multi-target** - seguimiento de varios objetos (zoom automático para encuadrar)

### **Límites y Constraints**
- **Bounds/World limits** - la cámara no puede salir del mundo
- **Soft limits** - con elasticidad o rebote
- **Hard limits** - límites estrictos

### **Efectos Dinámicos**
- **Shake** - vibración por impactos/explosiones (con intensidad, duración, decay)
- **Trauma system** - acumulación de shake que decae naturalmente
- **Parallax scrolling** - múltiples capas con velocidades diferentes
- **Screen transitions** - fades, wipes, transiciones suaves entre zonas

## 🛠️ Sistema de Coordenadas

### **Conversiones Esenciales**
- `WorldToScreen()` - transformar coordenadas del mundo a pantalla
- `ScreenToWorld()` - transformar coordenadas de pantalla a mundo (crítico para input)
- `WorldToViewport()` / `ViewportToWorld()` - para UI y debug

### **Matrices de Transformación**
- Matriz `View` (inversa de la transformación de la cámara)
- Matriz `Projection` (ortográfica para 2D)
- Caché de matrices para evitar recálculos innecesarios

## 🎨 Rendering

### **Culling/Optimización**
- **Frustum culling** 2D - solo dibujar lo visible
- `GetVisibleBounds()` - rectángulo del mundo visible actual
- Soporte para **SpriteBatch** con transform matrix

### **Post-procesado Básico**
- Estructura preparada para shaders (vignette, chromatic aberration, etc.)

## ⚙️ Arquitectura

### **Componentes del Sistema**
- **Camera Manager** - gestiona múltiples cámaras (split-screen, transiciones)
- **Camera Controller** - comportamientos intercambiables (follow, cinematic, manual)
- **Interpolator** - sistema de tweening para movimientos suaves

### **Eventos/Callbacks**
- `OnCameraMoved`, `OnZoomChanged`, `OnTargetChanged`
- Sistema de notificación para sistemas que dependen de la cámara (UI, audio posicional)

## 🔧 Utilidades

### **Debug y Desarrollo**
- Visualización de bounds, dead zone, grid
- Guardar/cargar posiciones de cámara
- Modo libre (fly camera) para debug

### **Serialización**
- Guardar estado de cámara (posición, zoom, target)
- Sistema de **cinematicas** con keyframes

## 🚀 Consideraciones de Performance

- **Lazy updates** - solo recalcular matriz cuando cambien parámetros
- **Object pooling** para efectos de shake
- **Spatial hashing** para culling eficiente

¿Quieres que profundice en algún aspecto específico antes de empezar a codear? Por ejemplo, el sistema de shake con acumulación de trauma es particularmente satisfactorio en juegos de acción.
User: Los sistemas adicionales como shake, deberían ser comportamientos añadidos a la cámara que se puedan activar o desactivar
Kimi: Excelente punto. Vamos a diseñar esto con una arquitectura **modular y desacoplada**, donde los efectos sean comportamientos independientes que se puedan apilar, configurar y activar/desactivar dinámicamente.

---

## 🏗️ Arquitectura de Comportamientos

### **Estrategia: Decorator Pattern + Component System**

En lugar de una cámara monolítica, separamos responsabilidades:

```
Camera (core) ← tiene → Lista de ICameraBehaviors
```

Cada comportamiento es un **procesador de transformación** que recibe la matriz/estado actual y devuelve uno modificado.

---

## 🧩 Interfaz Base

### **ICameraBehavior**
```csharp
// Pseudocódigo conceptual
interface ICameraBehavior {
    bool IsActive { get; set; }
    int Priority { get; } // Orden de aplicación
    
    void Update(GameTime gameTime, CameraState currentState, out CameraState modifiedState);
    void Reset(); // Para reiniciar estado interno
}
```

### **CameraState (Struct)**
Datos inmutables que fluyen por la cadena de comportamientos:
- Position, Rotation, Zoom, Origin
- Target (opcional)
- Timestamp/frame para interpolaciones

---

## 🎮 Comportamientos Core

### **1. FollowBehavior**
- Seguimiento suavizado con configurable `SmoothSpeed`
- Dead zone con `Vector2` (independiente X/Y)
- Look-ahead basado en velocidad del target
- **Snapping**: opción para teletransporte instantáneo si la distancia es muy grande

### **2. BoundsConstraintBehavior**
- Límites del mundo (`RectangleF` o polígono)
- Modo: Hard stop / Soft spring / Bounce back
- Padding configurable

### **3. ZoomToFitBehavior**
- Para estrategia o cutscenes
- Encuadra N targets manteniendo márgenes
- Zoom suavizado con damping

---

## 💥 Comportamientos de Efectos (Toggleables)

### **4. ShakeBehavior**
```csharp
// Configuración granular
float Trauma;          // 0-1, acumulable
float TraumaDecay;     // Cuánto baja por segundo
float ShakeIntensity;  // Amplitud máxima
float ShakeFrequency;  // Velocidad de oscilación
int Seed;              // Para reproducibilidad/perlin noise
```
- **Tipos**: Perlin (suave), Random (errático), Sine (ondulado)
- **Direccional**: opción para shake solo en eje X (daño lateral) o Y (aterrizaje)

### **5. TraumaSystemBehavior**
- Wrapper que otros sistemas alimentan:
  - `AddTrauma(0.3f)` desde explosiones
  - `AddTrauma(0.1f)` desde disparos
  - Decay automático, shake proporcional a trauma² (curva más natural)

### **6. RecoilBehavior**
- Empuje instantáneo + recuperación suave
- Stackable: cada disparo añade offset, se recupera progresivamente
- Configurable: `RecoilForce`, `RecoverySpeed`, `MaxRecoil`

### **7. ParallaxOffsetBehavior**
- Offset adicional basado en capa de fondo
- Útil para fake-3D o multiplicar movimiento de capas

### **8. CinematicBehavior**
- Keyframe animation system
- Curvas de animación (ease-in/out, bezier)
- Trigger por eventos o tiempo

### **9. FocusPointBehavior**
- Atrae la cámara hacia punto de interés temporal
- Útil para enemigos importantes, items, etc.
- Peso configurable (0 = ignorar, 1 = máxima prioridad)

---

## ⚙️ Sistema de Gestión

### **BehaviorStack**
```csharp
// API deseada
camera.AddBehavior(new ShakeBehavior { Priority = 100 });
camera.EnableBehavior<ShakeBehavior>();
camera.DisableBehavior<ShakeBehavior>();
camera.RemoveBehavior<ShakeBehavior>();
camera.ToggleBehavior<RecoilBehavior>();

// Grupos de comportamientos
camera.SetProfile("Combat"); // Activa shake+recoil, desactiva smooth follow lento
camera.SetProfile("Exploration"); // Follow suave, sin efectos
```

### **Resolución de Conflictos**
- **Prioridad numérica**: menor número = aplica primero (base), mayor = último (efectos)
- **Modos de combinación**:
  - `Additive`: suma offsets (shake + recoil)
  - `Override`: reemplaza valor anterior (cinematic anula follow)
  - `Blend`: interpola entre comportamientos (transiciones suaves)

---

## 🔌 Integración con Otros Sistemas

### **Eventos que pueden activar comportamientos**
```csharp
// Desde sistema de armas
eventBus.Subscribe<WeaponFired>(e => {
    camera.GetBehavior<RecoilBehavior>()?.ApplyRecoil(e.WeaponKick);
    camera.GetBehavior<TraumaSystemBehavior>()?.AddTrauma(0.05f);
});

// Desde sistema de daño
eventBus.Subscribe<DamageTaken>(e => {
    camera.GetBehavior<ShakeBehavior>()?.AddDirectionalTrauma(0.4f, e.HitDirection);
});
```

### **Debug Visual**
- Overlay que muestra qué comportamientos están activos
- Sliders en tiempo real para ajustar intensidad de efectos
- Historial de activaciones

---

## 🎯 Ventajas de Este Diseño

| Aspecto | Beneficio |
|--------|-----------|
| **Testeable** | Cada comportamiento se testea aislado |
| **Reutilizable** | Mismo shake para cámara de mundo y UI |
| **Data-driven** | Configuraciones desde JSON/scriptable objects |
| **Performante** | Solo procesar comportamientos activos |
| **Extensible** | Nuevo efecto = nueva clase, sin tocar código existente |

---

¿Te parece bien esta arquitectura? ¿Quieres que añada algún comportamiento específico (como **dolly zoom**, **screen shake basado en audio**, o **transiciones entre cámaras**) antes de pasar a implementación?
User: Aún no vamos a implementar Los comportamientos. Nuestra escena va a tener una serie de capas incluyendo capas de interface de usuario. El interface de usuario está definido con coordenadas relativas 0 a 1. Primero se dibujan las capas de gráficos y después las capas de interface de usuario. Puede que tengamos capas con imágenes que también tengan coordenadas relativas. Las coordenadas de cámara debe distinguir entre coordenadas de pantalla y mundo. Se debe separar el dibujo de modo que las entidades a dibujar se guardan para después dibujarla en una pipeline separada donde podremos aplicar efectos. No muestres código, sólo resume las características
Kimi: Entendido. Aquí está el diseño resumido de la arquitectura de renderizado:

---

## 🎯 Sistema de Capas

### **Tipos de Capa**
- **World Layers**: Coordenadas mundo (unidades de juego)
- **Screen Layers**: Coordenadas pantalla 0-1 (UI e imágenes fijas)
- **Hybrid Layers**: Elementos posicionados en mundo pero que ignoran cámara (HUDs 3D, marcas)

### **Orden de Renderizado**
1. World Layers (con transformación cámara)
2. Hybrid Layers (posición mundo, escala fija)
3. Screen Layers (0-1, independiente de cámara)

---

## 📐 Sistema de Coordenadas

| Espacio | Uso | Transformación |
|--------|-----|---------------|
| **World** | Entidades, física, gameplay | Base |
| **Camera** | Vista transformada por cámara | World × ViewMatrix |
| **Screen** | Input ratón, UI | Camera → Viewport |
| **Normalized (0-1)** | UI responsive, layouts | Screen / Resolution |

---

## 🏭 Pipeline de Renderizado Diferido

### **Fase 1: Culling y Registro**
- Cada entidad calcula su `RenderCommand` (textura, posición, profundidad, flags)
- Comandos se encolan en `RenderQueue` por capa
- Frustum culling en world layers

### **Fase 2: Clasificación**
- Sorting por: `Layer → Depth → Material/Texture` (minimiza cambios de estado GPU)

### **Fase 3: Ejecución**
- World: `SpriteBatch.Begin(transformMatrix: camera.ViewMatrix)`
- Screen: `SpriteBatch.Begin(matrix: resolutionScaleMatrix)`
- Post-procesado por capa disponible

---

## 🎨 Características Clave

- **RenderQueue inmutable**: snapshot del frame para paralelización futura
- **Command pattern**: cada draw es un objeto con todos los datos necesarios
- **Layer masks**: entidades pueden pertenecer a múltiples capas (sombras, reflejos)
- **Viewport virtual**: resolución de diseño escalada a pantalla real
- **Safe area**: márgenes para notch/overscan en capas screen

---

## 🔧 Efectos y Post-procesado

- **Por capa**: shaders aplicados al batch completo de una capa
- **Por entidad**: flags en RenderCommand para efectos individuales
- **Compositing**: capas pueden renderizarse a `RenderTarget` intermedios
- **Transitions**: interpolación de opacidad/transform entre estados de capa

---

## 🎮 Integración UI

- **Anchor system**: elementos UI anclados a bordes/center con offsets
- **Layout groups**: horizontal/vertical/grid con spacing proporcional
- **Escalado**: UI se adapta a aspect ratio manteniendo relaciones 0-1
- **Input**: mapeo automático screen→world cuando sea necesario (click en mundo desde UI)

---

¿Ajustamos algo antes de entrar en implementación?
User: Los rendercommand ¿Deberían tener información de clipping sobre la cámara? ¿Podemos manejar varias cámaras en el director y dibujar en un lote? ¿Podemos dibujar el interface de usuario en una cámara especial?
Kimi: Excelentes preguntas. Aquí el análisis:

---

## ✂️ Clipping en RenderCommand

### **Opción A: Sin clipping en comando**
- Cámara hace culling antes de encolar
- Comando solo si es visible
- **Problema**: efectos dinámicos (shake, zoom) pueden traer entidades de vuelta a pantalla

### **Opción B: Bounds en comando**
- Cada comando lleva `WorldBounds` (AABB o esfera)
- Pipeline de render puede recalcular visibilidad con matriz de cámara final
- **Ventaja**: válido para cualquier cámara que procese el comando

### **Opción C: Clipping planes + scissor**
- Para UI: `ScissorRectangle` en coordenadas screen
- Para mundo: `ClipBounds` opcional (ventanas, portales)
- **Decisión**: sí, campo opcional `ClipRegion?` en RenderCommand

---

## 🎥 Múltiples Cámaras en Director

### **Arquitectura Soportada**
```
Director
├── Camera "Main" → RenderTarget "World"
├── Camera "Minimap" → RenderTarget "Minimap"  
├── Camera "Cinematic" → RenderTarget "Cinematic"
└── Camera "UI" → Backbuffer (o RT)
```

### **Batching Estratégico**
- **No**: un solo lote para todas las cámaras (diferentes transforms)
- **Sí**: misma `RenderQueue` filtrada por `CameraMask` por cámara
- Cada cámara itera comandos, aplica su culling, renderiza su vista

### **Optimización**
- Comandos con `CameraMask.All` se renderizan en todas
- Comandos específicos solo en su cámara (efectos de primera persona, overlays)

---

## 🖥️ UI en Cámara Especial

### **Diseño Recomendado**
| Aspecto | Implementación |
|--------|---------------|
| **Cámara UI** | Ortográfica, 1:1 con virtual resolution |
| **Coordenadas** | 0-1 normalizadas → transformadas a pixels por cámara |
| **Proyección** | `Matrix.CreateOrthographicOffCenter(0, 1, 1, 0, 0, 1)` |
| **Viewport** | Puede ser sub-región (split-screen) o full screen |

### **Ventajas**
- UI no sufre efectos de cámara mundo (shake, zoom)
- UI puede tener su propio post-procesado (blur fondo, vignette)
- Fácil pausar mundo pero UI animada
- Soporte para múltiples viewports UI (pantalla dividida con HUDs separados)

### **Sincronización**
- Cámara UI opcionalmente sigue posición de cámara mundo (minimapa)
- O completamente independiente (HUD estático)

---

## 🎯 Síntesis de Decisiones

| Pregunta | Respuesta |
|----------|-----------|
| ¿Clipping en RenderCommand? | Sí, `Bounds` para culling + `ClipRegion?` opcional |
| ¿Varias cámaras, un lote? | Un `RenderQueue`, múltiples pasadas filtradas por máscaras |
| ¿UI en cámara especial? | Sí, cámara ortográfica dedicada con coordenadas 0-1 |

---

## 🏗️ Flujo de Render Finalizado

```
1. Update: Entidades generan RenderCommands → Queue global
2. Cámara World: Filtra por mask, culling bounds, renderiza a RT
3. Cámara UI: Filtra comandos UI, coordenadas 0-1, renderiza
4. Compositing: Combinar RTs (world + effects + UI)
5. Present: Escalado final a backbuffer
```

---

¿Confirmamos este diseño o ajustamos el manejo de máscaras de cámara?
User: Se podría dejar los métodos update y draw por separado. Update hace los cambios del actor y draw prepara el rendercommand? Eso nos facilitaría poder separar los conceptos en las entidades
Kimi: Exacto. Separar `Update` y `Draw` en responsabilidades distintas clarifica la arquitectura:

---

## 🔄 Separación de Responsabilidades

### **Update (Lógica del Actor)**
- Física, movimiento, animaciones, IA
- Estados internos, timers, colisiones
- **No toca nada de renderizado**

### **PrepareRender / CollectDraw (Generación de Comandos)**
- Crea `RenderCommand` con datos actuales del actor
- No ejecuta draw, solo describe qué dibujar
- Puede decidir no generar comando (invisible, culled por entidad)

---

## 🎯 Beneficios de la Separación

| Aspecto | Ventaja |
|--------|---------|
| **Testeabilidad** | Update testeable sin gráficos |
| **Paralelismo** | Update en thread, render en otro |
| **Repetición** | Puedes "draw" múltiples veces sin re-update (reflejos, sombras) |
| **Rollback** | Guardar commands de frame anterior para debug/replay |
| **Cámara independiente** | Mismo actor, diferentes vistas sin recalcular lógica |

---

## 🏗️ Estructura de la Entidad

```
Actor/Entity
├── Update(gameTime) → void
│   └── Modifica: Position, Rotation, AnimationFrame, State
│
└── CollectDraw(RenderQueue, CameraMask) → void
    └── Encola: RenderCommand(s) con datos snapshot actuales
```

---

## 📋 Flujo de Frame Separado

```
PHASE: UPDATE (lógica, secuencial o paralelo por grupos)
├── Actor1.Update()
├── Actor2.Update()
└── ...

PHASE: COLLECT (generación de comandos, por cámara o global)
├── foreach actor in visibleSet
│   └── actor.CollectDraw(queue, camera.Mask)
└── UI.CollectDraw(uiQueue)

PHASE: RENDER (pipeline, ordenado y optimizado)
├── Sort queues
├── Execute world camera
├── Execute UI camera  
└── Composite
```

---

## 🎨 Consideraciones Adicionales

### **Snapshot de Estado**
- `CollectDraw` lee estado actual pero crea comando inmutable
- Si el actor cambia después, el comando ya está "congelado"
- Evita tearing visual si update continúa

### **Múltiples Comandos por Actor**
- Un actor puede generar N comandos:
  - Cuerpo + sombra + outline + efecto glow
  - Cada uno con diferente `Layer`, `Depth`, `Flags`

### **Culling en Collect**
- Actor puede hacer culling temprano con `camera.Bounds`
- Más barato que encolar y descartar después

### **UI**
- También tiene `Update` (animaciones de botones, transiciones)
- Y `CollectDraw` (genera comandos en coordenadas 0-1)

---

## ✅ Resumen

| Método | Cuándo | Qué hace |
|--------|--------|----------|
| `Update` | Cada frame, lógica | Mueve, anima, decide |
| `CollectDraw` | Después de update, por cámara | Describe qué dibujar, dónde, cómo |
| Pipeline render | Final de frame | Ejecuta los comandos |

¿Confirmamos esta separación? ¿Añadimos algún detalle sobre cómo el actor conoce la cámara para culling en `CollectDraw`?
User: Se podría lanzar el rendercommand sobre la cámara en el draw? Así será cada cámara quién tenga la lista de comandos y el director puede recuperar toda la lista de comandos de todas las cámaras y ejecutar el renderizado real
Kimi: Perfecto. Invertimos el flujo: la cámara recibe los comandos, el director las recolecta y ejecuta.

---

## 🎥 Nueva Arquitectura: Cámara como Receptora

### **Cámara como Contenedor**
- Cada cámara mantiene su propia `RenderQueue`
- Actores "lanzan" comandos a cámaras específicas
- Cámara filtra por `CameraMask`, `Bounds`, `Layer`

---

## 🔄 Flujo de Draw Invertido

```
Actor.CollectDraw()
└── foreach camera in visibleCameras
    └── camera.Submit(RenderCommand)
        ├── Culling por bounds
        ├── Filtrado por mask/layer
        └── Encolar en camera.RenderQueue
```

---

## 🏛️ Director como Orquestador

```
Director.Update()
└── Actor.Update() en todas las entidades

Director.CollectDraw()
└── Actor.CollectDraw(cameraSystem) en todas las entidades

Director.Render()
├── foreach camera in activeCameras (ordenado por priority)
│   ├── Sort camera.RenderQueue
│   ├── Apply camera.Effects/PostProcess
│   └── Execute to camera.RenderTarget
└── Composite all camera outputs
```

---

## 📋 Responsabilidades Claras

| Componente | Tarea |
|-----------|-------|
| **Actor** | Generar comandos, decidir a qué cámaras enviar |
| **Camera** | Recibir, filtrar, almacenar, ordenar, renderizar su vista |
| **Director** | Coordinar fases, recolectar outputs, compositing final |

---

## 🎯 Ventajas del Diseño

- **Desacoplamiento**: Actor no conoce pipeline, solo "tira" comandos
- **Flexibilidad**: Cámara puede rechazar comandos (culling, layer disabled)
- **Multi-view**: Un actor puede aparecer en múltiples cámaras automáticamente
- **Optimización**: Cámaras inactivas no reciben comandos
- **Debugging**: Inspeccionar qué ve cada cámara individualmente

---

## 🎨 Casos de Uso

| Escenario | Comportamiento |
|----------|---------------|
| **Minimapa** | Cámara con bounds reducidos, recibe comandos de entidades + overlays |
| **Split-screen** | Dos cámaras, cada una recibe según `PlayerMask` |
| **Cinematic** | Cámara temporal que roba comandos de cámara principal |
| **UI** | Cámara ortográfica que solo recibe comandos `Layer.UI` |

---

## ✅ Resumen Final

```
ACTOR: "Tengo un sprite en (100,50)"
└── Submit to Camera.Main, Camera.Minimap

CAMERA.MAIN: "Recibido, visible, encolado"
CAMERA.MINIMAP: "Recibido, fuera de bounds, descartado"

DIRECTOR: "Renderizo Main primero, luego UI"
```

¿Confirmamos esta dirección? ¿Definimos cómo el actor descubre a qué cámaras enviar (registro global, broadcast, o cámara se suscribe a layers)?
User: El director de cámara pertenece a una escena que tiene capas que tiene actores. La capa es la que sabe las cámaras a las que se asocia del director de cámaras de la escena
Kimi: Entendido. La capa es el puente entre actores y cámaras.

---

## 🏛️ Jerarquía de Responsabilidades

```
Scene
├── LayerDirector
│   └── Layers[]
│       ├── Layer "Gameplay" → Cámaras [Main, Minimap]
│       ├── Layer "Background" → Cámara [Main]
│       └── Layer "UI" → Cámara [UI]
│
└── CameraDirector
    └── Cámaras [Main, Minimap, UI]
        └── cada una con su RenderQueue
```

---

## 🎯 Capa como Enrutadora

### **Configuración de Capa**
- `Layer.CameraMask` → qué cámaras reciben sus actores
- `Layer.DrawOrder` → orden entre capas
- `Layer.ParallaxFactor` → opcional, offset multiplicado

### **Flujo de Draw**

```
Scene.CollectDraw()
└── foreach layer in LayerDirector (ordenado)
    └── foreach actor in layer.Actors
        └── actor.CollectDraw(layer.CameraMask)
            └── foreach camera in CameraDirector.GetByMask(mask)
                └── camera.Submit(RenderCommand)
```

---

## 📋 Ventajas del Diseño

| Aspecto | Beneficio |
|--------|-----------|
| **Organización** | Actores agrupados por propósito, no por cámara |
| **Flexibilidad** | Mover capa a otra cámara sin tocar actores |
| **Optimización** | Capa puede desactivarse → ningún comando generado |
| **Parallax** | Capa "Nubes" con factor 0.5, misma cámara, offset automático |
| **Debug** | Visualizar qué capa va a qué cámara |

---

## 🎨 Casos Especiales

| Caso | Solución |
|-----|----------|
| **Actor en múltiples capas** | Raro, pero capa con `CameraMask.All` |
| **Cambio de cámara dinámico** | Modificar `layer.CameraMask` en runtime |
| **Layer solo para efectos** | Capa sin cámaras, usada para post-proceso |
| **UI sobre mundo** | Capa UI con `CameraMask.UI`, orden después de mundo |

---

## ✅ Síntesis

```
CAPA: "Soy Gameplay, mis actores van a Main y Minimap"
ACTOR: "Dibujo un sprite"
  → Layer lo envía a Camera.Main y Camera.Minimap
  → Cada cámara decide si lo encola o descarta

DIRECTOR: "Renderizo colas de cada cámara en orden"
```

¿Ajustamos algo? ¿Añadimos parallax por capa o efectos de capa (tintado global, etc.)?
User: Añadamos el parallax por capa y efectos sobre capa como comportamientos añadidos a la capa
Kimi: Excelente. Extendemos la capa con comportamientos modulares, igual que hicimos con la cámara.

---

## 🏛️ Capa con Comportamientos

```
Layer
├── Configuración Base
│   ├── CameraMask
│   ├── DrawOrder
│   └── Actors[]
│
└── Behaviors: ILayerBehavior[]
    ├── ParallaxBehavior
    ├── TintBehavior
    ├── ScrollBehavior
    └── Custom...
```

---

## 🎯 Comportamientos de Capa

### **ParallaxBehavior**
- `Factor: Vector2` (0 = fijo, 1 = velocidad cámara, 0.5 = mitad)
- Aplica offset a todos los `RenderCommand` de la capa
- Diferente factor por eje (horizontal vs vertical)

### **TintBehavior**
- `Color: Color` con `Intensity: float`
- Multiplica color de todos los sprites de la capa
- Para atmósfera (noche, veneno, calor)

### **ScrollBehavior**
- `Speed: Vector2` en unidades/segundo
- Offset automático independiente de cámara
- Para fondos infinitos, nubes moviéndose

### **TransformBehavior**
- `Scale`, `Rotation`, `Skew` globales a la capa
- Para efectos de "mundo inclinado", zoom dramático

### **MaskBehavior**
- `ClipRegion` o stencil
- Solo dibujar dentro de forma (ventanas, portales)

---

## 🔄 Flujo con Behaviors

```
Layer.CollectDraw()
├── foreach actor in Actors
│   └── actor.CollectDraw(CameraMask) → comandos temporales
│
├── foreach behavior in Behaviors (orden de prioridad)
│   └── behavior.Apply(ref commands, camera, gameTime)
│       // Parallax: offset posición
│       // Tint: modificar color
│       // etc.
│
└── foreach command in commands final
    └── Submit to cámaras del mask
```

---

## 🎨 Combinaciones de Efectos

| Escenario | Behaviors aplicados |
|----------|---------------------|
| **Fondo lejano** | Parallax(0.2), Tint(azul oscuro, 0.3) |
| **Nubes animadas** | Parallax(0.5), Scroll(10, 0) |
| **Mundo de espejo** | Transform(scaleY: -1) |
| **Interior ventana** | Mask(rect), Parallax(0.8) |

---

## 🔧 Arquitectura Paralela

```
CameraBehavior (afecta transform de cámara)
     ↑
ICameraBehavior

LayerBehavior (afecta comandos de la capa)
     ↑
ILayerBehavior
```

Ambos sistemas:
- `IsActive`, `Priority`, `Reset()`
- Encadenables y desactivables en runtime
- Configurables desde datos (JSON/scriptable)

---

## ✅ Síntesis Final

```
CAPA "Cielo":
├── CameraMask: Main
├── Behaviors: [Parallax(0.1), Scroll(5,0), Tint(#87CEEB,0.2)]
└── Actors: [Nube1, Nube2, Sol]

Resultado: Fondo azul claro, se mueve lento, nubes animadas
```

¿Definimos la interfaz `ILayerBehavior` o añadimos más comportamientos específicos?
User: Sí fuera posible me gustaría que los comportamientos de cámara y capa desciendan de la misma base
Kimi: Excelente idea. Unificamos en una jerarquía común para consistencia y reutilización.

---

## 🏛️ Jerarquía Unificada

```
IBehavior (base)
├── IsActive, Priority, Reset()
├── Update(gameTime) → lógica interna
└── Apply(context) → modificación

    ├── ICameraBehavior : IBehavior
    │   └── Apply(CameraContext)
    │       └── modifica: Position, Rotation, Zoom, etc.
    │
    └── ILayerBehavior : IBehavior  
    └── Apply(LayerContext)
        └── modifica: RenderCommands, offset, tint, etc.
```

---

## 🎯 Contextos Específicos

| Contexto | Contiene | Propósito |
|---------|---------|-----------|
| `CameraContext` | Cámara, DeltaTime, Target | Transformar vista |
| `LayerContext` | Layer, Cámara actual, Commands, DeltaTime | Transformar comandos |

---

## 🔧 Implementación Base

### **IBehavior<TContext>**
```csharp
// Genérico para reutilizar lógica común
interface IBehavior<TContext> {
    bool IsActive { get; set; }
    int Priority { get; }
    
    void Update(GameTime time);
    void Apply(TContext context);
    void Reset();
}
```

### **Comportamientos Compartidos**

| Comportamiento | Camera | Layer | Notas |
|---------------|--------|-------|-------|
| **Shake** | ✅ Aplica a posición | ✅ Aplica a offset | Misma lógica, diferente target |
| **SmoothFollow** | ✅ Sigue target | — | Solo cámara |
| **Parallax** | — | ✅ Offset commands | Solo capa |
| **Oscillate** | ✅ Zoom pulso | ✅ Posición Y onda | Matemática compartida |
| **Decay** | ✅ Trauma → shake | ✅ Tint → transparente | Curva de fade reutilizable |

---

## 🎨 Reutilización de Código

### **ShakeBehavior<T>**
- Lógica de ruido Perlin compartida
- `CameraShake : ShakeBehavior<CameraContext>`
- `LayerShake : ShakeBehavior<LayerContext>`

### **CurveBehavior<T>**
- Animación por curvas (Bezier, Ease)
- Aplica a zoom de cámara O escala de capa

---

## 🔄 Flujo Unificado

```
Update Phase:
├── foreach behavior in allBehaviors
│   └── behavior.Update(time)

CollectDraw Phase:
├── CameraDirector.UpdateBehaviors()
│   └── cámara lista con transforms aplicados
│
├── foreach layer
│   └── layer.CollectDraw(camera)
│       ├── actor commands
│       ├── layerBehaviors.Apply(LayerContext)
│       └── submit a cámara
│
└── CameraDirector.Render()
    └── cameraBehaviors ya aplicados en Update
```

---

## ✅ Beneficios

| Aspecto | Ganancia |
|--------|----------|
| **Código compartido** | Shake, oscilaciones, curvas en un solo lugar |
| **Editor unificado** | Mismo panel para behaviors de cámara y capa |
| **Extensibilidad** | Nuevo behavior = implementar dos contextos |
| **Testing** | Lógica base testeada una vez |
| **Serialización** | Formato común para guardar/cargar |

---

## 🎯 Ejemplo Visual

```
SHAKE en cámara:   [Cámara se mueve] → mundo entero tiembla
SHAKE en capa:     [Capa se desplaza] → solo nubes tiemblan

Mismo código base, diferente contexto de aplicación
```

¿Profundizamos en la implementación genérica o en comportamientos específicos compartidos?
User: No. Vamos a ir implementando por componentes. Cuál es el orden lógico de implementación?
Kimi: Aquí está el orden lógico de implementación, de base a features complejas:

---

## 🏗️ Fase 1: Fundamentos (sin los cuales no hay nada)

| Orden | Componente | Por qué primero |
|-------|-----------|-----------------|
| 1 | **Vector2/Rect utilidades** | Todo usa coordenadas |
| 2 | **CameraState** (struct inmutable) | Snapshot de transform |
| 3 | **ICameraBehavior** (interfaz base) | Contrato para behaviors |
| 4 | **Camera básica** | Posición, zoom, rotación + matriz View |
| 5 | **WorldToScreen / ScreenToWorld** | Input y debug indispensables |

---

## 🎨 Fase 2: Renderizado Básico

| Orden | Componente | Dependencia |
|-------|-----------|-------------|
| 6 | **RenderCommand** | Qué dibujar, dónde, cómo |
| 7 | **RenderQueue** | Lista ordenable de comandos |
| 8 | **SpriteBatch integración** | Ejecutar comandos con matriz cámara |
| 9 | **Actor.CollectDraw** | Generar comandos desde entidades |
| 10 | **Camera.Submit + Render** | Recibir, ordenar, dibujar |

---

## 🏛️ Fase 3: Escena y Capas

| Orden | Componente | Por qué ahora |
|-------|-----------|---------------|
| 11 | **Layer** | Contenedor de actores |
| 12 | **Layer.CameraMask** | Enrutar a cámaras |
| 13 | **Scene** | Orquesta Update + CollectDraw |
| 14 | **CameraDirector** | Gestiona múltiples cámaras |
| 15 | **LayerDirector** | Ordena capas, asigna cámaras |

---

## 🎮 Fase 4: Behaviors Básicos

| Orden | Componente | Validación |
|-------|-----------|------------|
| 16 | **FollowBehavior** | Cámara sigue target |
| 17 | **BoundsConstraint** | No salir del mundo |
| 18 | **ParallaxBehavior (capa)** | Fondo se mueve lento |
| 19 | **ScrollBehavior (capa)** | Capa auto-animada |

---

## 💥 Fase 5: Efectos y Polish

| Orden | Componente | Requiere |
|-------|-----------|----------|
| 20 | **ShakeBehavior (cámara)** | Trauma, decay, noise |
| 21 | **ShakeBehavior (capa)** | Reutilizar lógica base |
| 22 | **TintBehavior (capa)** | Color overlay |
| 23 | **RecoilBehavior (cámara)** | Impulso + recuperación |
| 24 | **ZoomToFitBehavior** | Múltiples targets |

---

## 🖥️ Fase 6: UI y Avanzado

| Orden | Componente | Contexto |
|-------|-----------|----------|
| 25 | **Camera ortográfica UI** | Coordenadas 0-1 |
| 26 | **Layer UI** | No afectada por cámara mundo |
| 27 | **RenderTargets** | Post-proceso por cámara |
| 28 | **Compositing Director** | Mezclar cámaras |
| 29 | **CinematicBehavior** | Keyframes, curvas |
| 30 | **Transition system** | Fade, wipe entre cámaras |

---

## 🧪 Regla de Oro

> **Cada fase debe poder probarse visualmente antes de pasar a la siguiente**

- Fase 1-2: Cámara azul, sprite rojo que sigue el ratón
- Fase 3-4: Dos capas, una parallax, cambio de cámara con tecla
- Fase 5: Shake al clicar, recoil al espacio
- Fase 6: UI con botón que cambia de escena

---

## 🚀 Checkpoint de Decisiones

| Punto | Decisión pendiente |
|-------|-------------------|
| Después de 5 | ¿Virtual resolution o pixel-perfect? |
| Después de 10 | ¿Culling por bounds o siempre submit? |
| Después de 15 | ¿Split-screen ahora o más tarde? |
| Después de 20 | ¿Trauma system separado o en Shake? |

---

¿Empezamos con la Fase 1 o quieres ajustar el orden?
User: Comencemos por la fase 1
Kimi: Entendido. Aquí está el diseño detallado de la **Fase 1: Fundamentos**.

---

## 1. Vector2/Rect Utilidades

### **MathUtils**
- `Lerp(float a, float b, float t)` - interpolación lineal
- `SmoothStep(float a, float b, float t)` - suavizado
- `Clamp(float value, float min, float max)`
- `Approach(float current, float target, float maxDelta)` - movimiento hacia valor con límite de velocidad

### **RectangleF**
- Estructura para bounds con floats (X, Y, Width, Height)
- `Contains(Vector2 point)`, `Intersects(RectangleF other)`
- `Center`, `TopLeft`, `BottomRight` como propiedades
- `Expand(float amount)` para padding

### **Vector2 Extensions**
- `Perpendicular()` - vector perpendicular (para rotaciones 2D)
- `Rotate(float radians)` - rotación alrededor de origen
- `Snap(float gridSize)` - ajustar a grid

---

## 2. CameraState (Struct Inmutable)

Datos que definen una cámara en un instante:

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `Position` | Vector2 | Punto focal de la cámara |
| `Rotation` | float | Ángulo en radianes |
| `Zoom` | float | 1.0 = normal, 2.0 = 2x ampliado |
| `Origin` | Vector2 | Punto de anclaje (normalmente centro pantalla) |

### **Propiedades derivadas**
- `ViewMatrix`: matriz transformación inversa (para SpriteBatch)
- `InverseViewMatrix`: de vuelta a mundo

---

## 3. ICameraBehavior (Interfaz Base)

```csharp
// Pseudocódigo de la interfaz unificada
interface IBehavior<TContext> {
    bool IsActive { get; set; }
    int Priority { get; } // Orden de aplicación, menor primero
    
    void Update(GameTime gameTime); // Lógica interna
    void Apply(TContext context);   // Modificación
    void Reset();                   // Reinicio de estado
}

interface ICameraBehavior : IBehavior<CameraContext> { }
```

### **CameraContext**
```csharp
struct CameraContext {
    public CameraState CurrentState;    // Entrada
    public CameraState ModifiedState;   // Salida (inicialmente = Current)
    public GameTime Time;
    public float DeltaTime;
}
```

---

## 4. Cámara Básica

### **Propiedades**
- `State: CameraState` - estado actual
- `Behaviors: List<ICameraBehavior>` - comportamientos activos
- `Viewport: Viewport` - área de pantalla asignada
- `VirtualResolution: Vector2` - resolución de diseño (opcional por ahora)

### **Métodos**
- `Update(gameTime)` - actualizar behaviors, recalcular matriz
- `GetViewMatrix()` - matriz para SpriteBatch
- `WorldToScreen(worldPos)` - transformar punto mundo a pantalla
- `ScreenToWorld(screenPos)` - transformar punto pantalla a mundo

### **Flujo Update**
```
1. context.CurrentState = this.State
2. foreach behavior activo (ordenado por Priority)
   - behavior.Update(gameTime)
   - behavior.Apply(context)
3. this.State = context.ModifiedState
4. Recalcular ViewMatrix
```

---

## 5. Conversiones de Coordenadas

### **WorldToScreen**
```
1. Aplicar ViewMatrix: world → camera space
2. Aplicar Projection: camera → normalized device
3. Aplicar Viewport: NDC → pixels pantalla
```

### **ScreenToWorld** (inversa)
```
1. Viewport inverso: pixels → NDC
2. Projection inverso: NDC → camera space  
3. ViewMatrix inversa: camera → world
```

### **Consideraciones**
- `Origin` afecta dónde es el "centro" de rotación/zoom
- Zoom > 1 = acercar, < 1 = alejar
- Rotación en sentido horario positivo (convención matemática)

---

## ✅ Checklist de Validación

| Prueba | Descripción |
|--------|-------------|
| Cámara en (0,0), zoom 1 | Sprite en (100,100) debe verse en esquina |
| Zoom 2 | Sprite debe verse 2x más grande, mitad de área visible |
| Rotación 45° | Sistema de coordenadas rota correctamente |
| WorldToScreen → ScreenToWorld | Debe ser operaciones inversas (tolerancia epsilon) |
| Behaviors ordenados | Priority 0 aplica antes que Priority 10 |

---

## 🎯 Próximo Paso

Una vez validada la Fase 1, pasamos a **Fase 2: Renderizado Básico** (RenderCommand, RenderQueue, integración SpriteBatch).

¿Confirmamos este diseño o ajustamos algún detalle antes de implementar?
User: Me parece bien este diseño pero antes de continuar veamos todo el código de la fase 1
Kimi: Entendido. Aquí está el código completo de la **Fase 1: Fundamentos**.

---

## 1. MathUtils

```csharp
using System;
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public static class MathUtils
    {
        public const float PI = (float)System.Math.PI;
        public const float TwoPI = PI * 2f;
        public const float DegToRad = PI / 180f;
        public const float RadToDeg = 180f / PI;
        public const float Epsilon = 0.0001f;

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(
                Lerp(a.X, b.X, t),
                Lerp(a.Y, b.Y, t)
            );
        }

        public static float SmoothStep(float a, float b, float t)
        {
            t = Clamp01(t);
            t = t * t * (3f - 2f * t);
            return Lerp(a, b, t);
        }

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static float Clamp01(float value)
        {
            return Clamp(value, 0f, 1f);
        }

        public static float Approach(float current, float target, float maxDelta)
        {
            if (maxDelta < 0) throw new ArgumentException("maxDelta must be positive");
            
            float diff = target - current;
            if (System.Math.Abs(diff) <= maxDelta)
                return target;
            
            return current + System.Math.Sign(diff) * maxDelta;
        }

        public static Vector2 Approach(Vector2 current, Vector2 target, float maxDelta)
        {
            Vector2 diff = target - current;
            float dist = diff.Length();
            
            if (dist <= maxDelta || dist < Epsilon)
                return target;
            
            return current + (diff / dist) * maxDelta;
        }

        public static float Snap(float value, float gridSize)
        {
            if (gridSize < Epsilon) return value;
            return (float)System.Math.Floor(value / gridSize) * gridSize;
        }

        public static bool Approx(float a, float b, float epsilon = Epsilon)
        {
            return System.Math.Abs(a - b) < epsilon;
        }

        public static bool Approx(Vector2 a, Vector2 b, float epsilon = Epsilon)
        {
            return Approx(a.X, b.X, epsilon) && Approx(a.Y, b.Y, epsilon);
        }
    }
}
```

---

## 2. RectangleF

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public struct RectangleF
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2 position, Vector2 size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.X;
            Height = size.Y;
        }

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        public Vector2 Center => new Vector2(X + Width * 0.5f, Y + Height * 0.5f);
        public Vector2 Size => new Vector2(Width, Height);
        public Vector2 Position => new Vector2(X, Y);

        public bool Contains(Vector2 point)
        {
            return point.X >= Left && point.X <= Right &&
                   point.Y >= Top && point.Y <= Bottom;
        }

        public bool Contains(RectangleF other)
        {
            return Left <= other.Left && Right >= other.Right &&
                   Top <= other.Top && Bottom >= other.Bottom;
        }

        public bool Intersects(RectangleF other)
        {
            return Left < other.Right && Right > other.Left &&
                   Top < other.Bottom && Bottom > other.Top;
        }

        public RectangleF Expand(float amount)
        {
            return new RectangleF(
                X - amount,
                Y - amount,
                Width + amount * 2f,
                Height + amount * 2f
            );
        }

        public RectangleF Expand(Vector2 amount)
        {
            return new RectangleF(
                X - amount.X,
                Y - amount.Y,
                Width + amount.X * 2f,
                Height + amount.Y * 2f
            );
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(
                (int)X,
                (int)Y,
                (int)Width,
                (int)Height
            );
        }

        public static RectangleF FromPoints(Vector2 a, Vector2 b)
        {
            float minX = System.Math.Min(a.X, b.X);
            float minY = System.Math.Min(a.Y, b.Y);
            float maxX = System.Math.Max(a.X, b.X);
            float maxY = System.Math.Max(a.Y, b.Y);

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        public static RectangleF Empty => new RectangleF(0, 0, 0, 0);

        public bool IsEmpty => Width <= 0 || Height <= 0;

        public override string ToString()
        {
            return $"[{X}, {Y}, {Width}, {Height}]";
        }
    }
}
```

---

## 3. Vector2 Extensions

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public static class Vector2Extensions
    {
        public static Vector2 Perpendicular(this Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }

        public static Vector2 Rotate(this Vector2 vector, float radians)
        {
            float cos = (float)System.Math.Cos(radians);
            float sin = (float)System.Math.Sin(radians);
            
            return new Vector2(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos
            );
        }

        public static Vector2 RotateAround(this Vector2 vector, Vector2 origin, float radians)
        {
            return origin + (vector - origin).Rotate(radians);
        }

        public static Vector2 Snap(this Vector2 vector, float gridSize)
        {
            return new Vector2(
                MathUtils.Snap(vector.X, gridSize),
                MathUtils.Snap(vector.Y, gridSize)
            );
        }

        public static Vector2 Snap(this Vector2 vector, Vector2 gridSize)
        {
            return new Vector2(
                MathUtils.Snap(vector.X, gridSize.X),
                MathUtils.Snap(vector.Y, gridSize.Y)
            );
        }

        public static float Cross(this Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(
                System.Math.Abs(vector.X),
                System.Math.Abs(vector.Y)
            );
        }

        public static Vector2 Sign(this Vector2 vector)
        {
            return new Vector2(
                System.Math.Sign(vector.X),
                System.Math.Sign(vector.Y)
            );
        }

        public static Vector2 ComponentMul(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2 ComponentDiv(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }
    }
}
```

---

## 4. CameraState

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public readonly struct CameraState
    {
        public readonly Vector2 Position;
        public readonly float Rotation;
        public readonly float Zoom;
        public readonly Vector2 Origin;

        public CameraState(Vector2 position, float rotation, float zoom, Vector2 origin)
        {
            Position = position;
            Rotation = rotation;
            Zoom = zoom;
            Origin = origin;
        }

        public static CameraState Default(Vector2 origin)
        {
            return new CameraState(Vector2.Zero, 0f, 1f, origin);
        }

        public CameraState WithPosition(Vector2 position)
        {
            return new CameraState(position, Rotation, Zoom, Origin);
        }

        public CameraState WithRotation(float rotation)
        {
            return new CameraState(Position, rotation, Zoom, Origin);
        }

        public CameraState WithZoom(float zoom)
        {
            return new CameraState(Position, Rotation, zoom, Origin);
        }

        public CameraState WithOrigin(Vector2 origin)
        {
            return new CameraState(Position, Rotation, Zoom, origin);
        }

        public Matrix GetViewMatrix()
        {
            // Transformación inversa: mover mundo, no cámara
            // Orden: Translate(-Pos) → Rotate(-Rot) → Scale(1/Zoom) → Translate(Origin)
            
            Matrix translation = Matrix.CreateTranslation(-Position.X, -Position.Y, 0f);
            Matrix rotation = Matrix.CreateRotationZ(-Rotation);
            Matrix scale = Matrix.CreateScale(Zoom, Zoom, 1f);
            Matrix originOffset = Matrix.CreateTranslation(Origin.X, Origin.Y, 0f);

            return translation * rotation * scale * originOffset;
        }

        public Matrix GetInverseViewMatrix()
        {
            Matrix scale = Matrix.CreateScale(1f / Zoom, 1f / Zoom, 1f);
            Matrix rotation = Matrix.CreateRotationZ(Rotation);
            Matrix translation = Matrix.CreateTranslation(Position.X, Position.Y, 0f);
            Matrix originOffset = Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0f);

            return originOffset * scale * rotation * translation;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition, Viewport viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix projection = GetProjectionMatrix(viewport);
            
            Vector3 transformed = Vector3.Transform(new Vector3(worldPosition, 0f), view * projection);
            return new Vector2(transformed.X, transformed.Y);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition, Viewport viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix projection = GetProjectionMatrix(viewport);
            Matrix inverse = Matrix.Invert(view * projection);
            
            Vector3 transformed = Vector3.Transform(new Vector3(screenPosition, 0f), inverse);
            return new Vector2(transformed.X, transformed.Y);
        }

        private Matrix GetProjectionMatrix(Viewport viewport)
        {
            // Ortográfica: mapea a viewport
            return Matrix.CreateOrthographicOffCenter(
                0, viewport.Width,
                viewport.Height, 0,  // Y invertido para coordenadas pantalla
                0f, 1f
            );
        }

        public RectangleF GetVisibleBounds(Viewport viewport)
        {
            Vector2 topLeft = ScreenToWorld(Vector2.Zero, viewport);
            Vector2 bottomRight = ScreenToWorld(new Vector2(viewport.Width, viewport.Height), viewport);
            
            return RectangleF.FromPoints(topLeft, bottomRight);
        }

        public override string ToString()
        {
            return $"Pos:{Position}, Rot:{Rotation:F2}, Zoom:{Zoom:F2}, Origin:{Origin}";
        }
    }
}
```

---

## 5. IBehavior (Interfaz Base Unificada)

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Behavior
{
    public interface IBehavior<TContext>
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        void Update(GameTime gameTime);
        void Apply(TContext context);
        void Reset();
    }
}
```

---

## 6. CameraContext

```csharp
namespace GameEngine.Camera
{
    public struct CameraContext
    {
        public CameraState CurrentState;
        public CameraState ModifiedState;
        public GameTime Time;
        public float DeltaTime;

        public CameraContext(CameraState currentState, GameTime time)
        {
            CurrentState = currentState;
            ModifiedState = currentState;
            Time = time;
            DeltaTime = (float)time.ElapsedGameTime.TotalSeconds;
        }

        public void ApplyModification(CameraState newState)
        {
            ModifiedState = newState;
        }
    }
}
```

---

## 7. ICameraBehavior

```csharp
using GameEngine.Behavior;

namespace GameEngine.Camera
{
    public interface ICameraBehavior : IBehavior<CameraContext>
    {
    }
}
```

---

## 8. Camera (Clase Principal)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public class Camera
    {
        private readonly List<ICameraBehavior> _behaviors = new List<ICameraBehavior>();
        private CameraState _state;
        private Matrix _viewMatrix;
        private bool _matrixDirty = true;

        public string Name { get; }
        public Viewport Viewport { get; set; }
        
        public CameraState State 
        { 
            get => _state;
            private set
            {
                _state = value;
                _matrixDirty = true;
            }
        }

        public Vector2 Position => State.Position;
        public float Rotation => State.Rotation;
        public float Zoom => State.Zoom;
        public Vector2 Origin => State.Origin;

        public Camera(string name, Vector2 origin)
        {
            Name = name;
            _state = CameraState.Default(origin);
            Viewport = new Viewport(0, 0, 800, 600); // Default, sobrescribir
        }

        public Matrix GetViewMatrix()
        {
            if (_matrixDirty)
            {
                _viewMatrix = State.GetViewMatrix();
                _matrixDirty = false;
            }
            return _viewMatrix;
        }

        public void Update(GameTime gameTime)
        {
            if (_behaviors.Count == 0)
                return;

            // Ordenar por prioridad
            var activeBehaviors = _behaviors
                .Where(b => b.IsActive)
                .OrderBy(b => b.Priority);

            var context = new CameraContext(State, gameTime);

            foreach (var behavior in activeBehaviors)
            {
                behavior.Update(gameTime);
                behavior.Apply(context);
            }

            State = context.ModifiedState;
        }

        public void SetPosition(Vector2 position)
        {
            State = State.WithPosition(position);
        }

        public void SetRotation(float rotation)
        {
            State = State.WithRotation(rotation);
        }

        public void SetZoom(float zoom)
        {
            State = State.WithZoom(MathUtils.Clamp(zoom, 0.001f, 100f));
        }

        public void Move(Vector2 delta)
        {
            State = State.WithPosition(State.Position + delta);
        }

        public void Rotate(float deltaRadians)
        {
            State = State.WithRotation(State.Rotation + deltaRadians);
        }

        public void ZoomBy(float factor)
        {
            SetZoom(State.Zoom * factor);
        }

        // Behaviors
        public void AddBehavior(ICameraBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public void RemoveBehavior(ICameraBehavior behavior)
        {
            _behaviors.Remove(behavior);
        }

        public T GetBehavior<T>() where T : ICameraBehavior
        {
            return _behaviors.OfType<T>().FirstOrDefault();
        }

        public void ClearBehaviors()
        {
            _behaviors.Clear();
        }

        // Coordenadas
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return State.WorldToScreen(worldPosition, Viewport);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return State.ScreenToWorld(screenPosition, Viewport);
        }

        public RectangleF GetVisibleBounds()
        {
            return State.GetVisibleBounds(Viewport);
        }

        public bool IsVisible(RectangleF bounds)
        {
            return GetVisibleBounds().Intersects(bounds);
        }

        public bool IsVisible(Vector2 point)
        {
            return GetVisibleBounds().Contains(point);
        }

        public override string ToString()
        {
            return $"Camera '{Name}': {State}, Viewport:{Viewport.Width}x{Viewport.Height}";
        }
    }
}
```

---

## ✅ Checklist de Validación (para probar)

| Test | Código de prueba esperado |
|------|--------------------------|
| Cámara default | `var cam = new Camera("Test", new Vector2(400, 300));` |
| Set position | `cam.SetPosition(new Vector2(100, 200));` |
| Zoom | `cam.SetZoom(2f);` |
| WorldToScreen | `var screen = cam.WorldToScreen(new Vector2(0, 0));` |
| ScreenToWorld inverso | `var back = cam.ScreenToWorld(screen); // debe ser ~(0,0)` |
| Visible bounds | `var bounds = cam.GetVisibleBounds();` |
| Update con behaviors | `cam.Update(gameTime);` |

---

¿Revisamos algo o pasamos a la **Fase 2: Renderizado Básico**?
User: Cómo conoce la cámara las coordenadas del mundo? Las pregunta al director? Los comportamientos puede que tengan una duración, por tanto necesitarían un delta time
Kimi: Tienes razón. Dos puntos importantes a corregir:

---

## 1. Cámara y Coordenadas del Mundo

La cámara **no necesita conocer** los bounds del mundo directamente. Eso es responsabilidad de un comportamiento específico (`BoundsConstraintBehavior` en Fase 4).

Lo que sí necesita la cámara base:
- Su propio estado (posición, zoom, rotación)
- El viewport de pantalla
- Behaviors que la modifiquen

Los límites del mundo vendrán de:
- **Escena** (tiene el tamaño del mundo)
- **BoundsConstraintBehavior** (recibe RectangleF del mundo y aplica a la cámara)

---

## 2. Behaviors con Duración

Necesitamos añadir soporte para behaviors temporales:

```csharp
public interface IBehavior<TContext>
{
    bool IsActive { get; set; }
    int Priority { get; }
    
    // Para behaviors con duración
    float? Duration { get; }      // null = infinito
    float ElapsedTime { get; }    // tiempo acumulado
    bool IsComplete { get; }      // true cuando termina
    
    void Update(GameTime gameTime);
    void Apply(TContext context);
    void Reset();
}
```

### **Behavior Base Abstracto**

```csharp
public abstract class BehaviorBase<TContext> : IBehavior<TContext>
{
    public bool IsActive { get; set; } = true;
    public abstract int Priority { get; }
    
    public float? Duration { get; protected set; } = null;
    public float ElapsedTime { get; private set; } = 0f;
    public bool IsComplete { get; protected set; } = false;
    
    public virtual void Update(GameTime gameTime)
    {
        if (!IsActive || IsComplete) return;
        
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        ElapsedTime += delta;
        
        if (Duration.HasValue && ElapsedTime >= Duration.Value)
        {
            IsComplete = true;
            OnComplete();
        }
        
        OnUpdate(delta);
    }
    
    protected abstract void OnUpdate(float deltaTime);
    public abstract void Apply(TContext context);
    
    public virtual void Reset()
    {
        ElapsedTime = 0f;
        IsComplete = false;
        IsActive = true;
    }
    
    protected virtual void OnComplete() { }
}
```

### **Ejemplo: ShakeBehavior (temporal)**

```csharp
public class ShakeBehavior : BehaviorBase<CameraContext>, ICameraBehavior
{
    public override int Priority => 100;
    
    public float Intensity { get; set; }
    public float Decay { get; set; } // reducción por segundo
    
    private float _currentTrauma;
    private Random _random = new Random();
    
    public ShakeBehavior(float duration, float intensity)
    {
        Duration = duration;
        Intensity = intensity;
        Decay = 1f / duration; // decae lineal
    }
    
    protected override void OnUpdate(float deltaTime)
    {
        _currentTrauma = MathUtils.Clamp01(1f - (ElapsedTime / Duration.Value));
    }
    
    public override void Apply(CameraContext context)
    {
        if (_currentTrauma <= 0) return;
        
        float shake = _currentTrauma * _currentTrauma * Intensity; // cuadrado para curva natural
        
        float offsetX = ((float)_random.NextDouble() * 2f - 1f) * shake;
        float offsetY = ((float)_random.NextDouble() * 2f - 1f) * shake;
        
        var newPos = context.ModifiedState.Position + new Vector2(offsetX, offsetY);
        context.ApplyModification(context.ModifiedState.WithPosition(newPos));
    }
}
```

---

## 3. Modificación a Camera.Update()

```csharp
public void Update(GameTime gameTime)
{
    // Actualizar y limpiar behaviors completados
    for (int i = _behaviors.Count - 1; i >= 0; i--)
    {
        var behavior = _behaviors[i];
        
        if (!behavior.IsActive) continue;
        
        behavior.Update(gameTime);
        
        // Auto-remove behaviors temporales completados
        if (behavior.IsComplete && behavior.Duration.HasValue)
        {
            _behaviors.RemoveAt(i);
        }
    }

    // Aplicar behaviors activos
    var activeBehaviors = _behaviors
        .Where(b => b.IsActive && !b.IsComplete)
        .OrderBy(b => b.Priority);

    var context = new CameraContext(State, gameTime);

    foreach (var behavior in activeBehaviors)
    {
        behavior.Apply(context);
    }

    State = context.ModifiedState;
}
```

---

## 4. Método Conveniente para Behaviors Temporales

```csharp
public void Shake(float duration, float intensity)
{
    AddBehavior(new ShakeBehavior(duration, intensity));
}
```

Uso: `camera.Shake(0.5f, 10f);` // medio segundo, 10 pixels de intensidad

---

## ✅ Resumen de Cambios

| Aspecto | Antes | Después |
|--------|-------|---------|
| Límites mundo | Cámara no los conoce | Behavior `BoundsConstraint` los recibe |
| Duración behaviors | No existía | `Duration`, `ElapsedTime`, `IsComplete` |
| Update | Solo aplicaba | Actualiza tiempo, limpia completados |
| Shake | No existía | Ejemplo de behavior temporal |

---

¿Aplicamos estos cambios al código de Fase 1 o pasamos directamente a Fase 2 con estas correcciones incorporadas?
User: No estoy de acuerdo, la cámara debe saber las coordenadas del mundo porque algunos actores no se modificarán a menos que estén dentro del foco de la cámara
Kimi: Tienes razón. La cámara necesita conocer los bounds del mundo para el culling de actores.

Aquí está el diseño corregido:

---

## Responsabilidad de la Cámara

La cámara **sí necesita** `WorldBounds` para:

- **Culling**: Decidir qué actores están visibles
- **Clamping**: Behaviors que restrinjan posición al mundo
- **Optimización**: No generar RenderCommands fuera de pantalla

---

## Modificaciones a Camera

```csharp
public class Camera
{
    // Nuevo: Límites del mundo
    public RectangleF? WorldBounds { get; set; }
    
    // Visible bounds combinados con world bounds
    public RectangleF GetVisibleBounds()
    {
        var visible = State.GetVisibleBounds(Viewport);
        
        if (WorldBounds.HasValue)
        {
            // Intersección: solo lo que ve la cámara Y existe en el mundo
            return Intersect(visible, WorldBounds.Value);
        }
        
        return visible;
    }
    
    // Culling eficiente
    public bool IsInView(RectangleF bounds)
    {
        return GetVisibleBounds().Intersects(bounds);
    }
    
    public bool IsInView(Vector2 point)
    {
        return GetVisibleBounds().Contains(point);
    }
}
```

---

## Flujo de Culling

```
Actor.CollectDraw(camera)
    ├── if (!camera.IsInView(actor.Bounds)) return;
    └── Generar RenderCommand
```

Esto evita que actores fuera de pantalla (y fuera del mundo) generen comandos innecesarios.

---

## Origen de WorldBounds

| Fuente | Cuándo |
|--------|--------|
| **Scene** | Al crear cámara, le pasa tamaño del nivel |
| **Tilemap** | Después de cargar mapa, actualiza bounds |
| **Procedural** | Mundo que crece, bounds dinámicos |

---

## Ejemplo de Uso

```csharp
// Scene crea cámara
var camera = new Camera("Main", screenCenter);
camera.WorldBounds = new RectangleF(0, 0, 2000, 1500); // Tamaño del nivel

// Actor decide si dibujar
void CollectDraw(Camera camera)
{
    if (!camera.IsInView(this.Bounds)) 
        return; // Culling temprano
    
    camera.Submit(new RenderCommand(...));
}
```

---

## Resumen

| Aspecto | Corrección |
|--------|-----------|
| Cámara conoce mundo | `WorldBounds: RectangleF?` |
| Culling | `IsInView()` usa intersección visible + mundo |
| Origen | Scene/Level establece bounds al inicializar |

¿Confirmamos esta corrección en el código de Fase 1?
User: En realidad, la cámara puede obtener las coordenadas del mundo del director al que está asociado que las obtendrá de la escena. Lo que tiene que tener la cámara es uno o varios métodos que utilizan los actores para saber si están en el foco de alguna de las cámaras asociadas a la capa
Kimi: Entendido. La cámara consulta al director, el director a la escena.

---

## Jerarquía de Coordenadas

```
Actor → Capa → ¿Estoy en foco? → Director → Escena → WorldBounds
                    ↑
                Cámara (no guarda bounds, pregunta)
```

---

## Modificaciones

### **ICameraDirector** (interfaz que la cámara conoce)

```csharp
public interface ICameraDirector
{
    RectangleF GetWorldBounds();
    // Futuro: múltiples cámaras, culling compartido, etc.
}
```

### **Camera**

```csharp
public class Camera
{
    public ICameraDirector Director { get; set; } // Inyectado al añadir a director
    
    // Métodos para actores/capas
    public bool IsInView(RectangleF bounds)
    {
        if (Director == null) return true; // Sin director, todo visible
        
        var visible = State.GetVisibleBounds(Viewport);
        var world = Director.GetWorldBounds();
        
        // Intersección: visible + mundo
        return Intersect(visible, world).Intersects(bounds);
    }
    
    public bool IsInView(Vector2 point)
    {
        if (Director == null) return true;
        
        var visible = State.GetVisibleBounds(Viewport);
        var world = Director.GetWorldBounds();
        
        return Intersect(visible, world).Contains(point);
    }
    
    // Visible crudo (para debug, efectos, etc.)
    public RectangleF GetVisibleBounds()
    {
        return State.GetVisibleBounds(Viewport);
    }
}
```

### **Actor / Capa**

```csharp
// Actor pregunta a través de la capa
void CollectDraw(Layer layer)
{
    foreach (var camera in layer.GetCameras())
    {
        if (!camera.IsInView(this.Bounds))
            continue; // Culling por cámara
        
        camera.Submit(new RenderCommand(...));
    }
}
```

---

## Flujo de Datos

```
Actor.Bounds ──┐
               ├──→ camera.IsInView() ──→ Director.GetWorldBounds()
Capa.Cameras ──┘                              ↑
                                              │
                                         Escena.WorldBounds
```

---

## Ventajas

| Aspecto | Beneficio |
|--------|-----------|
| **Single source of truth** | Bounds solo en escena |
| **Cámara desacoplada** | No necesita inicialización compleja |
| **Testeable** | Director mock con bounds fijos |
| **Múltiples escenas** | Cámara reusable, bounds cambian con director |

---

## Resumen de Responsabilidades

| Componente | Sabe | Hace |
|-----------|------|------|
| **Escena** | WorldBounds | Pasa a director |
| **Director** | Referencia escena | Responde `GetWorldBounds()` |
| **Cámara** | Director (opcional) | `IsInView()` consultando director |
| **Actor** | Su bounds | Pregunta a cámara antes de submit |

¿Aplicamos este diseño?
User: No es necesario el interface. El constructor de la cámara lleva un director. El constructor del director lleva la escena. Los métodos de la cámara consultan las coordenadas de mundo como director.scene.worldbounds
Kimi: Entendido. Referencias directas, sin interfaces.

---

## Jerarquía de Referencias

```
Camera → Director → Scene → WorldBounds
```

---

## Constructores

```csharp
public class Camera
{
    public CameraDirector Director { get; }
    
    public Camera(string name, Vector2 origin, CameraDirector director)
    {
        Name = name;
        Director = director ?? throw new ArgumentNullException(nameof(director));
        // ... resto
    }
    
    // Métodos de culling
    public bool IsInView(RectangleF bounds)
    {
        var visible = State.GetVisibleBounds(Viewport);
        var world = Director.Scene.WorldBounds;
        
        return Intersect(visible, world).Intersects(bounds);
    }
    
    public bool IsInView(Vector2 point)
    {
        var visible = State.GetVisibleBounds(Viewport);
        var world = Director.Scene.WorldBounds;
        
        return Intersect(visible, world).Contains(point);
    }
}
```

```csharp
public class CameraDirector
{
    public Scene Scene { get; }
    public List<Camera> Cameras { get; } = new();
    
    public CameraDirector(Scene scene)
    {
        Scene = scene ?? throw new ArgumentNullException(nameof(scene));
    }
    
    public Camera CreateCamera(string name, Vector2 origin)
    {
        var camera = new Camera(name, origin, this);
        Cameras.Add(camera);
        return camera;
    }
}
```

---

## Flujo

```
Actor.CollectDraw()
    └── foreach camera in layer.Cameras
            └── if (camera.IsInView(this.Bounds))
                    └── camera.Submit(command)
                            └── camera.Director.Scene.WorldBounds
```

---

## Cadena de Acceso

| Código | Significado |
|--------|-------------|
| `camera.Director` | El director que me contiene |
| `camera.Director.Scene` | La escena del director |
| `camera.Director.Scene.WorldBounds` | Límites del mundo |

---

## Nota de Diseño

- **Null checks en constructores**: Fallo temprano si falta referencia
- **Sin setters públicos**: Referencias inmutables después de construcción
- **Cámara sin director no existe**: Siempre hay contexto de escena

¿Confirmamos esta cadena de referencias para Fase 1?
User: No utilizar null check en constructor. Repite el código de la fase 1 completo con las consideraciones que tenemos hasta ahora
Kimi: Aquí está el código completo de la **Fase 1** con todas las consideraciones aplicadas.

---

## 1. MathUtils

```csharp
using System;
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public static class MathUtils
    {
        public const float PI = (float)System.Math.PI;
        public const float TwoPI = PI * 2f;
        public const float DegToRad = PI / 180f;
        public const float RadToDeg = 180f / PI;
        public const float Epsilon = 0.0001f;

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(
                Lerp(a.X, b.X, t),
                Lerp(a.Y, b.Y, t)
            );
        }

        public static float SmoothStep(float a, float b, float t)
        {
            t = Clamp01(t);
            t = t * t * (3f - 2f * t);
            return Lerp(a, b, t);
        }

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static float Clamp01(float value)
        {
            return Clamp(value, 0f, 1f);
        }

        public static float Approach(float current, float target, float maxDelta)
        {
            float diff = target - current;
            if (System.Math.Abs(diff) <= maxDelta)
                return target;
            
            return current + System.Math.Sign(diff) * maxDelta;
        }

        public static Vector2 Approach(Vector2 current, Vector2 target, float maxDelta)
        {
            Vector2 diff = target - current;
            float dist = diff.Length();
            
            if (dist <= maxDelta || dist < Epsilon)
                return target;
            
            return current + (diff / dist) * maxDelta;
        }

        public static float Snap(float value, float gridSize)
        {
            if (gridSize < Epsilon) return value;
            return (float)System.Math.Floor(value / gridSize) * gridSize;
        }

        public static bool Approx(float a, float b, float epsilon = Epsilon)
        {
            return System.Math.Abs(a - b) < epsilon;
        }

        public static bool Approx(Vector2 a, Vector2 b, float epsilon = Epsilon)
        {
            return Approx(a.X, b.X, epsilon) && Approx(a.Y, b.Y, epsilon);
        }
    }
}
```

---

## 2. RectangleF

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public struct RectangleF
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2 position, Vector2 size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.X;
            Height = size.Y;
        }

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        public Vector2 Center => new Vector2(X + Width * 0.5f, Y + Height * 0.5f);
        public Vector2 Size => new Vector2(Width, Height);
        public Vector2 Position => new Vector2(X, Y);

        public bool Contains(Vector2 point)
        {
            return point.X >= Left && point.X <= Right &&
                   point.Y >= Top && point.Y <= Bottom;
        }

        public bool Contains(RectangleF other)
        {
            return Left <= other.Left && Right >= other.Right &&
                   Top <= other.Top && Bottom >= other.Bottom;
        }

        public bool Intersects(RectangleF other)
        {
            return Left < other.Right && Right > other.Left &&
                   Top < other.Bottom && Bottom > other.Top;
        }

        public RectangleF Expand(float amount)
        {
            return new RectangleF(
                X - amount,
                Y - amount,
                Width + amount * 2f,
                Height + amount * 2f
            );
        }

        public RectangleF Expand(Vector2 amount)
        {
            return new RectangleF(
                X - amount.X,
                Y - amount.Y,
                Width + amount.X * 2f,
                Height + amount.Y * 2f
            );
        }

        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            float left = System.Math.Max(a.Left, b.Left);
            float top = System.Math.Max(a.Top, b.Top);
            float right = System.Math.Min(a.Right, b.Right);
            float bottom = System.Math.Min(a.Bottom, b.Bottom);

            if (right > left && bottom > top)
            {
                return new RectangleF(left, top, right - left, bottom - top);
            }

            return Empty;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(
                (int)X,
                (int)Y,
                (int)Width,
                (int)Height
            );
        }

        public static RectangleF FromPoints(Vector2 a, Vector2 b)
        {
            float minX = System.Math.Min(a.X, b.X);
            float minY = System.Math.Min(a.Y, b.Y);
            float maxX = System.Math.Max(a.X, b.X);
            float maxY = System.Math.Max(a.Y, b.Y);

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        public static RectangleF Empty => new RectangleF(0, 0, 0, 0);

        public bool IsEmpty => Width <= 0 || Height <= 0;

        public override string ToString()
        {
            return $"[{X}, {Y}, {Width}, {Height}]";
        }
    }
}
```

---

## 3. Vector2Extensions

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public static class Vector2Extensions
    {
        public static Vector2 Perpendicular(this Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }

        public static Vector2 Rotate(this Vector2 vector, float radians)
        {
            float cos = (float)System.Math.Cos(radians);
            float sin = (float)System.Math.Sin(radians);
            
            return new Vector2(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos
            );
        }

        public static Vector2 RotateAround(this Vector2 vector, Vector2 origin, float radians)
        {
            return origin + (vector - origin).Rotate(radians);
        }

        public static Vector2 Snap(this Vector2 vector, float gridSize)
        {
            return new Vector2(
                MathUtils.Snap(vector.X, gridSize),
                MathUtils.Snap(vector.Y, gridSize)
            );
        }

        public static Vector2 Snap(this Vector2 vector, Vector2 gridSize)
        {
            return new Vector2(
                MathUtils.Snap(vector.X, gridSize.X),
                MathUtils.Snap(vector.Y, gridSize.Y)
            );
        }

        public static float Cross(this Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(
                System.Math.Abs(vector.X),
                System.Math.Abs(vector.Y)
            );
        }

        public static Vector2 Sign(this Vector2 vector)
        {
            return new Vector2(
                System.Math.Sign(vector.X),
                System.Math.Sign(vector.Y)
            );
        }

        public static Vector2 ComponentMul(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2 ComponentDiv(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }
    }
}
```

---

## 4. CameraState

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public readonly struct CameraState
    {
        public readonly Vector2 Position;
        public readonly float Rotation;
        public readonly float Zoom;
        public readonly Vector2 Origin;

        public CameraState(Vector2 position, float rotation, float zoom, Vector2 origin)
        {
            Position = position;
            Rotation = rotation;
            Zoom = zoom;
            Origin = origin;
        }

        public static CameraState Default(Vector2 origin)
        {
            return new CameraState(Vector2.Zero, 0f, 1f, origin);
        }

        public CameraState WithPosition(Vector2 position)
        {
            return new CameraState(position, Rotation, Zoom, Origin);
        }

        public CameraState WithRotation(float rotation)
        {
            return new CameraState(Position, rotation, Zoom, Origin);
        }

        public CameraState WithZoom(float zoom)
        {
            return new CameraState(Position, Rotation, zoom, Origin);
        }

        public CameraState WithOrigin(Vector2 origin)
        {
            return new CameraState(Position, Rotation, Zoom, origin);
        }

        public Matrix GetViewMatrix()
        {
            Matrix translation = Matrix.CreateTranslation(-Position.X, -Position.Y, 0f);
            Matrix rotation = Matrix.CreateRotationZ(-Rotation);
            Matrix scale = Matrix.CreateScale(Zoom, Zoom, 1f);
            Matrix originOffset = Matrix.CreateTranslation(Origin.X, Origin.Y, 0f);

            return translation * rotation * scale * originOffset;
        }

        public Matrix GetInverseViewMatrix()
        {
            Matrix scale = Matrix.CreateScale(1f / Zoom, 1f / Zoom, 1f);
            Matrix rotation = Matrix.CreateRotationZ(Rotation);
            Matrix translation = Matrix.CreateTranslation(Position.X, Position.Y, 0f);
            Matrix originOffset = Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0f);

            return originOffset * scale * rotation * translation;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition, Viewport viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix projection = GetProjectionMatrix(viewport);
            
            Vector3 transformed = Vector3.Transform(new Vector3(worldPosition, 0f), view * projection);
            return new Vector2(transformed.X, transformed.Y);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition, Viewport viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix projection = GetProjectionMatrix(viewport);
            Matrix inverse = Matrix.Invert(view * projection);
            
            Vector3 transformed = Vector3.Transform(new Vector3(screenPosition, 0f), inverse);
            return new Vector2(transformed.X, transformed.Y);
        }

        private Matrix GetProjectionMatrix(Viewport viewport)
        {
            return Matrix.CreateOrthographicOffCenter(
                0, viewport.Width,
                viewport.Height, 0,
                0f, 1f
            );
        }

        public RectangleF GetVisibleBounds(Viewport viewport)
        {
            Vector2 topLeft = ScreenToWorld(Vector2.Zero, viewport);
            Vector2 bottomRight = ScreenToWorld(new Vector2(viewport.Width, viewport.Height), viewport);
            
            return RectangleF.FromPoints(topLeft, bottomRight);
        }

        public override string ToString()
        {
            return $"Pos:{Position}, Rot:{Rotation:F2}, Zoom:{Zoom:F2}, Origin:{Origin}";
        }
    }
}
```

---

## 5. IBehavior (Interfaz Base)

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Behavior
{
    public interface IBehavior<TContext>
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        float? Duration { get; }
        float ElapsedTime { get; }
        bool IsComplete { get; }
        
        void Update(GameTime gameTime);
        void Apply(TContext context);
        void Reset();
    }
}
```

---

## 6. BehaviorBase (Clase Abstracta)

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Behavior
{
    public abstract class BehaviorBase<TContext> : IBehavior<TContext>
    {
        public bool IsActive { get; set; } = true;
        public abstract int Priority { get; }
        
        public float? Duration { get; protected set; }
        public float ElapsedTime { get; private set; }
        public bool IsComplete { get; protected set; }

        public void Update(GameTime gameTime)
        {
            if (!IsActive || IsComplete)
                return;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ElapsedTime += delta;

            if (Duration.HasValue && ElapsedTime >= Duration.Value)
            {
                IsComplete = true;
                OnComplete();
            }

            OnUpdate(delta);
        }

        protected abstract void OnUpdate(float deltaTime);
        public abstract void Apply(TContext context);

        public virtual void Reset()
        {
            ElapsedTime = 0f;
            IsComplete = false;
            IsActive = true;
        }

        protected virtual void OnComplete() { }
    }
}
```

---

## 7. CameraContext

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
{
    public struct CameraContext
    {
        public CameraState CurrentState;
        public CameraState ModifiedState;
        public GameTime Time;
        public float DeltaTime;

        public CameraContext(CameraState currentState, GameTime time)
        {
            CurrentState = currentState;
            ModifiedState = currentState;
            Time = time;
            DeltaTime = (float)time.ElapsedGameTime.TotalSeconds;
        }

        public void ApplyModification(CameraState newState)
        {
            ModifiedState = newState;
        }
    }
}
```

---

## 8. ICameraBehavior

```csharp
using GameEngine.Behavior;

namespace GameEngine.Camera
{
    public interface ICameraBehavior : IBehavior<CameraContext>
    {
    }
}
```

---

## 9. Scene (Placeholder para referencia)

```csharp
using GameEngine.Math;

namespace GameEngine.Scene
{
    public class Scene
    {
        public RectangleF WorldBounds { get; set; }
        
        public Scene(RectangleF worldBounds)
        {
            WorldBounds = worldBounds;
        }
    }
}
```

---

## 10. CameraDirector

```csharp
using System.Collections.Generic;
using GameEngine.Scene;
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
{
    public class CameraDirector
    {
        public Scene.Scene Scene { get; }
        public List<Camera> Cameras { get; } = new List<Camera>();

        public CameraDirector(Scene.Scene scene)
        {
            Scene = scene;
        }

        public Camera CreateCamera(string name, Vector2 origin)
        {
            var camera = new Camera(name, origin, this);
            Cameras.Add(camera);
            return camera;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var camera in Cameras)
            {
                camera.Update(gameTime);
            }
        }
    }
}
```

---

## 11. Camera (Clase Principal)

```csharp
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public class Camera
    {
        private readonly List<ICameraBehavior> _behaviors = new List<ICameraBehavior>();
        private CameraState _state;
        private Matrix _viewMatrix;
        private bool _matrixDirty = true;

        public string Name { get; }
        public CameraDirector Director { get; }
        public Viewport Viewport { get; set; }
        
        public CameraState State 
        { 
            get => _state;
            private set
            {
                _state = value;
                _matrixDirty = true;
            }
        }

        public Vector2 Position => State.Position;
        public float Rotation => State.Rotation;
        public float Zoom => State.Zoom;
        public Vector2 Origin => State.Origin;

        public Camera(string name, Vector2 origin, CameraDirector director)
        {
            Name = name;
            Director = director;
            _state = CameraState.Default(origin);
            Viewport = new Viewport(0, 0, 800, 600);
        }

        public Matrix GetViewMatrix()
        {
            if (_matrixDirty)
            {
                _viewMatrix = State.GetViewMatrix();
                _matrixDirty = false;
            }
            return _viewMatrix;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                var behavior = _behaviors[i];
                
                if (!behavior.IsActive)
                    continue;
                
                behavior.Update(gameTime);
                
                if (behavior.IsComplete && behavior.Duration.HasValue)
                {
                    _behaviors.RemoveAt(i);
                }
            }

            var activeBehaviors = _behaviors
                .Where(b => b.IsActive && !b.IsComplete)
                .OrderBy(b => b.Priority);

            var context = new CameraContext(State, gameTime);

            foreach (var behavior in activeBehaviors)
            {
                behavior.Apply(context);
            }

            State = context.ModifiedState;
        }

        public void SetPosition(Vector2 position)
        {
            State = State.WithPosition(position);
        }

        public void SetRotation(float rotation)
        {
            State = State.WithRotation(rotation);
        }

        public void SetZoom(float zoom)
        {
            State = State.WithZoom(MathUtils.Clamp(zoom, 0.001f, 100f));
        }

        public void Move(Vector2 delta)
        {
            State = State.WithPosition(State.Position + delta);
        }

        public void Rotate(float deltaRadians)
        {
            State = State.WithRotation(State.Rotation + deltaRadians);
        }

        public void ZoomBy(float factor)
        {
            SetZoom(State.Zoom * factor);
        }

        public void AddBehavior(ICameraBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public void RemoveBehavior(ICameraBehavior behavior)
        {
            _behaviors.Remove(behavior);
        }

        public T GetBehavior<T>() where T : ICameraBehavior
        {
            return _behaviors.OfType<T>().FirstOrDefault();
        }

        public void ClearBehaviors()
        {
            _behaviors.Clear();
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return State.WorldToScreen(worldPosition, Viewport);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return State.ScreenToWorld(screenPosition, Viewport);
        }

        public RectangleF GetVisibleBounds()
        {
            return State.GetVisibleBounds(Viewport);
        }

        public RectangleF GetCullingBounds()
        {
            var visible = GetVisibleBounds();
            var world = Director.Scene.WorldBounds;
            return RectangleF.Intersect(visible, world);
        }

        public bool IsInView(RectangleF bounds)
        {
            return GetCullingBounds().Intersects(bounds);
        }

        public bool IsInView(Vector2 point)
        {
            return GetCullingBounds().Contains(point);
        }

        public override string ToString()
        {
            return $"Camera '{Name}': {State}, Viewport:{Viewport.Width}x{Viewport.Height}";
        }
    }
}
```

---

## Resumen de Cambios Clave

| Aspecto | Implementación |
|--------|---------------|
| Referencias | `Camera → Director → Scene → WorldBounds` |
| Sin null checks | Constructores con asignación directa |
| Culling | `GetCullingBounds()` = intersección visible + mundo |
| Behaviors temporales | `Duration`, `ElapsedTime`, `IsComplete`, auto-remove |
| Cleanup | `OnUpdate(delta)` para lógica, `Apply()` para modificación |

¿Pasamos a **Fase 2**?
User: Por qué se crea un nuevo contexto antes de aplicar los comportamientos? No sería mejor pasarle el delta time en el método de update del comportamiento?
Kimi: Tienes razón. El contexto se crea innecesariamente antes de saber si hay behaviors activos. Además, el delta time ya está disponible en `Update`.

Aquí está el diseño corregido:

---

## Problema Actual

```csharp
// Código problemático
var context = new CameraContext(State, gameTime); // Siempre se crea

foreach (var behavior in activeBehaviors)
{
    behavior.Update(gameTime);  // Ya tiene delta time aquí
    behavior.Apply(context);    // Pero luego necesita contexto
}
```

**Issues:**
- Contexto creado incluso sin behaviors
- Delta time duplicado (en `Update` y en `context.DeltaTime`)
- Behaviors no pueden modificar estado incrementalmente

---

## Solución: Behaviors Acumulativos

Cada behavior recibe estado actual y devuelve estado modificado:

```csharp
public interface IBehavior<TState>
{
    bool IsActive { get; set; }
    int Priority { get; }
    
    float? Duration { get; }
    float ElapsedTime { get; }
    bool IsComplete { get; }
    
    void Update(float deltaTime);  // Solo lógica interna
    TState Apply(TState current);  // Recibe y devuelve estado
    void Reset();
}
```

---

## Flujo Corregido

```csharp
public void Update(GameTime gameTime)
{
    // Update behaviors (lógica interna)
    float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
    for (int i = _behaviors.Count - 1; i >= 0; i--)
    {
        var behavior = _behaviors[i];
        
        if (!behavior.IsActive)
            continue;
        
        behavior.Update(delta);
        
        if (behavior.IsComplete && behavior.Duration.HasValue)
        {
            _behaviors.RemoveAt(i);
        }
    }

    // Aplicar modificaciones en cadena
    var activeBehaviors = _behaviors
        .Where(b => b.IsActive && !b.IsComplete)
        .OrderBy(b => b.Priority);

    CameraState current = State;
    
    foreach (var behavior in activeBehaviors)
    {
        current = behavior.Apply(current);  // Cada uno recibe resultado anterior
    }

    State = current;
}
```

---

## Ventajas

| Aspecto | Antes | Después |
|--------|-------|---------|
| Contexto | Siempre creado | No existe |
| Delta time | En `Update` + `context` | Solo en `Update` |
| Acumulación | Uno modifica, otros ignoran | Cada uno ve cambios previos |
| Simplicidad | Struct extra | Solo `TState` |

---

## Ejemplo: ShakeBehavior

```csharp
public class ShakeBehavior : BehaviorBase<CameraState>, ICameraBehavior
{
    public override int Priority => 100;
    
    private float _currentTrauma;
    private Random _random = new Random();
    
    public ShakeBehavior(float duration, float intensity)
    {
        Duration = duration;
    }
    
    protected override void OnUpdate(float deltaTime)
    {
        _currentTrauma = 1f - (ElapsedTime / Duration.Value);
        _currentTrauma = MathUtils.Clamp01(_currentTrauma);
        _currentTrauma *= _currentTrauma; // Curva cuadrática
    }
    
    public override CameraState Apply(CameraState current)
    {
        float shake = _currentTrauma * Intensity;
        
        float offsetX = ((float)_random.NextDouble() * 2f - 1f) * shake;
        float offsetY = ((float)_random.NextDouble() * 2f - 1f) * shake;
        
        return current.WithPosition(current.Position + new Vector2(offsetX, offsetY));
    }
}
```

---

## Código Corregido de Fase 1

### IBehavior

```csharp
public interface IBehavior<TState>
{
    bool IsActive { get; set; }
    int Priority { get; }
    
    float? Duration { get; }
    float ElapsedTime { get; }
    bool IsComplete { get; }
    
    void Update(float deltaTime);
    TState Apply(TState current);
    void Reset();
}
```

### BehaviorBase

```csharp
public abstract class BehaviorBase<TState> : IBehavior<TState>
{
    public bool IsActive { get; set; } = true;
    public abstract int Priority { get; }
    
    public float? Duration { get; protected set; }
    public float ElapsedTime { get; private set; }
    public bool IsComplete { get; protected set; }

    public void Update(float deltaTime)
    {
        if (!IsActive || IsComplete)
            return;

        ElapsedTime += deltaTime;

        if (Duration.HasValue && ElapsedTime >= Duration.Value)
        {
            IsComplete = true;
            OnComplete();
        }

        OnUpdate(deltaTime);
    }

    protected abstract void OnUpdate(float deltaTime);
    public abstract TState Apply(TState current);

    public virtual void Reset()
    {
        ElapsedTime = 0f;
        IsComplete = false;
        IsActive = true;
    }

    protected virtual void OnComplete() { }
}
```

### ICameraBehavior

```csharp
public interface ICameraBehavior : IBehavior<CameraState>
{
}
```

### Camera.Update (corregido)

```csharp
public void Update(GameTime gameTime)
{
    float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
    // Update lógica interna
    for (int i = _behaviors.Count - 1; i >= 0; i--)
    {
        var behavior = _behaviors[i];
        
        if (!behavior.IsActive)
            continue;
        
        behavior.Update(delta);
        
        if (behavior.IsComplete && behavior.Duration.HasValue)
        {
            _behaviors.RemoveAt(i);
        }
    }

    // Aplicar en cadena
    var activeBehaviors = _behaviors
        .Where(b => b.IsActive && !b.IsComplete)
        .OrderBy(b => b.Priority);

    CameraState current = State;
    
    foreach (var behavior in activeBehaviors)
    {
        current = behavior.Apply(current);
    }

    State = current;
}
```

---

## Eliminados

- `CameraContext` (ya no necesario)
- `CameraContext` en `Camera.Update`
- Duplicación de delta time

¿Confirmamos esta simplificación?
User: Has vuelto a crear un contexto antes de aplicar los comportamientos. Vamos a hacerlo de nuevo. Supongo que te sería más fácil si en el constructor del comportamiento le pasas el contexto de la cámara y en el update del comportamiento le pasas el delta time
Kimi: Entendido. El comportamiento recibe el contexto de la cámara en el constructor y solo recibe delta time en Update.

---

## Diseño Corregido

```csharp
public interface ICameraBehavior
{
    bool IsActive { get; set; }
    int Priority { get; }
    
    float? Duration { get; }
    float ElapsedTime { get; }
    bool IsComplete { get; }
    
    void Update(float deltaTime);
    void Apply();  // Usa contexto del constructor
    void Reset();
}
```

---

## Implementación Base

```csharp
public abstract class CameraBehaviorBase : ICameraBehavior
{
    protected readonly Camera Camera;
    
    public bool IsActive { get; set; } = true;
    public abstract int Priority { get; }
    
    public float? Duration { get; protected set; }
    public float ElapsedTime { get; private set; }
    public bool IsComplete { get; protected set; }

    protected CameraBehaviorBase(Camera camera)
    {
        Camera = camera;
    }

    public void Update(float deltaTime)
    {
        if (!IsActive || IsComplete)
            return;

        ElapsedTime += deltaTime;

        if (Duration.HasValue && ElapsedTime >= Duration.Value)
        {
            IsComplete = true;
            OnComplete();
        }

        OnUpdate(deltaTime);
    }

    protected abstract void OnUpdate(float deltaTime);
    public abstract void Apply();

    public virtual void Reset()
    {
        ElapsedTime = 0f;
        IsComplete = false;
        IsActive = true;
    }

    protected virtual void OnComplete() { }
}
```

---

## Camera.Update Simplificado

```csharp
public void Update(GameTime gameTime)
{
    float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
    // Update lógica interna y limpiar completados
    for (int i = _behaviors.Count - 1; i >= 0; i--)
    {
        var behavior = _behaviors[i];
        
        if (!behavior.IsActive)
            continue;
        
        behavior.Update(delta);
        
        if (behavior.IsComplete && behavior.Duration.HasValue)
        {
            _behaviors.RemoveAt(i);
        }
    }

    // Aplicar modificaciones
    var activeBehaviors = _behaviors
        .Where(b => b.IsActive && !b.IsComplete)
        .OrderBy(b => b.Priority);

    foreach (var behavior in activeBehaviors)
    {
        behavior.Apply();
    }
}
```

---

## Ejemplo: ShakeBehavior

```csharp
public class ShakeBehavior : CameraBehaviorBase
{
    public override int Priority => 100;
    
    private float _currentTrauma;
    private Random _random = new Random();
    private float _intensity;
    
    public ShakeBehavior(Camera camera, float duration, float intensity) 
        : base(camera)
    {
        Duration = duration;
        _intensity = intensity;
    }
    
    protected override void OnUpdate(float deltaTime)
    {
        _currentTrauma = 1f - (ElapsedTime / Duration.Value);
        _currentTrauma = MathUtils.Clamp01(_currentTrauma);
        _currentTrauma *= _currentTrauma;
    }
    
    public override void Apply()
    {
        float shake = _currentTrauma * _intensity;
        
        float offsetX = ((float)_random.NextDouble() * 2f - 1f) * shake;
        float offsetY = ((float)_random.NextDouble() * 2f - 1f) * shake;
        
        Camera.Move(new Vector2(offsetX, offsetY));
    }
}
```

---

## Cambios en Código de Fase 1

| Archivo | Cambio |
|---------|--------|
| `ICameraBehavior` | Eliminar genérico, `Apply()` sin parámetros |
| `CameraBehaviorBase` | Constructor recibe `Camera` |
| `Camera.Update` | Sin `CameraState current`, behaviors llaman `Apply()` directo |
| `CameraContext` | **Eliminado** |

---

## Flujo Final

```
Constructor:  ShakeBehavior(camera, 0.5f, 10f)
                    ↓
Update(delta):     behavior.Update(delta)  →  lógica interna
                    ↓
Apply():           Camera.Move(offset)     →  modifica cámara directo
```

¿Confirmamos este diseño para reescribir Fase 1?
User: No tiene sentido que los comportamientos tengan update y apply. Sólo necesitamos el update
Kimi: Entendido. Un solo método `Update` que recibe delta time y modifica la cámara directamente.

---

## Diseño Simplificado

```csharp
public interface ICameraBehavior
{
    bool IsActive { get; set; }
    int Priority { get; }
    
    float? Duration { get; }
    float ElapsedTime { get; }
    bool IsComplete { get; }
    
    void Update(float deltaTime);
    void Reset();
}
```

---

## Implementación Base

```csharp
public abstract class CameraBehaviorBase : ICameraBehavior
{
    protected readonly Camera Camera;
    
    public bool IsActive { get; set; } = true;
    public abstract int Priority { get; }
    
    public float? Duration { get; protected set; }
    public float ElapsedTime { get; private set; }
    public bool IsComplete { get; protected set; }

    protected CameraBehaviorBase(Camera camera)
    {
        Camera = camera;
    }

    public void Update(float deltaTime)
    {
        if (!IsActive || IsComplete)
            return;

        ElapsedTime += deltaTime;

        if (Duration.HasValue && ElapsedTime >= Duration.Value)
        {
            IsComplete = true;
            OnComplete();
            return;
        }

        OnUpdate(deltaTime);
    }

    protected abstract void OnUpdate(float deltaTime);

    public virtual void Reset()
    {
        ElapsedTime = 0f;
        IsComplete = false;
        IsActive = true;
    }

    protected virtual void OnComplete() { }
}
```

---

## Camera.Update

```csharp
public void Update(GameTime gameTime)
{
    float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
    for (int i = _behaviors.Count - 1; i >= 0; i--)
    {
        var behavior = _behaviors[i];
        
        if (!behavior.IsActive)
            continue;
        
        behavior.Update(delta);
        
        if (behavior.IsComplete && behavior.Duration.HasValue)
        {
            _behaviors.RemoveAt(i);
        }
    }
}
```

---

## Ejemplo: ShakeBehavior

```csharp
public class ShakeBehavior : CameraBehaviorBase
{
    public override int Priority => 100;
    
    private float _currentTrauma;
    private Random _random = new Random();
    private float _intensity;
    private Vector2 _originalPosition;
    
    public ShakeBehavior(Camera camera, float duration, float intensity) 
        : base(camera)
    {
        Duration = duration;
        _intensity = intensity;
        _originalPosition = camera.Position;
    }
    
    protected override void OnUpdate(float deltaTime)
    {
        _currentTrauma = 1f - (ElapsedTime / Duration.Value);
        _currentTrauma = MathUtils.Clamp01(_currentTrauma);
        _currentTrauma *= _currentTrauma;
        
        float shake = _currentTrauma * _intensity;
        float offsetX = ((float)_random.NextDouble() * 2f - 1f) * shake;
        float offsetY = ((float)_random.NextDouble() * 2f - 1f) * shake;
        
        Camera.SetPosition(_originalPosition + new Vector2(offsetX, offsetY));
    }
    
    protected override void OnComplete()
    {
        Camera.SetPosition(_originalPosition);
    }
}
```

---

## Código Completo de Fase 1

### 1. MathUtils (sin cambios)

```csharp
using System;
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public static class MathUtils
    {
        public const float PI = (float)System.Math.PI;
        public const float TwoPI = PI * 2f;
        public const float DegToRad = PI / 180f;
        public const float RadToDeg = 180f / PI;
        public const float Epsilon = 0.0001f;

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(
                Lerp(a.X, b.X, t),
                Lerp(a.Y, b.Y, t)
            );
        }

        public static float SmoothStep(float a, float b, float t)
        {
            t = Clamp01(t);
            t = t * t * (3f - 2f * t);
            return Lerp(a, b, t);
        }

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static float Clamp01(float value)
        {
            return Clamp(value, 0f, 1f);
        }

        public static float Approach(float current, float target, float maxDelta)
        {
            float diff = target - current;
            if (System.Math.Abs(diff) <= maxDelta)
                return target;
            
            return current + System.Math.Sign(diff) * maxDelta;
        }

        public static Vector2 Approach(Vector2 current, Vector2 target, float maxDelta)
        {
            Vector2 diff = target - current;
            float dist = diff.Length();
            
            if (dist <= maxDelta || dist < Epsilon)
                return target;
            
            return current + (diff / dist) * maxDelta;
        }

        public static float Snap(float value, float gridSize)
        {
            if (gridSize < Epsilon) return value;
            return (float)System.Math.Floor(value / gridSize) * gridSize;
        }

        public static bool Approx(float a, float b, float epsilon = Epsilon)
        {
            return System.Math.Abs(a - b) < epsilon;
        }

        public static bool Approx(Vector2 a, Vector2 b, float epsilon = Epsilon)
        {
            return Approx(a.X, b.X, epsilon) && Approx(a.Y, b.Y, epsilon);
        }
    }
}
```

### 2. RectangleF (sin cambios)

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public struct RectangleF
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2 position, Vector2 size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.X;
            Height = size.Y;
        }

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        public Vector2 Center => new Vector2(X + Width * 0.5f, Y + Height * 0.5f);
        public Vector2 Size => new Vector2(Width, Height);
        public Vector2 Position => new Vector2(X, Y);

        public bool Contains(Vector2 point)
        {
            return point.X >= Left && point.X <= Right &&
                   point.Y >= Top && point.Y <= Bottom;
        }

        public bool Contains(RectangleF other)
        {
            return Left <= other.Left && Right >= other.Right &&
                   Top <= other.Top && Bottom >= other.Bottom;
        }

        public bool Intersects(RectangleF other)
        {
            return Left < other.Right && Right > other.Left &&
                   Top < other.Bottom && Bottom > other.Top;
        }

        public RectangleF Expand(float amount)
        {
            return new RectangleF(
                X - amount,
                Y - amount,
                Width + amount * 2f,
                Height + amount * 2f
            );
        }

        public RectangleF Expand(Vector2 amount)
        {
            return new RectangleF(
                X - amount.X,
                Y - amount.Y,
                Width + amount.X * 2f,
                Height + amount.Y * 2f
            );
        }

        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            float left = System.Math.Max(a.Left, b.Left);
            float top = System.Math.Max(a.Top, b.Top);
            float right = System.Math.Min(a.Right, b.Right);
            float bottom = System.Math.Min(a.Bottom, b.Bottom);

            if (right > left && bottom > top)
            {
                return new RectangleF(left, top, right - left, bottom - top);
            }

            return Empty;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(
                (int)X,
                (int)Y,
                (int)Width,
                (int)Height
            );
        }

        public static RectangleF FromPoints(Vector2 a, Vector2 b)
        {
            float minX = System.Math.Min(a.X, b.X);
            float minY = System.Math.Min(a.Y, b.Y);
            float maxX = System.Math.Max(a.X, b.X);
            float maxY = System.Math.Max(a.Y, b.Y);

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        public static RectangleF Empty => new RectangleF(0, 0, 0, 0);

        public bool IsEmpty => Width <= 0 || Height <= 0;

        public override string ToString()
        {
            return $"[{X}, {Y}, {Width}, {Height}]";
        }
    }
}
```

### 3. Vector2Extensions (sin cambios)

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public static class Vector2Extensions
    {
        public static Vector2 Perpendicular(this Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }

        public static Vector2 Rotate(this Vector2 vector, float radians)
        {
            float cos = (float)System.Math.Cos(radians);
            float sin = (float)System.Math.Sin(radians);
            
            return new Vector2(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos
            );
        }

        public static Vector2 RotateAround(this Vector2 vector, Vector2 origin, float radians)
        {
            return origin + (vector - origin).Rotate(radians);
        }

        public static Vector2 Snap(this Vector2 vector, float gridSize)
        {
            return new Vector2(
                MathUtils.Snap(vector.X, gridSize),
                MathUtils.Snap(vector.Y, gridSize)
            );
        }

        public static Vector2 Snap(this Vector2 vector, Vector2 gridSize)
        {
            return new Vector2(
                MathUtils.Snap(vector.X, gridSize.X),
                MathUtils.Snap(vector.Y, gridSize.Y)
            );
        }

        public static float Cross(this Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(
                System.Math.Abs(vector.X),
                System.Math.Abs(vector.Y)
            );
        }

        public static Vector2 Sign(this Vector2 vector)
        {
            return new Vector2(
                System.Math.Sign(vector.X),
                System.Math.Sign(vector.Y)
            );
        }

        public static Vector2 ComponentMul(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2 ComponentDiv(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }
    }
}
```

### 4. CameraState (sin cambios)

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public readonly struct CameraState
    {
        public readonly Vector2 Position;
        public readonly float Rotation;
        public readonly float Zoom;
        public readonly Vector2 Origin;

        public CameraState(Vector2 position, float rotation, float zoom, Vector2 origin)
        {
            Position = position;
            Rotation = rotation;
            Zoom = zoom;
            Origin = origin;
        }

        public static CameraState Default(Vector2 origin)
        {
            return new CameraState(Vector2.Zero, 0f, 1f, origin);
        }

        public CameraState WithPosition(Vector2 position)
        {
            return new CameraState(position, Rotation, Zoom, Origin);
        }

        public CameraState WithRotation(float rotation)
        {
            return new CameraState(Position, rotation, Zoom, Origin);
        }

        public CameraState WithZoom(float zoom)
        {
            return new CameraState(Position, Rotation, zoom, Origin);
        }

        public CameraState WithOrigin(Vector2 origin)
        {
            return new CameraState(Position, Rotation, Zoom, origin);
        }

        public Matrix GetViewMatrix()
        {
            Matrix translation = Matrix.CreateTranslation(-Position.X, -Position.Y, 0f);
            Matrix rotation = Matrix.CreateRotationZ(-Rotation);
            Matrix scale = Matrix.CreateScale(Zoom, Zoom, 1f);
            Matrix originOffset = Matrix.CreateTranslation(Origin.X, Origin.Y, 0f);

            return translation * rotation * scale * originOffset;
        }

        public Matrix GetInverseViewMatrix()
        {
            Matrix scale = Matrix.CreateScale(1f / Zoom, 1f / Zoom, 1f);
            Matrix rotation = Matrix.CreateRotationZ(Rotation);
            Matrix translation = Matrix.CreateTranslation(Position.X, Position.Y, 0f);
            Matrix originOffset = Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0f);

            return originOffset * scale * rotation * translation;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition, Viewport viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix projection = GetProjectionMatrix(viewport);
            
            Vector3 transformed = Vector3.Transform(new Vector3(worldPosition, 0f), view * projection);
            return new Vector2(transformed.X, transformed.Y);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition, Viewport viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix projection = GetProjectionMatrix(viewport);
            Matrix inverse = Matrix.Invert(view * projection);
            
            Vector3 transformed = Vector3.Transform(new Vector3(screenPosition, 0f), inverse);
            return new Vector2(transformed.X, transformed.Y);
        }

        private Matrix GetProjectionMatrix(Viewport viewport)
        {
            return Matrix.CreateOrthographicOffCenter(
                0, viewport.Width,
                viewport.Height, 0,
                0f, 1f
            );
        }

        public RectangleF GetVisibleBounds(Viewport viewport)
        {
            Vector2 topLeft = ScreenToWorld(Vector2.Zero, viewport);
            Vector2 bottomRight = ScreenToWorld(new Vector2(viewport.Width, viewport.Height), viewport);
            
            return RectangleF.FromPoints(topLeft, bottomRight);
        }

        public override string ToString()
        {
            return $"Pos:{Position}, Rot:{Rotation:F2}, Zoom:{Zoom:F2}, Origin:{Origin}";
        }
    }
}
```

### 5. ICameraBehavior (nuevo)

```csharp
namespace GameEngine.Camera
{
    public interface ICameraBehavior
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        float? Duration { get; }
        float ElapsedTime { get; }
        bool IsComplete { get; }
        
        void Update(float deltaTime);
        void Reset();
    }
}
```

### 6. CameraBehaviorBase (nuevo)

```csharp
namespace GameEngine.Camera
{
    public abstract class CameraBehaviorBase : ICameraBehavior
    {
        protected readonly Camera Camera;
        
        public bool IsActive { get; set; } = true;
        public abstract int Priority { get; }
        
        public float? Duration { get; protected set; }
        public float ElapsedTime { get; private set; }
        public bool IsComplete { get; protected set; }

        protected CameraBehaviorBase(Camera camera)
        {
            Camera = camera;
        }

        public void Update(float deltaTime)
        {
            if (!IsActive || IsComplete)
                return;

            ElapsedTime += deltaTime;

            if (Duration.HasValue && ElapsedTime >= Duration.Value)
            {
                IsComplete = true;
                OnComplete();
                return;
            }

            OnUpdate(deltaTime);
        }

        protected abstract void OnUpdate(float deltaTime);

        public virtual void Reset()
        {
            ElapsedTime = 0f;
            IsComplete = false;
            IsActive = true;
        }

        protected virtual void OnComplete() { }
    }
}
```

### 7. Scene (placeholder)

```csharp
using GameEngine.Math;

namespace GameEngine.Scene
{
    public class Scene
    {
        public RectangleF WorldBounds { get; set; }
        
        public Scene(RectangleF worldBounds)
        {
            WorldBounds = worldBounds;
        }
    }
}
```

### 8. CameraDirector

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
{
    public class CameraDirector
    {
        public Scene.Scene Scene { get; }
        public List<Camera> Cameras { get; } = new List<Camera>();

        public CameraDirector(Scene.Scene scene)
        {
            Scene = scene;
        }

        public Camera CreateCamera(string name, Vector2 origin)
        {
            var camera = new Camera(name, origin, this);
            Cameras.Add(camera);
            return camera;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var camera in Cameras)
            {
                camera.Update(gameTime);
            }
        }
    }
}
```

### 9. Camera (actualizado)

```csharp
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public class Camera
    {
        private readonly List<ICameraBehavior> _behaviors = new List<ICameraBehavior>();
        private CameraState _state;
        private Matrix _viewMatrix;
        private bool _matrixDirty = true;

        public string Name { get; }
        public CameraDirector Director { get; }
        public Viewport Viewport { get; set; }
        
        public CameraState State 
        { 
            get => _state;
            private set
            {
                _state = value;
                _matrixDirty = true;
            }
        }

        public Vector2 Position => State.Position;
        public float Rotation => State.Rotation;
        public float Zoom => State.Zoom;
        public Vector2 Origin => State.Origin;

        public Camera(string name, Vector2 origin, CameraDirector director)
        {
            Name = name;
            Director = director;
            _state = CameraState.Default(origin);
            Viewport = new Viewport(0, 0, 800, 600);
        }

        public Matrix GetViewMatrix()
        {
            if (_matrixDirty)
            {
                _viewMatrix = State.GetViewMatrix();
                _matrixDirty = false;
            }
            return _viewMatrix;
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                var behavior = _behaviors[i];
                
                if (!behavior.IsActive)
                    continue;
                
                behavior.Update(delta);
                
                if (behavior.IsComplete && behavior.Duration.HasValue)
                {
                    _behaviors.RemoveAt(i);
                }
            }
        }

        public void SetPosition(Vector2 position)
        {
            State = State.WithPosition(position);
        }

        public void SetRotation(float rotation)
        {
            State = State.WithRotation(rotation);
        }

        public void SetZoom(float zoom)
        {
            State = State.WithZoom(MathUtils.Clamp(zoom, 0.001f, 100f));
        }

        public void Move(Vector2 delta)
        {
            State = State.WithPosition(State.Position + delta);
        }

        public void Rotate(float deltaRadians)
        {
            State = State.WithRotation(State.Rotation + deltaRadians);
        }

        public void ZoomBy(float factor)
        {
            SetZoom(State.Zoom * factor);
        }

        public void AddBehavior(ICameraBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public void RemoveBehavior(ICameraBehavior behavior)
        {
            _behaviors.Remove(behavior);
        }

        public T GetBehavior<T>() where T : ICameraBehavior
        {
            return _behaviors.OfType<T>().FirstOrDefault();
        }

        public void ClearBehaviors()
        {
            _behaviors.Clear();
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return State.WorldToScreen(worldPosition, Viewport);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return State.ScreenToWorld(screenPosition, Viewport);
        }

        public RectangleF GetVisibleBounds()
        {
            return State.GetVisibleBounds(Viewport);
        }

        public RectangleF GetCullingBounds()
        {
            var visible = GetVisibleBounds();
            var world = Director.Scene.WorldBounds;
            return RectangleF.Intersect(visible, world);
        }

        public bool IsInView(RectangleF bounds)
        {
            return GetCullingBounds().Intersects(bounds);
        }

        public bool IsInView(Vector2 point)
        {
            return GetCullingBounds().Contains(point);
        }

        public override string ToString()
        {
            return $"Camera '{Name}': {State}, Viewport:{Viewport.Width}x{Viewport.Height}";
        }
    }
}
```

---

## Resumen de Cambios

| Aspecto | Antes | Después |
|--------|-------|---------|
| Métodos behavior | `Update` + `Apply` | Solo `Update` |
| Contexto | `CameraContext` struct | Referencia directa en constructor |
| Modificación | `context.ModifiedState` | `Camera.SetPosition()` etc. |
| Acumulación | Cadena de estados | Behaviors modifican directo |

¿Confirmamos para pasar a Fase 2?
User: Pasemos a la fase 2. De ahora en adelante, no utilices nunca instrucciones break ni continue
Kimi: Entendido. Sin `break` ni `continue`. Aquí está la **Fase 2: Renderizado Básico**.

---

## Componentes de Fase 2

| Orden | Componente | Descripción |
|-------|-----------|-------------|
| 1 | **RenderCommand** | Qué dibujar, dónde, cómo |
| 2 | **RenderQueue** | Lista ordenable de comandos |
| 3 | **IRenderable** | Interfaz para objetos que generan comandos |
| 4 | **SpriteBatch integration** | Ejecutar comandos con matriz cámara |
| 5 | **Actor base** | Entidad que implementa IRenderable |

---

## 1. RenderCommand

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public class RenderCommand
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float LayerDepth { get; set; }
        
        // Culling
        public RectangleF Bounds { get; set; }
        
        // Opcional: shader, material, etc.
        public Effect Effect { get; set; }

        public RenderCommand()
        {
        }

        public RenderCommand(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            Bounds = new RectangleF(position, texture.Bounds.Size.ToVector2());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null) return;
            
            spriteBatch.Draw(
                Texture,
                Position,
                SourceRectangle,
                Color,
                Rotation,
                Origin,
                Scale,
                Effects,
                LayerDepth
            );
        }
    }
}
```

---

## 2. RenderQueue

```csharp
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Rendering
{
    public class RenderQueue
    {
        private readonly List<RenderCommand> _commands = new List<RenderCommand>();
        private bool _isSorted = false;

        public IReadOnlyList<RenderCommand> Commands => _commands;

        public void Clear()
        {
            _commands.Clear();
            _isSorted = false;
        }

        public void Enqueue(RenderCommand command)
        {
            _commands.Add(command);
            _isSorted = false;
        }

        public void EnqueueRange(IEnumerable<RenderCommand> commands)
        {
            _commands.AddRange(commands);
            _isSorted = false;
        }

        public void SortByLayerDepth()
        {
            if (_isSorted) return;
            
            _commands.Sort((a, b) => a.LayerDepth.CompareTo(b.LayerDepth));
            _isSorted = true;
        }

        public void SortByTexture()
        {
            if (_isSorted) return;
            
            // Minimiza cambios de textura en GPU
            _commands.Sort((a, b) => 
            {
                int textureCompare = ReferenceEquals(a.Texture, b.Texture) 
                    ? 0 
                    : a.Texture.GetHashCode().CompareTo(b.Texture.GetHashCode());
                    
                if (textureCompare != 0) return textureCompare;
                
                return a.LayerDepth.CompareTo(b.LayerDepth);
            });
            
            _isSorted = true;
        }

        public void Execute(SpriteBatch spriteBatch)
        {
            SortByLayerDepth();
            
            foreach (var command in _commands)
            {
                command.Draw(spriteBatch);
            }
        }
    }
}
```

---

## 3. IRenderable

```csharp
using GameEngine.Camera;

namespace GameEngine.Rendering
{
    public interface IRenderable
    {
        // Genera comandos para esta cámara
        void CollectRenderCommands(Camera camera, RenderQueue queue);
        
        // Bounds para culling
        RectangleF GetRenderBounds();
        
        // Visible en esta cámara?
        bool IsVisible(Camera camera);
    }
}
```

---

## 4. Actor (Base de Entidades)

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Camera;
using GameEngine.Rendering;

namespace GameEngine.Entities
{
    public abstract class Actor : IRenderable
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        
        protected Actor(Vector2 position)
        {
            Position = position;
        }

        public abstract void Update(GameTime gameTime);
        
        public virtual void CollectRenderCommands(Camera camera, RenderQueue queue)
        {
            if (IsActive == false) return;
            if (IsVisible == false) return;
            if (IsVisible(camera) == false) return;

            var commands = GenerateRenderCommands();
            
            foreach (var command in commands)
            {
                queue.Enqueue(command);
            }
        }

        protected abstract IEnumerable<RenderCommand> GenerateRenderCommands();

        public virtual RectangleF GetRenderBounds()
        {
            // Override en clases derivadas
            return new RectangleF(Position, Vector2.Zero);
        }

        public virtual bool IsVisible(Camera camera)
        {
            return camera.IsInView(GetRenderBounds());
        }
    }
}
```

---

## 5. SpriteActor (Implementación Simple)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Rendering;

namespace GameEngine.Entities
{
    public class SpriteActor : Actor
    {
        public Texture2D Texture { get; set; }
        public Color Tint { get; set; } = Color.White;
        public float LayerDepth { get; set; } = 0.5f;
        public Vector2 Origin { get; set; }

        public SpriteActor(Vector2 position, Texture2D texture) 
            : base(position)
        {
            Texture = texture;
            Origin = texture.Bounds.Center.ToVector2();
        }

        public override void Update(GameTime gameTime)
        {
            // Lógica específica del actor
        }

        protected override IEnumerable<RenderCommand> GenerateRenderCommands()
        {
            if (Texture == null) 
            {
                yield break;
            }

            yield return new RenderCommand
            {
                Texture = Texture,
                Position = Position,
                Color = Tint,
                Rotation = Rotation,
                Origin = Origin,
                Scale = Scale,
                LayerDepth = LayerDepth,
                Bounds = GetRenderBounds()
            };
        }

        public override RectangleF GetRenderBounds()
        {
            if (Texture == null) 
            {
                return new RectangleF(Position, Vector2.Zero);
            }

            Vector2 size = Texture.Bounds.Size.ToVector2();
            size = size * Scale;
            
            Vector2 halfSize = size * 0.5f;
            Vector2 min = Position - halfSize;
            
            return new RectangleF(min, size);
        }
    }
}
```

---

## 6. Camera (Métodos de Renderizado)

```csharp
// Añadir a clase Camera existente

public class Camera
{
    // ... código existente ...
    
    private readonly RenderQueue _renderQueue = new RenderQueue();

    public void Submit(RenderCommand command)
    {
        _renderQueue.Enqueue(command);
    }

    public void SubmitRange(IEnumerable<RenderCommand> commands)
    {
        _renderQueue.EnqueueRange(commands);
    }

    public void ClearRenderQueue()
    {
        _renderQueue.Clear();
    }

    public void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            null,
            null,
            null,
            GetViewMatrix()
        );

        _renderQueue.Execute(spriteBatch);
        
        spriteBatch.End();
        
        _renderQueue.Clear();
    }
}
```

---

## 7. CameraDirector (Renderizado)

```csharp
public class CameraDirector
{
    // ... código existente ...

    public void CollectRenderCommands(IEnumerable<IRenderable> renderables)
    {
        foreach (var camera in Cameras)
        {
            camera.ClearRenderQueue();
        }

        foreach (var renderable in renderables)
        {
            foreach (var camera in Cameras)
            {
                renderable.CollectRenderCommands(camera, camera.RenderQueue);
            }
        }
    }

    public void Render(SpriteBatch spriteBatch)
    {
        foreach (var camera in Cameras)
        {
            camera.Render(spriteBatch);
        }
    }
}
```

---

## Flujo Completo de Renderizado

```
Scene.Update()
    └── Actor.Update()

Scene.CollectRender()
    └── Actor.CollectRenderCommands(camera, queue)
            └── if (camera.IsInView(bounds))
                    └── queue.Enqueue(command)

CameraDirector.Render()
    └── camera.Render(spriteBatch)
            └── spriteBatch.Begin(viewMatrix)
            └── queue.Execute()
            └── spriteBatch.End()
```

---

## Checklist Fase 2

| Prueba | Descripción |
|--------|-------------|
| Sprite visible | Actor con textura se ve en pantalla |
| Culling | Actor fuera de cámara no genera comandos |
| Orden por profundidad | Sprites con LayerDepth diferente se ordenan |
| Múltiples cámaras | Mismo actor en dos cámaras genera comandos separados |
| Queue limpia | Después de Render, queue está vacía |

---

¿Pasamos a **Fase 3: Escena y Capas**?
User: Antes corrijamos los componentes de esta fase. Un rendercommand ahora mismo sólo dibuja textura pero debería ser también capaz de contener texto. Es decir deberíamos crear una clase abstracta y de ahí derivar un comando para texto y otro para textura. Por su parte, la textura debe poder indicar el rectángulo que vamos a dibujar por si no dibujamos la textura entera.
Kimi: Entendido. `RenderCommand` como clase abstracta, con derivados para textura y texto.

---

## 1. RenderCommand (Abstracta)

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public abstract class RenderCommand
    {
        public float LayerDepth { get; set; }
        public RectangleF Bounds { get; set; }
        public Effect Effect { get; set; }
        
        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont);
    }
}
```

---

## 2. TextureRenderCommand

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public class TextureRenderCommand : RenderCommand
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Rectangle? DestinationRectangle { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            if (Texture == null) return;
            
            if (DestinationRectangle.HasValue)
            {
                Rectangle dest = DestinationRectangle.Value;
                Vector2 origin = Origin;
                
                if (SourceRectangle.HasValue)
                {
                    spriteBatch.Draw(
                        Texture,
                        dest,
                        SourceRectangle.Value,
                        Color,
                        Rotation,
                        origin,
                        Effects,
                        LayerDepth
                    );
                }
                else
                {
                    spriteBatch.Draw(
                        Texture,
                        dest,
                        null,
                        Color,
                        Rotation,
                        origin,
                        Effects,
                        LayerDepth
                    );
                }
            }
            else
            {
                spriteBatch.Draw(
                    Texture,
                    Position,
                    SourceRectangle,
                    Color,
                    Rotation,
                    Origin,
                    Scale,
                    Effects,
                    LayerDepth
                );
            }
        }
    }
}
```

---

## 3. TextRenderCommand

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public class TextRenderCommand : RenderCommand
    {
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            SpriteFont fontToUse = Font ?? defaultFont;
            
            if (fontToUse == null) return;
            if (string.IsNullOrEmpty(Text)) return;
            
            spriteBatch.DrawString(
                fontToUse,
                Text,
                Position,
                Color,
                Rotation,
                Origin,
                Scale,
                Effects,
                LayerDepth
            );
        }
    }
}
```

---

## 4. RenderQueue (Actualizado)

```csharp
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Rendering
{
    public class RenderQueue
    {
        private readonly List<RenderCommand> _commands = new List<RenderCommand>();
        private bool _isSorted = false;

        public IReadOnlyList<RenderCommand> Commands => _commands;

        public void Clear()
        {
            _commands.Clear();
            _isSorted = false;
        }

        public void Enqueue(RenderCommand command)
        {
            _commands.Add(command);
            _isSorted = false;
        }

        public void EnqueueRange(IEnumerable<RenderCommand> commands)
        {
            foreach (var command in commands)
            {
                _commands.Add(command);
            }
            _isSorted = false;
        }

        public void SortByLayerDepth()
        {
            if (_isSorted) return;
            
            _commands.Sort((a, b) => a.LayerDepth.CompareTo(b.LayerDepth));
            _isSorted = true;
        }

        public void Execute(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            SortByLayerDepth();
            
            foreach (var command in _commands)
            {
                command.Draw(spriteBatch, defaultFont);
            }
        }
    }
}
```

---

## 5. SpriteActor (Actualizado)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Rendering;

namespace GameEngine.Entities
{
    public class SpriteActor : Actor
    {
        public Texture2D Texture { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Color Tint { get; set; } = Color.White;
        public float LayerDepth { get; set; } = 0.5f;
        public Vector2 Origin { get; set; }

        public SpriteActor(Vector2 position, Texture2D texture) 
            : base(position)
        {
            Texture = texture;
            Origin = texture.Bounds.Center.ToVector2();
        }

        public override void Update(GameTime gameTime)
        {
        }

        protected override IEnumerable<RenderCommand> GenerateRenderCommands()
        {
            if (Texture == null) 
            {
                yield break;
            }

            yield return new TextureRenderCommand
            {
                Texture = Texture,
                Position = Position,
                SourceRectangle = SourceRectangle,
                Color = Tint,
                Rotation = Rotation,
                Origin = Origin,
                Scale = Scale,
                LayerDepth = LayerDepth,
                Bounds = GetRenderBounds()
            };
        }

        public override RectangleF GetRenderBounds()
        {
            if (Texture == null) 
            {
                return new RectangleF(Position, Vector2.Zero);
            }

            Vector2 sourceSize;
            
            if (SourceRectangle.HasValue)
            {
                sourceSize = new Vector2(
                    SourceRectangle.Value.Width,
                    SourceRectangle.Value.Height
                );
            }
            else
            {
                sourceSize = Texture.Bounds.Size.ToVector2();
            }
            
            Vector2 size = sourceSize * Scale;
            Vector2 halfSize = size * 0.5f;
            Vector2 min = Position - halfSize;
            
            return new RectangleF(min, size);
        }
    }
}
```

---

## 6. TextActor (Nuevo)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Rendering;

namespace GameEngine.Entities
{
    public class TextActor : Actor
    {
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; } = Color.White;
        public float LayerDepth { get; set; } = 0.5f;
        public Vector2 Origin { get; set; }

        public TextActor(Vector2 position, string text, SpriteFont font) 
            : base(position)
        {
            Text = text;
            Font = font;
        }

        public override void Update(GameTime gameTime)
        {
        }

        protected override IEnumerable<RenderCommand> GenerateRenderCommands()
        {
            if (string.IsNullOrEmpty(Text))
            {
                yield break;
            }

            yield return new TextRenderCommand
            {
                Text = Text,
                Font = Font,
                Position = Position,
                Color = Color,
                Rotation = Rotation,
                Origin = Origin,
                Scale = Scale,
                LayerDepth = LayerDepth,
                Bounds = GetRenderBounds()
            };
        }

        public override RectangleF GetRenderBounds()
        {
            if (Font == null || string.IsNullOrEmpty(Text))
            {
                return new RectangleF(Position, Vector2.Zero);
            }

            Vector2 size = Font.MeasureString(Text) * Scale;
            Vector2 halfSize = size * 0.5f;
            Vector2 min = Position - halfSize;
            
            return new RectangleF(min, size);
        }
    }
}
```

---

## 7. Camera (Actualizado)

```csharp
public class Camera
{
    // ... código existente ...
    
    private readonly RenderQueue _renderQueue = new RenderQueue();

    public void Submit(RenderCommand command)
    {
        _renderQueue.Enqueue(command);
    }

    public void SubmitRange(IEnumerable<RenderCommand> commands)
    {
        _renderQueue.EnqueueRange(commands);
    }

    public void ClearRenderQueue()
    {
        _renderQueue.Clear();
    }

    public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
    {
        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            null,
            null,
            null,
            GetViewMatrix()
        );

        _renderQueue.Execute(spriteBatch, defaultFont);
        
        spriteBatch.End();
        
        _renderQueue.Clear();
    }
}
```

---

## Resumen de Cambios

| Aspecto | Antes | Después |
|--------|-------|---------|
| RenderCommand | Clase concreta | Abstracta |
| Dibujo textura | Un tipo | `TextureRenderCommand` |
| Dibujo texto | No existía | `TextRenderCommand` |
| SourceRectangle | No existía | En `TextureRenderCommand` |
| DestinationRectangle | No existía | Opcional en `TextureRenderCommand` |

¿Confirmamos para pasar a **Fase 3**?
User: Los actores van a pasar los comandos por orden de dibujo en la capa para evitar popping, por tanto no necesitamos layerdepth en el spritebatch
Kimi: Entendido. Sin `LayerDepth`, el orden es FIFO según llegan de la capa.

---

## Cambios

### RenderCommand

```csharp
public abstract class RenderCommand
{
    public RectangleF Bounds { get; set; }
    public Effect Effect { get; set; }
    
    public abstract void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont);
}
```

### RenderQueue

```csharp
public class RenderQueue
{
    private readonly List<RenderCommand> _commands = new List<RenderCommand>();

    public IReadOnlyList<RenderCommand> Commands => _commands;

    public void Clear()
    {
        _commands.Clear();
    }

    public void Enqueue(RenderCommand command)
    {
        _commands.Add(command);
    }

    public void EnqueueRange(IEnumerable<RenderCommand> commands)
    {
        foreach (var command in commands)
        {
            _commands.Add(command);
        }
    }

    public void Execute(SpriteBatch spriteBatch, SpriteFont defaultFont)
    {
        foreach (var command in _commands)
        {
            command.Draw(spriteBatch, defaultFont);
        }
    }
}
```

### TextureRenderCommand.Draw

```csharp
public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
{
    if (Texture == null) return;
    
    if (DestinationRectangle.HasValue)
    {
        Rectangle dest = DestinationRectangle.Value;
        
        if (SourceRectangle.HasValue)
        {
            spriteBatch.Draw(
                Texture,
                dest,
                SourceRectangle.Value,
                Color,
                Rotation,
                Origin,
                Effects,
                0f  // layerDepth fijo
            );
        }
        else
        {
            spriteBatch.Draw(
                Texture,
                dest,
                null,
                Color,
                Rotation,
                Origin,
                Effects,
                0f
            );
        }
    }
    else
    {
        spriteBatch.Draw(
            Texture,
            Position,
            SourceRectangle,
            Color,
            Rotation,
            Origin,
            Scale,
            Effects,
            0f  // layerDepth fijo
        );
    }
}
```

### TextRenderCommand.Draw

```csharp
public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
{
    SpriteFont fontToUse = Font ?? defaultFont;
    
    if (fontToUse == null) return;
    if (string.IsNullOrEmpty(Text)) return;
    
    spriteBatch.DrawString(
        fontToUse,
        Text,
        Position,
        Color,
        Rotation,
        Origin,
        Scale,
        Effects,
        0f  // layerDepth fijo
    );
}
```

### SpriteActor

```csharp
yield return new TextureRenderCommand
{
    Texture = Texture,
    Position = Position,
    SourceRectangle = SourceRectangle,
    Color = Tint,
    Rotation = Rotation,
    Origin = Origin,
    Scale = Scale,
    Bounds = GetRenderBounds()
    // Sin LayerDepth
};
```

### TextActor

```csharp
yield return new TextRenderCommand
{
    Text = Text,
    Font = Font,
    Position = Position,
    Color = Color,
    Rotation = Rotation,
    Origin = Origin,
    Scale = Scale,
    Bounds = GetRenderBounds()
    // Sin LayerDepth
};
```

### Camera.Render

```csharp
public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
{
    spriteBatch.Begin(
        SpriteSortMode.Deferred,  // Orden de llamada, no por profundidad
        BlendState.AlphaBlend,
        SamplerState.PointClamp,
        null,
        null,
        null,
        GetViewMatrix()
    );

    _renderQueue.Execute(spriteBatch, defaultFont);
    
    spriteBatch.End();
    
    _renderQueue.Clear();
}
```

---

## Resumen

| Eliminado | Razón |
|-----------|-------|
| `LayerDepth` en comandos | Orden determinado por capa |
| `SortByLayerDepth()` | FIFO preserva orden de capa |
| Parámetro `layerDepth` en Draw | Siempre `0f` |

El orden de renderizado es: **Capa (orden) → Actor (orden en capa) → Comando (orden en actor)**.

¿Pasamos a **Fase 3: Escena y Capas**?
User: Pasemos a la fase 3
Kimi: Aquí está la **Fase 3: Escena y Capas**.

---

## Componentes de Fase 3

| Orden | Componente | Descripción |
|-------|-----------|-------------|
| 1 | **Layer** | Contenedor de actores, enrutado a cámaras |
| 2 | **ILayerBehavior** | Comportamientos modulares de capa |
| 3 | **LayerBehaviorBase** | Base para behaviors de capa |
| 4 | **LayerDirector** | Gestiona orden de capas |
| 5 | **Scene** | Orquesta Update, CollectDraw, Render |
| 6 | **Integración cámara** | Capa sabe sus cámaras, cámara recibe comandos |

---

## 1. ILayerBehavior

```csharp
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GameEngine.Rendering;

namespace GameEngine.Layers
{
    public interface ILayerBehavior
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        float? Duration { get; }
        float ElapsedTime { get; }
        bool IsComplete { get; }
        
        void Update(float deltaTime);
        void Apply(List<RenderCommand> commands, Camera.Camera camera);
        void Reset();
    }
}
```

---

## 2. LayerBehaviorBase

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers
{
    public abstract class LayerBehaviorBase : ILayerBehavior
    {
        protected readonly Layer Layer;
        
        public bool IsActive { get; set; } = true;
        public abstract int Priority { get; }
        
        public float? Duration { get; protected set; }
        public float ElapsedTime { get; private set; }
        public bool IsComplete { get; protected set; }

        protected LayerBehaviorBase(Layer layer)
        {
            Layer = layer;
        }

        public void Update(float deltaTime)
        {
            if (IsActive == false) return;
            if (IsComplete) return;

            ElapsedTime += deltaTime;

            if (Duration.HasValue && ElapsedTime >= Duration.Value)
            {
                IsComplete = true;
                OnComplete();
                return;
            }

            OnUpdate(deltaTime);
        }

        protected abstract void OnUpdate(float deltaTime);
        public abstract void Apply(List<RenderCommand> commands, Camera camera);

        public virtual void Reset()
        {
            ElapsedTime = 0f;
            IsComplete = false;
            IsActive = true;
        }

        protected virtual void OnComplete() { }
    }
}
```

---

## 3. Layer

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Entities;
using GameEngine.Camera;
using GameEngine.Rendering;

namespace GameEngine.Layers
{
    public class Layer
    {
        public string Name { get; }
        public int DrawOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        
        private readonly List<Actor> _actors = new List<Actor>();
        private readonly List<ILayerBehavior> _behaviors = new List<ILayerBehavior>();
        
        public IReadOnlyList<Actor> Actors => _actors;
        public IReadOnlyList<ILayerBehavior> Behaviors => _behaviors;
        
        public uint CameraMask { get; set; } = 0xFFFFFFFF;

        public Layer(string name, int drawOrder)
        {
            Name = name;
            DrawOrder = drawOrder;
        }

        public void AddActor(Actor actor)
        {
            _actors.Add(actor);
        }

        public void RemoveActor(Actor actor)
        {
            _actors.Remove(actor);
        }

        public void AddBehavior(ILayerBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public void RemoveBehavior(ILayerBehavior behavior)
        {
            _behaviors.Remove(behavior);
        }

        public void Update(float deltaTime)
        {
            if (IsActive == false) return;

            foreach (var actor in _actors)
            {
                actor.Update(deltaTime);
            }

            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                var behavior = _behaviors[i];
                
                if (behavior.IsActive == false) continue;
                
                behavior.Update(deltaTime);
                
                if (behavior.IsComplete && behavior.Duration.HasValue)
                {
                    _behaviors.RemoveAt(i);
                }
            }
        }

        public void CollectRenderCommands(CameraDirector cameraDirector)
        {
            if (IsVisible == false) return;

            List<Camera> targetCameras = new List<Camera>();
            
            foreach (var camera in cameraDirector.Cameras)
            {
                uint cameraBit = (uint)(1 << cameraDirector.Cameras.IndexOf(camera));
                
                if ((CameraMask & cameraBit) != 0)
                {
                    targetCameras.Add(camera);
                }
            }

            foreach (var camera in targetCameras)
            {
                List<RenderCommand> commands = new List<RenderCommand>();

                foreach (var actor in _actors)
                {
                    if (actor.IsActive == false) continue;
                    
                    actor.CollectRenderCommands(camera, commands);
                }

                var activeBehaviors = new List<ILayerBehavior>();
                
                foreach (var behavior in _behaviors)
                {
                    if (behavior.IsActive && behavior.IsComplete == false)
                    {
                        activeBehaviors.Add(behavior);
                    }
                }
                
                activeBehaviors.Sort((a, b) => a.Priority.CompareTo(b.Priority));

                foreach (var behavior in activeBehaviors)
                {
                    behavior.Apply(commands, camera);
                }

                foreach (var command in commands)
                {
                    camera.Submit(command);
                }
            }
        }
    }
}
```

---

## 4. LayerDirector

```csharp
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Layers
{
    public class LayerDirector
    {
        private readonly List<Layer> _layers = new List<Layer>();

        public IReadOnlyList<Layer> Layers => _layers;

        public Layer CreateLayer(string name, int drawOrder)
        {
            var layer = new Layer(name, drawOrder);
            _layers.Add(layer);
            return layer;
        }

        public void RemoveLayer(Layer layer)
        {
            _layers.Remove(layer);
        }

        public void SortLayersByDrawOrder()
        {
            _layers.Sort((a, b) => a.DrawOrder.CompareTo(b.DrawOrder));
        }

        public void Update(float deltaTime)
        {
            foreach (var layer in _layers)
            {
                layer.Update(deltaTime);
            }
        }

        public void CollectRenderCommands(Camera.CameraDirector cameraDirector)
        {
            SortLayersByDrawOrder();
            
            foreach (var layer in _layers)
            {
                layer.CollectRenderCommands(cameraDirector);
            }
        }
    }
}
```

---

## 5. Scene

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Camera;
using GameEngine.Layers;
using GameEngine.Math;

namespace GameEngine.Scene
{
    public class Scene
    {
        public RectangleF WorldBounds { get; set; }
        
        public CameraDirector CameraDirector { get; }
        public LayerDirector LayerDirector { get; }

        public Scene(RectangleF worldBounds)
        {
            WorldBounds = worldBounds;
            CameraDirector = new CameraDirector(this);
            LayerDirector = new LayerDirector();
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            CameraDirector.Update(gameTime);
            LayerDirector.Update(delta);
        }

        public void CollectRenderCommands()
        {
            LayerDirector.CollectRenderCommands(CameraDirector);
        }

        public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            foreach (var camera in CameraDirector.Cameras)
            {
                camera.Render(spriteBatch, defaultFont);
            }
        }

        public void RenderToTarget(SpriteBatch spriteBatch, SpriteFont defaultFont, RenderTarget2D target)
        {
            // Para post-procesado futuro
        }
    }
}
```

---

## 6. Actor (Actualizado para múltiples comandos)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Camera;
using GameEngine.Rendering;

namespace GameEngine.Entities
{
    public abstract class Actor : IRenderable
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        
        protected Actor(Vector2 position)
        {
            Position = position;
        }

        public abstract void Update(float deltaTime);
        
        public void CollectRenderCommands(Camera camera, List<RenderCommand> commands)
        {
            if (IsActive == false) return;
            if (IsVisible == false) return;
            if (IsVisible(camera) == false) return;

            foreach (var command in GenerateRenderCommands())
            {
                commands.Add(command);
            }
        }

        protected abstract IEnumerable<RenderCommand> GenerateRenderCommands();

        public abstract RectangleF GetRenderBounds();

        public virtual bool IsVisible(Camera camera)
        {
            return camera.IsInView(GetRenderBounds());
        }
    }
}
```

---

## 7. IRenderable (Actualizado)

```csharp
using System.Collections.Generic;
using GameEngine.Camera;
using GameEngine.Rendering;

namespace GameEngine.Rendering
{
    public interface IRenderable
    {
        void CollectRenderCommands(Camera camera, List<RenderCommand> commands);
        RectangleF GetRenderBounds();
        bool IsVisible(Camera camera);
    }
}
```

---

## Flujo Completo de Fase 3

```
Scene.Update(gameTime)
├── CameraDirector.Update(gameTime)
│   └── Camera.Update(gameTime)
│       └── Behavior.Update(delta) → modifica cámara
│
└── LayerDirector.Update(delta)
    └── Layer.Update(delta)
        ├── Actor.Update(delta)
        └── LayerBehavior.Update(delta)

Scene.CollectRenderCommands()
└── LayerDirector.CollectRenderCommands(CameraDirector)
    └── Layer.CollectRenderCommands(CameraDirector)
        ├── Filtra cámaras por CameraMask
        ├── Actor.CollectRenderCommands(camera, list)
        │   └── Genera comandos si IsVisible(camera)
        ├── LayerBehavior.Apply(commands, camera)
        └── Camera.Submit(command)

Scene.Render(spriteBatch, font)
└── Camera.Render(spriteBatch, font)
    └── RenderQueue.Execute(spriteBatch, font)
```

---

## Ejemplo de Uso

```csharp
var scene = new Scene(new RectangleF(0, 0, 2000, 1500));

var camera = scene.CameraDirector.CreateCamera("Main", new Vector2(400, 300));
camera.Viewport = new Viewport(0, 0, 800, 600);

var gameplayLayer = scene.LayerDirector.CreateLayer("Gameplay", 0);
gameplayLayer.CameraMask = 1 << 0; // Solo cámara 0

var uiLayer = scene.LayerDirector.CreateLayer("UI", 100);
uiLayer.CameraMask = 0; // Sin cámara mundo, se maneja aparte

gameplayLayer.AddActor(new SpriteActor(new Vector2(100, 100), playerTexture));
```

---

¿Pasamos a **Fase 4: Behaviors Básicos** (Follow, Bounds, Parallax)?
User: Ahora me surge una duda sobre las cámaras. Sí tengo varias cámaras que cada una se va a dibujar en una posición y con diferentes tamaños ¿Se le deben pasar estás coordenadas en la creación de cámara o es el director quien decide donde se dibuja y como se escala?
Kimi: Buena pregunta. Separar las responsabilidades:

- **Cámara**: Qué ve del mundo (transformación)
- **Director**: Dónde se dibuja en pantalla (viewport) y cómo se escala (proyección)

---

## Diseño Corregido

### ViewportConfig (configuración de renderizado)

```csharp
public class ViewportConfig
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    
    // Escalado de la vista
    public float ScaleX { get; set; } = 1f;
    public float ScaleY { get; set; } = 1f;
    
    // Para letterboxing/pillarboxing
    public bool MaintainAspectRatio { get; set; } = true;
    public Color LetterboxColor { get; set; } = Color.Black;
    
    public Viewport ToViewport()
    {
        return new Viewport(X, Y, Width, Height);
    }
    
    public Rectangle GetDestinationRectangle()
    {
        return new Rectangle(X, Y, Width, Height);
    }
}
```

---

## Cámara (solo transformación mundo)

```csharp
public class Camera
{
    public string Name { get; }
    public CameraDirector Director { get; }
    
    // Transformación del mundo
    public CameraState State { get; private set; }
    public float VirtualWidth { get; set; } = 800f;
    public float VirtualHeight { get; set; } = 600f;
    
    // No tiene Viewport aquí
    // No sabe dónde se dibuja en pantalla
    
    public Camera(string name, Vector2 origin, CameraDirector director)
    {
        Name = name;
        Director = director;
        State = CameraState.Default(origin);
        VirtualWidth = origin.X * 2f;
        VirtualHeight = origin.Y * 2f;
    }
    
    public Matrix GetViewMatrix()
    {
        return State.GetViewMatrix();
    }
    
    public Matrix GetProjectionMatrix(ViewportConfig viewport)
    {
        // Proyección ortográfica basada en virtual resolution
        float targetAspect = viewport.Width / (float)viewport.Height;
        float virtualAspect = VirtualWidth / VirtualHeight;
        
        float viewWidth = VirtualWidth;
        float viewHeight = VirtualHeight;
        
        if (viewport.MaintainAspectRatio)
        {
            if (targetAspect > virtualAspect)
            {
                // Letterboxing horizontal
                viewWidth = VirtualHeight * targetAspect;
            }
            else
            {
                // Pillarboxing vertical
                viewHeight = VirtualWidth / targetAspect;
            }
        }
        
        return Matrix.CreateOrthographicOffCenter(
            0, viewWidth,
            viewHeight, 0,  // Y invertido
            0f, 1f
        );
    }
    
    // ... resto de métodos (Update, culling, etc.)
}
```

---

## CameraDirector (controla dónde se dibuja)

```csharp
public class CameraDirector
{
    public Scene.Scene Scene { get; }
    public List<Camera> Cameras { get; } = new List<Camera>();
    
    // Configuración de viewport por cámara
    private readonly Dictionary<Camera, ViewportConfig> _viewportConfigs = 
        new Dictionary<Camera, ViewportConfig>();

    public CameraDirector(Scene.Scene scene)
    {
        Scene = scene;
    }

    public Camera CreateCamera(string name, Vector2 origin)
    {
        var camera = new Camera(name, origin, this);
        Cameras.Add(camera);
        
        // Viewport por defecto: pantalla completa
        _viewportConfigs[camera] = new ViewportConfig
        {
            X = 0,
            Y = 0,
            Width = 800,
            Height = 600
        };
        
        return camera;
    }

    public void SetViewport(Camera camera, ViewportConfig config)
    {
        if (Cameras.Contains(camera))
        {
            _viewportConfigs[camera] = config;
        }
    }

    public ViewportConfig GetViewport(Camera camera)
    {
        if (_viewportConfigs.TryGetValue(camera, out var config))
        {
            return config;
        }
        
        return null;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var camera in Cameras)
        {
            camera.Update(gameTime);
        }
    }

    public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
    {
        foreach (var camera in Cameras)
        {
            var viewport = _viewportConfigs[camera];
            
            // Guardar viewport anterior
            var oldViewport = spriteBatch.GraphicsDevice.Viewport;
            
            // Aplicar viewport de esta cámara
            spriteBatch.GraphicsDevice.Viewport = viewport.ToViewport();
            
            // Renderizar cámara con su proyección específica
            camera.Render(spriteBatch, defaultFont, viewport);
            
            // Restaurar viewport
            spriteBatch.GraphicsDevice.Viewport = oldViewport;
        }
    }
}
```

---

## Camera.Render (actualizado)

```csharp
public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont, ViewportConfig viewport)
{
    Matrix view = GetViewMatrix();
    Matrix projection = GetProjectionMatrix(viewport);
    Matrix transform = view * projection;

    spriteBatch.Begin(
        SpriteSortMode.Deferred,
        BlendState.AlphaBlend,
        SamplerState.PointClamp,
        null,
        null,
        null,
        transform
    );

    _renderQueue.Execute(spriteBatch, defaultFont);
    
    spriteBatch.End();
    
    _renderQueue.Clear();
}
```

---

## Ejemplos de Uso

### Split-screen horizontal

```csharp
var cam1 = director.CreateCamera("Player1", new Vector2(400, 300));
var cam2 = director.CreateCamera("Player2", new Vector2(400, 300));

director.SetViewport(cam1, new ViewportConfig
{
    X = 0,
    Y = 0,
    Width = 800,
    Height = 300
});

director.SetViewport(cam2, new ViewportConfig
{
    X = 0,
    Y = 300,
    Width = 800,
    Height = 300
});
```

### Minimapa (escalado)

```csharp
var minimap = director.CreateCamera("Minimap", new Vector2(100, 100));
minimap.VirtualWidth = 400f;
minimap.VirtualHeight = 400f;

director.SetViewport(minimap, new ViewportConfig
{
    X = 650,
    Y = 450,
    Width = 150,
    Height = 150,
    MaintainAspectRatio = true
});
```

### Picture-in-picture

```csharp
var main = director.CreateCamera("Main", new Vector2(400, 300));
var pip = director.CreateCamera("PIP", new Vector2(400, 300));

director.SetViewport(main, new ViewportConfig
{
    X = 0,
    Y = 0,
    Width = 800,
    Height = 600
});

director.SetViewport(pip, new ViewportConfig
{
    X = 20,
    Y = 20,
    Width = 200,
    Height = 150,
    ScaleX = 0.25f,
    ScaleY = 0.25f
});
```

---

## Resumen de Responsabilidades

| Componente | Sabe | Hace |
|-----------|------|------|
| **Camera** | Virtual resolution, transformación mundo | Genera ViewMatrix, ProjectionMatrix |
| **ViewportConfig** | Posición pantalla, tamaño, escalado | Define dónde se renderiza |
| **CameraDirector** | Mapeo cámara→viewport | Aplica viewports, orquesta render |
| **Layer** | CameraMask | Decide qué cámaras reciben comandos |

---

¿Ajustamos el código de Fase 3 con este diseño o pasamos a Fase 4?
User: Me preocupa además si hemos tenido en cuenta que hay cámaras donde se va a dibujar con coordenadas relativas
Kimi: Tienes razón. Necesitamos distinguir tipos de cámara:

- **WorldCamera**: Coordenadas mundo (unidades de juego)
- **ScreenCamera**: Coordenadas relativas 0-1 (UI, HUD)
- **HybridCamera**: Posición mundo pero escala fija (HUDs 3D, marcas sobre objetos)

---

## Jerarquía de Cámaras

```csharp
public enum CameraType
{
    World,      // Coordenadas mundo, afectada por transform
    Screen,     // Coordenadas 0-1, ortográfica fija
    Hybrid      // Posición mundo, escala fija
}

public abstract class Camera
{
    public string Name { get; }
    public CameraDirector Director { get; }
    public CameraType Type { get; }
    
    // Estado base (posición, rotación, zoom)
    public CameraState State { get; protected set; }
    
    // Virtual resolution para proyección
    public float VirtualWidth { get; set; }
    public float VirtualHeight { get; set; }
    
    protected Camera(string name, CameraType type, Vector2 origin, CameraDirector director)
    {
        Name = name;
        Type = type;
        Director = director;
        State = CameraState.Default(origin);
        VirtualWidth = origin.X * 2f;
        VirtualHeight = origin.Y * 2f;
    }
    
    public abstract Matrix GetViewMatrix();
    public abstract Matrix GetProjectionMatrix(ViewportConfig viewport);
    
    // Culling según tipo
    public abstract bool IsInView(RectangleF bounds);
    public abstract bool IsInView(Vector2 point);
    
    // Transformaciones de coordenadas
    public abstract Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport);
    public abstract Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport);
}
```

---

## WorldCamera

```csharp
public class WorldCamera : Camera
{
    public WorldCamera(string name, Vector2 origin, CameraDirector director)
        : base(name, CameraType.World, origin, director)
    {
    }

    public override Matrix GetViewMatrix()
    {
        return State.GetViewMatrix();
    }

    public override Matrix GetProjectionMatrix(ViewportConfig viewport)
    {
        float targetAspect = viewport.Width / (float)viewport.Height;
        float virtualAspect = VirtualWidth / VirtualHeight;
        
        float viewWidth = VirtualWidth;
        float viewHeight = VirtualHeight;
        
        if (viewport.MaintainAspectRatio)
        {
            if (targetAspect > virtualAspect)
            {
                viewWidth = VirtualHeight * targetAspect;
            }
            else
            {
                viewHeight = VirtualWidth / targetAspect;
            }
        }
        
        return Matrix.CreateOrthographicOffCenter(
            0, viewWidth,
            viewHeight, 0,
            0f, 1f
        );
    }

    public override bool IsInView(RectangleF bounds)
    {
        var visible = State.GetVisibleBounds(Director.GetViewport(this).ToViewport());
        var world = Director.Scene.WorldBounds;
        return RectangleF.Intersect(visible, world).Intersects(bounds);
    }

    public override bool IsInView(Vector2 point)
    {
        var visible = State.GetVisibleBounds(Director.GetViewport(this).ToViewport());
        var world = Director.Scene.WorldBounds;
        return RectangleF.Intersect(visible, world).Contains(point);
    }

    public override Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport)
    {
        Matrix view = GetViewMatrix();
        Matrix proj = GetProjectionMatrix(viewport);
        Vector3 transformed = Vector3.Transform(new Vector3(worldPos, 0f), view * proj);
        return new Vector2(transformed.X, transformed.Y);
    }

    public override Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport)
    {
        Matrix view = GetViewMatrix();
        Matrix proj = GetProjectionMatrix(viewport);
        Matrix inverse = Matrix.Invert(view * proj);
        Vector3 transformed = Vector3.Transform(new Vector3(screenPos, 0f), inverse);
        return new Vector2(transformed.X, transformed.Y);
    }
}
```

---

## ScreenCamera (UI)

```csharp
public class ScreenCamera : Camera
{
    public ScreenCamera(string name, Vector2 origin, CameraDirector director)
        : base(name, CameraType.Screen, origin, director)
    {
    }

    public override Matrix GetViewMatrix()
    {
        // Identidad: no transforma
        return Matrix.Identity;
    }

    public override Matrix GetProjectionMatrix(ViewportConfig viewport)
    {
        // 0,0 es top-left, 1,1 es bottom-right
        return Matrix.CreateOrthographicOffCenter(
            0, 1,
            1, 0,  // Y invertido
            0f, 1f
        );
    }

    public override bool IsInView(RectangleF bounds)
    {
        // En coordenadas 0-1
        return bounds.Left >= 0 && bounds.Right <= 1 &&
               bounds.Top >= 0 && bounds.Bottom <= 1;
    }

    public override bool IsInView(Vector2 point)
    {
        return point.X >= 0 && point.X <= 1 &&
               point.Y >= 0 && point.Y <= 1;
    }

    public override Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport)
    {
        // worldPos ya está en 0-1
        return worldPos;
    }

    public override Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport)
    {
        // screenPos en pixels a 0-1
        return new Vector2(
            screenPos.X / viewport.Width,
            screenPos.Y / viewport.Height
        );
    }
}
```

---

## HybridCamera

```csharp
public class HybridCamera : Camera
{
    // Escala fija: 1 unidad mundo = X pixels pantalla
    public float PixelsPerUnit { get; set; } = 100f;

    public HybridCamera(string name, Vector2 origin, CameraDirector director)
        : base(name, CameraType.Hybrid, origin, director)
    {
    }

    public override Matrix GetViewMatrix()
    {
        // Solo traslación, no rotación ni zoom
        return Matrix.CreateTranslation(-State.Position.X, -State.Position.Y, 0f);
    }

    public override Matrix GetProjectionMatrix(ViewportConfig viewport)
    {
        // Centro en origen, escala fija
        float halfWidth = viewport.Width / (2f * PixelsPerUnit);
        float halfHeight = viewport.Height / (2f * PixelsPerUnit);
        
        return Matrix.CreateOrthographicOffCenter(
            -halfWidth, halfWidth,
            halfHeight, -halfHeight,
            0f, 1f
        );
    }

    public override bool IsInView(RectangleF bounds)
    {
        // Culling en mundo con tamaño fijo en pantalla
        var viewport = Director.GetViewport(this);
        float halfWidth = viewport.Width / (2f * PixelsPerUnit);
        float halfHeight = viewport.Height / (2f * PixelsPerUnit);
        
        var visible = new RectangleF(
            State.Position.X - halfWidth,
            State.Position.Y - halfHeight,
            halfWidth * 2f,
            halfHeight * 2f
        );
        
        return visible.Intersects(bounds);
    }

    public override bool IsInView(Vector2 point)
    {
        var viewport = Director.GetViewport(this);
        float halfWidth = viewport.Width / (2f * PixelsPerUnit);
        float halfHeight = viewport.Height / (2f * PixelsPerUnit);
        
        return point.X >= State.Position.X - halfWidth &&
               point.X <= State.Position.X + halfWidth &&
               point.Y >= State.Position.Y - halfHeight &&
               point.Y <= State.Position.Y + halfHeight;
    }

    public override Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport)
    {
        Vector2 offset = worldPos - State.Position;
        return new Vector2(
            viewport.Width / 2f + offset.X * PixelsPerUnit,
            viewport.Height / 2f + offset.Y * PixelsPerUnit
        );
    }

    public override Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport)
    {
        float x = (screenPos.X - viewport.Width / 2f) / PixelsPerUnit;
        float y = (screenPos.Y - viewport.Height / 2f) / PixelsPerUnit;
        return State.Position + new Vector2(x, y);
    }
}
```

---

## CameraDirector (Factory)

```csharp
public Camera CreateWorldCamera(string name, Vector2 origin)
{
    var camera = new WorldCamera(name, origin, this);
    Cameras.Add(camera);
    _viewportConfigs[camera] = CreateDefaultViewport();
    return camera;
}

public Camera CreateScreenCamera(string name, Vector2 origin)
{
    var camera = new ScreenCamera(name, origin, this);
    Cameras.Add(camera);
    _viewportConfigs[camera] = CreateDefaultViewport();
    return camera;
}

public Camera CreateHybridCamera(string name, Vector2 origin, float pixelsPerUnit)
{
    var camera = new HybridCamera(name, origin, this)
    {
        PixelsPerUnit = pixelsPerUnit
    };
    Cameras.Add(camera);
    _viewportConfigs[camera] = CreateDefaultViewport();
    return camera;
}
```

---

## Layer (Filtrado por Tipo de Cámara)

```csharp
public void CollectRenderCommands(CameraDirector cameraDirector)
{
    if (IsVisible == false) return;

    foreach (var camera in cameraDirector.Cameras)
    {
        uint cameraBit = (uint)(1 << cameraDirector.Cameras.IndexOf(camera));
        
        if ((CameraMask & cameraBit) == 0) continue;

        List<RenderCommand> commands = new List<RenderCommand>();

        foreach (var actor in _actors)
        {
            if (actor.IsActive == false) continue;
            
            // Actor decide si es visible según tipo de cámara
            if (actor.IsVisible(camera) == false) continue;
            
            // Actor genera comandos apropiados al tipo
            actor.CollectRenderCommands(camera, commands);
        }

        // Aplicar behaviors de capa
        var activeBehaviors = new List<ILayerBehavior>();
        foreach (var behavior in _behaviors)
        {
            if (behavior.IsActive && behavior.IsComplete == false)
            {
                activeBehaviors.Add(behavior);
            }
        }
        
        activeBehaviors.Sort((a, b) => a.Priority.CompareTo(b.Priority));

        foreach (var behavior in activeBehaviors)
        {
            behavior.Apply(commands, camera);
        }

        foreach (var command in commands)
        {
            camera.Submit(command);
        }
    }
}
```

---

## Actor (Adaptativo)

```csharp
public void CollectRenderCommands(Camera camera, List<RenderCommand> commands)
{
    if (IsActive == false) return;
    if (IsVisible == false) return;
    if (IsVisible(camera) == false) return;

    if (camera.Type == CameraType.Screen)
    {
        foreach (var command in GenerateScreenCommands())
        {
            commands.Add(command);
        }
    }
    else
    {
        foreach (var command in GenerateWorldCommands())
        {
            commands.Add(command);
        }
    }
}

protected abstract IEnumerable<RenderCommand> GenerateWorldCommands();
protected abstract IEnumerable<RenderCommand> GenerateScreenCommands();
```

---

## Resumen de Tipos

| Tipo | Coordenadas | Uso | Ejemplo |
|------|-------------|-----|---------|
| **World** | Unidades mundo | Juego, niveles | Personajes, mapas |
| **Screen** | 0-1 relativo | UI, HUD | Botones, barras vida |
| **Hybrid** | Mundo + escala fija | Marcadores, nombres | Nicknames sobre jugadores |

¿Ajustamos Fase 3 con este diseño o pasamos a Fase 4?
User: Repite los componentes que hemos cambiado de las fases 1, 2 y 3
Kimi: Aquí están todos los componentes modificados de las Fases 1, 2 y 3.

---

## FASE 1: Fundamentos (Modificados)

### 1. ICameraBehavior (Simplificado)

```csharp
namespace GameEngine.Camera
{
    public interface ICameraBehavior
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        float? Duration { get; }
        float ElapsedTime { get; }
        bool IsComplete { get; }
        
        void Update(float deltaTime);
        void Reset();
    }
}
```

### 2. CameraBehaviorBase (Simplificado)

```csharp
namespace GameEngine.Camera
{
    public abstract class CameraBehaviorBase : ICameraBehavior
    {
        protected readonly Camera Camera;
        
        public bool IsActive { get; set; } = true;
        public abstract int Priority { get; }
        
        public float? Duration { get; protected set; }
        public float ElapsedTime { get; private set; }
        public bool IsComplete { get; protected set; }

        protected CameraBehaviorBase(Camera camera)
        {
            Camera = camera;
        }

        public void Update(float deltaTime)
        {
            if (IsActive == false) return;
            if (IsComplete) return;

            ElapsedTime += deltaTime;

            if (Duration.HasValue && ElapsedTime >= Duration.Value)
            {
                IsComplete = true;
                OnComplete();
                return;
            }

            OnUpdate(deltaTime);
        }

        protected abstract void OnUpdate(float deltaTime);

        public virtual void Reset()
        {
            ElapsedTime = 0f;
            IsComplete = false;
            IsActive = true;
        }

        protected virtual void OnComplete() { }
    }
}
```

### 3. Camera (Actualizado con tipos y sin Viewport propio)

```csharp
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public enum CameraType
    {
        World,
        Screen,
        Hybrid
    }

    public abstract class Camera
    {
        protected readonly List<ICameraBehavior> _behaviors = new List<ICameraBehavior>();
        
        public string Name { get; }
        public CameraDirector Director { get; }
        public CameraType Type { get; }
        
        public float VirtualWidth { get; set; }
        public float VirtualHeight { get; set; }
        
        protected CameraState _state;
        private Matrix _viewMatrix;
        private bool _matrixDirty = true;

        public CameraState State 
        { 
            get => _state;
            protected set
            {
                _state = value;
                _matrixDirty = true;
            }
        }

        public Vector2 Position => State.Position;
        public float Rotation => State.Rotation;
        public float Zoom => State.Zoom;
        public Vector2 Origin => State.Origin;

        protected Camera(string name, CameraType type, Vector2 origin, CameraDirector director)
        {
            Name = name;
            Type = type;
            Director = director;
            _state = CameraState.Default(origin);
            VirtualWidth = origin.X * 2f;
            VirtualHeight = origin.Y * 2f;
        }

        public Matrix GetViewMatrix()
        {
            if (_matrixDirty)
            {
                _viewMatrix = CalculateViewMatrix();
                _matrixDirty = false;
            }
            return _viewMatrix;
        }

        protected abstract Matrix CalculateViewMatrix();
        public abstract Matrix GetProjectionMatrix(ViewportConfig viewport);
        
        public abstract bool IsInView(RectangleF bounds);
        public abstract bool IsInView(Vector2 point);
        
        public abstract Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport);
        public abstract Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport);

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                var behavior = _behaviors[i];
                
                if (behavior.IsActive == false) continue;
                
                behavior.Update(delta);
                
                if (behavior.IsComplete && behavior.Duration.HasValue)
                {
                    _behaviors.RemoveAt(i);
                }
            }
        }

        public void SetPosition(Vector2 position)
        {
            State = State.WithPosition(position);
        }

        public void SetRotation(float rotation)
        {
            State = State.WithRotation(rotation);
        }

        public void SetZoom(float zoom)
        {
            State = State.WithZoom(MathUtils.Clamp(zoom, 0.001f, 100f));
        }

        public void Move(Vector2 delta)
        {
            State = State.WithPosition(State.Position + delta);
        }

        public void Rotate(float deltaRadians)
        {
            State = State.WithRotation(State.Rotation + deltaRadians);
        }

        public void ZoomBy(float factor)
        {
            SetZoom(State.Zoom * factor);
        }

        public void AddBehavior(ICameraBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public void RemoveBehavior(ICameraBehavior behavior)
        {
            _behaviors.Remove(behavior);
        }

        public T GetBehavior<T>() where T : ICameraBehavior
        {
            return _behaviors.OfType<T>().FirstOrDefault();
        }

        public void ClearBehaviors()
        {
            _behaviors.Clear();
        }

        public override string ToString()
        {
            return $"Camera '{Name}' ({Type}): {State}";
        }
    }
}
```

### 4. WorldCamera (Nueva clase derivada)

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public class WorldCamera : Camera
    {
        public WorldCamera(string name, Vector2 origin, CameraDirector director)
            : base(name, CameraType.World, origin, director)
        {
        }

        protected override Matrix CalculateViewMatrix()
        {
            return State.GetViewMatrix();
        }

        public override Matrix GetProjectionMatrix(ViewportConfig viewport)
        {
            float targetAspect = viewport.Width / (float)viewport.Height;
            float virtualAspect = VirtualWidth / VirtualHeight;
            
            float viewWidth = VirtualWidth;
            float viewHeight = VirtualHeight;
            
            if (viewport.MaintainAspectRatio)
            {
                if (targetAspect > virtualAspect)
                {
                    viewWidth = VirtualHeight * targetAspect;
                }
                else
                {
                    viewHeight = VirtualWidth / targetAspect;
                }
            }
            
            return Matrix.CreateOrthographicOffCenter(
                0, viewWidth,
                viewHeight, 0,
                0f, 1f
            );
        }

        public override bool IsInView(RectangleF bounds)
        {
            var viewport = Director.GetViewport(this);
            if (viewport == null) return true;
            
            var visible = State.GetVisibleBounds(viewport.ToViewport());
            var world = Director.Scene.WorldBounds;
            var cullingBounds = RectangleF.Intersect(visible, world);
            
            return cullingBounds.IsEmpty == false && cullingBounds.Intersects(bounds);
        }

        public override bool IsInView(Vector2 point)
        {
            var viewport = Director.GetViewport(this);
            if (viewport == null) return true;
            
            var visible = State.GetVisibleBounds(viewport.ToViewport());
            var world = Director.Scene.WorldBounds;
            var cullingBounds = RectangleF.Intersect(visible, world);
            
            return cullingBounds.IsEmpty == false && cullingBounds.Contains(point);
        }

        public override Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix proj = GetProjectionMatrix(viewport);
            Vector3 transformed = Vector3.Transform(new Vector3(worldPos, 0f), view * proj);
            return new Vector2(transformed.X, transformed.Y);
        }

        public override Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix proj = GetProjectionMatrix(viewport);
            Matrix inverse = Matrix.Invert(view * proj);
            Vector3 transformed = Vector3.Transform(new Vector3(screenPos, 0f), inverse);
            return new Vector2(transformed.X, transformed.Y);
        }
    }
}
```

### 5. ScreenCamera (Nueva clase derivada)

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
{
    public class ScreenCamera : Camera
    {
        public ScreenCamera(string name, Vector2 origin, CameraDirector director)
            : base(name, CameraType.Screen, origin, director)
        {
        }

        protected override Matrix CalculateViewMatrix()
        {
            return Matrix.Identity;
        }

        public override Matrix GetProjectionMatrix(ViewportConfig viewport)
        {
            return Matrix.CreateOrthographicOffCenter(
                0, 1,
                1, 0,
                0f, 1f
            );
        }

        public override bool IsInView(RectangleF bounds)
        {
            return bounds.Left >= 0 && bounds.Right <= 1 &&
                   bounds.Top >= 0 && bounds.Bottom <= 1;
        }

        public override bool IsInView(Vector2 point)
        {
            return point.X >= 0 && point.X <= 1 &&
                   point.Y >= 0 && point.Y <= 1;
        }

        public override Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport)
        {
            return worldPos;
        }

        public override Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport)
        {
            return new Vector2(
                screenPos.X / viewport.Width,
                screenPos.Y / viewport.Height
            );
        }
    }
}
```

### 6. HybridCamera (Nueva clase derivada)

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public class HybridCamera : Camera
    {
        public float PixelsPerUnit { get; set; } = 100f;

        public HybridCamera(string name, Vector2 origin, CameraDirector director)
            : base(name, CameraType.Hybrid, origin, director)
        {
        }

        protected override Matrix CalculateViewMatrix()
        {
            return Matrix.CreateTranslation(-State.Position.X, -State.Position.Y, 0f);
        }

        public override Matrix GetProjectionMatrix(ViewportConfig viewport)
        {
            float halfWidth = viewport.Width / (2f * PixelsPerUnit);
            float halfHeight = viewport.Height / (2f * PixelsPerUnit);
            
            return Matrix.CreateOrthographicOffCenter(
                -halfWidth, halfWidth,
                halfHeight, -halfHeight,
                0f, 1f
            );
        }

        public override bool IsInView(RectangleF bounds)
        {
            var viewport = Director.GetViewport(this);
            if (viewport == null) return true;
            
            float halfWidth = viewport.Width / (2f * PixelsPerUnit);
            float halfHeight = viewport.Height / (2f * PixelsPerUnit);
            
            var visible = new RectangleF(
                State.Position.X - halfWidth,
                State.Position.Y - halfHeight,
                halfWidth * 2f,
                halfHeight * 2f
            );
            
            return visible.Intersects(bounds);
        }

        public override bool IsInView(Vector2 point)
        {
            var viewport = Director.GetViewport(this);
            if (viewport == null) return true;
            
            float halfWidth = viewport.Width / (2f * PixelsPerUnit);
            float halfHeight = viewport.Height / (2f * PixelsPerUnit);
            
            return point.X >= State.Position.X - halfWidth &&
                   point.X <= State.Position.X + halfWidth &&
                   point.Y >= State.Position.Y - halfHeight &&
                   point.Y <= State.Position.Y + halfHeight;
        }

        public override Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport)
        {
            Vector2 offset = worldPos - State.Position;
            return new Vector2(
                viewport.Width / 2f + offset.X * PixelsPerUnit,
                viewport.Height / 2f + offset.Y * PixelsPerUnit
            );
        }

        public override Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport)
        {
            float x = (screenPos.X - viewport.Width / 2f) / PixelsPerUnit;
            float y = (screenPos.Y - viewport.Height / 2f) / PixelsPerUnit;
            return State.Position + new Vector2(x, y);
        }
    }
}
```

### 7. ViewportConfig (Nuevo)

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
{
    public class ViewportConfig
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public float ScaleX { get; set; } = 1f;
        public float ScaleY { get; set; } = 1f;
        
        public bool MaintainAspectRatio { get; set; } = true;
        public Color LetterboxColor { get; set; } = Color.Black;

        public Viewport ToViewport()
        {
            return new Viewport(X, Y, Width, Height);
        }

        public Rectangle GetDestinationRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }
}
```

### 8. CameraDirector (Actualizado con factories y viewports)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Camera
{
    public class CameraDirector
    {
        public Scene.Scene Scene { get; }
        public List<Camera> Cameras { get; } = new List<Camera>();
        
        private readonly Dictionary<Camera, ViewportConfig> _viewportConfigs = new Dictionary<Camera, ViewportConfig>();
        private readonly Dictionary<Camera, RenderQueue> _renderQueues = new Dictionary<Camera, RenderQueue>();

        public CameraDirector(Scene.Scene scene)
        {
            Scene = scene;
        }

        private ViewportConfig CreateDefaultViewport()
        {
            return new ViewportConfig
            {
                X = 0,
                Y = 0,
                Width = 800,
                Height = 600
            };
        }

        public WorldCamera CreateWorldCamera(string name, Vector2 origin)
        {
            var camera = new WorldCamera(name, origin, this);
            Cameras.Add(camera);
            _viewportConfigs[camera] = CreateDefaultViewport();
            _renderQueues[camera] = new RenderQueue();
            return camera;
        }

        public ScreenCamera CreateScreenCamera(string name, Vector2 origin)
        {
            var camera = new ScreenCamera(name, origin, this);
            Cameras.Add(camera);
            _viewportConfigs[camera] = CreateDefaultViewport();
            _renderQueues[camera] = new RenderQueue();
            return camera;
        }

        public HybridCamera CreateHybridCamera(string name, Vector2 origin, float pixelsPerUnit)
        {
            var camera = new HybridCamera(name, origin, this)
            {
                PixelsPerUnit = pixelsPerUnit
            };
            Cameras.Add(camera);
            _viewportConfigs[camera] = CreateDefaultViewport();
            _renderQueues[camera] = new RenderQueue();
            return camera;
        }

        public void SetViewport(Camera camera, ViewportConfig config)
        {
            if (Cameras.Contains(camera))
            {
                _viewportConfigs[camera] = config;
            }
        }

        public ViewportConfig GetViewport(Camera camera)
        {
            if (_viewportConfigs.TryGetValue(camera, out var config))
            {
                return config;
            }
            return null;
        }

        public RenderQueue GetRenderQueue(Camera camera)
        {
            if (_renderQueues.TryGetValue(camera, out var queue))
            {
                return queue;
            }
            return null;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var camera in Cameras)
            {
                camera.Update(gameTime);
            }
        }

        public void Submit(Camera camera, RenderCommand command)
        {
            var queue = GetRenderQueue(camera);
            if (queue != null)
            {
                queue.Enqueue(command);
            }
        }

        public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            foreach (var camera in Cameras)
            {
                var viewport = _viewportConfigs[camera];
                var oldViewport = spriteBatch.GraphicsDevice.Viewport;
                
                spriteBatch.GraphicsDevice.Viewport = viewport.ToViewport();
                
                var queue = _renderQueues[camera];
                
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    camera.GetViewMatrix() * camera.GetProjectionMatrix(viewport)
                );

                queue.Execute(spriteBatch, defaultFont);
                
                spriteBatch.End();
                
                queue.Clear();
                
                spriteBatch.GraphicsDevice.Viewport = oldViewport;
            }
        }
    }
}
```

---

## FASE 2: Renderizado (Modificados)

### 9. RenderCommand (Abstracta)

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public abstract class RenderCommand
    {
        public RectangleF Bounds { get; set; }
        public Effect Effect { get; set; }
        
        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont);
    }
}
```

### 10. TextureRenderCommand (Sin LayerDepth)

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public class TextureRenderCommand : RenderCommand
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Rectangle? DestinationRectangle { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            if (Texture == null) return;
            
            if (DestinationRectangle.HasValue)
            {
                Rectangle dest = DestinationRectangle.Value;
                
                if (SourceRectangle.HasValue)
                {
                    spriteBatch.Draw(
                        Texture,
                        dest,
                        SourceRectangle.Value,
                        Color,
                        Rotation,
                        Origin,
                        Effects,
                        0f
                    );
                }
                else
                {
                    spriteBatch.Draw(
                        Texture,
                        dest,
                        null,
                        Color,
                        Rotation,
                        Origin,
                        Effects,
                        0f
                    );
                }
            }
            else
            {
                spriteBatch.Draw(
                    Texture,
                    Position,
                    SourceRectangle,
                    Color,
                    Rotation,
                    Origin,
                    Scale,
                    Effects,
                    0f
                );
            }
        }
    }
}
```

### 11. TextRenderCommand (Sin LayerDepth)

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public class TextRenderCommand : RenderCommand
    {
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            SpriteFont fontToUse = Font ?? defaultFont;
            
            if (fontToUse == null) return;
            if (string.IsNullOrEmpty(Text)) return;
            
            spriteBatch.DrawString(
                fontToUse,
                Text,
                Position,
                Color,
                Rotation,
                Origin,
                Scale,
                Effects,
                0f
            );
        }
    }
}
```

### 12. RenderQueue (Sin sorting por LayerDepth)

```csharp
using System.Collections.Generic;

namespace GameEngine.Rendering
{
    public class RenderQueue
    {
        private readonly List<RenderCommand> _commands = new List<RenderCommand>();

        public IReadOnlyList<RenderCommand> Commands => _commands;

        public void Clear()
        {
            _commands.Clear();
        }

        public void Enqueue(RenderCommand command)
        {
            _commands.Add(command);
        }

        public void EnqueueRange(IEnumerable<RenderCommand> commands)
        {
            foreach (var command in commands)
            {
                _commands.Add(command);
            }
        }

        public void Execute(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            foreach (var command in _commands)
            {
                command.Draw(spriteBatch, defaultFont);
            }
        }
    }
}
```

### 13. IRenderable (Actualizado con List)

```csharp
using System.Collections.Generic;
using GameEngine.Camera;

namespace GameEngine.Rendering
{
    public interface IRenderable
    {
        void CollectRenderCommands(Camera camera, List<RenderCommand> commands);
        RectangleF GetRenderBounds();
        bool IsVisible(Camera camera);
    }
}
```

### 14. Actor (Actualizado)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Camera;
using GameEngine.Rendering;

namespace GameEngine.Entities
{
    public abstract class Actor : IRenderable
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        
        protected Actor(Vector2 position)
        {
            Position = position;
        }

        public abstract void Update(float deltaTime);
        
        public void CollectRenderCommands(Camera camera, List<RenderCommand> commands)
        {
            if (IsActive == false) return;
            if (IsVisible == false) return;
            if (IsVisible(camera) == false) return;

            IEnumerable<RenderCommand> generatedCommands;
            
            if (camera.Type == CameraType.Screen)
            {
                generatedCommands = GenerateScreenCommands();
            }
            else
            {
                generatedCommands = GenerateWorldCommands();
            }
            
            foreach (var command in generatedCommands)
            {
                commands.Add(command);
            }
        }

        protected abstract IEnumerable<RenderCommand> GenerateWorldCommands();
        protected abstract IEnumerable<RenderCommand> GenerateScreenCommands();

        public abstract RectangleF GetRenderBounds();

        public virtual bool IsVisible(Camera camera)
        {
            return camera.IsInView(GetRenderBounds());
        }
    }
}
```

### 15. SpriteActor (Actualizado)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Rendering;

namespace GameEngine.Entities
{
    public class SpriteActor : Actor
    {
        public Texture2D Texture { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Color Tint { get; set; } = Color.White;
        public Vector2 Origin { get; set; }

        public SpriteActor(Vector2 position, Texture2D texture) 
            : base(position)
        {
            Texture = texture;
            Origin = texture.Bounds.Center.ToVector2();
        }

        public override void Update(float deltaTime)
        {
        }

        protected override IEnumerable<RenderCommand> GenerateWorldCommands()
        {
            if (Texture == null) 
            {
                yield break;
            }

            yield return new TextureRenderCommand
            {
                Texture = Texture,
                Position = Position,
                SourceRectangle = SourceRectangle,
                Color = Tint,
                Rotation = Rotation,
                Origin = Origin,
                Scale = Scale,
                Bounds = GetRenderBounds()
            };
        }

        protected override IEnumerable<RenderCommand> GenerateScreenCommands()
        {
            yield break;
        }

        public override RectangleF GetRenderBounds()
        {
            if (Texture == null) 
            {
                return new RectangleF(Position, Vector2.Zero);
            }

            Vector2 sourceSize;
            
            if (SourceRectangle.HasValue)
            {
                sourceSize = new Vector2(
                    SourceRectangle.Value.Width,
                    SourceRectangle.Value.Height
                );
            }
            else
            {
                sourceSize = Texture.Bounds.Size.ToVector2();
            }
            
            Vector2 size = sourceSize * Scale;
            Vector2 halfSize = size * 0.5f;
            Vector2 min = Position - halfSize;
            
            return new RectangleF(min, size);
        }
    }
}
```

### 16. TextActor (Actualizado)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Rendering;

namespace GameEngine.Entities
{
    public class TextActor : Actor
    {
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; } = Color.White;
        public Vector2 Origin { get; set; }

        public TextActor(Vector2 position, string text, SpriteFont font) 
            : base(position)
        {
            Text = text;
            Font = font;
        }

        public override void Update(float deltaTime)
        {
        }

        protected override IEnumerable<RenderCommand> GenerateWorldCommands()
        {
            yield break;
        }

        protected override IEnumerable<RenderCommand> GenerateScreenCommands()
        {
            if (string.IsNullOrEmpty(Text))
            {
                yield break;
            }

            yield return new TextRenderCommand
            {
                Text = Text,
                Font = Font,
                Position = Position,
                Color = Color,
                Rotation = Rotation,
                Origin = Origin,
                Scale = Scale,
                Bounds = GetRenderBounds()
            };
        }

        public override RectangleF GetRenderBounds()
        {
            if (Font == null || string.IsNullOrEmpty(Text))
            {
                return new RectangleF(Position, Vector2.Zero);
            }

            Vector2 size = Font.MeasureString(Text) * Scale;
            Vector2 halfSize = size * 0.5f;
            Vector2 min = Position - halfSize;
            
            return new RectangleF(min, size);
        }
    }
}
```

---

## FASE 3: Escena y Capas (Modificados)

### 17. ILayerBehavior

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers
{
    public interface ILayerBehavior
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        float? Duration { get; }
        float ElapsedTime { get; }
        bool IsComplete { get; }
        
        void Update(float deltaTime);
        void Apply(List<RenderCommand> commands, Camera camera);
        void Reset();
    }
}
```

### 18. LayerBehaviorBase

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers
{
    public abstract class LayerBehaviorBase : ILayerBehavior
    {
        protected readonly Layer Layer;
        
        public bool IsActive { get; set; } = true;
        public abstract int Priority { get; }
        
        public float? Duration { get; protected set; }
        public float ElapsedTime { get; private set; }
        public bool IsComplete { get; protected set; }

        protected LayerBehaviorBase(Layer layer)
        {
            Layer = layer;
        }

        public void Update(float deltaTime)
        {
            if (IsActive == false) return;
            if (IsComplete) return;

            ElapsedTime += deltaTime;

            if (Duration.HasValue && ElapsedTime >= Duration.Value)
            {
                IsComplete = true;
                OnComplete();
                return;
            }

            OnUpdate(deltaTime);
        }

        protected abstract void OnUpdate(float deltaTime);
        public abstract void Apply(List<RenderCommand> commands, Camera camera);

        public virtual void Reset()
        {
            ElapsedTime = 0f;
            IsComplete = false;
            IsActive = true;
        }

        protected virtual void OnComplete() { }
    }
}
```

### 19. Layer

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Entities;
using GameEngine.Camera;
using GameEngine.Rendering;

namespace GameEngine.Layers
{
    public class Layer
    {
        public string Name { get; }
        public int DrawOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        
        private readonly List<Actor> _actors = new List<Actor>();
        private readonly List<ILayerBehavior> _behaviors = new List<ILayerBehavior>();
        
        public IReadOnlyList<Actor> Actors => _actors;
        public IReadOnlyList<ILayerBehavior> Behaviors => _behaviors;
        
        public uint CameraMask { get; set; } = 0xFFFFFFFF;

        public Layer(string name, int drawOrder)
        {
            Name = name;
            DrawOrder = drawOrder;
        }

        public void AddActor(Actor actor)
        {
            _actors.Add(actor);
        }

        public void RemoveActor(Actor actor)
        {
            _actors.Remove(actor);
        }

        public void AddBehavior(ILayerBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public void RemoveBehavior(ILayerBehavior behavior)
        {
            _behaviors.Remove(behavior);
        }

        public void Update(float deltaTime)
        {
            if (IsActive == false) return;

            foreach (var actor in _actors)
            {
                actor.Update(deltaTime);
            }

            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                var behavior = _behaviors[i];
                
                if (behavior.IsActive == false) continue;
                
                behavior.Update(deltaTime);
                
                if (behavior.IsComplete && behavior.Duration.HasValue)
                {
                    _behaviors.RemoveAt(i);
                }
            }
        }

        public void CollectRenderCommands(CameraDirector cameraDirector)
        {
            if (IsVisible == false) return;

            List<Camera> targetCameras = new List<Camera>();
            
            for (int i = 0; i < cameraDirector.Cameras.Count; i++)
            {
                var camera = cameraDirector.Cameras[i];
                uint cameraBit = (uint)(1 << i);
                
                if ((CameraMask & cameraBit) != 0)
                {
                    targetCameras.Add(camera);
                }
            }

            foreach (var camera in targetCameras)
            {
                List<RenderCommand> commands = new List<RenderCommand>();

                foreach (var actor in _actors)
                {
                    if (actor.IsActive == false) continue;
                    if (actor.IsVisible(camera) == false) continue;
                    
                    actor.CollectRenderCommands(camera, commands);
                }

                List<ILayerBehavior> activeBehaviors = new List<ILayerBehavior>();
                
                foreach (var behavior in _behaviors)
                {
                    if (behavior.IsActive && behavior.IsComplete == false)
                    {
                        activeBehaviors.Add(behavior);
                    }
                }
                
                activeBehaviors.Sort((a, b) => a.Priority.CompareTo(b.Priority));

                foreach (var behavior in activeBehaviors)
                {
                    behavior.Apply(commands, camera);
                }

                foreach (var command in commands)
                {
                    cameraDirector.Submit(camera, command);
                }
            }
        }
    }
}
```

### 20. LayerDirector

```csharp
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Layers
{
    public class LayerDirector
    {
        private readonly List<Layer> _layers = new List<Layer>();

        public IReadOnlyList<Layer> Layers => _layers;

        public Layer CreateLayer(string name, int drawOrder)
        {
            var layer = new Layer(name, drawOrder);
            _layers.Add(layer);
            return layer;
        }

        public void RemoveLayer(Layer layer)
        {
            _layers.Remove(layer);
        }

        public void SortLayersByDrawOrder()
        {
            _layers.Sort((a, b) => a.DrawOrder.CompareTo(b.DrawOrder));
        }

        public void Update(float deltaTime)
        {
            foreach (var layer in _layers)
            {
                layer.Update(deltaTime);
            }
        }

        public void CollectRenderCommands(Camera.CameraDirector cameraDirector)
        {
            SortLayersByDrawOrder();
            
            foreach (var layer in _layers)
            {
                layer.CollectRenderCommands(cameraDirector);
            }
        }
    }
}
```

### 21. Scene (Actualizado)

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Camera;
using GameEngine.Layers;
using GameEngine.Math;

namespace GameEngine.Scene
{
    public class Scene
    {
        public RectangleF WorldBounds { get; set; }
        
        public CameraDirector CameraDirector { get; }
        public LayerDirector LayerDirector { get; }

        public Scene(RectangleF worldBounds)
        {
            WorldBounds = worldBounds;
            CameraDirector = new CameraDirector(this);
            LayerDirector = new LayerDirector();
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            CameraDirector.Update(gameTime);
            LayerDirector.Update(delta);
        }

        public void CollectRenderCommands()
        {
            LayerDirector.CollectRenderCommands(CameraDirector);
        }

        public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            CameraDirector.Render(spriteBatch, defaultFont);
        }
    }
}
```

---

¿Pasamos a **Fase 4: Behaviors Básicos**?
User: Pasemos a la fase 4
Kimi: Aquí está la **Fase 4: Behaviors Básicos**.

---

## Componentes de Fase 4

| Orden | Componente | Descripción |
|-------|-----------|-------------|
| 1 | **FollowBehavior** | Cámara sigue target con suavizado |
| 2 | **BoundsConstraintBehavior** | Restringe cámara a límites del mundo |
| 3 | **ParallaxBehavior** | Capa se mueve a velocidad diferente |
| 4 | **ScrollBehavior** | Capa con movimiento automático |
| 5 | **TintBehavior** | Color overlay en capa |
| 6 | **ShakeBehavior** | Vibración de cámara con trauma |

---

## 1. FollowBehavior (Cámara)

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class FollowBehavior : CameraBehaviorBase
    {
        public override int Priority => 10;
        
        public Vector2 Target { get; set; }
        public float SmoothSpeed { get; set; } = 5f;
        public Vector2 Offset { get; set; } = Vector2.Zero;
        
        public Vector2 DeadZone { get; set; } = Vector2.Zero;
        public bool UseDeadZone { get; set; } = false;
        
        private Vector2 _currentVelocity;

        public FollowBehavior(Camera camera) : base(camera)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            Vector2 targetPos = Target + Offset;
            Vector2 currentPos = Camera.Position;
            
            if (UseDeadZone)
            {
                Vector2 diff = targetPos - currentPos;
                
                if (System.Math.Abs(diff.X) < DeadZone.X * 0.5f)
                {
                    targetPos.X = currentPos.X;
                }
                else
                {
                    float sign = System.Math.Sign(diff.X);
                    targetPos.X -= sign * DeadZone.X * 0.5f;
                }
                
                if (System.Math.Abs(diff.Y) < DeadZone.Y * 0.5f)
                {
                    targetPos.Y = currentPos.Y;
                }
                else
                {
                    float sign = System.Math.Sign(diff.Y);
                    targetPos.Y -= sign * DeadZone.Y * 0.5f;
                }
            }
            
            Vector2 newPos = MathUtils.Lerp(currentPos, targetPos, SmoothSpeed * deltaTime);
            Camera.SetPosition(newPos);
        }
    }
}
```

---

## 2. BoundsConstraintBehavior (Cámara)

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class BoundsConstraintBehavior : CameraBehaviorBase
    {
        public override int Priority => 100;
        
        public bool SoftConstraint { get; set; } = false;
        public float Softness { get; set; } = 0.5f;

        public BoundsConstraintBehavior(Camera camera) : base(camera)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            var viewport = Camera.Director.GetViewport(Camera);
            if (viewport == null) return;
            
            var visible = Camera.GetVisibleBounds();
            var world = Camera.Director.Scene.WorldBounds;
            
            if (world.IsEmpty) return;
            
            float halfVisibleWidth = visible.Width * 0.5f;
            float halfVisibleHeight = visible.Height * 0.5f;
            
            Vector2 targetPos = Camera.Position;
            
            float minX = world.Left + halfVisibleWidth;
            float maxX = world.Right - halfVisibleWidth;
            float minY = world.Top + halfVisibleHeight;
            float maxY = world.Bottom - halfVisibleHeight;
            
            if (SoftConstraint)
            {
                if (targetPos.X < minX)
                {
                    float over = minX - targetPos.X;
                    targetPos.X = minX - over * Softness;
                }
                else if (targetPos.X > maxX)
                {
                    float over = targetPos.X - maxX;
                    targetPos.X = maxX + over * Softness;
                }
                
                if (targetPos.Y < minY)
                {
                    float over = minY - targetPos.Y;
                    targetPos.Y = minY - over * Softness;
                }
                else if (targetPos.Y > maxY)
                {
                    float over = targetPos.Y - maxY;
                    targetPos.Y = maxY + over * Softness;
                }
            }
            else
            {
                targetPos.X = MathUtils.Clamp(targetPos.X, minX, maxX);
                targetPos.Y = MathUtils.Clamp(targetPos.Y, minY, maxY);
            }
            
            Camera.SetPosition(targetPos);
        }
    }
}
```

---

## 3. ParallaxBehavior (Capa)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class ParallaxBehavior : LayerBehaviorBase
    {
        public override int Priority => 10;
        
        public Vector2 Factor { get; set; } = new Vector2(0.5f, 0.5f);
        public Vector2 BasePosition { get; set; } = Vector2.Zero;

        public ParallaxBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            Vector2 cameraPos = camera.Position;
            Vector2 parallaxOffset = BasePosition - cameraPos * Factor;
            Vector2 delta = parallaxOffset - BasePosition + cameraPos;
            
            foreach (var command in commands)
            {
                command.Bounds = new RectangleF(
                    command.Bounds.Position + delta,
                    command.Bounds.Size
                );
                
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Position += delta;
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Position += delta;
                }
            }
        }
    }
}
```

---

## 4. ScrollBehavior (Capa)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class ScrollBehavior : LayerBehaviorBase
    {
        public override int Priority => 20;
        
        public Vector2 Speed { get; set; } = Vector2.Zero;
        public bool Wrap { get; set; } = false;
        public Vector2 WrapBounds { get; set; } = Vector2.Zero;
        
        private Vector2 _currentOffset;

        public ScrollBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            _currentOffset += Speed * deltaTime;
            
            if (Wrap && WrapBounds != Vector2.Zero)
            {
                _currentOffset.X = _currentOffset.X % WrapBounds.X;
                _currentOffset.Y = _currentOffset.Y % WrapBounds.Y;
                
                if (_currentOffset.X < 0) _currentOffset.X += WrapBounds.X;
                if (_currentOffset.Y < 0) _currentOffset.Y += WrapBounds.Y;
            }
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            foreach (var command in commands)
            {
                command.Bounds = new RectangleF(
                    command.Bounds.Position + _currentOffset,
                    command.Bounds.Size
                );
                
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Position += _currentOffset;
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Position += _currentOffset;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            _currentOffset = Vector2.Zero;
        }
    }
}
```

---

## 5. TintBehavior (Capa)

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class TintBehavior : LayerBehaviorBase
    {
        public override int Priority => 50;
        
        public Color Tint { get; set; } = Color.White;
        public float Intensity { get; set; } = 1f;

        public TintBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            Color blended = new Color(
                (byte)(Tint.R * Intensity),
                (byte)(Tint.G * Intensity),
                (byte)(Tint.B * Intensity),
                (byte)(Tint.A * Intensity)
            );
            
            foreach (var command in commands)
            {
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Color = BlendColors(texCmd.Color, blended);
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Color = BlendColors(textCmd.Color, blended);
                }
            }
        }

        private Color BlendColors(Color baseColor, Color tint)
        {
            return new Color(
                (byte)((baseColor.R * tint.R) / 255),
                (byte)((baseColor.G * tint.G) / 255),
                (byte)((baseColor.B * tint.B) / 255),
                (byte)((baseColor.A * tint.A) / 255)
            );
        }
    }
}
```

---

## 6. ShakeBehavior (Cámara)

```csharp
using System;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class ShakeBehavior : CameraBehaviorBase
    {
        public override int Priority => 90;
        
        public float Intensity { get; set; } = 10f;
        public float Frequency { get; set; } = 20f;
        
        private float _currentTrauma;
        private Vector2 _originalPosition;
        private Random _random;
        private float _seed;

        public ShakeBehavior(Camera camera, float duration, float intensity) : base(camera)
        {
            Duration = duration;
            Intensity = intensity;
            _random = new Random();
            _seed = (float)_random.NextDouble() * 100f;
            _originalPosition = camera.Position;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (ElapsedTime == 0f)
            {
                _originalPosition = Camera.Position;
            }
            
            float t = ElapsedTime / Duration.Value;
            _currentTrauma = MathUtils.Clamp01(1f - t);
            _currentTrauma = _currentTrauma * _currentTrauma;
            
            float shake = _currentTrauma * Intensity;
            
            float noiseX = PerlinNoise(_seed + ElapsedTime * Frequency);
            float noiseY = PerlinNoise(_seed + 100f + ElapsedTime * Frequency);
            
            float offsetX = noiseX * shake;
            float offsetY = noiseY * shake;
            
            Camera.SetPosition(_originalPosition + new Vector2(offsetX, offsetY));
        }

        protected override void OnComplete()
        {
            Camera.SetPosition(_originalPosition);
        }

        private float PerlinNoise(float x)
        {
            return (float)(System.Math.Sin(x) * 0.5 + System.Math.Sin(x * 2.3) * 0.25 + System.Math.Sin(x * 4.7) * 0.125);
        }

        public override void Reset()
        {
            base.Reset();
            _seed = (float)_random.NextDouble() * 100f;
        }
    }
}
```

---

## Ejemplo de Uso

```csharp
// Setup escena
var scene = new Scene(new RectangleF(0, 0, 2000, 1500));

// Cámara principal
var mainCam = scene.CameraDirector.CreateWorldCamera("Main", new Vector2(400, 300));
mainCam.AddBehavior(new FollowBehavior(mainCam)
{
    SmoothSpeed = 3f,
    DeadZone = new Vector2(100, 50),
    UseDeadZone = true
});
mainCam.AddBehavior(new BoundsConstraintBehavior(mainCam));

// Capa de fondo con parallax
var bgLayer = scene.LayerDirector.CreateLayer("Background", 0);
bgLayer.AddBehavior(new ParallaxBehavior(bgLayer)
{
    Factor = new Vector2(0.3f, 0.3f)
});
bgLayer.AddBehavior(new ScrollBehavior(bgLayer)
{
    Speed = new Vector2(-10, 0),
    Wrap = true,
    WrapBounds = new Vector2(800, 0)
});

// Capa de gameplay
var gameLayer = scene.LayerDirector.CreateLayer("Gameplay", 10);
gameLayer.CameraMask = 1 << 0; // Solo cámara principal

// Shake al recibir daño
mainCam.AddBehavior(new ShakeBehavior(mainCam, 0.5f, 15f));
```

---

¿Pasamos a **Fase 5: Behaviors Avanzados** o añadimos más behaviors básicos?
User: El comportamiento que sigue un objetivo debe permitir seguir a varios cambiando el zoom de cámara hasta un máximo
Kimi: Aquí está el **MultiTargetFollowBehavior** con ajuste de zoom automático.

---

## MultiTargetFollowBehavior

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class MultiTargetFollowBehavior : CameraBehaviorBase
    {
        public override int Priority => 15;
        
        public List<Vector2> Targets { get; } = new List<Vector2>();
        public float SmoothSpeed { get; set; } = 3f;
        public Vector2 Offset { get; set; } = Vector2.Zero;
        
        public float MinZoom { get; set; } = 0.5f;
        public float MaxZoom { get; set; } = 2f;
        public float ZoomMargin { get; set; } = 100f;
        public float ZoomSmoothSpeed { get; set; } = 2f;
        
        private float _currentZoom;
        private Vector2 _currentCenter;

        public MultiTargetFollowBehavior(Camera camera) : base(camera)
        {
            _currentZoom = camera.Zoom;
            _currentCenter = camera.Position;
        }

        public void AddTarget(Vector2 target)
        {
            Targets.Add(target);
        }

        public void RemoveTarget(Vector2 target)
        {
            Targets.Remove(target);
        }

        public void ClearTargets()
        {
            Targets.Clear();
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (Targets.Count == 0) return;
            
            RectangleF bounds = CalculateTargetsBounds();
            Vector2 center = bounds.Center + Offset;
            
            float requiredZoom = CalculateRequiredZoom(bounds);
            requiredZoom = MathUtils.Clamp(requiredZoom, MinZoom, MaxZoom);
            
            _currentCenter = MathUtils.Lerp(_currentCenter, center, SmoothSpeed * deltaTime);
            _currentZoom = MathUtils.Lerp(_currentZoom, requiredZoom, ZoomSmoothSpeed * deltaTime);
            
            Camera.SetPosition(_currentCenter);
            Camera.SetZoom(_currentZoom);
        }

        private RectangleF CalculateTargetsBounds()
        {
            if (Targets.Count == 0) return RectangleF.Empty;
            
            float minX = Targets[0].X;
            float maxX = Targets[0].X;
            float minY = Targets[0].Y;
            float maxY = Targets[0].Y;
            
            for (int i = 1; i < Targets.Count; i++)
            {
                Vector2 target = Targets[i];
                
                if (target.X < minX) minX = target.X;
                if (target.X > maxX) maxX = target.X;
                if (target.Y < minY) minY = target.Y;
                if (target.Y > maxY) maxY = target.Y;
            }
            
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        private float CalculateRequiredZoom(RectangleF bounds)
        {
            var viewport = Camera.Director.GetViewport(Camera);
            if (viewport == null) return 1f;
            
            float viewWidth = viewport.Width;
            float viewHeight = viewport.Height;
            
            float contentWidth = bounds.Width + ZoomMargin * 2f;
            float contentHeight = bounds.Height + ZoomMargin * 2f;
            
            if (contentWidth <= 0 || contentHeight <= 0) return MaxZoom;
            
            float zoomX = viewWidth / contentWidth;
            float zoomY = viewHeight / contentHeight;
            
            return System.Math.Min(zoomX, zoomY);
        }

        public override void Reset()
        {
            base.Reset();
            Targets.Clear();
            _currentZoom = Camera.Zoom;
            _currentCenter = Camera.Position;
        }
    }
}
```

---

## Ejemplo de Uso

```csharp
var multiFollow = new MultiTargetFollowBehavior(mainCam)
{
    SmoothSpeed = 2.5f,
    MinZoom = 0.8f,
    MaxZoom = 1.5f,
    ZoomMargin = 150f,
    ZoomSmoothSpeed = 1.5f
};

multiFollow.AddTarget(player1.Position);
multiFollow.AddTarget(player2.Position);
multiFollow.AddTarget(player3.Position);

mainCam.AddBehavior(multiFollow);

// En update del juego
multiFollow.Targets[0] = player1.Position;
multiFollow.Targets[1] = player2.Position;
multiFollow.Targets[2] = player3.Position;

// Al morir un jugador
multiFollow.RemoveTarget(deadPlayer.Position);
```

---

## Combinación con BoundsConstraint

```csharp
mainCam.AddBehavior(multiFollow);
mainCam.AddBehavior(new BoundsConstraintBehavior(mainCam)
{
    SoftConstraint = true,
    Softness = 0.8f
});
```

El `BoundsConstraint` con `Priority = 100` se aplica después del `MultiTargetFollow` con `Priority = 15`, corrigiendo si el zoom requerido sale de los límites del mundo.

---

¿Pasamos a **Fase 5** o añadimos más behaviors?
User: Añade otros comportamientos interesantes
Kimi: Aquí están behaviors adicionales interesantes para cámara y capas.

---

## CÁMARA: RecoilBehavior

Retroceso por disparo/impacto con recuperación suave.

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class RecoilBehavior : CameraBehaviorBase
    {
        public override int Priority => 80;
        
        public Vector2 Direction { get; set; } = Vector2.Zero;
        public float MaxRecoil { get; set; } = 20f;
        public float RecoverySpeed { get; set; } = 10f;
        public float Decay { get; set; } = 0.9f;
        
        private Vector2 _currentRecoil;
        private Vector2 _originalPosition;

        public RecoilBehavior(Camera camera) : base(camera)
        {
        }

        public void AddRecoil(float amount, Vector2 direction)
        {
            Vector2 newRecoil = direction * amount;
            _currentRecoil += newRecoil;
            
            float length = _currentRecoil.Length();
            if (length > MaxRecoil)
            {
                _currentRecoil = _currentRecoil / length * MaxRecoil;
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (ElapsedTime == 0f)
            {
                _originalPosition = Camera.Position;
            }
            
            _currentRecoil *= Decay;
            
            Vector2 targetPos = _originalPosition + _currentRecoil;
            Camera.SetPosition(targetPos);
            
            _originalPosition = MathUtils.Lerp(
                _originalPosition, 
                Camera.Position - _currentRecoil, 
                RecoverySpeed * deltaTime
            );
            
            if (_currentRecoil.LengthSquared() < 0.01f)
            {
                IsComplete = true;
            }
        }

        protected override void OnComplete()
        {
            Camera.SetPosition(_originalPosition);
        }

        public override void Reset()
        {
            base.Reset();
            _currentRecoil = Vector2.Zero;
        }
    }
}
```

---

## CÁMARA: DollyZoomBehavior

Efecto Hitchcock: zoom in + alejar cámara simultáneamente.

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class DollyZoomBehavior : CameraBehaviorBase
    {
        public override int Priority => 20;
        
        public Vector2 Target { get; set; }
        public float StartDistance { get; set; } = 300f;
        public float EndDistance { get; set; } = 150f;
        public float StartZoom { get; set; } = 1f;
        public float EndZoom { get; set; } = 2f;
        
        private Vector2 _startPosition;
        private float _initialZoom;

        public DollyZoomBehavior(Camera camera, float duration) : base(camera)
        {
            Duration = duration;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (ElapsedTime == 0f)
            {
                _startPosition = Camera.Position;
                _initialZoom = Camera.Zoom;
            }
            
            float t = ElapsedTime / Duration.Value;
            t = MathUtils.SmoothStep(0f, 1f, t);
            
            float currentDistance = MathUtils.Lerp(StartDistance, EndDistance, t);
            float currentZoom = MathUtils.Lerp(StartZoom, EndZoom, t);
            
            Vector2 toTarget = Target - _startPosition;
            toTarget.Normalize();
            
            Vector2 newPos = Target - toTarget * currentDistance;
            
            Camera.SetPosition(newPos);
            Camera.SetZoom(currentZoom);
        }

        protected override void OnComplete()
        {
            Camera.SetZoom(EndZoom);
        }

        public override void Reset()
        {
            base.Reset();
            Camera.SetZoom(_initialZoom);
        }
    }
}
```

---

## CÁMARA: CinematicPathBehavior

Movimiento a lo largo de curva Bezier.

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class CinematicPathBehavior : CameraBehaviorBase
    {
        public override int Priority => 5;
        
        public Vector2[] ControlPoints { get; set; }
        public float[] WaitTimes { get; set; }
        public bool Loop { get; set; } = false;
        
        private int _currentSegment;
        private float _segmentProgress;
        private float _waitTimer;

        public CinematicPathBehavior(Camera camera) : base(camera)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (ControlPoints == null || ControlPoints.Length < 2) return;
            
            if (_waitTimer > 0)
            {
                _waitTimer -= deltaTime;
                return;
            }
            
            int segments = ControlPoints.Length - 1;
            float segmentDuration = Duration.Value / segments;
            
            _segmentProgress += deltaTime / segmentDuration;
            
            while (_segmentProgress >= 1f && _currentSegment < segments)
            {
                _segmentProgress -= 1f;
                _currentSegment++;
                
                if (WaitTimes != null && _currentSegment < WaitTimes.Length)
                {
                    _waitTimer = WaitTimes[_currentSegment];
                }
            }
            
            if (_currentSegment >= segments)
            {
                if (Loop)
                {
                    _currentSegment = 0;
                    _segmentProgress = 0f;
                }
                else
                {
                    IsComplete = true;
                    return;
                }
            }
            
            Vector2 p0 = ControlPoints[System.Math.Max(0, _currentSegment - 1)];
            Vector2 p1 = ControlPoints[_currentSegment];
            Vector2 p2 = ControlPoints[System.Math.Min(ControlPoints.Length - 1, _currentSegment + 1)];
            Vector2 p3 = ControlPoints[System.Math.Min(ControlPoints.Length - 1, _currentSegment + 2)];
            
            Vector2 newPos = CatmullRom(p0, p1, p2, p3, _segmentProgress);
            Camera.SetPosition(newPos);
        }

        private Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;
            
            return 0.5f * (
                (2f * p1) +
                (-p0 + p2) * t +
                (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
                (-p0 + 3f * p1 - 3f * p2 + p3) * t3
            );
        }

        public override void Reset()
        {
            base.Reset();
            _currentSegment = 0;
            _segmentProgress = 0f;
            _waitTimer = 0f;
        }
    }
}
```

---

## CAPA: WaveDistortionBehavior

Onda sinusoidal en posición Y (efecto agua/heat).

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class WaveDistortionBehavior : LayerBehaviorBase
    {
        public override int Priority => 30;
        
        public float Amplitude { get; set; } = 5f;
        public float Frequency { get; set; } = 0.1f;
        public float Speed { get; set; } = 2f;
        public bool Vertical { get; set; } = false;
        
        private float _phase;

        public WaveDistortionBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            _phase += Speed * deltaTime;
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            foreach (var command in commands)
            {
                float pos = Vertical ? command.Bounds.Position.Y : command.Bounds.Position.X;
                float wave = System.MathF.Sin(pos * Frequency + _phase) * Amplitude;
                
                Vector2 offset = Vertical ? new Vector2(0, wave) : new Vector2(wave, 0);
                
                command.Bounds = new RectangleF(
                    command.Bounds.Position + offset,
                    command.Bounds.Size
                );
                
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Position += offset;
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Position += offset;
                }
            }
        }
    }
}
```

---

## CAPA: FlashBehavior

Flash de color momentáneo (daño, power-up).

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class FlashBehavior : LayerBehaviorBase
    {
        public override int Priority => 60;
        
        public Color FlashColor { get; set; } = Color.White;
        public float FadeOutSpeed { get; set; } = 5f;
        
        private float _currentIntensity;

        public FlashBehavior(Layer layer, float duration) : base(layer)
        {
            Duration = duration;
            _currentIntensity = 1f;
        }

        protected override void OnUpdate(float deltaTime)
        {
            _currentIntensity -= FadeOutSpeed * deltaTime;
            
            if (_currentIntensity <= 0)
            {
                _currentIntensity = 0;
                IsComplete = true;
            }
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            Color flash = new Color(
                (byte)(FlashColor.R * _currentIntensity),
                (byte)(FlashColor.G * _currentIntensity),
                (byte)(FlashColor.B * _currentIntensity),
                (byte)(255 * _currentIntensity)
            );
            
            foreach (var command in commands)
            {
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Color = BlendAdditive(texCmd.Color, flash);
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Color = BlendAdditive(textCmd.Color, flash);
                }
            }
        }

        private Color BlendAdditive(Color baseColor, Color blend)
        {
            return new Color(
                (byte)System.Math.Min(255, baseColor.R + blend.R),
                (byte)System.Math.Min(255, baseColor.G + blend.G),
                (byte)System.Math.Min(255, baseColor.B + blend.B),
                baseColor.A
            );
        }
    }
}
```

---

## CAPA: MaskBehavior

Máscara circular o rectangular.

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class MaskBehavior : LayerBehaviorBase
    {
        public override int Priority => 70;
        
        public enum MaskShape { Rectangle, Circle, Ellipse }
        
        public MaskShape Shape { get; set; } = MaskShape.Circle;
        public Vector2 Center { get; set; }
        public float Radius { get; set; } = 100f;
        public Vector2 Size { get; set; } = new Vector2(200, 200);
        public bool Invert { get; set; } = false;
        public float Softness { get; set; } = 0f;

        public MaskBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                var command = commands[i];
                bool inside = IsInside(command.Bounds.Center);
                
                if (Invert)
                {
                    inside = !inside;
                }
                
                if (inside == false)
                {
                    if (Softness > 0 && IsNearEdge(command.Bounds.Center))
                    {
                        ApplySoftness(command);
                    }
                    else
                    {
                        commands.RemoveAt(i);
                    }
                }
            }
        }

        private bool IsInside(Vector2 point)
        {
            switch (Shape)
            {
                case MaskShape.Rectangle:
                    float halfW = Size.X * 0.5f;
                    float halfH = Size.Y * 0.5f;
                    return point.X >= Center.X - halfW && point.X <= Center.X + halfW &&
                           point.Y >= Center.Y - halfH && point.Y <= Center.Y + halfH;
                           
                case MaskShape.Circle:
                    return (point - Center).LengthSquared() <= Radius * Radius;
                    
                case MaskShape.Ellipse:
                    float dx = (point.X - Center.X) / (Size.X * 0.5f);
                    float dy = (point.Y - Center.Y) / (Size.Y * 0.5f);
                    return dx * dx + dy * dy <= 1f;
            }
            
            return false;
        }

        private bool IsNearEdge(Vector2 point)
        {
            // Simplificado: verificar distancia al borde
            return true;
        }

        private void ApplySoftness(RenderCommand command)
        {
            if (command is TextureRenderCommand texCmd)
            {
                texCmd.Color = new Color(
                    texCmd.Color.R,
                    texCmd.Color.G,
                    texCmd.Color.B,
                    (byte)(texCmd.Color.A * 0.5f)
                );
            }
        }
    }
}
```

---

## Ejemplos de Uso

```csharp
// Recoil al disparar
var recoil = new RecoilBehavior(mainCam);
recoil.AddRecoil(15f, new Vector2(-1, 0.3f)); // Hacia atrás y arriba
mainCam.AddBehavior(recoil);

// Dolly zoom dramático
var dolly = new DollyZoomBehavior(mainCam, 2f)
{
    Target = bossPosition,
    StartDistance = 400f,
    EndDistance = 100f,
    StartZoom = 0.8f,
    EndZoom = 2.5f
};
mainCam.AddBehavior(dolly);

// Path cinemático
var path = new CinematicPathBehavior(mainCam)
{
    ControlPoints = new[] { start, mid1, mid2, end },
    WaitTimes = new[] { 0f, 1f, 0.5f, 0f },
    Loop = false
};
mainCam.AddBehavior(path);

// Onda en capa de agua
var wave = new WaveDistortionBehavior(waterLayer)
{
    Amplitude = 8f,
    Frequency = 0.05f,
    Speed = 3f,
    Vertical = false
};
waterLayer.AddBehavior(wave);

// Flash de daño
var flash = new FlashBehavior(gameLayer, 0.3f)
{
    FlashColor = Color.Red,
    FadeOutSpeed = 8f
};
gameLayer.AddBehavior(flash);

// Máscara de linterna
var mask = new MaskBehavior(darknessLayer)
{
    Shape = MaskBehavior.MaskShape.Circle,
    Center = player.Position,
    Radius = 150f,
    Invert = true, // Todo oscuro excepto círculo
    Softness = 20f
};
darknessLayer.AddBehavior(mask);
```

---

¿Pasamos a **Fase 5: Integración y Optimización**?
User: Pasemos a la fase 5
Kimi: Aquí está la **Fase 5: Integración y Optimización**.

---

## Componentes de Fase 5

| Orden | Componente | Descripción |
|-------|-----------|-------------|
| 1 | **CameraManager** | Gestión global de cámaras activas/inactivas |
| 2 | **RenderTargetPool** | Pool de RenderTargets para post-proceso |
| 3 | **PostProcessStack** | Efectos de post-procesado por cámara |
| 4 | **TransitionSystem** | Transiciones suaves entre cámaras |
| 5 | **CullingOptimizer** | Spatial hashing para culling masivo |
| 6 | **BehaviorDebugger** | Visualización de bounds y comportamientos |

---

## 1. CameraManager

Gestiona estado global de cámaras, prioridades y activación/desactivación.

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
{
    public class CameraManager
    {
        private readonly List<Camera> _activeCameras = new List<Camera>();
        private readonly List<Camera> _inactiveCameras = new List<Camera>();
        private readonly Dictionary<string, Camera> _cameraRegistry = new Dictionary<string, Camera>();

        public IReadOnlyList<Camera> ActiveCameras => _activeCameras;

        public void RegisterCamera(Camera camera)
        {
            _cameraRegistry[camera.Name] = camera;
            
            if (camera.IsActive)
            {
                _activeCameras.Add(camera);
                SortActiveCameras();
            }
            else
            {
                _inactiveCameras.Add(camera);
            }
        }

        public void UnregisterCamera(Camera camera)
        {
            _cameraRegistry.Remove(camera.Name);
            _activeCameras.Remove(camera);
            _inactiveCameras.Remove(camera);
        }

        public void ActivateCamera(string name)
        {
            if (_cameraRegistry.TryGetValue(name, out var camera))
            {
                if (_inactiveCameras.Contains(camera))
                {
                    _inactiveCameras.Remove(camera);
                    _activeCameras.Add(camera);
                    camera.IsActive = true;
                    SortActiveCameras();
                }
            }
        }

        public void DeactivateCamera(string name)
        {
            if (_cameraRegistry.TryGetValue(name, out var camera))
            {
                if (_activeCameras.Contains(camera))
                {
                    _activeCameras.Remove(camera);
                    _inactiveCameras.Add(camera);
                    camera.IsActive = false;
                }
            }
        }

        public void SetCameraPriority(string name, int priority)
        {
            if (_cameraRegistry.TryGetValue(name, out var camera))
            {
                camera.Priority = priority;
                SortActiveCameras();
            }
        }

        private void SortActiveCameras()
        {
            _activeCameras.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }

        public Camera GetCamera(string name)
        {
            _cameraRegistry.TryGetValue(name, out var camera);
            return camera;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var camera in _activeCameras)
            {
                camera.Update(gameTime);
            }
        }
    }
}
```

---

## 2. RenderTargetPool

Reutilización de RenderTargets para evitar GC.

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public class RenderTargetPool
    {
        private readonly GraphicsDevice _device;
        private readonly Dictionary<RenderTargetFormat, Queue<RenderTarget2D>> _pools = 
            new Dictionary<RenderTargetFormat, Queue<RenderTarget2D>>();

        private struct RenderTargetFormat
        {
            public int Width;
            public int Height;
            public SurfaceFormat Format;

            public bool Equals(RenderTargetFormat other)
            {
                return Width == other.Width && 
                       Height == other.Height && 
                       Format == other.Format;
            }
        }

        public RenderTargetPool(GraphicsDevice device)
        {
            _device = device;
        }

        public RenderTarget2D Acquire(int width, int height, SurfaceFormat format = SurfaceFormat.Color)
        {
            var key = new RenderTargetFormat { Width = width, Height = height, Format = format };
            
            if (_pools.TryGetValue(key, out var pool) && pool.Count > 0)
            {
                return pool.Dequeue();
            }

            return new RenderTarget2D(
                _device,
                width,
                height,
                false,
                format,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents
            );
        }

        public void Release(RenderTarget2D target)
        {
            if (target == null || target.IsDisposed) return;

            var key = new RenderTargetFormat 
            { 
                Width = target.Width, 
                Height = target.Height, 
                Format = target.Format 
            };

            if (_pools.TryGetValue(key, out var pool) == false)
            {
                pool = new Queue<RenderTarget2D>();
                _pools[key] = pool;
            }

            pool.Enqueue(target);
        }

        public void Clear()
        {
            foreach (var pool in _pools.Values)
            {
                while (pool.Count > 0)
                {
                    pool.Dequeue().Dispose();
                }
            }
            _pools.Clear();
        }
    }
}
```

---

## 3. PostProcessStack

Efectos aplicados después del render de la cámara.

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering.PostProcess
{
    public interface IPostProcessEffect
    {
        bool IsEnabled { get; set; }
        void Apply(SpriteBatch spriteBatch, RenderTarget2D source, RenderTarget2D destination);
    }

    public class PostProcessStack
    {
        private readonly List<IPostProcessEffect> _effects = new List<IPostProcessEffect>();
        private readonly RenderTargetPool _targetPool;
        private readonly GraphicsDevice _device;

        public PostProcessStack(GraphicsDevice device, RenderTargetPool targetPool)
        {
            _device = device;
            _targetPool = targetPool;
        }

        public void AddEffect(IPostProcessEffect effect)
        {
            _effects.Add(effect);
        }

        public void RemoveEffect(IPostProcessEffect effect)
        {
            _effects.Remove(effect);
        }

        public RenderTarget2D Process(SpriteBatch spriteBatch, RenderTarget2D source, int targetWidth, int targetHeight)
        {
            if (_effects.Count == 0) return source;

            RenderTarget2D current = source;
            RenderTarget2D next = null;

            for (int i = 0; i < _effects.Count; i++)
            {
                var effect = _effects[i];
                
                if (effect.IsEnabled == false) continue;

                bool isLast = (i == _effects.Count - 1);
                
                if (isLast == false)
                {
                    next = _targetPool.Acquire(targetWidth, targetHeight);
                }

                _device.SetRenderTarget(isLast ? null : next);
                effect.Apply(spriteBatch, current, isLast ? null : next);

                if (current != source)
                {
                    _targetPool.Release(current);
                }

                current = next;
            }

            return current;
        }
    }
}
```

---

## 4. TransitionSystem

Transiciones entre cámaras o escenas.

```csharp
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Transitions
{
    public enum TransitionType
    {
        Fade,
        WipeLeft,
        WipeRight,
        WipeUp,
        WipeDown,
        Circle,
        Pixelate
    }

    public class TransitionSystem
    {
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        
        public bool IsTransitioning { get; private set; }
        public float Progress { get; private set; }
        public TransitionType CurrentType { get; private set; }
        
        private RenderTarget2D _fromTarget;
        private RenderTarget2D _toTarget;
        private float _duration;
        private Action _onComplete;
        private Effect _transitionEffect;

        public TransitionSystem(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            _device = device;
            _spriteBatch = spriteBatch;
        }

        public void StartTransition(
            RenderTarget2D from, 
            RenderTarget2D to, 
            TransitionType type, 
            float duration,
            Action onComplete = null)
        {
            _fromTarget = from;
            _toTarget = to;
            CurrentType = type;
            _duration = duration;
            _onComplete = onComplete;
            Progress = 0f;
            IsTransitioning = true;
        }

        public void Update(GameTime gameTime)
        {
            if (IsTransitioning == false) return;

            Progress += (float)gameTime.ElapsedGameTime.TotalSeconds / _duration;
            
            if (Progress >= 1f)
            {
                Progress = 1f;
                IsTransitioning = false;
                _onComplete?.Invoke();
            }
        }

        public void Draw()
        {
            if (IsTransitioning == false) return;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

            switch (CurrentType)
            {
                case TransitionType.Fade:
                    DrawFade();
                    break;
                case TransitionType.WipeLeft:
                    DrawWipe(new Vector2(-1, 0));
                    break;
                case TransitionType.WipeRight:
                    DrawWipe(new Vector2(1, 0));
                    break;
                case TransitionType.WipeUp:
                    DrawWipe(new Vector2(0, -1));
                    break;
                case TransitionType.WipeDown:
                    DrawWipe(new Vector2(0, 1));
                    break;
                case TransitionType.Circle:
                    DrawCircle();
                    break;
                case TransitionType.Pixelate:
                    DrawPixelate();
                    break;
            }

            _spriteBatch.End();
        }

        private void DrawFade()
        {
            float fromAlpha = 1f - Progress;
            float toAlpha = Progress;

            _spriteBatch.Draw(_fromTarget, Vector2.Zero, Color.White * fromAlpha);
            _spriteBatch.Draw(_toTarget, Vector2.Zero, Color.White * toAlpha);
        }

        private void DrawWipe(Vector2 direction)
        {
            Vector2 offset = direction * Progress * new Vector2(_fromTarget.Width, _fromTarget.Height);
            
            _spriteBatch.Draw(_fromTarget, offset, Color.White);
            _spriteBatch.Draw(_toTarget, offset - direction * new Vector2(_fromTarget.Width, _fromTarget.Height), Color.White);
        }

        private void DrawCircle()
        {
            float maxRadius = MathF.Sqrt(_fromTarget.Width * _fromTarget.Width + _fromTarget.Height * _fromTarget.Height) * 0.5f;
            float currentRadius = maxRadius * Progress;

            // Simplificado: usar shader o stencil para círculo real
            _spriteBatch.Draw(_fromTarget, Vector2.Zero, Color.White);
        }

        private void DrawPixelate()
        {
            int blockSize = (int)(Progress * 50) + 1;
            // Implementar con shader de pixelación
            _spriteBatch.Draw(_fromTarget, Vector2.Zero, Color.White);
        }
    }
}
```

---

## 5. CullingOptimizer

Spatial hash para culling eficiente de muchos actores.

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Optimization
{
    public class SpatialHash
    {
        private readonly float _cellSize;
        private readonly Dictionary<long, List<ICullable>> _cells = new Dictionary<long, List<ICullable>>();

        public SpatialHash(float cellSize)
        {
            _cellSize = cellSize;
        }

        public void Insert(ICullable item)
        {
            var bounds = item.GetBounds();
            var minCell = GetCellCoord(bounds.TopLeft);
            var maxCell = GetCellCoord(bounds.BottomRight);

            for (int x = minCell.X; x <= maxCell.X; x++)
            {
                for (int y = minCell.Y; y <= maxCell.Y; y++)
                {
                    long key = GetKey(x, y);
                    
                    if (_cells.TryGetValue(key, out var list) == false)
                    {
                        list = new List<ICullable>();
                        _cells[key] = list;
                    }
                    
                    list.Add(item);
                }
            }
        }

        public void Remove(ICullable item)
        {
            var bounds = item.GetBounds();
            var minCell = GetCellCoord(bounds.TopLeft);
            var maxCell = GetCellCoord(bounds.BottomRight);

            for (int x = minCell.X; x <= maxCell.X; x++)
            {
                for (int y = minCell.Y; y <= maxCell.Y; y++)
                {
                    long key = GetKey(x, y);
                    
                    if (_cells.TryGetValue(key, out var list))
                    {
                        list.Remove(item);
                    }
                }
            }
        }

        public void Query(RectangleF area, List<ICullable> results)
        {
            results.Clear();
            
            var minCell = GetCellCoord(area.TopLeft);
            var maxCell = GetCellCoord(area.BottomRight);

            var seen = new HashSet<ICullable>();

            for (int x = minCell.X; x <= maxCell.X; x++)
            {
                for (int y = minCell.Y; y <= maxCell.Y; y++)
                {
                    long key = GetKey(x, y);
                    
                    if (_cells.TryGetValue(key, out var list))
                    {
                        foreach (var item in list)
                        {
                            if (seen.Add(item))
                            {
                                if (area.Intersects(item.GetBounds()))
                                {
                                    results.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            _cells.Clear();
        }

        private Point GetCellCoord(Vector2 worldPos)
        {
            return new Point(
                (int)MathF.Floor(worldPos.X / _cellSize),
                (int)MathF.Floor(worldPos.Y / _cellSize)
            );
        }

        private long GetKey(int x, int y)
        {
            return ((long)x << 32) | (uint)y;
        }
    }

    public interface ICullable
    {
        RectangleF GetBounds();
    }
}
```

---

## 6. BehaviorDebugger

Visualización de información de debug.

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Camera;
using GameEngine.Layers;

namespace GameEngine.Debug
{
    public class BehaviorDebugger
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;
        private readonly Texture2D _pixel;

        public bool ShowCameraBounds { get; set; } = true;
        public bool ShowLayerInfo { get; set; } = true;
        public bool ShowBehaviorInfo { get; set; } = true;

        public BehaviorDebugger(SpriteBatch spriteBatch, SpriteFont font, GraphicsDevice device)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void DrawCameraDebug(CameraDirector director)
        {
            if (ShowCameraBounds == false) return;

            foreach (var camera in director.Cameras)
            {
                var viewport = director.GetViewport(camera);
                if (viewport == null) continue;

                var visible = camera.GetVisibleBounds();
                DrawRectangle(visible, Color.Yellow * 0.3f);
                DrawRectangleOutline(visible, Color.Yellow);

                var culling = camera.GetCullingBounds();
                DrawRectangleOutline(culling, Color.Red);

                DrawString($"Camera: {camera.Name}", new Vector2(viewport.X + 5, viewport.Y + 5), Color.White);
                DrawString($"Zoom: {camera.Zoom:F2}", new Vector2(viewport.X + 5, viewport.Y + 25), Color.White);
            }
        }

        public void DrawLayerDebug(LayerDirector director)
        {
            if (ShowLayerInfo == false) return;

            int y = 100;
            foreach (var layer in director.Layers)
            {
                DrawString($"Layer: {layer.Name} (Order: {layer.DrawOrder})", new Vector2(5, y), Color.Cyan);
                y += 20;
                DrawString($"  Actors: {layer.Actors.Count}, Behaviors: {layer.Behaviors.Count}", new Vector2(5, y), Color.Gray);
                y += 20;
            }
        }

        private void DrawRectangle(RectangleF rect, Color color)
        {
            _spriteBatch.Draw(_pixel, rect.Position, null, color, 0f, Vector2.Zero, rect.Size, SpriteEffects.None, 0f);
        }

        private void DrawRectangleOutline(RectangleF rect, Color color)
        {
            float thickness = 2f;
            
            _spriteBatch.Draw(_pixel, new Vector2(rect.Left, rect.Top), null, color, 0f, Vector2.Zero, new Vector2(rect.Width, thickness), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_pixel, new Vector2(rect.Left, rect.Bottom - thickness), null, color, 0f, Vector2.Zero, new Vector2(rect.Width, thickness), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_pixel, new Vector2(rect.Left, rect.Top), null, color, 0f, Vector2.Zero, new Vector2(thickness, rect.Height), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_pixel, new Vector2(rect.Right - thickness, rect.Top), null, color, 0f, Vector2.Zero, new Vector2(thickness, rect.Height), SpriteEffects.None, 0f);
        }

        private void DrawString(string text, Vector2 position, Color color)
        {
            _spriteBatch.DrawString(_font, text, position, color);
        }
    }
}
```

---

## Integración Final en Scene

```csharp
public class Scene
{
    public RectangleF WorldBounds { get; set; }
    
    public CameraDirector CameraDirector { get; }
    public LayerDirector LayerDirector { get; }
    public CameraManager CameraManager { get; }
    public RenderTargetPool RenderTargetPool { get; }
    public PostProcessStack PostProcess { get; }
    public TransitionSystem TransitionSystem { get; }
    public SpatialHash SpatialHash { get; }
    public BehaviorDebugger Debugger { get; set; }

    public Scene(GraphicsDevice device, SpriteBatch spriteBatch, SpriteFont font, RectangleF worldBounds)
    {
        WorldBounds = worldBounds;
        
        CameraDirector = new CameraDirector(this);
        LayerDirector = new LayerDirector();
        CameraManager = new CameraManager();
        RenderTargetPool = new RenderTargetPool(device);
        PostProcess = new PostProcessStack(device, RenderTargetPool);
        TransitionSystem = new TransitionSystem(device, spriteBatch);
        SpatialHash = new SpatialHash(256f);
        Debugger = new BehaviorDebugger(spriteBatch, font, device);
    }

    public void Update(GameTime gameTime)
    {
        CameraManager.Update(gameTime);
        CameraDirector.Update(gameTime);
        
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        LayerDirector.Update(delta);
        
        TransitionSystem.Update(gameTime);
        
        // Reconstruir spatial hash cada frame o incremental
        SpatialHash.Clear();
        foreach (var layer in LayerDirector.Layers)
        {
            foreach (var actor in layer.Actors)
            {
                if (actor is ICullable cullable)
                {
                    SpatialHash.Insert(cullable);
                }
            }
        }
    }

    public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
    {
        // Pre-render a targets si hay post-proceso
        // Aplicar post-proceso
        // Transiciones
        // Debug overlay
        
        LayerDirector.CollectRenderCommands(CameraDirector);
        CameraDirector.Render(spriteBatch, defaultFont);
        
        Debugger.DrawCameraDebug(CameraDirector);
        Debugger.DrawLayerDebug(LayerDirector);
    }
}
```

---

¿Pasamos a **Fase 6: Casos de Uso y Ejemplos** o finalizamos?
User: Veamos la fase 6
Kimi: Aquí está la **Fase 6: Casos de Uso y Ejemplos**.

---

## Caso 1: Juego de Plataformas Clásico

```csharp
// Setup inicial
var scene = new Scene(device, spriteBatch, font, new RectangleF(0, 0, 4000, 2000));

// Cámara principal con seguimiento suave
var mainCam = scene.CameraDirector.CreateWorldCamera("Main", new Vector2(400, 300));
mainCam.VirtualWidth = 800;
mainCam.VirtualHeight = 600;

var follow = new FollowBehavior(mainCam)
{
    SmoothSpeed = 4f,
    DeadZone = new Vector2(100, 50),
    UseDeadZone = true,
    Offset = new Vector2(0, -50) // Ligeramente arriba del jugador
};
mainCam.AddBehavior(follow);

var bounds = new BoundsConstraintBehavior(mainCam);
mainCam.AddBehavior(bounds);

// Capas
var bgLayer = scene.LayerDirector.CreateLayer("Background", 0);
bgLayer.AddBehavior(new ParallaxBehavior(bgLayer) { Factor = new Vector2(0.2f, 0.2f) });
bgLayer.AddBehavior(new ScrollBehavior(bgLayer) { Speed = new Vector2(-5, 0), Wrap = true, WrapBounds = new Vector2(800, 0) });

var gameLayer = scene.LayerDirector.CreateLayer("Gameplay", 10);

var fgLayer = scene.LayerDirector.CreateLayer("Foreground", 20);
fgLayer.AddBehavior(new ParallaxBehavior(fgLayer) { Factor = new Vector2(1.5f, 1.5f) });

// Actores
var player = new SpriteActor(new Vector2(200, 500), playerTexture);
gameLayer.AddActor(player);

// En el update del juego
follow.Target = player.Position;

// Shake al recibir daño
void OnPlayerDamaged()
{
    mainCam.AddBehavior(new ShakeBehavior(mainCam, 0.3f, 8f));
}

// Recoil al disparar
void OnPlayerShoot()
{
    var recoil = mainCam.GetBehavior<RecoilBehavior>();
    if (recoil == null)
    {
        recoil = new RecoilBehavior(mainCam);
        mainCam.AddBehavior(recoil);
    }
    recoil.AddRecoil(10f, new Vector2(-1, 0.1f));
}
```

---

## Caso 2: Juego de Lucha 1v1

```csharp
var scene = new Scene(device, spriteBatch, font, new RectangleF(0, 0, 2000, 1000));

// Cámara con seguimiento de dos jugadores
var fightCam = scene.CameraDirector.CreateWorldCamera("Fight", new Vector2(400, 300));
fightCam.VirtualWidth = 800;
fightCam.VirtualHeight = 450; // Aspect ratio 16:9

var multiFollow = new MultiTargetFollowBehavior(fightCam)
{
    MinZoom = 0.7f,
    MaxZoom = 1.3f,
    ZoomMargin = 200f,
    SmoothSpeed = 3f,
    ZoomSmoothSpeed = 2f
};
fightCam.AddBehavior(multiFollow);

var bounds = new BoundsConstraintBehavior(fightCam) { SoftConstraint = true, Softness = 0.5f };
fightCam.AddBehavior(bounds);

// Setup jugadores
var player1 = new SpriteActor(new Vector2(300, 500), p1Texture);
var player2 = new SpriteActor(new Vector2(500, 500), p2Texture);

gameLayer.AddActor(player1);
gameLayer.AddActor(player2);

multiFollow.AddTarget(player1.Position);
multiFollow.AddTarget(player2.Position);

// Update
multiFollow.Targets[0] = player1.Position;
multiFollow.Targets[1] = player2.Position;

// Zoom dramático en super ataque
void OnSuperAttack()
{
    fightCam.AddBehavior(new DollyZoomBehavior(fightCam, 1.5f)
    {
        Target = (player1.Position + player2.Position) * 0.5f,
        StartDistance = 400f,
        EndDistance = 200f,
        StartZoom = fightCam.Zoom,
        EndZoom = 1.8f
    });
}
```

---

## Caso 3: Estrategia/RTS con Minimapa

```csharp
var scene = new Scene(device, spriteBatch, font, new RectangleF(0, 0, 5000, 5000));

// Cámara principal (mundo)
var worldCam = scene.CameraDirector.CreateWorldCamera("World", new Vector2(400, 300));
worldCam.VirtualWidth = 800;
worldCam.VirtualHeight = 600;

worldCam.AddBehavior(new FollowBehavior(worldCam) { SmoothSpeed = 8f });
worldCam.AddBehavior(new BoundsConstraintBehavior(worldCam));

// Minimapa (hybrid camera)
var miniCam = scene.CameraDirector.CreateHybridCamera("Minimap", new Vector2(100, 100), 2f);
miniCam.PixelsPerUnit = 0.5f; // 1 unidad mundo = 0.5 pixels

scene.CameraDirector.SetViewport(miniCam, new ViewportConfig
{
    X = 650,
    Y = 450,
    Width = 150,
    Height = 150
});

// Capas
var terrainLayer = scene.LayerDirector.CreateLayer("Terrain", 0);
var unitsLayer = scene.LayerDirector.CreateLayer("Units", 10);
var uiLayer = scene.LayerDirector.CreateLayer("UI", 100);

// Minimapa solo ve terrain y units
terrainLayer.CameraMask = (1u << 0) | (1u << 1); // World + Minimap
unitsLayer.CameraMask = (1u << 0) | (1u << 1);
uiLayer.CameraMask = 1u << 0; // Solo World

// Marcadores en minimapa
foreach (var unit in units)
{
    var marker = new SpriteActor(unit.Position, markerTexture);
    marker.Scale = new Vector2(0.5f, 0.5f);
    unitsLayer.AddActor(marker);
}

// Click en minimapa mueve cámara
void OnMinimapClick(Vector2 screenPos)
{
    Vector2 worldPos = miniCam.ScreenToWorld(screenPos, scene.CameraDirector.GetViewport(miniCam));
    worldCam.SetPosition(worldPos);
}
```

---

## Caso 4: Juego de Terror con Linterna

```csharp
var scene = new Scene(device, spriteBatch, font, new RectangleF(0, 0, 3000, 2000));

var mainCam = scene.CameraDirector.CreateWorldCamera("Main", new Vector2(400, 300));
mainCam.AddBehavior(new FollowBehavior(mainCam) { SmoothSpeed = 2f }); // Más lento, tensión

// Capa de oscuridad con máscara
var darknessLayer = scene.LayerDirector.CreateLayer("Darkness", 50);
darknessLayer.AddBehavior(new TintBehavior(darknessLayer) 
{ 
    Tint = new Color(10, 10, 20), 
    Intensity = 0.95f 
});

var mask = new MaskBehavior(darknessLayer)
{
    Shape = MaskBehavior.MaskShape.Circle,
    Center = player.Position,
    Radius = 120f,
    Invert = true, // Todo oscuro excepto círculo
    Softness = 30f
};
darknessLayer.AddBehavior(mask);

// Update linterna
void Update()
{
    mask.Center = player.Position;
    
    // Parpadeo aleatorio de la linterna
    if (random.Next(100) < 2)
    {
        mask.Radius = 80f + random.Next(40);
    }
    else
    {
        mask.Radius = MathUtils.Lerp(mask.Radius, 120f, 0.1f);
    }
}

// Evento de susto: flash + shake
void OnJumpscare()
{
    mainCam.AddBehavior(new ShakeBehavior(mainCam, 0.5f, 15f));
    
    var flash = new FlashBehavior(gameLayer, 0.2f)
    {
        FlashColor = Color.White,
        FadeOutSpeed = 10f
    };
    gameLayer.AddBehavior(flash);
}
```

---

## Caso 5: Carrera con Split-Screen

```csharp
var scene = new Scene(device, spriteBatch, font, new RectangleF(0, 0, 10000, 5000));

// Jugador 1 - mitad superior
var p1Cam = scene.CameraDirector.CreateWorldCamera("P1", new Vector2(400, 150));
scene.CameraDirector.SetViewport(p1Cam, new ViewportConfig
{
    X = 0,
    Y = 0,
    Width = 800,
    Height = 300
});

// Jugador 2 - mitad inferior  
var p2Cam = scene.CameraDirector.CreateWorldCamera("P2", new Vector2(400, 150));
scene.CameraDirector.SetViewport(p2Cam, new ViewportConfig
{
    X = 0,
    Y = 300,
    Width = 800,
    Height = 300
});

// Setup cámaras
var p1Follow = new FollowBehavior(p1Cam) { SmoothSpeed = 5f };
var p2Follow = new FollowBehavior(p2Cam) { SmoothSpeed = 5f };

p1Cam.AddBehavior(p1Follow);
p2Cam.AddBehavior(p2Follow);

// Capas
var trackLayer = scene.LayerDirector.CreateLayer("Track", 0);
var carsLayer = scene.LayerDirector.CreateLayer("Cars", 10);
var effectsLayer = scene.LayerDirector.CreateLayer("Effects", 20);

// Asignar cámaras
trackLayer.CameraMask = 0xFFFFFFFF; // Todas
carsLayer.CameraMask = 0xFFFFFFFF;
effectsLayer.CameraMask = 0xFFFFFFFF;

// UI por jugador con ScreenCamera
var p1UI = scene.CameraDirector.CreateScreenCamera("P1UI", new Vector2(400, 150));
scene.CameraDirector.SetViewport(p1UI, new ViewportConfig
{
    X = 0,
    Y = 0,
    Width = 800,
    Height = 300
});

var p2UI = scene.CameraDirector.CreateScreenCamera("P2UI", new Vector2(400, 150));
scene.CameraDirector.SetViewport(p2UI, new ViewportConfig
{
    X = 0,
    Y = 300,
    Width = 800,
    Height = 300
});

// UI layers
var p1UILayer = scene.LayerDirector.CreateLayer("P1UI", 100);
p1UILayer.CameraMask = 1u << 2; // Solo P1UI

var p2UILayer = scene.LayerDirector.CreateLayer("P2UI", 101);
p2UILayer.CameraMask = 1u << 3; // Solo P2UI

// Update
p1Follow.Target = car1.Position;
p2Follow.Target = car2.Position;
```

---

## Caso 6: RPG con Transiciones de Zona

```csharp
var scene = new Scene(device, spriteBatch, font, new RectangleF(0, 0, 10000, 10000));

var mainCam = scene.CameraDirector.CreateWorldCamera("Main", new Vector2(400, 300));

// Zonas con cámaras predefinidas
var forestCam = scene.CameraDirector.CreateWorldCamera("Forest", new Vector2(2000, 2000));
var caveCam = scene.CameraDirector.CreateWorldCamera("Cave", new Vector2(5000, 3000));

scene.CameraManager.RegisterCamera(mainCam);
scene.CameraManager.RegisterCamera(forestCam);
scene.CameraManager.RegisterCamera(caveCam);

// Transición al cambiar de zona
void EnterForest()
{
    var from = scene.CameraDirector.GetRenderTarget(mainCam);
    var to = scene.CameraDirector.GetRenderTarget(forestCam);
    
    scene.TransitionSystem.StartTransition(
        from,
        to,
        TransitionType.Circle,
        1f,
        () =>
        {
            scene.CameraManager.DeactivateCamera("Main");
            scene.CameraManager.ActivateCamera("Forest");
        }
    );
}

// Cámara de cueva con path cinemático al entrar
void EnterCave()
{
    scene.CameraManager.ActivateCamera("Cave");
    
    var path = new CinematicPathBehavior(caveCam)
    {
        ControlPoints = new[]
        {
            new Vector2(4800, 2800),
            new Vector2(4900, 2900),
            new Vector2(5000, 3000),
            new Vector2(5100, 3100)
        },
        Duration = 3f
    };
    caveCam.AddBehavior(path);
    
    // Volver a follow del jugador al terminar
    path.Duration = 3f;
    // Al completar, agregar FollowBehavior...
}
```

---

## Caso 7: Shooter con Recoil Acumulativo

```csharp
var mainCam = scene.CameraDirector.CreateWorldCamera("Main", new Vector2(400, 300));

// Recoil persistente
var recoil = new RecoilBehavior(mainCam)
{
    MaxRecoil = 30f,
    RecoverySpeed = 5f,
    Decay = 0.85f
};
mainCam.AddBehavior(recoil);

// Disparo automático
void OnAutoFire()
{
    recoil.AddRecoil(3f, new Vector2(-1, RandomRange(-0.3f, 0.3f)));
}

// Disparo escopeta
void OnShotgunFire()
{
    recoil.AddRecoil(20f, new Vector2(-1, 0));
    mainCam.AddBehavior(new ShakeBehavior(mainCam, 0.4f, 12f));
}

// Recarga: reset suave
void OnReload()
{
    recoil.Reset(); // Vuelve gradualmente por RecoverySpeed
}
```

---

## Caso 8: Juego de Móvil con Safe Area

```csharp
var mainCam = scene.CameraDirector.CreateWorldCamera("Main", new Vector2(360, 640)); // Portrait

// Configurar viewport con safe area (notch, gestos)
scene.CameraDirector.SetViewport(mainCam, new ViewportConfig
{
    X = 0,
    Y = 50,  // Evitar notch
    Width = 720,
    Height = 1280 - 100, // Evitar gestos abajo
    MaintainAspectRatio = true
});

// UI en coordenadas relativas 0-1
var uiCam = scene.CameraDirector.CreateScreenCamera("UI", new Vector2(0.5f, 0.5f));
scene.CameraDirector.SetViewport(uiCam, new ViewportConfig
{
    X = 0,
    Y = 0,
    Width = 720,
    Height = 1280
});

var uiLayer = scene.LayerDirector.CreateLayer("UI", 100);
uiLayer.CameraMask = 1u << 1; // Solo UI cam

// Botones posicionados en 0-1
var button = new SpriteActor(new Vector2(0.5f, 0.9f), buttonTexture);
button.Scale = new Vector2(0.001f, 0.001f); // Escalado para pantalla
uiLayer.AddActor(button);
```

---

## Resumen de Patrones

| Patrón | Uso |
|--------|-----|
| **Follow + DeadZone** | Plataformas, acción |
| **MultiTarget + Zoom** | Lucha, coop |
| **Hybrid Camera** | Minimapas, HUDs 3D |
| **Mask + Tint** | Iluminación, atmósfera |
| **Shake + Recoil** | Impacto, armas |
| **Parallax + Scroll** | Profundidad, fondos |
| **DollyZoom** | Énfasis dramático |
| **CinematicPath** | Introducciones, eventos |
| **SplitScreen** | Multijugador local |
| **Transitions** | Cambios de zona, niveles |

---

¿Finalizamos el motor o hay algún caso adicional que quieras cubrir?
User: Muestra todo el código que tenemos hasta ahora con las modificaciones últimas que hemos hecho para poder pasarlo a un editor de código
Kimi: Aquí está el código completo del motor, organizado por namespaces y archivos.

---

## Estructura de Carpetas

```
GameEngine/
├── Math/
│   ├── MathUtils.cs
│   ├── RectangleF.cs
│   └── Vector2Extensions.cs
├── Behavior/
│   └── IBehavior.cs
├── Camera/
│   ├── CameraType.cs
│   ├── CameraState.cs
│   ├── ICameraBehavior.cs
│   ├── CameraBehaviorBase.cs
│   ├── Camera.cs
│   ├── WorldCamera.cs
│   ├── ScreenCamera.cs
│   ├── HybridCamera.cs
│   ├── ViewportConfig.cs
│   └── CameraDirector.cs
├── Camera/Behaviors/
│   ├── FollowBehavior.cs
│   ├── BoundsConstraintBehavior.cs
│   ├── MultiTargetFollowBehavior.cs
│   ├── ShakeBehavior.cs
│   ├── RecoilBehavior.cs
│   ├── DollyZoomBehavior.cs
│   └── CinematicPathBehavior.cs
├── Rendering/
│   ├── RenderCommand.cs
│   ├── TextureRenderCommand.cs
│   ├── TextRenderCommand.cs
│   ├── RenderQueue.cs
│   └── IRenderable.cs
├── Rendering/PostProcess/
│   ├── IPostProcessEffect.cs
│   └── PostProcessStack.cs
├── Entities/
│   └── Actor.cs
├── Entities/Actors/
│   ├── SpriteActor.cs
│   └── TextActor.cs
├── Layers/
│   ├── ILayerBehavior.cs
│   ├── LayerBehaviorBase.cs
│   ├── Layer.cs
│   └── LayerDirector.cs
├── Layers/Behaviors/
│   ├── ParallaxBehavior.cs
│   ├── ScrollBehavior.cs
│   ├── TintBehavior.cs
│   ├── WaveDistortionBehavior.cs
│   ├── FlashBehavior.cs
│   └── MaskBehavior.cs
├── Scene/
│   └── Scene.cs
├── Optimization/
│   ├── SpatialHash.cs
│   └── ICullable.cs
├── Transitions/
│   ├── TransitionType.cs
│   └── TransitionSystem.cs
└── Debug/
    └── BehaviorDebugger.cs
```

---

## 1. Math/MathUtils.cs

```csharp
using System;
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public static class MathUtils
    {
        public const float PI = (float)System.Math.PI;
        public const float TwoPI = PI * 2f;
        public const float DegToRad = PI / 180f;
        public const float RadToDeg = 180f / PI;
        public const float Epsilon = 0.0001f;

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(
                Lerp(a.X, b.X, t),
                Lerp(a.Y, b.Y, t)
            );
        }

        public static float SmoothStep(float a, float b, float t)
        {
            t = Clamp01(t);
            t = t * t * (3f - 2f * t);
            return Lerp(a, b, t);
        }

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static float Clamp01(float value)
        {
            return Clamp(value, 0f, 1f);
        }

        public static float Approach(float current, float target, float maxDelta)
        {
            float diff = target - current;
            if (System.Math.Abs(diff) <= maxDelta)
                return target;
            
            return current + System.Math.Sign(diff) * maxDelta;
        }

        public static Vector2 Approach(Vector2 current, Vector2 target, float maxDelta)
        {
            Vector2 diff = target - current;
            float dist = diff.Length();
            
            if (dist <= maxDelta || dist < Epsilon)
                return target;
            
            return current + (diff / dist) * maxDelta;
        }

        public static float Snap(float value, float gridSize)
        {
            if (gridSize < Epsilon) return value;
            return (float)System.Math.Floor(value / gridSize) * gridSize;
        }

        public static bool Approx(float a, float b, float epsilon = Epsilon)
        {
            return System.Math.Abs(a - b) < epsilon;
        }

        public static bool Approx(Vector2 a, Vector2 b, float epsilon = Epsilon)
        {
            return Approx(a.X, b.X, epsilon) && Approx(a.Y, b.Y, epsilon);
        }
    }
}
```

---

## 2. Math/RectangleF.cs

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public struct RectangleF
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2 position, Vector2 size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.X;
            Height = size.Y;
        }

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        public Vector2 Center => new Vector2(X + Width * 0.5f, Y + Height * 0.5f);
        public Vector2 Size => new Vector2(Width, Height);
        public Vector2 Position => new Vector2(X, Y);

        public bool Contains(Vector2 point)
        {
            return point.X >= Left && point.X <= Right &&
                   point.Y >= Top && point.Y <= Bottom;
        }

        public bool Contains(RectangleF other)
        {
            return Left <= other.Left && Right >= other.Right &&
                   Top <= other.Top && Bottom >= other.Bottom;
        }

        public bool Intersects(RectangleF other)
        {
            return Left < other.Right && Right > other.Left &&
                   Top < other.Bottom && Bottom > other.Top;
        }

        public RectangleF Expand(float amount)
        {
            return new RectangleF(
                X - amount,
                Y - amount,
                Width + amount * 2f,
                Height + amount * 2f
            );
        }

        public RectangleF Expand(Vector2 amount)
        {
            return new RectangleF(
                X - amount.X,
                Y - amount.Y,
                Width + amount.X * 2f,
                Height + amount.Y * 2f
            );
        }

        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            float left = System.Math.Max(a.Left, b.Left);
            float top = System.Math.Max(a.Top, b.Top);
            float right = System.Math.Min(a.Right, b.Right);
            float bottom = System.Math.Min(a.Bottom, b.Bottom);

            if (right > left && bottom > top)
            {
                return new RectangleF(left, top, right - left, bottom - top);
            }

            return Empty;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(
                (int)X,
                (int)Y,
                (int)Width,
                (int)Height
            );
        }

        public static RectangleF FromPoints(Vector2 a, Vector2 b)
        {
            float minX = System.Math.Min(a.X, b.X);
            float minY = System.Math.Min(a.Y, b.Y);
            float maxX = System.Math.Max(a.X, b.X);
            float maxY = System.Math.Max(a.Y, b.Y);

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        public static RectangleF Empty => new RectangleF(0, 0, 0, 0);

        public bool IsEmpty => Width <= 0 || Height <= 0;

        public override string ToString()
        {
            return $"[{X}, {Y}, {Width}, {Height}]";
        }
    }
}
```

---

## 3. Math/Vector2Extensions.cs

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Math
{
    public static class Vector2Extensions
    {
        public static Vector2 Perpendicular(this Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }

        public static Vector2 Rotate(this Vector2 vector, float radians)
        {
            float cos = (float)System.Math.Cos(radians);
            float sin = (float)System.Math.Sin(radians);
            
            return new Vector2(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos
            );
        }

        public static Vector2 RotateAround(this Vector2 vector, Vector2 origin, float radians)
        {
            return origin + (vector - origin).Rotate(radians);
        }

        public static Vector2 Snap(this Vector2 vector, float gridSize)
        {
            return new Vector2(
                MathUtils.Snap(vector.X, gridSize),
                MathUtils.Snap(vector.Y, gridSize)
            );
        }

        public static Vector2 Snap(this Vector2 vector, Vector2 gridSize)
        {
            return new Vector2(
                MathUtils.Snap(vector.X, gridSize.X),
                MathUtils.Snap(vector.Y, gridSize.Y)
            );
        }

        public static float Cross(this Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(
                System.Math.Abs(vector.X),
                System.Math.Abs(vector.Y)
            );
        }

        public static Vector2 Sign(this Vector2 vector)
        {
            return new Vector2(
                System.Math.Sign(vector.X),
                System.Math.Sign(vector.Y)
            );
        }

        public static Vector2 ComponentMul(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2 ComponentDiv(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }
    }
}
```

---

## 4. Behavior/IBehavior.cs

```csharp
namespace GameEngine.Behavior
{
    public interface IBehavior<TState>
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        float? Duration { get; }
        float ElapsedTime { get; }
        bool IsComplete { get; }
        
        void Update(float deltaTime);
        TState Apply(TState current);
        void Reset();
    }
}
```

---

## 5. Camera/CameraType.cs

```csharp
namespace GameEngine.Camera
{
    public enum CameraType
    {
        World,
        Screen,
        Hybrid
    }
}
```

---

## 6. Camera/CameraState.cs

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public readonly struct CameraState
    {
        public readonly Vector2 Position;
        public readonly float Rotation;
        public readonly float Zoom;
        public readonly Vector2 Origin;

        public CameraState(Vector2 position, float rotation, float zoom, Vector2 origin)
        {
            Position = position;
            Rotation = rotation;
            Zoom = zoom;
            Origin = origin;
        }

        public static CameraState Default(Vector2 origin)
        {
            return new CameraState(Vector2.Zero, 0f, 1f, origin);
        }

        public CameraState WithPosition(Vector2 position)
        {
            return new CameraState(position, Rotation, Zoom, Origin);
        }

        public CameraState WithRotation(float rotation)
        {
            return new CameraState(Position, rotation, Zoom, Origin);
        }

        public CameraState WithZoom(float zoom)
        {
            return new CameraState(Position, Rotation, zoom, Origin);
        }

        public CameraState WithOrigin(Vector2 origin)
        {
            return new CameraState(Position, Rotation, Zoom, origin);
        }

        public Matrix GetViewMatrix()
        {
            Matrix translation = Matrix.CreateTranslation(-Position.X, -Position.Y, 0f);
            Matrix rotation = Matrix.CreateRotationZ(-Rotation);
            Matrix scale = Matrix.CreateScale(Zoom, Zoom, 1f);
            Matrix originOffset = Matrix.CreateTranslation(Origin.X, Origin.Y, 0f);

            return translation * rotation * scale * originOffset;
        }

        public Matrix GetInverseViewMatrix()
        {
            Matrix scale = Matrix.CreateScale(1f / Zoom, 1f / Zoom, 1f);
            Matrix rotation = Matrix.CreateRotationZ(Rotation);
            Matrix translation = Matrix.CreateTranslation(Position.X, Position.Y, 0f);
            Matrix originOffset = Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0f);

            return originOffset * scale * rotation * translation;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition, Viewport viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix projection = GetProjectionMatrix(viewport);
            
            Vector3 transformed = Vector3.Transform(new Vector3(worldPosition, 0f), view * projection);
            return new Vector2(transformed.X, transformed.Y);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition, Viewport viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix projection = GetProjectionMatrix(viewport);
            Matrix inverse = Matrix.Invert(view * projection);
            
            Vector3 transformed = Vector3.Transform(new Vector3(screenPosition, 0f), inverse);
            return new Vector2(transformed.X, transformed.Y);
        }

        private Matrix GetProjectionMatrix(Viewport viewport)
        {
            return Matrix.CreateOrthographicOffCenter(
                0, viewport.Width,
                viewport.Height, 0,
                0f, 1f
            );
        }

        public RectangleF GetVisibleBounds(Viewport viewport)
        {
            Vector2 topLeft = ScreenToWorld(Vector2.Zero, viewport);
            Vector2 bottomRight = ScreenToWorld(new Vector2(viewport.Width, viewport.Height), viewport);
            
            return RectangleF.FromPoints(topLeft, bottomRight);
        }

        public override string ToString()
        {
            return $"Pos:{Position}, Rot:{Rotation:F2}, Zoom:{Zoom:F2}, Origin:{Origin}";
        }
    }
}
```

---

## 7. Camera/ICameraBehavior.cs

```csharp
namespace GameEngine.Camera
{
    public interface ICameraBehavior
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        float? Duration { get; }
        float ElapsedTime { get; }
        bool IsComplete { get; }
        
        void Update(float deltaTime);
        void Reset();
    }
}
```

---

## 8. Camera/CameraBehaviorBase.cs

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
{
    public abstract class CameraBehaviorBase : ICameraBehavior
    {
        protected readonly Camera Camera;
        
        public bool IsActive { get; set; } = true;
        public abstract int Priority { get; }
        
        public float? Duration { get; protected set; }
        public float ElapsedTime { get; private set; }
        public bool IsComplete { get; protected set; }

        protected CameraBehaviorBase(Camera camera)
        {
            Camera = camera;
        }

        public void Update(float deltaTime)
        {
            if (IsActive == false) return;
            if (IsComplete) return;

            ElapsedTime += deltaTime;

            if (Duration.HasValue && ElapsedTime >= Duration.Value)
            {
                IsComplete = true;
                OnComplete();
                return;
            }

            OnUpdate(deltaTime);
        }

        protected abstract void OnUpdate(float deltaTime);

        public virtual void Reset()
        {
            ElapsedTime = 0f;
            IsComplete = false;
            IsActive = true;
        }

        protected virtual void OnComplete() { }
    }
}
```

---

## 9. Camera/Camera.cs

```csharp
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public abstract class Camera
    {
        protected readonly List<ICameraBehavior> _behaviors = new List<ICameraBehavior>();
        
        public string Name { get; }
        public CameraDirector Director { get; }
        public CameraType Type { get; }
        
        public float VirtualWidth { get; set; }
        public float VirtualHeight { get; set; }
        
        protected CameraState _state;
        private Matrix _viewMatrix;
        private bool _matrixDirty = true;

        public CameraState State 
        { 
            get => _state;
            protected set
            {
                _state = value;
                _matrixDirty = true;
            }
        }

        public Vector2 Position => State.Position;
        public float Rotation => State.Rotation;
        public float Zoom => State.Zoom;
        public Vector2 Origin => State.Origin;

        protected Camera(string name, CameraType type, Vector2 origin, CameraDirector director)
        {
            Name = name;
            Type = type;
            Director = director;
            _state = CameraState.Default(origin);
            VirtualWidth = origin.X * 2f;
            VirtualHeight = origin.Y * 2f;
        }

        public Matrix GetViewMatrix()
        {
            if (_matrixDirty)
            {
                _viewMatrix = CalculateViewMatrix();
                _matrixDirty = false;
            }
            return _viewMatrix;
        }

        protected abstract Matrix CalculateViewMatrix();
        public abstract Matrix GetProjectionMatrix(ViewportConfig viewport);
        
        public abstract bool IsInView(RectangleF bounds);
        public abstract bool IsInView(Vector2 point);
        
        public abstract Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport);
        public abstract Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport);

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                var behavior = _behaviors[i];
                
                if (behavior.IsActive == false) continue;
                
                behavior.Update(delta);
                
                if (behavior.IsComplete && behavior.Duration.HasValue)
                {
                    _behaviors.RemoveAt(i);
                }
            }
        }

        public void SetPosition(Vector2 position)
        {
            State = State.WithPosition(position);
        }

        public void SetRotation(float rotation)
        {
            State = State.WithRotation(rotation);
        }

        public void SetZoom(float zoom)
        {
            State = State.WithZoom(MathUtils.Clamp(zoom, 0.001f, 100f));
        }

        public void Move(Vector2 delta)
        {
            State = State.WithPosition(State.Position + delta);
        }

        public void Rotate(float deltaRadians)
        {
            State = State.WithRotation(State.Rotation + deltaRadians);
        }

        public void ZoomBy(float factor)
        {
            SetZoom(State.Zoom * factor);
        }

        public void AddBehavior(ICameraBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public void RemoveBehavior(ICameraBehavior behavior)
        {
            _behaviors.Remove(behavior);
        }

        public T GetBehavior<T>() where T : ICameraBehavior
        {
            return _behaviors.OfType<T>().FirstOrDefault();
        }

        public void ClearBehaviors()
        {
            _behaviors.Clear();
        }

        public override string ToString()
        {
            return $"Camera '{Name}' ({Type}): {State}";
        }
    }
}
```

---

## 10. Camera/WorldCamera.cs

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public class WorldCamera : Camera
    {
        public WorldCamera(string name, Vector2 origin, CameraDirector director)
            : base(name, CameraType.World, origin, director)
        {
        }

        protected override Matrix CalculateViewMatrix()
        {
            return State.GetViewMatrix();
        }

        public override Matrix GetProjectionMatrix(ViewportConfig viewport)
        {
            float targetAspect = viewport.Width / (float)viewport.Height;
            float virtualAspect = VirtualWidth / VirtualHeight;
            
            float viewWidth = VirtualWidth;
            float viewHeight = VirtualHeight;
            
            if (viewport.MaintainAspectRatio)
            {
                if (targetAspect > virtualAspect)
                {
                    viewWidth = VirtualHeight * targetAspect;
                }
                else
                {
                    viewHeight = VirtualWidth / targetAspect;
                }
            }
            
            return Matrix.CreateOrthographicOffCenter(
                0, viewWidth,
                viewHeight, 0,
                0f, 1f
            );
        }

        public override bool IsInView(RectangleF bounds)
        {
            var viewport = Director.GetViewport(this);
            if (viewport == null) return true;
            
            var visible = State.GetVisibleBounds(viewport.ToViewport());
            var world = Director.Scene.WorldBounds;
            var cullingBounds = RectangleF.Intersect(visible, world);
            
            return cullingBounds.IsEmpty == false && cullingBounds.Intersects(bounds);
        }

        public override bool IsInView(Vector2 point)
        {
            var viewport = Director.GetViewport(this);
            if (viewport == null) return true;
            
            var visible = State.GetVisibleBounds(viewport.ToViewport());
            var world = Director.Scene.WorldBounds;
            var cullingBounds = RectangleF.Intersect(visible, world);
            
            return cullingBounds.IsEmpty == false && cullingBounds.Contains(point);
        }

        public override Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix proj = GetProjectionMatrix(viewport);
            Vector3 transformed = Vector3.Transform(new Vector3(worldPos, 0f), view * proj);
            return new Vector2(transformed.X, transformed.Y);
        }

        public override Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport)
        {
            Matrix view = GetViewMatrix();
            Matrix proj = GetProjectionMatrix(viewport);
            Matrix inverse = Matrix.Invert(view * proj);
            Vector3 transformed = Vector3.Transform(new Vector3(screenPos, 0f), inverse);
            return new Vector2(transformed.X, transformed.Y);
        }
    }
}
```

---

## 11. Camera/ScreenCamera.cs

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
{
    public class ScreenCamera : Camera
    {
        public ScreenCamera(string name, Vector2 origin, CameraDirector director)
            : base(name, CameraType.Screen, origin, director)
        {
        }

        protected override Matrix CalculateViewMatrix()
        {
            return Matrix.Identity;
        }

        public override Matrix GetProjectionMatrix(ViewportConfig viewport)
        {
            return Matrix.CreateOrthographicOffCenter(
                0, 1,
                1, 0,
                0f, 1f
            );
        }

        public override bool IsInView(RectangleF bounds)
        {
            return bounds.Left >= 0 && bounds.Right <= 1 &&
                   bounds.Top >= 0 && bounds.Bottom <= 1;
        }

        public override bool IsInView(Vector2 point)
        {
            return point.X >= 0 && point.X <= 1 &&
                   point.Y >= 0 && point.Y <= 1;
        }

        public override Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport)
        {
            return worldPos;
        }

        public override Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport)
        {
            return new Vector2(
                screenPos.X / viewport.Width,
                screenPos.Y / viewport.Height
            );
        }
    }
}
```

---

## 12. Camera/HybridCamera.cs

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera
{
    public class HybridCamera : Camera
    {
        public float PixelsPerUnit { get; set; } = 100f;

        public HybridCamera(string name, Vector2 origin, CameraDirector director)
            : base(name, CameraType.Hybrid, origin, director)
        {
        }

        protected override Matrix CalculateViewMatrix()
        {
            return Matrix.CreateTranslation(-State.Position.X, -State.Position.Y, 0f);
        }

        public override Matrix GetProjectionMatrix(ViewportConfig viewport)
        {
            float halfWidth = viewport.Width / (2f * PixelsPerUnit);
            float halfHeight = viewport.Height / (2f * PixelsPerUnit);
            
            return Matrix.CreateOrthographicOffCenter(
                -halfWidth, halfWidth,
                halfHeight, -halfHeight,
                0f, 1f
            );
        }

        public override bool IsInView(RectangleF bounds)
        {
            var viewport = Director.GetViewport(this);
            if (viewport == null) return true;
            
            float halfWidth = viewport.Width / (2f * PixelsPerUnit);
            float halfHeight = viewport.Height / (2f * PixelsPerUnit);
            
            var visible = new RectangleF(
                State.Position.X - halfWidth,
                State.Position.Y - halfHeight,
                halfWidth * 2f,
                halfHeight * 2f
            );
            
            return visible.Intersects(bounds);
        }

        public override bool IsInView(Vector2 point)
        {
            var viewport = Director.GetViewport(this);
            if (viewport == null) return true;
            
            float halfWidth = viewport.Width / (2f * PixelsPerUnit);
            float halfHeight = viewport.Height / (2f * PixelsPerUnit);
            
            return point.X >= State.Position.X - halfWidth &&
                   point.X <= State.Position.X + halfWidth &&
                   point.Y >= State.Position.Y - halfHeight &&
                   point.Y <= State.Position.Y + halfHeight;
        }

        public override Vector2 WorldToScreen(Vector2 worldPos, ViewportConfig viewport)
        {
            Vector2 offset = worldPos - State.Position;
            return new Vector2(
                viewport.Width / 2f + offset.X * PixelsPerUnit,
                viewport.Height / 2f + offset.Y * PixelsPerUnit
            );
        }

        public override Vector2 ScreenToWorld(Vector2 screenPos, ViewportConfig viewport)
        {
            float x = (screenPos.X - viewport.Width / 2f) / PixelsPerUnit;
            float y = (screenPos.Y - viewport.Height / 2f) / PixelsPerUnit;
            return State.Position + new Vector2(x, y);
        }
    }
}
```

---

## 13. Camera/ViewportConfig.cs

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
{
    public class ViewportConfig
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public float ScaleX { get; set; } = 1f;
        public float ScaleY { get; set; } = 1f;
        
        public bool MaintainAspectRatio { get; set; } = true;
        public Color LetterboxColor { get; set; } = Color.Black;

        public Viewport ToViewport()
        {
            return new Viewport(X, Y, Width, Height);
        }

        public Rectangle GetDestinationRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }
}
```

---

## 14. Camera/CameraDirector.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Camera
{
    public class CameraDirector
    {
        public Scene.Scene Scene { get; }
        public List<Camera> Cameras { get; } = new List<Camera>();
        
        private readonly Dictionary<Camera, ViewportConfig> _viewportConfigs = new Dictionary<Camera, ViewportConfig>();
        private readonly Dictionary<Camera, RenderQueue> _renderQueues = new Dictionary<Camera, RenderQueue>();

        public CameraDirector(Scene.Scene scene)
        {
            Scene = scene;
        }

        private ViewportConfig CreateDefaultViewport()
        {
            return new ViewportConfig
            {
                X = 0,
                Y = 0,
                Width = 800,
                Height = 600
            };
        }

        public WorldCamera CreateWorldCamera(string name, Vector2 origin)
        {
            var camera = new WorldCamera(name, origin, this);
            Cameras.Add(camera);
            _viewportConfigs[camera] = CreateDefaultViewport();
            _renderQueues[camera] = new RenderQueue();
            return camera;
        }

        public ScreenCamera CreateScreenCamera(string name, Vector2 origin)
        {
            var camera = new ScreenCamera(name, origin, this);
            Cameras.Add(camera);
            _viewportConfigs[camera] = CreateDefaultViewport();
            _renderQueues[camera] = new RenderQueue();
            return camera;
        }

        public HybridCamera CreateHybridCamera(string name, Vector2 origin, float pixelsPerUnit)
        {
            var camera = new HybridCamera(name, origin, this)
            {
                PixelsPerUnit = pixelsPerUnit
            };
            Cameras.Add(camera);
            _viewportConfigs[camera] = CreateDefaultViewport();
            _renderQueues[camera] = new RenderQueue();
            return camera;
        }

        public void SetViewport(Camera camera, ViewportConfig config)
        {
            if (Cameras.Contains(camera))
            {
                _viewportConfigs[camera] = config;
            }
        }

        public ViewportConfig GetViewport(Camera camera)
        {
            if (_viewportConfigs.TryGetValue(camera, out var config))
            {
                return config;
            }
            return null;
        }

        public RenderQueue GetRenderQueue(Camera camera)
        {
            if (_renderQueues.TryGetValue(camera, out var queue))
            {
                return queue;
            }
            return null;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var camera in Cameras)
            {
                camera.Update(gameTime);
            }
        }

        public void Submit(Camera camera, RenderCommand command)
        {
            var queue = GetRenderQueue(camera);
            if (queue != null)
            {
                queue.Enqueue(command);
            }
        }

        public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            foreach (var camera in Cameras)
            {
                var viewport = _viewportConfigs[camera];
                var oldViewport = spriteBatch.GraphicsDevice.Viewport;
                
                spriteBatch.GraphicsDevice.Viewport = viewport.ToViewport();
                
                var queue = _renderQueues[camera];
                
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    camera.GetViewMatrix() * camera.GetProjectionMatrix(viewport)
                );

                queue.Execute(spriteBatch, defaultFont);
                
                spriteBatch.End();
                
                queue.Clear();
                
                spriteBatch.GraphicsDevice.Viewport = oldViewport;
            }
        }
    }
}
```

---

## 15. Camera/Behaviors/FollowBehavior.cs

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class FollowBehavior : CameraBehaviorBase
    {
        public override int Priority => 10;
        
        public Vector2 Target { get; set; }
        public float SmoothSpeed { get; set; } = 5f;
        public Vector2 Offset { get; set; } = Vector2.Zero;
        
        public Vector2 DeadZone { get; set; } = Vector2.Zero;
        public bool UseDeadZone { get; set; } = false;
        
        private Vector2 _currentVelocity;

        public FollowBehavior(Camera camera) : base(camera)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            Vector2 targetPos = Target + Offset;
            Vector2 currentPos = Camera.Position;
            
            if (UseDeadZone)
            {
                Vector2 diff = targetPos - currentPos;
                
                if (System.Math.Abs(diff.X) < DeadZone.X * 0.5f)
                {
                    targetPos.X = currentPos.X;
                }
                else
                {
                    float sign = System.Math.Sign(diff.X);
                    targetPos.X -= sign * DeadZone.X * 0.5f;
                }
                
                if (System.Math.Abs(diff.Y) < DeadZone.Y * 0.5f)
                {
                    targetPos.Y = currentPos.Y;
                }
                else
                {
                    float sign = System.Math.Sign(diff.Y);
                    targetPos.Y -= sign * DeadZone.Y * 0.5f;
                }
            }
            
            Vector2 newPos = MathUtils.Lerp(currentPos, targetPos, SmoothSpeed * deltaTime);
            Camera.SetPosition(newPos);
        }
    }
}
```

---

## 16. Camera/Behaviors/BoundsConstraintBehavior.cs

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class BoundsConstraintBehavior : CameraBehaviorBase
    {
        public override int Priority => 100;
        
        public bool SoftConstraint { get; set; } = false;
        public float Softness { get; set; } = 0.5f;

        public BoundsConstraintBehavior(Camera camera) : base(camera)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            var viewport = Camera.Director.GetViewport(Camera);
            if (viewport == null) return;
            
            var visible = Camera.State.GetVisibleBounds(viewport.ToViewport());
            var world = Camera.Director.Scene.WorldBounds;
            
            if (world.IsEmpty) return;
            
            float halfVisibleWidth = visible.Width * 0.5f;
            float halfVisibleHeight = visible.Height * 0.5f;
            
            Vector2 targetPos = Camera.Position;
            
            float minX = world.Left + halfVisibleWidth;
            float maxX = world.Right - halfVisibleWidth;
            float minY = world.Top + halfVisibleHeight;
            float maxY = world.Bottom - halfVisibleHeight;
            
            if (SoftConstraint)
            {
                if (targetPos.X < minX)
                {
                    float over = minX - targetPos.X;
                    targetPos.X = minX - over * Softness;
                }
                else if (targetPos.X > maxX)
                {
                    float over = targetPos.X - maxX;
                    targetPos.X = maxX + over * Softness;
                }
                
                if (targetPos.Y < minY)
                {
                    float over = minY - targetPos.Y;
                    targetPos.Y = minY - over * Softness;
                }
                else if (targetPos.Y > maxY)
                {
                    float over = targetPos.Y - maxY;
                    targetPos.Y = maxY + over * Softness;
                }
            }
            else
            {
                targetPos.X = MathUtils.Clamp(targetPos.X, minX, maxX);
                targetPos.Y = MathUtils.Clamp(targetPos.Y, minY, maxY);
            }
            
            Camera.SetPosition(targetPos);
        }
    }
}
```

---

## 17. Camera/Behaviors/MultiTargetFollowBehavior.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class MultiTargetFollowBehavior : CameraBehaviorBase
    {
        public override int Priority => 15;
        
        public List<Vector2> Targets { get; } = new List<Vector2>();
        public float SmoothSpeed { get; set; } = 3f;
        public Vector2 Offset { get; set; } = Vector2.Zero;
        
        public float MinZoom { get; set; } = 0.5f;
        public float MaxZoom { get; set; } = 2f;
        public float ZoomMargin { get; set; } = 100f;
        public float ZoomSmoothSpeed { get; set; } = 2f;
        
        private float _currentZoom;
        private Vector2 _currentCenter;

        public MultiTargetFollowBehavior(Camera camera) : base(camera)
        {
            _currentZoom = camera.Zoom;
            _currentCenter = camera.Position;
        }

        public void AddTarget(Vector2 target)
        {
            Targets.Add(target);
        }

        public void RemoveTarget(Vector2 target)
        {
            Targets.Remove(target);
        }

        public void ClearTargets()
        {
            Targets.Clear();
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (Targets.Count == 0) return;
            
            RectangleF bounds = CalculateTargetsBounds();
            Vector2 center = bounds.Center + Offset;
            
            float requiredZoom = CalculateRequiredZoom(bounds);
            requiredZoom = MathUtils.Clamp(requiredZoom, MinZoom, MaxZoom);
            
            _currentCenter = MathUtils.Lerp(_currentCenter, center, SmoothSpeed * deltaTime);
            _currentZoom = MathUtils.Lerp(_currentZoom, requiredZoom, ZoomSmoothSpeed * deltaTime);
            
            Camera.SetPosition(_currentCenter);
            Camera.SetZoom(_currentZoom);
        }

        private RectangleF CalculateTargetsBounds()
        {
            if (Targets.Count == 0) return RectangleF.Empty;
            
            float minX = Targets[0].X;
            float maxX = Targets[0].X;
            float minY = Targets[0].Y;
            float maxY = Targets[0].Y;
            
            for (int i = 1; i < Targets.Count; i++)
            {
                Vector2 target = Targets[i];
                
                if (target.X < minX) minX = target.X;
                if (target.X > maxX) maxX = target.X;
                if (target.Y < minY) minY = target.Y;
                if (target.Y > maxY) maxY = target.Y;
            }
            
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        private float CalculateRequiredZoom(RectangleF bounds)
        {
            var viewport = Camera.Director.GetViewport(Camera);
            if (viewport == null) return 1f;
            
            float viewWidth = viewport.Width;
            float viewHeight = viewport.Height;
            
            float contentWidth = bounds.Width + ZoomMargin * 2f;
            float contentHeight = bounds.Height + ZoomMargin * 2f;
            
            if (contentWidth <= 0 || contentHeight <= 0) return MaxZoom;
            
            float zoomX = viewWidth / contentWidth;
            float zoomY = viewHeight / contentHeight;
            
            return System.Math.Min(zoomX, zoomY);
        }

        public override void Reset()
        {
            base.Reset();
            Targets.Clear();
            _currentZoom = Camera.Zoom;
            _currentCenter = Camera.Position;
        }
    }
}
```

---

## 18. Camera/Behaviors/ShakeBehavior.cs

```csharp
using System;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class ShakeBehavior : CameraBehaviorBase
    {
        public override int Priority => 90;
        
        public float Intensity { get; set; } = 10f;
        public float Frequency { get; set; } = 20f;
        
        private float _currentTrauma;
        private Vector2 _originalPosition;
        private Random _random;
        private float _seed;

        public ShakeBehavior(Camera camera, float duration, float intensity) : base(camera)
        {
            Duration = duration;
            Intensity = intensity;
            _random = new Random();
            _seed = (float)_random.NextDouble() * 100f;
            _originalPosition = camera.Position;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (ElapsedTime == 0f)
            {
                _originalPosition = Camera.Position;
            }
            
            float t = ElapsedTime / Duration.Value;
            _currentTrauma = MathUtils.Clamp01(1f - t);
            _currentTrauma = _currentTrauma * _currentTrauma;
            
            float shake = _currentTrauma * Intensity;
            
            float noiseX = PerlinNoise(_seed + ElapsedTime * Frequency);
            float noiseY = PerlinNoise(_seed + 100f + ElapsedTime * Frequency);
            
            float offsetX = noiseX * shake;
            float offsetY = noiseY * shake;
            
            Camera.SetPosition(_originalPosition + new Vector2(offsetX, offsetY));
        }

        protected override void OnComplete()
        {
            Camera.SetPosition(_originalPosition);
        }

        private float PerlinNoise(float x)
        {
            return (float)(System.Math.Sin(x) * 0.5 + System.Math.Sin(x * 2.3) * 0.25 + System.Math.Sin(x * 4.7) * 0.125);
        }

        public override void Reset()
        {
            base.Reset();
            _seed = (float)_random.NextDouble() * 100f;
        }
    }
}
```

---

## 19. Camera/Behaviors/RecoilBehavior.cs

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class RecoilBehavior : CameraBehaviorBase
    {
        public override int Priority => 80;
        
        public Vector2 Direction { get; set; } = Vector2.Zero;
        public float MaxRecoil { get; set; } = 20f;
        public float RecoverySpeed { get; set; } = 10f;
        public float Decay { get; set; } = 0.9f;
        
        private Vector2 _currentRecoil;
        private Vector2 _originalPosition;

        public RecoilBehavior(Camera camera) : base(camera)
        {
        }

        public void AddRecoil(float amount, Vector2 direction)
        {
            Vector2 newRecoil = direction * amount;
            _currentRecoil += newRecoil;
            
            float length = _currentRecoil.Length();
            if (length > MaxRecoil)
            {
                _currentRecoil = _currentRecoil / length * MaxRecoil;
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (ElapsedTime == 0f)
            {
                _originalPosition = Camera.Position;
            }
            
            _currentRecoil *= Decay;
            
            Vector2 targetPos = _originalPosition + _currentRecoil;
            Camera.SetPosition(targetPos);
            
            _originalPosition = MathUtils.Lerp(
                _originalPosition, 
                Camera.Position - _currentRecoil, 
                RecoverySpeed * deltaTime
            );
            
            if (_currentRecoil.LengthSquared() < 0.01f)
            {
                IsComplete = true;
            }
        }

        protected override void OnComplete()
        {
            Camera.SetPosition(_originalPosition);
        }

        public override void Reset()
        {
            base.Reset();
            _currentRecoil = Vector2.Zero;
        }
    }
}
```

---

## 20. Camera/Behaviors/DollyZoomBehavior.cs

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class DollyZoomBehavior : CameraBehaviorBase
    {
        public override int Priority => 20;
        
        public Vector2 Target { get; set; }
        public float StartDistance { get; set; } = 300f;
        public float EndDistance { get; set; } = 150f;
        public float StartZoom { get; set; } = 1f;
        public float EndZoom { get; set; } = 2f;
        
        private Vector2 _startPosition;
        private float _initialZoom;

        public DollyZoomBehavior(Camera camera, float duration) : base(camera)
        {
            Duration = duration;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (ElapsedTime == 0f)
            {
                _startPosition = Camera.Position;
                _initialZoom = Camera.Zoom;
            }
            
            float t = ElapsedTime / Duration.Value;
            t = MathUtils.SmoothStep(0f, 1f, t);
            
            float currentDistance = MathUtils.Lerp(StartDistance, EndDistance, t);
            float currentZoom = MathUtils.Lerp(StartZoom, EndZoom, t);
            
            Vector2 toTarget = Target - _startPosition;
            toTarget.Normalize();
            
            Vector2 newPos = Target - toTarget * currentDistance;
            
            Camera.SetPosition(newPos);
            Camera.SetZoom(currentZoom);
        }

        protected override void OnComplete()
        {
            Camera.SetZoom(EndZoom);
        }

        public override void Reset()
        {
            base.Reset();
            Camera.SetZoom(_initialZoom);
        }
    }
}
```

---

## 21. Camera/Behaviors/CinematicPathBehavior.cs

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Camera.Behaviors
{
    public class CinematicPathBehavior : CameraBehaviorBase
    {
        public override int Priority => 5;
        
        public Vector2[] ControlPoints { get; set; }
        public float[] WaitTimes { get; set; }
        public bool Loop { get; set; } = false;
        
        private int _currentSegment;
        private float _segmentProgress;
        private float _waitTimer;

        public CinematicPathBehavior(Camera camera) : base(camera)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (ControlPoints == null || ControlPoints.Length < 2) return;
            
            if (_waitTimer > 0)
            {
                _waitTimer -= deltaTime;
                return;
            }
            
            int segments = ControlPoints.Length - 1;
            float segmentDuration = Duration.Value / segments;
            
            _segmentProgress += deltaTime / segmentDuration;
            
            while (_segmentProgress >= 1f && _currentSegment < segments)
            {
                _segmentProgress -= 1f;
                _currentSegment++;
                
                if (WaitTimes != null && _currentSegment < WaitTimes.Length)
                {
                    _waitTimer = WaitTimes[_currentSegment];
                }
            }
            
            if (_currentSegment >= segments)
            {
                if (Loop)
                {
                    _currentSegment = 0;
                    _segmentProgress = 0f;
                }
                else
                {
                    IsComplete = true;
                    return;
                }
            }
            
            Vector2 p0 = ControlPoints[System.Math.Max(0, _currentSegment - 1)];
            Vector2 p1 = ControlPoints[_currentSegment];
            Vector2 p2 = ControlPoints[System.Math.Min(ControlPoints.Length - 1, _currentSegment + 1)];
            Vector2 p3 = ControlPoints[System.Math.Min(ControlPoints.Length - 1, _currentSegment + 2)];
            
            Vector2 newPos = CatmullRom(p0, p1, p2, p3, _segmentProgress);
            Camera.SetPosition(newPos);
        }

        private Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;
            
            return 0.5f * (
                (2f * p1) +
                (-p0 + p2) * t +
                (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
                (-p0 + 3f * p1 - 3f * p2 + p3) * t3
            );
        }

        public override void Reset()
        {
            base.Reset();
            _currentSegment = 0;
            _segmentProgress = 0f;
            _waitTimer = 0f;
        }
    }
}
```

---

## 22. Rendering/RenderCommand.cs

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public abstract class RenderCommand
    {
        public RectangleF Bounds { get; set; }
        public Effect Effect { get; set; }
        
        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont);
    }
}
```

---

## 23. Rendering/TextureRenderCommand.cs

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public class TextureRenderCommand : RenderCommand
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Rectangle? DestinationRectangle { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            if (Texture == null) return;
            
            if (DestinationRectangle.HasValue)
            {
                Rectangle dest = DestinationRectangle.Value;
                
                if (SourceRectangle.HasValue)
                {
                    spriteBatch.Draw(
                        Texture,
                        dest,
                        SourceRectangle.Value,
                        Color,
                        Rotation,
                        Origin,
                        Effects,
                        0f
                    );
                }
                else
                {
                    spriteBatch.Draw(
                        Texture,
                        dest,
                        null,
                        Color,
                        Rotation,
                        Origin,
                        Effects,
                        0f
                    );
                }
            }
            else
            {
                spriteBatch.Draw(
                    Texture,
                    Position,
                    SourceRectangle,
                    Color,
                    Rotation,
                    Origin,
                    Scale,
                    Effects,
                    0f
                );
            }
        }
    }
}
```

---

## 24. Rendering/TextRenderCommand.cs

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public class TextRenderCommand : RenderCommand
    {
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            SpriteFont fontToUse = Font ?? defaultFont;
            
            if (fontToUse == null) return;
            if (string.IsNullOrEmpty(Text)) return;
            
            spriteBatch.DrawString(
                fontToUse,
                Text,
                Position,
                Color,
                Rotation,
                Origin,
                Scale,
                Effects,
                0f
            );
        }
    }
}
```

---

## 25. Rendering/RenderQueue.cs

```csharp
using System.Collections.Generic;

namespace GameEngine.Rendering
{
    public class RenderQueue
    {
        private readonly List<RenderCommand> _commands = new List<RenderCommand>();

        public IReadOnlyList<RenderCommand> Commands => _commands;

        public void Clear()
        {
            _commands.Clear();
        }

        public void Enqueue(RenderCommand command)
        {
            _commands.Add(command);
        }

        public void EnqueueRange(IEnumerable<RenderCommand> commands)
        {
            foreach (var command in commands)
            {
                _commands.Add(command);
            }
        }

        public void Execute(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            foreach (var command in _commands)
            {
                command.Draw(spriteBatch, defaultFont);
            }
        }
    }
}
```

---

## 26. Rendering/IRenderable.cs

```csharp
using System.Collections.Generic;
using GameEngine.Camera;

namespace GameEngine.Rendering
{
    public interface IRenderable
    {
        void CollectRenderCommands(Camera camera, List<RenderCommand> commands);
        RectangleF GetRenderBounds();
        bool IsVisible(Camera camera);
    }
}
```

---

## 27. Rendering/PostProcess/IPostProcessEffect.cs

```csharp
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering.PostProcess
{
    public interface IPostProcessEffect
    {
        bool IsEnabled { get; set; }
        void Apply(SpriteBatch spriteBatch, RenderTarget2D source, RenderTarget2D destination);
    }
}
```

---

## 28. Rendering/PostProcess/PostProcessStack.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering.PostProcess
{
    public class PostProcessStack
    {
        private readonly List<IPostProcessEffect> _effects = new List<IPostProcessEffect>();
        private readonly RenderTargetPool _targetPool;
        private readonly GraphicsDevice _device;

        public PostProcessStack(GraphicsDevice device, RenderTargetPool targetPool)
        {
            _device = device;
            _targetPool = targetPool;
        }

        public void AddEffect(IPostProcessEffect effect)
        {
            _effects.Add(effect);
        }

        public void RemoveEffect(IPostProcessEffect effect)
        {
            _effects.Remove(effect);
        }

        public RenderTarget2D Process(SpriteBatch spriteBatch, RenderTarget2D source, int targetWidth, int targetHeight)
        {
            if (_effects.Count == 0) return source;

            RenderTarget2D current = source;
            RenderTarget2D next = null;

            for (int i = 0; i < _effects.Count; i++)
            {
                var effect = _effects[i];
                
                if (effect.IsEnabled == false) continue;

                bool isLast = (i == _effects.Count - 1);
                
                if (isLast == false)
                {
                    next = _targetPool.Acquire(targetWidth, targetHeight);
                }

                _device.SetRenderTarget(isLast ? null : next);
                effect.Apply(spriteBatch, current, isLast ? null : next);

                if (current != source)
                {
                    _targetPool.Release(current);
                }

                current = next;
            }

            return current;
        }
    }
}
```

---

## 29. Entities/Actor.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Camera;
using GameEngine.Rendering;

namespace GameEngine.Entities
{
    public abstract class Actor : IRenderable
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        
        protected Actor(Vector2 position)
        {
            Position = position;
        }

        public abstract void Update(float deltaTime);
        
        public void CollectRenderCommands(Camera camera, List<RenderCommand> commands)
        {
            if (IsActive == false) return;
            if (IsVisible == false) return;
            if (IsVisible(camera) == false) return;

            IEnumerable<RenderCommand> generatedCommands;
            
            if (camera.Type == CameraType.Screen)
            {
                generatedCommands = GenerateScreenCommands();
            }
            else
            {
                generatedCommands = GenerateWorldCommands();
            }
            
            foreach (var command in generatedCommands)
            {
                commands.Add(command);
            }
        }

        protected abstract IEnumerable<RenderCommand> GenerateWorldCommands();
        protected abstract IEnumerable<RenderCommand> GenerateScreenCommands();

        public abstract RectangleF GetRenderBounds();

        public virtual bool IsVisible(Camera camera)
        {
            return camera.IsInView(GetRenderBounds());
        }
    }
}
```

---

## 30. Entities/Actors/SpriteActor.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Rendering;

namespace GameEngine.Entities.Actors
{
    public class SpriteActor : Actor
    {
        public Texture2D Texture { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Color Tint { get; set; } = Color.White;
        public Vector2 Origin { get; set; }

        public SpriteActor(Vector2 position, Texture2D texture) 
            : base(position)
        {
            Texture = texture;
            Origin = texture.Bounds.Center.ToVector2();
        }

        public override void Update(float deltaTime)
        {
        }

        protected override IEnumerable<RenderCommand> GenerateWorldCommands()
        {
            if (Texture == null) 
            {
                yield break;
            }

            yield return new TextureRenderCommand
            {
                Texture = Texture,
                Position = Position,
                SourceRectangle = SourceRectangle,
                Color = Tint,
                Rotation = Rotation,
                Origin = Origin,
                Scale = Scale,
                Bounds = GetRenderBounds()
            };
        }

        protected override IEnumerable<RenderCommand> GenerateScreenCommands()
        {
            yield break;
        }

        public override RectangleF GetRenderBounds()
        {
            if (Texture == null) 
            {
                return new RectangleF(Position, Vector2.Zero);
            }

            Vector2 sourceSize;
            
            if (SourceRectangle.HasValue)
            {
                sourceSize = new Vector2(
                    SourceRectangle.Value.Width,
                    SourceRectangle.Value.Height
                );
            }
            else
            {
                sourceSize = Texture.Bounds.Size.ToVector2();
            }
            
            Vector2 size = sourceSize * Scale;
            Vector2 halfSize = size * 0.5f;
            Vector2 min = Position - halfSize;
            
            return new RectangleF(min, size);
        }
    }
}
```

---

## 31. Entities/Actors/TextActor.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Rendering;

namespace GameEngine.Entities.Actors
{
    public class TextActor : Actor
    {
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; } = Color.White;
        public Vector2 Origin { get; set; }

        public TextActor(Vector2 position, string text, SpriteFont font) 
            : base(position)
        {
            Text = text;
            Font = font;
        }

        public override void Update(float deltaTime)
        {
        }

        protected override IEnumerable<RenderCommand> GenerateWorldCommands()
        {
            yield break;
        }

        protected override IEnumerable<RenderCommand> GenerateScreenCommands()
        {
            if (string.IsNullOrEmpty(Text))
            {
                yield break;
            }

            yield return new TextRenderCommand
            {
                Text = Text,
                Font = Font,
                Position = Position,
                Color = Color,
                Rotation = Rotation,
                Origin = Origin,
                Scale = Scale,
                Bounds = GetRenderBounds()
            };
        }

        public override RectangleF GetRenderBounds()
        {
            if (Font == null || string.IsNullOrEmpty(Text))
            {
                return new RectangleF(Position, Vector2.Zero);
            }

            Vector2 size = Font.MeasureString(Text) * Scale;
            Vector2 halfSize = size * 0.5f;
            Vector2 min = Position - halfSize;
            
            return new RectangleF(min, size);
        }
    }
}
```

---

## 32. Layers/ILayerBehavior.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers
{
    public interface ILayerBehavior
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        float? Duration { get; }
        float ElapsedTime { get; }
        bool IsComplete { get; }
        
        void Update(float deltaTime);
        void Apply(List<RenderCommand> commands, Camera camera);
        void Reset();
    }
}
```

---

## 33. Layers/LayerBehaviorBase.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers
{
    public abstract class LayerBehaviorBase : ILayerBehavior
    {
        protected readonly Layer Layer;
        
        public bool IsActive { get; set; } = true;
        public abstract int Priority { get; }
        
        public float? Duration { get; protected set; }
        public float ElapsedTime { get; private set; }
        public bool IsComplete { get; protected set; }

        protected LayerBehaviorBase(Layer layer)
        {
            Layer = layer;
        }

        public void Update(float deltaTime)
        {
            if (IsActive == false) return;
            if (IsComplete) return;

            ElapsedTime += deltaTime;

            if (Duration.HasValue && ElapsedTime >= Duration.Value)
            {
                IsComplete = true;
                OnComplete();
                return;
            }

            OnUpdate(deltaTime);
        }

        protected abstract void OnUpdate(float deltaTime);
        public abstract void Apply(List<RenderCommand> commands, Camera camera);

        public virtual void Reset()
        {
            ElapsedTime = 0f;
            IsComplete = false;
            IsActive = true;
        }

        protected virtual void OnComplete() { }
    }
}
```

---

## 34. Layers/Layer.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Entities;
using GameEngine.Camera;
using GameEngine.Rendering;

namespace GameEngine.Layers
{
    public class Layer
    {
        public string Name { get; }
        public int DrawOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        
        private readonly List<Actor> _actors = new List<Actor>();
        private readonly List<ILayerBehavior> _behaviors = new List<ILayerBehavior>();
        
        public IReadOnlyList<Actor> Actors => _actors;
        public IReadOnlyList<ILayerBehavior> Behaviors => _behaviors;
        
        public uint CameraMask { get; set; } = 0xFFFFFFFF;

        public Layer(string name, int drawOrder)
        {
            Name = name;
            DrawOrder = drawOrder;
        }

        public void AddActor(Actor actor)
        {
            _actors.Add(actor);
        }

        public void RemoveActor(Actor actor)
        {
            _actors.Remove(actor);
        }

        public void AddBehavior(ILayerBehavior behavior)
        {
            _behaviors.Add(behavior);
        }

        public void RemoveBehavior(ILayerBehavior behavior)
        {
            _behaviors.Remove(behavior);
        }

        public void Update(float deltaTime)
        {
            if (IsActive == false) return;

            foreach (var actor in _actors)
            {
                actor.Update(deltaTime);
            }

            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                var behavior = _behaviors[i];
                
                if (behavior.IsActive == false) continue;
                
                behavior.Update(deltaTime);
                
                if (behavior.IsComplete && behavior.Duration.HasValue)
                {
                    _behaviors.RemoveAt(i);
                }
            }
        }

        public void CollectRenderCommands(CameraDirector cameraDirector)
        {
            if (IsVisible == false) return;

            List<Camera> targetCameras = new List<Camera>();
            
            for (int i = 0; i < cameraDirector.Cameras.Count; i++)
            {
                var camera = cameraDirector.Cameras[i];
                uint cameraBit = (uint)(1 << i);
                
                if ((CameraMask & cameraBit) != 0)
                {
                    targetCameras.Add(camera);
                }
            }

            foreach (var camera in targetCameras)
            {
                List<RenderCommand> commands = new List<RenderCommand>();

                foreach (var actor in _actors)
                {
                    if (actor.IsActive == false) continue;
                    if (actor.IsVisible(camera) == false) continue;
                    
                    actor.CollectRenderCommands(camera, commands);
                }

                List<ILayerBehavior> activeBehaviors = new List<ILayerBehavior>();
                
                foreach (var behavior in _behaviors)
                {
                    if (behavior.IsActive && behavior.IsComplete == false)
                    {
                        activeBehaviors.Add(behavior);
                    }
                }
                
                activeBehaviors.Sort((a, b) => a.Priority.CompareTo(b.Priority));

                foreach (var behavior in activeBehaviors)
                {
                    behavior.Apply(commands, camera);
                }

                foreach (var command in commands)
                {
                    cameraDirector.Submit(camera, command);
                }
            }
        }
    }
}
```

---

## 35. Layers/LayerDirector.cs

```csharp
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Layers
{
    public class LayerDirector
    {
        private readonly List<Layer> _layers = new List<Layer>();

        public IReadOnlyList<Layer> Layers => _layers;

        public Layer CreateLayer(string name, int drawOrder)
        {
            var layer = new Layer(name, drawOrder);
            _layers.Add(layer);
            return layer;
        }

        public void RemoveLayer(Layer layer)
        {
            _layers.Remove(layer);
        }

        public void SortLayersByDrawOrder()
        {
            _layers.Sort((a, b) => a.DrawOrder.CompareTo(b.DrawOrder));
        }

        public void Update(float deltaTime)
        {
            foreach (var layer in _layers)
            {
                layer.Update(deltaTime);
            }
        }

        public void CollectRenderCommands(Camera.CameraDirector cameraDirector)
        {
            SortLayersByDrawOrder();
            
            foreach (var layer in _layers)
            {
                layer.CollectRenderCommands(cameraDirector);
            }
        }
    }
}
```

---

## 36. Layers/Behaviors/ParallaxBehavior.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class ParallaxBehavior : LayerBehaviorBase
    {
        public override int Priority => 10;
        
        public Vector2 Factor { get; set; } = new Vector2(0.5f, 0.5f);
        public Vector2 BasePosition { get; set; } = Vector2.Zero;

        public ParallaxBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            Vector2 cameraPos = camera.Position;
            Vector2 parallaxOffset = BasePosition - cameraPos * Factor;
            Vector2 delta = parallaxOffset - BasePosition + cameraPos;
            
            foreach (var command in commands)
            {
                command.Bounds = new RectangleF(
                    command.Bounds.Position + delta,
                    command.Bounds.Size
                );
                
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Position += delta;
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Position += delta;
                }
            }
        }
    }
}
```

---

## 37. Layers/Behaviors/ScrollBehavior.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class ScrollBehavior : LayerBehaviorBase
    {
        public override int Priority => 20;
        
        public Vector2 Speed { get; set; } = Vector2.Zero;
        public bool Wrap { get; set; } = false;
        public Vector2 WrapBounds { get; set; } = Vector2.Zero;
        
        private Vector2 _currentOffset;

        public ScrollBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            _currentOffset += Speed * deltaTime;
            
            if (Wrap && WrapBounds != Vector2.Zero)
            {
                _currentOffset.X = _currentOffset.X % WrapBounds.X;
                _currentOffset.Y = _currentOffset.Y % WrapBounds.Y;
                
                if (_currentOffset.X < 0) _currentOffset.X += WrapBounds.X;
                if (_currentOffset.Y < 0) _currentOffset.Y += WrapBounds.Y;
            }
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            foreach (var command in commands)
            {
                command.Bounds = new RectangleF(
                    command.Bounds.Position + _currentOffset,
                    command.Bounds.Size
                );
                
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Position += _currentOffset;
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Position += _currentOffset;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            _currentOffset = Vector2.Zero;
        }
    }
}
```

---

## 38. Layers/Behaviors/TintBehavior.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class TintBehavior : LayerBehaviorBase
    {
        public override int Priority => 50;
        
        public Color Tint { get; set; } = Color.White;
        public float Intensity { get; set; } = 1f;

        public TintBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            Color blended = new Color(
                (byte)(Tint.R * Intensity),
                (byte)(Tint.G * Intensity),
                (byte)(Tint.B * Intensity),
                (byte)(Tint.A * Intensity)
            );
            
            foreach (var command in commands)
            {
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Color = BlendColors(texCmd.Color, blended);
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Color = BlendColors(textCmd.Color, blended);
                }
            }
        }

        private Color BlendColors(Color baseColor, Color tint)
        {
            return new Color(
                (byte)((baseColor.R * tint.R) / 255),
                (byte)((baseColor.G * tint.G) / 255),
                (byte)((baseColor.B * tint.B) / 255),
                (byte)((baseColor.A * tint.A) / 255)
            );
        }
    }
}
```

---

## 39. Layers/Behaviors/WaveDistortionBehavior.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class WaveDistortionBehavior : LayerBehaviorBase
    {
        public override int Priority => 30;
        
        public float Amplitude { get; set; } = 5f;
        public float Frequency { get; set; } = 0.1f;
        public float Speed { get; set; } = 2f;
        public bool Vertical { get; set; } = false;
        
        private float _phase;

        public WaveDistortionBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            _phase += Speed * deltaTime;
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            foreach (var command in commands)
            {
                float pos = Vertical ? command.Bounds.Position.Y : command.Bounds.Position.X;
                float wave = System.MathF.Sin(pos * Frequency + _phase) * Amplitude;
                
                Vector2 offset = Vertical ? new Vector2(0, wave) : new Vector2(wave, 0);
                
                command.Bounds = new RectangleF(
                    command.Bounds.Position + offset,
                    command.Bounds.Size
                );
                
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Position += offset;
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Position += offset;
                }
            }
        }
    }
}
```

---

## 40. Layers/Behaviors/FlashBehavior.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class FlashBehavior : LayerBehaviorBase
    {
        public override int Priority => 60;
        
        public Color FlashColor { get; set; } = Color.White;
        public float FadeOutSpeed { get; set; } = 5f;
        
        private float _currentIntensity;

        public FlashBehavior(Layer layer, float duration) : base(layer)
        {
            Duration = duration;
            _currentIntensity = 1f;
        }

        protected override void OnUpdate(float deltaTime)
        {
            _currentIntensity -= FadeOutSpeed * deltaTime;
            
            if (_currentIntensity <= 0)
            {
                _currentIntensity = 0;
                IsComplete = true;
            }
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            Color flash = new Color(
                (byte)(FlashColor.R * _currentIntensity),
                (byte)(FlashColor.G * _currentIntensity),
                (byte)(FlashColor.B * _currentIntensity),
                (byte)(255 * _currentIntensity)
            );
            
            foreach (var command in commands)
            {
                if (command is TextureRenderCommand texCmd)
                {
                    texCmd.Color = BlendAdditive(texCmd.Color, flash);
                }
                else if (command is TextRenderCommand textCmd)
                {
                    textCmd.Color = BlendAdditive(textCmd.Color, flash);
                }
            }
        }

        private Color BlendAdditive(Color baseColor, Color blend)
        {
            return new Color(
                (byte)System.Math.Min(255, baseColor.R + blend.R),
                (byte)System.Math.Min(255, baseColor.G + blend.G),
                (byte)System.Math.Min(255, baseColor.B + blend.B),
                baseColor.A
            );
        }
    }
}
```

---

## 41. Layers/Behaviors/MaskBehavior.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Camera;

namespace GameEngine.Layers.Behaviors
{
    public class MaskBehavior : LayerBehaviorBase
    {
        public override int Priority => 70;
        
        public enum MaskShape { Rectangle, Circle, Ellipse }
        
        public MaskShape Shape { get; set; } = MaskShape.Circle;
        public Vector2 Center { get; set; }
        public float Radius { get; set; } = 100f;
        public Vector2 Size { get; set; } = new Vector2(200, 200);
        public bool Invert { get; set; } = false;
        public float Softness { get; set; } = 0f;

        public MaskBehavior(Layer layer) : base(layer)
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        public override void Apply(List<RenderCommand> commands, Camera camera)
        {
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                var command = commands[i];
                bool inside = IsInside(command.Bounds.Center);
                
                if (Invert)
                {
                    inside = !inside;
                }
                
                if (inside == false)
                {
                    if (Softness > 0 && IsNearEdge(command.Bounds.Center))
                    {
                        ApplySoftness(command);
                    }
                    else
                    {
                        commands.RemoveAt(i);
                    }
                }
            }
        }

        private bool IsInside(Vector2 point)
        {
            switch (Shape)
            {
                case MaskShape.Rectangle:
                    float halfW = Size.X * 0.5f;
                    float halfH = Size.Y * 0.5f;
                    return point.X >= Center.X - halfW && point.X <= Center.X + halfW &&
                           point.Y >= Center.Y - halfH && point.Y <= Center.Y + halfH;
                           
                case MaskShape.Circle:
                    return (point - Center).LengthSquared() <= Radius * Radius;
                    
                case MaskShape.Ellipse:
                    float dx = (point.X - Center.X) / (Size.X * 0.5f);
                    float dy = (point.Y - Center.Y) / (Size.Y * 0.5f);
                    return dx * dx + dy * dy <= 1f;
            }
            
            return false;
        }

        private bool IsNearEdge(Vector2 point)
        {
            return true;
        }

        private void ApplySoftness(RenderCommand command)
        {
            if (command is TextureRenderCommand texCmd)
            {
                texCmd.Color = new Color(
                    texCmd.Color.R,
                    texCmd.Color.G,
                    texCmd.Color.B,
                    (byte)(texCmd.Color.A * 0.5f)
                );
            }
        }
    }
}
```

---

## 42. Scene/Scene.cs

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Camera;
using GameEngine.Layers;
using GameEngine.Math;
using GameEngine.Rendering;
using GameEngine.Optimization;
using GameEngine.Transitions;
using GameEngine.Debug;

namespace GameEngine.Scene
{
    public class Scene
    {
        public RectangleF WorldBounds { get; set; }
        
        public CameraDirector CameraDirector { get; }
        public LayerDirector LayerDirector { get; }
        public RenderTargetPool RenderTargetPool { get; }
        public PostProcessStack PostProcess { get; }
        public TransitionSystem TransitionSystem { get; }
        public SpatialHash SpatialHash { get; }
        public BehaviorDebugger Debugger { get; set; }

        public Scene(GraphicsDevice device, SpriteBatch spriteBatch, SpriteFont font, RectangleF worldBounds)
        {
            WorldBounds = worldBounds;
            
            CameraDirector = new CameraDirector(this);
            LayerDirector = new LayerDirector();
            RenderTargetPool = new RenderTargetPool(device);
            PostProcess = new PostProcessStack(device, RenderTargetPool);
            TransitionSystem = new TransitionSystem(device, spriteBatch);
            SpatialHash = new SpatialHash(256f);
            Debugger = new BehaviorDebugger(spriteBatch, font, device);
        }

        public void Update(GameTime gameTime)
        {
            CameraDirector.Update(gameTime);
            
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            LayerDirector.Update(delta);
            
            TransitionSystem.Update(gameTime);
            
            SpatialHash.Clear();
            foreach (var layer in LayerDirector.Layers)
            {
                foreach (var actor in layer.Actors)
                {
                    if (actor is ICullable cullable)
                    {
                        SpatialHash.Insert(cullable);
                    }
                }
            }
        }

        public void CollectRenderCommands()
        {
            LayerDirector.CollectRenderCommands(CameraDirector);
        }

        public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            CameraDirector.Render(spriteBatch, defaultFont);
            
            Debugger.DrawCameraDebug(CameraDirector);
            Debugger.DrawLayerDebug(LayerDirector);
        }
    }
}
```

---

## 43. Optimization/ICullable.cs

```csharp
using GameEngine.Math;

namespace GameEngine.Optimization
{
    public interface ICullable
    {
        RectangleF GetBounds();
    }
}
```

---

## 44. Optimization/SpatialHash.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Math;

namespace GameEngine.Optimization
{
    public class SpatialHash
    {
        private readonly float _cellSize;
        private readonly Dictionary<long, List<ICullable>> _cells = new Dictionary<long, List<ICullable>>();

        public SpatialHash(float cellSize)
        {
            _cellSize = cellSize;
        }

        public void Insert(ICullable item)
        {
            var bounds = item.GetBounds();
            var minCell = GetCellCoord(bounds.TopLeft);
            var maxCell = GetCellCoord(bounds.BottomRight);

            for (int x = minCell.X; x <= maxCell.X; x++)
            {
                for (int y = minCell.Y; y <= maxCell.Y; y++)
                {
                    long key = GetKey(x, y);
                    
                    if (_cells.TryGetValue(key, out var list) == false)
                    {
                        list = new List<ICullable>();
                        _cells[key] = list;
                    }
                    
                    list.Add(item);
                }
            }
        }

        public void Remove(ICullable item)
        {
            var bounds = item.GetBounds();
            var minCell = GetCellCoord(bounds.TopLeft);
            var maxCell = GetCellCoord(bounds.BottomRight);

            for (int x = minCell.X; x <= maxCell.X; x++)
            {
                for (int y = minCell.Y; y <= maxCell.Y; y++)
                {
                    long key = GetKey(x, y);
                    
                    if (_cells.TryGetValue(key, out var list))
                    {
                        list.Remove(item);
                    }
                }
            }
        }

        public void Query(RectangleF area, List<ICullable> results)
        {
            results.Clear();
            
            var minCell = GetCellCoord(area.TopLeft);
            var maxCell = GetCellCoord(area.BottomRight);

            var seen = new HashSet<ICullable>();

            for (int x = minCell.X; x <= maxCell.X; x++)
            {
                for (int y = minCell.Y; y <= maxCell.Y; y++)
                {
                    long key = GetKey(x, y);
                    
                    if (_cells.TryGetValue(key, out var list))
                    {
                        foreach (var item in list)
                        {
                            if (seen.Add(item))
                            {
                                if (area.Intersects(item.GetBounds()))
                                {
                                    results.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            _cells.Clear();
        }

        private Point GetCellCoord(Vector2 worldPos)
        {
            return new Point(
                (int)System.MathF.Floor(worldPos.X / _cellSize),
                (int)System.MathF.Floor(worldPos.Y / _cellSize)
            );
        }

        private long GetKey(int x, int y)
        {
            return ((long)x << 32) | (uint)y;
        }
    }
}
```

---

## 45. Transitions/TransitionType.cs

```csharp
namespace GameEngine.Transitions
{
    public enum TransitionType
    {
        Fade,
        WipeLeft,
        WipeRight,
        WipeUp,
        WipeDown,
        Circle,
        Pixelate
    }
}
```

---

## 46. Transitions/TransitionSystem.cs

```csharp
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Transitions
{
    public class TransitionSystem
    {
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        
        public bool IsTransitioning { get; private set; }
        public float Progress { get; private set; }
        public TransitionType CurrentType { get; private set; }
        
        private RenderTarget2D _fromTarget;
        private RenderTarget2D _toTarget;
        private float _duration;
        private Action _onComplete;

        public TransitionSystem(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            _device = device;
            _spriteBatch = spriteBatch;
        }

        public void StartTransition(
            RenderTarget2D from, 
            RenderTarget2D to, 
            TransitionType type, 
            float duration,
            Action onComplete = null)
        {
            _fromTarget = from;
            _toTarget = to;
            CurrentType = type;
            _duration = duration;
            _onComplete = onComplete;
            Progress = 0f;
            IsTransitioning = true;
        }

        public void Update(GameTime gameTime)
        {
            if (IsTransitioning == false) return;

            Progress += (float)gameTime.ElapsedGameTime.TotalSeconds / _duration;
            
            if (Progress >= 1f)
            {
                Progress = 1f;
                IsTransitioning = false;
                _onComplete?.Invoke();
            }
        }

        public void Draw()
        {
            if (IsTransitioning == false) return;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

            switch (CurrentType)
            {
                case TransitionType.Fade:
                    DrawFade();
                    break;
                case TransitionType.WipeLeft:
                    DrawWipe(new Vector2(-1, 0));
                    break;
                case TransitionType.WipeRight:
                    DrawWipe(new Vector2(1, 0));
                    break;
                case TransitionType.WipeUp:
                    DrawWipe(new Vector2(0, -1));
                    break;
                case TransitionType.WipeDown:
                    DrawWipe(new Vector2(0, 1));
                    break;
                case TransitionType.Circle:
                    DrawCircle();
                    break;
                case TransitionType.Pixelate:
                    DrawPixelate();
                    break;
            }

            _spriteBatch.End();
        }

        private void DrawFade()
        {
            float fromAlpha = 1f - Progress;
            float toAlpha = Progress;

            _spriteBatch.Draw(_fromTarget, Vector2.Zero, Color.White * fromAlpha);
            _spriteBatch.Draw(_toTarget, Vector2.Zero, Color.White * toAlpha);
        }

        private void DrawWipe(Vector2 direction)
        {
            Vector2 offset = direction * Progress * new Vector2(_fromTarget.Width, _fromTarget.Height);
            
            _spriteBatch.Draw(_fromTarget, offset, Color.White);
            _spriteBatch.Draw(_toTarget, offset - direction * new Vector2(_fromTarget.Width, _fromTarget.Height), Color.White);
        }

        private void DrawCircle()
        {
            _spriteBatch.Draw(_fromTarget, Vector2.Zero, Color.White);
        }

        private void DrawPixelate()
        {
            _spriteBatch.Draw(_fromTarget, Vector2.Zero, Color.White);
        }
    }
}
```

---

## 47. Debug/BehaviorDebugger.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Camera;
using GameEngine.Layers;

namespace GameEngine.Debug
{
    public class BehaviorDebugger
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;
        private readonly Texture2D _pixel;

        public bool ShowCameraBounds { get; set; } = true;
        public bool ShowLayerInfo { get; set; } = true;
        public bool ShowBehaviorInfo { get; set; } = true;

        public BehaviorDebugger(SpriteBatch spriteBatch, SpriteFont font, GraphicsDevice device)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void DrawCameraDebug(CameraDirector director)
        {
            if (ShowCameraBounds == false) return;

            foreach (var camera in director.Cameras)
            {
                var viewport = director.GetViewport(camera);
                if (viewport == null) continue;

                var visible = camera.State.GetVisibleBounds(viewport.ToViewport());
                DrawRectangle(visible, Color.Yellow * 0.3f);
                DrawRectangleOutline(visible, Color.Yellow);

                DrawString($"Camera: {camera.Name}", new Vector2(viewport.X + 5, viewport.Y + 5), Color.White);
                DrawString($"Zoom: {camera.Zoom:F2}", new Vector2(viewport.X + 5, viewport.Y + 25), Color.White);
            }
        }

        public void DrawLayerDebug(LayerDirector director)
        {
            if (ShowLayerInfo == false) return;

            int y = 100;
            foreach (var layer in director.Layers)
            {
                DrawString($"Layer: {layer.Name} (Order: {layer.DrawOrder})", new Vector2(5, y), Color.Cyan);
                y += 20;
                DrawString($"  Actors: {layer.Actors.Count}, Behaviors: {layer.Behaviors.Count}", new Vector2(5, y), Color.Gray);
                y += 20;
            }
        }

        private void DrawRectangle(RectangleF rect, Color color)
        {
            _spriteBatch.Draw(_pixel, rect.Position, null, color, 0f, Vector2.Zero, rect.Size, SpriteEffects.None, 0f);
        }

        private void DrawRectangleOutline(RectangleF rect, Color color)
        {
            float thickness = 2f;
            
            _spriteBatch.Draw(_pixel, new Vector2(rect.Left, rect.Top), null, color, 0f, Vector2.Zero, new Vector2(rect.Width, thickness), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_pixel, new Vector2(rect.Left, rect.Bottom - thickness), null, color, 0f, Vector2.Zero, new Vector2(rect.Width, thickness), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_pixel, new Vector2(rect.Left, rect.Top), null, color, 0f, Vector2.Zero, new Vector2(thickness, rect.Height), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_pixel, new Vector2(rect.Right - thickness, rect.Top), null, color, 0f, Vector2.Zero, new Vector2(thickness, rect.Height), SpriteEffects.None, 0f);
        }

        private void DrawString(string text, Vector2 position, Color color)
        {
            _spriteBatch.DrawString(_font, text, position, color);
        }
    }
}
```

---

Este es el código completo del motor de cámaras 2D para MonoGame. ¿Necesitas alguna modificación adicional?
User: Cómo se podría añadir efecto de luz por ejemplo a un actor?
Kimi: Aquí está el sistema de iluminación añadido al motor.

---

## Nuevos Componentes de Iluminación

```
GameEngine/
├── Lighting/
│   ├── ILightSource.cs
│   ├── PointLight.cs
│   ├── SpotLight.cs
│   ├── AmbientLight.cs
│   ├── LightMap.cs
│   └── LightingSystem.cs
└── Rendering/
    └── LightRenderCommand.cs
```

---

## 1. Lighting/ILightSource.cs

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Lighting
{
    public interface ILightSource
    {
        Vector2 Position { get; set; }
        Color Color { get; set; }
        float Intensity { get; set; }
        float Range { get; set; }
        bool CastShadows { get; set; }
        bool IsActive { get; set; }
        
        void Apply(LightMap lightMap, Camera.Camera camera);
    }
}
```

---

## 2. Lighting/PointLight.cs

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Lighting
{
    public class PointLight : ILightSource
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Intensity { get; set; } = 1f;
        public float Range { get; set; } = 200f;
        public bool CastShadows { get; set; } = true;
        public bool IsActive { get; set; } = true;
        
        public float Falloff { get; set; } = 1.5f;
        public Texture2D CookieTexture { get; set; }

        public void Apply(LightMap lightMap, Camera.Camera camera)
        {
            if (IsActive == false) return;
            
            var viewport = camera.Director.GetViewport(camera);
            if (viewport == null) return;
            
            Vector2 screenPos = camera.WorldToScreen(Position, viewport);
            float screenRange = Range * camera.Zoom;
            
            lightMap.DrawPointLight(screenPos, screenRange, Color, Intensity, Falloff, CookieTexture);
        }
    }
}
```

---

## 3. Lighting/SpotLight.cs

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Lighting
{
    public class SpotLight : ILightSource
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float ConeAngle { get; set; } = 45f;
        public float ConeDecay { get; set; } = 2f;
        
        public Color Color { get; set; } = Color.White;
        public float Intensity { get; set; } = 1f;
        public float Range { get; set; } = 300f;
        public bool CastShadows { get; set; } = true;
        public bool IsActive { get; set; } = true;

        public void Apply(LightMap lightMap, Camera.Camera camera)
        {
            if (IsActive == false) return;
            
            var viewport = camera.Director.GetViewport(camera);
            if (viewport == null) return;
            
            Vector2 screenPos = camera.WorldToScreen(Position, viewport);
            float screenRange = Range * camera.Zoom;
            
            lightMap.DrawSpotLight(screenPos, screenRange, Rotation, ConeAngle, Color, Intensity, ConeDecay);
        }
    }
}
```

---

## 4. Lighting/AmbientLight.cs

```csharp
using Microsoft.Xna.Framework;

namespace GameEngine.Lighting
{
    public class AmbientLight
    {
        public Color Color { get; set; } = new Color(20, 20, 30);
        public float Intensity { get; set; } = 0.3f;
        
        public void Apply(LightMap lightMap)
        {
            lightMap.Clear(Color.Multiply(Color, Intensity));
        }
    }
}
```

---

## 5. Lighting/LightMap.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Lighting
{
    public class LightMap
    {
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        private readonly RenderTarget2D _lightTarget;
        private readonly Texture2D _pointLightTexture;
        private readonly Texture2D _spotLightTexture;
        
        private readonly BlendState _lightBlendState;
        private readonly BlendState _shadowBlendState;

        public LightMap(GraphicsDevice device, int width, int height)
        {
            _device = device;
            _spriteBatch = new SpriteBatch(device);
            
            _lightTarget = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            
            _pointLightTexture = CreatePointLightTexture(device, 256);
            _spotLightTexture = CreateSpotLightTexture(device, 256);
            
            _lightBlendState = new BlendState
            {
                ColorBlendFunction = BlendFunction.Add,
                ColorSourceBlend = Blend.One,
                ColorDestinationBlend = Blend.One
            };
            
            _shadowBlendState = new BlendState
            {
                ColorBlendFunction = BlendFunction.ReverseSubtract,
                ColorSourceBlend = Blend.One,
                ColorDestinationBlend = Blend.One
            };
        }

        private Texture2D CreatePointLightTexture(GraphicsDevice device, int size)
        {
            var texture = new Texture2D(device, size, size);
            Color[] data = new Color[size * size];
            
            Vector2 center = new Vector2(size / 2f, size / 2f);
            float maxDist = size / 2f;
            
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = (new Vector2(x, y) - center).Length();
                    float intensity = 1f - MathHelper.Clamp(dist / maxDist, 0f, 1f);
                    intensity = (float)System.Math.Pow(intensity, 1.5f);
                    
                    data[y * size + x] = new Color(intensity, intensity, intensity, intensity);
                }
            }
            
            texture.SetData(data);
            return texture;
        }

        private Texture2D CreateSpotLightTexture(GraphicsDevice device, int size)
        {
            var texture = new Texture2D(device, size, size);
            Color[] data = new Color[size * size];
            
            Vector2 center = new Vector2(size / 2f, size / 2f);
            float maxDist = size / 2f;
            
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Vector2 pos = new Vector2(x, y) - center;
                    float dist = pos.Length();
                    float angle = (float)System.Math.Atan2(pos.Y, pos.X);
                    
                    float intensity = 1f - MathHelper.Clamp(dist / maxDist, 0f, 1f);
                    float angleAttenuation = 1f - MathHelper.Clamp(System.Math.Abs(angle) / 0.5f, 0f, 1f);
                    
                    intensity *= angleAttenuation;
                    intensity = (float)System.Math.Pow(intensity, 2f);
                    
                    data[y * size + x] = new Color(intensity, intensity, intensity, intensity);
                }
            }
            
            texture.SetData(data);
            return texture;
        }

        public void Clear(Color ambientColor)
        {
            _device.SetRenderTarget(_lightTarget);
            _device.Clear(ambientColor);
            _device.SetRenderTarget(null);
        }

        public void DrawPointLight(Vector2 position, float radius, Color color, float intensity, float falloff, Texture2D cookie)
        {
            _device.SetRenderTarget(_lightTarget);
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, _lightBlendState);
            
            Texture2D texture = cookie ?? _pointLightTexture;
            float scale = (radius * 2f) / texture.Width;
            
            Color lightColor = new Color(
                (byte)(color.R * intensity),
                (byte)(color.G * intensity),
                (byte)(color.B * intensity),
                255
            );
            
            _spriteBatch.Draw(
                texture,
                position,
                null,
                lightColor,
                0f,
                new Vector2(texture.Width / 2f, texture.Height / 2f),
                scale,
                SpriteEffects.None,
                0f
            );
            
            _spriteBatch.End();
            _device.SetRenderTarget(null);
        }

        public void DrawSpotLight(Vector2 position, float range, float rotation, float coneAngle, Color color, float intensity, float decay)
        {
            _device.SetRenderTarget(_lightTarget);
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, _lightBlendState);
            
            float scale = (range * 2f) / _spotLightTexture.Width;
            
            Color lightColor = new Color(
                (byte)(color.R * intensity),
                (byte)(color.G * intensity),
                (byte)(color.B * intensity),
                255
            );
            
            _spriteBatch.Draw(
                _spotLightTexture,
                position,
                null,
                lightColor,
                rotation,
                new Vector2(_spotLightTexture.Width / 2f, 0),
                scale,
                SpriteEffects.None,
                0f
            );
            
            _spriteBatch.End();
            _device.SetRenderTarget(null);
        }

        public void ApplyToScene(SpriteBatch spriteBatch, RenderTarget2D sceneTarget)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Multiply);
            spriteBatch.Draw(_lightTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public RenderTarget2D GetLightTarget()
        {
            return _lightTarget;
        }
    }
}
```

---

## 6. Lighting/LightingSystem.cs

```csharp
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Lighting
{
    public class LightingSystem
    {
        private readonly List<ILightSource> _lights = new List<ILightSource>();
        private readonly LightMap _lightMap;
        private readonly AmbientLight _ambient;
        private readonly GraphicsDevice _device;

        public bool Enabled { get; set; } = true;
        public AmbientLight Ambient => _ambient;

        public LightingSystem(GraphicsDevice device, int width, int height)
        {
            _device = device;
            _lightMap = new LightMap(device, width, height);
            _ambient = new AmbientLight();
        }

        public void AddLight(ILightSource light)
        {
            _lights.Add(light);
        }

        public void RemoveLight(ILightSource light)
        {
            _lights.Remove(light);
        }

        public void ClearLights()
        {
            _lights.Clear();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Render(Camera.Camera camera)
        {
            if (Enabled == false) return;
            
            _ambient.Apply(_lightMap);
            
            foreach (var light in _lights)
            {
                light.Apply(_lightMap, camera);
            }
        }

        public void ApplyToSpriteBatch(SpriteBatch spriteBatch)
        {
            if (Enabled == false) return;
            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Multiply);
            spriteBatch.Draw(_lightMap.GetLightTarget(), Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public RenderTarget2D GetLightMap()
        {
            return _lightMap.GetLightTarget();
        }
    }
}
```

---

## 7. Rendering/LightRenderCommand.cs

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Rendering
{
    public class LightRenderCommand : RenderCommand
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public float NormalStrength { get; set; } = 1f;

        public override void Draw(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            if (Texture == null) return;
            
            spriteBatch.Draw(
                Texture,
                Position,
                null,
                Color,
                Rotation,
                Origin,
                Scale,
                Effects,
                0f
            );
        }
    }
}
```

---

## 8. Actor con Luz (Ejemplo)

```csharp
using Microsoft.Xna.Framework;
using GameEngine.Lighting;

namespace GameEngine.Entities.Actors
{
    public class LightActor : SpriteActor
    {
        public PointLight Light { get; private set; }

        public LightActor(Vector2 position, Texture2D texture, LightingSystem lighting) 
            : base(position, texture)
        {
            Light = new PointLight
            {
                Position = position,
                Color = Color.White,
                Intensity = 1f,
                Range = 150f,
                CastShadows = true
            };
            
            lighting.AddLight(Light);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            Light.Position = Position;
        }

        public void SetLightColor(Color color)
        {
            Light.Color = color;
        }

        public void Pulse(float intensity, float duration)
        {
            // Animación de pulso
        }
    }
}
```

---

## 9. Modificación a Scene para Iluminación

```csharp
public class Scene
{
    public LightingSystem LightingSystem { get; }

    public Scene(GraphicsDevice device, SpriteBatch spriteBatch, SpriteFont font, RectangleF worldBounds, int screenWidth, int screenHeight)
    {
        // ... código existente ...
        
        LightingSystem = new LightingSystem(device, screenWidth, screenHeight);
    }

    public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
    {
        // Renderizar luces para cada cámara
        foreach (var camera in CameraDirector.Cameras)
        {
            LightingSystem.Render(camera);
        }
        
        // Renderizar escena
        CameraDirector.Render(spriteBatch, defaultFont);
        
        // Aplicar iluminación
        LightingSystem.ApplyToSpriteBatch(spriteBatch);
        
        // Debug
        Debugger.DrawCameraDebug(CameraDirector);
        Debugger.DrawLayerDebug(LayerDirector);
    }
}
```

---

## 10. Uso de Iluminación

```csharp
// Setup
var scene = new Scene(device, spriteBatch, font, worldBounds, 800, 600);
scene.LightingSystem.Ambient.Color = new Color(10, 10, 20);
scene.LightingSystem.Ambient.Intensity = 0.2f;

// Jugador con linterna
var player = new LightActor(new Vector2(100, 100), playerTexture, scene.LightingSystem);
player.Light.Color = new Color(255, 240, 200);
player.Light.Range = 200f;
player.Light.Intensity = 1.2f;

// Antorcha estática
var torch = new PointLight
{
    Position = new Vector2(500, 300),
    Color = new Color(255, 150, 50),
    Range = 180f,
    Intensity = 0.8f,
    Falloff = 1.2f
};
scene.LightingSystem.AddLight(torch);

// Linterna de enemigo
var enemyLight = new SpotLight
{
    Position = enemy.Position,
    Rotation = enemy.Rotation,
    ConeAngle = 30f,
    Range = 250f,
    Color = new Color(255, 0, 0),
    Intensity = 0.9f
};
scene.LightingSystem.AddLight(enemyLight);

// En update
enemyLight.Position = enemy.Position;
enemyLight.Rotation = enemy.Rotation;
```

---

## 11. Shader de Iluminación Normal (Opcional)

```hlsl
// LightEffect.fx
sampler ColorSampler : register(s0);
sampler NormalSampler : register(s1);
sampler LightSampler : register(s2);

float2 ScreenSize;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(ColorSampler, texCoord);
    float4 normal = tex2D(NormalSampler, texCoord);
    float4 light = tex2D(LightSampler, texCoord);
    
    // Convertir normal de [0,1] a [-1,1]
    float3 normalVector = normalize(normal.rgb * 2.0 - 1.0);
    
    // Aplicar iluminación
    float3 litColor = color.rgb * light.rgb;
    
    return float4(litColor, color.a);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
```

---

¿Necesitas alguna modificación adicional al sistema de iluminación?
