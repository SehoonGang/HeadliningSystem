namespace HeadliningSystem.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _counter = 0;

        [ObservableProperty]
        private int _pageWidth;

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter++;
        }
    }
}
