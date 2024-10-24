using HeadliningSystem.ViewModels.Pages;
using Wpf.Ui.Controls;
using OpenTK;
using OpenTK.GLControl;
using HeadliningSystem.Services;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Controls;

namespace HeadliningSystem.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        private static readonly LoggerService Logger = LoggerService.Logger;

        public DashboardViewModel ViewModel { get; }

        public System.Windows.Controls.RichTextBox LogBox
        {
            get
            {
                return this.rtbLog;
            }
        }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            this.Loaded += Page_Loaded;
        }

        public void Page_Loaded(object sender, EventArgs e)
        {
            Logger.RtbLog = rtbLog;

            AddTextToRichTextBox();
        }

        private void AddTextToRichTextBox()
        {
            FlowDocument flowDoc = new FlowDocument();
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add("Hello, this is some text.");
            flowDoc.Blocks.Add(paragraph);
            rtbLog.Document = flowDoc;
        }
    }
}
