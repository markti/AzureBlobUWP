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
    public sealed partial class SettingsView : Page
    {
        SettingsViewModel viewModel;

        public SettingsView()
        {
            this.InitializeComponent();

            this.viewModel = new SettingsViewModel();
            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            viewModel.OnNavigatedTo();

            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = "Settings" });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
        }
    }
}