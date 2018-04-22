using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Mailer.Converters
{
    public class MainMenuGroupStyleSelector : StyleSelector
    {
        public Style EmptyHeaderGroupStyle { get; set; }

        public Style NormalGroupStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (!(item is CollectionViewGroup group))
                return null;

            return string.IsNullOrEmpty((string) group.Name) ? EmptyHeaderGroupStyle : NormalGroupStyle;
        }
    }
}