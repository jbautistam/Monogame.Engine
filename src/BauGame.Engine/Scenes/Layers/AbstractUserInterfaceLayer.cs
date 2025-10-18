using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers;

/// <summary>
///     Clase base para las definiciones de capas de interface de usuario
/// </summary>
public abstract class AbstractUserInterfaceLayer(AbstractScene scene, string name, int sortOrder) : AbstractLayer(scene, name, LayerType.UserInterface, sortOrder)
{
	/// <summary>
	///		Actualiza el interface de usuario de la capa
	/// </summary>
	protected override void UpdateLayer(GameTime gameTime)
	{
        if (Enabled && Scene.Camera is not null)
        {
            Rectangle viewportBounds = new(0, 0, Scene.Camera.ScreenViewport.Width, Scene.Camera.ScreenViewport.Height);

                // Actualiza el interface de usuario (antes de actualizar los elementos)
                UpdateUserInterface(gameTime);
                // Actualizar layout de elementos raíz
                foreach (UiElement element in Items)
                    if (element.Visible)
                    {
                        element.ComputeScreenBounds(viewportBounds);
                        element.Update(gameTime);
                    }
        }
	}

    /// <summary>
    ///     Actualiza el interface de usuario
    /// </summary>
    protected abstract void UpdateUserInterface(GameTime gameTime);

	/// <summary>
	///		Dibuja el interface de usuario sobre la capa
	/// </summary>
	protected override void DrawLayer(Cameras.Camera2D camera, GameTime gameTime)
	{
        if (Enabled)
            foreach (UiElement element in Items)
                if (element.Visible)
                    element.Draw(camera, gameTime);
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
    ///     Elementos de la interface de usuario
    /// </summary>
    public List<UiElement> Items { get; } = [];
}