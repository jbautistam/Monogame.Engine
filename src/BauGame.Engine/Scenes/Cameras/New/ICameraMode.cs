// ICameraMode.cs
using Microsoft.Xna.Framework;

public interface ICameraMode
{
    void Update(GameTime gameTime, Camera2D camera);
    void Reset();
}