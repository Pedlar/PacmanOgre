using SharpEngine.Editor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpEngine.Editor.View.EditorViewportView;

namespace SharpEngine.Editor.ViewModel
{
    public class EditorViewportViewModel : DockWindowViewModel
    {
        private EditorViewportView _view;
        public EditorViewportView View { get { return _view; }
            set
            {
                _view = value;
                _view.HWNDInitialised += ReadyToInitOgre;
            }
        }

        public EditorViewportViewModel() : base(false)
        {
            
        }

        private void ReadyToInitOgre(object sender, WindowEventArgs args)
        {
            GameWPF.Singleton.ExternalWindowHandle = args.WinPtr;

            if (GameWPF.Singleton.getRoot() == null)
            {
                GameWPF.Singleton.initApp();
            }
        }
    }
}
