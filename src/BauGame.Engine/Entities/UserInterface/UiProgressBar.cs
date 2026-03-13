using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///     Control de barra de progreso para el interface de usuario
/// </summary>
public class UiProgressBar : UiElement
{
    // Enumerados públicos
    /// <summary>
    ///     Orientación
    /// </summary>
    public enum OrientationMode
    {
        /// <summary>Orientación horizontal</summary>
        Horizontal,
        /// <summary>Orientación vertical</summary>
        Vertical
    }
    // Variables privadas
    private float _currentValue = 0;
    private float _animationTimer;

    public UiProgressBar(AbstractUserInterfaceLayer layer, UiPosition position) : base(layer, position)
    {
        Position.Padding = new UiMargin(10);
    }

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() 
    {
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        // Normaliza el valor
        Value = Math.Clamp(Value, 0, Maximum);
        // Actualiza los elementos
        Background?.Update(gameContext);
        // 1. Animación de progreso (tweening suave)
        if (_currentValue != Value)
        {
            float progress = Math.Min(_animationTimer / MathHelper.Clamp(AnimationTime, 0.1f, AnimationTime), 1f);
            float eased = 1f - (1f - progress) * (1f - progress);
            
                // Cambia el valor actual interpolando
                _currentValue = MathHelper.Lerp(_currentValue, Value, eased);
                // Termina la animación
                if (progress >= 1f)
                {
                    _currentValue = Value;
                    _animationTimer = 0;
                }
                else
                    _animationTimer += gameContext.DeltaTime;
        }
    }

    /// <summary>
    ///     Dibuja el contenido del elemento
    /// </summary>
    public override void Draw(Camera2D camera, Managers.GameContext gameContext)
    {
        Rectangle target;
        float percent = _currentValue / Maximum;
        Styles.UiStyle style = Layer.Styles.GetDefault(Style);

            // Dibuja la textura de fondo si existe
            Background?.Renderer.Draw(camera, Position.Bounds, Vector2.Zero, 0, Color.White);
            // Calcular rectángulo de relleno según orientación y modo
            switch (Orientation)
            {
                case OrientationMode.Horizontal:
                        target = new Rectangle(Position.ContentBounds.X, Position.ContentBounds.Y, 
                                               (int) (Position.ContentBounds.Width * percent),
                                               Position.ContentBounds.Height);
                    break;
                default:
                        target = new Rectangle(Position.ContentBounds.X, Position.ContentBounds.Y, 
                                               Position.ContentBounds.Width,
                                               (int) (Position.ContentBounds.Height * percent));
                break;
            }
            // Calcula un rectángulo ligeramente más pequeño cuando tenemos una textura de fondo de la barra de progreso
            if (Bar is not null)
                target = new Rectangle(target.X + 2, target.Y + 2, target.Width - 4, target.Height - 4);
            // Dibuja la barra de progreso
            Bar?.Renderer.Draw(camera, target, Vector2.Zero, 0, style.Color * style.Opacity);
    }

    /// <summary>
    ///     Prepara los comandos de presentación
    /// </summary>
	public override void PrepareRenderCommands(Scenes.Cameras.Rendering.Builders.RenderCommandsBuilder builder, Managers.GameContext gameContext)
	{
		//TODO: aquí debería ir todo
	}

    /// <summary>
    ///     Fondo de todo el control
    /// </summary>
    public SpriteDefinition? Background { get; set; }

    /// <summary>
    ///     Imagen de la barra de progreso
    /// </summary>
    public SpriteDefinition? Bar { get; set; }

    /// <summary>
    ///     Valor máximo
    /// </summary>
    public int Maximum { get; set; }

    /// <summary>
    ///     Valor actual
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    ///     Orientación
    /// </summary>
    public OrientationMode Orientation { get; set; } = OrientationMode.Horizontal;

    /// <summary>
    ///     Duración de la animación del relleno
    /// </summary>
    public float AnimationTime { get; set; } = 0.1f;
}