using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Builders;

/// <summary>
///		Generador de una secuencia
/// </summary>
public class SequenceBuilder
{
	// Variables privadas
	private string _actor;
	private float _start;

	public SequenceBuilder(string actor)
	{
		_actor = actor;
	}

	/// <summary>
	///		Cambia el actor
	/// </summary>
	public SequenceBuilder WithActor(string actor)
	{
		// Cambia el actor e iicia el tiempo
		_start = 0;
		_actor = actor;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Cambia el momento de inicio
	/// </summary>
	public SequenceBuilder WithStart(float start)
	{
		// Cambia el momento de inicio
		_start = start;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para mover el actor directamente a una posición
	/// </summary>
	public SequenceBuilder WithReset(float duration, Vector2 position, Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects, 
									 float opacity, float? start = null)
	{
		// Añade el comando
		Commands.Add(new ResetCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Position = position,
									Opacity = opacity,
									SpriteEffects = spriteEffects
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para mover el actor directamente a una posición
	/// </summary>
	public SequenceBuilder WithMove(float duration, TranslateCommand.MovementMode mode, Vector2 target, float? start = null)
	{
		// Añade el comando
		Commands.Add(new TranslateCommand(_actor, GetAndUpdateStart(duration, start), duration)
												{
													Mode = mode,
													Target = target
												}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para hacer un fade in / fade out con la opacidad
	/// </summary>
	public SequenceBuilder WithFade(float duration, float opacity, float? start = null)
	{
		// Añade el comando
		Commands.Add(new FadeCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Opacity = opacity
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para hacer un flash con el color
	/// </summary>
	public SequenceBuilder WithFlash(float duration, float intensity, float? start = null)
	{
		// Añade el comando
		Commands.Add(new FlashCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									PeakIntensity = intensity
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para hacer un movimiento de salto
	/// </summary>
	public SequenceBuilder WithJump(float duration, float height, float offsetX, float? start = null)
	{
		// Añade el comando
		Commands.Add(new JumpCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Height = height,
									OffsetX = offsetX
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para hacer un efecto de Nod o asentamieno
	/// </summary>
	public SequenceBuilder WithNod(float duration, float angle, int nods, float? start = null)
	{
		// Añade el comando
		Commands.Add(new NodCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Angle = angle,
									Nods = nods
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para hacer un efecto de pulso
	/// </summary>
	public SequenceBuilder WithPulse(float duration, float amplitude, int pulses, float? start = null)
	{
		// Añade el comando
		Commands.Add(new PulseCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Amplitude = amplitude,
									Pulses = pulses
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para cambiar la escala
	/// </summary>
	public SequenceBuilder WithScale(float duration, Vector2 scale, float? start = null)
	{
		// Añade el comando
		Commands.Add(new ScaleCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Scale = scale
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para cambiar la escala al mundo
	/// </summary>
	public SequenceBuilder WithScaleToWorld(float duration, Vector2 scale, bool uniform, float? start = null)
	{
		// Añade el comando
		Commands.Add(new ScaleToWorldCommand(_actor, GetAndUpdateStart(duration, start), duration)
									{
										Scale = scale,
										Uniform = uniform
									}
						);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para hacer una sacudida
	/// </summary>
	public SequenceBuilder WithShake(float duration, float intensity, int oscillations, bool horizontal, bool vertical, float? start = null)
	{
		// Añade el comando
		Commands.Add(new ShakeCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Intensity = intensity,
									Oscillations = oscillations,
									Horizontal = horizontal,
									Vertical = vertical
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para cambiar la textura
	/// </summary>
	public SequenceBuilder WithTexture(float duration, string texture, string? region, float? start = null)
	{
		// Añade el comando
		Commands.Add(new TextureCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Texture = texture,
									Region = region
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para cambiar la textura
	/// </summary>
	public SequenceBuilder WithExpression(float duration, string expression, float? start = null)
	{
		// Añade el comando
		Commands.Add(new ExpressionCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Expression = expression
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para cambiar el color
	/// </summary>
	public SequenceBuilder WithTint(float duration, Color color, float? start = null)
	{
		// Añade el comando
		Commands.Add(new TintCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Target = color
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para cambiar el ZOrder
	/// </summary>
	public SequenceBuilder WithZOrder(float duration, int zOrder, float? start = null)
	{
		// Añade el comando
		Commands.Add(new ZOrderCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									ZOrder = zOrder
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para hacer un zoom sobre un punto del actor en coordenadas relativas
	/// </summary>
	public SequenceBuilder WithZoomOnPoint(float duration, Vector2 pointLocal, Vector2 zoom, float? start = null)
	{
		// Añade el comando
		Commands.Add(new ZoomOnPointCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									LocalPoint = pointLocal,
									Scale = zoom
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para una entrada cinemática
	/// </summary>
	public SequenceBuilder WithEntrance(float duration, CinematicEntranceCommand.EntranceType type, Vector2 to, 
										float impactPause, float anticipation, float? start = null)
	{
		// Añade el comando
		Commands.Add(new CinematicEntranceCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Type = type,
									To = to,
									ImpactPause = impactPause,
									Anticipation = anticipation
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un comando para cambio en la pose
	/// </summary>
	public SequenceBuilder WithPose(float duration, DramaticPoseCommand.PoseStyle pose, Vector2 offset, Vector2 scale, float rotation, float? start = null)
	{
		// Añade el comando
		Commands.Add(new DramaticPoseCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									Pose = pose,
									Offset = offset,
									Scale = scale,
									Rotation = rotation
								}
					);
		// Devuelve el generador
		return this;
	}


	/// <summary>
	///		Añade un comando para cambiar la orientación del sprite
	/// </summary>
	public SequenceBuilder WithSpriteEffects(float duration, Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects, float? start = null)
	{
		// Añade el comando
		Commands.Add(new SpriteEffectsCommand(_actor, GetAndUpdateStart(duration, start), duration)
								{
									SpriteEffects = spriteEffects
								}
					);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Obtiene el valor de inicio y acumula la duración
	/// </summary>
	private float GetAndUpdateStart(float duration, float? start)
	{
		float oldStart = _start;

			// Asgina el momento de inicio
			if (start is not null)
			{
				oldStart = start ?? 0;
				_start = start ?? 0;
			}
			// Normaliza la duración
			if (duration == 0)
				duration = 0.1f;
			// Incrementa el momento de inicio con la duración
			_start += duration + 0.1f;
			// Devuelve el momento de inicio que teníamos hasta ahora
			return oldStart;
	}

	/// <summary>
	///		Genera una lista de comandos
	/// </summary>
	public List<AbstractSequenceCommand> Build() => Commands;

	/// <summary>
	///		Comandos de la secuencia
	/// </summary>
	public List<AbstractSequenceCommand> Commands { get; } = [];
}
