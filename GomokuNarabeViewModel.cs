using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Messages;
using MauiGomokuNarabeGame.Models;
using System.Collections.ObjectModel;

namespace MauiGomokuNarabeGame;

public partial class GomokuNarabeViewModel : ObservableObject
{
    static double s_fieldHeightOccupancy = 0.8;
    [ObservableProperty] double _coinSize;
    [ObservableProperty] double _fieldWidth;
    [ObservableProperty] double _fieldHeight;

    [ObservableProperty] ColumnDefinitionCollection _fieldColumns;

    public ObservableCollection<Lane> Lanes { get; set; } = new();
    [ObservableProperty] Coin _nextCoin;
    [ObservableProperty] bool _inputEnabled;

    int? _fieldLanes;
    int? _fieldStacks;

    GomokuNarabe _gomokuNarabe;

    Dictionary<object, bool> _isInitialized = new();

    public GomokuNarabeViewModel()
    {
        InputEnabled = false;

        WeakReferenceMessenger.Default.Register<InitializingMessage>(this, (r, m) =>
        {
            object key = m.Value;

            if (_isInitialized.ContainsKey(key)) throw new Exception("Given key is already set.");

            _isInitialized[key] = false;
        });

        WeakReferenceMessenger.Default.Register<InitializedMessage>(this, (r, m) =>
        {
            object key = m.Value;

            if (!_isInitialized.ContainsKey(key)) throw new IndexOutOfRangeException(nameof(_isInitialized));

            _isInitialized[key] = true;

            if (_isInitialized.Values.All(x=>x)) InputEnabled = true;
        });
    }

    public GomokuNarabeViewModel SetFieldSize(int fieldLanes, int fieldStacks)
    {
        if (_fieldLanes.HasValue || _fieldStacks.HasValue)
            throw new Exception("Field size is already set. Do not set twice.");

        _fieldLanes = fieldLanes;
        _fieldStacks = fieldStacks;

        _gomokuNarabe = new(fieldLanes, fieldStacks);

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

        CoinSize = FieldHeight/stacks;
    }

    [RelayCommand]
    async void SummonCoin(int laneIndex)
    {
        InputEnabled = false;

        Coin coin = _gomokuNarabe.NextCoin;

        var success = _gomokuNarabe.TryPushAt(laneIndex);

        if (!success) return;

        // Send message to animate coin Image
        Image coinImage = await WeakReferenceMessenger.Default.Send(new PopCoinRequestMessage() { RequestCoin = coin });

        // TODO: awaitable
        var result = await StrongReferenceMessenger.Default.Send(new InsertCoinRequestMessage() { CoinImage = coinImage, TargetLane = laneIndex });

        // Update next coin
        NextCoin = _gomokuNarabe.NextCoin;

        InputEnabled = true;
    }

    [RelayCommand]
    async void ResetGame()
    {
        InputEnabled = false;

        _gomokuNarabe.Reset();

        // Async messages for await
        var t = StrongReferenceMessenger.Default.Send(new ClearFieldRequestMessage());
        var result = await StrongReferenceMessenger.Default.Send(new FillPoolRequestMessage()).GetResponsesAsync();

        await t;

        InputEnabled = true;
    }

}
