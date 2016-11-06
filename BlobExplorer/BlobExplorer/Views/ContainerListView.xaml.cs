using BlobExplorer.Model;
using BlobExplorer.Navigation;
using BlobExplorer.ViewModel;
using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace BlobExplorer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContainerListView : Page
    {
        ContainerListViewModel viewModel;

        public ContainerListView()
        {
            this.InitializeComponent();

            this.viewModel = new ContainerListViewModel();
            this.DataContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            viewModel.OnNavigatedTo(e);
        }

        private void OnRemoveStorageAccountClick(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveStorageAccount();
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.Refresh();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as AzureStorageContainer;

            var context = new BlobListNavigationContext();
            context.Account = viewModel.StorageAccount;
            context.BlobPrefix = "";
            context.Container = item;

            this.Frame.Navigate(typeof(BlobListView), context);
        }

        private void GridView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            var item = element.DataContext as AzureStorageContainer;

            var context = new BlobListNavigationContext();
            context.Account = viewModel.StorageAccount;
            context.BlobPrefix = "";
            context.Container = item;

            this.Frame.Navigate(typeof(BlobListView), context);
        }

        private void OnCreateContainerClick(object sender, RoutedEventArgs e)
        {
            var context = new ContainerEditorNavigationContext();
            context.Account = viewModel.StorageAccount;
            context.IsNew = true;

            this.Frame.Navigate(typeof(ContainerEditorView), context);
        }

        private async void OnDeleteContainersClick(object sender, RoutedEventArgs e)
        {
            var affirmateev = new UICommand("Yes") { Id = 1 };
            var nein = new UICommand("No") { Id = 0 };
            var dialog = new Windows.UI.Popups.MessageDialog("You are about to delete a container from your Azure Storage Account. All blobs stored within this container will be deleted as a result of this action. This action cannot be undone.", "Are you sure?");
            dialog.Commands.Add(affirmateev);
            dialog.Commands.Add(nein);
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            var result = await dialog.ShowAsync();

            if(result.Equals(affirmateev))
            {
                // DELETE!
                viewModel.DeleteSelectedContainers();
            }
            else
            {
                // ABORT!
            }
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedContainers.Clear();
            foreach(var item in ContainerListGridView.SelectedItems)
            {
                var typedItem = item as AzureStorageContainer;
                viewModel.SelectedContainers.Add(typedItem);
            }
        }

        private void OnEditContainerClick(object sender, RoutedEventArgs e)
        {
            var context = new ContainerEditorNavigationContext();
            context.Account = viewModel.StorageAccount;
            context.Container = viewModel.SelectedContainers.FirstOrDefault();
            context.IsNew = false;

            this.Frame.Navigate(typeof(ContainerEditorView), context);
        }
    }
}