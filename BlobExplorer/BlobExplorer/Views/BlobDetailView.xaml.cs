using BlobExplorer.Navigation;
using BlobExplorer.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlobDetailView : Page
    {
        BlobDetailViewModel viewModel;

        public BlobDetailView()
        {
            this.InitializeComponent();

            this.viewModel = new BlobDetailViewModel();

            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var context = e.Parameter as BlobDetailNavigationContext;

            viewModel.OnNavigatedTo(context);
        }
    }
}