
## Conceptos Básicos de Shaders en MonoGame

### ¿Qué es un Shader?
Un **shader** es un programa que se ejecuta en la GPU para modificar cómo se dibujan los píxeles. En 2D, principalmente usaremos **Pixel Shaders** (efectos por píxel).

### Dos Formas de Usar Shaders en 2D

| Método | Descripción | Cuándo usarlo |
|--------|-------------|---------------|
| **Efectos personalizados (.fx)** | Shaders escritos en HLSL | Efectos complejos, iluminación, distorsión |
| **BasicEffect y SpriteBatch** | Configuración del SpriteBatch existente | Tintado simple, matrices de transformación |

---

## Método 1: Configuración Básica de SpriteBatch (Sin Shaders Personalizados)

Antes de crear shaders propios, aprovecha al máximo el `SpriteBatch`:

```csharp
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _texture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _texture = Content.Load<Texture2D>("imagen");
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // === MODO POR DEFECTO (AlphaBlend) ===
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
        _spriteBatch.End();

        // === MULTIPLY (Multiplica colores, útil para sombras) ===
        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Multiply);
        _spriteBatch.Draw(_texture, new Vector2(100, 0), Color.Gray);
        _spriteBatch.End();

        // === ADDITIVE (Luz, fuego, partículas brillantes) ===
        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
        _texture2D glow = Content.Load<Texture2D>("glow");
        _spriteBatch.Draw(glow, new Vector2(200, 100), Color.Yellow);
        _spriteBatch.End();

        // === CON TRANSFORMACIÓN (Cámara 2D básica) ===
        Matrix camera = Matrix.CreateTranslation(-100, -50, 0) * 
                       Matrix.CreateScale(2.0f);
        
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.Default,
            RasterizerState.CullNone,
            null, // <-- Aquí iría el shader personalizado
            camera);
            
        _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
        _spriteBatch.End();
    }
}
```

---

## Método 2: Shader Personalizado (Efecto .fx)

### Paso 1: Crear el Archivo del Shader

Crea un archivo `Efecto2D.fx` en tu proyecto:

```hlsl
// Efecto2D.fx
// Shaders para efectos 2D en MonoGame

// Variables que recibimos desde C#
float4 TintColor;
float Time;
float2 Resolution;

// Textura y sampler
Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

// ESTRUCTURAS DE ENTRADA/SALIDA
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;  // SV_POSITION obligatorio en DX11
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

// VERTEX SHADER (Transformación básica)
VertexShaderOutput MainVS(VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = input.Position;
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
}

// ============================================
// PIXEL SHADERS (Efectos por píxel)
// ============================================

// 1. TINTADO SIMPLE (Multiplica por color)
float4 TintPS(VertexShaderOutput input) : SV_Target
{
    float4 texColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    return texColor * input.Color * TintColor;
}

// 2. ESCALA DE GRISES
float4 GrayscalePS(VertexShaderOutput input) : SV_Target
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    float gray = dot(color.rgb, float3(0.299, 0.587, 0.114));
    return float4(gray, gray, gray, color.a) * input.Color;
}

// 3. NEGATIVO / INVERTIR COLORES
float4 InvertPS(VertexShaderOutput input) : SV_Target
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    return float4(1.0 - color.rgb, color.a) * input.Color;
}

// 4. EFECTO DE ONDA / DISTORSIÓN
float4 WavePS(VertexShaderOutput input) : SV_Target
{
    float2 uv = input.TextureCoordinates;
    
    // Distorsión sinusoidal basada en tiempo
    uv.y += sin(uv.x * 10.0 + Time * 3.0) * 0.02;
    
    float4 color = tex2D(SpriteTextureSampler, uv);
    return color * input.Color;
}

// 5. EFECTO DE BRILLO / GLOW SIMPLE
float4 GlowPS(VertexShaderOutput input) : SV_Target
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    
    // Aumentar intensidad manteniendo transparencia
    float3 bright = color.rgb * 1.5;
    bright = saturate(bright); // clamp entre 0 y 1
    
    return float4(bright, color.a) * input.Color;
}

// 6. PIXELADO / RETRO (Muestreo cuantizado)
float4 PixelatePS(VertexShaderOutput input) : SV_Target
{
    float pixelSize = 20.0; // Tamaño de píxel
    
    float2 uv = input.TextureCoordinates;
    uv.x = floor(uv.x * Resolution.x / pixelSize) * pixelSize / Resolution.x;
    uv.y = floor(uv.y * Resolution.y / pixelSize) * pixelSize / Resolution.y;
    
    return tex2D(SpriteTextureSampler, uv) * input.Color;
}

// ============================================
// TÉCNICAS (Elige cuál usar desde C#)
// ============================================

technique Tint
{
    pass P0
    {
        VertexShader = compile vs_4_0 MainVS();
        PixelShader = compile ps_4_0 TintPS();
    }
};

technique Grayscale
{
    pass P0
    {
        VertexShader = compile vs_4_0 MainVS();
        PixelShader = compile ps_4_0 GrayscalePS();
    }
};

technique Invert
{
    pass P0
    {
        VertexShader = compile vs_4_0 MainVS();
        PixelShader = compile ps_4_0 InvertPS();
    }
};

technique Wave
{
    pass P0
    {
        VertexShader = compile vs_4_0 MainVS();
        PixelShader = compile ps_4_0 WavePS();
    }
};

technique Glow
{
    pass P0
    {
        VertexShader = compile vs_4_0 MainVS();
        PixelShader = compile ps_4_0 GlowPS();
    }
};

technique Pixelate
{
    pass P0
    {
        VertexShader = compile vs_4_0 MainVS();
        PixelShader = compile ps_4_0 PixelatePS();
    }
};
```

