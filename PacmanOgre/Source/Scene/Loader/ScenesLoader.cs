using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PacmanOgre.Scene.Loader
{
    public class ScenesLoader
    {
        public class SceneNode
        {
            [XmlAttribute]
            public string Name { get; set; }
            [XmlElement]
            public string File { get; set; }
        }

        [XmlRoot("scenes")]
        public class SceneList
        {
            [XmlElement("scene")]
            public List<SceneNode> scenes { get; set; }

            public static SceneList Load(string fileName)
            {
                if(!File.Exists(fileName))
                {
                    throw new ArgumentException("Invalid Filename, Scenes List not found");
                }

                XmlSerializer serializer = new XmlSerializer(typeof(SceneList));

                FileStream fs = File.OpenRead(fileName);

                SceneList sceneList = serializer.Deserialize(fs) as SceneList;

                return sceneList;
            }
        }

        public string FilePath { get; set; } = "Media/scenes/Scenes.xml";
        public SceneManager SceneManager { get; set; }

        public ScenesLoader(SceneManager sceneManager)
        {
            SceneManager = sceneManager;
        }

        public void LoadScenes()
        {
            SceneList sceneList = SceneList.Load(FilePath);

            sceneList.scenes.ForEach(sceneNode =>
            {
                try
                {
                    // TODO: Pass in Namespace so this can be created from the editor adding a new scene.
                    Type classType = Type.GetType($"PacmanOgre.Scene.{sceneNode.Name}");
                    SceneManager.AddScene(classType);
                } catch(Exception e)
                {
                    throw new Exception($"Error loading Scene {sceneNode.Name}", e);
                }
            });
        }
    }
}
