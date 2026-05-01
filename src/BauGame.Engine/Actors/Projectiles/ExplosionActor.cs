using Bau.BauEngine.Actors.Components.Renderers;
using Bau.BauEngine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.Projectiles;

/// <summary>
///     Actor para presentación de explosiones
/// </summary>
public class ExplosionActor(Scenes.Layers.AbstractLayer layer) : AbstractActorDrawable(layer, null)
{
    // Variables privadas
    private float _currentTime;

    /// <summary>
    ///     Crea una explosión
    /// </summary>
    public void Shoot(ExplosionProperties properties, Vector2 position)
    {
        // Asigna las propiedades
        Properties = properties;
        // Actualiza los datos de posición
        Transform.Bounds = new Entities.Common.RectangleF(position.X, position.Y, 0, 0);
        // Actualiza los datos de dibujo
        Renderer.Sprite = new Entities.Sprites.SpriteDefinition(properties.Texture, properties.Region);
        Renderer.Opacity = 1;
        if (!string.IsNullOrWhiteSpace(Properties.Animation) && Renderer is RendererAnimatorComponent animator)
        {
            animator.Animator.Reset();
            animator.StartAnimation(Renderer.Sprite.Asset, Properties.Animation, false);
        }
        // Inicializa las variables y activa la explosión
        _currentTime = 0f;
        Enabled = true;
    }

    /// <summary>
    ///     Inicializa el actor
    /// </summary>
	protected override void StartActor()
	{
	}

    /// <summary>
    ///     Actualiza el actor
    /// </summary>
	protected override void UpdateActor(Managers.GameContext gameContext)
	{
        if (Enabled)
        {
            // Incrementa el tiempo y calcula el progreso
            _currentTime += gameContext.DeltaTime;
            // Cambia la opacidad cuando no hay animación pero sí duración
            if (Properties is not null && Properties.Duration > 0 && !Properties.HasAnimation)
                Renderer.Opacity = 1f - MathHelper.Clamp(_currentTime / Properties!.Duration, 0f, 1f);
            // Finaliza la explosión
            if (HasEndDuration() || HasEndAnimation())
            {
                Enabled = false;
                Renderer.Opacity = 0;
            }
        }

        // Comprueba si ha finalizado la explosión por la duración
        bool HasEndDuration() => Properties?.Duration > 0 && _currentTime > Properties.Duration;

        // Comprueba si ha finalizado la explosión por la animación
        bool HasEndAnimation()
        {
             if ((Properties?.HasAnimation ?? false) && Renderer is RendererAnimatorComponent animator)
                return !animator.Animator.IsPlaying;
            else
                return false;
        }
	}

    /// <summary>
    ///     Dibuja el actor
    /// </summary>
	protected override void DrawSelf(Scenes.Rendering.AbstractRenderingManager renderingManager, Managers.GameContext gameContext)
	{
		// ... en este caso no hace nada
	}

    /// <summary>
    ///     Verifica si una posición está dentro del radio de explosión
    /// </summary>
    public bool IsInRange(Vector2 targetPosition) => Vector2.Distance(Transform.Bounds.Location, targetPosition) <= Properties?.Radius;

    /// <summary>
    ///     Calcular daño basado en distancia (daño decreciente)
    /// </summary>
    public int GetDamageAtPosition(Vector2 targetPosition)
    {
        int damage = 0;

            // Calcula el daño sobre un objetivo
            if (Properties is not null)
            {
                float distance = Vector2.Distance(Transform.Bounds.Location, targetPosition);

                    if (distance <= Properties.Radius) 
                    {
                        float damageMultiplier = 1f - (distance / Properties.Radius);

                            damage = (int) (Properties.Damage * damageMultiplier);
                    }
            }
            // Devuelve el daño
            return damage;
    }

    /// <summary>
    ///     Calcula la fuerza aplicada basada en distancia
    /// </summary>
    public Vector2 GetForceAtPosition(Vector2 targetPosition)
    {
        Vector2 direction = Vector2.Zero;

            // Calcula la dirección
            if (Properties is not null && Properties.AppliesForce && Properties.ForceStrength > 0)
            {
                direction = targetPosition - Transform.Bounds.Location;
                float distance = direction.Length();
        
                if (distance < Properties.Radius && distance != 0)
                {
                    float forceMultiplier = 1f - (distance / Properties.Radius);

                        // Normaliza la dirección
                        direction.Normalize();
                        // Calcula una fuerza decreciente basada en distancia
                        direction = direction * Properties.ForceStrength * forceMultiplier;
                }
            }
            // Devuelve la dirección de la fuerza
            return direction;
    }

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor(Managers.GameContext gameContext)
	{
        // ... no hace nada, sólo implementa la interface
	}

    /// <summary>
    ///     Propiedades 
    /// </summary>
    public ExplosionProperties? Properties { get; private set; }
}
