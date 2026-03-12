User: Eres un programador especializado en monogame. Estás programando un sistema de manejo de sprites, vamos a separar los conceptos en dos partes: la definición y la representación. Un Sprite estaría formado por otros sprites con coordenadas locales y relativas
Kimi: Entiendo perfectamente lo que buscas. Este es un patrón clásico de **Composite Pattern** aplicado a sistemas de sprites, donde un sprite puede contener otros sprites con transformaciones locales. Te propongo esta arquitectura:

## Arquitectura del Sistema

### 1. Definición (Data/Configuración)

```csharp
// Transformación local relativa al padre
public struct LocalTransform
{
    public Vector2 Position;      // Offset respecto al padre
    public float Rotation;        // Rotación relativa
    public Vector2 Scale;         // Escala local
    public float LayerDepth;      // Profundidad relativa
    
    public static readonly LocalTransform Identity = new() 
    { 
        Position = Vector2.Zero, 
        Rotation = 0f, 
        Scale = Vector2.One,
        LayerDepth = 0f
    };
}

// Definición pura de datos, serializable
public class SpriteDefinition
{
    public string Name { get; set; }
    public string TexturePath { get; set; }
    public Rectangle? SourceRectangle { get; set; }
    public Vector2 Origin { get; set; } = new Vector2(0.5f, 0.5f); // Centro por defecto
    public Color Tint { get; set; } = Color.White;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    
    // Jerarquía: definiciones hijas con sus transformaciones locales
    public List<ChildSpriteDefinition> Children { get; set; } = new();
}

public class ChildSpriteDefinition
{
    public string ChildName { get; set; }  // Referencia por nombre para lazy loading
    public LocalTransform LocalTransform { get; set; }
    // O directamente la definición embebida si es específica
    public SpriteDefinition EmbeddedDefinition { get; set; }
}
```

### 2. Representación (Runtime/Instancia)

```csharp
// Nodo en la jerarquía de renderizado
public class SpriteNode
{
    // Referencia a definición (compartida entre instancias)
    public SpriteDefinition Definition { get; private set; }
    
    // Estado mutable de esta instancia
    public LocalTransform LocalTransform { get; set; }
    public Color CurrentTint { get; set; }
    public bool IsVisible { get; set; } = true;
    public bool IsActive { get; set; } = true;
    
    // Jerarquía
    public SpriteNode Parent { get; private set; }
    public List<SpriteNode> Children { get; private set; } = new();
    
    // Caché de transformación mundial (se recalcula solo cuando cambia)
    private Matrix? _cachedWorldTransform;
    private bool _transformDirty = true;
    
    public SpriteNode(SpriteDefinition definition, LocalTransform? localTransform = null)
    {
        Definition = definition;
        LocalTransform = localTransform ?? LocalTransform.Identity;
        CurrentTint = definition.Tint;
    }
    
    // Transformación mundial calculada
    public Matrix WorldTransform
    {
        get
        {
            if (_transformDirty)
            {
                var local = Matrix.CreateScale(LocalTransform.Scale.X, LocalTransform.Scale.Y, 1f) *
                           Matrix.CreateRotationZ(LocalTransform.Rotation) *
                           Matrix.CreateTranslation(LocalTransform.Position.X, LocalTransform.Position.Y, 0f);
                
                _cachedWorldTransform = Parent != null 
                    ? local * Parent.WorldTransform 
                    : local;
                    
                _transformDirty = false;
            }
            return _cachedWorldTransform.Value;
        }
    }
    
    // Extraer posición/rotación/escala del mundo para debug
    public Vector2 WorldPosition => Vector2.Transform(Vector2.Zero, WorldTransform);
    public float WorldRotation => Parent?.WorldRotation + LocalTransform.Rotation ?? LocalTransform.Rotation;
    public Vector2 WorldScale => Parent?.WorldScale * LocalTransform.Scale ?? LocalTransform.Scale;
    public float WorldLayerDepth => (Parent?.WorldLayerDepth ?? 0f) + LocalTransform.LayerDepth;
    
    public void AddChild(SpriteNode child)
    {
        child.Parent?.RemoveChild(child);
        child.Parent = this;
        Children.Add(child);
        child.InvalidateTransform();
    }
    
    public void RemoveChild(SpriteNode child)
    {
        if (Children.Remove(child))
        {
            child.Parent = null;
            child.InvalidateTransform();
        }
    }
    
    public void InvalidateTransform()
    {
        _transformDirty = true;
        foreach (var child in Children)
            child.InvalidateTransform();
    }
    
    // Notificar cambio de transformación local
    public void SetLocalPosition(Vector2 pos) 
    { 
        LocalTransform.Position = pos; 
        InvalidateTransform(); 
    }
    
    public void SetLocalRotation(float rot) 
    { 
        LocalTransform.Rotation = rot; 
        InvalidateTransform(); 
    }
}
```

### 3. Sistema de Renderizado

```csharp
public class SpriteRenderer
{
    private SpriteBatch _spriteBatch;
    private Dictionary<string, Texture2D> _textureCache;
    private List<SpriteNode> _renderQueue = new();
    
    public SpriteRenderer(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        _textureCache = new Dictionary<string, Texture2D>();
    }
    
    public void LoadContent(ContentManager content, IEnumerable<SpriteDefinition> definitions)
    {
        foreach (var def in definitions)
            LoadTexture(content, def.TexturePath);
    }
    
    private void LoadTexture(ContentManager content, string path)
    {
        if (!_textureCache.ContainsKey(path))
            _textureCache[path] = content.Load<Texture2D>(path);
    }
    
    // Recolecta todos los nodos visibles en orden de profundidad
    public void CullAndQueue(SpriteNode root)
    {
        CollectVisibleNodes(root);
        _renderQueue.Sort((a, b) => a.WorldLayerDepth.CompareTo(b.WorldLayerDepth));
    }
    
    private void CollectVisibleNodes(SpriteNode node)
    {
        if (!node.IsVisible) return;
        
        _renderQueue.Add(node);
        
        foreach (var child in node.Children)
            CollectVisibleNodes(child);
    }
    
    public void Draw(Matrix? viewMatrix = null)
    {
        var transformMatrix = viewMatrix ?? Matrix.Identity;
        
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            transformMatrix);
            
        foreach (var node in _renderQueue)
        {
            DrawNode(node);
        }
        
        _spriteBatch.End();
        _renderQueue.Clear();
    }
    
    private void DrawNode(SpriteNode node)
    {
        if (!_textureCache.TryGetValue(node.Definition.TexturePath, out var texture))
            return;
            
        var world = node.WorldTransform;
        
        // Extraer componentes de la matriz para SpriteBatch
        DecomposeMatrix(world, out var position, out var rotation, out var scale);
        
        var sourceRect = node.Definition.SourceRectangle ?? texture.Bounds;
        
        _spriteBatch.Draw(
            texture,
            position,
            sourceRect,
            node.CurrentTint,
            rotation,
            node.Definition.Origin * new Vector2(sourceRect.Width, sourceRect.Height),
            scale,
            node.Definition.Effects,
            node.WorldLayerDepth);
    }
    
    private void DecomposeMatrix(Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
    {
        position = new Vector2(matrix.M41, matrix.M42);
        
        scale = new Vector2(
            MathF.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12),
            MathF.Sqrt(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22));
            
        rotation = MathF.Atan2(matrix.M12, matrix.M11);
    }
}
```

### 4. Factory para construir desde definiciones

```csharp
public class SpriteFactory
{
    private Dictionary<string, SpriteDefinition> _definitions;
    
    public SpriteFactory(IEnumerable<SpriteDefinition> definitions)
    {
        _definitions = definitions.ToDictionary(d => d.Name);
    }
    
    // Crea una instancia completa de la jerarquía
    public SpriteNode Create(string definitionName, LocalTransform? rootTransform = null)
    {
        if (!_definitions.TryGetValue(definitionName, out var def))
            throw new ArgumentException($"Definición '{definitionName}' no encontrada");
            
        return CreateNode(def, rootTransform ?? LocalTransform.Identity);
    }
    
    private SpriteNode CreateNode(SpriteDefinition def, LocalTransform localTransform)
    {
        var node = new SpriteNode(def, localTransform);
        
        foreach (var childDef in def.Children)
        {
            SpriteNode child;
            
            if (childDef.EmbeddedDefinition != null)
            {
                child = CreateNode(childDef.EmbeddedDefinition, childDef.LocalTransform);
            }
            else if (_definitions.TryGetValue(childDef.ChildName, out var referencedDef))
            {
                child = CreateNode(referencedDef, childDef.LocalTransform);
            }
            else
            {
                continue; // O lanzar excepción
            }
            
            node.AddChild(child);
        }
        
        return node;
    }
}
```

