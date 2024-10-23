using HeadliningSystem.ViewModels.Pages;
using Wpf.Ui.Controls;
using OpenTK;
using OpenTK.GLControl;

namespace HeadliningSystem.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        public void OnPageLoaded(object sender, EventArgs e)
        {
            if (ViewModel != null)
            {
                //ViewModel.PageWidth
            }
        }
    }
}
