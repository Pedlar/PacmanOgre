using System;
using org.ogre;

namespace SharpEngine.Editor.Widget
{
    public partial class OgreImage
    {
        private double _resourceItemScalar;
        private double _currentProcess;

        protected virtual void CallResourceItemLoaded(ResourceLoadEventArgs e)
        {
            Dispatcher.Invoke((MethodInvoker)(() => OnResourceItemLoaded(e)));
        }

        protected virtual void OnResourceItemLoaded(ResourceLoadEventArgs e)
        {
            ResourceLoadItemProgress?.Invoke(this, e);
        }

        private void InitResourceLoad()
        {
            GroupEventListener rgl = new GroupEventListener();

            rgl.ResourceGroupLoadStarted += Singleton_ResourceGroupLoadStarted;
            rgl.ResourceGroupScriptingStarted += Singleton_ResourceGroupScriptingStarted;
            rgl.ScriptParseStarted += Singleton_ScriptParseStarted;
            rgl.ResourceLoadStarted += Singleton_ResourceLoadStarted;
            rgl.WorldGeometryStageStarted += Singleton_WorldGeometryStageStarted;

            ResourceGroupManager.getSingleton().addResourceGroupListener(rgl);

            _currentProcess = 0;
        }

        private void Singleton_WorldGeometryStageStarted(string description)
        {
            _currentProcess += _resourceItemScalar;
            CallResourceItemLoaded(new ResourceLoadEventArgs(description, _currentProcess));
        }

        private void Singleton_ResourceLoadStarted(ResourcePtr resource)
        {
            _currentProcess += _resourceItemScalar;
            CallResourceItemLoaded(new ResourceLoadEventArgs(resource.getName(), _currentProcess));
        }

        private void Singleton_ScriptParseStarted(string scriptName)
        {
            _currentProcess += _resourceItemScalar;
            CallResourceItemLoaded(new ResourceLoadEventArgs(scriptName, _currentProcess));
        }

        private void Singleton_ResourceGroupScriptingStarted(string groupName, uint scriptCount)
        {
            _resourceItemScalar = (scriptCount > 0)
                                      ? 0.4d / scriptCount
                                      : 0;
        }

        private void Singleton_ResourceGroupLoadStarted(string groupName, uint resourceCount)
        {
            _resourceItemScalar = (resourceCount > 0)
                                      ? 0.6d / resourceCount
                                      : 0;
        }

        public event EventHandler<ResourceLoadEventArgs> ResourceLoadItemProgress;

        class GroupEventListener : ResourceGroupListener
        {

            public Action<string, uint> ResourceGroupLoadStarted;
            public Action<string, uint> ResourceGroupScriptingStarted;
            public Action<ResourcePtr> ResourceLoadStarted;
            public Action<string> ScriptParseStarted;
            public Action<string> WorldGeometryStageStarted;

            public override void resourceGroupLoadStarted(string groupName, uint resourceCount) =>
                ResourceGroupLoadStarted?.Invoke(groupName, resourceCount);

            public override void resourceGroupScriptingStarted(string groupName, uint scriptCount) =>
                ResourceGroupScriptingStarted?.Invoke(groupName, scriptCount);

            public override void resourceLoadStarted(ResourcePtr resource) =>
                ResourceLoadStarted?.Invoke(resource);

            public override void scriptParseStarted(string scriptName, SWIGTYPE_p_bool skipThisScript) =>
                ScriptParseStarted?.Invoke(scriptName);

            public override void worldGeometryStageStarted(string description) =>
                WorldGeometryStageStarted?.Invoke(description);
        }
    }

    public class ResourceLoadEventArgs : EventArgs
    {
        public ResourceLoadEventArgs(string name, double progress)
        {
            this.Name = name;
            this.Progress = progress;
        }

        public string Name { get; private set; }
        public double Progress { get; private set; }
    }
}