### 5. Ejemplo de uso

```csharp
// Definición de un personaje compuesto
var heroDef = new SpriteDefinition
{
    Name = "Hero",
    TexturePath = "sprites/body",
    Origin = new Vector2(0.5f, 1f), // Anclado abajo
    Children = new List<ChildSpriteDefinition>
    {
        new() {
            ChildName = "Head",  // Referencia a otra definición
            LocalTransform = new LocalTransform 
            { 
                Position = new Vector2(0, -32), 
                LayerDepth = 0.1f 
            }
        },
        new() {
            ChildName = "Weapon",
            LocalTransform = new LocalTransform 
            { 
                Position = new Vector2(12, -20),
                Rotation = MathHelper.ToRadians(45),
                LayerDepth = -0.1f  // Detrás del cuerpo
            }
        }
    }
};

// En el juego
public class Game1 : Game
{
    private SpriteRenderer _renderer;
    private SpriteFactory _factory;
    private SpriteNode _heroInstance;
    
    protected override void LoadContent()
    {
        var definitions = LoadDefinitionsFromJson("Content/sprites/definitions.json");
        _factory = new SpriteFactory(definitions);
        _renderer = new SpriteRenderer(new SpriteBatch(GraphicsDevice));
        _renderer.LoadContent(Content, definitions);
        
        // Crear instancia
        _heroInstance = _factory.Create("Hero", new LocalTransform 
        { 
            Position = new Vector2(400, 300) 
        });
    }
    
    protected override void Update(GameTime gameTime)
    {
        // Animar brazo relativo al cuerpo
        var weapon = _heroInstance.Children.First(c => c.Definition.Name == "Weapon");
        weapon.SetLocalRotation(
            MathF.Sin(gameTime.TotalGameTime.TotalSeconds * 2f) * 0.5f);
            
        // Mover todo el personaje
        _heroInstance.SetLocalPosition(
            new Vector2(400 + MathF.Cos((float)gameTime.TotalGameTime.TotalSeconds) * 100, 300));
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _renderer.CullAndQueue(_heroInstance);
        _renderer.Draw();
    }
}
```

## Características clave

| Aspecto | Implementación |
|---------|---------------|
| **Separación** | Definición (datos estáticos) vs Representación (estado mutable) |
| **Jerarquía** | Composite pattern con transformaciones locales |
| **Performance** | Caché de matrices mundiales, invalidación dirty-flag |
| **Flexibilidad** | Referencias por nombre o definiciones embebidas |
| **Batching** | Un solo SpriteBatch.Draw por frame ordenado por profundidad |

¿Necesitas que profundice en algún aspecto específico? Por ejemplo:
- Serialización JSON/XML de las definiciones
- Sistema de animación de huesos/skeleton
- Culling por viewport
- Pooling de nodos para evitar GC en móviles
User: Todas las coordenadas deben ser relativas de 0 a 1 en la definición en el dibujo deben convertirse a coordenadas de mundo
Kimi: Entendido. Sistema con **coordenadas normalizadas (0-1)** en definición que se convierten a **coordenadas de mundo** en runtime. Esto permite escalado automático y resolución-independencia.

## Arquitectura Normalizada

### 1. Definición (Coordenadas 0-1)

```csharp
// Unidades normalizadas (0.0 = inicio, 1.0 = tamaño del padre)
public struct NormalizedTransform
{
    public Vector2 Position;      // 0-1 relativo al padre (0,0=arriba-izq, 1,1=abajo-der)
    public float Rotation;        // Radianes, relativo
    public Vector2 Scale;         // Multiplicador (1.0 = tamaño original textura)
    public float LayerDepth;      // 0-1 para orden Z
    
    public static readonly NormalizedTransform Identity = new()
    {
        Position = Vector2.Zero,
        Rotation = 0f,
        Scale = Vector2.One,
        LayerDepth = 0.5f
    };
}

public class SpriteDefinition
{
    public string Name { get; set; }
    public string TexturePath { get; set; }
    
    // Rectángulo fuente en UVs (0-1) en lugar de píxeles
    public RectangleF? SourceUV { get; set; }  // X,Y,Width,Height en 0-1
    
    // Punto de anclaje (0-1)
    public Vector2 Anchor { get; set; } = new Vector2(0.5f, 0.5f);
    
    public Color Tint { get; set; } = Color.White;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    
    // Tamaño base en unidades de mundo (opcional, si null usa tamaño textura)
    public Vector2? WorldSize { get; set; }
    
    public List<ChildDefinition> Children { get; set; } = new();
}

public class ChildDefinition
{
    public string ChildName { get; set; }
    public NormalizedTransform Transform { get; set; }
    public SpriteDefinition Embedded { get; set; }
}
```

### 2. Representación (Coordenadas de Mundo)

```csharp
public class SpriteNode
{
    public SpriteDefinition Definition { get; }
    public SpriteNode Parent { get; private set; }
    public List<SpriteNode> Children { get; } = new();
    
    // Transformación normalizada local (editable en runtime)
    public NormalizedTransform LocalNorm { get; set; }
    
    // Estado mutable
    public Color Tint { get; set; }
    public bool Visible { get; set; } = true;
    public bool Active { get; set; } = true;
    
    // Caché de cálculos
    private WorldTransform? _cachedWorld;
    private bool _dirty = true;
    private Vector2 _parentWorldSize;  // Tamaño acumulado del padre
    
    public SpriteNode(SpriteDefinition def, NormalizedTransform? local = null)
    {
        Definition = def;
        LocalNorm = local ?? NormalizedTransform.Identity;
        Tint = def.Tint;
    }
    
    // === CONVERSIÓN NORMALIZADO → MUNDO ===
    
    public WorldTransform World
    {
        get
        {
            if (_dirty)
            {
                _cachedWorld = CalculateWorld();
                _dirty = false;
            }
            return _cachedWorld.Value;
        }
    }
    
    private WorldTransform CalculateWorld()
    {
        // Tamaño de este sprite en unidades de mundo
        var mySize = GetWorldSize();
        
        // Posición: Position(0-1) * ParentSize + ParentPosition
        Vector2 worldPos;
        float worldRot;
        Vector2 worldScale;
        float worldLayer;
        
        if (Parent == null)
        {
            // Nodo raíz: Position(0-1) se interpreta como unidades directas
            // o multiplicado por un "tamaño de escena" configurable
            worldPos = LocalNorm.Position * 1000f; // 1000 = tamaño de escena base
            worldRot = LocalNorm.Rotation;
            worldScale = LocalNorm.Scale;
            worldLayer = LocalNorm.LayerDepth;
        }
        else
        {
            var parentWorld = Parent.World;
            var parentSize = Parent.GetWorldSize();
            
            // Posición relativa al padre: UV * tamaño_padre
            worldPos = parentWorld.Position + 
                      (LocalNorm.Position * parentSize);
            
            worldRot = parentWorld.Rotation + LocalNorm.Rotation;
            worldScale = parentWorld.Scale * LocalNorm.Scale;
            worldLayer = parentWorld.LayerDepth + LocalNorm.LayerDepth;
        }
        
        return new WorldTransform
        {
            Position = worldPos,
            Rotation = worldRot,
            Scale = worldScale,
            LayerDepth = MathHelper.Clamp(worldLayer, 0f, 1f),
            Size = mySize  // Tamaño en unidades de mundo
        };
    }
    
    // Calcula tamaño en unidades de mundo
    public Vector2 GetWorldSize()
    {
        if (Definition.WorldSize.HasValue)
            return Definition.WorldSize.Value * LocalNorm.Scale;
            
        // Si no hay tamaño definido, usar tamaño de textura escalado
        // (requiere que el renderer pase el tamaño de textura)
        return _intrinsicSize * LocalNorm.Scale;
    }
    
    private Vector2 _intrinsicSize = Vector2.One;
    internal void SetIntrinsicSize(Vector2 texSize) => _intrinsicSize = texSize;
    
    public void AddChild(SpriteNode child)
    {
        child.Parent?.RemoveChild(child);
        child.Parent = this;
        Children.Add(child);
        child.Invalidate();
    }
    
    public void RemoveChild(SpriteNode child)
    {
        if (Children.Remove(child))
        {
            child.Parent = null;
            child.Invalidate();
        }
    }
    
    public void Invalidate()
    {
        _dirty = true;
        foreach (var c in Children) c.Invalidate();
    }
    
    // Helpers para modificar transformación normalizada
    public void SetNormPos(Vector2 pos) { LocalNorm.Position = pos; Invalidate(); }
    public void SetNormRot(float rot) { LocalNorm.Rotation = rot; Invalidate(); }
    public void SetNormScale(Vector2 scl) { LocalNorm.Scale = scl; Invalidate(); }
}

public struct WorldTransform
{
    public Vector2 Position;      // Unidades de mundo (píxeles/unidades lógicas)
    public float Rotation;        // Radianes
    public Vector2 Scale;         // Multiplicador acumulado
    public float LayerDepth;      // 0-1 para Z-order
    public Vector2 Size;          // Tamaño en unidades de mundo
}
```

