using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

using org.ogre;
using static SharpEngine.Editor.View.EditorViewportView;

namespace SharpEngine.Editor.Widget
{
    public partial class OgreImage : D3DImage,
                                     ISupportInitialize
    {
        public event EventHandler<WindowEventArgs> WindowReadyEvent;

        private delegate void MethodInvoker();

        private Root _root;
        private TexturePtr _texture;
        private RenderWindow _renderWindow;
        private Camera _camera;
        private Viewport _viewport;
        private RenderTarget _renTarget;
        private int _reloadRenderTargetTime;
        private bool _imageSourceValid;
        private Thread _currentThread;
        private bool _eventAttatched;

        public OgreImage()
        {

        }

        #region IDisposable Members

        public void Dispose()
        {
            IsFrontBufferAvailableChanged -= _isFrontBufferAvailableChanged;

            DetachRenderTarget(true, true);

            if (_currentThread != null)
            {
                _currentThread.Abort();
            }

            if (_root != null)
            {
                DisposeRenderTarget();
                CompositorManager.getSingleton().removeAll();

                _root.Dispose();
                _root = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region ISupportInitialize Members

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            if (AutoInitialise)
            {
                InitOgreAsync();
            }
        }

        #endregion

        protected bool _InitOgre()
        {
            lock (this)
            {
                IntPtr hWnd = IntPtr.Zero;

                foreach (PresentationSource source in PresentationSource.CurrentSources)
                {
                    var hwndSource = (source as HwndSource);
                    if (hwndSource != null)
                    {
                        hWnd = hwndSource.Handle;
                        break;
                    }
                }

                if (hWnd == IntPtr.Zero) return false;

                CallResourceItemLoaded(new ResourceLoadEventArgs("Engine", 0));

                WindowReadyEvent?.Invoke(this, new WindowEventArgs { WinPtr = hWnd });

                _root = GameWPF.Singleton.getRoot();

                if(_root == null)
                {
                    throw new Exception("Ogre not Initialised properly, need to register WindowReadyEvent and Initialize");
                }

                _renderWindow = GameWPF.Singleton.getRenderWindow();

                this.Dispatcher.Invoke(
                    (MethodInvoker)delegate
                    {
                        IsFrontBufferAvailableChanged += _isFrontBufferAvailableChanged;

                        Initialised?.Invoke(this, new RoutedEventArgs());

                        GameWPF.Singleton.Game.IsEditor = true;
                        GameWPF.Singleton.SetupGame();

                        var _sceneManager = GameWPF.Singleton.Game.CurrentScene.GetOgreSceneManager();

                        _camera = _sceneManager.createCamera("EditorCamera");
                        //_camera.setAutoAspectRatio(true);
                        _camera.setNearClipDistance(5);
                        SceneNode camnode = _sceneManager.getRootSceneNode().createChildSceneNode(/* TODO: Name Component? */);
                        camnode.setPosition(0f, 50, 10f);
                        camnode.yaw(new Radian(0f));
                        camnode.pitch(new Radian(-1.2f));
                        camnode.attachObject(_camera);


                        ReInitRenderTarget();
                        AttachRenderTarget(true);

                        OnFrameRateChanged(this.FrameRate);

                        _currentThread = null;
                    });

                return true;
            }
        }


        public bool InitOgre()
        {
            return _InitOgre();
        }

        public Thread InitOgreAsync(ThreadPriority priority, RoutedEventHandler completeHandler)
        {
            if (completeHandler != null)
                Initialised += completeHandler;

            _currentThread = new Thread(() => _InitOgre())
            {
                Priority = priority
            };
            _currentThread.Start();

            return _currentThread;
        }

        public void InitOgreAsync()
        {
            InitOgreAsync(ThreadPriority.Normal, null);
        }

        public event RoutedEventHandler Initialised;
        public event EventHandler PreRender;
        public event EventHandler PostRender;

        protected void RenderFrame()
        {
            if ((_camera != null) && (_viewport == null))
            {
                _viewport = _renTarget.addViewport(_camera);
                _viewport.setBackgroundColour(new ColourValue(0.0f, 0.0f, 0.0f, 0.0f));
            }

            PreRender?.Invoke(this, EventArgs.Empty);

            _root.renderOneFrame();

            //GameWPF.Singleton.getRenderWindow().update();
            _renTarget.update();

            PostRender?.Invoke(this, EventArgs.Empty);
        }

        protected void DisposeRenderTarget()
        {
            if (_renTarget != null)
            {
                CompositorManager.getSingleton().removeCompositorChain(_viewport);
                _renTarget.removeAllListeners();
                _renTarget.removeAllViewports();
                _root.getRenderSystem().destroyRenderTarget(_renTarget.getName());
                _renTarget = null;
                _viewport = null;
            }

            if (_texture != null)
            {
                TextureManager.getSingleton().remove(_texture.getHandle());
                _texture.Dispose();
                _texture = null;
            }
        }

        protected void ReInitRenderTarget()
        {

            DetachRenderTarget(true, false);
            DisposeRenderTarget();

            var texManager = TextureManager.getSingleton();
            var defaultResourceGroup = ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME;
            _texture = texManager.createManual(
                "OgreImageSource RenderTarget",
                defaultResourceGroup,
                TextureType.TEX_TYPE_2D,
                (uint)ViewportSize.Width, (uint)ViewportSize.Height,
                0, org.ogre.PixelFormat.PF_A8R8G8B8,
                (int)0x20);

            _renTarget = _texture.getBuffer().getRenderTarget();
            _renTarget.setAutoUpdated(false);

            _reloadRenderTargetTime = 0;
        }

        public Root Root
        {
            get { return _root; }
        }

        public Camera Camera
        {
            get { return _camera; }
        }

        protected virtual void AttachRenderTarget(bool attachEvent)
        {
            if (!_imageSourceValid)
            {
                Lock();
                try
                {
                    IntPtr surface = (IntPtr)_renTarget.getCustomAttribute("DDBACKBUFFER");
                    SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface);

                    _imageSourceValid = true;
                }
                finally
                {
                    Unlock();
                }
            }

            if (attachEvent)
                UpdateEvents(true);
        }

        protected virtual void DetachRenderTarget(bool detatchSurface, bool detatchEvent)
        {
            if (detatchSurface && _imageSourceValid)
            {
                Lock();
                SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
                Unlock();

                _imageSourceValid = false;
            }

            if (detatchEvent)
                UpdateEvents(false);
        }

        protected virtual void UpdateEvents(bool attach)
        {
            _eventAttatched = attach;
            if (attach)
            {
                CompositionTarget.Rendering += _rendering;
            }
            else
            {
                CompositionTarget.Rendering -= _rendering;
            }
        }

        private void _isFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsFrontBufferAvailable)
                AttachRenderTarget(true); // might not succeed
            else
                // need to keep old surface attached because it's the only way to get the front buffer active event.
                DetachRenderTarget(false, true);
        }

        private void _rendering(object sender, EventArgs e)
        {
            if (_root == null) return;

            if (IsFrontBufferAvailable)
            {
                long durationTicks = ResizeRenderTargetDelay.TimeSpan.Ticks;

                // if the new next ReInitRenderTarget() interval is up
                if (((_reloadRenderTargetTime < 0) || (durationTicks <= 0))
                    // negative time will be reloaded immediatly
                    ||
                    ((_reloadRenderTargetTime > 0) &&
                     (Environment.TickCount >= (_reloadRenderTargetTime + durationTicks))))
                {
                    ReInitRenderTarget();
                }

                if (!_imageSourceValid)
                    AttachRenderTarget(false);

                Lock();
                RenderFrame();
                AddDirtyRect(new Int32Rect(0, 0, PixelWidth, PixelHeight));
                Unlock();
            }
        }

        private void OnFrameRateChanged(int? newFrameRate)
        {
            bool wasAttached = _eventAttatched;
            UpdateEvents(false);

            //if (newFrameRate == null)
            //{
            //    if (_timer != null)
            //    {
            //        _timer.Tick -= _rendering;
            //        _timer.Stop();
            //        _timer = null;
            //    }
            //}
            //else
            //{
            //    if (_timer == null)
            //        _timer = new DispatcherTimer();

            //    _timer.Interval = new TimeSpan(1000 / newFrameRate.Value);
            //    _timer.Start();
            //}

            if (wasAttached)
                UpdateEvents(true);
        }
    }
}
