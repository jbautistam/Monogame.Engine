using Bau.Libraries.BauGame.Engine.Core.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Actors.Projectiles;

/// <summary>
///     Actor para presentación de explosiones
/// </summary>
public class ExplosionActor(Scenes.Layers.AbstractLayer layer) : AbstractActor(layer)
{
    // Variables privadas
    private float _currentTime;

    /// <summary>
    ///     Crea una explosión
    /// </summary>
    public void Shoot(string texture, string region, Vector2 position, float radius, int damage, float duration = 0.5f)
    {
        // Actualiza los datos de posición
        Transform.WorldBounds = new Models.RectangleF(position.X, position.Y, radius, radius);
        // Actualiza los datos de dibujo
        Renderer.Texture = texture;
        Renderer.Region = region;
        Renderer.Opacity = 1f;
        // Actualiza las propiedades
        Radius = radius;
        Damage = damage;
        Duration = duration;
        DamagesOnlyOnCreation = true;
        AppliesForce = false;
        ForceStrength = 0f;
        // Inicializa las variables y activa la explosión
        _currentTime = 0f;
        Enabled = true;
    }

    /// <summary>
    ///     Inicializa el actor
    /// </summary>
	public override void Start()
	{
	}

    /// <summary>
    ///     Actualiza el actor
    /// </summary>
	protected override void UpdateActor(GameTime gameTime)
	{
        if (Enabled)
        {
            // Incrementa el tiempo y calcula el progreso
            _currentTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
            // Cambia la opacidad
            Renderer.Opacity = 1f - MathHelper.Clamp(_currentTime / Duration, 0f, 1f);
            // Finaliza la explosión
            if (_currentTime >= Duration)
                Enabled = false;
        }
	}

    /// <summary>
    ///     Dibuja el actor
    /// </summary>
	protected override void DrawActor(Camera2D camera, GameTime gameTime)
	{
		// ... en este caso no hace nada
	}

    /// <summary>
    ///     Verifica si una posición está dentro del radio de explosión
    /// </summary>
    public bool IsInRange(Vector2 targetPosition) => Vector2.Distance(Transform.WorldBounds.TopLeft, targetPosition) <= Radius;

    /// <summary>
    ///     Calcular daño basado en distancia (daño decreciente)
    /// </summary>
    public int GetDamageAtPosition(Vector2 targetPosition)
    {
        float distance = Vector2.Distance(Transform.WorldBounds.TopLeft, targetPosition);

            if (distance > Radius) 
                return 0;
            else
            {
                float damageMultiplier = 1f - (distance / Radius);
                return (int) (Damage * damageMultiplier);
            }
    }

    /// <summary>
    ///     Calcula la fuerza aplicada basada en distancia
    /// </summary>
    public Vector2 GetForceAtPosition(Vector2 targetPosition)
    {
        if (!AppliesForce || ForceStrength <= 0) return Vector2.Zero;

        Vector2 direction = targetPosition - Transform.WorldBounds.TopLeft;
        float distance = direction.Length();
        
        if (distance > Radius || distance == 0) return Vector2.Zero;

        direction.Normalize();
        
        // Fuerza decreciente basada en distancia
        float forceMultiplier = 1f - (distance / Radius);
        return direction * ForceStrength * forceMultiplier;
    }

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor()
	{
        // ... no hace nada, sólo implementa la interface
	}

    /// <summary>
    ///     Daño provocado
    /// </summary>
    public int Damage { get; set; }

    /// <summary>
    ///     Duración de la explosión
    /// </summary>
    public float Duration { get; set; }

    /// <summary>
    ///     Radio de la explosión
    /// </summary>
    public float Radius { get; set; }

    /// <summary>
    ///     Indica si provoca daños sólo cuando se crea
    /// </summary>
    public bool DamagesOnlyOnCreation { get; set; } // Daña en el primer frame o durante toda la explosión
    
    /// <summary>
    ///     Indica si se deben aplicar fuerzas con la explosión
    /// </summary>
    public bool AppliesForce { get; set; }

    /// <summary>
    ///     Fuerza
    /// </summary>
    public float ForceStrength { get; set; }
}
