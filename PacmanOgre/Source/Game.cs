using System;
using System.Collections.Generic;
using System.Linq;

using SharpEngine;
using org.ogre;
using PacmanOgre.Input;
using PacmanOgre.Scene;

public class KeyListener : InputListener
{
    ApplicationContextBase ctx;

    public KeyListener(ApplicationContextBase ctx)
    {
        this.ctx = ctx;
    }

    public override bool keyPressed(KeyboardEvent evt)
    {
        if (evt.keysym.sym == 27)
            ctx.getRoot().queueEndRendering();
        return true;
    }
}

namespace PacmanOgre
{
    public partial class Game : FrameListener, IContext, IDisposable
    {
        private readonly ApplicationContextBase _applicationContextBase;
        private readonly List<IDisposable> _disposables;
        private readonly InputManager _inputManager;
        private readonly ITickManager _tickManager;
        private readonly ISceneManager _sceneManager;

        private KeyListener _keyListener;

        public Game(ApplicationContextBase applicationContextBase)
        {
            _applicationContextBase = applicationContextBase;
            _disposables = new List<IDisposable>();
            _keyListener = new KeyListener(applicationContextBase);
            _inputManager = new InputManager(this);
            _tickManager = new TickManager();
            _sceneManager = new Scene.SceneManager(this);

            _disposables
                .Append(_inputManager)
                .Append(_keyListener)
                .Append(_tickManager)
                .Append(_sceneManager);
        }

        public IScene CurrentScene { get { return _sceneManager.Scenes[_sceneManager.CurrentScene]; } }
        public bool IsEditor { get; set; }

        public Root GetRoot() => _applicationContextBase.getRoot();
        public RenderWindow GetRenderWindow() => _applicationContextBase?.getRenderWindow();
        public ISceneManager GetSceneManager() => _sceneManager;
        public ITickManager GetITickManager() => _tickManager;
        public InputManager GetInputManager() => _inputManager;

        public void Setup()
        {
            _applicationContextBase.addInputListener(_keyListener);
            _applicationContextBase.addInputListener(_inputManager);

            _tickManager.Setup();
            _sceneManager.Setup();

            _sceneManager.DisplayScene(MainScene.MainSceneId);
        }
        
        public override bool frameStarted(FrameEvent evt)
        {
            _tickManager.sendFrameStart(new TickEventArgs { TimeDelta = evt.timeSinceLastFrame } );
            return true;
        }

        public override bool frameRenderingQueued(FrameEvent evt)
        {
            _tickManager.sendFrameQueued(new TickEventArgs { TimeDelta = evt.timeSinceLastFrame });
            _sceneManager.Update(evt.timeSinceLastFrame);
            return true;
        }

        public override bool frameEnded(FrameEvent evt)
        {
            _tickManager.sendFrameEnd(new TickEventArgs { TimeDelta = evt.timeSinceLastFrame });
            return true;
        }
        
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual new void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _disposables.ForEach(disposable => disposable.Dispose());
                    base.Dispose();
                }
                disposedValue = true;
            }
        }

        public new void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
