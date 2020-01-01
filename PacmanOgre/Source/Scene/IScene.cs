using SharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanOgre.Scene
{
    public class SceneId
    {
        public string SceneName { get; set; }

        public static explicit operator string(SceneId sceneId) => sceneId.SceneName;
        
        public static bool operator ==(SceneId left, SceneId right) => left?.SceneName == right?.SceneName;
        public static bool operator !=(SceneId left, SceneId right) => !(left == right);

        public override bool Equals(object obj)
        {
            var id = obj as SceneId;
            return id != null &&
                   SceneName == id.SceneName;
        }

        public override int GetHashCode()
        {
            return 232260920 + EqualityComparer<string>.Default.GetHashCode(SceneName);
        }
    }

    public interface IScene : IDisposable
    {
        SceneId SceneId { get; set; }

        EntityManager GetEntityManager();
        org.ogre.SceneManager GetOgreSceneManager();

        void Setup();
        void LoadScene();
        void UnloadScene();
        void Update(float timeDelta, bool sceneActive);
    }
}