### 3. Renderer con Conversión

```csharp
public class NormalizedSpriteRenderer
{
    private SpriteBatch _sb;
    private ContentManager _content;
    private Dictionary<string, Texture2D> _textures = new();
    private Dictionary<string, Vector2> _textureSizes = new();
    
    // Configuración de escala mundo
    public float WorldUnitsPerPixel { get; set; } = 1f;
    public Vector2 ReferenceResolution { get; set; } = new Vector2(1920, 1080);
    
    public NormalizedSpriteRenderer(SpriteBatch sb, ContentManager content)
    {
        _sb = sb;
        _content = content;
    }
    
    public void Preload(SpriteDefinition[] defs)
    {
        foreach (var d in defs)
            LoadTexture(d);
    }
    
    private void LoadTexture(SpriteDefinition def)
    {
        if (_textures.ContainsKey(def.TexturePath)) return;
        
        var tex = _content.Load<Texture2D>(def.TexturePath);
        _textures[def.TexturePath] = tex;
        _textureSizes[def.TexturePath] = new Vector2(tex.Width, tex.Height);
    }
    
    // === CONVERSIÓN CLAVE: UV → Pixeles → Mundo ===
    
    public void Draw(SpriteNode root, Matrix? view = null)
    {
        // Primera pasada: asignar tamaños intrínsecos
        AssignIntrinsicSizes(root);
        
        // Segunda pasada: recolectar nodos visibles
        var visible = new List<SpriteNode>();
        Collect(root, visible);
        visible.Sort((a, b) => a.World.LayerDepth.CompareTo(b.World.LayerDepth));
        
        // Dibujar
        _sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
                 SamplerState.LinearClamp, null, null, null, view);
        
        foreach (var node in visible)
            DrawNode(node);
            
        _sb.End();
    }
    
    private void AssignIntrinsicSizes(SpriteNode node)
    {
        if (_textureSizes.TryGetValue(node.Definition.TexturePath, out var texSize))
        {
            // Si hay SourceUV, calcular tamaño sub-textura
            if (node.Definition.SourceUV.HasValue)
            {
                var uv = node.Definition.SourceUV.Value;
                texSize = new Vector2(
                    texSize.X * uv.Width,
                    texSize.Y * uv.Height);
            }
            
            // Convertir píxeles a unidades de mundo
            var worldSize = texSize * WorldUnitsPerPixel;
            node.SetIntrinsicSize(worldSize);
        }
        
        foreach (var child in node.Children)
            AssignIntrinsicSizes(child);
    }
    
    private void Collect(SpriteNode node, List<SpriteNode> list)
    {
        if (!node.Visible) return;
        list.Add(node);
        foreach (var c in node.Children) Collect(c, list);
    }
    
    private void DrawNode(SpriteNode node)
    {
        if (!_textures.TryGetValue(node.Definition.TexturePath, out var tex))
            return;
            
        var world = node.World;
        
        // Source rectangle: UV → Píxeles
        Rectangle? sourceRect = null;
        if (node.Definition.SourceUV.HasValue)
        {
            var uv = node.Definition.SourceUV.Value;
            sourceRect = new Rectangle(
                (int)(uv.X * tex.Width),
                (int)(uv.Y * tex.Height),
                (int)(uv.Width * tex.Width),
                (int)(uv.Height * tex.Height));
        }
        
        // Origen: Anchor(0-1) * tamaño_fuente
        Vector2 origin;
        if (sourceRect.HasValue)
            origin = node.Definition.Anchor * new Vector2(sourceRect.Value.Width, sourceRect.Value.Height);
        else
            origin = node.Definition.Anchor * new Vector2(tex.Width, tex.Height);
        
        // Escala final: escala_acumulada * (tamaño_mundial / tamaño_textura)
        // Para que 1 unidad de mundo = 1 píxel a escala 1
        var drawScale = world.Scale * WorldUnitsPerPixel;
        
        _sb.Draw(
            tex,
            world.Position,
            sourceRect,
            node.Tint,
            world.Rotation,
            origin,
            drawScale,
            node.Definition.Effects,
            world.LayerDepth);
    }
}
```

### 4. Factory y Utilidades

```csharp
public class SpriteFactory
{
    private Dictionary<string, SpriteDefinition> _defs;
    
    public SpriteFactory(IEnumerable<SpriteDefinition> defs)
    {
        _defs = defs.ToDictionary(d => d.Name);
    }
    
    public SpriteNode Create(string name, NormalizedTransform? rootTransform = null)
    {
        if (!_defs.TryGetValue(name, out var def))
            throw new ArgumentException($"Sprite '{name}' no existe");
            
        return BuildNode(def, rootTransform ?? NormalizedTransform.Identity);
    }
    
    private SpriteNode BuildNode(SpriteDefinition def, NormalizedTransform local)
    {
        var node = new SpriteNode(def, local);
        
        foreach (var child in def.Children)
        {
            SpriteNode childNode;
            if (child.Embedded != null)
                childNode = BuildNode(child.Embedded, child.Transform);
            else if (_defs.TryGetValue(child.ChildName, out var childDef))
                childNode = BuildNode(childDef, child.Transform);
            else
                continue;
                
            node.AddChild(childNode);
        }
        
        return node;
    }
}

// Helper para crear definiciones desde código
public static class SpriteBuilder
{
    public static SpriteDefinition Create(string name, string texture, Vector2? worldSize = null)
    {
        return new SpriteDefinition
        {
            Name = name,
            TexturePath = texture,
            WorldSize = worldSize
        };
    }
    
    public static ChildDefinition Attach(this SpriteDefinition parent, 
        string childName, Vector2 normPos, Vector2? normScale = null, float rot = 0)
    {
        var child = new ChildDefinition
        {
            ChildName = childName,
            Transform = new NormalizedTransform
            {
                Position = normPos,
                Scale = normScale ?? Vector2.One,
                Rotation = rot
            }
        };
        parent.Children.Add(child);
        return child;
    }
    
    public static ChildDefinition Attach(this SpriteDefinition parent,
        SpriteDefinition embedded, Vector2 normPos)
    {
        var child = new ChildDefinition
        {
            Embedded = embedded,
            Transform = new NormalizedTransform { Position = normPos }
        };
        parent.Children.Add(child);
        return child;
    }
}
```

### 5. Ejemplo de Uso Completo

