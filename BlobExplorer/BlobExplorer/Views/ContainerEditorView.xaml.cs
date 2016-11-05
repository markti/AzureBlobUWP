using BlobExplorer.Navigation;
using BlobExplorer.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

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