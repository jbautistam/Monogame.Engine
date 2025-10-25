using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public interface ISteeringBehavior
{
    Vector2 Calculate(Agent agent);
}
