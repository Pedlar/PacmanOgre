using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using org.ogre;
using PacmanOgre;

namespace SharpEngine.Editor
{
    class GameWPF : ApplicationContextBase
    {
        private static GameWPF inst;
        public static GameWPF Singleton {
            get
            {
                if(inst == null)
                {
                    inst = new GameWPF();
                }

                return inst;
            }
        }

        public Game Game { get; set; }

        public IntPtr ExternalWindowHandle { get; set; }

        public GameWPF()
        {
            Game = new Game(this);
        }

        public void SetupGame()
        {
            Game.Setup();
        }

        public override void setup()
        {
            base.setup();
        }

        public override NativeWindowPair createWindow(string name, uint w, uint h, NameValuePairList miscParams)
        {
            miscParams["externalWindowHandle"] = ExternalWindowHandle.ToString();
            miscParams["vsync"] = "False";
            miscParams["FSAA"] = "2";
            miscParams["Multithreaded"] = "true";

            NativeWindowPair nwp = base.createWindow(name, w, h, miscParams);
            nwp.render.setAutoUpdated(false);
            return nwp;
        }

        public override bool oneTimeConfig()
        {
            bool foundit = false;
            foreach (RenderSystem rs in getRoot().getAvailableRenderers())
            {
                if (rs == null) continue;

                getRoot().setRenderSystem(rs);
                String rname = getRoot().getRenderSystem().getName();
                if (rname == "Direct3D9 Rendering Subsystem")
                {
                    foundit = true;
                    break;
                }
            }
            return foundit;
        }

        public override bool frameStarted(FrameEvent evt)
        {
            Game.frameStarted(evt);
            return base.frameStarted(evt);
        }

        public override bool frameRenderingQueued(FrameEvent evt)
        {
            Game.frameRenderingQueued(evt);
            return base.frameStarted(evt);
        }

        public override bool frameEnded(FrameEvent evt)
        {
            Game.frameEnded(evt);
            return base.frameStarted(evt);
        }
    }
}
