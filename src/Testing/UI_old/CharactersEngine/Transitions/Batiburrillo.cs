using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VisualNovelEngine.Transitions;

namespace VisualNovelEngine
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TransitionManager _transitionManager;
        private Texture2D _bgScene1, _bgScene2, _character;
        private SpriteFont _font;
        
        // Estado del juego
        private int _currentScene = 1;
        private bool _isFading;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _transitionManager = new TransitionManager(GraphicsDevice, Content);
            
            // Cargar assets de ejemplo
            _bgScene1 = Content.Load<Texture2D>("Backgrounds/classroom");
            _bgScene2 = Content.Load<Texture2D>("Backgrounds/hallway");
            _character = Content.Load<Texture2D>("Characters/heroine");
            _font = Content.Load<SpriteFont>("Fonts/dialogue");
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Controles de prueba
            if (keyboard.IsKeyDown(Keys.D1) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.Fade);
                
            if (keyboard.IsKeyDown(Keys.D2) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.WipeRight);
                
            if (keyboard.IsKeyDown(Keys.D3) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.IrisIn);
                
            if (keyboard.IsKeyDown(Keys.D4) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.Pixelate);
                
            if (keyboard.IsKeyDown(Keys.D5) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.NoiseDissolve);
                
            if (keyboard.IsKeyDown(Keys.D6) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.ZoomBlur);
                
            if (keyboard.IsKeyDown(Keys.D7) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.ChromaticAberration);
                
            if (keyboard.IsKeyDown(Keys.D8) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.Ripple);
                
            if (keyboard.IsKeyDown(Keys.D9) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.PageTurnRight);
                
            if (keyboard.IsKeyDown(Keys.D0) && !_transitionManager.IsTransitioning)
                StartTransition(TransitionType.DirectionalBlurRight);
            
            // Transición solo de fondo (personajes estáticos)
            if (keyboard.IsKeyDown(Keys.Space) && !_transitionManager.IsTransitioning)
                StartBackgroundTransition();
            
            if (keyboard.IsKeyDown(Keys.Enter))
                _transitionManager.Skip();
            
            _transitionManager.Update(deltaTime);
            base.Update(gameTime);
        }
        
        private void StartTransition(TransitionType type)
        {
            var oldScene = _currentScene;
            _currentScene = _currentScene == 1 ? 2 : 1;
            
            var parameters = new TransitionParameters 
            { 
                Type = type, 
                Duration = 0.8f,
                Easing = EasingFunction.EaseInOutCubic
            };
            
            _transitionManager.StartTransition(
                parameters,
                () => RenderScene(oldScene), // Escena anterior
                () => RenderScene(_currentScene) // Escena nueva
            );
        }
        
        private void StartBackgroundTransition()
        {
            // Cambiar solo el fondo, mantener personaje
            var oldBg = _currentScene;
            _currentScene = _currentScene == 1 ? 2 : 1;
            
            _transitionManager.StartBackgroundTransition(
                TransitionParameters.Iris(1.0f),
                () => // Fondo viejo
                {
                    var bg = oldBg == 1 ? _bgScene1 : _bgScene2;
                    _spriteBatch.Draw(bg, Vector2.Zero, Color.White);
                },
                () => // Fondo nuevo
                {
                    var bg = _currentScene == 1 ? _bgScene1 : _bgScene2;
                    _spriteBatch.Draw(bg, Vector2.Zero, Color.White);
                },
                () => // Personajes (se mantienen fijos)
                {
                    _spriteBatch.Draw(_character, new Vector2(400, 200), Color.White);
                }
            );
        }

        protected override void Draw(GameTime gameTime)
        {
            if (_transitionManager.IsTransitioning)
            {
                // La transición maneja el renderizado
                _transitionManager.Draw();
                
                // Dibujar UI encima si es necesario
                _spriteBatch.Begin();
                _spriteBatch.DrawString(_font, "Transitioning...", new Vector2(10, 10), Color.White);
                _spriteBatch.End();
            }
            else
            {
                // Renderizado normal
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin();
                RenderScene(_currentScene);
                _spriteBatch.End();
                
                // UI de debug
                _spriteBatch.Begin();
                _spriteBatch.DrawString(_font, "1:Fade 2:Wipe 3:Iris 4:Pixelate 5:Noise", new Vector2(10, 10), Color.White);
                _spriteBatch.DrawString(_font, "6:Zoom 7:Chromatic 8:Ripple 9:Page 0:Blur", new Vector2(10, 40), Color.White);
                _spriteBatch.DrawString(_font, "Space: BgOnly Transition", new Vector2(10, 70), Color.Yellow);
                _spriteBatch.End();
            }
            
            base.Draw(gameTime);
        }
        
        private void RenderScene(int scene)
        {
            var bg = scene == 1 ? _bgScene1 : _bgScene2;
            _spriteBatch.Draw(bg, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_character, new Vector2(400, 200), Color.White);
        }
    }
}

