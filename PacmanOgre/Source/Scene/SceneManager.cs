﻿using PacmanOgre.Scene.Loader;
using SharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PacmanOgre.Scene
{
    using static PacmanOgre.Utilities.EnumerableExtensions;

    public class SceneManager : ISceneManager
    {
        public IDictionary<SceneId, IScene> Scenes { get; private set; } = new Dictionary<SceneId, IScene>();

        public SceneId CurrentScene { get; private set; } = new SceneId() { SceneName = "InvalidScene" };

        public event EventHandler<EventArgs> OnSceneLoaded;
        public event EventHandler<EventArgs> OnSceneUnloaded;

        private IContext _context;
        private ScenesLoader _scenesLoader;

        public SceneManager(IContext context)
        {
            _context = context;
            _scenesLoader = new ScenesLoader(this);
        }

        public void Setup()
        {
            _scenesLoader.LoadScenes();
        }

        public void AddScene<T>() where T : IScene
        {
            AddScene(typeof(T));
        }

        public void AddScene(Type sceneType)
        {
            ConstructorInfo constructorInfo = sceneType.GetConstructor(new Type[] { typeof(IContext) });
            IScene scene;
            object[] constructorParams = new object[] { };

            if(constructorInfo != null)
            {
                constructorParams = new object[] { _context };
            }
            else if((constructorInfo = sceneType.GetConstructor(new Type[] { typeof(IContext), typeof(EntityManager) })) != null)
            {
                EntityManager entityManager = new EntityManager();
                constructorParams = new object[] { _context, entityManager };
            }
            else if ((constructorInfo = sceneType.GetConstructor(new Type[] { typeof(IContext), typeof(EntityManager), typeof(IMovementManager) })) != null)
            {
                EntityManager entityManager = new EntityManager();
                IMovementManager movementManager = new MovementManager(entityManager);
                constructorParams = new object[] { _context, entityManager,  movementManager };
            }
            else
            {
                constructorInfo = sceneType.GetConstructors()[0];
            }

            scene = constructorInfo.Invoke(constructorParams) as IScene;
            scene.Setup();

            Scenes.Add(scene.SceneId, scene);
        }

        public bool HasIScene(Type type)
        {
            foreach(var scene in Scenes.Values)
            {
                if (scene.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }

        public void DisplayScene(SceneId sceneId)
        {
            if(CurrentScene == sceneId)
            {
                return;
            }
            try
            {
                Scenes[CurrentScene].UnloadScene();
            }
            catch(KeyNotFoundException knfe)
            {
                // Don't do anything, Don't care.
            }

            CurrentScene = sceneId;
            Scenes[CurrentScene].LoadScene();
        }

        public void Update(float timeDelta)
        {
            foreach (var scene in Scenes.Values)
            {
                scene.Update(timeDelta, scene.SceneId == CurrentScene);
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
