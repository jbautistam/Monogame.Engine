using EngineSample.Core;

internal class Program
{
	/// <summary>
	///		Preparación del juego
	/// </summary>
	private static void Main(string[] args)
	{
		using EngineSampleGame game = new();
		game.Run();
	}
}