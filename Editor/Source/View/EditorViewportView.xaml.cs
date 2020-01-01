using SharpEngine.Editor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharpEngine.Editor.View
{
    /// <summary>
    /// Interaction logic for EditorViewportView.xaml
    /// </summary>
    public partial class EditorViewportView : UserControl
    {
        public class WindowEventArgs : EventArgs
        {
            public IntPtr WinPtr { get; set; }
        }

        public event EventHandler<WindowEventArgs> HWNDInitialised;

        public EditorViewportView()
        {
            InitializeComponent();
            EditorViewportView view = this;
            PART_OgreImage.WindowReadyEvent += (s, e) =>
            {
                HWNDInitialised(this, e);
            };
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {

        }
    }
}
