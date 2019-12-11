using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanOgre.Scene
{
    public class SceneManager : ISceneManager
    {
        public IDictionary<SceneId, IScene> Scenes { get; private set; } = new Dictionary<SceneId, IScene>();

        public SceneId CurrentScene { get; private set; }

        public event EventHandler<EventArgs> OnSceneLoaded;
        public event EventHandler<EventArgs> OnSceneUnloaded;

        public SceneManager(IContext context)
        {

        }

        public void LoadScene(SceneId sceneId)
        {
            if(CurrentScene == sceneId)
            {
                return;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
