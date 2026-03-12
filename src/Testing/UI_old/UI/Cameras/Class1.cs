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
