using org.ogre;
using PacmanOgre;

public class Program
{
    static void Main()
    {
        var gameSDL = new GameSDL();

        gameSDL.initApp();
        gameSDL.getRoot().startRendering();
        gameSDL.closeApp();

        gameSDL.Dispose();
    }
}