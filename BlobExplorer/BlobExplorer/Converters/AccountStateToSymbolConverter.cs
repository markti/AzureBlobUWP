using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BlobExplorer.Converters
{
    public class AccountStateToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            char symbol = 'c';
            bool connectedState = (bool)value;
            if(connectedState)
            {
                // connected
                symbol = '\xE8CE';
            }
            else
            {
                // disconnected
                symbol = '\xE8CD';
            }
            return symbol;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}