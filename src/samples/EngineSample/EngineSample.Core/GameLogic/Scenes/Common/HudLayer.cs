using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

namespace EngineSample.Core.GameLogic.Scenes.Common;

/// <summary>
///		Capa con la interface de usuario del juego
/// </summary>
internal class HudLayer(AbstractScene scene, string name, int sortOrder) : AbstractUserInterfaceLayer(scene, name, sortOrder)
{
	/// <summary>
	///		Inicializa la capa
	/// </summary>
	protected override void StartLayer()
	{
		UserInterfaceBuilder builder = new();

			// Añade la etiqueta de la puntuación
			builder.WithItem(new UserInterfaceLabelBuilder(this, "Score", 0.01f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithColor(Color.Red)
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(this, "0", 0.07f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithColor(Color.White)
									.WithId("lblScore")
									.Build()
							);
			// Añade la etiqueta de la posición
			builder.WithItem(new UserInterfaceLabelBuilder(this, "Position", 0.7f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithColor(Color.Red)
									.Build()
							);
			builder.WithItem(new UserInterfaceLabelBuilder(this, "0", 0.9f, 0.01f, 1, 1)
									.WithFont("Fonts/Hud")
									.WithColor(Color.White)
									.WithId("lblPosition")
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
		UiLabel? lblPosition = GetItem<UiLabel>("lblPosition");

			// Cambia las etiquetas
			if (lblScore is not null)
				lblScore.Text = "1.221";
			if (lblPosition is not null && Scene.Camera is not null)
				lblPosition.Text = $"{Scene.Camera.Position.X:0,00},{Scene.Camera.Position.Y:0,00}";
	}
}