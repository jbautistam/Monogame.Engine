using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.UI;

/// <summary>
///     Componente base para los contenedores en el interface de usuario
/// </summary>
public abstract class UiContainerComponent : UiComponent
{
    /// <summary>
    ///     Actualiza los datos absolutos forzando el recálulo
    /// </summary>
    protected override void InvalidateSelf()
    {
        foreach (UiComponent child in Children)
            child.Invalidate();
    }

    /// <summary>
    ///     Actualiza el componente
    /// </summary>
    protected override void UpdateSelf(GameTime gameTime)
    {
        foreach (UiComponent child in Children)
            child.Update(gameTime);
    }

    /// <summary>
    ///     Dibuja los datos específicos del componente
    /// </summary>
    protected override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (UiComponent child in Children)
            child.Draw(spriteBatch, gameTime);
    }

    /// <summary>
    ///     Componentes hijo
    /// </summary>
    public List<UiComponent> Children { get; } = [];
}