```csharp
public class Game1 : Game
{
    private NormalizedSpriteRenderer _renderer;
    private SpriteFactory _factory;
    private SpriteNode _uiRoot;
    private SpriteNode _player;
    
    protected override void LoadContent()
    {
        // === DEFINICIONES CON COORDENADAS 0-1 ===
        
        // Barra de vida: ocupa todo el ancho superior
        var healthBarDef = SpriteBuilder.Create("HealthBar", "ui/bar", new Vector2(400, 40))
            .Attach("HealthIcon", new Vector2(0.05f, 0.5f), new Vector2(0.8f))  // 5% desde izq, centrado vertical
            .Attach("HealthText", new Vector2(0.5f, 0.5f));                    // Centro
        
        // Personaje: tamaño automático de textura
        var playerDef = SpriteBuilder.Create("Player", "chars/hero")
            .Attach("Head", new Vector2(0.5f, 0.1f))      // Centro-arriba del cuerpo
            .Attach("Weapon", new Vector2(0.8f, 0.6f), rot: MathF.PI / 4)  // 80% derecha, 60% abajo
            .Attach("Shield", new Vector2(0.2f, 0.5f), new Vector2(1.2f)); // 20% izq, escala 1.2x
        
        // Inventario: grid 3x3 normalizado
        var invDef = SpriteBuilder.Create("Inventory", "ui/panel", new Vector2(300, 300));
        for (int y = 0; y < 3; y++)
            for (int x = 0; x < 3; x++)
                invDef.Attach("Slot", new Vector2((x + 0.5f) / 3f, (y + 0.5f) / 3f));
        
        // === INICIALIZACIÓN ===
        
        var defs = new[] { healthBarDef, playerDef, invDef, /* ... */ };
        _factory = new SpriteFactory(defs);
        _renderer = new NormalizedSpriteRenderer(
            new SpriteBatch(GraphicsDevice), Content)
        {
            WorldUnitsPerPixel = 1f,  // 1 píxel = 1 unidad mundo
            ReferenceResolution = new Vector2(1920, 1080)
        };
        _renderer.Preload(defs);
        
        // Crear instancias
        _uiRoot = _factory.Create("HealthBar", new NormalizedTransform
        {
            Position = new Vector2(0.5f, 0.05f),  // Centro-arriba de pantalla
            LayerDepth = 0.9f
        });
        
        _player = _factory.Create("Player", new NormalizedTransform
        {
            Position = new Vector2(0.5f, 0.5f),   // Centro de pantalla
            Scale = new Vector2(2f, 2f),          // 2x tamaño
            LayerDepth = 0.5f
        });
    }
    
    protected override void Update(GameTime gt)
    {
        // Animación: mover en coordenadas normalizadas
        var t = (float)gt.TotalGameTime.TotalSeconds;
        
        // Cabeza sigue al mouse (convertido a 0-1)
        var mouseNorm = new Vector2(
            Mouse.GetState().X / 1920f,
            Mouse.GetState().Y / 1080f);
            
        var head = _player.Children.First(c => c.Definition.Name == "Head");
        head.SetNormPos(Vector2.Lerp(head.LocalNorm.Position, mouseNorm, 0.1f));
        
        // Arma rota
        var weapon = _player.Children.First(c => c.Definition.Name == "Weapon");
        weapon.SetNormRot(MathF.Sin(t * 3f) * 0.5f);
        
        // Mover player en mundo
        _player.SetNormPos(new Vector2(
            0.5f + MathF.Cos(t) * 0.3f,
            0.5f + MathF.Sin(t * 2f) * 0.2f));
    }
    
    protected override void Draw(GameTime gt)
    {
        GraphicsDevice.Clear(Color.DarkSlateGray);
        
        _renderer.Draw(_uiRoot);
        _renderer.Draw(_player);
    }
}
```

## Diagrama de Conversión

```
DEFINICIÓN (0-1)          RUNTIME (Mundo)
═══════════════════════════════════════════════════════
Position: (0.5, 0.25)  →  Pos = ParentPos + (0.5,0.25)*ParentSize
Scale: (1, 1)          →  Scale = ParentScale * (1,1)
Rotation: 0.5 rad      →  Rot = ParentRot + 0.5

SourceUV: (0,0,0.5,1)  →  Rect(0,0, texW*0.5, texH*1)
Anchor: (0.5, 0.5)     →  Origin = (0.5,0.5) * sourceSize
```

¿Necesitas ajustar el sistema de coordenadas? Por ejemplo:
- **Y-down** vs **Y-up** (MonoGame usa Y-down)
- **Pivot** dinámico (esquinas vs centro)
- **9-slice** escalado para UI
User: Me gustaría saber cómo se puede relacionar este sistema con un sistema ik
Kimi: Excelente pregunta. Un sistema **IK (Inverse Kinematics)** encaja perfectamente con tu jerarquía de sprites normalizados. La clave es que el IK trabaja en **coordenadas de mundo** para resolver posiciones objetivo, luego convierte esos resultados a **coordenadas normalizadas locales** para actualizar la jerarquía.

## Arquitectura IK + Sprites

### 1. Estructura de Huesos (Bones)

```csharp
// Representa un hueso físico en el sistema IK
public class Bone
{
    public string Name { get; set; }
    public SpriteNode SpriteNode { get; set; }  // Referencia al sprite visual
    
    // Longitud en unidades de mundo (calculada o definida)
    public float Length { get; set; }
    
    // Restricciones articulares (opcional)
    public float MinAngle { get; set; } = -MathF.PI;
    public float MaxAngle { get; set; } = MathF.PI;
    
    // Estado actual
    public Vector2 WorldStart { get; set; }  // Posición inicio en mundo
    public Vector2 WorldEnd { get; set; }    // Posición final en mundo
    public float WorldRotation { get; set; } // Rotación absoluta
    
    // Referencias jerárquicas
    public Bone Parent { get; set; }
    public List<Bone> Children { get; } = new();
    
    // Conversión a normalizado
    public NormalizedTransform ToNormalizedTransform()
    {
        if (Parent == null)
        {
            // Raíz: posición directa en 0-1 (necesita referencia de escena)
            return new NormalizedTransform
            {
                Position = WorldStart / 1000f, // Asumiendo escena 1000x1000
                Rotation = WorldRotation,
                Scale = Vector2.One
            };
        }
        
        // Hijo: relativo al padre
        var parentEnd = Parent.WorldEnd;
        var parentRot = Parent.WorldRotation;
        
        // Offset local respecto al padre
        var localOffset = WorldStart - parentEnd;
        
        // Rotar al espacio local del padre
        var localRotated = RotateBy(-localOffset, parentRot);
        
        // Normalizar por la longitud del padre (si es 0, usar distancia)
        var parentLength = Parent.Length > 0 ? Parent.Length : 1f;
        
        return new NormalizedTransform
        {
            Position = new Vector2(
                localRotated.X / parentLength,  // X: a lo largo del hueso padre
                localRotated.Y / parentLength), // Y: perpendicular
            Rotation = WorldRotation - parentRot,
            Scale = Vector2.One
        };
    }
    
    private Vector2 RotateBy(Vector2 v, float angle)
    {
        var cos = MathF.Cos(angle);
        var sin = MathF.Sin(angle);
        return new Vector2(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
    }
}
```

### 2. Cadena IK (CCD - Cyclic Coordinate Descent)

