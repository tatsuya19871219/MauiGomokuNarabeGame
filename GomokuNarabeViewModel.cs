using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame;

public partial class GomokuNarabeViewModel : ObservableObject
{
    static double s_fieldHeightOccupancy = 0.7;
    [ObservableProperty] double _coinSize;
    [ObservableProperty] double _fieldWidth;
    [ObservableProperty] double _fieldHeight;
    [ObservableProperty] ColumnDefinitionCollection _fieldColumns;

    public ObservableCollection<Lane> Lanes { get; set; } = new();

    int? _fieldLanes;
    int? _filedStacks;

    GomokuNarabe _gomokuNarabe;

    public GomokuNarabeViewModel()
    {
    }

    public GomokuNarabeViewModel SetFieldSize(int fieldLanes, int fieldStacks)
    {
        if (_fieldLanes.HasValue || _filedStacks.HasValue)
            throw new Exception("Field size is already set. Do not set twice.");

        _fieldLanes = fieldLanes;
        _filedStacks = fieldStacks;

        _gomokuNarabe = new(fieldLanes, fieldStacks);

        FieldColumns = new();

        foreach(var lane in _gomokuNarabe.Lanes)
        {
            Lanes.Add(lane);
            FieldColumns.Add(new ColumnDefinition());
        }

        return this;
    }

    [RelayCommand]
    void PageSizeChanged(Size pageSize)
    {
        if (_fieldLanes <= 0 || _filedStacks <= 0)
            throw new Exception("Field size is not set yet.");

        var lanes = _fieldLanes.Value;
        var stacks = _filedStacks.Value;

        var ratio = (double)lanes/stacks;

        FieldHeight  = s_fieldHeightOccupancy * pageSize.Height;
        FieldWidth = ratio * FieldHeight;

        CoinSize = FieldHeight/stacks;
    }

    [RelayCommand]
    void SummonCoin(int laneIndex)
    {
        
    }

}
