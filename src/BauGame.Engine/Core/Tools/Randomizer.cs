using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Tools;

/// <summary>
///		Funciones para generación de valores aleatorios
/// </summary>
public static class Randomizer
{
	/// <summary>
	///		Obtiene una dirección aleatoria
	/// </summary>
	public static Vector2 GetRandomDirection()
	{
        float angle = (float) (Random.NextDouble() * Math.PI * 2);

			// Crea un vector unitario con la dirección a partir del ángulo en radianes
			return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
	}

	/// <summary>
	///		Obtiene un valor float entre dos valores
	/// </summary>
	public static float GetRandom(float minimum, float maximum) => (float) (Random.NextDouble() * (maximum - minimum) + minimum);

	/// <summary>
	///		Obtiene un color aleatorio
	/// </summary>
	public static Color GetRandomColor() => new(Random.Next(256), Random.Next(256), Random.Next(256));

	/// <summary>
	///		Obtiene un color aleatorio
	/// </summary>
	public static Color GetRandomColor(int minimumRed, int maximumRed, int minimumGreen, int maximumGreen, 
									   int minimumBlue, int maximumBlue)
	{
		return new(Random.Next(minimumRed, maximumRed), 
				   Random.Next(minimumGreen, maximumGreen), 
				   Random.Next(minimumBlue, maximumBlue));
	}

	/// <summary>
	///		Objeto para generación de valores aleatorios
	/// </summary>
	public static Random Random { get; } = new();
}