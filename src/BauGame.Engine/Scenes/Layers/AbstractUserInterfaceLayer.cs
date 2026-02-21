using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Popups;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers;

/// <summary>
///     Clase base para las definiciones de capas de interface de usuario
/// </summary>
public abstract class AbstractUserInterfaceLayer : AbstractLayer
{
    // Variables privadas
    private Rectangle _bounds = new();
    private bool _invalidateTransforms = true;

    public AbstractUserInterfaceLayer(AbstractScene scene, string name, int sortOrder) : base(scene, name, LayerType.UserInterface, sortOrder)
    {
        Styles = new UiStylesCollection(this);
        PopupManager = new UiPopupManager(this);
    }

	/// <summary>
	///		Actualiza las capas de la física
	/// </summary>
	protected override void UpdatePhysicsLayer(GameContext gameContext)
	{
		// ... en este caso no hace nada
	}

	/// <summary>
	///		Actualiza el interface de usuario de la capa
	/// </summary>
	protected override void UpdateLayer(GameContext gameContext)
	{
        if (Enabled && Scene.Camera is not null)
        {
            // Ajusta los límites del interface de usuario
            Bounds = new(0, 0, Scene.Camera.ScreenViewport.Width, Scene.Camera.ScreenViewport.Height);
            // Actualiza los estilos
            Styles.Update(gameContext);
            // Actualiza el interface de usuario (antes de actualizar los elementos)
            UpdateUserInterface(gameContext);
            // Actualizar layout de elementos raíz
            foreach (UiElement element in Items)
            {
                // Si han cambiado los límites de la pantalla, invalida el elemento
                if (_invalidateTransforms)
                    element.Invalidate();
                // Actualiza el elemento
                if (element.Visible)
                    element.Update(gameContext);
            }
            // Indica que los límites de pantalla no han cambiado
            _invalidateTransforms = false;
        }
	}

    /// <summary>
    ///     Actualiza el interface de usuario
    /// </summary>
    protected abstract void UpdateUserInterface(GameContext gameContext);

	/// <summary>
	///		Dibuja el interface de usuario sobre la capa
	/// </summary>
	protected override void DrawLayer(Camera2D camera, GameContext gameContext)
	{
        if (Enabled)
            foreach (UiElement element in Items)
                if (element.Visible)
                    element.Draw(camera, gameContext);
	}

    /// <summary>
    ///     Dibuja un componente en la interface de usuario con un estilo
    /// </summary>
	public void DrawStyle(Camera2D camera, string? style, UiStyle.StyleType type, Rectangle bounds, GameContext gameContext)
	{
        if (!string.IsNullOrWhiteSpace(style))
            Styles.Draw(camera, style, type, bounds, gameContext);
	}

    /// <summary>
    ///     Obtiene un elemento del interface de usuario
    /// </summary>
    public TypeElement? GetItem<TypeElement>(string id) where TypeElement : UiElement
    {
        return Items.FirstOrDefault(item => item.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase)) as TypeElement;
    }

    /// <summary>
    ///     Detiene el proceso de la capa
    /// </summary>
	protected override void EndLayer()
	{
        Items.Clear();
	}

	/// <summary>
	///     Textura de pixel
	/// </summary>
	public Texture2D? PixelTexture { get; set; }

    /// <summary>
    ///     Fuente predeterminada
    /// </summary>
    public SpriteFont? DefaultFont { get; set; }

    /// <summary>
    ///     Límites de la pantalla
    /// </summary>
    public Rectangle Bounds
    {
        get { return _bounds; }
        set
        {
            if (_bounds.X != value.X || _bounds.Y != value.Y || _bounds.Width != value.Width || _bounds.Height != value.Height)
            {
                _bounds = value;
                _invalidateTransforms = true;
            }
        }
    }

    /// <summary>
    ///     Elementos de la interface de usuario
    /// </summary>
    public List<UiElement> Items { get; } = [];

    /// <summary>
    ///     Estilos
    /// </summary>
    public UiStylesCollection Styles { get; set; }

    /// <summary>
    ///     Manager de popup
    /// </summary>
    public UiPopupManager PopupManager { get; }
}