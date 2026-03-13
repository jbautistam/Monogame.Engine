using EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;
using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Builders;

/// <summary>
///		Generador de una secuencia
/// </summary>
public class SequenceBuilder
{
	// Variables privadas
	private string _actor;

	public SequenceBuilder(string actor)
	{
		_actor = actor;
	}

	/// <summary>
	///		Cambia el actor
	/// </summary>
	public SequenceBuilder WithActor(string actor)
	{
		// Cambia el actor
		_actor = actor;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para mover el actor directamente a una posición
	/// </summary>
	public SequenceBuilder WithMoveTo(float start, float duration, Vector2 target)
	{
		// Añade el comando
		Commands.Add(new MoveToCommand(_actor, start, duration)
								{
									Target = target
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para mover el actor a una posición
	/// </summary>
	public SequenceBuilder WithMove(float start, float duration, Vector2 target)
	{
		// Añade el comando
		Commands.Add(new MoveCommand(_actor, start, duration)
								{
									Target = target
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Genera una lista de comandos
	/// </summary>
	public List<Commands.AbstractSequenceCommand> Build() => Commands;

	/// <summary>
	///		Comandos de la secuencia
	/// </summary>
	public List<Commands.AbstractSequenceCommand> Commands { get; } = [];
}
