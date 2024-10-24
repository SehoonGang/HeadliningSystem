using CoPick.Logging;
using HeadliningSystem.Services;
using HeadliningSystem.ViewModels.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace HeadliningSystem.Views.Windows
{
    public partial class MainWindow : INavigationWindow
    {
        private static readonly LoggerService Logger = LoggerService.Logger;

        public MainWindowViewModel ViewModel { get; }
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        public MainWindow(
            MainWindowViewModel viewModel,
            IPageService pageService,
            INavigationService navigationService
        )
        {
            ViewModel = viewModel;
            DataContext = this;
            SystemThemeWatcher.Watch(this);

            InitializeComponent();

            Logger.LogPath = "./LOG";
            
            this.SourceInitialized += MainWindow_SourceInitialized;
            SetPageService(pageService);
            navigationService.SetNavigationControl(RootNavigation);

            AllocConsole();
        }

        private void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);            
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();



        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
