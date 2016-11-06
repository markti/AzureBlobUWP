using BlobExplorer.Events;
using BlobExplorer.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewStorageAccountView : Page
    {
        NewStorageAccountViewModel viewModel;

        public NewStorageAccountView()
        {
            this.InitializeComponent();

            viewModel = new NewStorageAccountViewModel();
            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = "Add Storage Account" });
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            await viewModel.Save();
            this.Frame.GoBack();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}