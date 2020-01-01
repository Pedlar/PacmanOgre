using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Editor.ViewModel
{
    public class MainViewModel
    {
        public DockManagerViewModel DockManagerViewModel { get; private set; }
        public MenuViewModel MenuViewModel { get; set; }

        public MainViewModel(MainWindow view)
        {
            // Load the Windows
            
            var documents = new List<DockWindowViewModel>();
            documents.Add(new EditorViewportViewModel() { Title = "Viewport", CanClose = false });

            DockManagerViewModel = new DockManagerViewModel(documents);
            MenuViewModel = new MenuViewModel(documents);
        }
    }
}
