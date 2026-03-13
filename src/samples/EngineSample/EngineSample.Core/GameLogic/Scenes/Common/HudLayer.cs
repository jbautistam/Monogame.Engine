using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Scenes.Messages;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;

namespace EngineSample.Core.GameLogic.Scenes.Common;

/// <summary>
///		Capa con la interface de usuario del juego
/// </summary>
internal class HudLayer(AbstractScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
	// Contantes privadas
	private const string HeaderStyle = nameof(HeaderStyle);
	private const string TextStyle = nameof(TextStyle);

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
			// Genera los estilos
			Styles = builder.Build();
	}

	/// <summary>
	///		Crea los componentes de la interface
	/// </summary>
	private void CreateComponents()
	{
		UserInterfaceBuilder builder = new();

			// Añade la etiqueta de la puntuación
			builder.WithItem(new UserInterfaceLabelBuilder(this, "Score", 0.01f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithStyle(HeaderStyle)
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(this, "0", 0.07f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithStyle(TextStyle)
									.WithId("lblScore")
									.Build()
							);
			// Añade la etiqueta del número de vidas
			builder.WithItem(new UserInterfaceLabelBuilder(this, "Lives", 0.5f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithStyle(HeaderStyle)
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(this, "0", 0.57f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithStyle(TextStyle)
									.WithId("lblLives")
									.Build()
							);
			// Añade la etiqueta de la posición
			builder.WithItem(new UserInterfaceLabelBuilder(this, "Position", 0.7f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithStyle(HeaderStyle)
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(this, "0", 0.9f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithStyle(TextStyle)
									.WithId("lblCameraPosition")
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(this, "0", 0.8f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithStyle(TextStyle)
									.WithId("lblPlayerPosition")
									.Build()
							);
			// y devuelve la capa creada
			Items.AddRange(builder.Build());
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
				lblPlayerPosition.Text = GetMessageText(Constants.PlayerPosition, "-");
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
}