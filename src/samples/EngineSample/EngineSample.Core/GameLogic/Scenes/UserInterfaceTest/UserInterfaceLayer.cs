using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Scenes.Messages;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.EventArguments;

namespace EngineSample.Core.GameLogic.Scenes.UserInterfaceTest;

/// <summary>
///		Layer de la partida
/// </summary>
public class UserInterfaceLayer(AbstractScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
	// Contantes privadas
	private const string PromptControlName = nameof(PromptControlName);
	private const string CharacterJames = nameof(CharacterJames);
	// Enumerados privadas
	private enum MenuOption
	{
		TypeWriterFull,
		TypeWriterWords,
		TypeWriterCharacter,
		TypeWriterShowLeftAvatar, 
		TypeWriterShowRightAvatar,
		CharacterJamesUpdate
	}
	// Variables privadas
	private int _actualText = 0, _actualJames = 0;

	/// <summary>
	///		Inicializa la capa
	/// </summary>
	protected override void StartLayer()
	{
		Configuration.ConfigurationLoader loader = new();

			// Carga los estilos
			Styles = loader.LoadStyles(this, "Settings/VisualNovel/Styles.xml");
			// Carga el archivo de elementos de la pantalla
			Items.Clear();
			Items.AddRange(loader.LoadScreen(this, "Settings/VisualNovel/ScreenUserInterface.xml"));
			// Asigna los manejadores de eventos
			CommandExecute += (sender, args) => TreatTypeWriterEvent(args);
			Click += (sender, args) => TreatClick(args.Component, args.Tag);
	}

	/// <summary>
	///		Actualiza el interface de usuario
	/// </summary>
	protected override void UpdateUserInterface(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		UiLabel? lblScore = GetItem<UiLabel>("lblScore");
		UiLabel? lblLives = GetItem<UiLabel>("lblLives");

			//// Trata las opciones de menú
			//TreatMenu();
			// Cambia la etiqueta de puntuación
			if (lblScore is not null)
				lblScore.Text = GetMessageText("Score", "0");
			// Cambia la etiqueta de número de vidas
			if (lblLives is not null)
				lblLives.Text = GetMessageText("Lives", "0");
	}

	/// <summary>
	///		Trata las opciones del menú
	/// </summary>
	private void TreatMenuOption(MenuOption option)
	{
		UiVisualNovelDialog? lblTypeWriter = GetTypeWriter();

			// Trata la opción seleccionada
			if (lblTypeWriter is not null)
				switch (option)
				{
					case MenuOption.TypeWriterCharacter:
							if (lblTypeWriter.TypeWriter is not null)
								lblTypeWriter.TypeWriter.Mode = UiTypeWriterLabel.WriteMode.Characters;
						break;
					case MenuOption.TypeWriterFull:
							if (lblTypeWriter.TypeWriter is not null)
								lblTypeWriter.TypeWriter.Mode = UiTypeWriterLabel.WriteMode.Full;
						break;
					case MenuOption.TypeWriterWords:
							if (lblTypeWriter.TypeWriter is not null)
								lblTypeWriter.TypeWriter.Mode = UiTypeWriterLabel.WriteMode.Words;
						break;
					case MenuOption.TypeWriterShowLeftAvatar:
							if (lblTypeWriter.LeftAvatar is not null)
							{
								lblTypeWriter.LeftAvatar.Visible = !lblTypeWriter.LeftAvatar.Visible;
								lblTypeWriter.Invalidate();
							}
						break;
					case MenuOption.TypeWriterShowRightAvatar:
							if (lblTypeWriter.RightAvatar is not null)
							{
								lblTypeWriter.RightAvatar.Visible = !lblTypeWriter.RightAvatar.Visible;
								lblTypeWriter.Invalidate();
							}
						break;
					case MenuOption.CharacterJamesUpdate:
							UpdateCharacterImage(CharacterJames, ++_actualJames);
						break;
				}
	}

	/// <summary>
	///		Actualiza la imagen de un personaje
	/// </summary>
	private void UpdateCharacterImage(string character, int actual)
	{
		UiImage? image = GetItem<UiImage>(character);
		List<string> types = [ "james-sad", "james-smile", "james-default" ];

			// Devuelve un texto
			if (image is not null && image.Sprite is not null)
				image.Sprite.Asset = types[actual % types.Count];
	}

	/// <summary>
	///		Obtiene el texto de un mensaje
	/// </summary>
	private string GetMessageText(string type, string defaultValue)
	{
		MessageModel? message = Scene.MessagesManager.GetLastReceived(Id, type);

			// Devuelve el contenido del mensaje
			if (message is not null)
				return message.Message;
			else
				return defaultValue;
	}

	/// <summary>
	///		Trata las pulsaciones
	/// </summary>
	private void TreatClick(UiElement component, string? tag)
	{
		switch (component)
		{
			case UiMenu menu:
					if (!string.IsNullOrWhiteSpace(tag))
						TreatMenuOption((MenuOption) tag.GetInt(0));
				break;
			case UiButton button:
					if (component.Id.Equals("cmdNext", StringComparison.CurrentCultureIgnoreCase))
						TreatClickNextButton();
				break;
		}
	}

	/// <summary>
	///		Trata el evento cuando se pulsa el botón de siguiente
	/// </summary>
	private void TreatClickNextButton()
	{
		UiVisualNovelDialog? lblTypeWriter = GetTypeWriter();

			if (lblTypeWriter is not null && lblTypeWriter.TypeWriter is not null)
				lblTypeWriter.TypeWriter.Text = GetTypeWriterText(++_actualText);
	}

	/// <summary>
	///		Trata un evento del control de etiqueta
	/// </summary>
	private void TreatTypeWriterEvent(CommandEventArgs args)
	{
		(string name, Dictionary<string, string> parameters) = args.Parse();

			if (name.Equals("sound", StringComparison.CurrentCultureIgnoreCase))
			{
				if (parameters.TryGetValue("name", out string? song))
					Bau.Libraries.BauGame.Engine.GameEngine.Instance.AudioManager.PlaySong(song, Bau.Libraries.BauGame.Engine.Managers.Audio.AudioManager.TransitionType.Fade, 8);
			}
			else if (name.Equals("soundoff", StringComparison.CurrentCultureIgnoreCase))
				Bau.Libraries.BauGame.Engine.GameEngine.Instance.AudioManager.Stop();
	}

	/// <summary>
	///		Trata el evento cuando se pulsa el botón de siguiente
	/// </summary>
	private UiVisualNovelDialog? GetTypeWriter() => GetItem<UiVisualNovelDialog>(PromptControlName);

	/// <summary>
	///		Obtiene el texto para escribir en la etiqueta que simula la máquina de escribir
	/// </summary>
	private string GetTypeWriterText(int actual)
	{
		List<string> texts = [
							   "Un texto simple de pruebas",
							   """
								Quiero comprobar cuanto es el ancho de este elemento en la pantalla y para eso tenemos que tener un texto muy largo que se salga de la pantalla
								Este es un texto muy largo que queremos mostrar
								en varias líneas y poco
								a poco
								""",
								"Después de esto espera 2 segundos ... [wait=2] y continúa",
								"Escribe normal ... [speed=0.5] ahora más rápido ... [speed=4] ahora más lento\n[speed=1]... y ahora normal",
								"""
								Esto con color normal ... [color=red] esto rojo ... [color=blue] esto azul[/color]
								esto rojo de nuevo[/color] y esto con color normal
								""",
								"""
								Cuidado ahí ... [event=sound(name=sounds/music)] espero que te haya gustado [wait=2] 
								porque lo voy a parar [event=soundoff]
								"""
							];

			// Devuelve un texto
			return texts[actual % texts.Count];
	}
}
