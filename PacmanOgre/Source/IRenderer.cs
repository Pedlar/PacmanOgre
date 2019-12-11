using System;
using org.ogre;

namespace PacmanOgre
{
    public interface IRenderer : IDisposable
    {
        event EventHandler OnSetup;

        void Setup();
        void Update();

        SceneManager GetSceneManager();
    }
}
