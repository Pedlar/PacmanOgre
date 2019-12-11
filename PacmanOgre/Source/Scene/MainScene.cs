using PacmanOgre.Components;
using SharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OgreSceneManager = org.ogre.SceneManager;

namespace PacmanOgre.Scene
{
    class MainScene : IScene
    {
        public static readonly SceneId MainSceneId = new SceneId { SceneName = "MainScene" };
        private IContext _context;
        private EntityManager _entityManager;
        private OgreSceneManager _sceneManager;

        public MainScene(IContext context, EntityManager entityManager)
        {
            _context = context;
            _entityManager = entityManager;
            _sceneManager = _context.GetRenderer().GetSceneManager();
        }

        public SceneId SceneId { get; set; } = MainSceneId;

        public void Setup()
        {
            
            var entity = _entityManager.CreateEntity();
            entity.AddComponent<TransformComponent>(_context, entity, new org.ogre.Vector3(0, 0, 0));
            entity.AddComponent<MeshComponent>(_context, entity, "Sinbad.mesh");

            entity.ForEachComponent(component => component.OnEntityCreated());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _entityManager.Clear();
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
