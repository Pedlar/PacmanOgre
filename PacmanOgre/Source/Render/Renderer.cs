using System;

using org.ogre;

namespace PacmanOgre.Render
{
    internal class Renderer : IRenderer
    {
        public event EventHandler OnSetup;

        private ApplicationContext _appContext;
        private SceneManager _sceneManager;

        public Renderer(ApplicationContext applicationContext)
        {
            _appContext = applicationContext;
        }

        public void Dispose()
        {
            _appContext = null;
        }

        public SceneManager GetSceneManager() => _sceneManager;

        public void Setup()
        {
            var root = _appContext.getRoot();
            _sceneManager = root.createSceneManager();

            var shadergen = ShaderGenerator.getSingleton();
            shadergen.addSceneManager(_sceneManager); // must be done before we do anything with the scene

            _sceneManager.setAmbientLight(new ColourValue(.9f, .9f, .9f));

#if false
            var light = scnMgr.createLight("MainLight");
            var lightnode = scnMgr.getRootSceneNode().createChildSceneNode();
            lightnode.setPosition(0f, 10f, 15f);
            lightnode.attachObject(light); 
#endif

            float scale = 10f; // Your scale here.

            Matrix4 p = this.BuildScaledOrthoMatrix(_appContext.getRenderWindow().getWidth() / scale / -2.0f,
                                                    _appContext.getRenderWindow().getWidth() / scale / 2.0f,
                                                    _appContext.getRenderWindow().getHeight() / scale / -2.0f,
                                                    _appContext.getRenderWindow().getHeight() / scale / 2.0f, 0, 1000);


            var cam = _sceneManager.createCamera("myCam");
            cam.setAutoAspectRatio(true);
            cam.setNearClipDistance(5);
            cam.setProjectionType(ProjectionType.PT_ORTHOGRAPHIC);
            cam.setCustomProjectionMatrix(true, p);

            var camnode = _sceneManager.getRootSceneNode().createChildSceneNode();
            camnode.translate(new Vector3(0, 0, 15f), Node.TransformSpace.TS_LOCAL);
            camnode.yaw(new Radian(0f));
            camnode.pitch(new Radian(-1.5f));
            camnode.attachObject(cam);

            MeshPtr meshPtr = MeshManager.getSingleton().createPlane("background", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, new Plane(new Vector3(0f, 1f, 0f), 1f), _appContext.getRenderWindow().getWidth(), _appContext.getRenderWindow().getHeight(), 1, 1, true, 1, 1, 1, new Vector3(0f, 0f, 1f));
            var floorEnt = _sceneManager.createEntity("background", "background");
            var floorNode = _sceneManager.getRootSceneNode().createChildSceneNode();
            floorNode.attachObject(floorEnt);

            meshPtr.Dispose();

            var vp = _appContext.getRenderWindow().addViewport(cam);
            vp.setBackgroundColour(new ColourValue(.3f, .3f, .3f));
        }

        public void Update()
        {

        }

        public Matrix4 BuildScaledOrthoMatrix(float left, float right, float bottom, float top, float near, float far)
        {
            float invw = 1 / (right - left);
            float invh = 1 / (top - bottom);
            float invd = 1 / (far - near);

            var zerozero = 2 * invw;
            var zerothree = -(right + left) * invw;
            var oneone = 2 * invh;
            var onethree = -(top + bottom) * invh;
            var twotwo = -2 * invd;
            var twothree = -(far + near) * invd;
            var threethree = 1;

            return new Matrix4(zerozero, 0, 0, zerothree, 
                               0, oneone, 0, onethree, 
                               0, 0, twotwo, twothree, 
                               0, 0, 0, threethree);
        }

        protected virtual void OnSetupEvent()
        {
            OnSetup?.Invoke(this, EventArgs.Empty);
        }
    }
}
