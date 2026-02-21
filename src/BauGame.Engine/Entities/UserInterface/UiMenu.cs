using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface;

/// <summary>
///		Componente para presentación de un menú
/// </summary>
public class UiMenu(AbstractUserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    // Eventos públicos
    public event EventHandler<EventArguments.OptionClickEventArgs>? Click;
    // Variables privadas
    private int _selectedOption = 0, _hoverOption = -1;
    private int? _clickedOption;

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenBoundsSelf() 
    {
        // Calcula los límites de los elementos hijo
        NormalizeOptionsBounds();
        foreach (UiElement element in Options)
            element.ComputeScreenBounds(Position.ContentBounds);

        // Normaliza las posiciones de las opciones
        void NormalizeOptionsBounds()
        {
            int count = Options.Count(item => item.Visible);
            float offset = 0.1f;
            float limits = 1.0f - 4 * offset;
            float height = limits / count;

                foreach (UiElement element in Options)
                    if (element.Visible)
                    {
                        element.Position = new UiPosition(element.Position.X, offset, element.Position.Width, height);
                        offset += height + 0.01f;
                    }
        }
    }

    /// <summary>
    ///     Actualiza el contenido del elemento
    /// </summary>
    public override void UpdateSelf(Managers.GameContext gameContext) 
    {
        // Cambia la opción seleccionada
        TreatInputs();
        TreatMouse();
        UpdateOptions();
        // Actualiza los elementos
        Title?.UpdateSelf(gameContext);
        // Calcula los límites de los elementos hijo
        for (int index = 0; index < Options.Count; index++)
            if (Options[index].Visible)
                Options[index].UpdateSelf(gameContext);
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
            _clickedOption = Options[selectedOption].OptionId;
            Click?.Invoke(this, new EventArguments.OptionClickEventArgs(Options[selectedOption].OptionId));
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
    public override void Draw(Camera2D camera, Managers.GameContext gameContext)
    {
        // Dibuja el título, borde y fondo
        Title?.Draw(camera, gameContext);
        Layer.DrawStyle(camera, Style, Styles.UiStyle.StyleType.Normal, Position.Bounds, gameContext);
        // Dibuja los elementos hijo
        foreach (UiElement child in Options)
            if (child.Visible)
                child.Draw(camera, gameContext);
    }

    /// <summary>
    ///     Etiqueta del título
    /// </summary>
    public UiLabel? Title { get; set; }

    /// <summary>
    ///     Estilo de las opciones
    /// </summary>
    public string? StyleOptions { get; set; }

    /// <summary>
    ///     Elementos hijo
    /// </summary>
    public List<UiMenuOption> Options { get; } = [];
}
