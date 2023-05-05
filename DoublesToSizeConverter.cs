using System.Globalization;

namespace MauiGomokuNarabeGame;

public class DoublesToSizeConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double width = (double)values[0];
        double height = (double)values[1];

        return new Size(width, height);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        Size size = (Size)value;

        return new object[] { size.Width, size.Height };
    }
}
