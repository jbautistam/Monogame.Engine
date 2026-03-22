using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Entities.UserInterface.Popups;
using Bau.BauEngine.Entities.UserInterface.Styles;
using Bau.BauEngine.Scenes.Cameras;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Entities.UserInterface.EventArguments;

namespace Bau.BauEngine.Scenes.Layers;

/// <summary>
///     Clase base para las definiciones de capas de interface de usuario
/// </summary>
public abstract class AbstractUserInterfaceLayer : AbstractLayer
{
    // Eventos públicos
    public event EventHandler<CommandEventArgs>? CommandExecute;
    public event EventHandler<ValueChangedEventArgs>? ValueChanged;
    public event EventHandler<ClickEventArgs>? Click;

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
	protected override void UpdateSelf(GameContext gameContext)
	{
        if (Enabled)
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
	protected override void DrawSelf(Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
        if (Enabled)
            foreach (UiElement item in Items)
                if (item.Visible)
                    item.Draw(renderingManager, gameContext);
	}

    /// <summary>
    ///     Dibuja un componente en la interface de usuario con un estilo
    /// </summary>
	public void DrawStyle(Rendering.RenderingManager renderingManager, string? style, UiStyle.StyleType type, Rectangle bounds, GameContext gameContext)
	{
        if (!string.IsNullOrWhiteSpace(style))
            Styles.Draw(renderingManager, style, type, bounds, gameContext);
	}

    /// <summary>
    ///     Obtiene un elemento del interface de usuario
    /// </summary>
    public TypeData? GetItem<TypeData>(string id) where TypeData : UiElement
    {
        // Busca el elemento en la lista o en sus componetes hijo
        foreach (UiElement item in Items)
            if (item.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase) && item is TypeData converted)
                return converted;
            else if (item is Entities.UserInterface.Interfaces.IComponentPanel panel)
            {
                TypeData? child = panel.GetItem<TypeData>(id);

                    if (child is not null)
                        return child;
            }
        // Si ha llegado hasta aquí es porque no ha encontrado nada
        return null;
    }

    /// <summary>
    ///     Detiene el proceso de la capa
    /// </summary>
	protected override void EndLayer()
	{
        Items.Clear();
	}

    /// <summary>
    ///     Lanza un evento de ejecución
    /// </summary>
	public void RaiseCommandExecute(CommandEventArgs commandEventArgs)
	{
        CommandExecute?.Invoke(this, commandEventArgs);
	}

    /// <summary>
    ///     Lanza un evento de pulsación sobre un componente
    /// </summary>
	public void RaiseCommandClick(ClickEventArgs clickEventArgs)
	{
		Click?.Invoke(this, clickEventArgs);
	}

    /// <summary>
    ///     Lanza un evento de modificación de un valor
    /// </summary>
	public void RaiseValueChanged(ValueChangedEventArgs valueChangedEventArgs)
	{
		ValueChanged?.Invoke(this, valueChangedEventArgs);
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