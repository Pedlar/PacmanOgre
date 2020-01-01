using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SharpEngine.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window1_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window1_OnClosing(object sender, CancelEventArgs e)
        {
            //GameWPF.Singleton.closeApp();
        }

        public void _ogre_OnInitialised(object sender, EventArgs e)
        {

        }
    }
}
