using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Tools.Extensors;

/// <summary>
///		Funciones de cálculo de vectores
/// </summary>
public static class VectorMaths
{
	/// <summary>
	///		Limita los componentes de un vector
	/// </summary>
	public static Vector2 ClampComponents(this Vector2 vector, float maximum)
	{
		return new Vector2(MathHelper.Clamp(vector.X, -maximum, maximum), MathHelper.Clamp(vector.Y, -maximum, maximum));
	}

	/// <summary>
	///		Limita la magnitud del vector
	/// </summary>
	public static Vector2 ClampMagnitude(this Vector2 vector, float maxLength)
	{
		float length = vector.Length();

			// Normaliza el vector
			if (length > maxLength && length > 0f)
				return vector / length * maxLength;
			else
				return vector;
	}
}
