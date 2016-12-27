using BlobExplorer.Events;
using BlobExplorer.Model;
using BlobExplorer.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.HockeyApp;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransferListView : Page
    {
        TransferListViewModel viewModel;

        public TransferListView()
        {
            this.InitializeComponent();

            this.viewModel = new TransferListViewModel();
            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            HockeyClient.Current.TrackPageView("TransferList");
            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = "Transfers" });

            viewModel.OnNavigatedTo();
        }

        private void OnCancelOperationClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var cancelButton = sender as Button;
            var transferItem = cancelButton.DataContext as BlobTransfer;

            viewModel.CancelTransfer(transferItem);
        }
    }
}