```csharp
public class IKChain
{
    public string Name { get; set; }
    public List<Bone> Bones { get; } = new();
    public Bone Root => Bones.FirstOrDefault();
    public Bone EndEffector => Bones.LastOrDefault();
    
    // Configuración
    public int MaxIterations { get; set; } = 10;
    public float Threshold { get; set; } = 1f; // Distancia mínima al objetivo
    public bool SolveRotation { get; set; } = true; // Orientar último hueso
    
    // Resuelve IK: mueve EndEffector hacia targetWorld
    public bool Solve(Vector2 targetWorld, float? targetRotation = null)
    {
        if (Bones.Count < 2) return false;
        
        for (int iter = 0; iter < MaxIterations; iter++)
        {
            // CCD: desde el efector final hacia la raíz
            for (int i = Bones.Count - 2; i >= 0; i--)
            {
                var bone = Bones[i];
                var effector = EndEffector;
                
                // Vectores desde la articulación actual
                var toEffector = effector.WorldEnd - bone.WorldStart;
                var toTarget = targetWorld - bone.WorldStart;
                
                if (toEffector.LengthSquared() < 0.0001f) continue;
                
                // Calcular rotación necesaria
                var currentAngle = MathF.Atan2(toEffector.Y, toEffector.X);
                var targetAngle = MathF.Atan2(toTarget.Y, toTarget.X);
                var deltaAngle = NormalizeAngle(targetAngle - currentAngle);
                
                // Aplicar restricciones
                var newAngle = NormalizeAngle(bone.WorldRotation + deltaAngle);
                newAngle = Math.Clamp(newAngle, bone.MinAngle, bone.MaxAngle);
                
                // Actualizar rotación del hueso
                var actualDelta = newAngle - bone.WorldRotation;
                RotateBoneFrom(bone, actualDelta);
                
                // Verificar convergencia
                if ((EndEffector.WorldEnd - targetWorld).LengthSquared() < Threshold * Threshold)
                {
                    if (targetRotation.HasValue && SolveRotation)
                        OrientEndEffector(targetRotation.Value);
                    return true;
                }
            }
        }
        
        // No convergió pero aplica rotación final si aplica
        if (targetRotation.HasValue && SolveRotation)
            OrientEndEffector(targetRotation.Value);
            
        return false;
    }
    
    private void RotateBoneFrom(Bone fromBone, float deltaAngle)
    {
        int startIdx = Bones.IndexOf(fromBone);
        
        for (int i = startIdx; i < Bones.Count; i++)
        {
            var bone = Bones[i];
            bone.WorldRotation += deltaAngle;
            
            // Recalcular posiciones desde el inicio de la cadena
            if (i == 0)
            {
                // Raíz mantiene posición, solo rota
                bone.WorldEnd = bone.WorldStart + 
                    new Vector2(MathF.Cos(bone.WorldRotation), MathF.Sin(bone.WorldRotation)) * bone.Length;
            }
            else
            {
                // Hijos se conectan al final del padre
                var parent = Bones[i - 1];
                bone.WorldStart = parent.WorldEnd;
                bone.WorldEnd = bone.WorldStart + 
                    new Vector2(MathF.Cos(bone.WorldRotation), MathF.Sin(bone.WorldRotation)) * bone.Length;
            }
        }
    }
    
    private void OrientEndEffector(float targetRotation)
    {
        // Ajustar rotación del último hueso para alinear con target
        var last = EndEffector;
        var parent = last.Parent;
        
        if (parent != null)
        {
            // Rotación local necesaria
            var localTarget = targetRotation - parent.WorldRotation;
            var currentLocal = last.WorldRotation - parent.WorldRotation;
            var delta = NormalizeAngle(localTarget - currentLocal);
            
            last.WorldRotation = NormalizeAngle(last.WorldRotation + delta);
            last.WorldEnd = last.WorldStart + 
                new Vector2(MathF.Cos(last.WorldRotation), MathF.Sin(last.WorldRotation)) * last.Length;
        }
    }
    
    private float NormalizeAngle(float angle)
    {
        while (angle > MathF.PI) angle -= MathF.Tau;
        while (angle < -MathF.PI) angle += MathF.Tau;
        return angle;
    }
    
    // Sincroniza huesos con la jerarquía de sprites
    public void SyncToSprites()
    {
        foreach (var bone in Bones)
        {
            if (bone.SpriteNode != null)
            {
                // Convertir transformación mundo → normalizado → aplicar
                var normTransform = bone.ToNormalizedTransform();
                bone.SpriteNode.LocalNorm = normTransform;
                bone.SpriteNode.Invalidate();
            }
        }
    }
    
    // Inicializa huesos desde la jerarquía de sprites
    public void SyncFromSprites()
    {
        foreach (var bone in Bones)
        {
            if (bone.SpriteNode == null) continue;
            
            var world = bone.SpriteNode.World;
            bone.WorldStart = world.Position;
            
            // Calcular end basado en el tamaño del sprite (o longitud definida)
            var size = bone.SpriteNode.GetWorldSize();
            bone.Length = size.Y > size.X ? size.Y : size.X; // Mayor dimensión como longitud
            
            // Dirección basada en rotación
            bone.WorldRotation = world.Rotation;
            bone.WorldEnd = bone.WorldStart + 
                new Vector2(MathF.Cos(bone.WorldRotation), MathF.Sin(bone.WorldRotation)) * bone.Length;
        }
    }
}
```

### 3. Sistema IK Manager

```csharp
public class IKSolver
{
    private List<IKChain> _chains = new();
    private NormalizedSpriteRenderer _renderer;
    
    // Mapeo de definiciones de sprites a huesos
    private Dictionary<string, BoneDefinition> _boneDefs = new();
    
    public IKSolver(NormalizedSpriteRenderer renderer)
    {
        _renderer = renderer;
    }
    
    // Registra una cadena IK asociada a una jerarquía de sprites
    public IKChain CreateChain(string name, SpriteNode rootNode, params string[] boneNames)
    {
        var chain = new IKChain { Name = name };
        
        Bone previousBone = null;
        foreach (var boneName in boneNames)
        {
            var node = FindNode(rootNode, boneName);
            if (node == null) continue;
            
            var bone = new Bone
            {
                Name = boneName,
                SpriteNode = node,
                Parent = previousBone
            };
            
            if (previousBone != null)
                previousBone.Children.Add(bone);
                
            chain.Bones.Add(bone);
            previousBone = bone;
        }
        
        chain.SyncFromSprites(); // Inicializar posiciones mundo
        _chains.Add(chain);
        return chain;
    }
    
    // Resuelve todas las cadenas hacia sus objetivos
    public void Update(GameTime gameTime)
    {
        foreach (var chain in _chains)
        {
            if (chain.TargetPosition.HasValue)
            {
                chain.Solve(chain.TargetPosition.Value, chain.TargetRotation);
                chain.SyncToSprites();
            }
        }
    }
    
    private SpriteNode FindNode(SpriteNode root, string name)
    {
        if (root.Definition.Name == name) return root;
        foreach (var child in root.Children)
        {
            var found = FindNode(child, name);
            if (found != null) return found;
        }
        return null;
    }
}

// Extensión de IKChain con objetivo
public static class IKChainExtensions
{
    public static Vector2? TargetPosition { get; set; }
    public static float? TargetRotation { get; set; }
    
    public static void SetTarget(this IKChain chain, Vector2 worldPos, float? rotation = null)
    {
        chain.TargetPosition = worldPos;
        chain.TargetRotation = rotation;
    }
    
    public static void ClearTarget(this IKChain chain)
    {
        chain.TargetPosition = null;
        chain.TargetRotation = null;
    }
}
```

### 4. Integración Completa - Ejemplo Brazo Robot

