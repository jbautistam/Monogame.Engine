using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.CharactersEngine.Sequences;

/// <summary>
///     Manager de personajes
/// </summary>
public class CharacterLayer
{
    // Variables privadas
    private List<Actor> _actors = [];

    public CharacterLayer()
    {
        CinematicManager = new CinematicManager(this);
    }
    
    /// <summary>
    ///     Actualiza el manager
    /// </summary>
    public void Update(float deltaTime)
    {
        CinematicManager.Update(deltaTime);
    }
    
    /// <summary>
    ///     Obtiene un actor
    /// </summary>
    public Actor? GetActor(string id) => _actors.FirstOrDefault(item => item.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Ordena por capa y Zorder
        _actors.Sort((first, second) => first.DrawOrder.CompareTo(second.DrawOrder));
        //var sorted = _actors.Values
        //    .Where(a => a.IsVisible)
        //    .OrderBy(a => a.Transform.ZOrder);
            
        //foreach (var actor in sorted)
        //    actor.Draw(spriteBatch);
    }

    /// <summary>
    ///     Manager para cinemáticas
    /// </summary>
    public CinematicManager CinematicManager { get; }
}
