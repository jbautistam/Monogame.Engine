using Bau.BauEngine.Actors;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes.Layers;
using EngineSample.Core.GameLogic.Scenes.GraphicNovel.Actors;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Managers;

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
	public CharacterDefinition Add(string name, CharacterDefinition.CharacterType type, int logicalLayer)
	{
		CharacterDefinition character = new(name, type, logicalLayer, GetLogicalZOrder(logicalLayer));

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
						AbstractCharacterActor characterActor = CreateActor(Layer, definition);

							// Asigna el Id al actor
							characterActor.Id = actorId;
							characterActor.ZOrder = characterActor.GetZOrder();
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
	///		Crea el personaje del tipo adecuado
	/// </summary>
	private AbstractCharacterActor CreateActor(AbstractLayer layer, CharacterDefinition definition)
	{
		switch (definition.Type)
		{
			case CharacterDefinition.CharacterType.Background:
				return new BackgroundActor(layer, definition, definition.LogicalLayer, GetLogicalZOrder(definition.LogicalLayer));
			default:
				return new CharacterActor(layer, definition, definition.LogicalLayer, GetLogicalZOrder(definition.LogicalLayer));
		}
	}

	/// <summary>
	///		Obtiene el ZOrder lógico dentro de la capa
	/// </summary>
	private int GetLogicalZOrder(int logicalLayer)
	{
		int zOrder = 0;

			// Busca el ZOrder máximo
			foreach (AbstractActor actor in Layer.Actors.Enumerate())
				if (actor is CharacterActor character && character.LogicalLayer == logicalLayer)
				{
					if (character.Definition.LogicalZOrder > zOrder)
						zOrder = character.Definition.LogicalZOrder;
				}
			// Devuelve el ZOrder localizado
			return zOrder;
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
	private Bau.BauEngine.Entities.Common.DictionaryModel<CharacterDefinition> Characters { get; } = new();

	/// <summary>
	///		Comandos
	/// </summary>
	public Sequences.CinematicSequenceList Sequences { get; }
}