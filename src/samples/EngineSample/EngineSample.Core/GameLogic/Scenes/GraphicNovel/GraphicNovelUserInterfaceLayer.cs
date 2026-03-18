using Microsoft.Xna.Framework;
using Bau.Libraries.LibHelper.Extensors;
using Bau.BauEngine.Entities.Sprites;
using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Entities.UserInterface.TypeWriter;
using Bau.BauEngine.Entities.UserInterface.EventArguments;
using Bau.BauEngine.Entities.UserInterface.Popups.MobileChats;
using Bau.BauEngine.Entities.UserInterface.ComicBubbles;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel;

/// <summary>
///		Layer de la partida
/// </summary>
public class GraphicNovelUserInterfaceLayer(AbstractScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
	// Contantes privadas
	private const string PromptControlName = nameof(PromptControlName);
	// Enumerados privadas
	private enum MenuOption
	{
		TypeWriterFull,
		TypeWriterWords,
		TypeWriterCharacter,
		TypeWriterShowLeftAvatar, 
		TypeWriterShowRightAvatar
	}
	// Variables privadas
	private int _actualText = 0;

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
			Items.AddRange(loader.LoadScreen(this, "Settings/VisualNovel/GraphicNovelUserInterface.xml"));
			Items.Add(PrepareMobileChat());
			Items.AddRange(PrepareComicBubble());
			// Asigna los manejadores de eventos
			CommandExecute += (sender, args) => TreatTypeWriterEvent(args);
			Click += (sender, args) => TreatClick(args.Component, args.Tag);
	}

	/// <summary>
	///		Prepara un chat móvil
	/// </summary>
	private UiMobileChat PrepareMobileChat()
	{
		UiMobileChat chat = new(this, new UiPosition(0.1f, 0.1f, 0.4f, 0.4f))
								{
									Style = "MenuStyle",
									Font = new SpriteTextDefinition("Fonts/Hud"),
									MessageSpacing = 10,
									SpriteWriting = new SpriteDefinition("Tiles/BlockA3", string.Empty)
								};

			// Añade los participantes
			chat.AddParticipant(new MobileSender()
										{
											ShowName = true,
											Name = "James",
											IsPlayer = false,
											NameForecolor = Color.AliceBlue,
											Forecolor = Color.Navy,
											SpriteBackground = new SpriteDefinition("Tiles/BlockA3", string.Empty),
											BackgroundColor = Color.Yellow,
											Avatar = new SpriteDefinition("james-avatar", null)
										}
								);
			chat.AddParticipant(new MobileSender()
										{
											ShowName = true,
											Name = "Sylvie",
											IsPlayer = false,
											Forecolor = Color.Red,
											NameForecolor = Color.Navy,
											BackgroundColor = Color.Green,
											Avatar = new SpriteDefinition("sylvie-avatar", null)
										}
								);
			chat.AddParticipant(new MobileSender()
										{
											ShowName = true,
											Name = "Narrator",
											IsPlayer = true,
											Forecolor = Color.Red,
											NameForecolor = Color.Navy,
											BackgroundColor = Color.Green,
											Avatar = new SpriteDefinition("narrator-avatar", null)
										}
								);
			// Añade mensajes
			chat.AddMessage("James", "1. James Mensaje", 1);
			chat.AddMessage("Sylvie", "1. Sylvie Mensaje", 1);
			chat.AddMessage("Narrator", "1. Jugador Mensaje", 1);
			chat.AddMessage("Sylvie", "2. Sylvie Mensaje", 1);
			chat.AddMessage("James", "2 James Mensaje", 1);
			chat.AddMessage("Narrator", "Uno del jugador - 2", 1);
			chat.AddMessage("Sylvie", "Otro de Sylvie este bastante largo para saltar al cabo del tiempo en algunas líneas", 1);
			chat.AddMessage("James", "Otro de James - 2", 1);
			chat.AddMessage("Narrator", "Uno del jugador - 3", 1);
			chat.AddMessage("Sylvie", "Otro de Sylvie - 4", 1);
			chat.AddMessage("James", "Otro de James- 5", 1);
			chat.AddMessage("Narrator", "Uno del jugador - 6", 1);
			chat.AddMessage("Sylvie", "Otro de Sylvie - 7", 1);
			chat.AddMessage("James", "Otro de James- 8", 1);
			chat.AddMessage("Narrator", "Uno del jugador - 9", 1);
			chat.AddMessage("Sylvie", "Otro de Sylvie - 10", 1);
			chat.AddMessage("James", "Otro de James- 11", 1);
			chat.AddMessage("Narrator", "Ya no esperes más", 1);
			// Devuelve el chat
			return chat;
	}

	/// <summary>
	///		Prepara una lista de bocadillos de cómic
	/// </summary>
	private List<UiComicBubble> PrepareComicBubble()
	{
		List<UiComicBubble> bubbles = [];

			// Asigna las propiedades
			bubbles.Add(CreateBubble("bubbles-01", "bubble-01", 
									 "Primer texto del bocadillo que queremos que sea un poco largo para que salte entre líneas",
									 new UiPosition(0.5f, 0.1f, 0.2f, 0.2f)));
			bubbles.Add(CreateBubble("bubbles-01", "bubble-02", 
									 "Texto del segundo bocadillo",
									 new UiPosition(0.5f, 0.4f, 0.2f, 0.2f)));
			bubbles.Add(CreateBubble("bubbles-01", "bubble-03", 
									 "Texto del tercer bocadillo",
									 new UiPosition(0.85f, 0.55f, 0.1f, 0.1f)));
			// Devuelve los bocadillos creados
			return bubbles;

		// Crea una etiqueta con un bocadillo
		UiComicBubble CreateBubble(string asset, string region, string text, UiPosition position)
		{
			 return new UiComicBubble(this, position)
							{
								BubbleSprite = new SpriteDefinition(asset, region),
								Font = new SpriteTextDefinition("Fonts/Hud"),
								TextParameters = new SpriteTextParameters
															{
																Text = text,
																Color = Color.Black,
																HorizontalAlignment = UiLabel.HorizontalAlignmentType.Center,
																VerticalAlignment = UiLabel.VerticalAlignmentType.Center
															}
							};
		}
	}

	/// <summary>
	///		Actualiza el interface de usuario
	/// </summary>
	protected override void UpdateUserInterface(Bau.BauEngine.Managers.GameContext gameContext)
	{
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
				}
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
					Bau.BauEngine.GameEngine.Instance.AudioManager.PlaySong(song, Bau.BauEngine.Managers.Audio.AudioManager.TransitionType.Fade, 8);
			}
			else if (name.Equals("soundoff", StringComparison.CurrentCultureIgnoreCase))
				Bau.BauEngine.GameEngine.Instance.AudioManager.Stop();
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
								... porque lo voy a parar [event=soundoff]
								"""
							];

			// Devuelve un texto
			return texts[actual % texts.Count];
	}
}
