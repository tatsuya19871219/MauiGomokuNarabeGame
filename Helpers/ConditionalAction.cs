namespace MauiGomokuNarabeGame.Helpers;

internal class ConditionalAction
{
    public int MillisecondsDelay { get; init; } = 100;
    public int MillisecondsTimeout { get; init; } = 1000;

    readonly Action _action;
    readonly Func<bool> _condition;

    public ConditionalAction(Action action, params Func<bool>[] conditions)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));
        if (conditions is null) throw new ArgumentNullException(nameof(conditions));

        _action = action;
        _condition = () => conditions.All(c => c.Invoke());
    }

    public async void Invoke()
    {
        if (MillisecondsDelay <= 0) throw new ArgumentOutOfRangeException(nameof(MillisecondsDelay));
        if (MillisecondsTimeout <= 0) throw new ArgumentOutOfRangeException(nameof(MillisecondsTimeout));

        int elapsedDelay = 0;
        while (true)
        {
            if (elapsedDelay > MillisecondsTimeout) throw new TimeoutException("Conditional action faces timeout.");

            if (_condition.Invoke()) break;

            elapsedDelay += MillisecondsDelay;
            await Task.Delay(MillisecondsDelay);
        }

        _action.Invoke();
    }

}
