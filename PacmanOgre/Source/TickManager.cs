using System;

using org.ogre;

namespace PacmanOgre
{
    public class TickManager : ITickManager
    {

        public TickManager(ApplicationContext applicationContext)
        {

        }

        public event EventHandler<TickEventArgs> OnFrameStart;
        public event EventHandler<TickEventArgs> OnFrameQueued;
        public event EventHandler<TickEventArgs> OnFrameEnd;

        public void Setup()
        {
            
        }

        public void sendFrameStart(TickEventArgs tickEventArgs) 
            => OnFrameStartEvent(tickEventArgs);

        public void sendFrameQueued(TickEventArgs tickEventArgs) 
            => OnFrameQueuedEvent(tickEventArgs);

        public void sendFrameEnd(TickEventArgs tickEventArgs) 
            => OnFrameStartEnd(tickEventArgs);

        protected virtual void OnFrameStartEvent(TickEventArgs tickEventArgs)
            => OnFrameStart?.Invoke(this, tickEventArgs);

        protected virtual void OnFrameQueuedEvent(TickEventArgs tickEventArgs)
            => OnFrameQueued?.Invoke(this, tickEventArgs);

        protected virtual void OnFrameStartEnd(TickEventArgs tickEventArgs)
            => OnFrameEnd?.Invoke(this, tickEventArgs);

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
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
