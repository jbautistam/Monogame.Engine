namespace Bau.Monogame.Engine.Domain.Core.Scenes.Physics;

public class OptimizedCollisionSystem
{
    private SpatialGrid spatialGrid;
    private List<GameObject> allObjects;
    
    public OptimizedCollisionSystem(List<GameObject> objects, int screenWidth, int screenHeight)
    {
        allObjects = objects;
        spatialGrid = new SpatialGrid(screenWidth, screenHeight, 64); // 64x64 celdas
    }
    
    public void Update(GameTime gameTime)
    {
        // Limpiar y reconstruir la grilla
        spatialGrid.Clear();
        
        // Añadir todos los objetos activos a la grilla
        foreach (var obj in allObjects.Where(o => o.IsActive))
        {
            spatialGrid.AddObject(obj);
        }
        
        // Broad phase: encontrar pares potenciales
        var potentialPairs = FindPotentialPairs();
        
        // Narrow phase: verificar colisiones reales
        foreach (var pair in potentialPairs)
        {
            if (CheckActualCollision(pair.Item1, pair.Item2))
            {
                ResolveCollision(pair.Item1, pair.Item2);
            }
        }
    }
    
    private List<(GameObject, GameObject)> FindPotentialPairs()
    {
        var pairs = new HashSet<(GameObject, GameObject)>();
        
        foreach (var obj in allObjects.Where(o => o.IsActive))
        {
            var potentialColliders = spatialGrid.GetPotentialColliders(obj);
            
            foreach (var other in potentialColliders)
            {
                if (other.IsActive)
                {
                    // Evitar duplicados (A,B) y (B,A)
                    var pair = obj.Id < other.Id ? (obj, other) : (other, obj);
                    pairs.Add(pair);
                }
            }
        }
        
        return new List<(GameObject, GameObject)>(pairs);
    }
    
    private bool CheckActualCollision(GameObject obj1, GameObject obj2)
    {
        var collider1 = obj1.GetComponent<ColliderComponent>();
        var collider2 = obj2.GetComponent<ColliderComponent>();
        
        if (collider1 == null || collider2 == null) return false;
        
        return CollisionSystem.CheckCollision(collider1.Bounds, collider2.Bounds);
    }
    
    private void ResolveCollision(GameObject obj1, GameObject obj2)
    {
        CollisionSystem.ResolveCollision(obj1, obj2);
    }
}

public static class CollisionSystem
{
    // Detectar colisión entre dos rectángulos
    public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
    {
        return rect1.Intersects(rect2);
    }
    
    // Calcular penetración entre dos rectángulos
    public static Vector2 GetPenetration(Rectangle rect1, Rectangle rect2)
    {
        float xOverlap = Math.Min(rect1.Right - rect2.Left, rect2.Right - rect1.Left);
        float yOverlap = Math.Min(rect1.Bottom - rect2.Top, rect2.Bottom - rect1.Top);
        
        return new Vector2(
            xOverlap > 0 ? (rect1.Center.X < rect2.Center.X ? -xOverlap : xOverlap) : 0,
            yOverlap > 0 ? (rect1.Center.Y < rect2.Center.Y ? -yOverlap : yOverlap) : 0
        );
    }
    
    // Resolver colisión entre dos objetos
    public static void ResolveCollision(GameObject obj1, GameObject obj2)
    {
        var collider1 = obj1.GetComponent<ColliderComponent>();
        var collider2 = obj2.GetComponent<ColliderComponent>();
        var rb1 = obj1.GetComponent<RigidbodyComponent>();
        var rb2 = obj2.GetComponent<RigidbodyComponent>();
        
        if (collider1 == null || collider2 == null) return;
        
        if (CheckCollision(collider1.Bounds, collider2.Bounds))
        {
            var penetration = GetPenetration(collider1.Bounds, collider2.Bounds);
            
            // Separar objetos
            if (rb1 != null && !rb1.IsStatic)
            {
                if (rb2 == null || rb2.IsStatic)
                {
                    obj1.Position -= penetration * 0.5f;
                }
                else if (!rb2.IsStatic)
                {
                    obj1.Position -= penetration * 0.5f;
                    obj2.Position += penetration * 0.5f;
                }
            }
            else if (rb2 != null && !rb2.IsStatic)
            {
                obj2.Position += penetration * 0.5f;
            }
            
            // Resolver respuesta física
            if (rb1 != null && rb2 != null)
            {
                ResolvePhysics(rb1, rb2, penetration);
            }
        }
    }
    
    private static void ResolvePhysics(RigidbodyComponent rb1, RigidbodyComponent rb2, Vector2 penetration)
    {
        if (rb1.IsStatic && rb2.IsStatic) return;
        
        // Calcular dirección de separación
        Vector2 normal = Vector2.Zero;
        if (Math.Abs(penetration.X) < Math.Abs(penetration.Y))
            normal = new Vector2(Math.Sign(penetration.X), 0);
        else
            normal = new Vector2(0, Math.Sign(penetration.Y));
        
        // Aplicar rebote
        if (!rb1.IsStatic)
        {
            float dot1 = Vector2.Dot(rb1.Velocity, normal);
            if (dot1 < 0)
            {
                rb1.Velocity -= (1 + rb1.Bounciness) * dot1 * normal;
            }
        }
        
        if (!rb2.IsStatic)
        {
            float dot2 = Vector2.Dot(rb2.Velocity, -normal);
            if (dot2 < 0)
            {
                rb2.Velocity -= (1 + rb2.Bounciness) * dot2 * (-normal);
            }
        }
    }
}

public static class CollisionSystem
{
    public static bool ShouldCollide(GameObject obj1, GameObject obj2)
    {
        var collider1 = obj1.GetComponent<ColliderComponent>();
        var collider2 = obj2.GetComponent<ColliderComponent>();
        
        if (collider1 == null || collider2 == null) return false;
        
        // Verificar si las capas son compatibles
        return (collider1.Layer & collider2.CollidesWith) != 0 &&
               (collider2.Layer & collider1.CollidesWith) != 0;
    }
}