```csharp
public class RobotArmGame : Game
{
    private NormalizedSpriteRenderer _renderer;
    private SpriteFactory _factory;
    private IKSolver _ik;
    
    // Entidades
    private SpriteNode _robotBase;
    private IKChain _armChain;
    private Vector2 _targetPosition;
    private bool _hasTarget;
    
    protected override void LoadContent()
    {
        // === DEFINICIONES CON COORDENADAS NORMALIZADAS ===
        
        // Base fija
        var baseDef = new SpriteDefinition
        {
            Name = "Base",
            TexturePath = "robot/base",
            WorldSize = new Vector2(100, 50),
            Anchor = new Vector2(0.5f, 1f) // Anclado abajo
        };
        
        // Hombro: pivote en la base
        var shoulderDef = new SpriteDefinition
        {
            Name = "Shoulder",
            TexturePath = "robot/segment",
            WorldSize = new Vector2(20, 80),
            Anchor = new Vector2(0.5f, 0f), // Anclado arriba
            Children = new List<ChildDefinition>()
        };
        
        // Codo: continúa el brazo
        var elbowDef = new SpriteDefinition
        {
            Name = "Elbow",
            TexturePath = "robot/segment",
            WorldSize = new Vector2(15, 70),
            Anchor = new Vector2(0.5f, 0f)
        };
        
        // Muñeca
        var wristDef = new SpriteDefinition
        {
            Name = "Wrist",
            TexturePath = "robot/segment",
            WorldSize = new Vector2(10, 40),
            Anchor = new Vector2(0.5f, 0f)
        };
        
        // Mano/efector final
        var handDef = new SpriteDefinition
        {
            Name = "Hand",
            TexturePath = "robot/hand",
            WorldSize = new Vector2(30, 30),
            Anchor = new Vector2(0.5f, 0.5f)
        };
        
        // Construir jerarquía: Base → Shoulder → Elbow → Wrist → Hand
        // Las posiciones normalizadas son relativas al padre:
        // (0.5, 0) = centro-arriba del padre
        
        baseDef.Children.Add(new ChildDefinition
        {
            ChildName = "Shoulder",
            Transform = new NormalizedTransform 
            { 
                Position = new Vector2(0.5f, 0f), // Centro-top de la base
                LayerDepth = 0.1f
            }
        });
        
        shoulderDef.Children.Add(new ChildDefinition
        {
            ChildName = "Elbow",
            Transform = new NormalizedTransform 
            { 
                Position = new Vector2(0.5f, 1f), // Centro-bottom del segmento (donde conecta)
                LayerDepth = 0.2f
            }
        });
        
        elbowDef.Children.Add(new ChildDefinition
        {
            ChildName = "Wrist",
            Transform = new NormalizedTransform 
            { 
                Position = new Vector2(0.5f, 1f),
                LayerDepth = 0.3f
            }
        });
        
        wristDef.Children.Add(new ChildDefinition
        {
            ChildName = "Hand",
            Transform = new NormalizedTransform 
            { 
                Position = new Vector2(0.5f, 1f),
                LayerDepth = 0.4f
            }
        });
        
        // === INICIALIZACIÓN ===
        
        var defs = new[] { baseDef, shoulderDef, elbowDef, wristDef, handDef };
        _factory = new SpriteFactory(defs);
        _renderer = new NormalizedSpriteRenderer(
            new SpriteBatch(GraphicsDevice), Content)
        {
            WorldUnitsPerPixel = 1f
        };
        _renderer.Preload(defs);
        
        _ik = new IKSolver(_renderer);
        
        // Crear instancia
        _robotBase = _factory.Create("Base", new NormalizedTransform
        {
            Position = new Vector2(0.5f, 0.9f), // Centro-abajo de pantalla
            LayerDepth = 0f
        });
        
        // Crear cadena IK vinculada a los sprites
        _armChain = _ik.CreateChain("Arm", _robotBase, 
            "Shoulder", "Elbow", "Wrist", "Hand");
        
        // Configurar restricciones articulares (opcional)
        _armChain.Bones[0].MinAngle = -MathF.PI / 2; // Hombro
        _armChain.Bones[0].MaxAngle = MathF.PI / 2;
        
        _armChain.Bones[1].MinAngle = -MathF.PI * 0.8f; // Codo
        _armChain.Bones[1].MaxAngle = MathF.PI * 0.1f;  // Solo dobla hacia adentro
        
        _armChain.Bones[2].MinAngle = -MathF.PI / 4; // Muñeca
        _armChain.Bones[2].MaxAngle = MathF.PI / 4;
    }
    
    protected override void Update(GameTime gameTime)
    {
        var mouse = Mouse.GetState();
        
        if (mouse.LeftButton == ButtonState.Pressed)
        {
            // Target en coordenadas de mundo (píxeles)
            _targetPosition = new Vector2(mouse.X, mouse.Y);
            _hasTarget = true;
            
            // Opcional: rotación objetivo según dirección del mouse
            var hand = _armChain.EndEffector;
            var toTarget = _targetPosition - hand.WorldStart;
            var targetRot = MathF.Atan2(toTarget.Y, toTarget.X);
            
            _armChain.SetTarget(_targetPosition, targetRot);
        }
        else if (mouse.RightButton == ButtonState.Pressed)
        {
            _hasTarget = false;
            _armChain.ClearTarget();
        }
        
        // Actualizar IK
        _ik.Update(gameTime);
        
        // Animación suave si no hay target (pose idle)
        if (!_hasTarget)
        {
            var t = (float)gameTime.TotalGameTime.TotalSeconds;
            var idlePos = new Vector2(
                400 + MathF.Cos(t) * 100,
                300 + MathF.Sin(t * 2) * 50);
            _armChain.SetTarget(idlePos);
        }
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkSlateBlue);
        
        _renderer.Draw(_robotBase);
        
        // Debug: dibujar líneas de huesos
        if (_hasTarget)
        {
            DrawBoneDebug();
            DrawTarget();
        }
    }
    
    private void DrawBoneDebug()
    {
        var sb = new SpriteBatch(GraphicsDevice);
        sb.Begin();
        
        foreach (var bone in _armChain.Bones)
        {
            // Línea del hueso
            DrawLine(sb, bone.WorldStart, bone.WorldEnd, Color.Yellow);
            // Articulación
            DrawCircle(sb, bone.WorldStart, 5, Color.Red);
        }
        
        // Efector final
        DrawCircle(sb, _armChain.EndEffector.WorldEnd, 8, Color.Green);
        
        sb.End();
    }
    
    private void DrawTarget()
    {
        var sb = new SpriteBatch(GraphicsDevice);
        sb.Begin();
        DrawCircle(sb, _targetPosition, 10, Color.Cyan * 0.5f);
        sb.End();
    }
    
    // Helpers de debug...
    void DrawLine(SpriteBatch sb, Vector2 a, Vector2 b, Color c) { }
    void DrawCircle(SpriteBatch sb, Vector2 p, float r, Color c) { }
}
```

### 5. Sistema Avanzado: FABRIK + Múltiples Cadenas

```csharp
// FABRIK (Forward And Backward Reaching IK) - más estable que CCD
public class FABRIKChain : IKChain
{
    public override bool Solve(Vector2 target, float? targetRot = null)
    {
        if (Bones.Count == 0) return false;
        
        var positions = Bones.Select(b => b.WorldStart).ToList();
        positions.Add(EndEffector.WorldEnd); // Agregar end effector
        
        var rootPos = positions[0];
        var totalLength = Bones.Sum(b => b.Length);
        var distToTarget = (target - rootPos).Length();
        
        // Target inalcanzable: estirar completamente
        if (distToTarget > totalLength)
        {
            for (int i = 0; i < positions.Count - 1; i++)
            {
                var dir = (target - positions[i]).Normalized();
                positions[i + 1] = positions[i] + dir * Bones[i].Length;
            }
        }
        else
        {
            // Iteraciones FABRIK
            for (int iter = 0; iter < MaxIterations; iter++)
            {
                // Forward: desde target hacia raíz
                positions[positions.Count - 1] = target;
                for (int i = positions.Count - 2; i >= 0; i--)
                {
                    var dir = (positions[i] - positions[i + 1]).Normalized();
                    positions[i] = positions[i + 1] + dir * Bones[i].Length;
                }
                
                // Backward: desde raíz hacia target
                positions[0] = rootPos;
                for (int i = 0; i < positions.Count - 1; i++)
                {
                    var dir = (positions[i + 1] - positions[i]).Normalized();
                    positions[i + 1] = positions[i] + dir * Bones[i].Length;
                }
                
                // Convergencia
                if ((positions[positions.Count - 1] - target).LengthSquared() < Threshold * Threshold)
                    break;
            }
        }
        
        // Aplicar posiciones calculadas a huesos
        for (int i = 0; i < Bones.Count; i++)
        {
            Bones[i].WorldStart = positions[i];
            Bones[i].WorldEnd = positions[i + 1];
            Bones[i].WorldRotation = MathF.Atan2(
                positions[i + 1].Y - positions[i].Y,
                positions[i + 1].X - positions[i].X);
        }
        
        if (targetRot.HasValue)
            OrientEndEffector(targetRot.Value);
            
        return true;
    }
}

// Sistema de múltiples cadenas con prioridad
public class IK Rig
{
    private List<IKChain> _chains = new();
    private Dictionary<Bone, List<IKConstraint>> _constraints = new();
    
    // Resuelve cadenas en orden de prioridad, considerando conflictos
    public void SolveRig()
    {
        // 1. Cadenas de alta prioridad (manos, pies)
        // 2. Cadenas de baja prioridad (cola, orejas)
        // 3. Resolver conflictos en articulaciones compartidas mediante blending
    }
}
```

## Diagrama de Flujo

```
INPUT: Target (Mundo)
         │
         ▼
    ┌─────────┐
    │  IK Solve │ ← Trabaja en coordenadas MUNDO
    │  (CCD/FABRIK)│   (posiciones absolutas)
    └─────────┘
         │
         ▼
    Bone.WorldPos ──┐
    Bone.WorldRot   │  ← Resultado IK
         │          │
         ▼          │
    ToNormalized()  │  ← Conversión a 0-1
         │          │
         ▼          │
    SpriteNode.LocalNorm  ← Actualiza jerarquía
         │          │
         ▼          │
    Invalidate() ◄──┘
         │
         ▼
    WorldTransform (recalculado)
         │
         ▼
    SpriteBatch.Draw()
```

## Casos de Uso Comunes

| Escenario | Implementación |
|-----------|---------------|
| **Brazo apuntando al mouse** | IKChain de 2-3 huesos, target = mouse pos |
| **Pies adaptándose a terreno** | Raycast al suelo → IK para piernas |
| **Cabeza mirando al jugador** | Cadena corta cuello-cabeza, constrain ángulos |
| **Cola procedural** | FABRIK con delay/lag para suavidad |
| **Escalada** | Múltiples cadenas: manos y pies con targets en holds |

¿Te gustaría que profundice en algún aspecto específico?
- **Restricciones articulares** (conos de rotación)
- **IK de 2 extremos** (codo/knee hint)
- **Blend entre animación e IK**
- **CCD 3D** (para juegos con profundidad)
User: Por qué utiliza coordenadas de mundo en lugar de relativas?
Kimi: Tienes toda la razón. Me he contradicho. Si el sistema de sprites ya usa **coordenadas normalizadas (0-1)** relativas, el IK debería trabajar directamente en ese mismo espacio, no en mundo.

Aquí está la corrección:

## IK en Coordenadas Normalizadas (0-1)

### 1. Hueso en Espacio Normalizado

