using Entity.Component;
using SharpEngine;

namespace PacmanOgre.Components
{
    public class MeshComponent : IComponent
    {
        private readonly IContext context;
        private readonly IEntity entity;

        [Description("Mesh Name")]
        public string MeshName { get; set; }

        public MeshComponent(IContext context, IEntity entity)
        {
            this.context = context;
            this.entity = entity;
        }

        public void Setup()
        {

        }

        public void OnLoaded()
        {

        }
    }
}
