using System;

using org.ogre;

namespace PacmanOgre.Input
{
    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardEvent KeyboardEvent { get; set; }
    }

    public class InputManager : InputListener, IDisposable
    {
        public event EventHandler<KeyboardEventArgs> OnKeyPressed;
        public event EventHandler<KeyboardEventArgs> OnKeyReleased;

        public InputManager(IContext ctx)
        {

        }

        public override bool keyPressed(KeyboardEvent evt)
        {
            OnKeyPressed?.Invoke(this, new KeyboardEventArgs() { KeyboardEvent = evt });
            return false; // propgate further;
        }

        public override bool keyReleased(KeyboardEvent evt)
        {
            OnKeyReleased?.Invoke(this, new KeyboardEventArgs() { KeyboardEvent = evt });
            return false; // propgate further;
        }

        public override bool mouseMoved(MouseMotionEvent evt)
        {
            return false; // propgate further;
        }

        public override bool mousePressed(MouseButtonEvent evt)
        {
            return false; // propgate further;
        }

        public override bool mouseReleased(MouseButtonEvent evt)
        {
            return false; // propgate further;
        }

        public override bool mouseWheelRolled(MouseWheelEvent evt)
        {
            return false; // propgate further;
        }

        public override bool touchMoved(TouchFingerEvent evt)
        {
            return false; // propgate further;
        }

        public override bool touchPressed(TouchFingerEvent evt)
        {
            return false; // propgate further;
        }

        public override bool touchReleased(TouchFingerEvent evt)
        {
            return false; // propgate further;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual new void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    base.Dispose();
                }
                disposedValue = true;
            }
        }

        public new void Dispose() => Dispose(true);
        #endregion

    }
}
