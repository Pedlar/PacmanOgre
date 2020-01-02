using org.ogre;
using PacmanOgre.Components;
using SharpEngine;
using SharpEngine.Component;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PacmanOgre.Scene.Loader
{

    public class SceneLoader
    {
        public string FilePath { get; set; }
        private IContext _context;
        private EntityManager _entityManager;

        public SceneLoader(IContext context, EntityManager entityManager)
        {
            _context = context;
            _entityManager = entityManager;
        }

        public void LoadScene(string sceneName)
        {
            XmlDocument sceneXml = new XmlDocument();
            sceneXml.Load(FilePath);

            XmlNode scene = sceneXml.DocumentElement;

            foreach (XmlNode node in scene)
            {
                switch(node.Name)
                {
                    case "entities":
                        parseEntities(node);
                        break;
                }
            }
        }

        private void parseEntities(XmlNode entityNodes)
        {
            foreach(XmlNode entityNode in entityNodes)
            {
                _entityManager.CreateEntity(configure: entity =>
                {
                    foreach (XmlNode attrNode in entityNode)
                    {
                        switch (attrNode.Name)
                        {
                            case "components":
                                parseComponents(entity, attrNode);
                                break;
                        }
                    }
                });
            }
        }

        private delegate (Type, ComponentProperties) ComponentParser(IEntity entity, XmlNode componentNode);

        private void parseComponents(IEntity entity, XmlNode componentsNode)
        {
            foreach (XmlNode componentNode in componentsNode)
            {
                ComponentParser componentParser = null;
                switch (componentNode.Name)
                {
                    case "position":
                        componentParser = parsePosition;
                        break;
                    case "mesh":
                        componentParser = parseMesh;
                        break;
                    case "camera":
                        componentParser = parseCamera;
                        break;
                    case "renderable":
                        componentParser = parseRenderable;
                        break;
                    case "velocity":
                        componentParser = parseVelocity;
                        break;
                    case "scale":
                        componentParser = parseScale;
                        break;
                    case "playerinput":
                        componentParser = parsePlayerInput;
                        break;
                    case "animation":
                        componentParser = parseAnimation;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                Type componentType;
                ComponentProperties componentProperties;
                (componentType, componentProperties) = componentParser(entity, componentNode);

                entity.AddComponent(componentType, componentProperties, _context, entity);
            }
        }

        private (Type, ComponentProperties) parseAnimation(IEntity entity, XmlNode attrNode)
        {
            return (typeof(AnimationComponent), null);
        }

        private (Type, ComponentProperties) parsePlayerInput(IEntity entity, XmlNode attrNode)
        {
            return (typeof(PlayerInputComponent), null);
        }

        private (Type, ComponentProperties) parseScale(IEntity entity, XmlNode attrNode)
        {
            Vector3 scaleVector = new Vector3();
            foreach (XmlAttribute attribute in attrNode.Attributes)
            {
                float value = float.Parse(attribute.Value, CultureInfo.InvariantCulture.NumberFormat);
                switch (attribute.Name)
                {
                    case "x":
                        scaleVector.x = value;
                        break;
                    case "y":
                        scaleVector.y = value;
                        break;
                    case "z":
                        scaleVector.z = value;
                        break;
                }
            }

            return (typeof(ScaleComponent), new ComponentProperties()
            {
                ["Scale"] = scaleVector
            });
        }

        private (Type, ComponentProperties) parseVelocity(IEntity entity, XmlNode attrNode)
        {
            return (typeof(VelocityComponent), null);
        }

        private (Type, ComponentProperties) parseRenderable(IEntity entity, XmlNode attrNode)
        {
            return (typeof(RenderableComponent), null);
        }

        private (Type, ComponentProperties) parseCamera(IEntity entity, XmlNode attrNode)
        {
            XmlAttribute activeAttr = attrNode.Attributes[0];
            if (activeAttr.Name != "active")
            {
                throw new XmlException("Camera Component Can Only Have Active Param");
            }

            bool active = bool.Parse(activeAttr.Value);

            return (typeof(CameraComponent), new ComponentProperties()
            {
                ["ActiveCamera"] = active
            });
        }

        private (Type, ComponentProperties) parseMesh(IEntity entity, XmlNode attrNode)
        {
            XmlAttribute nameAttr = attrNode.Attributes[0];
            if(nameAttr.Name != "name")
            {
                throw new XmlException("Mesh Component Can Only Have Name Param");
            }

            return (typeof(MeshComponent), new ComponentProperties()
            {
                ["MeshName"] = nameAttr.Value
            });
        }

        private (Type, ComponentProperties) parsePosition(IEntity entity, XmlNode positionNode)
        {
            Vector3 positionVector = new Vector3();
            foreach (XmlAttribute attribute in positionNode.Attributes)
            {
                float value = float.Parse(attribute.Value, CultureInfo.InvariantCulture.NumberFormat);
                switch (attribute.Name)
                {
                    case "x":
                        positionVector.x = value;
                        break;
                    case "y":
                        positionVector.y = value;
                        break;
                    case "z":
                        positionVector.z = value;
                        break;
                }
            }

            return (typeof(PositionComponent), new ComponentProperties()
            {
                ["Position"] = positionVector
            });
        }
    }
}
