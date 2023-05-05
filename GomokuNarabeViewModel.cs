using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiGomokuNarabeGame;

public partial class GomokuNarabeViewModel : ObservableObject
{

    //[ObservableProperty] double _pageHeight; // one-way to source

    //public ICommand PageSizeChangedCommand { get; }
    //public ICommand SummonCoinCommand { get; }

    public GomokuNarabeViewModel()
    {
        //PageSizeChangedCommand = new RelayCommand(PageSizeChanged);
        //SummonCoinCommand = new RelayCommand(SummonCoin);
    }

    [RelayCommand]
    void PageSizeChanged(Size pageSize)
    {

    }

    [RelayCommand]
    void SummonCoin()
    {
        
    }


    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            //case nameof(PageHeight):
                
              //  break;

            default:
                base.OnPropertyChanged(e);
                break;
        }
    }
}
