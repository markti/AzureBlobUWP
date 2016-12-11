using BlobExplorer.Events;
using BlobExplorer.Model;
using BlobExplorer.ViewModel;
using BlobExplorer.Views;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainViewModel viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            this.viewModel = new MainViewModel();
            this.DataContext = viewModel;

            Messenger.Default.Register<AccountCreatedEvent>(this, HandleAccountCreatedEvent);
            Messenger.Default.Register<AccountDeletedEvent>(this, HandleAccountDeletedEvent);
            Messenger.Default.Register<SelectionClearedEvent>(this, HandleSelectionClearedEvent);

            // Register a handler for BackRequested events and set the
            // visibility of the Back button
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            ContentFrame.Navigate(typeof(NoAccountsView));
        }

        private void HandleSelectionClearedEvent(SelectionClearedEvent obj)
        {
            if(this.HamburgerMenuControl != null)
            {
                try
                {
                    this.HamburgerMenuControl.SelectedOptionsItem = null;
                    this.HamburgerMenuControl.SelectedItem = null;
                }
                catch (Exception ex)
                {
                    // do something
                    Debug.WriteLine(ex);
                }
            }
        }

        private void HandleAccountDeletedEvent(AccountDeletedEvent obj)
        {
            this.ResetNavigation();
        }

        private void HandleAccountCreatedEvent(AccountCreatedEvent obj)
        {
            this.ResetNavigation();
        }

        private void ResetNavigation()
        {
            // after we have saved a new account we should navigate away from the page
            ContentFrame.Navigate(typeof(NoAccountsView));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            viewModel.OnNavigatedTo();
        }
        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as AzureStorageAccount;
            ContentFrame.Navigate(typeof(ContainerListView), item);
        }
        private async void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as MenuItemViewModel;
            if(item.PageType != null)
            {
                ContentFrame.Navigate(item.PageType);
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ContentFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                e.Handled = true;
                ContentFrame.GoBack();
            }
        }
    }
}