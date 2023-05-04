using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace MauiGomokuNarabeGame;

public partial class GomokuNarabeViewModel : ObservableObject
{
    public ICommand SummonCoinCommand { get; }

    public GomokuNarabeViewModel()
    {
        SummonCoinCommand = new RelayCommand(SummonCoin);
    }

    void SummonCoin()
    {
        
    }
}
