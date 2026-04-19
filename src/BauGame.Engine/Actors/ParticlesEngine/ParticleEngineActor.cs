using Microsoft.Xna.Framework;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Actors.ParticlesEngine.Emitters;
using Bau.BauEngine.Scenes.Layers;
using Bau.BauEngine.Scenes.Rendering;

namespace Bau.BauEngine.Actors.ParticlesEngine;

/// <summary>
///		Motor de partículas
/// </summary>
public class ParticleEngineActor : AbstractActorDrawable, Entities.Common.Collections.ISecureListItem
{
	// Eventos públicos
	public event EventHandler? EndParticleEngine;

	public ParticleEngineActor(AbstractLayer layer, Vector2 position, int? zOrder) : base(layer, zOrder)
	{
		StartPosition = position;
	}

	/// <summary>
	///		Arranca el elemento cuando se añade a la lista
	/// </summary>
	protected override void StartActor()
	{
		// Desactiva el renderer del actor
		Renderer.Enabled = false;
		// Cambia la posición de inicio del actor
		Transform.Bounds.Location = StartPosition;
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
		int disabledNumber = 0, enabledParticles = 0;

			// Actualiza todos los emisores (generan las partículas)
			foreach (ParticleEmitter emitter in Emitters)
				if (emitter.Enabled)
					emitter.Update(gameContext);
				else
					disabledNumber++;
			// Modifica las partículas activas
			foreach (ParticleEmitter emitter in Emitters)
				foreach (Particles.ParticleModel particle in emitter.Pool.Particles)
					if (particle.Enabled)
					{
						// Ejecuta los modificadores
						foreach (Modifiers.AbstractParticleModifier modifier in emitter.Modifiers)
							modifier.Update(particle, gameContext.DeltaTime);
						// Cambia el tiempo de vida de la partícula e incrementa el número de partículas activas
						particle.Update(gameContext);
						enabledParticles++;
					}
			// Si todos los emisores están inactivos y no quedan partículas, elimina este actor de la capa
			if (disabledNumber == Emitters.Count && enabledParticles == 0)
				Layer.Actors.MarkToDestroy(this, TimeSpan.FromSeconds(2));
	}

	/// <summary>
	///		Indica que se debe dibujar aunque su posición esté fuera de cámara: pueden tener los emisores fuera del punto de vista 
	/// de la cámara pero que haya partículas que se tienen que dibujar
	/// </summary>
	protected override bool MustDrawOutOfCamera() => true;

	/// <summary>
	///		Dibuja el actor: dibuja todas las partículas asociadas a los emisores
	/// </summary>
    protected override void DrawSelf(RenderingManager renderingManager, GameContext gameContext)
    {
		foreach (ParticleEmitter emitter in Emitters)
			foreach (Particles.ParticleModel particle in emitter.Pool.Particles)
				if (particle.Enabled)
					DrawParticle(renderingManager, particle);
    }

	/// <summary>
	///		Dibuja una partícula
	/// </summary>
	private void DrawParticle(RenderingManager renderingManager, Particles.ParticleModel particle)
	{
		if (particle.Opacity > 0.1f)
		{
			if (particle.Sprite is not null)
				renderingManager.SpriteRenderer.Draw(particle.Sprite, particle.Position, Vector2.Zero, new Vector2(particle.Scale, particle.Scale), 
													 particle.Rotation, particle.Color * particle.Opacity);
			else
			{
				int thickness = Math.Max((int) particle.Scale, 1);

					// Dibuja un rectángulo representando la partícula
					renderingManager.FiguresRenderer.DrawRectangle(new Rectangle((int) particle.Position.X, (int) particle.Position.Y, thickness, thickness),
																   particle.Color * particle.Opacity);
			}													 
		}
	}

	/// <summary>
	///		Finaliza el elemento cuando se borra de la lista
	/// </summary>
	protected override void EndActor(GameContext gameContext)
	{
		EndParticleEngine?.Invoke(this, EventArgs.Empty);
	}

	/// <summary>
	///		Posición de inicio
	/// </summary>
	public Vector2 StartPosition { get; }

	/// <summary>
	///		Emisores
	/// </summary>
	public List<ParticleEmitter> Emitters { get; } = [];
}