---

### Paso 2: Compilar y Cargar en MonoGame

#### Opción A: Pipeline Tool (Recomendado para producción)

1. Abre **MonoGame Content Builder (MGCB Editor)**
2. Añade `Efecto2D.fx` al proyecto
3. Establece **Build Action**: `Build`
4. Establece **Importer**: `EffectImporter`
5. Establece **Processor**: `EffectProcessor`
6. Compila (F6)

#### Opción B: Carga en Tiempo de Ejecución (Desarrollo rápido)

```csharp
// Compilar y cargar el .fx directamente
Effect effect;
        
protected override void LoadContent()
{
    _spriteBatch = new SpriteBatch(GraphicsDevice);
    _texture = Content.Load<Texture2D>("imagen");
    
    // Cargar el efecto compilado desde Content
    effect = Content.Load<Effect>("Efecto2D");
}
```

---

### Paso 3: Usar el Shader en el Juego

```csharp
public class ShaderDemo : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _playerTexture;
    private Texture2D _background;
    private Effect _effect;
    
    private float _time = 0f;
    private string _currentTechnique = "Tint";
    private Vector2 _resolution;

    public ShaderDemo()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _playerTexture = Content.Load<Texture2D>("player");
        _background = Content.Load<Texture2D>("bg");
        
        // Cargar shader
        _effect = Content.Load<Effect>("Efecto2D");
        
        // Guardar resolución para shaders que la necesiten
        _resolution = new Vector2(
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height);
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboard = Keyboard.GetState();
        var gamepad = GamePad.GetState(PlayerIndex.One);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
            || keyboard.IsKeyDown(Keys.Escape))
            Exit();

        // Cambiar efecto con teclas numéricas
        if (keyboard.IsKeyDown(Keys.D1)) _currentTechnique = "Tint";
        if (keyboard.IsKeyDown(Keys.D2)) _currentTechnique = "Grayscale";
        if (keyboard.IsKeyDown(Keys.D3)) _currentTechnique = "Invert";
        if (keyboard.IsKeyDown(Keys.D4)) _currentTechnique = "Wave";
        if (keyboard.IsKeyDown(Keys.D5)) _currentTechnique = "Glow";
        if (keyboard.IsKeyDown(Keys.D6)) _currentTechnique = "Pixelate";

        _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Actualizar parámetros del shader
        UpdateShaderParameters();

        base.Update(gameTime);
    }

    private void UpdateShaderParameters()
    {
        // Seleccionar técnica
        _effect.CurrentTechnique = _effect.Techniques[_currentTechnique];
        
        // Enviar variables al shader
        _effect.Parameters["Time"].SetValue(_time);
        _effect.Parameters["Resolution"].SetValue(_resolution);
        
        // Color de tintado dinámico (oscila entre rojo y azul)
        var tint = new Vector4(
            0.5f + 0.5f * (float)Math.Sin(_time),
            0.5f + 0.5f * (float)Math.Sin(_time + 2),
            0.5f + 0.5f * (float)Math.Sin(_time + 4),
            1.0f);
        _effect.Parameters["TintColor"].SetValue(tint);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // === DIBUJAR BACKGROUND SIN EFECTO ===
        _spriteBatch.Begin();
        _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
        _spriteBatch.End();

        // === DIBUJAR JUGADOR CON SHADER ===
        // Pasamos el efecto como parámetro en Begin()
        _spriteBatch.Begin(
            SpriteSortMode.Immediate,      // Aplica inmediatamente
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            _effect);                      // <-- ¡NUESTRO SHADER!
            
        // Todas las draws dentro de este Begin/End usan el shader
        _spriteBatch.Draw(
            _playerTexture, 
            new Vector2(400, 300), 
            null, 
            Color.White, 
            0f, 
            new Vector2(_playerTexture.Width / 2, _playerTexture.Height / 2), 
            2.0f, // Escala
            SpriteEffects.None, 
            0f);
            
        _spriteBatch.End();

        // === UI SIN EFECTO ===
        // Importante: nuevo Begin/End sin shader para la interfaz
        var font = Content.Load<SpriteFont>("font");
        _spriteBatch.Begin();
        _spriteBatch.DrawString(font, $"Efecto: {_currentTechnique}", new Vector2(10, 10), Color.White);
        _spriteBatch.DrawString(font, "1-6: Cambiar efecto", new Vector2(10, 40), Color.Yellow);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
```

