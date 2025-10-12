using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Bau.Monogame.Engine.Domain.Tilemaps;

public class TileAnimationManager
{
    private List<AnimatedTile> _animations = new();

    public void Add(AnimatedTile animation)
    {
        if (!_animations.Contains(animation))
            _animations.Add(animation);
    }

    // En este diseño, las animaciones se actualizan "perezosamente" al renderizar
    // Si quieres actualizar todas cada frame, puedes iterar aquí
    public void Update(GameTime gameTime) { }
}
