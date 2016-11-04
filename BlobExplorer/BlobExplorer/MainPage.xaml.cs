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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            viewModel.OnNavigatedTo();
        }
        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
        }
        private async void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as MenuItemViewModel;
            if(item.PageType != null)
            {
                ContentFrame.Navigate(item.PageType);
            }
        }
    }
}