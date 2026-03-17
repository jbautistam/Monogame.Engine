using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

/// <summary>
///     Comando para presentar una pose dramática con estilo
/// </summary>
public class DramaticPoseCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Tipo de pose
    /// </summary>
    public enum PoseStyle 
    {
        /// <summary>Heroica: aplica un brillo sutil</summary>
        Heroic, 
        /// <summary>Villano: oscurece el personaje</summary>
        Villainous, 
        /// <summary>Comedia</summary>
        Comedic, 
        /// <summary>Trágico</summary>
        Tragic, 
        /// <summary>Misterioso: fluctúa entre valores de transparencia</summary>
        Mysterious
    }
    // Variables privadas
    private Vector2 _start, _target, _startScale;
    private float _startRotation;
    private bool _initialized;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        EasingFunctionsHelper.EasingType easing = GetStyleEasing();

            // Inicializa los datos si es necesario
            if (!_initialized)
            {
                _start = actor.Transform.Bounds.Location;
                _target = _start + ToWorld(Offset);
                _startScale = actor.Renderer.Scale;
                _startRotation = actor.Transform.Rotation;
                _initialized = true;
            }
            // Interpola los datos
            actor.Transform.Bounds.Location = EasingFunctionsHelper.Interpolate(_start, _target, Progress, easing);
            actor.Renderer.Scale = EasingFunctionsHelper.Interpolate(_startScale, Scale, Progress, easing);
            actor.Transform.Rotation = EasingFunctionsHelper.Interpolate(_startRotation, Rotation, Progress, easing);
            // Añade los efectos adicionales
            ApplyStyleEffects(actor);

        // Obtiene la función de interpolación
        EasingFunctionsHelper.EasingType GetStyleEasing()
        {
            return Pose switch
                    {
                        PoseStyle.Heroic => EasingFunctionsHelper.EasingType.BackOut,
                        PoseStyle.Villainous => EasingFunctionsHelper.EasingType.CubicIn,
                        PoseStyle.Comedic => EasingFunctionsHelper.EasingType.ElasticInOut,
                        PoseStyle.Tragic => EasingFunctionsHelper.EasingType.QuadInOut,
                        PoseStyle.Mysterious => EasingFunctionsHelper.EasingType.SmoothStep,
                        _ => EasingFunctionsHelper.EasingType.Linear
                    };
        }

        // Aplica los efectos adicionales de la pose seleccionada
        void ApplyStyleEffects(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
        {
            switch (Pose)
            {
                case PoseStyle.Heroic: // Brillo sutil
                        actor.Renderer.Color = Color.Lerp(Color.White, Color.LightYellow, Progress * 0.3f);
                    break;
                case PoseStyle.Villainous: // Oscurecimiento
                        actor.Renderer.Color = Color.Lerp(Color.White, Color.DarkGray, Progress * 0.4f);
                    break;
                case PoseStyle.Tragic: // Desaturación
                    float gray = 0.8f - Progress * 0.3f;

                        actor.Renderer.Color = new Color(gray, gray, gray);
                    break;
                case PoseStyle.Mysterious: // Transparencia fluctuante
                        actor.Renderer.Opacity = 0.8f + MathF.Sin(Progress * MathF.PI * 4) * 0.2f;
                    break;
            }
        }
    }    

    /// <summary>
    ///     Pose
    /// </summary>
    public required PoseStyle Pose { get; init; }

    /// <summary>
    ///     Desplazamineto sobre la posición
    /// </summary>
    public required Vector2 Offset { get; init; }

    /// <summary>
    ///     Escala final
    /// </summary>
    public required Vector2 Scale { get; init; }

    /// <summary>
    ///     Rotación final
    /// </summary>
    public required float Rotation { get; init; }
}