/*
## 🛠️ COMPILACIÓN DE SHADERS

Para MonoGame, compila los `.fx` usando **MonoGame Content Pipeline** o **mgcb**:

```bash
# mgcb-editor comando de línea
mgcb /build:Effects/Transitions/Fade.fx
```

O en tu `.mgcb`:
```
#begin Effects/Transitions/Fade.fx
/importer:EffectImporter
/processor:EffectProcessor
/processorParam:DebugMode=Auto
/build:Effects/Transitions/Fade.fx

## 🎨 Variantes Creativas Adicionales

### **Barras con Offset (Cascada)**
```hlsl
float offset = barIndex * 0.05; // Retraso por barra
float adjustedProgress = saturate(Progress * 1.5 - offset);
```
Efecto: las barras se abren en secuencia como una cascada.

### **Barras con Color Intermedio**
```hlsl
float4 middleColor = float4(1,1,1,1); // Flash blanco entre medias
float4 result = lerp(from, to, blend);

// Añadir brillo en el borde de corte
float edge = abs(mask - threshold);
if (edge < 0.1) result = lerp(result, middleColor, 1 - edge * 10);
```

### **Barras Irregulares (Aleatorias)**
Usar una textura de ruido para determinar el grosor de cada barra en lugar de `BarCount` fijo.

---

## 🚀 Uso en el Juego

```csharp
// Transición por barras horizontales suaves
_transitionManager.StartTransition(
    new TransitionParameters 
    { 
        Type = TransitionType.BarsHorizontal,
        Duration = 1.2f,
        Intensity = 16 // Número de barras (opcional, si quieres parametrizar)
    },
    () => RenderCurrentScene(),
    () => RenderNextScene()
);

// Efecto persiana rápido para cambios de tiempo
_transitionManager.StartTransition(
    new TransitionParameters 
    { 
        Type = TransitionType.BarsAlternateHorizontal,
        Duration = 0.8f,
        Easing = EasingFunction.EaseInOutBack // Rebote al final
    },
    // ...
);
```

¿Quieres que añada alguna variante específica como barras con forma de arco o barras que se deslicen en lugar de abrirse?
User: Existen otras transiciones interesantes que podemos añadir?
Kimi: Aquí tienes una colección de transiciones avanzadas y creativas que elevarán la calidad visual de tu motor:

---

## 🎭 Transiciones Cinematográficas

### 1. **Dream/Blur Wipe** (Desenfoque direccional con dirección)
```hlsl
// Variante del DirectionalBlur pero con máscara de wipe
// La imagen se "difumina" en una dirección mientras aparece la nueva
```
Uso: Recuerdos borrosos, cambios de tiempo, desmayos.

---

### 2. **Shutter/Split** (Persianas partidas)
```hlsl
// La pantalla se divide en 2-4 partes que se deslizan en direcciones opuestas
float4 PS_Shutter(VertexShaderOutput input) : COLOR
{
    float2 uv = input.UV;
    float split = step(0.5, uv.x); // 0 o 1
    
    // Mitad izquierda se mueve arriba, derecha abajo
    float offset = (split - 0.5) * 2.0 * Progress * 0.5;
    uv.y += offset;
    
    // Clamp o mirror para evitar bordes negros
    uv.y = saturate(uv.y);
    
    float4 from = tex2D(PreviousSampler, uv);
    float4 to = tex2D(NextSampler, input.UV);
    
    // La nueva imagen aparece en el "hueco"
    float reveal = abs(offset) > 0.1 ? 1.0 : 0.0;
    return lerp(from, to, reveal);
}
```
Visual: La pantalla se parte como una puerta de ascensor.

---

### 3. **Tile/Flip** (Volteo de fichas)
```hlsl
// Divide en grid, cada celda rota 90° como dominó
float4 PS_TileFlip(VertexShaderOutput input) : COLOR
{
    int tilesX = 8;
    int tilesY = 6;
    
    float2 tileUV = input.UV * float2(tilesX, tilesY);
    float2 tileId = floor(tileUV);
    float2 localUV = frac(tileUV);
    
    // Retraso basado en posición (cascada)
    float delay = (tileId.x + tileId.y) / (tilesX + tilesY) * 0.5;
    float localProgress = saturate((Progress - delay) * 2.0);
    
    // Rotación 3D simulada
    float angle = localProgress * 1.5708; // 90 grados
    float cosA = cos(angle);
    
    // Perspectiva simple
    float2 distortedUV = localUV;
    distortedUV.x = (localUV.x - 0.5) / cosA + 0.5;
    
    // Seleccionar textura según lado de la "ficha"
    float4 color;
    if (localProgress < 0.5)
    {
        // Mitad del giro: from
        color = tex2D(PreviousSampler, (tileId + distortedUV) / float2(tilesX, tilesY));
    }
    else
    {
        // Mitad final: to
        color = tex2D(NextSampler, (tileId + distortedUV) / float2(tilesX, tilesY));
    }
    
    // Sombra del borde durante el giro
    float shadow = 1.0 - abs(localProgress - 0.5) * 2.0;
    color.rgb *= lerp(0.5, 1.0, shadow);
    
    return color;
}
```
Visual: Grid de fichas que se voltean como cartas de memoria.

---

### 4. **Ink Bleed** (Difusión de tinta)
```hlsl
// Simula gotas de tinta que se expanden
Texture2D NoiseTexture; // Ruido de Perlin o cellular

float4 PS_InkBleed(VertexShaderOutput input) : COLOR
{
    float noise = tex2D(NoiseSampler, input.UV).r;
    
    // Múltiples "gotas" con diferentes escalas
    float drop1 = smoothstep(Progress * 1.2, Progress * 1.2 + 0.1, noise);
    float drop2 = smoothstep(Progress * 0.8 - 0.2, Progress * 0.8 + 0.1, 
                   tex2D(NoiseSampler, input.UV * 2.0).r);
    
    float mask = max(drop1, drop2);
    
    // Bordes irregulares tipo tinta
    float edge = fwidth(mask) * 2.0;
    mask = smoothstep(0.5 - edge, 0.5 + edge, mask);
    
    float4 from = tex2D(PreviousSampler, input.UV);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Color sepia en la transición
    float4 inkColor = float4(0.2, 0.1, 0.05, 1.0);
    float4 result = lerp(from, to, mask);
    
    // Tinte en el borde de expansión
    float border = abs(mask - 0.5) < 0.05 ? 1.0 : 0.0;
    return lerp(result, inkColor, border * 0.3);
}
```
Visual: Manchas de tinta que se expanden organicamente (estilo sumi-e).

---

### 5. **Lens Distortion** (Ojo de pez/burbuja)
```hlsl
// Distorsión de lente que expande desde el centro
float4 PS_Lens(VertexShaderOutput input) : COLOR
{
    float2 center = float2(0.5, 0.5);
    float2 dir = input.UV - center;
    float dist = length(dir);
    
    // Curvatura de barril/pincushion
    float strength = sin(Progress * 3.14159) * 0.5;
    float curvature = 1.0 + strength * dist * dist;
    
    float2 distortedUV = center + dir * curvature;
    
    // Efecto cromático en los bordes
    float aberration = dist * strength * 0.05;
    
    float r = tex2D(PreviousSampler, distortedUV + aberration).r;
    float g = tex2D(PreviousSampler, distortedUV).g;
    float b = tex2D(PreviousSampler, distortedUV - aberration).b;
    
    float4 from = float4(r, g, b, 1.0);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Vignette durante la transición
    float vignette = 1.0 - dist * Progress;
    from *= vignette;
    
    return lerp(from, to, Progress);
}
```
Visual: Como mirar por una burbuja de jabón que explota.

---

### 6. **Glitch Digital** (Corrupción de datos)
```hlsl
// Bloques de píxeles que se desplazan, scanlines, color splitting
float Random(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

float4 PS_Glitch(VertexShaderOutput input) : COLOR
{
    float2 uv = input.UV;
    
    // Bloques que se desplazan horizontalmente
    float blockSize = 0.05;
    float2 blockId = floor(uv / blockSize);
    float blockRandom = Random(blockId + floor(Progress * 10));
    
    // Solo algunos bloques se mueven
    float shift = (blockRandom > 0.7) ? (Random(blockId) - 0.5) * Progress * 0.2 : 0.0;
    uv.x += shift;
    
    // Scanlines intermitentes
    float scanline = sin(uv.y * 800.0 + Progress * 10.0) > 0.9 ? 0.5 : 1.0;
    
    // RGB split aleatorio
    float rgbSplit = (Random(float2(Progress, blockId.y)) - 0.5) * Progress * 0.02;
    
    float4 from;
    from.r = tex2D(PreviousSampler, uv + float2(rgbSplit, 0)).r;
    from.g = tex2D(PreviousSampler, uv).g;
    from.b = tex2D(PreviousSampler, uv - float2(rgbSplit, 0)).b;
    from.a = 1.0;
    
    from.rgb *= scanline;
    
    // Bloques de "datos corruptos" (ruido)
    float noiseBlock = blockRandom > 0.95 ? Random(uv * Progress) : 0.0;
    from.rgb = lerp(from.rgb, float3(noiseBlock), Progress * 0.5);
    
    float4 to = tex2D(NextSampler, input.UV);
    
    // Transición con más glitch en el medio
    float glitchIntensity = 1.0 - abs(Progress - 0.5) * 2.0;
    return lerp(from, to, Progress + glitchIntensity * 0.1);
}
```
Visual: Corrupción de señal de TV, cyberpunk, horror tecnológico.

---

### 7. **Water Drop/Rain** (Gotas de lluvia)
```hlsl
// Múltiples gotas circulares que distorsionan y revelan
Texture2D DropTexture; // Normal map de gotas o calculado

float4 PS_RainDrop(VertexShaderOutput input) : COLOR
{
    // Simular N gotas
    float4 finalColor = tex2D(PreviousSampler, input.UV);
    
    for(int i = 0; i < 5; i++) // 5 gotas simultáneas
    {
        // Posición animada de cada gota
        float2 dropPos = float2(
            Random(float2(i, 0)) + sin(Progress * 3.0 + i) * 0.1,
            Random(float2(0, i)) - Progress * (1.0 + i * 0.2)
        );
        
        float dist = distance(input.UV, dropPos);
        float dropSize = 0.05 + i * 0.01;
        
        // Dentro de la gota: lupa + reflejo
        if (dist < dropSize)
        {
            float2 offset = (input.UV - dropPos) * 2.0;
            float2 refractUV = input.UV + offset * (1.0 - dist/dropSize);
            
            // La gota revela la siguiente imagen
            float4 dropColor = tex2D(NextSampler, refractUV);
            float alpha = smoothstep(dropSize, dropSize * 0.8, dist);
            
            finalColor = lerp(finalColor, dropColor, alpha * Progress);
        }
    }
    
    return finalColor;
}
```
Visual: Gotas de lluvia corriendo por el cristal que revelan otra escena.

---

### 8. **Mosaic/Block Dissolve** (Mosaico que se resuelve)
```hlsl
// Bloques que cambian de imagen en orden aleatorio o secuencial
float4 PS_Mosaic(VertexShaderOutput input) : COLOR
{
    float2 blockCount = float2(20.0, 15.0);
    float2 blockUV = floor(input.UV * blockCount) / blockCount;
    float2 localUV = frac(input.UV * blockCount);
    
    // Valor aleatorio por bloque
    float blockRandom = Random(blockUV);
    
    // Orden de aparición: aleatorio o secuencial
    float threshold = Progress;
    
    // Efecto "onda" desde esquina
    float wave = (blockUV.x + blockUV.y) / 2.0;
    blockRandom = lerp(blockRandom, wave, 0.5); // Mezcla aleatorio + secuencial
    
    float mask = step(blockRandom, threshold);
    
    // Suavizado por bloque (opcional)
    mask = smoothstep(threshold - 0.1, threshold + 0.1, blockRandom);
    
    float4 from = tex2D(PreviousSampler, input.UV);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Escala del bloque (zoom durante transición)
    float scale = lerp(1.0, 0.8, mask * (1.0 - mask) * 4.0); // Pulso en el cambio
    float2 centerUV = blockUV + localUV * scale + (1.0 - scale) * 0.5 / blockCount;
    
    from = tex2D(PreviousSampler, centerUV);
    to = tex2D(NextSampler, centerUV);
    
    return lerp(from, to, mask);
}
```
Visual: Como el efecto de transición de *Minecraft* o *The Witness*.

---

### 9. **Vortex/Twirl** (Espiral)
```hlsl
// Espiral que absorbe la imagen anterior
float4 PS_Vortex(VertexShaderOutput input) : COLOR
{
    float2 center = float2(0.5, 0.5);
    float2 dir = input.UV - center;
    
    float dist = length(dir);
    float angle = atan2(dir.y, dir.x);
    
    // Espiral de Arquímedes modificada
    float spiralStrength = Progress * 10.0;
    float newAngle = angle + dist * spiralStrength * (1.0 - Progress);
    
    float2 spiralUV = center + float2(cos(newAngle), sin(newAngle)) * dist;
    
    // Escala que se reduce hacia el centro
    float scale = lerp(1.0, 0.0, Progress * dist * 2.0);
    spiralUV = center + (spiralUV - center) * scale;
    
    float4 from = tex2D(PreviousSampler, spiralUV);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Vórtice oscuro en el centro
    float darkness = smoothstep(0.0, 0.3, dist) * Progress;
    from.rgb *= 1.0 - darkness;
    
    return lerp(from, to, Progress);
}
```
Visual: Agujero negro que succiona la escena, transiciones de terror.

---

### 10. **Flash/Whiteout** (Sobrexposición)
```hlsl
// Flash blanco intenso que quema la imagen
float4 PS_Flash(VertexShaderOutput input) : COLOR
{
    float4 from = tex2D(PreviousSampler, input.UV);
    float4 to = tex2D(NextSampler, input.UV);
    
    // Curva de intensidad: 0 -> 1 -> 0 con pico en medio
    float flash = sin(Progress * 3.14159); // Pico en 0.5
    
    // Sobrexposición
    float4 white = float4(1, 1, 1, 1);
    
    // Burn effect: las áreas claras se queman primero
    float luminance = dot(from.rgb, float3(0.299, 0.587, 0.114));
    float burnThreshold = flash * 2.0 - 1.0; // -1 a 1
    
    float burn = smoothstep(burnThreshold - 0.2, burnThreshold + 0.2, luminance);
    
    // Transición: from -> flash blanco -> to
    float4 flashed = lerp(from, white, burn * flash);
    
    // La nueva imagen aparece desde el flash
    float reveal = smoothstep(0.4, 0.6, Progress);
    
    return lerp(flashed, to, reveal);
}
```
Visual: Flash de cámara, explosiones, cambios de día a día.

---

## 📊 Tabla Comparativa

| Efecto | Complejidad | Estilo | Uso Recomendado |
|--------|-------------|--------|-----------------|
| **Dream Wipe** | Media | Suave | Flashbacks, emociones |
| **Shutter** | Baja | Mecánico | Suspense, revelaciones |
| **Tile Flip** | Alta | Lúdico | Juegos casuales, puzzles |
| **Ink Bleed** | Media | Artístico | Historias orientales, drama |
| **Lens** | Media | Surrealista | Psicodélico, fantasía |
| **Glitch** | Media | Tecnológico | Sci-fi, horror, moderno |
| **Rain Drop** | Alta | Natural | Romance melancólico, lluvia |
| **Mosaic** | Media | Retro/Pixel | Nostalgia, videojuegos |
| **Vortex** | Media | Intenso | Terror, acción, climaxes |
| **Flash** | Baja | Cinemático | Cambios de tiempo, impacto |

---

## 🔧 Implementación Rápida en C#

```csharp
public enum TransitionType
{
    // ... anteriores ...
    ShutterHorizontal,
    ShutterVertical,
    TileFlip,
    InkBleed,
    LensDistortion,
    GlitchDigital,
    RainDrop,
    Mosaic,
    Vortex,
    FlashWhiteout
}

// En GetShaderForTransition:
TransitionType.ShutterHorizontal or TransitionType.ShutterVertical => "Shutter",
TransitionType.TileFlip => "TileFlip",
TransitionType.InkBleed => "InkBleed",
TransitionType.LensDistortion => "Lens",
TransitionType.GlitchDigital => "Glitch",
TransitionType.RainDrop => "RainDrop",
TransitionType.Mosaic => "Mosaic",
TransitionType.Vortex => "Vortex",
TransitionType.FlashWhiteout => "Flash"
```

---

## 💡 Combinaciones Creativas

Puedes **encadenar transiciones** para efectos únicos:

```csharp
// Ejemplo: Flash + Vortex para cambio de dimensión
await Transition(FlashWhiteout, 0.3f);
await Transition(Vortex, 1.0f);

// Ejemplo: Glitch + estabilización
await Transition(GlitchDigital, 0.5f);
await Transition(Fade, 0.3f); // Suavizar el final
```

¿Quieres que profundice en alguno de estos efectos o que cree uno específico para un género (horror, romance, sci-fi)?
*/