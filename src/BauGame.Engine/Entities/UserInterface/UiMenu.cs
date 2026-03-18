using Bau.BauEngine.Scenes.Cameras;
using Bau.BauEngine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface;

/// <summary>
///		Componente para presentación de un menú
/// </summary>
public class UiMenu(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position), Interfaces.IComponentPanel
{
    // Variables privadas
    private int _selectedOption = 0, _hoverOption = -1;
    private int? _clickedOption;

    /// <summary>
    ///     Añade una opción
    /// </summary>
    public void AddOption(UiMenuOption option)
    {
        // Añade la opción
        Options.Add(option);
        // Asigna el padre
        option.Parent = this;
        // Invalida el control
        Invalidate();
    }

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() 
    {
        int count = Options.Count(item => item.Visible);
        float offset = 0.1f;
        float limits = 1.0f - 4 * offset;
        float height = limits / count;

            // Coloca los elementos
            foreach (UiElement element in Options)
                if (element.Visible)
                {
                    element.Position = new UiPosition(element.Position.X, offset, element.Position.Width, height);
                    offset += height + 0.01f;
                }
            // y los invalida
            foreach (UiElement element in Options)
                element.Invalidate();
    }

    /// <summary>
    ///     Obtiene un elemento del interface de usuario
    /// </summary>
    public TypeData? GetItem<TypeData>(string id) where TypeData : UiElement
    {
        // Busca el elemento en la lista o en sus componetes hijo
        foreach (UiElement item in Options)
            if (item.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase) && item is TypeData converted)
                return converted;
        // Si ha llegado hasta aquí es porque no ha encontrado nada
        return null;
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext) 
    {
        // Cambia la opción seleccionada
        TreatInputs();
        TreatMouse();
        UpdateOptions();
        // Actualiza los elementos
        Title?.Update(gameContext);
        // Calcula los límites de los elementos hijo
        for (int index = 0; index < Options.Count; index++)
            if (Options[index].Visible)
                Options[index].Update(gameContext);
    }

    /// <summary>
    ///     Actualiza las opciones
    /// </summary>
    private void UpdateOptions()
    {
        for (int index = 0; index < Options.Count; index++)
        {
            Options[index].IsPressed = index == _clickedOption;
            Options[index].IsHovered = index == _hoverOption;
            Options[index].IsSelected = index == _selectedOption;
        }
    }

    /// <summary>
    ///     Trata las entradas
    /// </summary>
    private void TreatInputs()
    {
        // Pasa a la opción anterior
        if (GameEngine.Instance.InputManager.IsAction(Managers.Input.InputMappings.DefaultActionUp, Managers.Input.InputMappings.Status.JustPressed))
        {
            _selectedOption--;
            if (_selectedOption < 0)
                _selectedOption = Options.Count - 1;
        }
        // Pasa a la siguiente opción
        if (GameEngine.Instance.InputManager.IsAction(Managers.Input.InputMappings.DefaultActionDown, Managers.Input.InputMappings.Status.JustPressed))
        {
            _selectedOption++;
            if (_selectedOption > Options.Count - 1)
                _selectedOption = 0;
        }
        // Lanza el evento cuando se ha pulsado el enter
        if (GameEngine.Instance.InputManager.IsAction(Managers.Input.InputMappings.DefaultIntroAction))
            TreatClicked(_selectedOption);
    }

    /// <summary>
    ///     Trata la pulsación sobre una opción: guarda la opción pulsada y lanza un evento
    /// </summary>
	private void TreatClicked(int selectedOption)
	{
        if (selectedOption >= 0 && selectedOption < Options.Count)
        {
            // Cambia la opción seleccionada
            _selectedOption = selectedOption;
            _clickedOption = Options[selectedOption].OptionId;
            // Lanza el evento
            Layer.RaiseCommandClick(new EventArguments.ClickEventArgs(this, Options[selectedOption].OptionId.ToString()));
        }
	}

    /// <summary>
    ///     Obtiene la opción pulsada y la resetea
    /// </summary>
    public int? GetAndResetClickOption()
    {
        int? option = _clickedOption;

            // Vacía la opción pulsada
            _clickedOption = null;
            // Devuelve la opción que se había pulsado
            return option;
    }

	/// <summary>
	///     Trata el ratón
	/// </summary>
	private void TreatMouse()
    {
        Vector2 mousePosition = GameEngine.Instance.InputManager.MouseManager.MousePosition;

            // Indica que no hay ninguna opción seleccionada
            _hoverOption = -1;
            // Comprueba si el ratón está encima de una de las opciones
            for (int index = 0; index < Options.Count; index++ )
                if (_hoverOption == -1 && Options[index].Visible && Options[index].Position.Bounds.Contains(mousePosition))
                    _hoverOption = index;
            // Comprueba si se ha pulsado sobre la opción seleccionada
            if (_hoverOption != -1 && GameEngine.Instance.InputManager.IsAction(Managers.Input.InputMappings.DefaultMouseClickAction))
                TreatClicked(_hoverOption);
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Scenes.Rendering.RenderingManager renderingManager, Managers.GameContext gameContext)
    {
        // Dibuja el título, borde y fondo
        Title?.Draw(renderingManager, gameContext);
        Layer.DrawStyle(renderingManager, Style, Styles.UiStyle.StyleType.Normal, Position.Bounds, gameContext);
        // Dibuja los elementos hijo
        foreach (UiElement child in Options)
            if (child.Visible)
                child.Draw(renderingManager, gameContext);
    }

    /// <summary>
    ///     Etiqueta del título
    /// </summary>
    public UiLabel? Title { get; set; }

    /// <summary>
    ///     Elementos hijo
    /// </summary>
    private List<UiMenuOption> Options { get; } = [];
}
