using System;

using SharpEngine;

namespace PacmanOgre.Components
{
    public class MeshComponent : IComponent
    {
        private readonly IContext context;
        private readonly IEntity entity;

        public string MeshName { get; private set; }

        public MeshComponent(IContext context, IEntity entity, string meshName)
        {
            this.context = context;
            this.entity = entity;
            MeshName = meshName;
        }

        public void Setup()
        {

        }

        public void OnLoaded()
        {

        }
    }
}
