using System.Windows;
using System.Windows.Controls;
using Pcy.Win32Api;

namespace Pcy.Wpf.Growl
{
    internal sealed class NoticeGWindow : Window
    {
        internal Panel GrowlPanel { get; set; }

        internal NoticeGWindow()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;

            GrowlPanel = new StackPanel();

            Content = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                //IsInertiaEnabled = true,
                Content = GrowlPanel
            };
            //WindowAttach.SetIgnoreAltF4(this, true);
            //WindowAttach.SetShowInTaskManager(this, false);
            this.Width = this.MaxWidth = 340;
            this.Background = System.Windows.Media.Brushes.Transparent;
            this.ShowActivated = false;
            this.ShowInTaskbar = false;
            this.Topmost = true;
            this.BorderThickness = new Thickness(0d);

            if (Application.Current.MainWindow != null)
            {
                try
                {
                    Application.Current.MainWindow.Closed += MainWindow_Closed;
                }
                catch (System.Exception)
                {
                }              
            }
        }

        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            try
            {
                this.Close();
                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Closed -= MainWindow_Closed;
                }
            }
            catch (System.Exception)
            {
            }           
        }


        internal void Init()
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Height = desktopWorkingArea.Height;
            Left = desktopWorkingArea.Right - Width;
            Top = 0;
        }

    }
}


