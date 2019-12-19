using org.ogre;
using PacmanOgre.Input;

namespace PacmanOgre
{
    public interface IContext
    {
        IRenderer GetRenderer();
        RenderWindow GetRenderWindow();


        ITickManager GetITickManager();
        InputManager GetInputManager();
    }
}
