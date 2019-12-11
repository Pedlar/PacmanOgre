using System;

using org.ogre;

namespace PacmanOgre.Input
{

    internal class InputManager : InputListener, IDisposable
    {

        public InputManager(IContext ctx)
        {

        }

        public override bool keyPressed(KeyboardEvent evt)
        {
            return false; // propgate further;
        }

        public override bool keyReleased(KeyboardEvent evt)
        {
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
