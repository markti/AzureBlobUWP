using BlobExplorer.Events;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.HockeyApp;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoAccountsView : Page
    {
        public NoAccountsView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            HockeyClient.Current.TrackPageView("NoAccounts");
            Messenger.Default.Send<PageTitleChangedEvent>(new PageTitleChangedEvent() { Title = "Home" });
            Messenger.Default.Send<SelectionClearedEvent>(new SelectionClearedEvent());
        }
    }
}
