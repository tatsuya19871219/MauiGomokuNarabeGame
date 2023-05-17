using Microsoft.Maui.Controls;

namespace MauiGomokuNarabeGame.Views;

internal class CoinImage : Image
{
    internal CoinImage(string filename) : base()
    {
        this.Source = filename;    
    }
}