```csharp
public class Bone
{
    public string Name { get; set; }
    public SpriteNode SpriteNode { get; set; }
    
    // Longitud relativa al padre (0-1)
    // 1.0 = tamaño completo del padre, 0.5 = mitad, etc.
    public float RelativeLength { get; set; }
    
    // Estado en coordenadas normalizadas
    public Vector2 NormStart { get; set; }  // Posición relativa al padre (0-1)
    public Vector2 NormEnd { get; set; }    // Calculado desde Start + Length + Rotación
    public float NormRotation { get; set; } // Rotación relativa al padre
    
    // Restricciones en espacio normalizado
    public float MinAngle { get; set; } = -MathF.PI;
    public float MaxAngle { get; set; } = MathF.PI;
    
    public Bone Parent { get; set; }
    public List<Bone> Children { get; } = new();
    
    // Conversión directa a transform del sprite
    public NormalizedTransform ToSpriteTransform()
    {
        return new NormalizedTransform
        {
            Position = NormStart,
            Rotation = NormRotation,
            Scale = Vector2.One
        };
    }
}
```

### 2. Cadena IK en 0-1

```csharp
public class IKChain
{
    public List<Bone> Bones { get; } = new();
    
    // Target en coordenadas normalizadas (relativo a la raíz de la cadena)
    public Vector2? NormTarget { get; set; }
    
    public bool Solve()
    {
        if (Bones.Count < 2 || !NormTarget.HasValue) return false;
        
        var target = NormTarget.Value;
        
        // CCD en espacio normalizado
        for (int iter = 0; iter < 10; iter++)
        {
            for (int i = Bones.Count - 2; i >= 0; i--)
            {
                var bone = Bones[i];
                var effector = Bones[Bones.Count - 1];
                
                // Vectores en espacio normalizado
                var toEffector = effector.NormEnd - bone.NormStart;
                var toTarget = target - bone.NormStart;
                
                if (toEffector.LengthSquared() < 0.0001f) continue;
                
                // Ángulos en espacio relativo
                var currentAngle = MathF.Atan2(toEffector.Y, toEffector.X);
                var targetAngle = MathF.Atan2(toTarget.Y, toTarget.X);
                var delta = NormalizeAngle(targetAngle - currentAngle);
                
                // Aplicar y clamp
                var newAngle = NormalizeAngle(bone.NormRotation + delta);
                bone.NormRotation = Math.Clamp(newAngle, bone.MinAngle, bone.MaxAngle);
                
                // Recalcular cadena desde este hueso
                RecalculateFrom(i);
                
                // Check convergencia
                if ((effector.NormEnd - target).LengthSquared() < 0.0001f)
                    return true;
            }
        }
        return false;
    }
    
    void RecalculateFrom(int startIdx)
    {
        for (int i = startIdx; i < Bones.Count; i++)
        {
            var bone = Bones[i];
            
            if (i == 0)
            {
                // Raíz: mantiene NormStart, calcula NormEnd
                bone.NormEnd = bone.NormStart + 
                    new Vector2(MathF.Cos(bone.NormRotation), MathF.Sin(bone.NormRotation)) * bone.RelativeLength;
            }
            else
            {
                // Hijo: conecta al final del padre
                var parent = Bones[i - 1];
                bone.NormStart = parent.NormEnd;
                bone.NormEnd = bone.NormStart + 
                    new Vector2(MathF.Cos(bone.NormRotation), MathF.Sin(bone.NormRotation)) * bone.RelativeLength;
            }
        }
    }
    
    // Aplica resultados directamente a los sprites
    public void ApplyToSprites()
    {
        foreach (var bone in Bones)
        {
            if (bone.SpriteNode != null)
            {
                bone.SpriteNode.LocalNorm = bone.ToSpriteTransform();
                bone.SpriteNode.Invalidate();
            }
        }
    }
}
```

### 3. Ejemplo Corregido - Brazo Robot

```csharp
// Definiciones con coordenadas 0-1
var shoulderDef = new SpriteDefinition
{
    Name = "Shoulder",
    TexturePath = "robot/segment",
    // Tamaño en unidades de mundo (para render)
    WorldSize = new Vector2(20, 80),
    Anchor = new Vector2(0.5f, 0f)
};

var elbowDef = new SpriteDefinition
{
    Name = "Elbow", 
    TexturePath = "robot/segment",
    WorldSize = new Vector2(15, 70),
    Anchor = new Vector2(0.5f, 0f)
};

// Jerarquía: posiciones relativas 0-1
shoulderDef.Children.Add(new ChildDefinition
{
    ChildName = "Elbow",
    Transform = new NormalizedTransform 
    { 
        Position = new Vector2(0.5f, 1f) // Centro-bottom del segmento padre
    }
});

// Crear cadena IK
var chain = new IKChain();
chain.Bones.Add(new Bone 
{ 
    Name = "Shoulder",
    SpriteNode = shoulderNode,
    NormStart = new Vector2(0.5f, 0f), // Posición relativa a su padre (base)
    RelativeLength = 0.3f, // 30% del tamaño del contenedor
    MinAngle = -MathF.PI/2,
    MaxAngle = MathF.PI/2
});

chain.Bones.Add(new Bone
{
    Name = "Elbow",
    SpriteNode = elbowNode,
    RelativeLength = 0.25f,
    MinAngle = -MathF.PI * 0.8f,
    MaxAngle = 0
});

// Target en coordenadas normalizadas (0-1)
// (0,0) = esquina sup-izq del contenedor padre
// (1,1) = esquina inf-der
var mouseNorm = new Vector2(
    Mouse.GetState().X / 1920f,
    Mouse.GetState().Y / 1080f);

chain.NormTarget = mouseNorm;
chain.Solve();
chain.ApplyToSprites();
```

## Comparación

| Aspecto | Mundo (Incorrecto) | Normalizado (Correcto) |
|---------|-------------------|------------------------|
| **Espacio de trabajo** | Píxeles/unidades | 0-1 relativo al padre |
| **Independencia resolución** | ❌ Requiere escala | ✅ Inherente |
| **Acoplamiento con sprites** | Alto (necesita conversión) | Ninguno (mismo espacio) |
| **Jerarquía anidada** | Complejo | Directo |
| **Longitud huesos** | Píxeles | Fracción del padre (0-1) |

La clave es que **todo el sistema** (sprites + IK) comparte el mismo espacio de coordenadas 0-1, eliminando conversiones constantes y manteniendo la resolución-independencia.
User: A mí modo de ver, un sistema de ik nos permite definir animaciones, estás animaciones después de pueden relacionar con huesos definidos como sprites
Kimi: Tienes toda la razón. El IK es un **generador de animaciones**, no solo un posicionador. Las animaciones IK producen **curvas de transformación** que luego se aplican a huesos (sprites). Esto separa la lógica cinemática de la representación visual.

## Arquitectura: Animaciones IK → Huesos → Sprites

### 1. Clip de Animación IK

```csharp
// Una animación es una secuencia de poses objetivo
public class IKAnimationClip
{
    public string Name { get; set; }
    public float Duration { get; set; }
    public bool Loop { get; set; }
    
    // Keyframes: en cada instante, dónde debe estar el efector
    public List<IKKeyframe> Keyframes { get; set; } = new();
}

public class IKKeyframe
{
    public float Time { get; set; } // 0 a Duration
    
    // Target del efector en coordenadas normalizadas
    public Vector2 EffectorPosition { get; set; }
    public float? EffectorRotation { get; set; }
    
    // Puede incluir hints para articulaciones intermedias
    public float? ElbowHint { get; set; }
}

// Ejemplo: animación de "saludar"
var waveClip = new IKAnimationClip
{
    Name = "Wave",
    Duration = 2f,
    Loop = true,
    Keyframes = new List<IKKeyframe>
    {
        new() { Time = 0.0f, EffectorPosition = new Vector2(0.5f, 0.8f) }, // Mano abajo
        new() { Time = 0.5f, EffectorPosition = new Vector2(0.7f, 0.3f) }, // Levantar
        new() { Time = 1.0f, EffectorPosition = new Vector2(0.9f, 0.3f) }, // Extender
        new() { Time = 1.5f, EffectorPosition = new Vector2(0.7f, 0.3f) }, // Volver
        new() { Time = 2.0f, EffectorPosition = new Vector2(0.5f, 0.8f) }  // Cerrar
    }
};
```

### 2. Player de Animaciones IK

