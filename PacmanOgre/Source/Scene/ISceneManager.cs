using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanOgre.Scene
{
    public interface ISceneManager : IDisposable
    {
        IDictionary<SceneId, IScene> Scenes { get; }

        SceneId CurrentScene { get; }

        event EventHandler<EventArgs> OnSceneLoaded;
        event EventHandler<EventArgs> OnSceneUnloaded;

        void AddScene<T>() where T : IScene;
        void DisplayScene(SceneId sceneId);
        void Update(float timeDelta);
    }
}
