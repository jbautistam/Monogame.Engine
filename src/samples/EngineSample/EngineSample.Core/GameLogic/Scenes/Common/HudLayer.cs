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

			// Añade la etiqueta
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
			// y devuelve la capa creada
			Items.AddRange(builder.Build());
	}

	/// <summary>
	///		Actualiza el interface de usuario
	/// </summary>
	protected override void UpdateUserInterface(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		UiLabel? lblScore = GetItem<UiLabel>("lblScore");

			// Cambia las etiquetas
			if (lblScore is not null)
				lblScore.Text = "1.221";
	}
}