using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras;

/// <summary>
///		Manager para objetivos de una cámara
/// </summary>
public class CameraTargetsManager
{
    // Variables privadas
    private List<Actors.AbstractActor> _targets = [];

    /// <summary>
    ///     Añade un actor a la lista de objetivos
    /// </summary>
    public void Add(Actors.AbstractActor actor)
    {
        if (!_targets.Contains(actor))
            _targets.Add(actor);
    }

    /// <summary>
    ///     Elimina un actor de la lista de objetivos
    /// </summary>
    public void Remove(Actors.AbstractActor actor)
    {
        _targets.Remove(actor);
    }

    /// <summary>
    ///     Limpia la lista de objetivos
    /// </summary>
    public void Clear()
    {
        _targets.Clear();
    }

    /// <summary>
    ///     Obtiene el punto objetivo de la cámara
    /// </summary>
    public Vector2? LookAt()
    {
        Vector2 desiredTarget = new(0, 0);
        int count = 0;

            // Añade las coordenadas de los objetivos para obtener la posición donde debe colocarse la cámara
            foreach (Actors.AbstractActor actor in _targets)
                if (actor.Enabled)
                {
                    desiredTarget += actor.Transform.Bounds.TopLeft;
                    count++;
                }
            // Si hay algún objetivo, el punto deseado está en el medio de todos los objetivos
            if (count > 0)
                return desiredTarget /= count;
            else
                return null;
    }
}
