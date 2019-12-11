using PacmanOgre;

public class Program
{
    static void Main()
    {
        var game = new PacmanGame();
        game.initApp();
        game.getRoot().startRendering();
        game.closeApp();

        game.Dispose();
    }
}