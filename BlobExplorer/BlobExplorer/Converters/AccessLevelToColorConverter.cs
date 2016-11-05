using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace BlobExplorer.Converters
{
    public class AccessLevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var accessLevel = (BlobContainerPublicAccessType)value;
            SolidColorBrush brush = null;

            switch(accessLevel)
            {
                case BlobContainerPublicAccessType.Container:
                    brush = new SolidColorBrush(Colors.Green);
                    break;
                case BlobContainerPublicAccessType.Blob:
                    brush = new SolidColorBrush(Colors.Goldenrod);
                    break;
                case BlobContainerPublicAccessType.Off:
                    brush = new SolidColorBrush(Colors.Red);
                    break;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}