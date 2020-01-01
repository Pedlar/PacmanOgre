using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SharpEngine.Editor.Widget
{
    [TemplatePart(Name = "PART_OverlayTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_OgreImage", Type = typeof(OgreImage))]
    public class OgreControl : Control
    {
        private Image _renderTargetControl;
        private OgreImage _ogreImage;
        private TextBlock _overlayTextBlock;

        private bool _cursorHidden;
        private bool _initialized;

        public OgreControl()
        {
            _initialized = false;
            Loaded += Control_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _renderTargetControl = Template.FindName("PART_RenderTargetControl", this) as Image;
            _ogreImage = Template.FindName("PART_OgreImage", this) as OgreImage;
            _overlayTextBlock = Template.FindName("PART_OverlayTextBlock", this) as TextBlock;

            SizeChanged += (s, e) =>
            {
                _ogreImage.ViewportSize = e.NewSize;
            };

            _ogreImage.Initialised += (s, e) =>
            {
                _initialized = true;
                _ogreImage.InitOgreAsync();
            };
        }

        public void SetCursor(Cursor cursor)
        {
            Cursor = cursor;
        }

        public void ShowCursorEx(bool bShow)
        {
            if (_cursorHidden == bShow)
            {
                Cursor = !bShow ? Cursors.None : Cursors.Arrow;
                _cursorHidden = !bShow;
            }
        }

        private void Control_Loaded(object sender, RoutedEventArgs args)
        {
            if(_initialized)
            {   //Protect from Multi Loading
                return;
            }

            Application.Current.Exit += (s, e) =>
            {
                _renderTargetControl.Source = null;
                _ogreImage.Dispose();
            };

            PreviewDragEnter += new DragEventHandler(Control_PreviewDragEnter);
            PreviewDragLeave += new DragEventHandler(Control_PreviewDragLeave);
            PreviewDragOver += new DragEventHandler(Control_PreviewDragOver);
            PreviewDrop += new DragEventHandler(Control_PreviewDrop);
        }

        private void Control_PreviewDragEnter(object sender, DragEventArgs args)
        {
            /*if (!MogitorsRoot.Instance.OnDragEnter(args.Data))
            {
                args.Effects = DragDropEffects.None;
                args.Handled = true;
                return;
            }*/

            args.Effects = DragDropEffects.Copy;
        }

        private void Control_PreviewDragLeave(object sender, DragEventArgs args)
        {
            //MogitorsRoot.Instance.OnDragLeave(args.Data);
        }

        private void Control_PreviewDragOver(object sender, DragEventArgs args)
        {
            /*if (!MogitorsRoot.Instance.OnDragOver(args.Data, args.GetPosition(this)))
            {
                args.Effects = DragDropEffects.None;
                args.Handled = true;
                return;
            }*/

            args.Effects = DragDropEffects.Copy;
        }

        private void Control_PreviewDrop(object sender, DragEventArgs args)
        {
            //MogitorsRoot.Instance.OnDragDrop(args.Data, args.GetPosition(this));
        }
    }
}
