using org.ogre;
using PacmanOgre.Input;
using PacmanOgre.Scene;

namespace PacmanOgre
{
    public interface IContext
    {
        bool IsEditor { get; set; }

        IScene CurrentScene { get; }

        Root GetRoot();
        RenderWindow GetRenderWindow();

        ISceneManager GetSceneManager();

        ITickManager GetITickManager();
        InputManager GetInputManager();
    }
}
