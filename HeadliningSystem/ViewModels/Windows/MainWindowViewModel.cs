using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace HeadliningSystem.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Headlining Auto Mounting System";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "General",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            },
            new NavigationViewItem()
            {
                Content = "Task",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Notepad24 },
                TargetPageType = typeof(Views.Pages.TaskSettingPage)
            },
            new NavigationViewItem()
            {
                Content = "PLC",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.PlcSettingPage)
            },
            new NavigationViewItem()
            {
                Content = "Robot",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.RobotSettingPage)
            },
            new NavigationViewItem()
            {
                Content = "Camera",
                Icon = new SymbolIcon { Symbol = SymbolRegular.ScanCamera24 },
                TargetPageType = typeof(Views.Pages.CameraSettingPage)
            },
            new NavigationViewItem()
            {
                Content = "Offset",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.OffsetSettingPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
