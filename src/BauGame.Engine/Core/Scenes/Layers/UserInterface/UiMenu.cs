using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Scenes.Layers.UserInterface;

/// <summary>
///		Componente para presentación de un menú
/// </summary>
public class UiMenu(UserInterfaceLayer layer, UiPosition position) : UiElement(layer, position)
{
    // Eventos públicos
    public event EventHandler<EventArguments.OptionClickEventArgs>? Click;
    // Variables privadas
    private int _selectedOption = 0, _hoverOption = -1;
    private int? _clickedOption;

    /// <summary>
    ///     Cálculo del layout del elemento
    /// </summary>
    protected override void ComputeScreenComponentBounds() 
    {
        // Calcula los límites de fondo y borde
        Background?.ComputeScreenBounds(Position.ScreenBounds);
        // Calcula los límites de los elementos hijo
        NormalizeOptionsBounds();
        foreach (UiElement element in Options)
            element.ComputeScreenBounds(Position.ScreenPaddedBounds);

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
    public override void Update(GameTime gameTime) 
    {
        // Cambia la opción seleccionada
        TreatInputs();
        TreatMouse();
        // Actualiza los elementos
        Title?.Update(gameTime);
        HoverBackground?.Update(gameTime);
        UnselectedBackground?.Update(gameTime);
        SelectedBackground?.Update(gameTime);
        // Calcula los límites de los elementos hijo
        for (int index = 0; index < Options.Count; index++)
            if (Options[index].Visible)
            {
                Options[index].Color = GetColor(index);
                Options[index].Background = GetBackground(index);
                Options[index].Update(gameTime);
            }
    }

    /// <summary>
    ///     Trata las entradas
    /// </summary>
    private void TreatInputs()
    {
        // Pasa a la opción anterior
        if (GameEngine.Instance.InputManager.KeyboardManager.JustPressed(Microsoft.Xna.Framework.Input.Keys.Up))
        {
            _selectedOption--;
            if (_selectedOption < 0)
                _selectedOption = Options.Count - 1;
        }
        // Pasa a la siguiente opción
        if (GameEngine.Instance.InputManager.KeyboardManager.JustPressed(Microsoft.Xna.Framework.Input.Keys.Down))
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
                if (_hoverOption == -1 && Options[index].Visible && Options[index].Position.ScreenBounds.Contains(mousePosition))
                    _hoverOption = index;
            // Comprueba si se ha pulsado sobre la opción seleccionada
            if (_hoverOption != -1 && GameEngine.Instance.InputManager.IsAction(Managers.Input.InputMappings.DefaultMouseClickAction))
                TreatClicked(_hoverOption);
    }

    /// <summary>
    ///     Dibuja el contenido
    /// </summary>
    public override void Draw(Cameras.Camera2D camera, GameTime gameTime)
    {
        // Dibuja el título, borde y fondo
        Title?.Draw(camera, gameTime);
        Background?.Draw(camera, gameTime);
        // Dibuja los elementos hijo
        foreach (UiElement child in Options)
            if (child.Visible)
                child.Draw(camera, gameTime);
    }

    /// <summary>
    ///     Obtiene el color asociado a una opción
    /// </summary>
    private Color GetColor(int option)
    {
        if (option == _selectedOption)
            return SelectedColor;
        else if (option == _hoverOption)
            return HoverColor;
        else
            return UnselectedColor;
    }

    /// <summary>
    ///     Obtiene el fondo asociado a una opción
    /// </summary>
    private UiBackground? GetBackground(int option)
    {
        if (option == _selectedOption && SelectedBackground is not null)
            return SelectedBackground;
        else if (option == _hoverOption && HoverBackground is not null)
            return HoverBackground;
        else
            return UnselectedBackground;
    }

    /// <summary>
    ///     Etiqueta del título
    /// </summary>
    public UiLabel? Title { get; set; }

    /// <summary>
    ///     Fondo
    /// </summary>
    public UiBackground? Background { get; set; }
    
    /// <summary>
    ///     Color de los elementos no seleccionados
    /// </summary>
    public Color UnselectedColor { get; set; } = Color.White;

    /// <summary>
    ///     Color del elemento seleccionado
    /// </summary>
    public Color SelectedColor { get; set; } = Color.Red;
    
    /// <summary>
    ///     Color de los elementos cuando el ratón está sobre el elemento
    /// </summary>
    public Color HoverColor { get; set; } = Color.Navy;

    /// <summary>
    ///     Fondo cuando no está seleccionado
    /// </summary>
    public UiBackground? UnselectedBackground { get; set; }

    /// <summary>
    ///     Fondo
    /// </summary>
    public UiBackground? SelectedBackground { get; set; }

    /// <summary>
    ///     Fondo cuando el ratón está sobre el elemento
    /// </summary>
    public UiBackground? HoverBackground { get; set; }

    /// <summary>
    ///     Elementos hijo
    /// </summary>
    public List<UiMenuOption> Options { get; } = [];
}