```csharp
public class IKAnimationPlayer
{
    private IKAnimationClip _currentClip;
    private float _currentTime;
    private bool _isPlaying;
    
    // Pose resultante (output)
    public IKPose CurrentPose { get; private set; }
    
    public void Play(IKAnimationClip clip)
    {
        _currentClip = clip;
        _currentTime = 0f;
        _isPlaying = true;
    }
    
    public void Update(float deltaTime)
    {
        if (!_isPlaying || _currentClip == null) return;
        
        _currentTime += deltaTime;
        
        if (_currentTime >= _currentClip.Duration)
        {
            if (_currentClip.Loop)
                _currentTime %= _currentClip.Duration;
            else
            {
                _currentTime = _currentClip.Duration;
                _isPlaying = false;
            }
        }
        
        // Samplear la animación
        CurrentPose = Sample(_currentClip, _currentTime);
    }
    
    private IKPose Sample(IKAnimationClip clip, float time)
    {
        // Encontrar keyframes adyacentes
        var prev = clip.Keyframes.LastOrDefault(k => k.Time <= time) ?? clip.Keyframes.First();
        var next = clip.Keyframes.FirstOrDefault(k => k.Time > time) ?? prev;
        
        // Interpolar
        float t = (time - prev.Time) / (next.Time - prev.Time);
        t = MathHelper.Clamp(t, 0f, 1f);
        
        // Suavizado
        t = SmoothStep(t);
        
        return new IKPose
        {
            EffectorPosition = Vector2.Lerp(prev.EffectorPosition, next.EffectorPosition, t),
            EffectorRotation = LerpAngle(prev.EffectorRotation, next.EffectorRotation, t),
            ElbowHint = Lerp(prev.ElbowHint, next.ElbowHint, t)
        };
    }
    
    private float SmoothStep(float t) => t * t * (3f - 2f * t);
}

public struct IKPose
{
    public Vector2 EffectorPosition; // Target normalizado
    public float? EffectorRotation;
    public float? ElbowHint;
}
```

### 3. Rig: Conexión Animación → Huesos → Sprites

```csharp
public class IKRig
{
    // Definición de huesos (estructura estática)
    public List<BoneDefinition> BoneDefinitions { get; set; }
    
    // Instancia de huesos (estado runtime)
    public List<Bone> Bones { get; private set; }
    
    // Cadenas IK que resuelven las poses
    public List<IKChain> Chains { get; private set; }
    
    // Animador
    public IKAnimationPlayer Player { get; } = new();
    
    // Mapeo: cada cadena sigue una pose específica
    public Dictionary<string, IKChain> ChainMap { get; } = new();
    
    public void Initialize(SpriteNode rootNode)
    {
        // Crear huesos desde la jerarquía de sprites
        Bones = BuildBonesFromHierarchy(rootNode);
        
        // Crear cadenas IK
        foreach (var def in BoneDefinitions)
        {
            var chain = new IKChain();
            
            // Agregar huesos en orden padre→hijo
            foreach (var boneName in def.ChainNames)
            {
                var bone = Bones.First(b => b.Name == boneName);
                chain.Bones.Add(bone);
            }
            
            Chains.Add(chain);
            ChainMap[def.Name] = chain;
        }
    }
    
    public void Update(float deltaTime)
    {
        // 1. Actualizar animación (genera poses)
        Player.Update(deltaTime);
        
        if (Player.CurrentPose == null) return;
        
        // 2. Aplicar poses a cadenas IK
        foreach (var chain in Chains)
        {
            // La pose define el target del efector
            chain.NormTarget = Player.CurrentPose.EffectorPosition;
            chain.NormTargetRotation = Player.CurrentPose.EffectorRotation;
            
            // Resolver IK
            chain.Solve();
        }
        
        // 3. Aplicar resultados a sprites
        foreach (var bone in Bones)
        {
            if (bone.SpriteNode != null)
            {
                bone.SpriteNode.LocalNorm = new NormalizedTransform
                {
                    Position = bone.NormStart,
                    Rotation = bone.NormRotation,
                    Scale = Vector2.One
                };
                bone.SpriteNode.Invalidate();
            }
        }
    }
}
```

### 4. Definición de Rig (JSON/Editor)

```json
{
  "rigName": "Humanoid",
  "bones": [
    {
      "name": "Torso",
      "sprite": "body/torso",
      "parent": null,
      "normPos": [0.5, 0.5]
    },
    {
      "name": "UpperArm_L",
      "sprite": "body/arm_upper",
      "parent": "Torso",
      "normPos": [0.2, 0.3],
      "length": 0.15
    },
    {
      "name": "LowerArm_L",
      "sprite": "body/arm_lower", 
      "parent": "UpperArm_L",
      "normPos": [0.5, 1.0],
      "length": 0.12
    },
    {
      "name": "Hand_L",
      "sprite": "body/hand",
      "parent": "LowerArm_L",
      "normPos": [0.5, 1.0],
      "length": 0.05
    }
  ],
  "chains": [
    {
      "name": "LeftArm",
      "bones": ["UpperArm_L", "LowerArm_L", "Hand_L"],
      "constraints": {
        "UpperArm_L": {"min": -2.5, "max": 0.5},
        "LowerArm_L": {"min": -2.0, "max": 0}
      }
    }
  ]
}
```

### 5. Uso Completo - Personaje Interactivo

```csharp
public class Character
{
    private IKRig _rig;
    private SpriteNode _root;
    
    // Animaciones disponibles
    private Dictionary<string, IKAnimationClip> _clips = new();
    
    public void Load(ContentManager content)
    {
        // Cargar definición del rig
        var rigDef = content.Load<RigDefinition>("rigs/humanoid");
        
        // Crear jerarquía de sprites
        _root = BuildSpriteHierarchy(rigDef);
        
        // Inicializar rig IK
        _rig = new IKRig();
        _rig.BoneDefinitions = rigDef.Chains;
        _rig.Initialize(_root);
        
        // Cargar animaciones
        _clips["idle"] = content.Load<IKAnimationClip>("anims/idle");
        _clips["wave"] = content.Load<IKAnimationClip>("anims/wave");
        _clips["point"] = content.Load<IKAnimationClip>("anims/point");
        
        // Iniciar con idle
        _rig.Player.Play(_clips["idle"]);
    }
    
    public void Update(GameTime gameTime)
    {
        // Transiciones entre animaciones
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
            _rig.Player.Play(_clips["wave"]);
        else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            // Animación procedural: apuntar al mouse
            var mouseNorm = GetMouseNormalized();
            var pointClip = CreateProceduralPoint(mouseNorm);
            _rig.Player.Play(pointClip);
        }
        
        // Actualizar rig (samplea animación + resuelve IK + actualiza sprites)
        _rig.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
    }
    
    // Crear animación procedural en tiempo real
    private IKAnimationClip CreateProceduralPoint(Vector2 target)
    {
        return new IKAnimationClip
        {
            Duration = 0.1f, // Instantáneo
            Keyframes = new List<IKKeyframe>
            {
                new() { 
                    Time = 0f, 
                    EffectorPosition = target,
                    EffectorRotation = MathF.Atan2(target.Y - 0.5f, target.X - 0.5f)
                }
            }
        };
    }
    
    public void Draw(SpriteRenderer renderer)
    {
        renderer.Draw(_root);
    }
}
```

## Flujo de Datos

```
┌─────────────────┐     ┌──────────────────┐     ┌──────────────┐
│  Animation Clip │────▶│  IK Animation    │────▶│  IKPose      │
│  (Keyframes)    │     │  Player          │     │  (Target)    │
└─────────────────┘     └──────────────────┘     └──────┬───────┘
                                                        │
                              ┌───────────────────────┘
                              ▼
┌─────────────────┐     ┌──────────────────┐     ┌──────────────┐
│  Sprite Node    │◀────│  Bone            │◀────│  IK Chain    │
│  (Visual)       │     │  (Norm 0-1)      │     │  (Solver)    │
└─────────────────┘     └──────────────────┘     └──────────────┘
        ▲                                                    │
        └────────────────────────────────────────────────────┘
                         Aplica resultado
```

## Ventajas de esta Separación

| Componente | Responsabilidad | Ventaja |
|------------|----------------|---------|
| **Animation Clip** | Datos temporales | Reutilizable, editable |
| **IK Player** | Interpolación | Blending, transiciones |
| **IK Chain** | Resolver cinemática | Física/constraint real |
| **Bone** | Puente 0-1 | Independiente resolución |
| **Sprite Node** | Renderizado | Visualización pura |

Esto permite:
- **Animar un rig** y aplicarlo a **múltiples personajes**
- **Blendar** animaciones IK con animaciones tradicionales
- **Sobrescribir** dinámicamente (apuntar al mouse mientras camina)
- **Guardar/Cargar** animaciones como assets separados del código
