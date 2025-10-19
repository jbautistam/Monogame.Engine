using Microsoft.Xna.Framework;

using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using EngineSample.Core.GameLogic.Actors.Characters.Actions;

namespace EngineSample.Core.GameLogic.Actors.Characters;

/// <summary>
///		Actor de un personaje del cómic
/// </summary>
public class CharacterActor(CharacterManager manager, int zOrder) : AbstractActor(manager.Layer, zOrder)
{
	// Variables privadas
	private CharacterActorDefinition? _actualDefinition;

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public override void Start()
	{
		// Inicializa los componentes
		foreach (CharacterActorDefinition definition in Definitions)
			definition.Start();
		//// Crea el componente de animación
		//_animator = new AnimatorComponent(this); 
		//// Añade el animador a la lista de componentes
		//Components.Add(_animator);
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
		// Ejecuta las acciones
		if (Actions.Count > 0)
		{
			bool end = Actions[0].Update(this, gameContext);

				// Si se ha terminado la acción, la elimina de la lista
				if (end)
					Actions.RemoveAt(0);
				// Indica que el actor está activo
				Enabled = true;
		}
		// Ajusta los datos de dibujo
		Renderer.Texture = _actualDefinition?.Texture;
		Renderer.Region = _actualDefinition?.Region;
		Renderer.Opacity = Opacity;
		Renderer.ScaleToViewPort = true;
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawActor(Camera2D camera, Bau.Libraries.BauGame.Engine.Managers.GameContext gameContext)
	{
/*
		if (_actualDefinition is not null)
		{
			TextureRegion? region = _actualDefinition.GetTextureRegion();

				if (region is not null)
					region.Draw(camera, camera.WorldToScreenRelative(Transform.Position), 
								region.Center, CalculateScale(camera.ScreenViewport, region.Region, false), 
								SpriteEffects.None, Opacity * Color.White, 0);
		}
*/
/*
		TextureRegion? texture = _animator?.GetTexture();

			// Dibuja el sprite del jugador
			if (texture is not null)
			{
				SpriteEffects effect = SpriteEffects.None;

					// Cambia la orientación del sprite
					if (_speed.X > 0)
						effect = SpriteEffects.FlipHorizontally;
					// Dibuja el sprite
					texture.Draw(camera, Position, new Vector2(0, 0), new Vector2(1.5f, 1.5f), effect, Color.White, 0);
			}
*/
	}

	/// <summary>
	///		Añade una definición al actor
	/// </summary>
	public CharacterActorDefinition AddDefinition(string definitionId, string texture, string region)
	{
		CharacterActorDefinition definition = new(this, definitionId, texture, region);

			// Añade la definición a la lista
			Definitions.Add(definition);
			// Devuelve la definición
			return definition;
	}

	/// <summary>
	///		Actualiza la definición
	/// </summary>
	public void SetDefinition(string definitionId)
	{
		// Cambia la definición actual
		_actualDefinition = SearchDefinition(definitionId);
		// Si no se ha encontrado ninguna definición, asigna la predeterminada
		if (_actualDefinition is null)
			_actualDefinition = SearchDefinition(CharacterActorDefinition.DefaultType);
	}

	/// <summary>
	///		Añade una acción
	/// </summary>
	public void AddAction(AbstractCharacterAction action)
	{
		Actions.Add(action);
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor()
	{
	}

	/// <summary>
	///		Busca una definición
	/// </summary>
	public CharacterActorDefinition? SearchDefinition(string type) => Definitions.FirstOrDefault(item => item.Type.Equals(type, StringComparison.CurrentCultureIgnoreCase));

	/// <summary>
	///		Opacidad
	/// </summary>
	public float Opacity { get; set; } = 1;

	/// <summary>
	///		Definiciones
	/// </summary>
	private List<CharacterActorDefinition> Definitions { get; } = [];

	/// <summary>
	///		Acciones pendientes de ejecución
	/// </summary>
	private List<AbstractCharacterAction> Actions { get; } = [];
}