using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;
using Bau.Libraries.BauGame.Engine.Entities.Common;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

/// <summary>
///     Comando para escalar un actor sobre la escala del mundo. Es posible que ScaleCommand sea más que suficiente
///     TODO: no sé si esto funciona de verdad
/// </summary>
public class ScaleToWorldCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Vector2 _startScale, _endScale;
    private bool _initialized;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        // Guarda la escala inicial
        if (!_initialized)
        {
            Size size = actor.Renderer.GetSize();

                // Obtiene los valores iniciales
                _startScale = actor.Renderer.Scale;
                // Calcula la escala final
                if (Uniform)
                    _endScale = CalculateUniformScale(size, actor.Layer.Scene.WorldDefinition.WorldBounds, Scale);
                else
                    _endScale = CalculateScale(size, actor.Layer.Scene.WorldDefinition.WorldBounds, Scale);
                // Indica que se ha inicializado correctamente
                _initialized = true;
        }
        // Asigna la escala actual
        actor.Renderer.Scale = EasingFunctionsHelper.Interpolate(_startScale, _endScale, Progress, Easing);
    }

    /// <summary>
    ///     Calcula la escala necesaria para que la textura ocupe el porcentaje indicado del tamaño de la cámara
    /// </summary>
    private Vector2 CalculateScale(Size size, Rectangle worldBounds, Vector2 scale)
    {
        if (size.Width <= 0 || size.Height <= 0)
            return Vector2.One;
        else
        {
            float targetWidth = worldBounds.Width * scale.X;
            float targetHeight = worldBounds.Height * scale.Y;

                // Devuelve el vector de escala
                return new Vector2(targetWidth / size.Width, targetHeight / size.Height);
        }
    }

    /// <summary>
    ///     Calcula la escala necesaria para que la textura mantenga el ratio de aspecto
    /// </summary>
    private Vector2 CalculateUniformScale(Size size, Rectangle worldBounds, Vector2 scale)
    {
        if (size.Width <= 0 || size.Height <= 0)
            return Vector2.One;
        else
        {
            float uniformScale = GetScale(size, worldBounds, scale);

                // Devuelve el vector de escala
                return new Vector2(uniformScale, uniformScale);
        }

        // Obtiene el valor de escala uniforme
        float GetScale(Size size, Rectangle worldBounds, Vector2 scale)
        {
            float targetWidth = worldBounds.Width * scale.X;
            float targetHeight = worldBounds.Height * scale.Y;
            float scaleX = targetWidth / size.Width;
            float scaleY = targetHeight / size.Height;

                // Develve el valor de escala adecuado para mantener el ratio de aspecto
                return MathHelper.Min(scaleX, scaleY);
        }
    }

    /// <summary>
    ///     Escala final sobre el tamaño de la cámara
    /// </summary>
    public required Vector2 Scale { get; init; }

    /// <summary>
    ///     Indica si se va a calcular un escalado uniforme
    /// </summary>
    public bool Uniform { get; set; }
}