---

## Estructura Recomendada: Sistema de Efectos

Para proyectos más grandes, organiza tus shaders así:

```csharp
// Manager centralizado de efectos
public class EffectManager
{
    private readonly Effect _effectSheet;
    private readonly Dictionary<string, EffectParameter> _parameters;
    
    public string CurrentEffect { get; private set; } = "Normal";
    
    public EffectManager(ContentManager content)
    {
        _effectSheet = content.Load<Effect>("Shaders/All2DEffects");
    }
    
    public void SetEffect(string techniqueName)
    {
        if (_effectSheet.Techniques[techniqueName] != null)
        {
            _effectSheet.CurrentTechnique = _effectSheet.Techniques[techniqueName];
            CurrentEffect = techniqueName;
        }
    }
    
    public void SetParameter(string name, float value) => 
        _effectSheet.Parameters[name]?.SetValue(value);
    
    public void SetParameter(string name, Vector2 value) => 
        _effectSheet.Parameters[name]?.SetValue(value);
    
    public void SetParameter(string name, Vector4 value) => 
        _effectSheet.Parameters[name]?.SetValue(value);
    
    public void Apply(SpriteBatch spriteBatch)
    {
        // Helper para Begin con el efecto actual
        spriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            _effectSheet);
    }
    
    public void Reset(SpriteBatch spriteBatch)
    {
        spriteBatch.End();
        spriteBatch.Begin();
    }
}

// Uso:
// _effects.SetEffect("Wave");
// _effects.SetParameter("Time", gameTime.TotalGameTime.Seconds);
// _effects.Apply(_spriteBatch);
// _spriteBatch.Draw(...);
// _effects.Reset(_spriteBatch);
```

---

## Shader Especial: Iluminación 2D Básica

```hlsl
// Añadir a tu archivo .fx existente

// Para iluminación 2D sencilla
float3 LightPosition; // Posición en coordenadas de pantalla
float LightRadius;
float4 LightColor;

float4 Light2DPS(VertexShaderOutput input) : SV_Target
{
    float4 texColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    
    // Calcular distancia desde el píxel hasta la luz
    // Nota: input.Position está en coordenadas de pantalla
    float2 pixelPos = input.Position.xy;
    float dist = distance(pixelPos, LightPosition.xy);
    
    // Factor de atenuación
    float attenuation = 1.0 - saturate(dist / LightRadius);
    attenuation = attenuation * attenuation; // Caída cuadrática
    
    // Aplicar luz sobre la textura
    float3 litColor = texColor.rgb + (LightColor.rgb * attenuation);
    
    return float4(litColor, texColor.a);
}

technique Light2D
{
    pass P0
    {
        VertexShader = compile vs_4_0 MainVS();
        PixelShader = compile ps_4_0 Light2DPS();
    }
};
```

---

## Referencia Rápida de Blend States

```csharp
// Útiles para efectos de partículas y luces
_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);      // Luz, fuego
_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Multiply);      // Sombras, tintado oscuro
_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);        // Sin transparencia
_spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied); // Texturas con alpha no-premultiplicado
```

---

## Consejos Prácticos

| Problema | Solución |
|----------|----------|
| Shader no compila | Verifica versión (`vs_4_0`/`ps_4_0` para DirectX 11) |
| Pantalla negra | Revisa que la textura se pase correctamente (nombre del sampler) |
| Efecto no se ve | Usa `SpriteSortMode.Immediate` en Begin() |
| Parámetros no actualizan | Llama `SetValue()` antes de `Begin()` |
| Rendimiento bajo | Evita cambiar de técnica cada frame; agrupa draws por shader |

¿Quieres que profundice en algún efecto específico, como transiciones entre escenas, outline de sprites, o sombras dinámicas 2D?
