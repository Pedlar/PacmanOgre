using System;
using System.Collections.Generic;
using System.Linq;

using SharpEngine;
using org.ogre;
using PacmanOgre.Input;
using PacmanOgre.Render;
using PacmanOgre.Scene;

public class KeyListener : InputListener
{
    ApplicationContext ctx;

    public KeyListener(ApplicationContext ctx)
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
    public class PacmanGame : ApplicationContext, IContext, IDisposable
    {
        private readonly List<IDisposable> _disposables;
        private readonly IRenderer _renderer;
        private readonly InputManager _inputManager;
        private readonly ITickManager _tickManager;
        private readonly ISceneManager _sceneManager;

        private KeyListener _keyListener;

        public PacmanGame()
        {
            _disposables = new List<IDisposable>();
            _renderer = new Renderer(this);
            _keyListener = new KeyListener(this);
            _inputManager = new InputManager(this);
            _tickManager = new TickManager(this);
            _sceneManager = new Scene.SceneManager(this);

            _disposables
                .Append(_inputManager)
                .Append(_keyListener)
                .Append(_tickManager)
                .Append(_sceneManager)
                .Append(_renderer);
        }

        public IRenderer GetRenderer() => _renderer;
        public RenderWindow GetRenderWindow() => getRenderWindow();
        public ITickManager GetITickManager() => _tickManager;
        public InputManager GetInputManager() => _inputManager;

        public override void setup()
        {
            base.setup();
            addInputListener(_keyListener);
            addInputListener(_inputManager);

            _renderer.Setup();

            _tickManager.Setup();

            _sceneManager.AddScene<MainScene>();

            _sceneManager.DisplayScene(MainScene.MainSceneId);
        }
        
        public override bool frameStarted(FrameEvent evt)
        {
            _tickManager.sendFrameStart(new TickEventArgs { TimeDelta = evt.timeSinceLastFrame } );
            _renderer.Update();
            return base.frameStarted(evt);
        }

        public override bool frameRenderingQueued(FrameEvent evt)
        {
            _tickManager.sendFrameQueued(new TickEventArgs { TimeDelta = evt.timeSinceLastFrame });
            _sceneManager.Update(evt.timeSinceLastFrame);
            return base.frameRenderingQueued(evt);
        }

        public override bool frameEnded(FrameEvent evt)
        {
            _tickManager.sendFrameEnd(new TickEventArgs { TimeDelta = evt.timeSinceLastFrame });
            return base.frameEnded(evt);
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
