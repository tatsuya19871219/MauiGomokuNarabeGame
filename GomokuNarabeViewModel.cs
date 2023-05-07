using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Messages;
using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame;

public partial class GomokuNarabeViewModel : ObservableObject
{
    static double s_fieldHeightOccupancy = 0.8;
    [ObservableProperty] double _coinSize;
    [ObservableProperty] double _fieldWidth;
    [ObservableProperty] double _fieldHeight;
    [ObservableProperty] double _touchAreaHeight;
    [ObservableProperty] double _poolAreaWidth;

    [ObservableProperty] RowDefinitionCollection _pageRows;
    [ObservableProperty] ColumnDefinitionCollection _pageColumns;
    [ObservableProperty] ColumnDefinitionCollection _fieldColumns;

    public ObservableCollection<Lane> Lanes { get; set; } = new();
    [ObservableProperty] Coin _nextCoin;

    int? _fieldLanes;
    int? _fieldStacks;

    GomokuNarabe _gomokuNarabe;

    public GomokuNarabeViewModel()
    {
    }

    public GomokuNarabeViewModel SetFieldSize(int fieldLanes, int fieldStacks)
    {
        if (_fieldLanes.HasValue || _fieldStacks.HasValue)
            throw new Exception("Field size is already set. Do not set twice.");

        _fieldLanes = fieldLanes;
        _fieldStacks = fieldStacks;

        _gomokuNarabe = new(fieldLanes, fieldStacks);

        PageRows = new();
        PageColumns = new();
        FieldColumns = new();

        foreach(var lane in _gomokuNarabe.Lanes)
        {
            Lanes.Add(lane);
            FieldColumns.Add(new ColumnDefinition());
        }

        NextCoin = _gomokuNarabe.NextCoin;

        return this;
    }

    [RelayCommand]
    void PageSizeChanged(Size pageSize)
    {
        if (_fieldLanes <= 0 || _fieldStacks <= 0)
            throw new Exception("Field size is not set yet.");

        var lanes = _fieldLanes.Value;
        var stacks = _fieldStacks.Value;

        var ratio = (double)lanes/stacks;

        FieldHeight  = s_fieldHeightOccupancy * pageSize.Height;
        FieldWidth = ratio * FieldHeight;

        TouchAreaHeight = pageSize.Height - FieldHeight;
        PoolAreaWidth = (pageSize.Width - FieldWidth) / 2;

        PageRows.Clear();
        PageRows.Add(new RowDefinition(){ Height = TouchAreaHeight });
        PageRows.Add(new RowDefinition(){ Height = FieldHeight });

        PageColumns.Clear();
        PageColumns.Add(new ColumnDefinition(){ Width = PoolAreaWidth });
        PageColumns.Add(new ColumnDefinition(){ Width = FieldWidth });
        PageColumns.Add(new ColumnDefinition(){ Width = PoolAreaWidth });

        CoinSize = FieldHeight/stacks;
    }

    [RelayCommand]
    void SummonCoin(int laneIndex)
    {
        Coin coin = _gomokuNarabe.NextCoin;

        var success = _gomokuNarabe.TryPushAt(laneIndex);

        if (!success) return;

        // Send message to animate coin Image
        Image coinImage = StrongReferenceMessenger.Default.Send(new PopCoinMessage() { RequestCoin = coin });

        StrongReferenceMessenger.Default.Send(new InsertCoinMessage(coinImage) { TargetLane = laneIndex });

        // Update next coin
        NextCoin = _gomokuNarabe.NextCoin;
    }

}
