using Microsoft.Xna.Framework;
using Bau.BauEngine.Scenes;
using Bau.BauEngine.Scenes.Cameras;
using Bau.BauEngine;
using Bau.BauEngine.Scenes.Layers.Games;
using EngineSample.Core.GameLogic.Scenes.Animations.Actors;
using Bau.BauEngine.Actors;
using Bau.BauEngine.Managers;

namespace EngineSample.Core.GameLogic.Scenes.Animations;

/// <summary>
///		Layer de la partida
/// </summary>
public class AnimationsLayer(AbstractScene scene, string name, int sortOrder) : AbstractGameLayer(scene, name, sortOrder)
{
	// Animaciones
	private List<string> AnimationGroups = [ "Player", "MonsterA", "MonsterB", "MonsterC", "MonsterD" ];
	private List<(string texture, string animation)> Animations = [ 
																	("explosion", "explosion-animation"),
																	("bombs", "bomb-1-idle"),
																	("bombs", "bomb-2-idle"),
																	("bombs", "bomb-3-idle"),
																	("bombs", "bomb-1-explosion"),
																	("bombs", "bomb-2-explosion"),
																	("bombs", "bomb-3-explosion"),
																	("corvete", "corvete-run-animation"),
																	("corvete-destroyed", "corvete-destroy-animation")
																  ];

	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartGameLayer()
	{
		float column = 0, row = 100;

			// Crea los actores con animación por grupo
			foreach (string animationGroup in AnimationGroups)
			{
				AnimationBlenderActor actor = CreateAnimation(animationGroup);

					actor.Transform.Bounds.MoveTo(column, 0);
					column += 100;
			}
			// Crea los actores con animación
			column = 0;
			foreach ((string texture, string animation) in Animations)
			{
				AnimationActor actor = CreateAnimation(texture, animation);

					actor.Transform.Bounds.MoveTo(column, row);
					if (column > 700)
					{
						row += 100;
						column = 0;
					}
					else
						column += 200;
			}
	}

	/// <summary>
	///		Crea un actor para grupos de animación
	/// </summary>
	private AnimationBlenderActor CreateAnimation(string groupAnimation)
	{
		AnimationBlenderActor actor = new(this, groupAnimation);

			// Añade el actor a la lista
			Actors.Add(actor);
			// Devuelve el actor
			return actor;
	}

	/// <summary>
	///		Crea un actor de animación
	/// </summary>
	private AnimationActor CreateAnimation(string texture, string animation)
	{
		AnimationActor actor = new(this, texture, animation);

			// Añade el actor a la lista
			Actors.Add(actor);
			// Devuelve el actor
			return actor;
	}

	/// <summary>
	///		Actualiza la capa
	/// </summary>
	protected override void UpdateGameLayer(GameContext gameContext)
	{
		if (Scene.SceneManager.EngineManager.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaultActionUp))
			UpdateAnimationsGroup(new Vector2(0, 0), false);
		if (Scene.SceneManager.EngineManager.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaultActionDown))
			UpdateAnimationsGroup(new Vector2(0, 0), true);
		if (Scene.SceneManager.EngineManager.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaultActionLeft))
			UpdateAnimationsGroup(new Vector2(-1, 0), false);
		if (Scene.SceneManager.EngineManager.InputManager.IsAction(Bau.BauEngine.Managers.Input.InputMappings.DefaultActionRight))
			UpdateAnimationsGroup(new Vector2(1, 0), false);
		if (Scene.SceneManager.EngineManager.InputManager.KeyboardManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.P))
			PlayAnimations();
	}

	/// <summary>
	///		Actualiza el grupo de animaciones
	/// </summary>
	private void UpdateAnimationsGroup(Vector2 speed, bool isDead)
	{
		foreach (AbstractActorDrawable actor in Actors.Enumerate())
			if (actor is AnimationBlenderActor blenderActor)
			{
				blenderActor.Speed = speed;
				blenderActor.IsDead = isDead;
			}
	}

	/// <summary>
	///		Inicia las animaciones
	/// </summary>
	private void PlayAnimations()
	{
		foreach (AbstractActorDrawable actor in Actors.Enumerate())
			if (actor is AnimationActor animationActor)
				animationActor.Play();
	}

	/// <summary>
	///		Dibuja la capa (los actores se dibujan por separado)
	/// </summary>
	protected override void DrawGameLayer(Bau.BauEngine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
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
