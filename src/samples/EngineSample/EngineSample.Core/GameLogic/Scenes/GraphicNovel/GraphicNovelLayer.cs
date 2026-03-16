using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Games;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras.Rendering.Builders;
using EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;
using EngineSample.Core.GameLogic.Actors.Characters;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel;

/// <summary>
///		Layer de la partida
/// </summary>
public class GraphicNovelLayer(AbstractScene scene, string name, int sortOrder) : AbstractGameLayer(scene, name, sortOrder)
{
	// Variables privadas
	private CharacterManager? _characterManager;

	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartGameLayer()
	{
		CreateCharacters();
	}

	/// <summary>
	///		Crea los personajes
	/// </summary>
	private void CreateCharacters()
	{
		// Crea el manager de personajes
		_characterManager = new(this);
		// Crea los personajes
		CreateCharacter(_characterManager, "sylvie");
		CreateCharacter(_characterManager, "james");
		CreateCharacter(_characterManager, "narrator");
		// Añade el personaje
		Actors.Add(_characterManager);
	}

	/// <summary>
	///		Crea un personaje
	/// </summary>
	private void CreateCharacter(CharacterManager manager, string name)
	{
		CharacterDefinition character = manager.Add(name);

			// Crea las definiciones
			character.AddExpression(CharacterExpressionDefinition.DefaultType, $"{name}-default", null);
			character.AddExpression("avatar", $"{name}-avatar", null);
			character.AddExpression("sad", $"{name}-sad", null);
			character.AddExpression("smile", $"{name}-smile", null);
	}

	/// <summary>
	///		Actualiza la capa (los actores se actualizan por separado)
	/// </summary>
	protected override void UpdateGameLayer(GameContext gameContext)
	{
		TreatInputs(gameContext);
	}

