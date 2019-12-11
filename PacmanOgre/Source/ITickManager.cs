using System;

namespace PacmanOgre
{
    public class TickEventArgs : EventArgs
    {
        public float TimeDelta { get; set; }
    }

    public interface ITickManager : IDisposable
    {
        event EventHandler<TickEventArgs> OnFrameStart;
        event EventHandler<TickEventArgs> OnFrameQueued;
        event EventHandler<TickEventArgs> OnFrameEnd;

        void Setup();
        void sendFrameStart(TickEventArgs tickEventArgs);
        void sendFrameQueued(TickEventArgs tickEventArgs);
        void sendFrameEnd(TickEventArgs tickEventArgs);
    }
}
