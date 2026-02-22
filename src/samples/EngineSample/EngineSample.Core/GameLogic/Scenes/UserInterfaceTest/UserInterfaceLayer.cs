using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Scenes.Messages;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.EventArguments;

namespace EngineSample.Core.GameLogic.Scenes.UserInterfaceTest;

/// <summary>
///		Layer de la partida
/// </summary>
public class UserInterfaceLayer(AbstractScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
	// Contantes privadas
	private const string DefaultFontName = "Fonts/Hud";
	private const string HeaderStyle = nameof(HeaderStyle);
	private const string TextStyle = nameof(TextStyle);
	private const string TypeWriterStyle = nameof(TypeWriterStyle);
	private const string ButtonStyle = nameof(ButtonStyle);
	private const string PromptControlName = nameof(PromptControlName);
	private const string MenuStyle = nameof(MenuStyle);
	private const string MenuOptionsStyle = nameof(MenuOptionsStyle);
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
	private UiMenu? _menu;

	/// <summary>
	///		Inicializa la capa
	/// </summary>
	protected override void StartLayer()
	{
		CreateStyles();
		CreateComponents();
	}

	/// <summary>
	///		Crea los estilos de la capa
	/// </summary>
	private void CreateStyles()
	{
		UserInterfaceStylesBuilder builder = new(this);

			// Estilos de las etiquetas
			builder.WithStyle(HeaderStyle, UiStyle.StyleType.Normal)
									.WithColor(Color.Red);
			builder.WithStyle(TextStyle, UiStyle.StyleType.Normal)
									.WithColor(Color.White);
			// Estilos del cuadro de texto de máquina de escribir
			builder.WithStyle(TypeWriterStyle, UiStyle.StyleType.Normal)
									.WithBorder(Color.Red, new Vector2(3, 3), 5)
									.WithBackground("sprites/gradient", null, 0.7f)
									.WithPadding(25, 8);
			// Estilos de la etiqueta de menú
			builder.WithStyle(ButtonStyle, UiStyle.StyleType.Normal)
									.WithBackground("Tiles/BlockA4")
									.WithColor(Color.Green)
				   .WithStyle(ButtonStyle, UiStyle.StyleType.Selected)
									.WithBackground("Tiles/BlockA0")
									.WithColor(Color.Navy)
				   .WithStyle(ButtonStyle, UiStyle.StyleType.Hover)
									.WithBackground("Tiles/BlockB0")
									.WithColor(Color.White)
				   .WithStyle(ButtonStyle, UiStyle.StyleType.Disabled)
									.WithBackground("Tiles/BlockA3")
									.WithColor(Color.Gray);
			// Estilos del menú
			builder.WithStyle(MenuStyle, UiStyle.StyleType.Normal)
									.WithBackground("sprites/gradient")
									.WithColor(Color.Green);
			// Estilos de las opciones de menú
			builder.WithStyle(MenuOptionsStyle, UiStyle.StyleType.Normal)
									.WithBackground("Tiles/BlockA4")
									.WithColor(Color.Green)
				   .WithStyle(MenuOptionsStyle, UiStyle.StyleType.Selected)
									.WithBackground("Tiles/BlockA0")
									.WithColor(Color.Navy)
				   .WithStyle(MenuOptionsStyle, UiStyle.StyleType.Hover)
									.WithBackground("Tiles/BlockB0")
									.WithColor(Color.White)
				   .WithStyle(MenuOptionsStyle, UiStyle.StyleType.Disabled)
									.WithBackground("Tiles/BlockA3")
									.WithColor(Color.Gray);
			// Genera los estilos
			Styles = builder.Build();
	}

	/// <summary>
	///		Crea los componentes
	/// </summary>
	private void CreateComponents()
	{
		UserInterfaceBuilder builder = new();
		UiButton button;
		UiVisualNovelText typeWriter;

			// Añade la etiqueta de la puntuación
			builder.WithItem(new UserInterfaceLabelBuilder(this, "Score", 0.01f, 0.01f, 1, 1)
									.WithFont(DefaultFontName)
									.WithStyle(HeaderStyle)
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(this, "0", 0.07f, 0.01f, 1, 1)
									.WithFont(DefaultFontName)
									.WithStyle(TextStyle)
									.WithId("lblScore")
									.Build()
							);
			// Añade la etiqueta del número de vidas
			builder.WithItem(new UserInterfaceLabelBuilder(this, "Lives", 0.7f, 0.01f, 1, 1)
									.WithFont(DefaultFontName)
									.WithStyle(HeaderStyle)
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(this, "0", 0.8f, 0.01f, 1, 1)
									.WithFont(DefaultFontName)
									.WithStyle(TextStyle)
									.WithId("lblLives")
									.Build()
							);
			// Añade la barra de progreso
			builder.WithItem(new UserInterfaceProgressBarBuilder(this, 0.5f, 0.5f, 0.3f, 0.05f)
									.WithMaximum(100)
									.WithValue(50)
									.WithBar("sprites/gradient")
									.Build()
							);
			// Añade una imagen
			builder.WithItem(new UserInterfaceImageBuilder(this, "james-sad", "", 0.7f, 0.2f, 0.3f, 1f)
									.WithStretch(true, true)
									.WithId(CharacterJames)
									.Build()
							);
			// Añade el cuadro de diálogo que imita la máquina de escribir
			typeWriter = new UserInterfaceVisuaNovelTextBuilder(this, GetTypeWriterText(0), 0.05f, 0.7f, 0.75f, 0.2f)
									.WithLeftAvatar("james-avatar", null, false, string.Empty)
									.WithRightAvatar("sylvie-avatar", null, false, string.Empty)
									.WithFont(DefaultFontName)
									.WithSpeed(0.05f)
									.WithTextStyle(TypeWriterStyle)
									.WithId(PromptControlName)
								.Build();
			if (typeWriter.TypeWriter is not null)
				typeWriter.TypeWriter.CommandExecute += (sender, args) => TreatTypeWriterEvent(args);
			builder.WithItem(typeWriter);
			// Añade un botón
			button = new UserInterfaceButtonBuilder(this, 0.8f, 0.7f, 0.2f, 0.2f)
									.WithLabel(new UserInterfaceLabelBuilder(this, "Next", 0.8f, 0.8f, 0.2f, 0.2f).Build())
									.WithId("cmdNext")
									.WithStyle(ButtonStyle)
									.Build();
			button.Click += (sender, args) => TreatClickNextButton();
			builder.WithItem(button);
			// Añade el menú
			builder.WithItem(CreateMainMenu(this));
			// y añade los elementos
			Items.AddRange(builder.Build());
	}

	/// <summary>
	///		Crea el menú principal
	/// </summary>
	private UiMenu CreateMainMenu(AbstractUserInterfaceLayer layer)
	{
		UserInterfaceMenuBuilder builder = new(layer, 0.05f, 0.05f, 0.4f, 0.4f);

			// Asigna los elementos al menú
			builder.WithOption((int) MenuOption.TypeWriterFull, "Type writer full", DefaultFontName, 0.2f, 0, 0.6f, 1)
					.WithOption((int) MenuOption.TypeWriterWords, "Type writer words", DefaultFontName, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.TypeWriterCharacter, "Type writer characters", DefaultFontName, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.TypeWriterShowLeftAvatar, "Show left avatar", DefaultFontName, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.TypeWriterShowRightAvatar, "Show right avatar", DefaultFontName, 0.2f, 0.4f, 0.6f, 1)
					.WithOption((int) MenuOption.CharacterJamesUpdate, "Update James", DefaultFontName, 0.2f, 0.4f, 0.6f, 1)
					.WithOptionsStyle(MenuOptionsStyle)
					.WithStyle(MenuStyle);
			// Guarda el menú en una variable
			_menu = builder.Build();
			return _menu;
	}

	/// <summary>
	///		Actualiza el interface de usuario
	/// </summary>
	protected override void UpdateUserInterface(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		UiLabel? lblScore = GetItem<UiLabel>("lblScore");
		UiLabel? lblLives = GetItem<UiLabel>("lblLives");
		UiLabel? lblCameraPosition = GetItem<UiLabel>("lblCameraPosition");
		UiLabel? lblPlayerPosition = GetItem<UiLabel>("lblPlayerPosition");

			// Trata las opciones de menú
			TreatMenu();
			// Cambia la etiqueta de puntuación
			if (lblScore is not null)
				lblScore.Text = GetMessageText("Score", "0");
			// Cambia la etiqueta de número de vidas
			if (lblLives is not null)
				lblLives.Text = GetMessageText("Lives", "0");
			// Cambia la etiqueta de la posición de cámara
			if (lblCameraPosition is not null && Scene.Camera is not null)
				lblCameraPosition.Text = $"{Scene.Camera.Position.X:0,00},{Scene.Camera.Position.Y:0,00}";
			// Cambia la etiqueta de la posición del jugador
			if (lblPlayerPosition is not null)
				lblPlayerPosition.Text = GetMessageText("PlayerPosition", "-");
			// Marca los mensajes como leidos
			Scene.MessagesManager.MarkReceived(Name);
	}

	/// <summary>
	///		Trata las opciones del menú
	/// </summary>
	private void TreatMenu()
	{
		UiVisualNovelText? lblTypeWriter = GetTypeWriter();

			// Trata la opción seleccionada
			if (_menu is not null && lblTypeWriter is not null)
				switch ((MenuOption?) _menu.GetAndResetClickOption())
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
							lblTypeWriter.LeftAvatarVisible = !lblTypeWriter.LeftAvatarVisible;
						break;
					case MenuOption.TypeWriterShowRightAvatar:
							lblTypeWriter.RightAvatarVisible = !lblTypeWriter.RightAvatarVisible;
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
		MessageModel? message = Scene.MessagesManager.GetFirstReceived(Name, type);

			// Devuelve el contenido del mensaje
			if (message is not null)
				return message.Message;
			else
				return defaultValue;
	}

	/// <summary>
	///		Trata el evento cuando se pulsa el botón de siguiente
	/// </summary>
	private void TreatClickNextButton()
	{
		UiVisualNovelText? lblTypeWriter = GetTypeWriter();

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
	}

	/// <summary>
	///		Trata el evento cuando se pulsa el botón de siguiente
	/// </summary>
	private UiVisualNovelText? GetTypeWriter() => GetItem<UiVisualNovelText>(PromptControlName);

	/// <summary>
	///		Obtiene el texto para escribir en la etiqueta que simula la máquina de escribir
	/// </summary>
	private string GetTypeWriterText(int actual)
	{
		List<string> texts = [
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
								Cuidado ahí ... [event=sound(name=sounds/music)] espero que te haya gustado
								"""
							];

			// Devuelve un texto
			return texts[actual % texts.Count];
	}
}