	/// <summary>
	///		Trata las entradas
	/// </summary>
	private void TreatInputs(GameContext gameContext)
	{
		if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.S))
			CreateSylvieSequence();
		else if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.J))
			CreateJamesSequence();
		else if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.N))
			CreateNarratorSequence();
		else if (GameEngine.Instance.InputManager.KeyboardManager.JustReleased(Microsoft.Xna.Framework.Input.Keys.C))
			CreateCombinedSequence();
	}

	/// <summary>
	///		Crea una secuencia para Sylvie
	/// </summary>
	private void CreateSylvieSequence()
	{
		Actors.Characters.Sequences.Builders.SequenceBuilder builder = new("sylvie");

			// Crea la secuencia
			builder.WithStart(0)
				   .WithReset(0.1f, Vector2.Zero, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1)
				   .WithMove(0.1f, TranslateCommand.MovementMode.ToPointInmediate, Vector2.Zero)
				   .WithMove(3, TranslateCommand.MovementMode.Relative, new Vector2(0, 0.3f))
				   .WithMove(3, TranslateCommand.MovementMode.To, new Vector2(1, 0.1f))
				   .WithMove(2, TranslateCommand.MovementMode.To, new Vector2(0, 0))
				   .WithFade(2, 0.5f)
				   .WithMove(2, TranslateCommand.MovementMode.Relative, new Vector2(-0.2f, 0))
				   .WithMove(1, TranslateCommand.MovementMode.To, new Vector2(0, 0))
				   .WithFade(1, 1)
				   .WithFlash(1, 0.5f)
				   .WithExpression(3, "sad")
				   .WithZoomOnPoint(3, new Vector2(0.5f, 0.5f), new Vector2(1.5f, 1.5f))
				   .WithZoomOnPoint(1, new Vector2(0.5f, 0.5f), new Vector2(1, 1))
				   .WithEntrance(5, CinematicEntranceCommand.EntranceType.Teleport, new Vector2(0.4f, 0), 0.7f, 0.2f)
				   .WithSpriteEffects(3, Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally)
				   .WithSpriteEffects(3, Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically);
			// Añade la secuencia al manager
			_characterManager?.Sequences.Add(builder.Build());
	}

	/// <summary>
	///		Crea una secuencia para el narrador
	/// </summary>
	private void CreateNarratorSequence()
	{
		Actors.Characters.Sequences.Builders.SequenceBuilder builder = new("Narrator");

			// Crea la secuencia
			builder.WithStart(0)
				   .WithMove(0.1f, TranslateCommand.MovementMode.ToPointInmediate, Vector2.Zero)
				   .WithPose(2, DramaticPoseCommand.PoseStyle.Heroic, new Vector2(0.3f, 0), Vector2.One, 0)
				   .WithPose(2, DramaticPoseCommand.PoseStyle.Villainous, new Vector2(-0.3f, 0), Vector2.One, 0)
				   .WithPose(2, DramaticPoseCommand.PoseStyle.Comedic, new Vector2(0.3f, 0), Vector2.One, 0)
				   .WithPose(2, DramaticPoseCommand.PoseStyle.Tragic, new Vector2(-0.3f, 0), Vector2.One, 0)
				   .WithPose(2, DramaticPoseCommand.PoseStyle.Mysterious, new Vector2(0.3f, 0), Vector2.One, 0)
				   .WithMove(2, TranslateCommand.MovementMode.ToPointInmediate, new Vector2(0.3f, 0))
				   .WithFade(2, 1)
				   .WithTint(2, Color.White);
			// Añade la secuencia al manager
			_characterManager?.Sequences.Add(builder.Build());
	}

	/// <summary>
	///		Crea una secuencia para James
	/// </summary>
	private void CreateJamesSequence()
	{
		Actors.Characters.Sequences.Builders.SequenceBuilder builder = new("James");

			// Crea la secuencia
			builder.WithStart(0)
				   .WithReset(0, new Vector2(0.2f, 0), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0)
				   .WithFade(2, 1)
				   .WithJump(1, 0.2f, 0.3f)
				   .WithMove(1, TranslateCommand.MovementMode.Relative, new Vector2(0.2f, 0));
			// Añade la secuencia al manager
			_characterManager?.Sequences.Add(builder.Build());
	}

	/// <summary>
	///		Crea una secuencia combinada
	/// </summary>
	private void CreateCombinedSequence()
	{
		Actors.Characters.Sequences.Builders.SequenceBuilder builder = new("sylvie");

			// Crea la secuencia
			builder.WithStart(0)
				   .WithMove(0.1f, TranslateCommand.MovementMode.ToPointInmediate, Vector2.Zero)
				   .WithMove(3, TranslateCommand.MovementMode.Relative, new Vector2(0, 0.3f))
				   .WithMove(3, TranslateCommand.MovementMode.To, new Vector2(1, 0.1f))
				   .WithMove(2, TranslateCommand.MovementMode.To, new Vector2(0, 0))
				   .WithFade(2, 0.5f)
				   .WithMove(2, TranslateCommand.MovementMode.Relative, new Vector2(-0.2f, 0))
				   .WithMove(1, TranslateCommand.MovementMode.To, new Vector2(0, 0))
				   .WithFade(1, 1)
				   .WithFlash(1, 0.5f)
				   .WithExpression(3, "sad")
				   .WithZoomOnPoint(3, new Vector2(0.5f, 0.5f), new Vector2(1.5f, 1.5f))
				   .WithActor("James")
						.WithReset(2, new Vector2(0.1f, 0.0f), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0)
						.WithMove(2, TranslateCommand.MovementMode.Relative, new Vector2(0.3f, 0.1f), 2)
						.WithFade(2, 1, 2)
						.WithShake(3, 2, 4, true, true)
					.WithActor("Narrator")
						.WithReset(0.2f, new Vector2(1, 0.2f), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0)
						.WithMove(2, TranslateCommand.MovementMode.Relative, new Vector2(-0.5f, 0))
						.WithFade(2, 0.5f, 0)
						.WithScale(1, new Vector2(1.5f, 1.5f));
			// Añade la secuencia al manager
			_characterManager?.Sequences.Add(builder.Build());
	}

	/// <summary>
	///		Dibuja la capa (los actores se dibujan por separado)
	/// </summary>
	protected override void DrawGameLayer(Camera2D camera, GameContext gameContext)
	{
		// ... no hace nada, los actores ya se han modificado y esta capa no necesita nada más
	}

    /// <summary>
    ///     Prepara los comandos de representación de la capa
    /// </summary>
    protected override void PrepareRenderCommandsSelf(RenderCommandsBuilder builder, GameContext gameContext)
	{
		// ... no hace nada, los actores ya se han modificado y esta capa no necesita nada más
	}

	/// <summary>
	///		Finaliza la capa
	/// </summary>
	protected override void EndGameLayer()
	{
	}
}
