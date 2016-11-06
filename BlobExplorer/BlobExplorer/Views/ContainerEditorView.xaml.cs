using BlobExplorer.Navigation;
using BlobExplorer.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContainerEditorView : Page
    {
        ContainerEditorViewModel viewModel;

        public ContainerEditorView()
        {
            this.InitializeComponent();

            this.viewModel = new ContainerEditorViewModel();
            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            viewModel.OnNavigatedTo(e.Parameter as ContainerEditorNavigationContext);
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var result = await viewModel.Save();
            if (result)
            {
                this.Frame.GoBack();
            }
            else
            {
                // something broke
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}