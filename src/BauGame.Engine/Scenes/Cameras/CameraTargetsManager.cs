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
        Vector2? target = null;

            // Busca el objetivo
            if (_targets.Count == 1)
                target = _targets[0].Transform.Bounds.TopLeft;
            else if (_targets.Count > 1)
            {
                float minX = float.MaxValue, minY = float.MaxValue;
                float maxX = float.MinValue, maxY = float.MinValue;

                // Encuentra el rectángulo que contiene a todos los objetivos
                foreach (Actors.AbstractActor actor in _targets)
                    if (actor.Enabled)
                    {
                        minX = Math.Min(minX, actor.Transform.Bounds.X);
                        minY = Math.Min(minY, actor.Transform.Bounds.Y);
                        maxX = Math.Max(maxX, actor.Transform.Bounds.Right);
                        maxY = Math.Max(maxY, actor.Transform.Bounds.Bottom);
                    }
                // El objetivo es el centro del grupo
                target = new Vector2(0.5f * (minX + maxX), 0.5f * (minY + maxY));
                // Si no todos los objetivos caben en pantalla no se podrán mostrar todos, se podría variar el zoom
                //float requiredWidth = maxX - minX;
                //float requiredHeight = maxY - minY;
            }
            // Devuelve el objetivo donde debe mirar la cámara
            return target;
    }
}
