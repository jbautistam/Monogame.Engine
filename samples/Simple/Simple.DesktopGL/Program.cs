using Simple.Core;

internal class Program
{
    /// <summary>
    ///     Crea una instancia del juego y la ejecuta
    /// </summary>
    private static void Main(string[] args)
    {
        using (SimpleGame game = new())
        {
            game.Run();
        };
    }
}