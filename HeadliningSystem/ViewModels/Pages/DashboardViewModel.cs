using HeadliningSystem.Models;
using System.ComponentModel;
using System.Security.Cryptography;

namespace HeadliningSystem.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private int _counter = 0;
        [ObservableProperty]
        private int _pageWidth;

        private bool _isCameraConnected = false;
        private bool _isPlcConnected = false;
        private bool _isHyundaiRobotConnected = false;
        private bool _isFanucRobotConnected = false;
        private Config _config;

        public bool IsCameraConnected
        {
            get => _isCameraConnected;
            set
            {
                _isCameraConnected = value;
                OnPropertyChanged(nameof(IsCameraConnected));
            }
        }

        public bool IsPlcConnected
        {
            get => _isPlcConnected;
            set
            {
                _isPlcConnected = value;
                OnPropertyChanged(nameof(IsPlcConnected));
            }
        }

        public bool IsHyundaiRobotConnected
        {
            get => _isHyundaiRobotConnected;
            set
            {
                _isHyundaiRobotConnected = value;
                OnPropertyChanged(nameof(IsHyundaiRobotConnected));
            }
        }

        public bool IsFanucRobotConnected
        {
            get => _isFanucRobotConnected;
            set
            {
                _isFanucRobotConnected = value;
                OnPropertyChanged(nameof(IsFanucRobotConnected));
            }
        }

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter++;
        }

        [RelayCommand]
        private void OnConnectCamera()
        {
            IsCameraConnected = !IsCameraConnected;
        }

        public DashboardViewModel(Config config)
        {
            _config = config;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
