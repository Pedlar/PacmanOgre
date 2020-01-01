using org.ogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanOgre
{
    public class GameSDL : ApplicationContext
    {
        Game _game;
        public GameSDL()
        {
            _game = new Game(this);
        }

        public override void setup()
        {
            base.setup();
            _game.Setup();
        }

        public override bool frameStarted(FrameEvent evt)
        {
            _game.frameStarted(evt);
            return base.frameStarted(evt);
        }

        public override bool frameRenderingQueued(FrameEvent evt)
        {
            _game.frameRenderingQueued(evt);
            return base.frameStarted(evt);
        }

        public override bool frameEnded(FrameEvent evt)
        {
            _game.frameEnded(evt);
            return base.frameStarted(evt);
        }
    }
}
