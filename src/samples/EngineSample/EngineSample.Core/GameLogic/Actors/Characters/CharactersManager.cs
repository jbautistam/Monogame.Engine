using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;

namespace EngineSample.Core.GameLogic.Actors.Characters;

/// <summary>
///		Actor del manager de personajes
/// </summary>
public class CharacterManager : AbstractActor
{
	// Eventos públicos
	public event EventHandler? EndSequence;
	// Variables privadas
	private bool _raisedEvent = true; // ... no se debe lanzar el evento la primera vez

	public CharacterManager(AbstractLayer layer) : base(layer, 0)
	{
		Sequences = new Sequences.CinematicSequenceList(this);
	}

	/// <summary>
	///		Añade un personaje
	/// </summary>
	public CharacterDefinition Add(string name)
	{
		CharacterDefinition character = new(name);

			// Añade el personaje al diccionario
			Characters.Add(name, character);
			// Devuelve el personaje
			return character;
	}

	/// <summary>
	///		Obtiene el actor correspondiente a un Id (si no existe, se crea)
	/// </summary>
	public AbstractActor? GetActor(string actorId)
	{
		AbstractActor? actor = Layer.Actors.Get(actorId);

			// Si no encuentra el actor en la capa, lo crea
			if (actor is null)
			{
				CharacterDefinition? definition = Characters.Get(actorId);

					if (definition is not null)
					{
						CharacterActor characterActor = new(Layer, definition);

							// Asigna el Id al actor
							characterActor.Id = actorId;
							// Añade el actor a la lista
							Layer.Actors.Add(characterActor);
							// y lo asigna a la salida
							actor = characterActor;
					}
			}
			// Devuelve el actor creado
			return actor;
	}

	/// <summary>
	///		Inicializa el manager
	/// </summary>
	protected override void StartSelf()
	{
	}

	/// <summary>
	///		Actualiza los personajes
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
		// Actualiza las secuencias
		Sequences.Update(gameContext);
		// Si no hay nada por ejecutar se lanza el evento de fin
		if (!Sequences.Enabled && !_raisedEvent)
		{
			// Lanza el evento de fin de secuencia
			EndSequence?.Invoke(this, EventArgs.Empty);
			// Indica que se ha lanzado el evento
			_raisedEvent = true;
		}
	}

	/// <summary>
	///		Finaliza el trabajo con los personajes
	/// </summary>
	protected override void EndSelf(GameContext gameContext)
	{
	}

	/// <summary>
	///		Personajes
	/// </summary>
	private Bau.Libraries.BauGame.Engine.Entities.Common.DictionaryModel<CharacterDefinition> Characters { get; } = new();

	/// <summary>
	///		Comandos
	/// </summary>
	public Sequences.CinematicSequenceList Sequences { get; }
}